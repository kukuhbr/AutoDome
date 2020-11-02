using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEvents : MonoBehaviour
{
    public static BattleEvents battleEvents;

    private void Awake()
    {
        battleEvents = this;
    }

    public event Action onGameOver;
    public void TriggerGameOver()
    {
        if(onGameOver != null)
        {
            onGameOver();
        }
    }

}
