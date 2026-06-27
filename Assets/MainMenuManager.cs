using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject optionsPanel;
    public GameObject leaderboardPanel;
    public GameObject creditsPanel;

    void Start()
    {
        if (mainPanel == null)
            mainPanel = GameObject.Find("Canvas/MainPanel");
        if (optionsPanel == null)
            optionsPanel = GameObject.Find("Canvas/OptionsPanel");
        if (leaderboardPanel == null)
            leaderboardPanel = GameObject.Find("Canvas/LeaderboardPanel");
        if (creditsPanel == null)
            creditsPanel = GameObject.Find("Canvas/CreditsPanel");

        var main = mainPanel.transform;
        main.Find("StartGameButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(StartGame);
        main.Find("OptionsButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OpenOptions);
        main.Find("LeaderboardButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OpenLeaderboard);
        main.Find("CreditsButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OpenCredits);

        optionsPanel.transform.Find("BackButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(BackToMenu);
        leaderboardPanel.transform.Find("BackButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(BackToMenu);
        creditsPanel.transform.Find("BackButton").GetComponent<UnityEngine.UI.Button>().onClick.AddListener(BackToMenu);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("StartGame");
    }

    public void OpenOptions()
    {
        ShowPanel(optionsPanel);
    }

    public void OpenLeaderboard()
    {
        ShowPanel(leaderboardPanel);
    }

    public void OpenCredits()
    {
        ShowPanel(creditsPanel);
    }

    public void BackToMenu()
    {
        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);
        leaderboardPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void ShowPanel(GameObject panel)
    {
        mainPanel.SetActive(false);
        panel.SetActive(true);
    }
}
