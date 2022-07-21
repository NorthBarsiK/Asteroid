using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private Canvas hud = null;
    [SerializeField] private Canvas gameOverScreen = null;
    [SerializeField] private Canvas mainMenu = null;

    private bool isAllCanvasesAssigned = true;

    private void Start()
    {
        CheckCanvases();
    }

    public void GameOver()
    {
        if (isAllCanvasesAssigned)
        {
            hud.gameObject.SetActive(false);
            gameOverScreen.gameObject.SetActive(true);
            mainMenu.gameObject.SetActive(false);
            DisableMenuContinueButton();
        }
    }

    public void MainMenu()
    {
        if (isAllCanvasesAssigned)
        {
            hud.gameObject.SetActive(true);
            gameOverScreen.gameObject.SetActive(false);
            mainMenu.gameObject.SetActive(true);
        }
    }

    public void HUD()
    {
        if (isAllCanvasesAssigned)
        {
            hud.gameObject.SetActive(true);
            gameOverScreen.gameObject.SetActive(false);
            mainMenu.gameObject.SetActive(false);
        }
    }

    public void UpdateHudScore(int newScore)
    {
        if (hud != null)
        {
            hud.GetComponent<HUD>().UpdateScore(newScore);
        }
        else
        {
            Debug.LogError("HUD canvas is not assigned in UI manager");
        }
    }

    public void UpdateHudPlayerLives(int newLives)
    {
        if (hud != null)
        {
            hud.GetComponent<HUD>().UpdateLives(newLives);
        }
        else
        {
            Debug.LogError("HUD canvas is not assigned in UI manager");
        }
    }

    public void DisableMenuContinueButton()
    {
        if (mainMenu != null)
        {
            mainMenu.GetComponent<MainMenu>().DisableContinue();
        }
        else
        {
            Debug.LogError("Main menu canvas is not assigned in UI manager");
        }
    }
    
    private void CheckCanvases()
    {
        if(hud == null)
        {
            Debug.LogError("HUD canvas is not assigned in UI manager");
            isAllCanvasesAssigned = false;
        }

        if(mainMenu == null)
        {
            Debug.LogError("Main menu canvas is not assigned in UI manager");
            isAllCanvasesAssigned = false;
        }

        if (gameOverScreen == null)
        {
            Debug.LogError("Game over screen canvas is not assigned in UI manager");
            isAllCanvasesAssigned = false;
        }
    }
}
