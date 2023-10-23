using UnityEngine;

[System.Serializable]
public class UpgradeData
{
    public GameObject UpgradePrefab;
    public string UpgradeName;
    public int upgradeLevel = 0;
    public string[] upgradeDescriptions;
    public int index;
    public Sprite icon;
    public GameObject activeUpgrade;
}
