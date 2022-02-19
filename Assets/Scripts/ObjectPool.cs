using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {
    public static ObjectPool SharedInstance;
    public Dictionary<string,List<GameObject>> pooledObjects;
    public List<PoolObject> objectsToPool;

    private void Awake() {
        SharedInstance = this;
    }

    private void Start() {
        pooledObjects = new Dictionary<string, List<GameObject>>();
        for (int i = 0; i < objectsToPool.Count; i++) {
            List<GameObject> gos = new List<GameObject>();
            for(int j = 0; j < objectsToPool[i].poolAmount; j++) {
                GameObject obj = (GameObject)Instantiate(objectsToPool[i].gameObj);
                obj.name = objectsToPool[i].name + i;
                obj.SetActive(false);
                gos.Add(obj);
            }
            pooledObjects.Add(objectsToPool[i].name, gos);
        }
    }

    public GameObject GetPooledObject(string name) {
        for (int i = 0; i < pooledObjects[name].Count; i++)
            if (!pooledObjects[name][i].activeInHierarchy)
                return pooledObjects[name][i];
        return null;
    }

}
