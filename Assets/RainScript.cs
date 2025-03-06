using UnityEngine;

public class RainScript : MonoBehaviour
{
    public ParticleSystem RainObj;
    int RandomInt = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RandomInt = Random.Range(0, 20);

        print(RandomInt + "\n");

        if (RandomInt < 10)
        {
            RainObj.Pause();
        }
        else
        {
            RainObj.Play();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
