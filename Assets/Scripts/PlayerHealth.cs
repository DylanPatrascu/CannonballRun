using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Health Stats")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    private bool isDead = false;

    private CarDeath carDeath;

    public delegate void OnHealthChanged(float newHealth);
    public event OnHealthChanged onHealthChanged;

    void Start()
    {
        maxHealth += StaticData.healthIncrease;
        currentHealth = maxHealth;
        carDeath = GetComponent<CarDeath>(); //  CarDeath.cs should be on the same object
        onHealthChanged?.Invoke(currentHealth);
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        Debug.Log("Player took " + damageAmount + " of damage. Current health: " + currentHealth);
        onHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            TriggerDeath();
        }
    }

    private void TriggerDeath()
    {
        if (!isDead)
        {
            isDead = true;
            Debug.Log("Player is dead! Triggering CarDeath...");
            carDeath?.TriggerDeath();
        }
    }

    public float GetHealth() => currentHealth;
    public float GetMaxHealth() => maxHealth;
}
