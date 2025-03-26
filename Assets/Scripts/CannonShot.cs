using Unity.Burst.Intrinsics;
using UnityEngine;

public class CannonShot : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 5f;
    public int damage = 10;

    private Transform player;
    private Rigidbody rb;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        rb = GetComponent<Rigidbody>();

        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;

            rb.linearVelocity = direction * speed;
        }

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(5);
            }

            Destroy(gameObject);
        }
    }
}
