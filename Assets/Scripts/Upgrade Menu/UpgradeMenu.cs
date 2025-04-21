using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class UpgradeMenu : MonoBehaviour
{
    [Header("Cameras")]
    public CinemachineCamera mainCam;
    public CinemachineCamera engineCam;
    public CinemachineCamera handlingCam;
    public CinemachineCamera gunCam;
    public CinemachineCamera cowCam;
    public CinemachineCamera techCam;

    [Header("Upgrades")]
    public List<UpgradeData> upgradeList;
    private List<UpgradeData> selectedUpgrades;
    public GameObject turret1;
    public GameObject turret2;
    public GameObject turret3;
    public GameObject cow1;
    public GameObject cow2;
    public GameObject cow3;
    public GameObject sandevistan;

    [Header("UI Elements")]
    public List<Button> buttons;
    public Image descriptionBox;
    public TMP_Text scrapText;
    public TMP_Text upgradeName;
    public TMP_Text upgradeDescription;
    public TMP_Text upgradeFlavour;
    public TMP_Text upgradeCost;
    public Image upgradeIcon;
    [Range(0, 2)] public float menuSwapTime;

    [SerializeField] private Transition transition;

    private enum Direction
    {
        up, down
    };

    private Coroutine running;

    private void Start()
    {
        scrapText.text = StaticData.scrap.ToString();
        selectedUpgrades = SelectUpgrades();
        PopulateMenu();
        running = StartCoroutine(SlideMenu(menuSwapTime, 540, -540));
    }

    private List<UpgradeData> SelectUpgrades()
    {
        HashSet<int> indexList = new HashSet<int>();
        List<UpgradeData> selectedUpgrades = new List<UpgradeData>();

        while (indexList.Count < 3) 
        {
            int index = Random.Range(0, upgradeList.Count);
            if (!StaticData.upgrades.Contains(upgradeList[index])) indexList.Add(index);
        }
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
        if (StaticData.scrap - selectedUpgrades[button].cost >= 0)
        {
            StaticData.scrap -= selectedUpgrades[button].cost;
            AddUpgrade(selectedUpgrades[button]);
            transition.TransitionScene("AfterGarage");
        }
        
    }

    public void SkipShop()
    {
        transition.TransitionScene("AfterGarage");
    }

    public void ButtonSelected(int button)
    {

        mainCam.gameObject.SetActive(false);
        engineCam.gameObject.SetActive(false);
        handlingCam.gameObject.SetActive(false);
        gunCam.gameObject.SetActive(false);
        cowCam.gameObject.SetActive(false);
        techCam.gameObject.SetActive(false);

        if (selectedUpgrades[button].type == UpgradeData.types.handling) { handlingCam.gameObject.SetActive(true); }
        else if (selectedUpgrades[button].type == UpgradeData.types.tech) { techCam.gameObject.SetActive(true); }
        else if (selectedUpgrades[button].type == UpgradeData.types.gun) { gunCam.gameObject.SetActive(true); }
        else if (selectedUpgrades[button].type == UpgradeData.types.cowCatcher) { cowCam.gameObject.SetActive(true); }
        else if (selectedUpgrades[button].type == UpgradeData.types.speed) { engineCam.gameObject.SetActive(true); }

        turret1.SetActive(false);
        turret2.SetActive(false);
        turret3.SetActive(false);
        cow1.SetActive(false);
        cow2.SetActive(false);
        cow3.SetActive(false);
        sandevistan.SetActive(false);

        if (selectedUpgrades[button].upgradeName == "Cool Gun") { turret1.SetActive(true); }
        else if (selectedUpgrades[button].upgradeName == "Cooler Gun") { turret2.SetActive(true); }
        else if (selectedUpgrades[button].upgradeName == "Coolest Gun") { turret3.SetActive(true); }
        else if (selectedUpgrades[button].upgradeName == "Cowcatcher") { cow1.SetActive(true); }
        else if (selectedUpgrades[button].upgradeName == "Cowcatcherer") { cow2.SetActive(true); }
        else if (selectedUpgrades[button].upgradeName == "Cowcatcherest") { cow3.SetActive(true); }
        else if (selectedUpgrades[button].upgradeName == "Sandevistan") { sandevistan.SetActive(true); }

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
        upgradeCost.text = upgrade.cost.ToString();
        upgradeIcon.sprite = Sprite.Create(
                upgrade.icon,
                new Rect(0, 0, upgrade.icon.width, upgrade.icon.height),
                new Vector2(0.5f, 0.5f)
            );

        t = 0;

        while (t < halftime)
        {
            descriptionBox.rectTransform.position = new Vector2(
                descriptionBox.rectTransform.position.x, 
                Mathf.Lerp(offScreen, onScreen, t / halftime)
            );
            t += Time.deltaTime;
            yield return null;
        }

        descriptionBox.rectTransform.position = new Vector2(descriptionBox.rectTransform.position.x, onScreen);

    }

    private IEnumerator SwapSelected(int duration, Direction d)
    {

        int dirModifier = 1;
        if (d == Direction.up) dirModifier *= -1;

        foreach (Button button in buttons)
        {
            button.transform.SetSiblingIndex((button.transform.GetSiblingIndex() + dirModifier) % 3);
            yield return null;
        }
    }

}
