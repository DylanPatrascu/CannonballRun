using UnityEngine;
using UnityEngine.Rendering;

public class EnemyDrone : Enemy
{

    [Header("Shooting Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 20f;
    [SerializeField] private float fireRate = 2f;

    private float fireTimer = 0f;

    protected override void Start()
    {
        base.Start();
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
        damage = DifficultyScaler.GetProjectileDamage();
        Debug.Log($"[EnemyDrone] Damage set to {damage} at depth {DifficultyScaler.GetDepth()}");

    }

    protected override void Update()
    {
        base.Update();
        if (player == null) return;


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
        Vector3 playerVelocity = playerRb != null ? playerRb.linearVelocity : Vector3.zero; 

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
            rb.linearVelocity = aimDirection * projectileSpeed; 
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
        StaticData.scrap += scrapReward;
        Debug.Log("Drone Exploded! Awarding " + scrapReward + " scrap!");

        // Destroy all bullets belonging to this enemy
        EnemyProjectile[] allProjectiles = FindObjectsByType<EnemyProjectile>(FindObjectsSortMode.None);
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
