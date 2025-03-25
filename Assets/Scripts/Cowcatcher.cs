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
        if (level == 1 && other.CompareTag("Enemy"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (Random.Range(0f, 1f) < 0.5f)
            {
                rb.AddForce(Vector3.right * 600f);
            }
            else
            {
                rb.AddForce(Vector3.left * 600f);
            }
        }
        else if (level == 2 && other.CompareTag("Enemy"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb.AddForce(Vector3.up * 1000f);
            if(Random.Range(0f, 1f) < 0.5f)
            {
                rb.AddForce(Vector3.right * 1000f);
            } else
            {
                rb.AddForce(Vector3.left * 1000f);
            }
        }

        else if (level == 3 && other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            explode.Play();

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (level == 1 && other.CompareTag("Enemy"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb.AddForce(Vector3.left * 300f);
        }
        else if (level == 2 && other.CompareTag("Enemy"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb.AddForce(Vector3.up * 1000f);
            rb.AddForce(Vector3.right * 1000f);
           
        }

        else
        {
            Destroy(other.gameObject);
        }
    }
}
