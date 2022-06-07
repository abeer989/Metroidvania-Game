using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Slider healthBar;

    private void Awake()
    {
        // Singleton:
        if (!instance)
            instance = this;

        else
        {
            Destroy(instance);
            instance = this;
        }
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }
}
