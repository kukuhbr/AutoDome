using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombScript : MonoBehaviour
{
    Effector effector;
    int damage;
    void Start()
    {
        effector = GetComponent<Effector>();
    }

    public void SetDamage(int _damage) {
        damage = _damage;
    }

    void OnTriggerEnter(Collider col) {
        //if (!col.gameObject.activeSelf) return;
        if(col.tag == effector.target) {
            EnemyScript enemy = col.GetComponent<EnemyScript>();
            if(enemy) {
                enemy.AddDamage(damage);
            }
        }
    }
}