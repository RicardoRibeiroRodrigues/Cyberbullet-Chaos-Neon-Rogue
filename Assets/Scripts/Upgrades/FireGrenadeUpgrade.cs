using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGrenadeUpgrade : MonoBehaviour, IUpgradable
{
    public GameObject grenadeFirePrefab;
    public GameObject grenadeFireExplosionPrefab;
    private int level = 0;
    private float grenadeFireInterval = 5f;
    private int numberOfGrenades = 1;
    private List<GameObject> fireGrenades = new();
    private int damage = 15;
    private Vector3 explosionSize = new(0.6f, 0.6f, 0.6f);
    
    void Start()
    {
        InvokeRepeating(nameof(ShootFireGrenade), 0.7f, grenadeFireInterval);
    }

    void ShootFireGrenadeExplosion()
    {   
        GameObject fireGrenade = fireGrenades[0];
        fireGrenades.RemoveAt(0);
        if (fireGrenade != null)
        {
            Vector3 transform_position = fireGrenade.transform.position;
            Quaternion rotation_position = new(0, 0, 0, 0);
            Vector3 transform_right = fireGrenade.transform.right;

            Destroy(fireGrenade);

            var explosion = Instantiate(grenadeFireExplosionPrefab, transform_position, rotation_position);
            explosion.GetComponent<FireDamage>().damage = damage;
            explosion.transform.localScale = explosionSize;
        }
        
    }
    
    void ShootFireGrenade()
    {
        for (int i = 0; i < numberOfGrenades; i++)
        {
            var angle = Quaternion.Euler(0, 0, Random.Range(0, 360));
            var direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            GameObject fireGrenade = Instantiate(grenadeFirePrefab, transform.position, angle);
            fireGrenade.GetComponent<ProjectileController>().direction = direction;
            fireGrenades.Add(fireGrenade);
            Invoke(nameof(ShootFireGrenadeExplosion), 1.0f);
        }
    }

    public void LevelUp()
    {
        level++;
        if (level == 1){ 
            explosionSize = new Vector3(0.75f, 0.75f, 0.75f); 
        } else if (level == 2){
            damage += 15;
        } else if (level == 3){
            numberOfGrenades = 3;
        } else if (level == 4){
            grenadeFireInterval = 3f;
            CancelInvoke(nameof(ShootFireGrenade));
            InvokeRepeating(nameof(ShootFireGrenade), grenadeFireInterval, grenadeFireInterval);
        } else if (level == 5){
            numberOfGrenades = 5;
            explosionSize = new Vector3(0.95f, 0.95f, 0.95f); 
        }
    }
}
