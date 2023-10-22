using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScopeCursorController : MonoBehaviour
{
    public float ScaleAmplitude = 1.0f;
    public float AnimationFrequency = 0.2f;

    private Vector3 baseScale;

    void Start()
    {
        baseScale = transform.localScale;
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * 90 * Time.deltaTime * AnimationFrequency);
        transform.localScale = baseScale + Vector3.one * ScaleAmplitude * Mathf.Sin(Time.time * 2 * Mathf.PI * AnimationFrequency);
    }
}
