using UnityEngine;

[CreateAssetMenu]
public class UpgradeData : ScriptableObject
{
    public enum types {
        speed,
        handling,
        weapon,
        tech
    };

    public string upgradeName;
    public string description;
    public string flavour;
    public int value;
    public types type;
    public Texture2D icon;
}
