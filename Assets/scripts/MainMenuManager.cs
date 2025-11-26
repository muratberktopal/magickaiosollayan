using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Paneller")]
    public GameObject modeSelectionPanel;

    // ... (Ses kodlarýn burada kalabilir, dokunma) ...

    public void OpenModeSelection()
    {
        modeSelectionPanel.SetActive(true);
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

    public void CloseModeSelection()
    {
        modeSelectionPanel.SetActive(false);
    }
}