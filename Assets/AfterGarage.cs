using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AfterGarage : MonoBehaviour
{

    public TextMeshProUGUI leftText;
    public TextMeshProUGUI rightText;

    private int randomInt;

    void Start()
    {

        randomInt = Random.Range(1, 100);

        print("Random Node Roll: " + randomInt);

        if (StaticData.section != 5)
        {
            leftText.SetText(StaticData.areas[StaticData.currentNode + StaticData.section]);
            rightText.SetText(StaticData.areas[StaticData.currentNode + StaticData.section + 1]);
        }
        else if (StaticData.section == 5)
        {
            leftText.SetText("Boss");
            rightText.SetText("Boss");
        }

    }


    public void goLeft()
    {

        if (StaticData.section == 5)
        {
            SceneManager.LoadScene("BossScene");
        }

        if (StaticData.areas[StaticData.currentNode + StaticData.section].Equals("road"))
        {
            StaticData.currentNode = StaticData.currentNode + StaticData.section;
            StaticData.section += 1;
            SceneManager.LoadScene("GameScene");
        }
        else if (StaticData.areas[StaticData.currentNode + StaticData.section].Equals("garage"))
        {
            StaticData.currentNode = StaticData.currentNode + StaticData.section;
            StaticData.section += 1;
            SceneManager.LoadScene("Garage");
        }
        else if (StaticData.areas[StaticData.currentNode + StaticData.section].Equals("random"))
        {
            StaticData.currentNode = StaticData.currentNode + StaticData.section;
            StaticData.section += 1;
            if (randomInt < 10)
            {
                SceneManager.LoadScene("GameScene");
            }
            else if (randomInt >= 10 && randomInt < 20)
            {
                SceneManager.LoadScene("Garage");
            }
            else
            {
                SceneManager.LoadScene("RandomScene");
            }
        }
    }
    public void goRight()
    {
        if (StaticData.section == 5)
        {
            SceneManager.LoadScene("BossScene");
        }

        if (StaticData.areas[StaticData.currentNode + StaticData.section +1 ].Equals("road"))
        {
            StaticData.currentNode = StaticData.currentNode + StaticData.section + 1;
            StaticData.section += 1;
            SceneManager.LoadScene("GameScene");
        }
        else if (StaticData.areas[StaticData.currentNode + StaticData.section + 1].Equals("garage"))
        {
            StaticData.currentNode = StaticData.currentNode + StaticData.section + 1;
            StaticData.section += 1;
            SceneManager.LoadScene("Garage");
        }
        else if (StaticData.areas[StaticData.currentNode + StaticData.section + 1].Equals("random"))
        {
            StaticData.currentNode = StaticData.currentNode + StaticData.section + 1;
            StaticData.section += 1;

            if (randomInt < 10)
            {
                SceneManager.LoadScene("GameScene");
            }
            else if (randomInt >= 10 && randomInt < 20)
            {
                SceneManager.LoadScene("Garage");
            }
            else
            {
                SceneManager.LoadScene("RandomScene");
            }
        }
    }

}
