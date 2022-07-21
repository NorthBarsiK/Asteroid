using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private Player playerPrefab = null;
    [SerializeField] private float playerCreationDelay = 3.0f;

    private Player playerObj = null;
    private GameManager gameManager = null;
    private ObjectPooler objectPool = null;

    private void Start()
    {
        gameManager = GameManager.Instance;
        objectPool = ObjectPooler.Instance;
        InstantiatePlayer();
    }

    public Vector3 GetPlayerPosition()
    {
        if (playerObj != null)
        {
            return playerObj.transform.position;
        }
        else
        {
            Debug.Log("Player doesn't have instance!");
            return Vector3.zero;
        }
    }

    public void Reset()
    {
        objectPool.DestroyObjects("PlayerBullet");
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        if (playerObj != null)
        {
            playerObj.gameObject.SetActive(true);
            playerObj.transform.SetPositionAndRotation(Vector3.zero, Quaternion.Euler(Vector3.zero));
            playerObj.ResetMovement();
            playerObj.SetInvicibleState(true);
        }
        else
        {
            Debug.Log("Player doesn't have instance!");
        }
    }

    private void InstantiatePlayer()
    {
        if (playerPrefab != null)
        {
            playerObj = Instantiate(playerPrefab, Vector3.zero, Quaternion.Euler(Vector3.zero));
        }
        else
        {
            Debug.LogError("Player prefab is not assigned in Player manager");
        }
    }

    public IEnumerator SpawnPlayerCoroutine()
    {
        gameManager.SubtractLives();

        yield return new WaitForSeconds(playerCreationDelay);

        if (!(gameManager.playerLives <= 0))
        {
            SpawnPlayer();
        }
    }
}
