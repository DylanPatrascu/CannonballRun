using UnityEngine;

public class RunScript : MonoBehaviour
{
    int RandomInt = 0;
    int x = 1;
    int y = 1;
    int[] intArray1 = { 1 };
    int[] intArray2 = { 1, 2 };
    int[] intArray3 = { 1, 2, 3 };
    int[] intArray4 = { 1, 2, 3, 4 };
    int[] intArray5 = { 1, 2, 3, 4, 5 };

    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            y = x;

            while (y > 0)
            {
                RandomInt = Random.Range(1, 100);
                print(RandomInt);

                if (x == 1)
                {
                    intArray1[0] = 0;
                }
                if (x == 2)
                {
                    intArray2[y-1] = spotGenerator(RandomInt);
                }
                if (x == 3)
                {
                    intArray3[y - 1] = spotGenerator(RandomInt);
                }
                if (x == 4)
                {
                    intArray4[y - 1] = spotGenerator(RandomInt);
                }
                if (x == 5)
                {
                    intArray5[y - 1] = spotGenerator(RandomInt);
                }

                print("num: " + y);

                y--;
            }

            x++;
        }
    }

    int spotGenerator(int random)
    {
        if (random <= 60)
        {
            print("spot: " + 0);
            return 0;
        }
        else if (random >= 80)
        {
            print("spot: " + 1);
            return 1;
        }
        else
        {
            print("spot: " + 2);
            return 2;
        }
    }

    void Update()
    {
        
    }
}
