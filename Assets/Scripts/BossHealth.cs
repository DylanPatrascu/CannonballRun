using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        Debug.Log("Boss took " + damageAmount + " of damage. Current health: " + currentHealth);

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
            Debug.Log("Boss is dead");
        }

    }
    public float GetHealth() => currentHealth;
}
