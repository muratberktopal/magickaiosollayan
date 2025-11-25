using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Game/Upgrade Card")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    public string description;
    public Sprite icon;

    public UpgradeType upgradeType; // Stat mý yoksa Eþya mý?
    public float value;        // Stat ise deðeri

    // YENÝ: Eðer bu bir Eþya/Malzeme kartýysa, hangi eþyayý verecek?
    public ItemType itemReward;
}

public enum UpgradeType
{
    MoveSpeed,
    Damage,
    Health,
    AttackSpeed,
    Item // <-- YENÝ TÜR: Bunu seçersen Stat deðil Eþya verir
}