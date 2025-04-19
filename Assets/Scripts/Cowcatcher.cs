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
            Debug.Log("Before:" + rb.linearVelocity);
            rb.AddForce(Vector3.forward * 100000f);

            rb.AddForce(Vector3.up * 100000f);

            if (Random.Range(0f, 1f) < 0.5f)
            {
                rb.AddForce(Vector3.right * 5000f);
            }
            else
            {
                rb.AddForce(Vector3.left * 5000f);
            }
            Debug.Log("After:" + rb.linearVelocity);

        }
        else if (level == 2 && other.CompareTag("Enemy"))
        {

            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb.AddForce(Vector3.up * 10000f);
            if(Random.Range(0f, 1f) < 0.5f)
            {
                rb.AddForce(Vector3.right * 10000f);
            } else
            {
                rb.AddForce(Vector3.left * 10000f);
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
            Debug.Log("Before:" + rb.linearVelocity);
            rb.AddForce(Vector3.forward * 100000f);

            rb.AddForce(Vector3.up * 100000f);

            if (Random.Range(0f, 1f) < 0.5f)
            {
                rb.AddForce(Vector3.right * 5000f);
            }
            else
            {
                rb.AddForce(Vector3.left * 5000f);
            }
            Debug.Log("After:" + rb.linearVelocity);

        }

        else if (level == 2 && other.CompareTag("Enemy"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb.AddForce(Vector3.up * 1000f);
            rb.AddForce(Vector3.right * 1000f);
           
        }
        else if (level == 3 && other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            explode.Play();

        }
    }
}
