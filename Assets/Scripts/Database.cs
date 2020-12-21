using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    public static Database database;
    public DatabaseItem databaseItem;
    public DatabaseVehicle databaseVehicle;
    public DatabaseVehicleUpgrade databaseVehicleUpgrade;
    void Awake()
    {
        database = this;
        databaseItem = Resources.Load<DatabaseItem>("DatabaseItem");
        databaseVehicle = Resources.Load<DatabaseVehicle>("DatabaseVehicle");
        databaseVehicleUpgrade = Resources.Load<DatabaseVehicleUpgrade>("DatabaseVehicleUpgrade");
    }
}
