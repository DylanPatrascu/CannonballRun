using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Health Stats")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    private bool isDead = false;

    private CarDeath carDeath;

    void Start()
    {
        currentHealth = maxHealth;
        carDeath = GetComponent<CarDeath>(); //  CarDeath.cs should be on the same object
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        Debug.Log("Player took " + damageAmount + " of damage. Current health: " + currentHealth);

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
}
