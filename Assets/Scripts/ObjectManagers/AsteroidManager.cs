using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    public static AsteroidManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private float createOffset = 40.0f;
    [SerializeField] private float minSpeed = 1.0f;
    [SerializeField] private float maxSpeed = 5.0f;
    [SerializeField] private int startAsteroidCount = 2;
    [SerializeField] private int currentAsteroidCount = 0;
    [SerializeField] private bool isEnableAsteroids = true;
    [SerializeField] private float spawningAngle = 45.0f;

    private int asteroidCount = 0;

    private ObjectPooler objcetPool = null;

    private void Start()
    {
        objcetPool = ObjectPooler.Instance;
        asteroidCount = startAsteroidCount;
    }

    private void Update()
    {
        CheckAsteroidsInGame();
    }

    public void SpawnAsteroid(int creatingCount)
    {
        for (int i = 0; i < creatingCount; i++)
        {
            float createPointX = Random.Range(0 - createOffset, Screen.width + createOffset);
            float createPointY = Random.Range(0, 1);
            
            if (createPointY == 0)
            {
                createPointY = 0 - createOffset;
            }
            else
            {
                createPointY = Screen.height + createOffset;
            }

            Vector3 createPoint = new Vector3(createPointX, createPointY, 0);
            Vector3 createWorldPoint = Camera.main.ScreenToWorldPoint(createPoint);
            
            float rotation = Random.Range(0.0f, 360.0f);
            float speed = Random.Range(minSpeed, maxSpeed);

            GameObject newAsteroid = objcetPool.SpawnObject("BigAsteroid", createWorldPoint, Quaternion.Euler(Vector3.forward * rotation));
            newAsteroid.GetComponent<Asteroid>().SetSpeed(speed);
        }
    }
    
    public void SpawnChildAsteroids(Asteroid.childAsteroids child, Vector3 position, Vector3 rotationEuler)
    {
        string asteroid = "";

        if (child == Asteroid.childAsteroids.MiddleAsteroid)
        {
            asteroid = "MiddleAsteroid";
        }
        else if (child == Asteroid.childAsteroids.SmallAsteroid)
        {
            asteroid = "SmallAsteroid";
        }

        if (asteroid != "")
        {
            float speed = Random.Range(minSpeed, maxSpeed);
            GameObject newAsteroid = new GameObject();
            Vector3 rotation = new Vector3();

            rotation = rotationEuler + Vector3.forward * spawningAngle;
            newAsteroid = objcetPool.SpawnObject(asteroid, position, Quaternion.Euler(rotation));
            newAsteroid.GetComponent<Asteroid>().SetSpeed(speed);

            rotation = rotationEuler + Vector3.forward * -spawningAngle;
            newAsteroid = objcetPool.SpawnObject(asteroid, position, Quaternion.Euler(rotation));
            newAsteroid.GetComponent<Asteroid>().SetSpeed(speed);
            AddAsteroid(2);
        }

        SubtractAsteroid();
    }

    public void Reset()
    {
        objcetPool.DestroyObjects("BigAsteroid");
        objcetPool.DestroyObjects("MiddleAsteroid");
        objcetPool.DestroyObjects("SmallAsteroid");

        asteroidCount = startAsteroidCount;
        if (isEnableAsteroids)
        {
            SpawnAsteroid(asteroidCount);
            currentAsteroidCount = asteroidCount;
            asteroidCount++;
        }
    }

    private void AddAsteroid(int additive)
    {
        currentAsteroidCount += additive;
    }

    private void SubtractAsteroid()
    {
        currentAsteroidCount -= 1;
    }

    private void CheckAsteroidsInGame()
    {
        if (currentAsteroidCount <= 0 && isEnableAsteroids)
        {
            SpawnAsteroid(asteroidCount);
            currentAsteroidCount = asteroidCount;
            asteroidCount++;
        }
    }
}
