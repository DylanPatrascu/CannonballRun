using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class TreeLogic : MonoBehaviour
{

    public List<Image> nodes = new List<Image>();
    public Sprite road;
    public Sprite garage;
    public Sprite qmark;

    void Start()
    {
            
    }

    void Update()
    {
        nodes[StaticData.currentNode].color = Color.red;

        for (int i = 0; i < nodes.Count; i++)
        {

            if (StaticData.areas[i].Equals("garage"))
            {
                nodes[i].sprite = garage;
            }
            else if (StaticData.areas[i].Equals("road"))
            {
                nodes[i].sprite = road;
            }
            else
            {
                nodes[i].sprite = qmark;
            }
        }

    }
}
