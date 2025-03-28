using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    private bool isDead = false;

    public ParticleSystem explosion;
    private float timer = 0;

    void Start()
    {
        currentHealth = maxHealth;
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            timer += Time.fixedDeltaTime;
            if (timer > 1){
                SceneManager.LoadScene("WinScene");
            }
        }
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
            explosion.Play();
        }

    }
    public float GetHealth() => currentHealth;
}
