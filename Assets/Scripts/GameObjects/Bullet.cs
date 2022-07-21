using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D), typeof(ObjectOutScreen))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 0.0f;

    private float bulletMaxDistance = 0.0f;
    private float currentBulletDistance = 0.0f;

    private GameManager gameManager = null;
    private Camera cam = null;

    private void Start()
    {
        gameManager = GameManager.Instance;
        cam = Camera.main;
        SetBulletDistance();
        GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    private void Update()
    {
        Movement();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool isThisPlayerBullet = gameObject.layer == LayerMask.NameToLayer("PlayerBullet");

        if (collision.gameObject.GetComponent<Player>())
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.Die();
        }
        else if (collision.gameObject.GetComponent<Asteroid>())
        {
            Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();
            asteroid.Die();
            if (isThisPlayerBullet)
            {
                int score = asteroid.scoreOnDestroy;
                gameManager.AddScore(score);
            }
        }
        else if (collision.gameObject.GetComponent<UFO>())
        {
            UFO ufo = collision.gameObject.GetComponent<UFO>();
            ufo.Die();
            if (isThisPlayerBullet)
            {
                int score = ufo.scoreOnDestroy;
                gameManager.AddScore(score);
            }
        }

        Die();
    }

    private void SetBulletDistance()
    {
        Vector3 screenBegin = cam.ScreenToWorldPoint(Vector3.zero);
        Vector3 screenEnd = cam.ScreenToWorldPoint(Vector3.right * Screen.width);
        float distance = screenEnd.x - screenBegin.x;
        bulletMaxDistance = distance;
    }

    private void Movement()
    {
        currentBulletDistance += speed * Time.deltaTime;
        transform.Translate(0, speed * Time.deltaTime, 0);
        if (currentBulletDistance >= bulletMaxDistance && gameObject.activeSelf)
        {
            Die();
        }
    }

    private void Die()
    {
        currentBulletDistance = 0.0f;
        gameObject.SetActive(false);
    }
}
