using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningUpgrade : MonoBehaviour, IUpgradable
{
    // Start is called before the first frame update
    public GameObject LightningPrefab;
    public GameObject LightningExplosionPrefab;

    private GameObject Lightning;
    private float LightningInterval = 4f;
    private int level = 0;
    private int damage = 50;
    private float explosionScale = 1;


    void Start()
    {
        InvokeRepeating(nameof(ShootLightning), 0.0f, LightningInterval);
    }

    void ShootLightningExplosion()
    {
        // Se o raio ainda existe, destrói ele e cria a explosão instantaneamente na mesma posição
        if (Lightning != null)
        {
            Vector3 transform_position = Lightning.transform.position + new Vector3(0, 6 + (explosionScale * 4.8f - 4.8f), 0);
            Quaternion rotation_position = Lightning.transform.rotation;

            // Actite collider
            Lightning.GetComponent<CircleCollider2D>().enabled = true;

            var explosion = Instantiate(LightningExplosionPrefab, transform_position, rotation_position);
            explosion.transform.localScale = new Vector3(explosionScale, explosionScale, explosionScale);

            Destroy(Lightning, 0.6f);
        }
    }

    void ShootLightning()
    {
        Debug.Log("Shoot Lightning");
        // Spawn at a random circle position
        Vector2 spawnPos = transform.position;
        spawnPos += Random.insideUnitCircle.normalized * 3f;
        // Shoot at a random position in the screen
        Lightning = Instantiate(LightningPrefab, spawnPos, Quaternion.identity);
        Lightning.transform.localScale = new Vector3(10f, 10f, 10f) * explosionScale;
        Lightning.GetComponent<LightningController>().damage = damage;

        Invoke(nameof(ShootLightningExplosion), 1.5f);
    }

    public void LevelUp()
    {
        level++;
        // diminui o intervalo de tiro se for nivel 1
        if (level == 1)
        {
            LightningInterval = 2.5f;
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
            explosionScale = 2;
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
            explosionScale = 2.5f;
        }
    }
}
