using UnityEngine;
using UnityEngine.Rendering;

public class EnemyDrone : Enemy
{

    [Header("Drone Specifics")]
    [SerializeField] private Transform player; 
    [SerializeField] private float moveSpeed = 5f;
    
    [SerializeField] private float detectionRange = 15f;
    
    [SerializeField] private float followDistance = 10f; 
    [SerializeField] private float followHeight = 5f;

    [Header("Shooting Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 20f;
    [SerializeField] private float fireRate = 2f;

    private float fireTimer = 0f;

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
    }

    private void Update()
    {
        if (player == null) return;

        Vector3 targetPosition = player.position + (player.forward * followDistance);
        targetPosition.y = followHeight;

        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // This is the shooting timer
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            Debug.Log("Enemy shooting at player!");
            Shoot();
            fireTimer = 0f;
        }
    }

    private void Shoot()
    {
        if (projectilePrefab == null || firePoint == null || player == null) return;

        Debug.Log("Enemy fired a shot!");

        Rigidbody playerRb = player.GetComponent<Rigidbody>(); 
        Vector3 playerVelocity = playerRb != null ? playerRb.velocity : Vector3.zero; 

        float distanceToPlayer = Vector3.Distance(firePoint.position, player.position);
        float timeToReach = distanceToPlayer / projectileSpeed; // This will estimate the time for projectile to reach the player

        // Predicts the future position of the player
        Vector3 futurePosition = player.position + (playerVelocity * timeToReach);

        
        Vector3 aimDirection = (futurePosition - firePoint.position).normalized;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(aimDirection));

        if (projectile == null)
        {
            return;
        }

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = aimDirection * projectileSpeed; 
        }

        Debug.Log("Shot fired successfully!");

        EnemyProjectile enemyProjectile = projectile.GetComponent<EnemyProjectile>();
        if (enemyProjectile != null)
        {
            enemyProjectile.SetDamage(damage);
            enemyProjectile.SetOwner(this);
        }
    }


    protected override void Die()
    {
        isDead = true;
        Debug.Log("Drone Exploded! Awarding " + scrapReward + " scrap!");

        // Destroy all bullets belonging to this enemy
        EnemyProjectile[] allProjectiles = FindObjectsOfType<EnemyProjectile>();
        foreach (EnemyProjectile projectile in allProjectiles)
        {
            if (projectile != null)
            {
                projectile.DestroyProjectile();
            }
        }
        base.Die();

        // This is where I add death effects
        Debug.Log("Drone Exploded");
    }
}
