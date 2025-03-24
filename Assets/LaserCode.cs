using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class LaserCode : MonoBehaviour
{

    public GameObject LeftLaser;
    public GameObject RightLaser;
    public GameObject MiddleLaser;

    public LaserProperties left;
    public LaserProperties right;
    public LaserProperties middle;

    int RandomInt = 0;

    int stage = 1;

    void Start()
    {
        LeftLaser.SetActive(false);
        RightLaser.SetActive(false);
        MiddleLaser.SetActive(false);

        StartCoroutine(clock());
    }


    private IEnumerator clock()
    {
        while (true)
        {

            // Blink the laser 
            if (stage == 1)
            {
                RandomInt = Random.Range(1, 100);
                if (RandomInt <= 33)
                {
                    LeftLaser.SetActive(true);
                }
                else if (RandomInt >= 66)
                {
                    MiddleLaser.SetActive(true);
                }
                else
                {
                    RightLaser.SetActive(true);
                }
            }
            else if (stage == 2)
            {
                LeftLaser.SetActive(false);
                RightLaser.SetActive(false);
                MiddleLaser.SetActive(false);
            }
            else if (stage == 3)
            {
                if (RandomInt <= 33)
                {
                    LeftLaser.SetActive(true);
                }
                else if (RandomInt >= 66)
                {
                    MiddleLaser.SetActive(true);
                }
                else
                {
                    RightLaser.SetActive(true);
                }
            }
            else if (stage == 4)
            {
                LeftLaser.SetActive(false);
                RightLaser.SetActive(false);
                MiddleLaser.SetActive(false);
            }
            else if (stage == 5) // turn laser on now
            {
                if (RandomInt <= 33)
                {
                    LeftLaser.SetActive(true);
                    left.dangerous = true;
                }
                else if (RandomInt >= 66)
                {
                    MiddleLaser.SetActive(true);
                    middle.dangerous = true;
                }
                else
                {
                    RightLaser.SetActive(true);
                    right.dangerous = true;
                }
            }
            else if (stage == 10)
            {
                if (RandomInt <= 33)
                {
                    LeftLaser.SetActive(false);
                    left.dangerous = false;
                }
                else if (RandomInt >= 66)
                {
                    MiddleLaser.SetActive(false);
                    middle.dangerous = false;
                }
                else
                {
                    RightLaser.SetActive(false);
                    right.dangerous = false;
                }
                stage = 0;
            }

            stage += 1;

            yield return new WaitForSeconds(0.5f);
        }
    }
}
