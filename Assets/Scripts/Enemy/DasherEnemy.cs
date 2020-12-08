using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DasherEnemy : EnemyScript
{
    private int rotateSpeed;
    private float maxSpeed;
    new void Update() {
        if(target)
        {
            moveDirection = (target.position - transform.position).normalized;
            Vector3 targetDirection = target.GetComponent<PlayerScript>().moveDirection;
            rotateSpeed = Mathf.RoundToInt(Mathf.Lerp(5f, 15f, targetDirection.magnitude));
            moveDirection *= Mathf.Lerp(0.2f, maxSpeed, targetDirection.magnitude);
        }
        base.Update();
    }

    void FixedUpdate() {
        if (isAlive) {
            rb.velocity = moveDirection * moveSpeed;
            transform.Rotate(0, rotateSpeed, 0);
        }
    }

    public override void Spawn() {
        base.Spawn();
        if (target) {
            maxSpeed = 1f + (target.GetComponent<PlayerScript>().moveSpeed / 100);
        } else {
            maxSpeed = 1f;
        }
    }
}
