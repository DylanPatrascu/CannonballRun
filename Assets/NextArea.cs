using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;

public class NextArea : MonoBehaviour
{

    int section = 1;
    public TextMeshProUGUI leftText;
    public TextMeshProUGUI rightText;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (section == 1)
        {

            leftText.SetText("< " + StaticData.areas[1]);
            rightText.SetText(StaticData.areas[2] + " >");
        }
    }
}
