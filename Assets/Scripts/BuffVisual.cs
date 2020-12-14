using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffVisual : MonoBehaviour
{
    private float minScale = .25f;
    private float maxScale = .4f;
    private float startTime;
    void Start()
    {
        startTime = Time.time;
    }
    void Update()
    {
        transform.Rotate(0, 5, 0);
        float currentScale = Mathf.Lerp(minScale, maxScale, Mathf.Abs(Mathf.Sin(Time.time - startTime)));
        transform.localScale = new Vector3(currentScale, 0, currentScale);
    }
}
