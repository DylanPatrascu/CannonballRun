using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEngine;

public class TreeGenerator : MonoBehaviour
{
    int RandomInt = 0;
    int x = 1;
    int y = 1;

    public List<string> areas = StaticData.areas;

    void Start()
    {

        for (int i = 0; i < 5; i++)
        {
            y = x;

            while (y > 0)
            {
                RandomInt = Random.Range(1, 100);

                if (x == 1)
                {
                    StaticData.areas.Add("road");
                }
                if (x == 2)
                {
                    StaticData.areas.Add(spotGenerator(RandomInt));
                }
                if (x == 3)
                {
                    StaticData.areas.Add(spotGenerator(RandomInt));
                }
                if (x == 4)
                {
                    StaticData.areas.Add(spotGenerator(RandomInt));
                }
                if (x == 5)
                {
                    StaticData.areas.Add(spotGenerator(RandomInt));
                }

                y--;
            }

            x++;
        }
    }

    string spotGenerator(int random)
    {
        if (random <= 60)
        {
            return "road";
        }
        else if (random >= 80)
        {
            return "garage";
        }
        else
        {
            return "random";
        }
    }

    void Update()
    {

    }
}
