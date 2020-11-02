using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUIScript : MonoBehaviour
{
    public PlayerScript player;
    public Slider hpBar;
    public Transform ammoUISection;
    public TextMeshProUGUI timer;
    public float timeLeft = 99f;
    public bool isTimeOver = false;
    private List<Slider> ammoList;
    public bool isGameOver;
    public GameObject gameText;
    private Animator gameTextAnimator;
    public TextMeshProUGUI gameTextBottom;

    void Start()
    {
        ammoList = new List<Slider>();
        int maxAmmo = Mathf.RoundToInt(player.GetAmmo(0));
        int ammoCount = 0;
        foreach (RectTransform child in ammoUISection)
        {
            Slider ammo = child.GetComponent<Slider>();
            if (ammoCount < maxAmmo) {
                ammo.value = 1;
                ammoList.Add(ammo);
                ammo.gameObject.SetActive(true);
                ammoCount++;
            } else {
                break;
            }
        }
        gameTextAnimator = gameText.GetComponent<Animator>();
        StartCoroutine(WaitForSceneLoad());
        BattleEvents.battleEvents.onGameOver += DisplayGameOverText;
    }

    void LateUpdate()
    {
        // If Scene Loaded
        if (SceneLoader.sceneLoader.isLoaded)
        {
            // Update Player Ammo
            float currentAmmo = player.GetAmmo(1);
            for(int i = 0; i < ammoList.Count; i++)
            {
                if (i == Mathf.FloorToInt(currentAmmo)) {
                    ammoList[i].value = currentAmmo % 1;
                } else if (i < currentAmmo) {
                    ammoList[i].value = 1;
                } else {
                    ammoList[i].value = 0;
                }
            }

            // Update Player HP
            hpBar.value = player.GetHp(1);

            // Update Timer
            if (!isTimeOver)
            {
                timeLeft -= Time.deltaTime;
                timer.text = Mathf.RoundToInt(timeLeft).ToString();
                if (timeLeft < 0) {
                    isTimeOver = true;
                }
            }

            // Game Over
            if (isGameOver)
            {
                // gameTextBottom.text = "OVER";
                // gameTextAnimator.SetBool("GameOver", true);
            }
        }
    }

    IEnumerator WaitForSceneLoad()
    {
        while (!SceneLoader.sceneLoader.isLoaded) {
            yield return null;
        }
        if (SceneLoader.sceneLoader.isLoaded) {
            // Animate Text
            gameText.SetActive(true);
            gameTextAnimator.enabled = true;
        }
    }

    private void DisplayGameOverText()
    {
        gameTextBottom.text = "OVER";
        gameTextAnimator.SetBool("GameOver", true);
    }
}
