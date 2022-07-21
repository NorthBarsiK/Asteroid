using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private Text scoreText = null;
    [SerializeField] private Text playerLivesText = null;

    public void UpdateScore(int newScore)
    {
        if (scoreText != null)
        {
            string newText = string.Format("SCORE: {0:d6}", newScore);
            scoreText.text = newText;
        }
        else
        {
            Debug.LogError("Score text is not assigned in HUD!");
        }
    }

    public void UpdateLives(int newLives)
    {
        if (playerLivesText != null)
        {
            string newText = string.Format("X {0:d3}", newLives);
            playerLivesText.text = newText;
        }
        else
        {
            Debug.LogError("Player lives text is not assigned in HUD!");
        }
    }
}
