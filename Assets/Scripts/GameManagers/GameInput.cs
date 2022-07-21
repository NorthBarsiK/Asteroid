using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance;

    private void Awake()
    {
        Instance = this;
    }

    [HideInInspector] public bool accelerate;
    [HideInInspector] public bool shoot;
    [HideInInspector] public bool leftRotation;
    [HideInInspector] public bool rightRotation;
    [HideInInspector] public bool pause;
    [HideInInspector] public bool isMouseControl;

    private GameManager gameManager = null;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        CheckInput();
    }

    public void SetMouseControlState(bool state)
    {
        isMouseControl = state;
    }

    private void CheckInput()
    {
        if (!gameManager.isPaused)
        {
            if (isMouseControl)
            {
                shoot = Input.GetMouseButtonDown(0);
            }
            else
            {
                leftRotation = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
                rightRotation = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
                shoot = Input.GetKeyDown(KeyCode.Space);
            }

            accelerate = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
            pause = Input.GetKeyDown(KeyCode.Escape);

            if (pause)
            {
                gameManager.PauseGame();
            }
        }
    }
}
