using TMPro;
using UnityEngine;

public class NextArea : MonoBehaviour
{

    int section = 1;
    public TextMeshProUGUI leftText;
    public TextMeshProUGUI rightText;


    void Start()
    {
        
    }

    void Update()
    {
        if (section == 1)
        {
            leftText.SetText("< " + StaticData.areas[0]);
            rightText.SetText(StaticData.areas[1] + " >");
        }
    }
}
