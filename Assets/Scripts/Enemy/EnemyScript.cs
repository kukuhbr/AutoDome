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
    [SerializeField]
    private List<Loot> lootTable;

    void Start() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        boxcol = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        hpBar.minValue = 0;
        hpBar.maxValue = maxHp;
        isAlive = false;
        BattleEvents.battleEvents.onGameOver += GameOver;
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
        rb.velocity = Vector3.zero;
        this.gameObject.SetActive(false);
    }

    void BulletKill() {
        isAlive = false;
        rb.velocity = Vector3.zero;
        if(deathParticle) {
            deathParticle.Play();
        }
        if(lootTable.Count > 0) {
            ItemDropManager.instance.CalculateDrop(transform.position, lootTable);
        }
        StartCoroutine(DeactivateEnemy(1f));
    }

    public virtual void Spawn() {
        currentHp = maxHp;
        moveDirection = Vector3.zero;
        SetDissolve(-.1f);
        this.gameObject.SetActive(true);
        StartCoroutine(SpawnWithDelay(.4f));
    }

    void OnTriggerEnter(Collider col) {
        if(isAlive) {
            if (col.tag == "Bullet") {
                col.gameObject.SetActive(false);
                currentHp -= col.GetComponent<BulletScript>().damage;
                if (currentHp <= 0) {
                    //isAlive = false;
                    BulletKill();
                    BattleEvents.battleEvents.TriggerScoreChange(100);
                }
            } else if (col.tag == "Player") {
                col.GetComponent<PlayerScript>().AddDamage(damage);
                Kill();
            }
        }

    }

    void OnTriggerStay(Collider col) {
        if(isAlive) {
            if (col.tag == "Enemy") {
                moveDirection += (transform.position - col.transform.position).normalized * 2;
            } else if (col.tag == "Player") {
                col.GetComponent<PlayerScript>().AddDamage(10f);
                Kill();
            }
        }
    }

    void GameOver() {
        StartCoroutine(SetAliveFalse(2f));
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

    IEnumerator SetAliveFalse(float time) {
        yield return new WaitForSeconds(time);
        isAlive = false;
    }

    void SetDissolve(float amount) {
        foreach(Renderer r in modelRenderer) {
            if(r) {
                r.material.SetFloat("_Dissolve", amount);
            }
        }
    }

    void OnDisable()
    {

        BattleEvents.battleEvents.onGameOver -= GameOver;
    }
}
