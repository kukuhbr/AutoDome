using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObject/Vehicle/VehicleScriptableObject", order = 1)]
public class VehicleScriptableObject : ScriptableObject
{
    public int id;
    [SerializeField]
    public List<VehicleData> grade;
}

[System.Serializable]
public class VehicleData {
    [Multiline]
    public string vehicleName;
    public float maxHp;
    public float maxAmmo;
    public float damage;
    public float moveSpeed = 10;
    public float bulletSpeed = .1f;
    public float reloadRate = .06f;
    public float fireRate = .5f;
    public GameObject model;
}


