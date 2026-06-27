using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class SetupHeartUI
{
    [MenuItem("Tools/Rocket Rats - Setup Heart UI")]
    public static void Execute()
    {
        Canvas canvas = Object.FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasGO = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas = canvasGO.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }

        Transform canvasTransform = canvas.transform;

        Transform existingHearts = canvasTransform.Find("Hearts");
        if (existingHearts != null)
            GameObject.DestroyImmediate(existingHearts.gameObject);

        GameObject heartsGO = new GameObject("Hearts", typeof(HeartDisplay));
        heartsGO.transform.SetParent(canvasTransform, false);
        HeartDisplay heartDisplay = heartsGO.GetComponent<HeartDisplay>();

        RectTransform heartsRect = heartsGO.GetComponent<RectTransform>();
        heartsRect.anchorMin = new Vector2(0, 1);
        heartsRect.anchorMax = new Vector2(0, 1);
        heartsRect.pivot = new Vector2(0, 1);
        heartsRect.anchoredPosition = new Vector2(20, -20);
        heartsRect.sizeDelta = new Vector2(200, 40);

        Sprite heartSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/images/heart.png");

        Image[] hearts = new Image[3];
        for (int i = 0; i < 3; i++)
        {
            GameObject heartObj = new GameObject("Heart" + (i + 1), typeof(Image));
            heartObj.transform.SetParent(heartsGO.transform, false);

            RectTransform rt = heartObj.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(40, 40);
            rt.anchoredPosition = new Vector2(i * 50, 0);
            rt.anchorMin = new Vector2(0, 0.5f);
            rt.anchorMax = new Vector2(0, 0.5f);
            rt.pivot = new Vector2(0, 0.5f);

            Image img = heartObj.GetComponent<Image>();
            if (heartSprite != null)
                img.sprite = heartSprite;
            img.preserveAspect = true;
            hearts[i] = img;
        }
        heartDisplay.hearts = hearts;

        Transform existingPanel = canvasTransform.Find("GameOverPanel");
        if (existingPanel != null)
            GameObject.DestroyImmediate(existingPanel.gameObject);

        GameObject panelGO = new GameObject("GameOverPanel", typeof(Image), typeof(CanvasGroup));
        panelGO.transform.SetParent(canvasTransform, false);
        RectTransform panelRect = panelGO.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.sizeDelta = Vector2.zero;

        Image panelBg = panelGO.GetComponent<Image>();
        panelBg.color = new Color(0, 0, 0, 0.7f);
        CanvasGroup canvasGroup = panelGO.GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = true;
        panelGO.SetActive(false);

        Font defaultFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (defaultFont == null)
            defaultFont = Resources.GetBuiltinResource<Font>("Arial.ttf");

        GameObject titleTextGO = new GameObject("TitleText", typeof(Text));
        titleTextGO.transform.SetParent(panelGO.transform, false);
        RectTransform titleRect = titleTextGO.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 0.6f);
        titleRect.anchorMax = new Vector2(0.5f, 0.6f);
        titleRect.pivot = new Vector2(0.5f, 0.5f);
        titleRect.anchoredPosition = Vector2.zero;
        titleRect.sizeDelta = new Vector2(300, 60);
        Text titleText = titleTextGO.GetComponent<Text>();
        titleText.text = "Game Over";
        titleText.fontSize = 48;
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.color = Color.white;
        if (defaultFont != null) titleText.font = defaultFont;

        GameObject scoreTextGO = new GameObject("FinalScore", typeof(Text));
        scoreTextGO.transform.SetParent(panelGO.transform, false);
        RectTransform scoreRect = scoreTextGO.GetComponent<RectTransform>();
        scoreRect.anchorMin = new Vector2(0.5f, 0.45f);
        scoreRect.anchorMax = new Vector2(0.5f, 0.45f);
        scoreRect.pivot = new Vector2(0.5f, 0.5f);
        scoreRect.anchoredPosition = Vector2.zero;
        scoreRect.sizeDelta = new Vector2(300, 40);
        Text scoreText = scoreTextGO.GetComponent<Text>();
        scoreText.text = "Score: 0";
        scoreText.fontSize = 32;
        scoreText.alignment = TextAnchor.MiddleCenter;
        scoreText.color = Color.white;
        if (defaultFont != null) scoreText.font = defaultFont;

        GameObject restartBtnGO = new GameObject("RestartButton", typeof(Image), typeof(Button));
        restartBtnGO.transform.SetParent(panelGO.transform, false);
        RectTransform btnRect = restartBtnGO.GetComponent<RectTransform>();
        btnRect.anchorMin = new Vector2(0.5f, 0.3f);
        btnRect.anchorMax = new Vector2(0.5f, 0.3f);
        btnRect.pivot = new Vector2(0.5f, 0.5f);
        btnRect.anchoredPosition = Vector2.zero;
        btnRect.sizeDelta = new Vector2(200, 50);
        Image btnImage = restartBtnGO.GetComponent<Image>();
        btnImage.color = new Color(0.2f, 0.6f, 0.2f);
        Button btn = restartBtnGO.GetComponent<Button>();

        GameObject btnTextGO = new GameObject("Text", typeof(Text));
        btnTextGO.transform.SetParent(restartBtnGO.transform, false);
        RectTransform btnTextRect = btnTextGO.GetComponent<RectTransform>();
        btnTextRect.anchorMin = Vector2.zero;
        btnTextRect.anchorMax = Vector2.one;
        btnTextRect.sizeDelta = Vector2.zero;
        Text btnText = btnTextGO.GetComponent<Text>();
        btnText.text = "Restart";
        btnText.fontSize = 28;
        btnText.alignment = TextAnchor.MiddleCenter;
        btnText.color = Color.white;
        if (defaultFont != null) btnText.font = defaultFont;

        LogicManager logic = Object.FindFirstObjectByType<LogicManager>();
        if (logic != null)
        {
            logic.gameOverPanel = panelGO;
            btn.onClick.AddListener(() => logic.RestartGame());
        }

        Rat rat = Object.FindFirstObjectByType<Rat>();
        if (rat != null)
        {
            rat.heartDisplay = heartDisplay;
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());

        Debug.Log("Heart UI setup complete!");
        EditorUtility.DisplayDialog("Success", "Heart UI setup complete!", "OK");
    }
}
