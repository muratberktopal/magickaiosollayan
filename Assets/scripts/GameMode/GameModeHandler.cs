using UnityEngine;

public class GameModeHandler : MonoBehaviour
{
    [Header("Battle Royale Parçalarý")]
    public GameObject battleRoyaleUI; // "Kalan: 10" yazýsý
    public BattleRoyaleManager brManager;

    [Header("Survival Parçalarý")]
    public GameObject survivalTimerUI; // YENÝ: "00:00" yazýsý

    void Start()
    {
        int mode = PlayerPrefs.GetInt("GameMode", 0);

        if (mode == 0) // BATTLE ROYALE
        {
            // BR aç, Survival kapat
            if (battleRoyaleUI) battleRoyaleUI.SetActive(true);
            if (brManager) brManager.enabled = true;

            if (survivalTimerUI) survivalTimerUI.SetActive(false); // Saati gizle
        }
        else // SURVIVAL
        {
            // BR kapat, Survival aç
            if (battleRoyaleUI) battleRoyaleUI.SetActive(false);
            if (brManager) brManager.enabled = false;

            if (survivalTimerUI) survivalTimerUI.SetActive(true); // Saati göster
        }
    }
}