using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoration : MonoBehaviour
{
    public Material original;
    public Material swap;
    private BoxCollider boxCollider;
    private MeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = original;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Bullet" || col.tag == "BulletEnemy") {
            StartCoroutine(MaterialSwap());
        }
    }

    IEnumerator MaterialSwap()
    {
        meshRenderer.material = swap;
        yield return new WaitForSeconds(.5f);
        meshRenderer.material = original;
    }
}
