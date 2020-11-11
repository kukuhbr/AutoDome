using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObject/VehicleScriptableObject", order = 1)]
public class VehicleScriptableObject : ScriptableObject
{
    public string vehicleName;
    public float maxHp;
    public float maxAmmo;
    public float moveSpeed = 10;
    public float bulletSpeed = .1f;
    public GameObject model;
}
