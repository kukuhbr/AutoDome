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
        yield return new WaitForSeconds(seconds);
        Destroy(this.gameObject);
    }

}