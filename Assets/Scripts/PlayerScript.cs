﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : Character {
    public Slider playerHpBar;
    public Joystick joystickMove;
    public Joystick joystickShoot;
    private Vector3 moveDirection;
    private Vector3 shootDirection;
    private Vector3 cameraForward;
    private Vector3 cameraRight;

    void Start() {
        boxcol = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        currentHp = maxHp;
        currentAmmo = maxAmmo;
        playerHpBar.maxValue = maxHp;
        playerHpBar.value = maxHp;
        cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        cameraRight = Camera.main.transform.right;
        Debug.Log("cam Forward" + cameraForward);
        Debug.Log("cam Right" + cameraRight);
    }

    new void Update() {
        if (isAlive) {
            //Check Movement and Rotation
            moveDirection = cameraForward * joystickMove.Vertical + cameraRight * joystickMove.Horizontal;
            //moveDirection = Vector3.forward * joystickMove.Vertical + Vector3.right * joystickMove.Horizontal;
            //moveDirection = Quaternion.AngleAxis(-35, Vector3.up) * (Vector3.forward * joystickMove.Vertical + Vector3.right * joystickMove.Horizontal);

            //Debug.Log(moveDirection);
            if (moveDirection.normalized != Vector3.zero) {
                transform.rotation = Quaternion.LookRotation(moveDirection.normalized);
            }

            //Check for Shoot
            shootDirection = cameraForward * joystickShoot.Vertical + cameraRight * joystickShoot.Horizontal;
            //shootDirection = Vector3.forward * joystickShoot.Vertical + Vector3.right * joystickShoot.Horizontal;
            if (Mathf.Abs(shootDirection.x) > 0.6 || Mathf.Abs(shootDirection.z) > 0.6) {
                isShooting = true;
                if (!isCooldown && currentAmmo >= 1) {
                    StartCoroutine(Fire(shootDirection.normalized));
                }
            } else {
                isShooting = false;
            }

            //Check Reload and Damage
            base.Update();
        } else {
            moveDirection = new Vector3(0,0,0);
        }
    }

    void FixedUpdate() {
        //this.transform.LookAt(direction * speed);
        LimitMovement(moveDirection);
        rb.velocity = moveDirection * moveSpeed;

    }

    void LateUpdate() {
        //playerHpBar.value = currentHp;
    }

    //Limit player movement
    //Shooting ray downward from around the player to detect ground
    //Set axis velocity to zero if ray return null
    void LimitMovement(Vector3 vInput) {
        Vector3[] checkPoints = new Vector3[4];
        bool[] hit = new bool[4];
        Vector3 res = new Vector3();

        checkPoints[0] = this.transform.position + new Vector3(boxcol.size.x, 0, 0);
        checkPoints[1] = this.transform.position + new Vector3(0, 0, boxcol.size.z);
        checkPoints[2] = this.transform.position + new Vector3(-boxcol.size.x, 0, 0);
        checkPoints[3] = this.transform.position + new Vector3(0, 0, -boxcol.size.z);

        for (int i = 0; i < 4; i++) {
            hit[i] = Physics.Raycast(checkPoints[i], Vector3.down, 5f);
        }

        if (vInput.x > 0) {
            res.x = vInput.x * System.Convert.ToInt32(hit[0]);
        } else {
            res.x = vInput.x * System.Convert.ToInt32(hit[2]);
        }

        if (vInput.z > 0) {
            res.z = vInput.z * System.Convert.ToInt32(hit[1]);
        } else {
            res.z = vInput.z * System.Convert.ToInt32(hit[3]);
        }
        moveDirection = res;
    }

    public void AddDamage(float damage) {
        damageBuffer += damage;
    }

    IEnumerator Fire(Vector3 vInput) {
        //Logic
        GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("Bullet");
        if (bullet != null) {
            bullet.transform.position = this.transform.position + new Vector3(0, 1, 0);
            bullet.transform.rotation = Quaternion.LookRotation(vInput + new Vector3(0, 90, 0));
            bullet.SetActive(true);
            bullet.GetComponent<BulletScript>().Shoot(vInput, bulletSpeed);
        }
        //ApplyCooldown
        isCooldown = true;
        currentAmmo -= 1;
        yield return new WaitForSeconds(0.5f);
        isCooldown = false;
    }
}
