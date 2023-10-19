using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePoint : MonoBehaviour
{
    public Transform firePoint;
    private Vector3 mousePosition;
    
    // Update is called once per frame
    void Update()
    {
        mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        firePoint.rotation = Quaternion.LookRotation(Vector3.forward, mousePosition - transform.position) * Quaternion.Euler(0, 0, 90f);
    }
    
}
