using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button continueButton = null;
    [SerializeField] private Button controlsButton = null;

    private string keyboardControlText = "CONTROLS: KEYBOARD";
    private string mouseControlText = "CONTROLS: KEYBOARD + MOUSE";

    private Color enableColor = Color.white;
    private GameManager gameManager = null;
    private GameInput input = null;

    private void Start()
    {
        if (continueButton != null)
        {
            enableColor = continueButton.GetComponentInChildren<Text>().color;
        }
        else
        {
            Debug.LogError("Continue button is not assigned is Main menu");
        }

        gameManager = GameManager.Instance;
        input = GameInput.Instance;
        DisableContinue();
    }

    public void DisableContinue()
    {
        if(continueButton != null)
        {
            continueButton.GetComponentInChildren<Text>().color = Color.gray;
        }
        else
        {
            Debug.LogError("Continue button is not assigned is Main menu");
        }
    }

    public void EnableContinue()
    {
        if (continueButton != null)
        {
            continueButton.GetComponentInChildren<Text>().color = enableColor;
        }
        else
        {
            Debug.LogError("Continue button is not assigned is Main menu");
        }
    }

    public void ContinueClick()
    {
        if(gameManager.isGameStarted)
        {
            gameManager.ContinueGame();
        }
    }

    public void NewGameClick()
    {
        gameManager.NewGame();
        EnableContinue();
    }

    public void ControlsClick()
    {
        if (controlsButton != null)
        {
            if (input.isMouseControl)
            {
                input.SetMouseControlState(false);
                controlsButton.GetComponentInChildren<Text>().text = keyboardControlText;
            }
            else
            {
                input.SetMouseControlState(true);
                controlsButton.GetComponentInChildren<Text>().text = mouseControlText;
            }
        }
        else
        {
            Debug.LogError("Controls button is not assigned is Main menu");
        }
    }

    public void QuitClick()
    {
        Application.Quit();
    }
}
