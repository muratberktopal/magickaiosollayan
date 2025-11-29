using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Paneller")]
    public GameObject modeSelectionPanel; // Survival/BR seçtiðin pencere

    [Header("Buton Gruplarý")]
    public GameObject mainMenuButtons; // Play, Settings ve Quit butonlarýný içine koyacaðýn obje

    public void OpenModeSelection()
    {
        // Mod seçimini aç, ana butonlarý gizle
        modeSelectionPanel.SetActive(true);

        if (mainMenuButtons != null)
            mainMenuButtons.SetActive(false);
    }

    public void CloseModeSelection()
    {
        // Mod seçimini kapat, ana butonlarý geri getir
        modeSelectionPanel.SetActive(false);

        if (mainMenuButtons != null)
            mainMenuButtons.SetActive(true);
    }

    public void PlayBattleRoyale()
    {
        // Hafýzaya 0 yaz (0 = Battle Royale)
        PlayerPrefs.SetInt("GameMode", 0);
        SceneManager.LoadScene(1); // Oyuna Gir
    }

    public void PlaySurvival()
    {
        // Hafýzaya 1 yaz (1 = Survival)
        PlayerPrefs.SetInt("GameMode", 1);
        SceneManager.LoadScene(1); // Oyuna Gir
    }
}