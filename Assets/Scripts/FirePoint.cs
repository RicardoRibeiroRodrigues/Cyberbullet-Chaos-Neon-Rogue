using System.Collections;
using UnityEngine;

public class FirePoint : MonoBehaviour
{
    private Vector3 mousePosition;
    public GameObject[] bulletPrefab;
    public int gunDamage;
    public int nShots = 1;
    public bool spread;
    public int weaponTier;
    
    // Update is called once per frame
    void Update()
    {
        GameObject[] enemies = FindEnemies();
        GameObject closest = FindClosestEnemy(enemies);
        if (closest != null)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, closest.transform.position - transform.position) * Quaternion.Euler(0, 0, 90f);
        }
    }

    public void setNShots(int n)
    {
        nShots = n;
    }

    GameObject FindClosestEnemy(GameObject[] enemies)
    {
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject enemy in enemies)
        {
            Vector3 diff = enemy.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = enemy;
                distance = curDistance;
            }
        }
        return closest;
    }

    GameObject[] FindEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] bosses = GameObject.FindGameObjectsWithTag("Boss");
        GameObject[] rangedEnemies = GameObject.FindGameObjectsWithTag("RangedEnemy");
        GameObject[] allEnemies = new GameObject[enemies.Length + bosses.Length + rangedEnemies.Length];
        enemies.CopyTo(allEnemies, 0);
        bosses.CopyTo(allEnemies, enemies.Length);
        rangedEnemies.CopyTo(allEnemies, enemies.Length + bosses.Length);
        return allEnemies;
    }
    public IEnumerator ShootBullet()
    {
        if (spread)
        {
            var angle_between_shots = 45 / nShots;
            var plus_one = nShots % 2 == 0 ? 0 : 1;
            var angle = transform.rotation;
            var direction = transform.right;
            for (int i = 0; i < nShots / 2 + plus_one; i++)
            {
                var bullet = Instantiate(bulletPrefab[weaponTier], transform.position, angle);
                var controller = bullet.GetComponent<ProjectileController>();
                controller.damage = gunDamage;
                controller.direction = direction;
                angle *= Quaternion.Euler(0, 0, angle_between_shots);
                float angleRadians = Mathf.Deg2Rad * angle.eulerAngles.z;
                direction = new Vector2(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians));
            }
            angle = transform.rotation * Quaternion.Euler(0, 0, -angle_between_shots);
            for (int i = 0; i < nShots / 2; i++)
            {
                float angleRadians = Mathf.Deg2Rad * angle.eulerAngles.z;
                direction = new Vector2(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians));
                var bullet = Instantiate(bulletPrefab[weaponTier], transform.position, angle);
                var controller = bullet.GetComponent<ProjectileController>();
                controller.damage = gunDamage;
                controller.direction = direction;
                angle *= Quaternion.Euler(0, 0, -angle_between_shots);
            }
        } else {
            for (int i = 0; i < nShots; i++)
            {
                var bullet = Instantiate(bulletPrefab[weaponTier], transform.position, transform.rotation);
                var controller = bullet.GetComponent<ProjectileController>();
                controller.damage = gunDamage;
                controller.direction = transform.right;
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}
