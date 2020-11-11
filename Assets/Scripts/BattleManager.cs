using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour {
    [Header("Object References")]
    public GameObject arena;
    public GameObject player;
    public GameObject playerSpawn;
    private Vector3 arenaSize;
    public List<VehicleScriptableObject> vehicleList;
    [Header("Wave Settings")]
    public float waveDelay;
    public float waveDelayDecay;
    public float waveEnemyCount;
    public float waveEnemyGrow;
    private bool isNewWave;
    private int waveNumber;
    private bool gameStart = false;
    private bool gameOver = false;

    // Use this for initialization

    void Awake() {
        //Assign selected Vehicle Scriptable Object to character
        if(SceneLoader.sceneLoader) {
            player.GetComponent<PlayerScript>().vso = vehicleList[SceneLoader.sceneLoader.selectedCharacterIndex];
        }
    }
    void Start() {
        Screen.orientation = ScreenOrientation.Landscape;

        //Get Arena Boundary information
        Mesh _mesh = arena.GetComponent<MeshFilter>().mesh;
        arenaSize = _mesh.bounds.size;
        //Debug.Log(arenaSize); //(10,0,10)

        //Spawn Character Entities
        player.transform.position = playerSpawn.transform.position;
        isNewWave = true;
        waveNumber = 1;
        StartCoroutine(WaitForSceneLoad());
    }

    void SpawnRandom(GameObject obj) {
        //Determine Spawn Boundary; xB == xBounds
        float xB = arenaSize.x - 1.4f;
        float zB = arenaSize.z - 1.4f;
        obj.transform.position = new Vector3(Random.Range(-xB, xB), 0, Random.Range(-zB, zB));
        //Debug.Log(obj.transform.position);
    }

    // Update is called once per frame
    void Update() {
        if(gameStart)
        {

            if (isNewWave) {
                StartCoroutine(SpawnEnemy(Mathf.RoundToInt(waveEnemyCount)));
            }
            if (!player.GetComponent<PlayerScript>().isAlive)
            {
                gameOver = true;
                gameStart = false;
            }
        }
        if(gameOver)
        {
            BattleEvents.battleEvents.TriggerGameOver();
            if (Time.timeScale > 0.2) {
                Time.timeScale -= 0.1f;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
            } else {
                StartCoroutine(GameOver());
            }
        }
    }

    IEnumerator SpawnEnemy(int n) {
        for (int i = 0; i < n; i++) {
            GameObject enemy = ObjectPooler.SharedInstance.GetPooledObject("Enemy");
            if (enemy != null) {
                SpawnRandom(enemy);
                enemy.SetActive(true);
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
            gameStart = true;
        }
    }

    IEnumerator GameOver()
    {
        gameOver = false;
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        SceneLoader.sceneLoader.LoadScene(SceneIndex.MAIN_MENU);
    }
}
