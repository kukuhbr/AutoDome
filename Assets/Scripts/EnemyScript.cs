using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : Character {
    private Transform target;
    private Vector3 moveDirection;

    void Start() {
        currentHp = maxHp;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        boxcol = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
    }

    new void Update() {
        if(target)
        {
            moveDirection = (target.position - transform.position).normalized;
        }
        base.Update();
    }

    void FixedUpdate() {
        if (isAlive) {
            rb.velocity = moveDirection * moveSpeed;
            transform.Rotate(0, 10, 0);
        }
    }

    // Deactivate and Reset Game Object
    void Kill() {
        this.gameObject.SetActive(false);
        currentHp = maxHp;
    }

    void OnTriggerEnter(Collider col) {
        if (col.tag == "Bullet") {
            col.gameObject.SetActive(false);
            currentHp -= 10;
            if (currentHp <= 0) {
                //isAlive = false;
                Kill();
                BattleEvents.battleEvents.TriggerScoreChange(100);
            }
        } else if (col.tag == "Player") {
            col.GetComponent<PlayerScript>().damageBuffer += 10f;
            //rb.MovePosition(new Vector3(-4, 0, 4));
            Kill();
        }
    }
}
