using UnityEngine;
using UnityEngine.SceneManagement;

public class RoadEnd : MonoBehaviour
{

    public enum areas
    {
        shop,
        godShop,
        gasStation,
        nextStage,
        boss
    };

    public GameObject player;
    public areas nextArea = areas.nextStage;

    public bool dir;

    public string shopScene = "Garage";
    public string godShopScene = "Garage";
    public string gasStationScene = "Garage";
    public string nextStageScene = "GameScene";
    public string bossScene = "BossScene";

    public void OnTriggerEnter(Collider other)
    {

        // false = right, true = left
        if (dir)
        {
            StaticData.currentNode = StaticData.currentNode + StaticData.section;
        }
        else
        {
            StaticData.currentNode = StaticData.currentNode + StaticData.section + 1;
        }




        if(other.gameObject == player)
        {
            if (nextArea == areas.shop) { StaticData.nextScene = shopScene; }
            else if (nextArea == areas.godShop) { StaticData.nextScene = godShopScene; }
            else if (nextArea == areas.gasStation) { StaticData.nextScene = gasStationScene; }
            else if (nextArea == areas.nextStage) { StaticData.nextScene = nextStageScene; }
            else if (nextArea == areas.boss) { StaticData.nextScene = bossScene; }

            StaticData.section += 1;

            UpdateScene("RoadEndScene");
        }
    }

    public void UpdateScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
