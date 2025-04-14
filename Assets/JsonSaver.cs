using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class JsonSaver : MonoBehaviour
{
    [System.Serializable]
    public class UserData
    {
        public string playerName;
        public int age;
    }

    public TMP_InputField nameInput;
    public TMP_InputField ageInput;
    public TMP_Text statusText;

    private string filePath;

    private void Start()
    {
        filePath = Application.persistentDataPath + "/userdata.json";
        statusText.text = "Ready";
    }

    public void SaveData()
    {
        UserData data = new UserData();
        data.playerName = nameInput.text;
        int.TryParse(ageInput.text, out data.age);

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);

        statusText.text = "Data Saved!";
        Debug.Log("Saved to: " + filePath);
    }

    public void LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            UserData data = JsonUtility.FromJson<UserData>(json);

            nameInput.text = data.playerName;
            ageInput.text = data.age.ToString();
            statusText.text = "Data Loaded!";
        }
        else
        {
            statusText.text = "No Save File Found!";
        }
    }

}
