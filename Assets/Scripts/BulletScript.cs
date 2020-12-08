using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Vector3 direction;
    private Rigidbody rb;
    public float speed;
    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    void Awake()
    {
        this.direction = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        //rb.MovePosition(this.transform.position + direction * speed * Time.deltaTime);
        rb.velocity = direction * speed;
    }

    public void Shoot(Vector3 vInput, float speed, float damage)
    {
        this.direction = vInput;
        this.speed = speed;
        this.damage = damage;
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Boundary")
        {
            this.gameObject.SetActive(false);
        }
    }
}
