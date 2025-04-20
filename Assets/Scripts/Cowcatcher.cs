using UnityEngine;


public class Cowcatcher : MonoBehaviour
{
    [SerializeField][RangeAttribute(1, 3)] private int level = 1;
    public ParticleSystem explode;

    private void Start()
    {
        explode.Pause();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy"))
        {
            return;
        }

        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (!rb)
        {
            return;
        }
        NPCController npc = other.GetComponent<NPCController>();
        if (!npc)
        {
            return;
        }

        switch (level)
        {
            case 1:
                // Medium push forward and a little upward
                npc.TakeDamage(10);
                rb.AddForce(Vector3.forward * 5000f, ForceMode.Impulse);
                rb.AddForce(Vector3.up * 1000f, ForceMode.Impulse);
                rb.AddForce(Random.value < 0.5f ? Vector3.right * 500f : Vector3.left * 500f, ForceMode.Impulse);
                break;

            case 2:
                // Harder upward and sideways knock
                npc.TakeDamage(15);
                rb.AddForce(Vector3.up * 2000f, ForceMode.Impulse);
                rb.AddForce(Random.value < 0.5f ? Vector3.right * 1500f : Vector3.left * 1500f, ForceMode.Impulse);
                break;

            case 3:
                // Destroy enemy + particle effect
                npc.Die();
                explode.Play();
                break;
        }
    }

}
