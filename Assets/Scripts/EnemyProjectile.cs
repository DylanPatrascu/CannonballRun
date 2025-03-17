using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private float damage;
    [SerializeField] private float lifetime = 2f;

    private Enemy ownerEnemy;

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    public void SetOwner(Enemy enemy)
    {
        ownerEnemy = enemy;
    }

    private void Start()
    {

    }

    public void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
