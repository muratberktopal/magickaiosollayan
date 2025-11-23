using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Game/Upgrade Card")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName; // Kartýn ismi (Örn: Hýzlý Koþ)
    public string description; // Açýklama (Örn: Hýzýný %10 artýrýr)
    public Sprite icon;        // Kartýn resmi

    // Hangi özelliði güçlendirecek?
    public UpgradeType upgradeType;
    public float value;        // Ne kadar artýracak? (Örn: 10, 0.5 vs.)
}

// Güçlendirme Türleri
public enum UpgradeType
{
    MoveSpeed,  // Hýz
    Damage,     // Hasar
    Health,     // Can
    AttackSpeed // Saldýrý Hýzý
}