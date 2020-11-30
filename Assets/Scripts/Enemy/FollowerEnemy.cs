using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerEnemy : EnemyScript
{
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

    void OnTriggerStay(Collider col) {
        if(col.tag == "Enemy") {
            moveDirection += (transform.position - col.transform.position).normalized;
        }
    }
}
