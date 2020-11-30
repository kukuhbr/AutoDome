using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : Character {

    protected Transform target;
    protected Vector3 moveDirection;

    void Start() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        boxcol = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
    }

    new void Update() {
        base.Update();
    }

    // Deactivate and Reset Game Object
    void Kill() {
        isAlive = false;
        this.gameObject.SetActive(false);
    }

    public virtual void Spawn() {
        Debug.Log("Base enemy spawn");
        currentHp = maxHp;
        isAlive = true;
        this.gameObject.SetActive(true);
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
            col.GetComponent<PlayerScript>().AddDamage(10f);
            //rb.MovePosition(new Vector3(-4, 0, 4));
            Kill();
        }
    }
}
