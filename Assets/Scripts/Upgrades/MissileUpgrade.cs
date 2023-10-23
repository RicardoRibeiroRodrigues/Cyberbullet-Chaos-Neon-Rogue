using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileUpgrade : MonoBehaviour, IUpgradable
{
    public GameObject MissilePrefab;
    public GameObject MissileExplosionPrefab;
    private GameObject Missile;

    private float MissileInterval = 5f;

    private int level = 0;

    private int damage = 50;
    private Vector3 explosionScale = new Vector3(0.3f, 0.3f, 0.3f);

    void Start()
    {
        InvokeRepeating(nameof(ShootMissile), 0.0f, MissileInterval);
    }
    
    void ShootMissile()
    {
        Debug.Log("Shoot Missile");
        // Shoot at a direction
        var angle = Quaternion.Euler(0, 0, Random.Range(0, 360));
        float anguloZ = angle.eulerAngles.z;
        float angleRadians = Mathf.Deg2Rad * anguloZ;
        float direcaoX = Mathf.Cos(angleRadians);
        float direcaoY = Mathf.Sin(angleRadians);
        var direction = new Vector2(direcaoX, direcaoY);
        Missile = Instantiate(MissilePrefab, transform.position, transform.rotation);
        var controller = Missile.GetComponent<MissileController>();
        // Damage, explosion scale and interval are set in the controller
        controller.damage = damage;
        controller.explosionScale = explosionScale;
        controller.direction = direction;
    }

    public void LevelUp()
    {
        level++;
        // aumenta o raio de explosão se for nivel 1
        if (level == 1){ 
            explosionScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else if (level == 2)
        {
            damage += 25;
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
