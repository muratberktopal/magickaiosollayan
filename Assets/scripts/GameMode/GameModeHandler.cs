using UnityEngine;

public class GameModeHandler : MonoBehaviour
{
    [Header("BATTLE ROYALE Parçalarý")]
    public GameObject brSpawnerObject;
    public GameObject brUI;
    public BattleRoyaleManager brManagerScript;

    [Header("SURVIVAL Parçalarý")]
    public GameObject survivalSpawnerObject;
    public GameObject survivalUI;

    void Start()
    {
        // 1. KONTROL: Script Çalýþýyor mu?
        Debug.Log("--- GAMEMODEHANDLER ÇALIÞTI ---");

        // 2. MOD OKUMA: Hafýzada ne var?
        int mode = PlayerPrefs.GetInt("GameMode", 0);
        Debug.Log("Hafýzadan Okunan Mod: " + (mode == 0 ? "BATTLE ROYALE" : "SURVIVAL"));

        if (mode == 0) // BATTLE ROYALE
        {
            // Açýlacaklar
            if (brSpawnerObject != null) brSpawnerObject.SetActive(true);
            else Debug.LogError("HATA: BR Spawner Object kutusu BOÞ!");

            if (brUI != null) brUI.SetActive(true);
            if (brManagerScript != null) brManagerScript.enabled = true;

            // Kapanacaklar
            if (survivalSpawnerObject != null) survivalSpawnerObject.SetActive(false);
            if (survivalUI != null) survivalUI.SetActive(false);
        }
        else // SURVIVAL
        {
            // Açýlacaklar
            if (survivalSpawnerObject != null) survivalSpawnerObject.SetActive(true);
            else Debug.LogError("HATA: Survival Spawner Object kutusu BOÞ!");

            if (survivalUI != null) survivalUI.SetActive(true);

            // Kapanacaklar
            if (brSpawnerObject != null) brSpawnerObject.SetActive(false);
            if (brUI != null) brUI.SetActive(false);
            if (brManagerScript != null) brManagerScript.enabled = false;
        }
    }
}