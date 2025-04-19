using System.IO;
using UnityEngine;

public class MainMenuLoad : MonoBehaviour
{
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

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            UserData data = JsonUtility.FromJson<UserData>(json);

            StaticData.startingScrap = data.pSScrap;
            StaticData.speedIncrease = data.pSpeed;
            StaticData.healthIncrease = data.pHealth;
            StaticData.parts = data.parts;

            print("Game Data Loaded!");
        }
        else
        {
            print("No Save File Found!");
        }
    }
}
