using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    static ObjectPooler poolerInstance;
    public Pool[] pools;
    public Dictionary<string, List<GameObject>> poolDictionary;
    GameObject pooledObjects;
    GameObject obj;

    void Awake()
    {
        poolerInstance = this;
        pooledObjects = new GameObject("PooledObjects");
        CreatePools();
    }

    public static ObjectPooler GetInstance()
    {
        if (poolerInstance == null)
        {
            poolerInstance = GameObject.FindObjectOfType<ObjectPooler>();

            if (poolerInstance == null)
            {
                GameObject container = Instantiate(Resources.Load("ObjectPooler")) as GameObject;
                poolerInstance = container.GetComponent<ObjectPooler>();
            }
        }

        return poolerInstance;
    }

    void CreatePools()
    {
        poolDictionary = new Dictionary<string, List<GameObject>>();
        foreach (Pool pool in pools)
        {
            List<GameObject> list = new List<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                obj = Instantiate(pool.prefab);
                obj.transform.SetParent(pooledObjects.transform);
                list.Add(obj);
                obj.SetActive(false);
            }
            poolDictionary[pool.tag] = list;
        }
    }

    public GameObject SpawnObject(string tag, Vector3 position, Quaternion rotation)
    {
        foreach (GameObject item in poolDictionary[tag])
        {
            if (!item.activeSelf) // If there is an unused item in the pool
            {
                item.SetActive(true);
                item.transform.SetPositionAndRotation(position, rotation);
                IPooledObject pooledObject = item.GetComponent<IPooledObject>();
                if (pooledObject != null)
                {
                    pooledObject.OnObjectSpawn();
                }
                return item;
            }
        }

        obj = InstansiateNewObject(tag);
        return obj;
        
    }

    GameObject InstansiateNewObject(string tag)
    {
        for (int i = 0; i < pools.Length; i++)
        {
            if(tag == pools[i].tag)
            {
                obj = Instantiate(pools[i].prefab);
                obj.transform.SetParent(pooledObjects.transform);
                poolDictionary[tag].Add(obj);
                return obj;
            }
        }
        return null;
    }

    public void returnToThePool(Transform t)
    {
        t.SetParent(pooledObjects.transform, false);
        t.gameObject.SetActive(false);
    }
}