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
        int mode = PlayerPrefs.GetInt("GameMode", 0);

        if (mode == 0) // --- BATTLE ROYALE ---
        {
            Debug.Log("MOD: Battle Royale Seçildi. BR Spawner Açýlýyor.");

            // BR'yi Uyandýr
            if (brSpawnerObject) brSpawnerObject.SetActive(true); // <--- BURASI AÇAR
            if (brUI) brUI.SetActive(true);
            if (brManagerScript) brManagerScript.enabled = true;

            // Survival'ý Garanti Kapat (Zaten kapalý ama olsun)
            if (survivalSpawnerObject) survivalSpawnerObject.SetActive(false);
            if (survivalUI) survivalUI.SetActive(false);
        }
        else // --- SURVIVAL ---
        {
            Debug.Log("MOD: Survival Seçildi. Survival Spawner Açýlýyor.");

            // Survival'ý Uyandýr
            if (survivalSpawnerObject) survivalSpawnerObject.SetActive(true); // <--- BURASI AÇAR
            if (survivalUI) survivalUI.SetActive(true);

            // BR'yi Garanti Kapat
            if (brSpawnerObject) brSpawnerObject.SetActive(false);
            if (brUI) brUI.SetActive(false);
            if (brManagerScript) brManagerScript.enabled = false;
        }
    }
}