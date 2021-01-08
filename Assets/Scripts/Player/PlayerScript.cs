using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : Character {
    public VehicleData vehicleData;
    public Joystick joystickMove;
    public Joystick joystickShoot;
    private Vector3 shootDirection;
    private Vector3 cameraForward;
    private Vector3 cameraRight;
    private GameObject playerModel;
    private GameObject playerModelBody;
    private bool isGameOver=false;

    void Awake()
    {
        // Assign values from selected Vehicle Scriptable Object
        vehicleData = SceneLoader.sceneLoader.GetVehicle();
        maxHp = vehicleData.maxHp;
        maxAmmo = vehicleData.maxAmmo;
        damage = vehicleData.damage;
        moveSpeed = vehicleData.moveSpeed;
        bulletSpeed = vehicleData.bulletSpeed;
        reloadRate = vehicleData.reloadRate;
        fireRate = vehicleData.fireRate;
        currentHp = vehicleData.maxHp;
        currentAmmo = vehicleData.maxAmmo;
    }
    void Start() {
        // Get Components
        boxcol = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        playerModel = Instantiate(vehicleData.model, transform);
        foreach (Transform child in playerModel.transform) {
            if (child.name == "Body") {
                playerModelBody = child.gameObject;
                break;
            }
        }
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
                playerModelBody.transform.rotation = Quaternion.LookRotation(shootDirection.normalized) * Quaternion.Euler(0,180,0);
                if (!isCooldown && currentAmmo >= 1) {
                    StartCoroutine(Fire(shootDirection.normalized));
                }
            } else {
                isShooting = false;
                playerModelBody.transform.localRotation = Quaternion.Euler(0,180,0);
            }

            // Check Reload and Damage
            isReloadAble = currentAmmo < maxAmmo;
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
            AddDamage(col.GetComponent<BulletScript>().damage);
        }
    }

    public override void DamageKill() {
        base.DamageKill();
        isAlive = false;
    }

    public void Heal(int n) {
        Debug.Log("Heal before " + currentHp + " " + n);
        if (currentHp + n < maxHp) {
            currentHp += n;
        } else {
            currentHp = maxHp;
        }
        Debug.Log("Heal after " + currentHp);
    }

    public void Reload(int n) {
        Debug.Log("Reload before " + currentAmmo + " " + n);
        if (currentAmmo + n < maxAmmo) {
            currentAmmo += n;
        } else {
            currentAmmo = maxAmmo;
        }
        Debug.Log("Reload after " + currentAmmo);
    }

    public void SpawnBomb(GameObject effector, int strength) {
        Vector3 spawnLocation = new Vector3(transform.position.x, 0, transform.position.z);
        GameObject obj = Instantiate(effector, spawnLocation, Quaternion.identity);
        obj.GetComponent<Effector>().Setup("Enemy", 12f);
        obj.GetComponent<BombScript>().SetDamage(strength);
    }

    IEnumerator Fire(Vector3 input) {
        //Logic
        GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject(ObjectPooler.Pooled.Bullet);
        if (bullet != null) {
            bullet.transform.position = this.transform.position + new Vector3(0, .5f, 0) + playerModelBody.transform.forward * -1f;
            bullet.transform.rotation = Quaternion.LookRotation(input + new Vector3(0, 90, 0));
            bullet.SetActive(true);
            bullet.GetComponent<BulletScript>().Shoot(input, bulletSpeed, damage);
            GetComponent<SoundsManager>().PlaySFX(SoundsManager.SoundsEnum.character_shoot, .4f);
        }
        //ApplyCooldown
        isCooldown = true;
        currentAmmo -= 1;
        yield return new WaitForSeconds(fireRate);
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
