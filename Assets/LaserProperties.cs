using UnityEngine;

public class LaserProperties : MonoBehaviour
{
    public bool dangerous = false;

    private void OnTriggerEnter(Collider other)
    {
        if (dangerous)
        {
            if (other.CompareTag("Player"))
                    {
                        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                        if (playerHealth != null)
                        {
                            playerHealth.TakeDamage(5);
                        }
                    }
                }
        }
        
}
