using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //public GameObject PlayerPlaceholder;
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }

    // Update is called once per frame
    void Update()
    {
        // if (PlayerPlaceholder)
        // {
        //     PlayerPlaceholder.transform.Rotate(0, 20*Time.deltaTime, 0, Space.World);
        // }
    }

    public void Deploy()
    {
        SceneLoader.sceneLoader.LoadScene(SceneIndex.BATTLE_SOLO);
    }
}
