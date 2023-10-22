using System.Collections;
using UnityEngine;

public class FirePoint : MonoBehaviour
{
    private Vector3 mousePosition;
    public GameObject bulletPrefab;
    public int gunDamage;
    private int nShots = 1;
    public bool spread;
    
    // Update is called once per frame
    void Update()
    {
        mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePosition - transform.position) * Quaternion.Euler(0, 0, 90f);
    }

    public void setNShots(int n)
    {
        nShots = n;
    }

    public IEnumerator ShootBullet()
    {
        for (int i = 0; i < nShots; i++)
        {
            var bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            var controller = bullet.GetComponent<ProjectileController>();
            controller.direction = transform.right;
            controller.damage = gunDamage;
            if (spread)
            {
                transform.Rotate(0, 0, 10);
            } else {
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
    
}
