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

        var panel = GameObject.Find("Canvas/GameOverPanel").transform;
        panel.Find("PlayAgainButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(PlayAgain);
        panel.Find("MainMenuButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(MainMenu);
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

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartGame");
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
