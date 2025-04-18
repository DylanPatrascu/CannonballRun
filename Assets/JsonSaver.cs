using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;

public class JsonSaver : MonoBehaviour
{
    [System.Serializable]
    public class UserData
    {
        public int pSpeed;
        public int pHealth;
        public int pSScrap;
        public int parts;
    }

    public TMP_Text statusText;
    public TMP_Text speed;
    public TMP_Text parts;
    public TMP_Text health;
    public TMP_Text startingScrap;
    
    private string filePath;

    private void Start()
    {
        filePath = Application.persistentDataPath + "/userdata.json";
        statusText.text = "Ready";
        LoadData();
    }
    private void Update()
    {
        parts.text = "Parts \n" + StaticData.parts.ToString();
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

        statusText.text = "Game Data Saved!";
    }
    
    // Inc and Dec Speed for buttons
    public void increaseSpeed()
    {
        if (StaticData.parts >= 20)
        {
            StaticData.speedIncrease += 1;
            StaticData.parts -= 20;
        }
        speed.text = StaticData.speedIncrease.ToString();
    }
    public void decreaseSpeed()
    {
        if (StaticData.speedIncrease > 0)
        {
            StaticData.speedIncrease -= 1;
            StaticData.parts += 20;
        }
        speed.text = StaticData.speedIncrease.ToString();
    }


    // Inc and Dec health for buttons
    public void increaseHealth()
    {
        if (StaticData.parts >= 20)
        {
            StaticData.healthIncrease += 1;
            StaticData.parts -= 20;
        }
        health.text = StaticData.healthIncrease.ToString();
    }
    public void decreaseHealth()
    {
        if (StaticData.healthIncrease > 0)
        {
            StaticData.healthIncrease -= 1;
            StaticData.parts += 20;
        }
        health.text = StaticData.healthIncrease.ToString();
    }

    // Inc and Dec start scrap for buttons
    public void increaseScrap()
    {
        if (StaticData.parts >= 20)
        {
            StaticData.startingScrap += 1;
            StaticData.parts -= 20;
        }
        startingScrap.text = StaticData.startingScrap.ToString();
    }
    public void decreaseScrap()
    {
        if (StaticData.startingScrap > 0)
        {
            StaticData.startingScrap -= 1;
            StaticData.parts += 20;
        }
        startingScrap.text = StaticData.startingScrap.ToString();
    }







    public void LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            UserData data = JsonUtility.FromJson<UserData>(json);

            parts.text = data.pSScrap.ToString();
            speed.text = data.pSpeed.ToString();
            health.text = data.pHealth.ToString();
            parts.text = "Parts \n" + data.parts.ToString();

            statusText.text = "Game Data Loaded!";
        }
        else
        {
            statusText.text = "No Save File Found!";
        }
    }

}
