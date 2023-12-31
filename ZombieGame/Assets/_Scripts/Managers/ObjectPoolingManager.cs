using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolingManager : StaticSingleton<ObjectPoolingManager>
{
    public List<ObjectPool> ObjectPools = new List<ObjectPool>();

    public GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        ObjectPool pool = ObjectPools.Find(p => p.LookupString == objectToSpawn.name);

        if (pool == null)
        {
            pool = new ObjectPool() { LookupString = objectToSpawn.name };
            ObjectPools.Add(pool);
        }

        GameObject spawnableObj = pool.AvaliableObjects.FirstOrDefault();
        if (spawnableObj == null)
        {
            spawnableObj = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
        }
        else
        {
            spawnableObj.transform.position = spawnPosition;
            spawnableObj.transform.rotation = spawnRotation;
            pool.AvaliableObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }
        return spawnableObj;
    }

    public void ReturnObjectToPool(GameObject obj)
    {

        string realName = obj.name.Substring(0, obj.name.Length - 7);
        ObjectPool pool = ObjectPools.Find(p => p.LookupString == realName);
        if (pool == null)
        {
            Debug.LogWarning("Tying to release an object that hasnt been pooled" + obj.name);
        }
        else
        {
            obj.SetActive(false);
            pool.AvaliableObjects.Add(obj);
        }
    }
}

public class ObjectPool
{
    public string LookupString;
    public List<GameObject> AvaliableObjects = new List<GameObject>();
}
