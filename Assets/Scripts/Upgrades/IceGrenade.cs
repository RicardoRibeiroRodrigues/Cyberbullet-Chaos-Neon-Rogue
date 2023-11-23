using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceGrenadeUpgrade : MonoBehaviour, IUpgradable
{
    public GameObject grenadeIcePrefab;
    public GameObject grenadeIceExplosionPrefab;
    public static int level = 0;
    private float grenadeIceInterval = 3f;
    private int numberOfGrenades = 1;
    private List<GameObject> iceGrenades = new();
    private int damage = 0;
    private Vector3 explosionSize = new(0.35f, 0.35f, 0.35f);
    private float freezeDuration = 3.0f;
    
    void Start()
    {
        InvokeRepeating(nameof(ShootIceGrenade), 0.5f, grenadeIceInterval);
    }

    void ShootIceGrenadeExplosion()
    {   
        GameObject IceGrenade = iceGrenades[0];
        iceGrenades.RemoveAt(0);
        if (IceGrenade != null)
        {
            Vector3 transform_position = IceGrenade.transform.position;
            Quaternion rotation_position = new(0, 0, 0, 0);
            Vector3 transform_right = IceGrenade.transform.right;

            Destroy(IceGrenade);

            var explosion = Instantiate(grenadeIceExplosionPrefab, transform_position, rotation_position);
            explosion.GetComponent<ProjectileController>().direction = transform_right;
            explosion.transform.localScale = explosionSize;
            explosion.GetComponent<ProjectileController>().damage = damage;
            explosion.GetComponent<ProjectileController>().freezeDuration = freezeDuration;
        }
        
    }
    
    void ShootIceGrenade()
    {
        for (int i = 0; i < numberOfGrenades; i++)
        {
            var angle = Quaternion.Euler(0, 0, Random.Range(0, 360));
            var direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            GameObject IceGrenade = Instantiate(grenadeIcePrefab, transform.position, angle);
            IceGrenade.GetComponent<ProjectileController>().direction = direction;
            iceGrenades.Add(IceGrenade);
            Invoke(nameof(ShootIceGrenadeExplosion), 1.0f);
        }
    }

    public void LevelUp()
    {
        level++;
        if (level == 1){ 
            explosionSize = new Vector3(0.58f, 0.58f, 0.58f); 
        } else if (level == 2){
            freezeDuration = 5.0f;
        } else if (level == 3){
            damage = 65;
        } else if (level == 4){
            numberOfGrenades = 3;
        } else if (level == 5){
            grenadeIceInterval = 1.3f; 
            CancelInvoke(nameof(ShootIceGrenade));
            InvokeRepeating(nameof(ShootIceGrenade), grenadeIceInterval, grenadeIceInterval);
        }
        Debug.Log("Grenade Ice Level up! Level: " + level);
    }
}
