using System.IO;
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

    [System.Serializable]
    public class UserData
    {
        public int pSpeed;
        public int pHealth;
        public int pSScrap;
        public int parts;
    }
    private string filePath;


    void Start()
    {
        filePath = Application.persistentDataPath + "/userdata.json";

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

            if(StaticData.scrap > StaticData.startingScrap)
            {
                StaticData.parts += (StaticData.scrap - StaticData.startingScrap) / 10;
                SaveData();
            }

        }
    }

    public void SaveData()
    {
        UserData data = new UserData();
        int.TryParse(StaticData.startingScrap.ToString(), out data.pSScrap);
        int.TryParse(StaticData.speedIncrease.ToString(), out data.pSpeed);
        int.TryParse(StaticData.healthIncrease.ToString(), out data.pHealth);
        int.TryParse(StaticData.parts.ToString(), out data.parts);

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);

    }
}
