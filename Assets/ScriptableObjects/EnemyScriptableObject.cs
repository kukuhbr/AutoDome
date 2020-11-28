using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObject/EnemyScriptableObject", order = 2)]
public class EnemyScriptableObject : ScriptableObject
{
    public string enemyName;
    public float maxHp;
    public float moveSpeed = 5;
    public int enemyType;
    public GameObject model;
}
