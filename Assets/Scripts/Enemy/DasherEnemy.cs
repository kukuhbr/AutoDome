using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DasherEnemy : EnemyScript
{
    new void Update() {
        if(target)
        {
            if(target.GetComponent<PlayerScript>().moveDirection != Vector3.zero) {
                moveDirection = (target.position - transform.position).normalized;
            } else {
                moveDirection = Vector3.zero;
            }
        }
        base.Update();
    }

    void FixedUpdate() {
        if (isAlive) {
            rb.velocity = moveDirection * moveSpeed;
        }
    }
}
