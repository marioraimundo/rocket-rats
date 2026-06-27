using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CheeseCounter : MonoBehaviour
{
    private Image cheeseIcon;
    private TMP_Text cheeseText;
    private int cheeseCount;

    void Start()
    {
        Transform cheeseContainer = transform.Find("Cheese");
        if (cheeseContainer != null)
        {
            Transform iconT = cheeseContainer.Find("CheeseIcon");
            if (iconT != null) cheeseIcon = iconT.GetComponent<Image>();
            Transform textT = cheeseContainer.Find("CheeseText");
            if (textT != null) cheeseText = textT.GetComponent<TMP_Text>();
        }
        UpdateUI();
    }

    public void AddCheese()
    {
        cheeseCount++;
        UpdateUI();
    }

    public int GetCheeseCount()
    {
        return cheeseCount;
    }

    private void UpdateUI()
    {
        if (cheeseText != null)
            cheeseText.text = "x " + cheeseCount;
    }
}
