using System.Collections.Generic;
using NUnit.Framework;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UpgradeMenu : MonoBehaviour
{
    public CinemachineCamera mainCam;
    public CinemachineCamera engineCam;
    public CinemachineCamera handlingCam;
    public CinemachineCamera weaponCam;
    public CinemachineCamera techCam;

    public List<UpgradeData> upgradeList;
    private List<UpgradeData> selectedUpgrades;

    public List<Button> buttons;

    private void Start()
    {
        selectedUpgrades = SelectUpgrades();
        PopulateMenu();
    }

    private List<UpgradeData> SelectUpgrades()
    {
        HashSet<int> indexList = new HashSet<int>();
        List<UpgradeData> selectedUpgrades = new List<UpgradeData>();

        while (indexList.Count < 3) { indexList.Add(Random.Range(0, upgradeList.Count)); }
        foreach (int index in indexList) { selectedUpgrades.Add(upgradeList[index]); }
        return selectedUpgrades;
    }

    private void PopulateMenu()
    {
        for (int i = 0; i < selectedUpgrades.Count; i++)
        {
            buttons[i].gameObject.GetComponentInChildren<Image>().sprite = Sprite.Create(
                selectedUpgrades[i].icon,
                new Rect(0, 0, selectedUpgrades[i].icon.width, selectedUpgrades[i].icon.height),
                new Vector2(0.5f, 0.5f)
            );
            buttons[i].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = selectedUpgrades[i].upgradeName;
        }
    }

    private void AddUpgrade(UpgradeData upgrade)
    {
        return;
    }

    public void UpgradeSubmit(int button)
    {
        AddUpgrade(selectedUpgrades[button]);
        SceneManager.LoadScene("GameScene");
    }

    public void ButtonSelected(int button)
    {
        mainCam.gameObject.SetActive(false);
        engineCam.gameObject.SetActive(false);
        handlingCam.gameObject.SetActive(false);
        weaponCam.gameObject.SetActive(false);
        techCam.gameObject.SetActive(false);

        if (selectedUpgrades[button].type == UpgradeData.types.handling) { handlingCam.gameObject.SetActive(true); }
        else if (selectedUpgrades[button].type == UpgradeData.types.tech) { techCam.gameObject.SetActive(true); }
        else if (selectedUpgrades[button].type == UpgradeData.types.weapon) { weaponCam.gameObject.SetActive(true); }
        else if (selectedUpgrades[button].type == UpgradeData.types.speed) { engineCam.gameObject.SetActive(true); }
    }

}
