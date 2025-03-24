using UnityEngine;
using UnityEngine.UI;

public class CarDeath : MonoBehaviour
{

    public Transform location;
    public bool dead = false;

    public Canvas deathScreen;
    public Image blackScreen;
    private Color alpha;
    private Color alpha2;
    private float time = 0;
    private float transitionTime = 2;

    public ParticleSystem explode;


    void Start()
    {
        deathScreen.enabled = false;
        alpha = blackScreen.color;
        alpha2 = alpha;
        alpha2.a = 0;
        explode.Pause();

    }

    void Update()
    {
        if (location.position.y < -5)
        {
            TriggerDeath();
        }
        if (dead)
        {
            deathScreen.enabled = true;
            blackScreen.color = Color.Lerp(alpha2, alpha, time / transitionTime);
            time += Time.deltaTime;
        }
    }

    public void TriggerDeath()
    {
        if (!dead)
        {
            dead = true;
            StaticData.alive = false;
            Debug.Log("Car is dead!");
            explode.Play();
        }
    }
}
