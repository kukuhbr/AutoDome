using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public enum Pooled {
        Bullet,
        BulletEnemy,
        FollowerEnemy,
        DasherEnemy,
        ShooterEnemy,
    }

    [System.Serializable]
    public class ObjectPoolItem {
        public int amountToPool;
        public GameObject objectToPool;
        public Pooled type;
        public bool shouldExpand = true;
    }
    public static ObjectPooler SharedInstance;

    public List<ObjectPoolItem> itemsToPool;
    public Dictionary<Pooled, List<GameObject>> pooledObjects;


    void Awake() {
        SharedInstance = this;
        pooledObjects = new Dictionary<Pooled, List<GameObject>>();
    }

    void Start() {
        // pooledObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool) {
            List<GameObject> pool = new List<GameObject>();
            for(int i=0; i < item.amountToPool; i++) {
                GameObject obj = (GameObject)Instantiate(item.objectToPool);
                obj.SetActive(false);
                //pooledObjects.Add(obj);
                pool.Add(obj);
            }
            pooledObjects.Add(item.type, pool);
        }
    }

    public GameObject GetPooledObject(Pooled type) {
        List<GameObject> pool = pooledObjects[type];
        for(int i = 0; i < pool.Count; i++) {
            if(!pool[i].activeInHierarchy) {
                return pool[i];
            }
        }
        foreach (ObjectPoolItem item in itemsToPool) {
            if (item.type == type) {
                if (item.shouldExpand) {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    pooledObjects[type].Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }
}
