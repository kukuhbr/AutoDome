using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    public float maxHp;
    public float maxAmmo;
    [SerializeField]
    protected float currentHp;
    [SerializeField]
    protected float currentAmmo;
    public float moveSpeed = 10;
    public float bulletSpeed = .1f;
    public float damageBuffer;
    public bool isAlive = true;
    protected bool isCooldown = false;
    protected bool isReloading = false;
    protected bool isShooting = false;
    protected Rigidbody rb;
    protected BoxCollider boxcol;

    void Start() {

    }
    public virtual void Update() {
        //Auto reload bullet
        if (currentAmmo < maxAmmo && !isReloading && !isShooting) {
            StartCoroutine(ReloadAmmo());
        }

        //Check for Damage
        if (damageBuffer > 0) {
            TakeDamage();
        }
    }

    public void AddDamage(float damage) {
        damageBuffer += damage;
    }

    void TakeDamage() {
        currentHp -= damageBuffer;
        if (currentHp <= 0) {
            isAlive = false;
            currentHp = 0;
        }
        damageBuffer = 0;
    }

    public float GetHp(int i)
    {
        if (i == 0) {
            return maxHp;
        }
        return currentHp;
    }

    public float GetAmmo(int i)
    {
        if (i == 0) {
            return maxAmmo;
        }
        return currentAmmo;
    }

    IEnumerator ReloadAmmo() {
        float reloadRate = 0.06f;
        if (currentAmmo + reloadRate > maxAmmo) {
            currentAmmo = maxAmmo;
        } else {
            currentAmmo += reloadRate;
        }
        isReloading = true;
        yield return new WaitForSeconds(.05f);
        isReloading = false;
    }
}