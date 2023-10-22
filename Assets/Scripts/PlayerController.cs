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
    public int health;
    public float moveSpeed;
    public float fireRate;
    public float luck;
    public int damage;
    // Level mechanic
    private int xp;
    public int xpToNextLevel;
    private int level = 1;
    // Upgrade mechanic
    public UpgradeData[] upgrades;
    // Sounds
    private AudioSource audioSource;
    public AudioClip shootSound;
    private int selectedUpgradeIndex;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        xp = 0;
    }

    void Start()
    {
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
        if (isDying)
            return;
        
        if (!isMoving)
        {
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
                StartCoroutine(Move(targetPos));
            }
        }
        animator.SetBool("isMoving", isMoving);
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        m_Rigidbody.MovePosition(Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime));
        yield return null;
        isMoving = false;
    }

    void OnMove(InputValue value)
    {
        Movement = value.Get<Vector2>();
    }

    public void TakeDamage(int damage)
    {
        if (isDying)
            return;
        
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
        GameManager.Instance.endGame(false, level * 100 - 100);
        Destroy(gameObject);
    }

    void Die()
    {
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
        // Possiveis upgrades: aqueles com level menor que 5.
        var possibleUpgrades = new List<UpgradeData>();
        foreach (var upgrade in upgrades)
        {
            if (upgrade.upgradeLevel < 5)
            {
                possibleUpgrades.Add(upgrade);
                Debug.Log("Possible upgrade: " + upgrade.UpgradeName);
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
                var randomIndex = Random.Range(0, possibleUpgrades.Count);
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
        }
    }

    public void setSelectedUpgrade(int index)
    {
        selectedUpgradeIndex = index;
    }
}
