using TMPro;
using UnityEngine;

public class NextArea : MonoBehaviour
{

    public TextMeshProUGUI leftText;
    public TextMeshProUGUI rightText;
    public RoadEnd leftEnd;
    public RoadEnd rightEnd;

    private int randomInt;


    void Start()
    {
        randomInt = Random.Range(1, 100);

        print("Random Node Roll: " + randomInt);

        print("C Node = " + StaticData.currentNode);
        print("C Section = " + StaticData.section);
    }

    void Update()
    {
        if (StaticData.section == 1)
        {
            ChooseLeft(StaticData.areas[StaticData.currentNode + StaticData.section]);
            leftText.SetText("< " + StaticData.areas[StaticData.currentNode + StaticData.section]);

            ChooseRight(StaticData.areas[StaticData.currentNode + StaticData.section + 1]);
            rightText.SetText(StaticData.areas[StaticData.currentNode + StaticData.section + 1] + " >");

        }
        else if (StaticData.section == 2)
        {
            ChooseLeft(StaticData.areas[StaticData.currentNode + StaticData.section]);
            leftText.SetText("< " + StaticData.areas[StaticData.currentNode + StaticData.section]);

            ChooseRight(StaticData.areas[StaticData.currentNode + StaticData.section + 1]);
            rightText.SetText(StaticData.areas[StaticData.currentNode + StaticData.section + 1] + " >");
        }
        else if (StaticData.section == 3)
        {
            ChooseLeft(StaticData.areas[StaticData.currentNode + StaticData.section]);
            leftText.SetText("< " + StaticData.areas[StaticData.currentNode + StaticData.section]);

            ChooseRight(StaticData.areas[StaticData.currentNode + StaticData.section + 1]);
            rightText.SetText(StaticData.areas[StaticData.currentNode + StaticData.section + 1] + " >");
        }
        else if (StaticData.section == 4)
        {

            ChooseLeft(StaticData.areas[StaticData.currentNode + StaticData.section]);
            leftText.SetText("< " + StaticData.areas[StaticData.currentNode + StaticData.section]);

            ChooseRight(StaticData.areas[StaticData.currentNode + StaticData.section + 1]);
            rightText.SetText(StaticData.areas[StaticData.currentNode + StaticData.section + 1] + " >");

        }
        else if (StaticData.section == 5)
        {

            leftText.SetText("< Boss");
            leftEnd.nextArea = RoadEnd.areas.boss;

            rightText.SetText("Boss >");
            rightEnd.nextArea = RoadEnd.areas.boss;
        }
        else if (StaticData.section == 6)
        {

            print("win!!!");

        }

    }


    void ChooseLeft(string next)
    {
        if (next.Equals("road"))
        {
            leftEnd.nextArea = RoadEnd.areas.nextStage;
        }
        else if (next.Equals("garage"))
        {
            leftEnd.nextArea = RoadEnd.areas.shop;
        }
        else
        {
            if (randomInt < 10)
            {
                leftEnd.nextArea = RoadEnd.areas.nextStage;
            }
            else if (randomInt >= 10 && randomInt < 20)
            {
                leftEnd.nextArea = RoadEnd.areas.shop;
            }
            else
            {
                leftEnd.nextArea = RoadEnd.areas.random;
            }
        }
    }

    void ChooseRight(string next)
    {
        if (next.Equals("road"))
        {
            rightEnd.nextArea = RoadEnd.areas.nextStage;
        }
        else if (next.Equals("garage"))
        {
            rightEnd.nextArea = RoadEnd.areas.shop;
        }
        else
        {

            if (randomInt < 10)
            {
                rightEnd.nextArea = RoadEnd.areas.nextStage;
            }
            else if (randomInt >= 10 && randomInt < 20)
            {
                rightEnd.nextArea = RoadEnd.areas.shop;
            }
            else
            {
                rightEnd.nextArea = RoadEnd.areas.random;
            }
        }
    }
}
