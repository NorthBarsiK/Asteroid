using UnityEngine;

[RequireComponent(typeof(AsteroidManager), typeof(PlayerManager), typeof(UFOManager))]
[RequireComponent(typeof(GameInput), typeof(ObjectPooler))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private int startPlayerLives = 100;

    [HideInInspector] public int score = 0;
    [HideInInspector] public int playerLives = 100;
    [HideInInspector] public bool isPaused = false;
    [HideInInspector] public bool isGameStarted = false;

    private UIManager uiManager = null;
    private AsteroidManager asteroidManager = null;
    private PlayerManager playerManager = null;
    private UFOManager ufoManager = null;

    private void Start()
    {
        uiManager = UIManager.Instance;
        asteroidManager = AsteroidManager.Instance;
        playerManager = PlayerManager.Instance;
        ufoManager = UFOManager.Instance;
        
        playerLives = startPlayerLives;
        uiManager.MainMenu();
        uiManager.UpdateHudPlayerLives(playerLives);
        uiManager.UpdateHudScore(score);
        PauseGame();
    }

    public void AddScore(int additiveScore)
    {
        score += additiveScore;
        uiManager.UpdateHudScore(score);
    }

    public void SubtractLives()
    {
        playerLives -= 1;
        uiManager.UpdateHudPlayerLives(playerLives);
        
        if (playerLives <= 0)
        {
            uiManager.GameOver();
        }
    }

    public void PauseGame()
    {
        if (!isPaused)
        {
            Time.timeScale = 0;
            isPaused = true;
            uiManager.MainMenu();
        }
    }

    public void ContinueGame()
    { 
        if (isPaused)
        {
            Time.timeScale = 1;
            isPaused = false;
            uiManager.HUD();
        }
    }

    public void NewGame()
    {
        Reset();
        isGameStarted = true;
        playerLives = startPlayerLives;
        score = 0;
        
        uiManager.HUD();
        uiManager.UpdateHudPlayerLives(playerLives);
        uiManager.UpdateHudScore(score);
        ContinueGame();
    }

    public void Reset()
    {
        ufoManager.Reset();
        playerManager.Reset();
        asteroidManager.Reset();
    }
}
