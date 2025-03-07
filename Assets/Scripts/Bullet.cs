using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Datamembers

    #region Editor Settings

    [SerializeField] private float speed;

    #endregion
    #region Private Fields

    private bool destroyed = false;

    #endregion

    #endregion


    #region Methods

    #region Unity Callbacks

    private void Start()
    {
        var rigidBody = GetComponent<Rigidbody>();

        // Rotate the bullet's forward direction by -90 degrees
        Vector3 adjustedDirection = Quaternion.Euler(0, 90, 0) * transform.forward;

        // Apply velocity in the corrected direction
        rigidBody.linearVelocity = adjustedDirection * speed;

        transform.rotation = Quaternion.LookRotation(adjustedDirection, Vector3.up);
        Destroy(gameObject, 10f);

    }




    private void OnCollisionEnter(Collision col)
    {
        Debug.Log("Print");
        if (destroyed == false)
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #endregion
}
