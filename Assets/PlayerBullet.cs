using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boss"))
        {
            BossHealth bossHealth = other.GetComponent<BossHealth>();
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(5);
            }

            Destroy(gameObject);
            return;
        }

        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(10f);
            Debug.Log("Hit enemy: " + enemy.name);
            Destroy(gameObject);
        }

    }
}
