using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : EnemyScript
{
    [SerializeField]
    private int maxBarrage;
    private int barrage;
    new void Update() {
        if(target)
        {
            transform.LookAt(target);
            StartCoroutine(Barrage());
        }
        base.Update();
    }

    public override void Spawn() {
        base.Spawn();
        Debug.Log("This is shooter enemy spawn");
        barrage = maxBarrage;
    }

    IEnumerator Fire(Vector3 input) {
        //Logic
        GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject(ObjectPooler.Pooled.BulletEnemy);
        if (bullet) {
            bullet.transform.position = this.transform.position + new Vector3(0, 1, 0);
            bullet.transform.rotation = Quaternion.LookRotation(input + new Vector3(0, 90, 0));
            bullet.SetActive(true);
            bullet.GetComponent<BulletScript>().Shoot(input, bulletSpeed);
        }
        //ApplyCooldown
        isCooldown = true;
        yield return new WaitForSeconds(0.5f);
        isCooldown = false;
    }

    IEnumerator Barrage() {
        if (barrage > 0 && isAlive) {
            if (!isCooldown) {
                StartCoroutine(Fire((target.position - transform.position).normalized));
                barrage -= 1;
            }
        } else {
            if(!isReloading) {
                isReloading = true;
                yield return new WaitForSeconds(3f);
                isReloading = false;
                barrage = maxBarrage;
            }
        }
    }
}
