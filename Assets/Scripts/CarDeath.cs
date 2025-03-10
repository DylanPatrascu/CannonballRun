using UnityEngine;
using UnityEngine.UI;

public class CarDeath : MonoBehaviour
{

    public Transform location;
    bool dead = false;

    public Canvas deathScreen;
    public Image blackScreen;
    private Color alpha;
    private Color alpha2;
    private float time = 0;
    public float transitionTime = 5;

    public ParticleSystem explode;


    void Start()
    {
        deathScreen.enabled = false;
        alpha = blackScreen.color;
        alpha2 = alpha;
        alpha2.a = 0;
        explode.Pause();

    }

    // Update is called once per frame
    void Update()
    {

        

        if (location.position.y < -5)
        {
            if (!dead)
            {
                dead = true;
                print("dead");
                explode.Play();
            }
            else
            {
                deathScreen.enabled = true;
                blackScreen.color = Color.Lerp(alpha2,alpha,time/transitionTime);
                time += Time.deltaTime;
            }
        }
    }
}
