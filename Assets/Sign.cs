using Unity.Cinemachine;
using UnityEngine;

public class Sign : MonoBehaviour
{

    [SerializeField] CinemachineVirtualCamera cam;
    [SerializeField] Transform lookTarget;
    [SerializeField] Transform car;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            cam.LookAt = lookTarget;
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            cam.LookAt = car;
        }
    }
}
