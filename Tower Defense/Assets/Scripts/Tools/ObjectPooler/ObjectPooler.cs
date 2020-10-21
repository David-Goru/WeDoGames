using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    static ObjectPooler poolerInstance;
    public Pool[] Pools;
    public Dictionary<string, List<GameObject>> PoolDictionary;
    GameObject pooledObjects;
    GameObject obj;

    void Awake()
    {
        poolerInstance = this;
        pooledObjects = new GameObject("PooledObjects");
        createPools();
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

    void createPools()
    {
        PoolDictionary = new Dictionary<string, List<GameObject>>();
        foreach (Pool pool in Pools)
        {
            List<GameObject> list = new List<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                obj = Instantiate(pool.prefab);
                obj.transform.SetParent(pooledObjects.transform);
                list.Add(obj);
                obj.SetActive(false);
            }
            PoolDictionary[pool.tag] = list;
        }
    }

    public GameObject SpawnObject(string tag, Vector3 position, Quaternion rotation)
    {
        foreach (GameObject item in PoolDictionary[tag])
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

        obj = instansiateNewObject(tag);
        return obj;
        
    }

    GameObject instansiateNewObject(string tag)
    {
        for (int i = 0; i < Pools.Length; i++)
        {
            if(tag == Pools[i].tag)
            {
                obj = Instantiate(Pools[i].prefab);
                obj.transform.SetParent(pooledObjects.transform);
                PoolDictionary[tag].Add(obj);
                return obj;
            }
        }
        return null;
    }

    public void ReturnToThePool(Transform t)
    {
        t.SetParent(pooledObjects.transform, false);
        t.gameObject.SetActive(false);
    }
}