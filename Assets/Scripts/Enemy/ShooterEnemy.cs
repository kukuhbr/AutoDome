using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : EnemyScript
{
    new void Update() {
        if(target)
        {
            transform.LookAt(target);
        }
        base.Update();
    }
}
