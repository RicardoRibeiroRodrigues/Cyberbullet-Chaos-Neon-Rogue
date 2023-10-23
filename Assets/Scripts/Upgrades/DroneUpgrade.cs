using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class DroneUpgrade : MonoBehaviour, IUpgradable
{
    public GameObject firePoint;
    public GameObject MissilePrefab;
    public int explosionDamage;
    public int attackSpeed;
    public int n_fires;
    private int level = 0;
    public float missileAttackDelay = 3f;

    private AudioSource audioSource;


    public void Attack() {
        audioSource.Play();
        StartCoroutine(firePoint.GetComponent<FirePoint>().ShootBullet());
    }

    public void LevelUp() {
        level++;
        if (level == 1)
        {
            // Increase attack speed.
            attackSpeed *= 2;
            // Remove the other Ivokes.
            CancelInvoke(nameof(Attack));
            InvokeRepeating(nameof(Attack), 0.0f, (float) 1/attackSpeed);
        }
        else if (level == 2)
        {
            n_fires = 3;
            firePoint.GetComponent<FirePoint>().setNShots(n_fires);
        } else if (level == 3)
        {
            // Mini missil.
            InvokeRepeating(nameof(LaunchMissile), 0.0f, missileAttackDelay);
        } else if (level == 4)
        {
            attackSpeed *= 2;
            // Remove the other Ivokes.
            CancelInvoke(nameof(Attack));
            InvokeRepeating(nameof(Attack), 0.0f, (float) 1/attackSpeed);
        } else if (level == 5)
        {
            missileAttackDelay = 1f;
            CancelInvoke(nameof(LaunchMissile));
            InvokeRepeating(nameof(LaunchMissile), 0.0f, missileAttackDelay);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        firePoint.GetComponent<FirePoint>().setNShots(n_fires);
        InvokeRepeating(nameof(Attack), 0.0f, (float)1 / attackSpeed);
    }

    void LaunchMissile()
    {
        var Missile = Instantiate(MissilePrefab, firePoint.transform.position, firePoint.transform.rotation);
        Missile.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        var controller = Missile.GetComponent<MissileController>();
        // Damage, explosion scale and interval are set in the controller
        controller.damage = explosionDamage;
        controller.explosionScale = new Vector3(0.2f, 0.2f, 0.2f);
        controller.direction = firePoint.transform.right;
    }
}
