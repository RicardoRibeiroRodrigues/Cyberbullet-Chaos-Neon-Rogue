using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileUpgrade : MonoBehaviour, IUpgradable
{
    public GameObject MissilePrefab;
    public GameObject MissileExplosionPrefab;
    private GameObject Missile;

    private float MissileInterval = 5f;

    private int level = 1;

    private int damage = 50;
    private Vector3 explosionScale = new Vector3(0.3f, 0.3f, 0.3f);

    void Start()
    {
        InvokeRepeating(nameof(ShootMissile), 0.0f, MissileInterval);
    }

    void ShootMissileExplosion()
    {   
        // Se o missil ainda existe, destrói ele e cria a explosão instantaneamente na mesma posição
        if (Missile != null)
        {
            Vector3 transform_position = Missile.transform.position;
            Quaternion rotation_position = Missile.transform.rotation;
            Vector3 transform_right = Missile.transform.right;

            Destroy(Missile);

            var explosion = Instantiate(MissileExplosionPrefab, transform_position, rotation_position);
            explosion.GetComponent<ProjectileController>().direction = transform_right;
            explosion.transform.localScale = explosionScale;
            explosion.GetComponent<ProjectileController>().damage = damage;
        }
    }
    
    void ShootMissile()
    {
        Debug.Log("Shoot Missile");
        // Shoot at a random angle
        var angle = Quaternion.Euler(0, 0, Random.Range(0, 360));
        var direction = new Vector2(1, 0);
        Missile = Instantiate(MissilePrefab, transform.position, angle);
        Missile.GetComponent<ProjectileController>().direction = direction;
        Invoke(nameof(ShootMissileExplosion), 1.0f);
    }

    public void LevelUp()
    {
        level++;
        // aumenta o raio de explosão se for nivel 2
        if (level == 2)
        {
            explosionScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        // diminui o intervalo de tiro se for nivel 3
        else if (level == 3)
        {
            MissileInterval = 3f;
        }
        // aumenta o dano se for nivel 4
        else if (level == 4)
        {
            damage += 25;
        }
        // aumenta o dano e raio de explosão se for nivel 5
        else if (level == 5)
        {
            damage += 25;
            explosionScale = new Vector3(0.8f, 0.8f, 0.8f);
        }
    }
}
