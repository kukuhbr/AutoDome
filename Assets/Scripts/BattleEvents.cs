using System;
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
    public event Action onGameStart;
    public void TriggerGameStart()
    {
        if(onGameStart != null)
        {
            onGameStart();
        }
    }
    public event Action<int> onScoreChange;
    public void TriggerScoreChange(int value)
    {
        if(onScoreChange != null)
        {
            onScoreChange(value);
        }
    }

}
