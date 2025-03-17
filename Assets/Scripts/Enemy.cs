using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] protected float health = 100f;
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected float attackSpeed = 1f;
    [SerializeField] protected int scrapReward = 10;

    protected bool isDead = false;

    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log("Enemy took " + amount + " damage. Health left: " + health);


        if (health <= 0) {
            Debug.Log("Enemy Destroyed!");
            Die();
        }
    }

    protected virtual void Die()
    {
        isDead = true;
        Debug.Log("Enemy was defeated. Awarding " + scrapReward + " scrap!");
        EnemyProjectile[] allProjectiles = FindObjectsOfType<EnemyProjectile>();
        foreach (EnemyProjectile projectile in allProjectiles)
        {
            if (projectile != null)
            {
                projectile.DestroyProjectile();
            }
        }
        Destroy(gameObject);
    }

    public float GetDamage() => damage;
    public float GetAttackSpeed() => attackSpeed;
    public int GetScrapReward() => scrapReward;

}
