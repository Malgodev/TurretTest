using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
struct Upgrade
{
    public EUpgradeName Name;
    public int MaxiumUpgrade;

    public Upgrade(EUpgradeName name, int maxiumUpgrade)
    {
        this.Name = name;
        this.MaxiumUpgrade = maxiumUpgrade;
    }
}

public enum EUpgradeName
{
    IncreaseDamage,
    BulletNumber,
    BulletSize,
    Ricochet,
    Piercing,
    Vapirism,
    Null,
}

public class TurretUpgradeController : MonoBehaviour
{
    [SerializeField] public static TurretUpgradeController Instance {get; private set;}

    [SerializeField] private List<Upgrade> upgradeList = new List<Upgrade>();
    public Dictionary<EUpgradeName, int> CurUpgrade { get; private set; } = new Dictionary<EUpgradeName, int>();

    [Header("Upgrade List")]
    [SerializeField] private EUpgradeName[] choseableUpgrade;
    [SerializeField] private PlayerController playerController;

    [Header("UI")]
    [SerializeField] private GameObject selectUpgradePanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        playerController = GameManager.Instance.PlayerController;

        int index = 0;

        foreach (Transform button in selectUpgradePanel.transform)
        {
            button.GetComponent<Button>().onClick.AddListener(() => SelectPower(index++));
        }
    }

    public void SelectUpgrade()
    {
        CreateUpgradeList();

        int index = 0;
        foreach (Transform button in selectUpgradePanel.transform)
        {
            foreach (Transform text in button)
            {
                text.GetComponent<TMP_Text>().text = choseableUpgrade[index++].ToString();
            }
        }
        Time.timeScale = 0;

        ShowSelectUpgradePanel(true);
    }

    public void CreateUpgradeList()
    {
        List<EUpgradeName> upgradeableList = upgradeList.Select(upgrade => upgrade.Name).ToList();

        foreach (EUpgradeName upgradeName in CurUpgrade.Keys)
        {
            Upgrade upgrade = GetUpgradeInfo(upgradeName);
            if (CurUpgrade[upgradeName] > upgrade.MaxiumUpgrade)
            {
                upgradeableList.Remove(upgradeName);
            }
        }

        List<EUpgradeName> result = new List<EUpgradeName>(upgradeableList);

        int count = Mathf.Min(3, upgradeableList.Count);

        if (count == 0)
        {
            return;
        }

        while (result.Count > count)
        {
            int randomIndex = UnityEngine.Random.Range(0, result.Count);
            result.RemoveAt(randomIndex);
        }

        choseableUpgrade = result.ToArray();
    }

    public void SelectPower(int i)
    {
        Time.timeScale = 1;
        EUpgradeName selectedUpgrade = choseableUpgrade[i];

        if (CurUpgrade.ContainsKey(selectedUpgrade))
        {
            CurUpgrade[selectedUpgrade]++;
        }
        else
        {
            CurUpgrade[selectedUpgrade] = 1;
        }

        UpgradePlayer(selectedUpgrade);
        ShowSelectUpgradePanel(false);
    }

    private void UpgradePlayer(EUpgradeName upgradeName)
    {
        CurUpgrade[upgradeName]++;
    }

    public void ShowSelectUpgradePanel(bool isShow)
    {
        selectUpgradePanel.SetActive(isShow);
    }

    private Upgrade GetUpgradeInfo(EUpgradeName upgradeName)
    {
        foreach (Upgrade upgrade in upgradeList)
        {
            if (upgrade.Name == upgradeName)
            {
                return upgrade;
            }
        }

        return new Upgrade(EUpgradeName.Null, 0);
    }
}
