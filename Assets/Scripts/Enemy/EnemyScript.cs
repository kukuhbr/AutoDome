using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : Character {

    protected Transform target;
    [SerializeField]
    private ParticleSystem deathParticle;
    [SerializeField]
    private Renderer modelRenderer;

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

    public virtual void BulletKill() {
        isAlive = false;
        if(deathParticle) {
            deathParticle.Play();
        }
        StartCoroutine(DeactivateEnemy(1f));
    }

    public virtual void Spawn() {
        Debug.Log("Base enemy spawn");
        currentHp = maxHp;
        if(modelRenderer) {
                modelRenderer.material.SetFloat("_Dissolve", 0f);
        }
        isAlive = true;
        this.gameObject.SetActive(true);
    }

    void OnTriggerEnter(Collider col) {
        if(isAlive) {
            if (col.tag == "Bullet") {
                col.gameObject.SetActive(false);
                currentHp -= 10;
                if (currentHp <= 0) {
                    //isAlive = false;
                    BulletKill();
                    BattleEvents.battleEvents.TriggerScoreChange(100);
                }
            } else if (col.tag == "Player") {
                col.GetComponent<PlayerScript>().AddDamage(10f);
                //rb.MovePosition(new Vector3(-4, 0, 4));
                Kill();
            }
        }

    }

    IEnumerator DeactivateEnemy(float time) {
        while (time - Time.deltaTime >= 0) {
            time -= Time.deltaTime;
            if(modelRenderer) {
                modelRenderer.material.SetFloat("_Dissolve", 1 - time);
            }
            yield return null;
        }
        //yield return new WaitForSeconds(1f);
        this.gameObject.SetActive(false);
    }
}
