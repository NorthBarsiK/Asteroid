using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D), typeof(ObjectOutScreen))]
public class Asteroid : MonoBehaviour
{
    public enum childAsteroids
    {
        MiddleAsteroid = 0,
        SmallAsteroid = 1,
        None = 2
    };

    public childAsteroids childAsteroid = childAsteroids.MiddleAsteroid;

    public int scoreOnDestroy = 0;

    [SerializeField] private ParticleSystem destroyParticle = null;

    private float speed = 0.0f;

    private AsteroidManager asteroidManager = null;

    private void Start()
    {
        asteroidManager = AsteroidManager.Instance;
        GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    private void Update()
    {
        Movement();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.Die();
            Die();
        }
        else if (collision.gameObject.GetComponent<UFO>())
        {
            UFO ufo = collision.gameObject.GetComponent<UFO>();
            ufo.Die();
            Die();
        }
    }

    public void Die()
    {
        asteroidManager.SpawnChildAsteroids(childAsteroid, transform.position, transform.rotation.eulerAngles);
        if (destroyParticle != null)
        {
            Instantiate(destroyParticle, transform.position, transform.rotation);
        }
        else
        {
            Debug.Log("Destroy particle is not assigned in Asteroid prefab");
        }    
        gameObject.SetActive(false);
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    private void Movement()
    {
        transform.Translate(0, speed * Time.deltaTime, 0);
    }
}
