using TMPro;
using UnityEngine;

public class NextArea : MonoBehaviour
{

    public TextMeshProUGUI leftText;
    public TextMeshProUGUI rightText;
    public RoadEnd leftEnd;
    public RoadEnd rightEnd;


    void Start()
    {
        if (StaticData.section == 1)
        {
            print("C Node = " + StaticData.currentNode);
            print("C Section = " + StaticData.section);
            ChooseLeft(StaticData.areas[StaticData.currentNode + StaticData.section]);
            leftText.SetText("< " + StaticData.areas[StaticData.currentNode + StaticData.section]);

            ChooseRight(StaticData.areas[StaticData.currentNode + StaticData.section + 1]);
            rightText.SetText(StaticData.areas[StaticData.currentNode + StaticData.section + 1] + " >");

            StaticData.section = 2;

        }
        else if (StaticData.section == 2)
        {

            print("C Node = " + StaticData.currentNode);
            print("C Section = " + StaticData.section);

            ChooseLeft(StaticData.areas[StaticData.currentNode + StaticData.section]);
            leftText.SetText("< " + StaticData.areas[StaticData.currentNode + StaticData.section]);

            ChooseRight(StaticData.areas[StaticData.currentNode + StaticData.section + 1]);
            rightText.SetText(StaticData.areas[StaticData.currentNode + StaticData.section + 1] + " >");

            StaticData.section = 3;

        }
        else if (StaticData.section == 3)
        {

            ChooseLeft(StaticData.areas[StaticData.currentNode + StaticData.section]);
            leftText.SetText("< " + StaticData.areas[StaticData.currentNode + StaticData.section]);

            ChooseRight(StaticData.areas[StaticData.currentNode + StaticData.section + 1]);
            rightText.SetText(StaticData.areas[StaticData.currentNode + StaticData.section + 1] + " >");

            StaticData.section = 4;

        }
        else if (StaticData.section == 4)
        {

            ChooseLeft(StaticData.areas[StaticData.currentNode + StaticData.section]);
            leftText.SetText("< " + StaticData.areas[StaticData.currentNode + StaticData.section]);

            ChooseRight(StaticData.areas[StaticData.currentNode + StaticData.section + 1]);
            rightText.SetText(StaticData.areas[StaticData.currentNode + StaticData.section + 1] + " >");

            StaticData.section = 5;

        }
        else if (StaticData.section == 5)
        {

            ChooseLeft(StaticData.areas[StaticData.currentNode + StaticData.section]);
            leftText.SetText("< " + StaticData.areas[StaticData.currentNode + StaticData.section]);

            ChooseRight(StaticData.areas[StaticData.currentNode + StaticData.section + 1]);
            rightText.SetText(StaticData.areas[StaticData.currentNode + StaticData.section + 1] + " >");

            StaticData.section = 6;

        }
        else if (StaticData.section == 6)
        {

            print("win!!!");

        }
    }

    void Update()
    {
      

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
            leftEnd.nextArea = RoadEnd.areas.nextStage;
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
            rightEnd.nextArea = RoadEnd.areas.nextStage;
        }
    }
}
