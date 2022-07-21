using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D), typeof(ObjectOutScreen))]
public class Player : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5.0f;
    [SerializeField] private float accelerate = 0.5f;
    [SerializeField] private float rotationSpeed = 15.0f;
    [SerializeField] private float rotationSpeedwithMouse = 1.0f;
    [SerializeField] private int bulletPerSecond = 3;
    [SerializeField] private int secondsInInvicible = 3;

    [SerializeField] private Transform directionPoint = null;
    [SerializeField] private Transform bulletSpawnPoint = null;
    [SerializeField] private Animator turboAnimator = null;
    [SerializeField] private Animator spriteAnimator = null;
    [SerializeField] private AudioSource turboAudioSource = null;
    [SerializeField] private AudioSource shootAudioSource = null;
    [SerializeField] private ParticleSystem destroyParticle = null;

    private bool isCanAccelerate = true;
    private bool isCanShoot = true;
    private bool isCanRotate = true;
    private bool isDirectionAssigned = false;

    private GameInput input = null;
    private PlayerManager playerManager = null;
    private ObjectPooler objectPool = null;
    private Camera cam = null;
    
    private Vector3 velocity = Vector3.zero;
    private Vector3 direction = Vector3.zero;

    private void Start()
    {
        cam = Camera.main;
        input = GameInput.Instance;
        playerManager = PlayerManager.Instance;
        objectPool = ObjectPooler.Instance;
        GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    private void Update()
    {
        Movement();
        Rotation();
        Shooting();
    }

    public void Die()
    {
        playerManager.StartCoroutine(playerManager.SpawnPlayerCoroutine());
        if (destroyParticle != null)
        {
            Instantiate(destroyParticle, transform.position, transform.rotation);
        }
        else
        {
            Debug.Log("Destroy particle not assigned in Player prefab!");
        }
        gameObject.SetActive(false);
    }

    public void ResetMovement()
    {
        velocity = Vector3.zero;
        isDirectionAssigned = false;
        isCanShoot = true;
    }

    public void SetInvicibleState(bool state)
    {
        if (state)
        {
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(InvicibleTimer());
            spriteAnimator.SetBool("IsInvicible", true);
        }
        else
        {
            GetComponent<Collider2D>().enabled = true;
            spriteAnimator.SetBool("IsInvicible", false);
        }
    }
    private void Movement()
    {
        if (directionPoint == null)
        {
            Debug.LogError("Direction point of Player is not assigned!!!");
        }
        else
        {
            bool isAccelerate = input.accelerate && isCanAccelerate;

            if (isAccelerate)
            {
                isCanRotate = false;
                
                if (isDirectionAssigned)
                {
                    velocity += (direction.normalized * accelerate) * Time.deltaTime;
                }
                else
                {
                    direction = directionPoint.position - transform.position;
                    isDirectionAssigned = true;
                }
            }
            else
            {
                isCanRotate = true;
            }

            if(turboAnimator != null)
            {
                if(isAccelerate)
                {
                    turboAnimator.SetBool("IsMoving", true);
                }
                else
                {
                    turboAnimator.SetBool("IsMoving", false);
                }
            }
            else
            {
                Debug.Log("Animator of turbo is not assigned in Player prefab");
            }

            if(turboAudioSource != null)
            {
                if (isAccelerate)
                {
                    if (!turboAudioSource.isPlaying)
                    {
                        turboAudioSource.Play();
                    }
                }
                else
                {
                    turboAudioSource.Stop();
                }
            }
            else
            {
                Debug.Log("Audio Source of turbo is not assigned in Player prefab");
            }

            velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);
            velocity.y = Mathf.Clamp(velocity.y, -maxSpeed, maxSpeed);
            transform.Translate(velocity * Time.deltaTime, Space.World);
        }
    }

    private void Rotation()
    {
        if (isCanRotate)
        {
            if (input.isMouseControl)
            {
                Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
                Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, mousePosition - transform.position);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeedwithMouse * Time.deltaTime);
                
                if (transform.rotation != targetRotation)
                {
                    isDirectionAssigned = false;
                }
            }
            else
            {
                bool isLeftRotation = input.leftRotation;
                bool isRightRotation = input.rightRotation;
                float rotation = 0.0f;

                if (isLeftRotation)
                {
                    isCanAccelerate = false;
                    isDirectionAssigned = false;
                    rotation = rotationSpeed;
                }
                else if (isRightRotation)
                {
                    isCanAccelerate = false;
                    isDirectionAssigned = false;
                    rotation = -rotationSpeed;
                }
                else
                {
                    isCanAccelerate = true;
                }

                transform.Rotate(0, 0, rotation * Time.deltaTime);
            }
        }
    }

    private void Shooting()
    {
        if(input.shoot && isCanShoot)
        {
            Vector3 _bulletSpawnPoint = transform.position;

            if(bulletSpawnPoint != null)
            {
                _bulletSpawnPoint = bulletSpawnPoint.position;
            }
            else
            {
                Debug.Log("Bullet spawn point is not assigned in Player prefab!");
            }

            if (shootAudioSource != null)
            {
                shootAudioSource.Play();
            }
            else
            {
                Debug.Log("Shoot Audio Source is not assigned in Player prefab!");
            }

            objectPool.SpawnObject("PlayerBullet", _bulletSpawnPoint, transform.rotation);
            isCanShoot = false;
            
            StartCoroutine(ShootingDelay());
        }
    }

    private IEnumerator ShootingDelay()
    {
        float shootingDelayTime = 1.0f / bulletPerSecond;
        yield return new WaitForSeconds(shootingDelayTime);
        isCanShoot = true;
    }

    private IEnumerator InvicibleTimer()
    {
        yield return new WaitForSeconds(secondsInInvicible);
        SetInvicibleState(false);
    }
}
