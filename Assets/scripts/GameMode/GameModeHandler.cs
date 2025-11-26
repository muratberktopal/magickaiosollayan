using UnityEngine;

public class GameModeHandler : MonoBehaviour
{
    [Header("Battle Royale Parçalarý")]
    public GameObject battleRoyaleUI; // "Kalan: 10" Yazýsý
    public BattleRoyaleManager brManager; // Yönetici Scripti

    void Start()
    {
        // Hafýzadan modu oku (0: BR, 1: Survival)
        int mode = PlayerPrefs.GetInt("GameMode", 0);

        if (mode == 0)
        {
            // --- BATTLE ROYALE MODU ---
            // Her þeyi aç
            if (battleRoyaleUI) battleRoyaleUI.SetActive(true);
            if (brManager) brManager.enabled = true;
        }
        else
        {
            // --- SURVIVAL MODU ---
            // Her þeyi kapat
            if (battleRoyaleUI)
            {
                battleRoyaleUI.SetActive(false); // <--- ÝÞTE BU SATIR GÝZLER
            }
            else
            {
                Debug.LogError("HATA: 'Battle Royale UI' kutusu boþ! Kapatacak bir þey bulamadým.");
            }

            if (brManager) brManager.enabled = false; // BR kurallarýný devre dýþý býrak
        }
    }
}