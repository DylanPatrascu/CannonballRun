using System.Runtime.CompilerServices;
using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class HUDController : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private GameObject player;
    [SerializeField] private Rigidbody carRB;
    [SerializeField] private UnityEngine.UI.Image[] speedbar;
    [SerializeField] private TMP_Text speedText;
    private const float MPS_TO_KMH = 3.6f;

    private float playerSpeed;
    private CarController car;
    private float displayedBarCount = 0f;

    private PlayerHealth playerHealth;
    [SerializeField] private UnityEngine.UI.Image[] healthBar;
    private float displayedHealthCount = 0f;


    [Header("Gun")]
    [SerializeField] private UnityEngine.UI.Image bullet;
    [SerializeField] private float moveDistance = 10f;
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private TMP_Text currentAmmo;
    [SerializeField] private TMP_Text maxAmmo;
    [SerializeField] private Turret turret1;
    [SerializeField] private Turret2 turret3;
    [SerializeField] private Turret3 turret2;




    private void Start()
    {
        carRB = player.GetComponent<Rigidbody>();
        car = player.GetComponent<CarController>();
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        playerSpeed = carRB.linearVelocity.magnitude * MPS_TO_KMH;
        UpdateSpeed();
        UpdateAmmo();
        UpdateHealth();
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

    private void UpdateHealth()
    {
        float healthRatio = Mathf.Clamp01(playerHealth.GetHealth() / playerHealth.GetMaxHealth());
        float targetHealthCount = healthRatio * healthBar.Length;
        displayedHealthCount = Mathf.Lerp(displayedHealthCount, targetHealthCount, Time.deltaTime * 10f);

        for (int i = 0; i < healthBar.Length; i++)
        {
            healthBar[i].enabled = i < displayedHealthCount;
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
            currentAmmo.text = turret3.GetCurrentAmmo().ToString();
            maxAmmo.text = turret3.GetMaxAmmo().ToString();
        }
    }

}