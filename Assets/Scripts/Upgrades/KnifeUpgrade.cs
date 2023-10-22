using System.Collections;
using UnityEngine;

public class KnifeUpgrade : MonoBehaviour, IUpgradable
{
    public GameObject knifePrefab;
    private int numKnifes = 8;
    private float knifeInterval = 2f;
    private Quaternion angle;
    private Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        angle = Quaternion.Euler(0, 0, 0);
        direction = new Vector2(1, 0);
        InvokeRepeating(nameof(Attack), 0.0f, knifeInterval);
    }


    void Attack()
    {
        Debug.Log("Shoot Knifes");
        StartCoroutine(ShootKnifes());
    }

    IEnumerator ShootKnifes()
    {
        for (int i = 0; i < numKnifes; i++)
        {
            var knife = Instantiate(knifePrefab, transform.position, angle);
            knife.GetComponent<ProjectileController>().direction = direction;
            angle *= Quaternion.Euler(0, 0, 45);
            float anguloZ = angle.eulerAngles.z;
            float angleRadians = Mathf.Deg2Rad * anguloZ;
            float direcaoX = Mathf.Cos(angleRadians);
            float direcaoY = Mathf.Sin(angleRadians);
            direction = new Vector2(direcaoX, direcaoY);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void LevelUp()
    {
        // TODO: Implementar o LevelUp do upgrade
    }
}
