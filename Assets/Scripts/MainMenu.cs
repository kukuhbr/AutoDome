using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI currencyText;
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        currencyText.text = PlayerManager.playerManager.GetCurrency("bolt").ToString();
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

    public void Garage()
    {

    }

    public void Inventory()
    {

    }
}
