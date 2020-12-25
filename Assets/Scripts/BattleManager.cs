using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour {
    [Header("Object References")]
    public GameObject arena;
    public GameObject player;
    public GameObject playerSpawn;
    public float outerSummonOffset;
    private float summonBounds;
    public List<VehicleScriptableObject> vehicleList;
    [Header("Wave Settings")]
    public float waveDelay;
    public float waveDelayDecay;
    public float waveEnemyCount;
    public float waveEnemyGrow;
    private bool isNewWave;
    private int waveNumber;
    private bool isGameStarted = false;

    // Use this for initialization

    void Awake() {
        //Assign selected Vehicle Scriptable Object to character
        // if(SceneLoader.sceneLoader) {
        //     player.GetComponent<PlayerScript>().vehicleData = vehicleList[SceneLoader.sceneLoader.selectedCharacterIndex].grade[0];
        // }
    }
    void Start() {
        Screen.orientation = ScreenOrientation.Landscape;

        //Get Arena Boundary information
        summonBounds = arena.transform.localScale.x / 2;
        summonBounds -= outerSummonOffset;
        /*Mesh _mesh = arena.GetComponent<MeshFilter>().mesh;
        arenaSize = _mesh.bounds.size;
        //Debug.Log(arenaSize); //(10,0,10)*/

        //Spawn Character Entities
        player.transform.position = playerSpawn.transform.position;
        isNewWave = true;
        waveNumber = 1;
        StartCoroutine(WaitForSceneLoad());
        BattleEvents.battleEvents.onGameOver += GameOver;
    }

    void SpawnRandom(GameObject obj) {
        /*//Determine Spawn Boundary; xB == xBounds
        float xB = arenaSize.x - 1.4f;
        float zB = arenaSize.z - 1.4f;
        obj.transform.position = new Vector3(Random.Range(-xB, xB), 0, Random.Range(-zB, zB));*/
        float angle = Random.Range(0, 2f * Mathf.PI);
        float r = summonBounds * Mathf.Sqrt(Random.Range(0f, 1f));
        Vector3 random = new Vector3(r * Mathf.Cos(angle), 0, r * Mathf.Sin(angle));
        obj.transform.position = random;
    }

    public Vector3 SpawnRandomPosition() {
        float angle = Random.Range(0, 2f * Mathf.PI);
        float r = summonBounds * Mathf.Sqrt(Random.Range(0f, 1f));
        Vector3 random = new Vector3(r * Mathf.Cos(angle), 0, r * Mathf.Sin(angle));
        return random;
    }

    // Update is called once per frame
    void Update() {
        if(isGameStarted)
        {
            if (isNewWave) {
                StartCoroutine(SpawnEnemy(Mathf.RoundToInt(waveEnemyCount)));
            }
            if (!player.GetComponent<PlayerScript>().isAlive)
            {
                BattleEvents.battleEvents.TriggerGameOver();
            }
        }
        // if(isGameOver)
        // {
        //     if (Time.timeScale > 0.2) {
        //         Time.timeScale -= 0.1f;
        //         Time.fixedDeltaTime = 0.02f * Time.timeScale;
        //     } else {
        //         StartCoroutine(LoadMenu());
        //     }
        // }
    }

    IEnumerator SpawnEnemy(int n) {
        for (int i = 0; i < n; i++) {

            GameObject enemy;
            int rand = Random.Range(0, 5);
            if (rand == 4) {
                enemy = ObjectPooler.SharedInstance.GetPooledObject(ObjectPooler.Pooled.DasherEnemy);
            } else if  (rand == 3) {
                enemy = ObjectPooler.SharedInstance.GetPooledObject(ObjectPooler.Pooled.ShooterEnemy);
            } else {
                enemy = ObjectPooler.SharedInstance.GetPooledObject(ObjectPooler.Pooled.FollowerEnemy);
            }
            if (enemy != null) {
                SpawnRandom(enemy);
                enemy.GetComponent<EnemyScript>().Spawn();
            }
        }
        //Delay between waves
        isNewWave = false;
        yield return new WaitForSeconds(waveDelay);
        waveNumber++;
        waveDelay *= waveDelayDecay;
        waveEnemyCount += waveEnemyGrow;
        isNewWave = true;
    }

    IEnumerator WaitForSceneLoad()
    {
        while (!SceneLoader.sceneLoader.isLoaded) {
            yield return null;
        }
        if (SceneLoader.sceneLoader.isLoaded) {
            // Animate Text
            yield return new WaitForSeconds(2f);
            isGameStarted = true;
        }
    }

    private void GameOver()
    {
        isGameStarted = false;
        StartCoroutine(SlowMotionEffect());
    }

    IEnumerator SlowMotionEffect()
    {
        while (Time.timeScale > 0.2) {
            Time.timeScale -= 0.1f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        //SceneLoader.sceneLoader.LoadScene(SceneIndex.MAIN_MENU);
    }

    void OnDestroy()
    {
        BattleEvents.battleEvents.onGameOver -= GameOver;
    }
}
