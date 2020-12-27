using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item3DViewScript : MonoBehaviour
{
    GameObject model;
    void Start()
    {
        MainMenu.mainMenu.onItemFocusChange += ChangeModel;
    }

    void ChangeModel(int id) {
        DatabaseItem databaseItem = Database.database.databaseItem;
        GameObject newModel = databaseItem.GetItemById(id).model;
        Quaternion modelRotation = newModel.transform.rotation;
        if (model) {
            modelRotation = model.transform.rotation;
            Destroy(model);
        }
        model = Instantiate(newModel, this.transform);
        model.transform.rotation = modelRotation;
        model.transform.localScale = new Vector3(8, 8, 8);
        model.layer = LayerMask.NameToLayer("ItemPanel");
    }

    void LateUpdate() {
        if (model) {
            model.transform.Rotate(0, 20*Time.deltaTime, 0, Space.World);
        }
    }

    void OnDestroy()
    {
        MainMenu.mainMenu.onItemFocusChange -= ChangeModel;
    }
}
