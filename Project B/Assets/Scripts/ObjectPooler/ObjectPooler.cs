using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    static ObjectPooler poolerInstance;
    public Pool[] Pools;
    public Dictionary<string, List<GameObject>> PoolDictionary;
    GameObject pooledObjects;

    void Awake()
    {
        if (poolerInstance != null && poolerInstance != this) Destroy(this.gameObject);
        else poolerInstance = this;
        pooledObjects = new GameObject("PooledObjects");
        createPools();
    }

    public static ObjectPooler GetInstance()
    {
        if (poolerInstance == null)
        {
            poolerInstance = FindObjectOfType<ObjectPooler>();

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
            if (pool.prefab == null)
            {
                Debug.Log("No prefab found for " + pool.name);
                continue;
            }

            List<GameObject> objectsList = new List<GameObject>();
            GameObject obj;
            for (int i = 0; i < pool.size; i++)
            {
                obj = instansiateItemAndAddToList(pool.prefab, objectsList);
                obj.SetActive(false);
            }
            PoolDictionary[pool.tag] = objectsList;
        }
    }

    GameObject instansiateItemAndAddToList(GameObject item, List<GameObject> objectsList)
    {
        GameObject obj = Instantiate(item);
        obj.transform.SetParent(pooledObjects.transform);
        objectsList.Add(obj);
        return obj;
    }

    public GameObject SpawnObject(string tag, Vector3 position)
    {
        GameObject obj = getObjectFromPool(tag);
        obj.transform.position = position;
        obj.SetActive(true);
        checkIPooledObjectInterface(obj);
        return obj;

    }

    public GameObject SpawnObject(string tag, Vector3 position, Quaternion rotation)
    {
        GameObject obj = getObjectFromPool(tag);
        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        checkIPooledObjectInterface(obj);
        return obj;

    }

    GameObject getObjectFromPool(string tag)
    {
        GameObject obj = null;
        bool isObjReceived = tryGetUnusedObjectFromPool(tag, ref obj);
        if (!isObjReceived) obj = instantiateNewObject(tag);
        return obj;
    }

    bool tryGetUnusedObjectFromPool(string tag, ref GameObject obj)
    {
        bool poolExists = PoolDictionary.ContainsKey(tag);
        if (!poolExists) Debug.LogError(tag + " is not in dictionary");

        foreach (GameObject item in PoolDictionary[tag])
        {
            bool isItemBeingUsed = item.activeSelf;
            if (!isItemBeingUsed)
            {
                obj = item;
                return true;
            }
        }
        return false;
    }

    void checkIPooledObjectInterface(GameObject obj)
    {
        IPooledObject pooledObject = obj.GetComponent<IPooledObject>();
        if (pooledObject != null) pooledObject.OnObjectSpawn();
    }

    GameObject instantiateNewObject(string tag)
    {
        for (int i = 0; i < Pools.Length; i++)
        {
            if(tag == Pools[i].tag)
            {
                GameObject obj;
                obj = instansiateItemAndAddToList(Pools[i].prefab, PoolDictionary[tag]);
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