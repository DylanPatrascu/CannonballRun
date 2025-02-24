using UnityEngine;

public class MenuCarSelection : MonoBehaviour
{
    [SerializeField] float m_rotateSpeed = 25;

    void Update()
    {
        transform.Rotate(new Vector3(0, Time.deltaTime * m_rotateSpeed, 0));
    }
}
