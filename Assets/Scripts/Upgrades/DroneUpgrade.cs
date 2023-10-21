using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class DroneUpgrade : MonoBehaviour, IUpgradable
{
    public GameObject firePoint;
    public int attackSpeed;
    public int n_fires;
    private int level = 1;

    public void Attack() {
        StartCoroutine(firePoint.GetComponent<FirePoint>().ShootBullet());
    }

    public void LevelUp() {
        level++;
        if (level == 2)
        {
            n_fires = 3;
            firePoint.GetComponent<FirePoint>().setNShots(n_fires);
        } else if (level == 3)
        {
            // Mini missil.

        } else if (level == 4)
        {
            attackSpeed *= 2;
            // Remove the other Ivokes.
            CancelInvoke(nameof(Attack));
            InvokeRepeating(nameof(Attack), 0.0f, (float) 1/attackSpeed);
        } else if (level == 5)
        {
            // Mini missil - attack speed.
        }
    }

    void Start()
    {
        firePoint.GetComponent<FirePoint>().setNShots(n_fires);
        InvokeRepeating(nameof(Attack), 0.0f, (float)1 / attackSpeed);
    }
}
