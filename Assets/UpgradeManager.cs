using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] GameObject cowcatcher1;
    [SerializeField] GameObject cowcatcher2;
    [SerializeField] GameObject cowcatcher3;

    [SerializeField] GameObject turret1;
    [SerializeField] GameObject turret2;
    [SerializeField] GameObject turret3;

    [SerializeField] GameObject sandevistan;

    [SerializeField] CarController car;

    void Start()
    {

        foreach (UpgradeData upgrade in StaticData.upgrades)
        {
            if (upgrade.upgradeName == "Cowcatcher" && !cowcatcher2.activeSelf && !cowcatcher3.activeSelf)
            {
                cowcatcher1.SetActive(true);
            }
            if (upgrade.upgradeName == "Cowcatcherer" && !cowcatcher3.activeSelf)
            {
                cowcatcher1.SetActive(false);
                cowcatcher2.SetActive(true);
            }
            if (upgrade.upgradeName == "Cowcatcherest")
            {
                cowcatcher1.SetActive(false);
                cowcatcher2.SetActive(false);
                cowcatcher3.SetActive(true);
            }

            if (upgrade.upgradeName == "Cool Gun" && !turret2.activeSelf && !turret3.activeSelf)
            {
                turret1.SetActive(true);
            }
            if (upgrade.upgradeName == "Cooler Gun" && !turret3.activeSelf)
            {
                turret1.SetActive(false);
                turret2.SetActive(true);
            }
            if (upgrade.upgradeName == "Coolest Gun")
            {
                turret1.SetActive(false);
                turret2.SetActive(false);
                turret3.SetActive(true);
            }

            if (upgrade.upgradeName == "Sandevistan")
            {
                sandevistan.SetActive(true);
            }

            if (upgrade.upgradeName == "Engine")
            {
                car.acceleration += 20;
                car.maxSpeed += 20;
                car.springStiffness += 20000;
            }


            if (upgrade.upgradeName == "Tires")
            {
                car.deceleration += 20;
                car.steerStrength += 5;
            }

        }
    }
}
