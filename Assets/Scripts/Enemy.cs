using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] protected float health = 100f;
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected float attackSpeed = 1f;
    [SerializeField] protected int scrapReward = 10;
    [SerializeField] public Transform player;
    [SerializeField] protected bool shouldLookAtPlayer = true;
    [SerializeField] protected float moveSpeed = 4f;
    [SerializeField] public float spawnDistance = 10f;
    [SerializeField] private Vector3 lookAtRotationOffset = new Vector3(0f, 90f, 0f);
    [SerializeField] private GameObject deathVFX;



    protected Vector3 offsetFromPlayer;

    protected bool isDead = false;


    public void SetOffset(Vector3 offset)
    {
        offsetFromPlayer = offset;
    }
    protected virtual void Start() {
        if (player == null) {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) {
                player = playerObj.transform;
            }
        }
    }

    protected virtual void Update() {


        if (player == null) return;

        Vector3 targetPos = player.position + offsetFromPlayer;

        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (shouldLookAtPlayer) {
            Vector3 dir = (player.position - transform.position).normalized;
            Quaternion rot = Quaternion.LookRotation(dir);
            Quaternion fix = Quaternion.Euler(lookAtRotationOffset);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot * fix, Time.deltaTime * 5f);

        }
    }


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

        if (deathVFX != null)
        {
            GameObject fx = Instantiate(deathVFX, transform.position, Quaternion.identity);
            Destroy(fx, 2f); 
        }

        StaticData.scrap += scrapReward;
        Debug.Log("Enemy was defeated. Awarding " + scrapReward + " scrap!");

        EnemyManager manager = FindFirstObjectByType<EnemyManager>();
        if (manager != null)
            manager.NotifyEnemyDeath(gameObject);

        Destroy(gameObject); // This structure will ensure that the VFX can play before the drone is destroyed
    }


    public float GetDamage() => damage;
    public float GetAttackSpeed() => attackSpeed;
    public int GetScrapReward() => scrapReward;

}
