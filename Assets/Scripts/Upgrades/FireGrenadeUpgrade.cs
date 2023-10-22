using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGrenadeUpgrade : MonoBehaviour, IUpgradable
{
    public GameObject grenadeFirePrefab;
    public GameObject grenadeFireExplosionPrefab;
    private GameObject fireGrenade;

    private float grenadeFireInterval = 5f;

    void Start()
    {
        InvokeRepeating(nameof(ShootFireGrenade), 0.0f, grenadeFireInterval);
    }

    void ShootFireGrenadeExplosion()
    {   
        if (fireGrenade != null) // Verifica se a granada ainda existe antes de destruí-la
        {
            Vector3 transform_position = fireGrenade.transform.position;
            Quaternion rotation_position = fireGrenade.transform.rotation;
            Vector3 transform_right = fireGrenade.transform.right;

            Destroy(fireGrenade);

            var explosion = Instantiate(grenadeFireExplosionPrefab, transform_position, rotation_position);
            explosion.GetComponent<ProjectileController>().direction = transform_right;
        }
        
    }
    
    void ShootFireGrenade()
    {
        Debug.Log("Shoot Fire Grenade");
        // Shoot at a random angle
        var angle = Quaternion.Euler(0, 0, Random.Range(0, 360));
        var direction = new Vector2(1, 0);
        fireGrenade = Instantiate(grenadeFirePrefab, transform.position, angle);
        fireGrenade.GetComponent<ProjectileController>().direction = direction;
        Invoke(nameof(ShootFireGrenadeExplosion), 1.0f);
    }

    public void LevelUp()
    {
        // TODO: Implementar o LevelUp do upgrade
    }
}
