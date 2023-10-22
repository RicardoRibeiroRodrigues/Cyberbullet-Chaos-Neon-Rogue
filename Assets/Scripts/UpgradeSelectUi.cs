using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UpgradeSelectUi : MonoBehaviour
{
    public GameObject[] upgradeCards;
    private List<UpgradeData> upgrades;

    public void selectUpgrade(List<UpgradeData> upgrades)
    {
        this.upgrades = upgrades;
        for (int i = 0; i < upgrades.Count; i++)
        {
            assembleUpgradeCard(upgradeCards[i], upgrades[i]);
        }
    }
    

    void assembleUpgradeCard(GameObject upgradeCard, UpgradeData upgradeData)
    {
        // Set upgrade card title
        var title = upgradeCard.transform.Find("Title");
        title.GetComponent<TextMeshProUGUI>().text = upgradeData.UpgradeName;
        // Set upgrade card description
        var description = upgradeCard.transform.Find("UpgradeDesc").transform.Find("DescText");
        description.GetComponentInChildren<TextMeshProUGUI>().text = upgradeData.upgradeDescriptions[upgradeData.upgradeLevel];
        // Set upgrade card icon
        var icon = upgradeCard.transform.Find("UpImage");
        // Raw image texture
        icon.GetComponent<UnityEngine.UI.RawImage>().texture = upgradeData.icon;
        upgradeCard.SetActive(true);
    }

    public void selectUpgrade(int upgradeIndex)
    {
        GameManager.Instance.setSelectedIndex(this.upgrades[upgradeIndex].index);
    }
}
