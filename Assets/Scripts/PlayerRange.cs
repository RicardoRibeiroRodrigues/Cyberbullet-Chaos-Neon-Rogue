using System.Collections.Generic;
using UnityEngine;

public class PlayerRange : MonoBehaviour
{
    public static List<GameObject> enemiesInRange;
    
    private void Awake()
    {
        enemiesInRange = new List<GameObject>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("RangedEnemy") || other.CompareTag("Boss"))
        {
            enemiesInRange.Add(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("RangedEnemy") || other.CompareTag("Boss"))
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }
}
