using UnityEngine;
using System.Collections;

public class ItemHolder : MonoBehaviour {
    public ItemBase itemReference;

    void Start() {
        GameObject obj = Instantiate(itemReference.model, transform);
        StartCoroutine(DeleteAfter(5f));
    }

    void OnTriggerEnter(Collider col) {
        if (col.tag == "Player") {
            itemReference.SetHandler(col.gameObject);
            itemReference.Pickup();
            Destroy(this.gameObject);
        }
    }

    void FixedUpdate() {
        transform.Rotate(0, 5, 0);
    }

    IEnumerator DeleteAfter(float seconds) {
        float timeLeft = seconds;
        float shrinkTime = 2f;
        while (timeLeft >= 0) {
            timeLeft -= Time.deltaTime;
            if (timeLeft < shrinkTime) {
                float shrink = Mathf.Lerp(0f, 1f, timeLeft / shrinkTime);
                transform.localScale = new Vector3(shrink, shrink, shrink);
            }
            yield return null;
        }
        Destroy(this.gameObject);
    }

}