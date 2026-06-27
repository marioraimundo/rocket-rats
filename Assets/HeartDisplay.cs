using UnityEngine;
using UnityEngine.UI;

public class HeartDisplay : MonoBehaviour
{
    public Image[] hearts;

    void Start()
    {
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
