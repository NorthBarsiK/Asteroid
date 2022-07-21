using System.Collections;
using UnityEngine;

public class UFOManager : MonoBehaviour
{
    public static UFOManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private UFO UFOPrefab = null;
    [SerializeField] private float spawnOffset = 15.0f;
    [SerializeField] private float minUFOCreateTime = 20.0f;
    [SerializeField] private float maxUFOCreateTime = 40.0f;

    private UFO ufoObj = null;
    private Camera cam = null;
    private ObjectPooler objectPool = null;

    private void Start()
    {
        cam = Camera.main;
        objectPool = ObjectPooler.Instance;
        InstantiateUFO();
    }

    public void SpawnUFO()
    {
        if (ufoObj != null)
        {
            float createPointY = Random.Range(0 + Screen.height / 5, Screen.height - (Screen.height / 5));
            float createPointX = Random.Range(0, 2);
            UFO.directions direction = UFO.directions.right;

            if (createPointX == 0)
            {
                createPointX = 0 - spawnOffset;
                direction = UFO.directions.right;
            }
            else
            {
                createPointX = Screen.width + spawnOffset;
                direction = UFO.directions.left;
            }

            Vector3 createScreenPoint = new Vector3(createPointX, createPointY, 0);
            Vector3 createWorldPoint = cam.ScreenToWorldPoint(createScreenPoint);

            ufoObj.direction = direction;
            ufoObj.transform.position = createWorldPoint;

            ufoObj.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("UFO doesn't have instance!");
        }
    }

    public void Reset()
    {
        ufoObj.gameObject.SetActive(false);
        objectPool.DestroyObjects("UFOBullet");
        DestroyUFO();
    }

    public void DestroyUFO()
    {
        StartCoroutine(CreateUFODelay());
    }

    private void InstantiateUFO()
    {
        if(UFOPrefab != null)
        {
            ufoObj = Instantiate(UFOPrefab, Vector3.zero, Quaternion.Euler(Vector3.zero));
            ufoObj.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("UFO prefab is not assigned in UFO manager");
        }
    }

    private IEnumerator CreateUFODelay()
    {
        float createDelay = Random.Range(minUFOCreateTime, maxUFOCreateTime);
        yield return new WaitForSeconds(createDelay);
        SpawnUFO();
    }
}
