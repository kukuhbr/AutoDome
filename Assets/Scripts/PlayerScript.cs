using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : Character {
    public VehicleScriptableObject vso;
    public Slider playerHpBar;
    public Joystick joystickMove;
    public Joystick joystickShoot;
    private Vector3 shootDirection;
    private Vector3 cameraForward;
    private Vector3 cameraRight;
    private GameObject playerModel;
    private bool isGameOver=false;

    void OnEnable()
    {

    }
    void Start() {
        // Get Components
        boxcol = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();

        // Assign values from selected Vehicle Scriptable Object
        maxHp = vso.maxHp;
        maxAmmo = vso.maxAmmo;
        moveSpeed = vso.moveSpeed;
        bulletSpeed = vso.bulletSpeed;
        currentHp = vso.maxHp;
        currentAmmo = vso.maxAmmo;
        playerHpBar.maxValue = vso.maxHp;
        playerHpBar.value = vso.maxHp;
        playerModel = Instantiate(vso.model, transform);
        playerModel.transform.localScale = new Vector3(.5f, .5f, .5f);
        playerModel.transform.position = new Vector3(0, .3f, 0);

        cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        cameraRight = Camera.main.transform.right;
        //Debug.Log("cam Forward" + cameraForward);
        //Debug.Log("cam Right" + cameraRight);
        BattleEvents.battleEvents.onGameOver += GameOver;
    }

    new void Update() {
        if (isAlive && !isGameOver) {
            // Check Movement and Rotation
            moveDirection = cameraForward * joystickMove.Vertical + cameraRight * joystickMove.Horizontal;
            //moveDirection = Vector3.forward * joystickMove.Vertical + Vector3.right * joystickMove.Horizontal;
            //moveDirection = Quaternion.AngleAxis(-35, Vector3.up) * (Vector3.forward * joystickMove.Vertical + Vector3.right * joystickMove.Horizontal);

            //Debug.Log(moveDirection);
            if (moveDirection.normalized != Vector3.zero) {
                transform.rotation = Quaternion.LookRotation(moveDirection.normalized);
            }

            // Check for Shoot
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

            // Check Reload and Damage
            base.Update();
        } else {
            moveDirection = new Vector3(0,0,0);
            if(!isGameOver)
            {
                BattleEvents.battleEvents.TriggerGameOver();
            }
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
    private Vector3[] checkPoints = new Vector3[4];
    private int[] hit = new int[4];
    void LimitMovement(Vector3 vInput) {
        Vector3 res = new Vector3();
        checkPoints[0] = this.transform.position + new Vector3(boxcol.size.x, .2f, 0);
        checkPoints[1] = this.transform.position + new Vector3(0, .2f, boxcol.size.z);
        checkPoints[2] = this.transform.position + new Vector3(-boxcol.size.x, .2f, 0);
        checkPoints[3] = this.transform.position + new Vector3(0, .2f, -boxcol.size.z);

        // Raycast to determine still in arena zone
        for (int i = 0; i < 4; i++) {
            if(Physics.Raycast(checkPoints[i], Vector3.down, 1f)) {
                hit[i] = 1;
            } else {
                hit[i] = 0;
            }
        }

        if (vInput.x > 0) {
            res.x = vInput.x * hit[0];
        } else {
            res.x = vInput.x * hit[2];
        }

        if (vInput.z > 0) {
            res.z = vInput.z * hit[1];
        } else {
            res.z = vInput.z * hit[3];
        }
        moveDirection = res;
    }

    void OnTriggerEnter(Collider col) {
        if (col.tag == "BulletEnemy") {
            col.gameObject.SetActive(false);
            AddDamage(10f);
        }
    }

    IEnumerator Fire(Vector3 input) {
        //Logic
        GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject(ObjectPooler.Pooled.Bullet);
        if (bullet != null) {
            bullet.transform.position = this.transform.position + new Vector3(0, 1, 0);
            bullet.transform.rotation = Quaternion.LookRotation(input + new Vector3(0, 90, 0));
            bullet.SetActive(true);
            bullet.GetComponent<BulletScript>().Shoot(input, bulletSpeed);
        }
        //ApplyCooldown
        isCooldown = true;
        currentAmmo -= 1;
        yield return new WaitForSeconds(0.5f);
        isCooldown = false;
    }

    void GameOver()
    {
        isGameOver = true;
    }

    void OnDestroy()
    {
        BattleEvents.battleEvents.onGameOver -= GameOver;
    }
}
