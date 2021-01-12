using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : EnemyScript
{
    private bool barrageStart;
    new void Update() {
        if(target && isAlive)
        {
            transform.LookAt(target);
            if (barrageStart) {
                //Fire
                if(!isCooldown && currentAmmo >= 1f) {
                    StartCoroutine(Fire((target.position - transform.position).normalized));
                    currentAmmo -= 1f;
                }
                if(currentAmmo <= 0f || Mathf.Approximately(currentAmmo, 0f)) {
                    barrageStart = false;
                    isReloadAble = true;
                }
            } else {
                //Reload
                if(currentAmmo >= maxAmmo) {
                    barrageStart = true;
                    isReloadAble = false;
                }
            }
        }
        base.Update();
    }

    public override void Spawn() {
        base.Spawn();
        currentAmmo = maxAmmo;
        barrageStart = true;
    }

    IEnumerator Fire(Vector3 input) {
        //Logic
        GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject(ObjectPooler.Pooled.BulletEnemy);
        if (bullet) {
            bullet.transform.position = this.transform.position + new Vector3(0, 1, 0);
            bullet.transform.rotation = Quaternion.LookRotation(input + new Vector3(0, 90, 0));
            bullet.SetActive(true);
            bullet.GetComponent<BulletScript>().Shoot(input, bulletSpeed, damage);
        }
        //ApplyCooldown
        isCooldown = true;
        yield return new WaitForSeconds(0.5f);
        isCooldown = false;
    }
}
