using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timeLeft = 99f;
    public bool isTimeOver = false;
    private TextMeshProUGUI text;
    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isTimeOver) {
            timeLeft -= Time.deltaTime;
            text.text = Mathf.RoundToInt(timeLeft).ToString();
            if(timeLeft <= 0) {
                isTimeOver = true;
            }
        }
    }
}
