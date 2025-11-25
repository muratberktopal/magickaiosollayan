using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    // Deðiþkenler (Class'ýn en tepesine ekle/güncelle)
    [Header("Level Ayarlarý")]
    public int currentLevel = 1;
    public int currentXP = 0;
    public int xpToNextLevel = 100; // Ýlk level için gereken XP
    public float levelMultiplier = 1.2f; // Her levelda zorluk %20 artsýn (Dengeli)
    public ParticleSystem levelUpEffect;

    // XP Taþý (LootItem) bu fonksiyonu çaðýrýr
    public void GainXP(int amount)
    {
        currentXP += amount;

        // XP barý doldu mu?
        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        currentLevel++;

        if (AudioManager.instance != null)
            AudioManager.instance.PlayLevelUp();

        // Fazla XP'yi sýfýrla (Basit yöntem)
        // Ýstersen artan XP'yi bir sonraki levele devredebilirsin: currentXP -= xpToNextLevel;
        currentXP = 0;

        // Her levelde gereken XP'yi biraz artýralým ki oyun zorlaþsýn (Opsiyonel)
        xpToNextLevel += 50;

        // --- BÜYÜME KODU SÝLÝNDÝ (ARTIK BÜYÜMÜYORUZ) ---
        // transform.localScale += ... (ÝPTAL)

        // --- YENÝ SÝSTEM: KART EKRANINI AÇ ---
        // UpgradeManager'a "Hey, level atladým, bana kartlarý göster" diyoruz.
        if (UpgradeManager.instance != null)
        {
            UpgradeManager.instance.ProcessLevelUp(currentLevel);
        }
        else
        {
            Debug.LogError("HATA: Sahnede 'UpgradeManager' yok! GameManager objene scripti attýn mý?");
        }

        // Görsel Efekt (Varsa)
        if (levelUpEffect != null)
        {
            Instantiate(levelUpEffect, transform.position, Quaternion.identity);
        }

        Debug.Log("LEVEL ATLADIN! Seviye: " + currentLevel);
    }
}