using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningUpgrade : MonoBehaviour, IUpgradable
{
    // Start is called before the first frame update
    public GameObject LightningPrefab;
    public GameObject LightningExplosionPrefab;

    private GameObject Lightning;

    private float LightningInterval = 5f;
    private int level = 0;
    private int damage = 50;
    private Vector3 explosionScale = new Vector3(0.3f, 0.3f, 0.3f);


    void Start()
    {
        InvokeRepeating(nameof(ShootLightning), 0.0f, LightningInterval);
    }

    void ShootLightningExplosion()
    {
        // Se o raio ainda existe, destrói ele e cria a explosão instantaneamente na mesma posição
        if (Lightning != null)
        {
            Vector3 transform_position = Lightning.transform.position;
            Quaternion rotation_position = Lightning.transform.rotation;
            Vector3 transform_right = Lightning.transform.right;

            Destroy(Lightning);

            var explosion = Instantiate(LightningExplosionPrefab, transform_position, rotation_position);
            explosion.GetComponent<ProjectileController>().direction = transform_right;
            explosion.transform.localScale = explosionScale;
            explosion.GetComponent<ProjectileController>().damage = damage;

            LightningDamageApply();
        }
    }

    void ShootLightning()
    {
        Debug.Log("Shoot Lightning");
        // Shoot at a random position in the screen
        var position = new Vector2(Random.Range(-8, 8), Random.Range(-4, 4));
        Lightning = Instantiate(LightningPrefab, position, Quaternion.identity);
        Invoke(nameof(ShootLightningExplosion), 1.0f);
    }

    void LightningDamageApply()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionScale.x);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                collider.gameObject.GetComponent<EnemyController>().TakeDamage(1);
            }
            if (collider.gameObject.CompareTag("RangedEnemy"))
            {
                collider.gameObject.GetComponent<RangedEnemyController>().TakeDamage(1);
            }
        }
    }

    public void LevelUp()
    {
        level++;
        // diminui o intervalo de tiro se for nivel 1
        if (level == 1)
        {
            LightningInterval = 3f;
            CancelInvoke(nameof(ShootLightning));
            InvokeRepeating(nameof(ShootLightning), 0.0f, LightningInterval);
        }
        // aumenta o dano se for nivel 2
        else if (level == 2)
        {
            damage += 25;
        }
        // aumenta a area de dano se for nivel 3
        else if (level == 3)
        {
            explosionScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        // aumenta o dano se for nivel 4
        else if (level == 4)
        {
            damage += 25;
        }
        // aumenta a área e dano se for nivel 5
        else if (level == 5)
        {
            damage += 25;
            explosionScale = new Vector3(0.8f, 0.8f, 0.8f);
        }
    }
}
