using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(ObjectOutScreen))]
public class UFO : MonoBehaviour
{
    public enum directions 
    {
        left = 0,
        right = 1
    };

    public directions direction = directions.left;

    public int scoreOnDestroy = 0;

    [SerializeField] private float movingTime = 0.0f;
    [SerializeField] private float minShootingDelay = 2.0f;
    [SerializeField] private float maxShootingDelay = 5.0f;

    [SerializeField] private ParticleSystem destroyParticle = null;

    private float speed = 0.0f;

    private ObjectPooler objectPool = null;
    private PlayerManager playerManager = null;
    private UFOManager ufoManager = null;

    private void Start()
    {
        objectPool = ObjectPooler.Instance;
        playerManager = PlayerManager.Instance;
        ufoManager = UFOManager.Instance;
        SetSpeed();
        StartCoroutine(Shooting());
    }

    private void Update()
    {
        Movement();
    }

    public void Die()
    {
        ufoManager.DestroyUFO();
        
        if (destroyParticle != null)
        {
            Instantiate(destroyParticle, transform.position, transform.rotation);
        }
        else
        {
            Debug.Log("Destroy particle not assigned in UFO prefab!");
        }

        gameObject.SetActive(false);
    }

    private void Movement()
    {
        float velocity = 0.0f;

        if(direction == directions.left)
        {
            velocity = -speed;
        }
        else
        {
            velocity = speed;
        }

        transform.Translate(velocity * Time.deltaTime, 0, 0);
    }

    private void SetSpeed()
    {
        Camera cam = Camera.main;
        float screenWidth = Screen.width;
        float screenBegin = cam.ScreenToWorldPoint(Vector3.zero).x;
        float screenEnd = cam.ScreenToWorldPoint(Vector3.right * screenWidth).x;
        float distance = screenEnd - screenBegin;
        speed = distance / movingTime;
    }

    private IEnumerator Shooting()
    {      
        float shootingDelay = Random.Range(minShootingDelay, maxShootingDelay);
        yield return new WaitForSeconds(shootingDelay);

        Vector3 playerPosition = playerManager.GetPlayerPosition();
        Vector3 targetRotation = playerPosition - transform.position;
        objectPool.SpawnObject("UFOBullet", transform.position, Quaternion.LookRotation(Vector3.forward, targetRotation));
        
        StartCoroutine(Shooting());
    }
}
