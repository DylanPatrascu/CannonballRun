using UnityEngine;


public class Cowcatcher : MonoBehaviour
{
    [SerializeField][RangeAttribute(1, 3)] private int level = 1;
    //public ParticleSystem explode;

    private void Start()
    {
        //explode.Pause();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy"))
        {
            return;
        }

        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        if (!rb)
        {
            Debug.Log("NO RB");

            return;
        }
        Debug.Log(rb);

        NPCController npc = other.gameObject.GetComponent<NPCController>();
        if (!npc)
        {
            Debug.Log("NO NPC");
            return;
        }
        Debug.Log(npc);

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
                rb.AddForce(Random.value < 0.5f ? Vector3.right * 3000f : Vector3.left * 3000f, ForceMode.Impulse);
                break;

            case 3:
                // Destroy enemy + particle effect
                npc.Die();
                //explode.Play();
                break;
        }
    }

}
