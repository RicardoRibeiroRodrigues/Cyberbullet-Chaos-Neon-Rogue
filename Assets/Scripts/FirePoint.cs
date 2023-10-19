using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePoint : MonoBehaviour
{
    public Transform firePoint;

    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerController>().Movement != Vector2.zero){
            var targetRotation = transform.rotation.eulerAngles;
            
            if (player.GetComponent<PlayerController>().Movement.y > 0  && player.GetComponent<PlayerController>().Movement.y < 180 
                && player.GetComponent<PlayerController>().Movement.x != 0 && 
                (player.GetComponent<PlayerController>().Movement.x < 180 || player.GetComponent<PlayerController>().Movement.x > -180)
            )
            {
                targetRotation.z = 45;
            }
            else if (player.GetComponent<PlayerController>().Movement.y < 0  && player.GetComponent<PlayerController>().Movement.y > -180 
                && player.GetComponent<PlayerController>().Movement.x != 0 && 
                (player.GetComponent<PlayerController>().Movement.x < 180 || player.GetComponent<PlayerController>().Movement.x > -180)
            )
            {
                targetRotation.z = -45;
            }
            else if (player.GetComponent<PlayerController>().Movement.y == 0)
            {
                targetRotation.z = 0;
               
            }
            else if (player.GetComponent<PlayerController>().Movement.x == 0)
            {
                if (player.GetComponent<PlayerController>().Movement.y > 0){
                    targetRotation.z = 90;
                }
                else{
                    targetRotation.z = -90;
                }
            }
          
            firePoint.rotation = Quaternion.Euler(targetRotation);
        } 
    }
    
}
