using System.Collections;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class chatScript : MonoBehaviour
{
    [System.Serializable]
    public class UserData
    {
        public int pSpeed;
        public int pHealth;
        public int pSScrap;
        public int parts;
    }
    public GameObject head;
    public AudioSource voice;

    public TMP_Text chat;
    public TMP_Text button1;
    public TMP_Text button2;
    public TMP_Text scrap;
    public TMP_Text parts;

    [SerializeField] private Transition transition;

    private Vector3 pos1;
    private Vector3 pos2;
    private Vector3 add = new Vector3(0, 30, 0);
    private int step = 0;

    private string filePath;

    void Start()
    {
        filePath = Application.persistentDataPath + "/userdata.json";

        pos1 = head.transform.position;
        pos2 = pos1 + add;

        button1.SetText("Who are you?");
        button2.SetText("Why?");
        voice.Play(); 
        StartCoroutine(animate());
        chat.SetText("You gotta watch out for the police drones out there. But every one knows the scrap you can get from them is worth the risk!!");
    }

    private IEnumerator animate()
    {

        for (int i = 0; i < 4; i++)
        {
            float t = 0;
            float timeElapsed = 0;

            while (t < 1)
            {
                t = timeElapsed * 4;

                head.transform.position = Vector3.Lerp(pos1, pos2, t);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            t = 0;
            timeElapsed = 0;
            while (t < 1)
            {
                t = timeElapsed * 4;

                head.transform.position = Vector3.Lerp(pos2, pos1, t);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
    }

    private void Update()
    {
        scrap.SetText(StaticData.scrap.ToString());
        parts.SetText(StaticData.parts.ToString());
    }
    public void click()
    {
        if (step == 0)
        {
            voice.Play();
            StartCoroutine(animate());
            chat.SetText("Im a garage bot I can turn some of your scrap into parts. Parts let you tune your car when you get off these roads. \n\nWant to give me some scrap?");

            button1.SetText("Yes");
            button2.SetText("No");

        }
        if (step == 1) 
        {
            voice.Play();
            StartCoroutine(animate());
            chat.SetText("Thanks! Here is some car parts!");

            button1.SetText("Leave");
            button2.SetText("Leave");

            StaticData.parts += StaticData.scrap;
            StaticData.scrap = 0;

            SaveData();
        }
        if (step == 2)
        {
            transition.TransitionScene("AfterGarage");
        }

        step++;
    }

    public void click2()
    {
        if (step == 0)
        {
            voice.Play();
            StartCoroutine(animate());
            chat.SetText("The government put those drones out to watch for modded vehicles like yours. Don't trust anyone outside these garages they have eyes everywhere. \n\nWant to give me some scrap to turn into parts?");

            button1.SetText("Yes");
            button2.SetText("No");

        }
        if (step == 1)
        {
            voice.Play();
            StartCoroutine(animate());
            chat.SetText("Maybe next time then.");

            button1.SetText("Leave");
            button2.SetText("Leave");
        }
        if (step == 2)
        {
            transition.TransitionScene("AfterGarage");
        }

        step++;
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
