using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
    }

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public int size = 3;
        public GameObject prefab;
    }

    public List<Pool> pools = new List<Pool>();
    public Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    private void Start()
    {
        CreatePools();
    }

    public GameObject SpawnObject(string poolTag, Vector3 objectPosition, Quaternion objectRotation)
    {
        if (!poolDictionary.ContainsKey(poolTag))
        {
            Debug.Log(poolTag + " doesn't exist!");
            return null;
        }

        GameObject obj = poolDictionary[poolTag].Dequeue();

        obj.SetActive(true);
        obj.transform.SetPositionAndRotation(objectPosition, objectRotation);

        poolDictionary[poolTag].Enqueue(obj);

        return obj;
    }

    public void DestroyObjects(string poolTag)
    {
        if (!poolDictionary.ContainsKey(poolTag))
        {
            Debug.Log(poolTag + " doesn't exist!");
        }
        else
        {
            foreach (GameObject obj in poolDictionary[poolTag])
            {
                obj.SetActive(false);
            }
        }
    }

    private void CreatePools()
    {
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectQueue = new Queue<GameObject>();

            if (pool.prefab != null)
            {
                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.prefab);
                    obj.SetActive(false);
                    obj.transform.position = Vector3.zero;
                    obj.transform.rotation = Quaternion.Euler(Vector3.zero);
                    objectQueue.Enqueue(obj);
                }

                poolDictionary.Add(pool.tag, objectQueue);
            }
            else
            {
                Debug.LogError("Prefab is not assigned in " + pool.tag + " pool!");
            }

        }
    }
}
