using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationInstant : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAfter(.8f));
    }

    IEnumerator DestroyAfter(float time) {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}