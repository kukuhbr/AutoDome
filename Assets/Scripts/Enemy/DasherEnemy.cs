using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DasherEnemy : EnemyScript
{
    private int rotateSpeed;
    new void Update() {
        if(target)
        {
            moveDirection = (target.position - transform.position).normalized;
            if(target.GetComponent<PlayerScript>().moveDirection != Vector3.zero) {
                rotateSpeed = 15;
            } else {
                moveDirection *= 0.2f;
                rotateSpeed = 5;
            }
        }
        base.Update();
    }

    void FixedUpdate() {
        if (isAlive) {
            rb.velocity = moveDirection * moveSpeed;
            transform.Rotate(0, rotateSpeed, 0);
        }
    }

    public override void BulletKill() {
        base.BulletKill();
        rb.velocity = Vector3.zero;
    }
}
