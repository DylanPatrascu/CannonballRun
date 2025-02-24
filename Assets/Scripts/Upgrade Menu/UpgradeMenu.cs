using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

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
    public Image descriptionBox;
    public TMP_Text upgradeName;
    public TMP_Text upgradeDescription;
    public TMP_Text upgradeFlavour;
    public Image upgradeIcon;
    [Range(0, 2)] public float menuSwapTime;

    private Coroutine running;

    private void Start()
    {
        selectedUpgrades = SelectUpgrades();
        PopulateMenu();
        running = StartCoroutine(SlideMenu(menuSwapTime, 540, -540));
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
        StaticData.upgrades.Add(upgrade);
    }

    public void UpgradeSubmit(int button)
    {
        Debug.Log(button);
        for (int i = 0; i < selectedUpgrades.Count; i++) Debug.Log(selectedUpgrades[i]);
        Debug.Log(selectedUpgrades[button]);
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

        if (running != null) { StopCoroutine(running); }
        StartCoroutine(SwapDescription(menuSwapTime, 540, -540, selectedUpgrades[button]));

    }

    private IEnumerator SlideMenu(float time, int onScreen, int offScreen)
    {
        float t = 0;

        while (t <= time / 2)
        {
            descriptionBox.rectTransform.position = new Vector2(descriptionBox.rectTransform.position.x, Mathf.Lerp(offScreen, offScreen, t / (time / 2)));
            t += Time.deltaTime;
            yield return null;
        }

        descriptionBox.rectTransform.position = new Vector2(descriptionBox.rectTransform.position.x, 540);

    }

    private IEnumerator SwapDescription(float time, int onScreen, int offScreen, UpgradeData upgrade)
    {
        float t = 0;
        float halftime = time / 2;

        while (t < halftime)
        {
            descriptionBox.rectTransform.position = new Vector2(descriptionBox.rectTransform.position.x, Mathf.Lerp(onScreen, offScreen, t / halftime));
            t += Time.deltaTime;
            yield return null;
        }

        upgradeName.text = upgrade.upgradeName;
        upgradeDescription.text = upgrade.description;
        upgradeFlavour.text = upgrade.flavour;
        upgradeIcon.sprite = Sprite.Create(
                upgrade.icon,
                new Rect(0, 0, upgrade.icon.width, upgrade.icon.height),
                new Vector2(0.5f, 0.5f)
            );

        t = 0;

        while (t < halftime)
        {
            descriptionBox.rectTransform.position = new Vector2(descriptionBox.rectTransform.position.x, Mathf.Lerp(offScreen, onScreen, t / halftime));
            t += Time.deltaTime;
            yield return null;
        }

        descriptionBox.rectTransform.position = new Vector2(descriptionBox.rectTransform.position.x, onScreen);

    }

}
