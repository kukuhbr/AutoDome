using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : Character {

    protected Transform target;
    [SerializeField]
    private ParticleSystem deathParticle;
    [SerializeField]
    private List<Renderer> modelRenderer;
    [SerializeField]
    private Slider hpBar;

    void Start() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        boxcol = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        hpBar.minValue = 0;
        hpBar.maxValue = maxHp;
        isAlive = false;
    }

    new void Update() {
        base.Update();
    }

    void LateUpdate() {
        hpBar.value = currentHp;
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
        SetDissolve(-.1f);
        this.gameObject.SetActive(true);
        StartCoroutine(SpawnWithDelay(.4f));
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
                Kill();
            }
        }

    }

    void OnTriggerStay(Collider col) {
        if(isAlive) {
            if (col.tag == "Enemy") {
                moveDirection += transform.position - col.transform.position;
            } else if (col.tag == "Player") {
                col.GetComponent<PlayerScript>().AddDamage(10f);
                Kill();
            }
        }
    }

    IEnumerator DeactivateEnemy(float time) {
        while (time - Time.deltaTime >= 0) {
            time -= Time.deltaTime;
            SetDissolve(1 - time);
            yield return null;
        }
        //yield return new WaitForSeconds(1f);
        this.gameObject.SetActive(false);
    }

    // Delay spawn for animation
    IEnumerator SpawnWithDelay(float time) {
        yield return new WaitForSeconds(time);
        isAlive = true;
    }

    void SetDissolve(float amount) {
        foreach(Renderer r in modelRenderer) {
            if(r) {
                r.material.SetFloat("_Dissolve", amount);
            }
        }
    }
}
