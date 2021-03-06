﻿using System.Collections;
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
    public bool isGameOver = false;
    public GameObject gameText;
    public GameObject rewardScreen;
    private Animator gameTextAnimator;
    public TextMeshProUGUI gameTextBottom;
    [Header("Scoreboard")]
    private int score;
    public TextMeshProUGUI scoreText;

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
        hpBar.maxValue = player.maxHp;
        hpBar.value = player.maxHp;
        gameTextAnimator = gameText.GetComponent<Animator>();
        StartCoroutine(WaitForSceneLoad());
        score = 0;
        BattleEvents.battleEvents.onGameOver += BattleUIGameOver;
        BattleEvents.battleEvents.onScoreChange += AdjustScore;
    }

    void LateUpdate()
    {
        // If Scene Loaded
        if (SceneLoader.sceneLoader.isLoaded)
        {
            UpdatePlayerAmmo();

            // Update Player HP
            hpBar.value = player.GetHp(1);

            UpdateTimer();
        }
    }

    void UpdatePlayerAmmo() {
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
    }

    void UpdateTimer() {
        if (!isTimeOver && !isGameOver)
        {
            timeLeft -= Time.deltaTime;
            timer.text = Mathf.RoundToInt(timeLeft).ToString();
            if (timeLeft < 0) {
                BattleEvents.battleEvents.TriggerGameOver();
                isTimeOver = true;
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

    private void BattleUIGameOver()
    {
        isGameOver = true;
        gameTextBottom.text = "OVER";
        gameTextAnimator.SetBool("GameOver", true);
        //after 3 seconds show reward screen
        StartCoroutine(ShowRewardScreen());
    }

    IEnumerator ShowRewardScreen()
    {
        yield return new WaitForSecondsRealtime(3f);
        SoundsManager.soundsManager.PlaySFX(SoundsManager.SoundsEnum.game_over_sfx);
        rewardScreen.SetActive(true);
        rewardScreen.GetComponent<RewardScreen>().AddBonusParts(score / 10);
        rewardScreen.GetComponent<RewardScreen>().Display();
        //PlayerManager.playerManager.playerData.inventory.Add(9, score / 10);
    }

    private void AdjustScore(int value)
    {
        if (score + value < 0) {
            score = 0;
        } else {
            score += value;
        }
		var f = new System.Globalization.NumberFormatInfo{NumberGroupSeparator=",", NumberDecimalDigits=0};
        scoreText.text = score.ToString("N", f);
    }

    void OnDestroy()
    {
        BattleEvents.battleEvents.onGameOver -= BattleUIGameOver;
        BattleEvents.battleEvents.onScoreChange -= AdjustScore;
    }
}
