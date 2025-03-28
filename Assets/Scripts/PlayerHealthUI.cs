using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Slider healthSlider;

    void Start()
    {
        if (playerHealth == null)
            playerHealth = FindObjectOfType<PlayerHealth>();

        healthSlider.maxValue = playerHealth.GetMaxHealth();
        healthSlider.value = playerHealth.GetHealth();

        playerHealth.onHealthChanged += UpdateHealthUI;
    }

    private void UpdateHealthUI(float newHealth)
    {
        healthSlider.value = newHealth;
    }

    private void OnDestroy()
    {
        // Clean up the subscription
        if (playerHealth != null)
            playerHealth.onHealthChanged -= UpdateHealthUI;
    }
}

