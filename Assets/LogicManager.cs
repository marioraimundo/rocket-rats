using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LogicManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public TMP_Text cheeseFinalText;

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void GameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            CheeseCounter counter = FindAnyObjectByType<CheeseCounter>();
            if (cheeseFinalText != null && counter != null)
                cheeseFinalText.text = "Queijos: " + counter.GetCheeseCount();
        }

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
