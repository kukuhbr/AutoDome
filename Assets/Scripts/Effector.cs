using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Effector : MonoBehaviour
{
    private float size;
    public string target;
    void Start()
    {
        StartCoroutine(DestroyAfter(2f));
    }

    public void Setup(string _target, float _size)
    {
        size = _size;
        target = _target;
    }

    IEnumerator DestroyAfter(float time)
    {
        float timeLeft = time;
        while (timeLeft - Time.deltaTime >= 0) {
            timeLeft -= Time.deltaTime;
            float grow = Mathf.Lerp(size, 1f, timeLeft / time);
            transform.localScale = new Vector3(grow, grow, grow);
            yield return null;
        }
        Destroy(gameObject);
    }
}