using System.Collections.Generic;
using UnityEngine;

public class StaticData : MonoBehaviour
{
    public static string nextScene = "GameScene";
    public static List<UpgradeData> upgrades = new List<UpgradeData>();
    public static List<string> areas = new List<string>();

    public static float totalTime = 0;
    public static int distanceTraveled = 0;
    public static float timeDrifted = 0;
    public static int scrap = 0;
    public static int enemiesKilled = 0;
    public static int section = 1;
    public static int currentNode = 0;
    public static float runTime = 0;
    public static int speedIncrease = 0;
    public static int healthIncrease = 0;
    public static int startingScrap = 0;
    public static int parts = 200;

    public static bool treeGen = false;
    public static bool alive = true;



}
