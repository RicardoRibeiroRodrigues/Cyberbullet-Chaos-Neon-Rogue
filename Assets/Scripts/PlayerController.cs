using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private bool isMoving;
    public Vector2 Movement;
    private Animator animator;
    private Rigidbody2D m_Rigidbody;
    public GameObject gun;
    public int selectedWeaponIndex = 0;
    public bool isDying = false;
    // Player stats
    private int maxHealth;
    public int health;
    public float moveSpeed;
    public float fireRate;
    public float luck;
    public int damage;
    // Level mechanic
    private int xp;
    public int xpToNextLevel;
    public int level = 1;
    // Upgrade mechanic
    public UpgradeData[] upgrades;
    // Sounds
    private AudioSource audioSource;
    public AudioClip shootSound;
    public AudioClip hurtSound;
    private int selectedUpgradeIndex;
    // HUD
    private HUDController hudController;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        xp = 0;
    }

    void Start()
    {
        hudController = GameObject.Find("Canvas").GetComponent<HUDController>();
        hudController.updateHealthValue(100);
        hudController.updateExperienceProgress(xp);
        maxHealth = health;
        audioSource = GetComponent<AudioSource>();
        var firepoint = gun.GetComponent<FirePoint>();
        firepoint.gunDamage = damage; 
        firepoint.weaponTier = selectedWeaponIndex;
        InvokeRepeating(nameof(ShootBullet), 0.0f, fireRate);
    }

    void ShootBullet()
    {
        if (isDying)
            return;
        
        audioSource.PlayOneShot(shootSound);
        StartCoroutine(gun.GetComponent<FirePoint>().ShootBullet());
    }

    void FixedUpdate()
    {
        var healthPercent = (float) health / maxHealth * 100;
        var xpPercent = (float) xp / xpToNextLevel;
        if (level == 17)
        {
            xpPercent = 1;
        }
        hudController.updateHealthValue((int) healthPercent);
        hudController.updateExperienceProgress(xpPercent);
    
        if (isDying)
            return;
        
        if (Movement != Vector2.zero)
        {
            // Rotate the player to face the direction of movement.
            if (Movement.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            } else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            var targetPos = transform.position;
            targetPos.x += Movement.x;
            targetPos.y += Movement.y;
            var direction = targetPos - transform.position;
            direction.Normalize();
            isMoving = true;
            m_Rigidbody.velocity = direction * moveSpeed;
        } else {
            m_Rigidbody.velocity = Vector3.zero;
            isMoving = false;
        }
        animator.SetBool("isMoving", isMoving);
    }

    void OnMove(InputValue value)
    {
        Movement = value.Get<Vector2>();
    }

    public void TakeDamage(int damage)
    {
        if (isDying)
            return;
        
        audioSource.PlayOneShot(hurtSound);
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        // Trigger hurt animation
        animator.SetTrigger("isHurt");
    }

    // Usado no evento da animacao de morrer.
    void FinishedDyingAnimation()
    {
        var coins = level * 25 - 25 > 0 ? level * 20 - 25 : 0;
        GameManager.Instance.AddCoins(coins);
        GameManager.Instance.endGame(false, coins);
        Destroy(gameObject);
    }

    void Die()
    {
        GetComponent<AudioSource>().Play();
        isDying = true;
        // Trigger death animation
        animator.SetTrigger("isDead");
        // Disable the player
        GetComponent<Collider2D>().enabled = false;
        Invoke(nameof(FinishedDyingAnimation), 3f);
    }

    IEnumerator LevelUp()
    {
        selectedUpgradeIndex = -1;
        xp = 0;
        xpToNextLevel = (int) (xpToNextLevel * 1.5f);
        level++;
        Debug.Log("Level up! Level: " + level);
        // Level maximo - do not upgrade.
        if (level >= 17)
        {
            // Stop the coroutine.
            yield break;
        }

        // Possiveis upgrades: aqueles com level menor que 5.
        var possibleUpgrades = new List<UpgradeData>();
        if (playerHasFullUpgradesSlots())
        {
            foreach (var upgrade in upgrades)
            {
                if (upgrade.upgradeLevel < 5 && upgrade.upgradeLevel > 0)
                {
                    possibleUpgrades.Add(upgrade);
                }
            }
        } else {
            foreach (var upgrade in upgrades)
            {
                if (upgrade.upgradeLevel < 5)
                {
                    possibleUpgrades.Add(upgrade);
                    Debug.Log("Possible upgrade: " + upgrade.UpgradeName);
                }
            }
        }
        // Now choose 3 random upgrades from the possible upgrades.
        var chosenUpgrades = new List<UpgradeData>();
        if (possibleUpgrades.Count <= 3)
        {
            chosenUpgrades = possibleUpgrades;
        } else {
            for (int i = 0; i < 3; i++)
            {
                // Most be unique.
                var randomIndex = Random.Range(0, possibleUpgrades.Count);
                while (chosenUpgrades.Contains(possibleUpgrades[randomIndex]))
                {
                    randomIndex = Random.Range(0, possibleUpgrades.Count);
                }
                Debug.Log("Random index: " + randomIndex);
                chosenUpgrades.Add(possibleUpgrades[randomIndex]);
            }
        }
        // Select one of the chosen upgrades.
        StartCoroutine(GameManager.Instance.SelectItem(chosenUpgrades, this));
        yield return new WaitUntil(() => selectedUpgradeIndex != -1);
        var selectedUpgrade = upgrades[selectedUpgradeIndex];
        selectedUpgrade.upgradeLevel++;
        // Aplica o upgrade.
        Debug.Log("Selected upgrade: " + selectedUpgrade.UpgradeName + "With index: " + selectedUpgradeIndex);

        if (selectedUpgrade.upgradeLevel == 1)
        {
            hudController.addItem(selectedUpgrade.icon);
            var upgrade = Instantiate(selectedUpgrade.UpgradePrefab, transform.position, transform.rotation);
            // Set upgrade parent to player.
            upgrade.transform.parent = transform;
            // Maintain same relative position.
            upgrade.transform.localPosition = selectedUpgrade.UpgradePrefab.transform.position;
            selectedUpgrade.activeUpgrade = upgrade;
        } else {
            if (selectedUpgrade.activeUpgrade.TryGetComponent(out IUpgradable upgradableObject))
            {
                upgradableObject.LevelUp();
            } else {
                Debug.Log("Upgrade not found!");
            }
        }
        yield return null;
    }

    bool playerHasFullUpgradesSlots()
    {
        var count = 0;
        foreach (var upgrade in upgrades)
        {
            if (upgrade.upgradeLevel > 0)
            {
                count++;
            }
        }
        return count == 3;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("XpOrb"))
        {
            xp += (int) luck * other.GetComponent<XpOrbController>().GetXp();
            if (xp >= xpToNextLevel)
            {
                StartCoroutine(LevelUp());
            }
            Destroy(other.gameObject);
        } else if (other.CompareTag("WeaponUpgrade"))
        {
            // Random upgrade from 0 to 2.
            var randomUpgrade = Random.Range(0, 2);
            if (randomUpgrade == 0)
            {
                SetFireRate(fireRate - 0.1f > 0.1f ? fireRate - 0.1f : 0.1f);
            } else if (randomUpgrade == 1)
            {
                setNShots(gun.GetComponent<FirePoint>().nShots + 1);
            } else if (randomUpgrade == 2)
            {
                // Damage up
                GameManager.Instance.SpawnPowerUpText("Dano upgrade!");
                gun.GetComponent<FirePoint>().gunDamage = (int) (gun.GetComponent<FirePoint>().gunDamage * 1.2);
            }
            Destroy(other.gameObject);
        } else if (other.CompareTag("ExtraLife"))
        {
            var new_health = health + 0.4f * maxHealth;
            health = new_health > maxHealth ? maxHealth : (int) new_health;
            Destroy(other.gameObject);
        }
    }

    public void setSelectedUpgrade(int index)
    {
        selectedUpgradeIndex = index;
    }
    
    // Stats upgrades
    public void SetFireRate(float newFireRate)
    {
        GameManager.Instance.SpawnPowerUpText("Fire rate upgrade!");
        fireRate = newFireRate;
        CancelInvoke(nameof(ShootBullet));
        InvokeRepeating(nameof(ShootBullet), 0.0f, fireRate);
    }

    public void setNShots(int n)
    {
        GameManager.Instance.SpawnPowerUpText("Numero de tiros upgrade!");
        gun.GetComponent<FirePoint>().nShots = n;
    }

    public void setSpread(bool spread)
    {
        gun.GetComponent<FirePoint>().spread = spread;
    }

}
