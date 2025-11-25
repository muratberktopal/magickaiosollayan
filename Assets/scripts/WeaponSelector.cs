using UnityEngine;
using UnityEngine.UI;

public class WeaponSelector : MonoBehaviour
{
    [Header("UI Elemanlarý")]
    public GameObject selectionPanel; // Baþlangýç paneli
    public Button gameAttackButton;   // Sað alttaki saldýrý butonu

    [Header("Player Silah Kodlarý")]
    public PlayerChaosController chaosScript;
    public PlayerScytheController scytheScript;
    public PlayerBowController bowScript;
    public EffectAttack slashScript;          // Kýlýç (Slash)
    public PlayerSpearController spearScript; // Mýzrak (Spear)
    public PlayerMagicCaster magicScript;     // Büyü (Magic)
    public PlayerClubController clubScript;   // Sopa (Club)

    void Start()
    {
        Time.timeScale = 0; // Oyunu durdur
        selectionPanel.SetActive(true);

        // Baþlangýçta hepsini kapat (Çakýþma olmasýn)
        DisableAllWeapons();
    }

    // --- SEÇÝM FONKSÝYONLARI ---
    
    public void SelectSlash() // KILIÇ
    {
        EnableWeapon(slashScript);
        SetButtonListener(() => slashScript.PerformAttack());

        EvolutionManager.instance.SetStarterWeapon(ItemType.Slash);
    }
    
    public void SelectClub() // SOPA
    {
        EnableWeapon(clubScript);
        SetButtonListener(() => clubScript.Attack());

        EvolutionManager.instance.SetStarterWeapon(ItemType.Club);
    }
    public void SelectMagic() // BÜYÜ
    {
        EnableWeapon(magicScript);
        SetButtonListener(() => magicScript.Attack());

        EvolutionManager.instance.SetStarterWeapon(ItemType.Staff);
    }

    public void SelectRope() // BÜYÜ
    {
        // BURA DOLCAK SONRA


    }

    public void SelectSpear() // MIZRAK
    {
        EnableWeapon(spearScript);
        SetButtonListener(() => spearScript.Attack());

        EvolutionManager.instance.SetStarterWeapon(ItemType.Spear);
    }

    
    public void SelectChaos()
    {
        EnableWeapon(chaosScript);
        SetButtonListener(() => chaosScript.Attack());

        EvolutionManager.instance.SetStarterWeapon(ItemType.Chaos);
    }
    public void SelectBow()
    {
        EnableWeapon(bowScript);
        SetButtonListener(() => bowScript.Attack());

        EvolutionManager.instance.SetStarterWeapon(ItemType.Bow);
    }

    public void SelectScythe()
    {
        EnableWeapon(scytheScript);
        SetButtonListener(() => scytheScript.Attack());

        EvolutionManager.instance.SetStarterWeapon(ItemType.Scythe);
    }
    // --- YARDIMCI FONKSÝYONLAR ---

    void DisableAllWeapons()
    {
        if (scytheScript) scytheScript.enabled = false;
        if (slashScript) slashScript.enabled = false;
        if (spearScript) spearScript.enabled = false;
        if (magicScript) magicScript.enabled = false;
        if (clubScript) clubScript.enabled = false;
        if (bowScript) bowScript.enabled = false;
        if (chaosScript) chaosScript.enabled = false;
    }

    void EnableWeapon(MonoBehaviour script)
    {
        DisableAllWeapons(); // Önce hepsini kapat
        if (script != null) script.enabled = true; // Sonra seçileni aç
    }

    void SetButtonListener(UnityEngine.Events.UnityAction action)
    {
        if (gameAttackButton != null)
        {
            gameAttackButton.onClick.RemoveAllListeners();
            gameAttackButton.onClick.AddListener(action);
        }
        StartGame();
    }

    void StartGame()
    {
        selectionPanel.SetActive(false);
        Time.timeScale = 1;
    }
}