using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private GameObject player;
    [SerializeField] private Rigidbody carRB;
    [SerializeField] private Image[] speedbar;
    [SerializeField] private TMP_Text speedText;
    private const float MPS_TO_KMH = 3.6f;

    private float playerSpeed;
    private CarController car;
    private float displayedBarCount  = 0f;

    [Header("Gun")]
    [SerializeField] private TMP_Text currentAmmo;
    [SerializeField] private TMP_Text maxAmmo;
    [SerializeField] private Turret turret1;
    [SerializeField] private Turret2 turret3;
    [SerializeField] private Turret3 turret2;




    private void Start()
    {
        carRB = player.GetComponent<Rigidbody>();
        car = player.GetComponent<CarController>();
    }

    private void Update()
    {
        playerSpeed = carRB.linearVelocity.magnitude * MPS_TO_KMH;
        UpdateSpeed();
        UpdateAmmo();
    }

    private void UpdateSpeed()
    {
        
        speedText.text = playerSpeed.ToString("000");
        float speedRatio = Mathf.Clamp01(playerSpeed / (car.maxSpeed * MPS_TO_KMH));
        float targetBarCount = speedRatio * speedbar.Length;
        displayedBarCount  = Mathf.Lerp(displayedBarCount , targetBarCount, Time.deltaTime * 10f);

        for(int i = 0; i < speedbar.Length; i++)
        {
            speedbar[i].enabled = i < displayedBarCount ;
        }
    }

    private void UpdateAmmo()
    {
        if(turret1.isActiveAndEnabled)
        {
            currentAmmo.text = turret1.GetCurrentAmmo().ToString();
            maxAmmo.text = turret1.GetMaxAmmo().ToString();
        }
        else if (turret2.isActiveAndEnabled)
        {
            currentAmmo.text = turret2.GetCurrentAmmo().ToString();
            maxAmmo.text = turret2.GetMaxAmmo().ToString();
        }
        else if(turret3.isActiveAndEnabled)
        {
            currentAmmo.text = turret2.GetCurrentAmmo().ToString();
            maxAmmo.text = turret2.GetMaxAmmo().ToString();
        }
    }


}
