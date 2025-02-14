using UnityEngine;
using UnityEngine.SceneManagement;

public class RoadEnd : MonoBehaviour
{

    public enum areas
    {
        shop,
        godShop,
        gasStation,
        nextStage
    };

    public GameObject player;
    public areas nextArea = areas.nextStage;

    public string shopScene = "Garage";
    public string godShopScene = "Garage";
    public string gasStationScene = "Garage";
    public string nextStageScene = "GameScene";

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            if (nextArea == areas.shop) { StaticData.nextScene = shopScene; }
            else if (nextArea == areas.godShop) { StaticData.nextScene = godShopScene; }
            else if (nextArea == areas.gasStation) { StaticData.nextScene = gasStationScene; }
            else if (nextArea == areas.nextStage) { StaticData.nextScene = nextStageScene; }

            UpdateScene("RoadEndScene");
        }
    }

    public void UpdateScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
