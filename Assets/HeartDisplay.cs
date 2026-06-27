using UnityEngine;
using UnityEngine.UI;

public class HeartDisplay : MonoBehaviour
{
    public Image[] hearts;

    void Start()
    {
        Transform heartsContainer = transform.Find("Hearts");
        if (heartsContainer != null)
            hearts = heartsContainer.GetComponentsInChildren<Image>();
        else
            hearts = GetComponentsInChildren<Image>();
        UpdateHearts(3);
    }

    public void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < currentHealth;
        }
    }
}
