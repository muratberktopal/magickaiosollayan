using UnityEngine;
using UnityEngine.UI;

public class WeaponSelector : MonoBehaviour
{
    [Header("Spawner Referanslarý (YENÝ)")]
    public SpawnerBR brSpawnerScript;
    public SpawnerSurvival survivalSpawnerScript;

    [Header("UI Elemanlarý")]
    public GameObject selectionPanel; // Baþlangýç paneli
    public Button gameAttackButton;   // Sað alttaki saldýrý butonu

    [Header("Player Silah Kodlarý")]
    public PlayerRazorController razorScript;
    public PlayerTreeController treeScript;
    public PlayerNunchakuController nunchakuScript;
    public PlayerFlailController flailScript;
    public PlayerNetController netScript;
    public PlayerDoubleSwordController doubleSwordScript;
    public PlayerTeslaController teslaScript;
    public PlayerIceShardController iceScript;
    public PlayerCompositeBowController compositeBowScript;
    public PlayerChainController chainScript;
    public PlayerBoomerangController boomerangScript;
    public PlayerWhipController whipScript;
    public PlayerGreatswordController greatswordScript;
    public PlayerFireballController fireballScript;
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

        int mode = PlayerPrefs.GetInt("GameMode", 0);

        if (mode == 0) // Battle Royale
        {
            // Eðer BR Spawner açýksa baþlat
            if (brSpawnerScript != null && brSpawnerScript.gameObject.activeInHierarchy)
            {
                brSpawnerScript.StartBattle();
            }
        }
        else // Survival
        {
            // Eðer Survival Spawner açýksa baþlat
            if (survivalSpawnerScript != null && survivalSpawnerScript.gameObject.activeInHierarchy)
            {
                survivalSpawnerScript.StartBattle();
            }
        }
    }


    // --- SEÇÝM FONKSÝYONLARI ---
    public void SelectTree()
    {
        EnableWeapon(treeScript);
        SetButtonListener(() => treeScript.Attack());

        EvolutionManager.instance.SetStarterWeapon(ItemType.Tree);
    }
    public void SelectTesla()
    {
        EnableWeapon(teslaScript);
        SetButtonListener(() => teslaScript.Attack());
        EvolutionManager.instance.SetStarterWeapon(ItemType.Lightning);
    }

    public void SelectIceShard()
    {
        EnableWeapon(iceScript);
        SetButtonListener(() => iceScript.Attack());

        
         EvolutionManager.instance.SetStarterWeapon(ItemType.IceShard);
    }

    public void SelectWhip()
    {
        EnableWeapon(whipScript);
        SetButtonListener(() => whipScript.Attack());
    }
    public void SelectGreatsword()
    {
        EnableWeapon(greatswordScript);
        SetButtonListener(() => greatswordScript.Attack());
    }
    public void SelectFireball()
    {
        EnableWeapon(fireballScript);
        SetButtonListener(() => fireballScript.Attack());
    }
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
    public void SelectGreatSword()
    {
        EnableWeapon(scytheScript);
        SetButtonListener(() => scytheScript.Attack());

        EvolutionManager.instance.SetStarterWeapon(ItemType.Scythe);
    }
    public void SelectNunchaku()
    {
        EnableWeapon(nunchakuScript);
        SetButtonListener(() => nunchakuScript.Attack());

        EvolutionManager.instance.SetStarterWeapon(ItemType.Nunchaku); 
    }
    public void SelectFlail()
    {
        EnableWeapon(flailScript);
        // Gürz fiziksel olduðu için Attack boþ olabilir ama hata vermemesi için baðlýyoruz
        SetButtonListener(() => flailScript.Attack());

        // Evolution Manager'a "Ben Gürz seçtim" diyoruz
        EvolutionManager.instance.SetStarterWeapon(ItemType.Flail);
    }
    public void SelectChain()
    {
        EnableWeapon(chainScript);
        SetButtonListener(() => chainScript.Attack());

        EvolutionManager.instance.SetStarterWeapon(ItemType.Scythe);
    }
    public void SelectDouble_Sided_Sword()
    {
        EnableWeapon(doubleSwordScript);
        SetButtonListener(() => doubleSwordScript.Attack());

        EvolutionManager.instance.SetStarterWeapon(ItemType.Double_Sided_Sword);
    }
    public void SelectComposite_bow()
    {
        EnableWeapon(compositeBowScript);
        SetButtonListener(() => compositeBowScript.Attack());

        

        EvolutionManager.instance.SetStarterWeapon(ItemType.Composite_bow);
    }
    public void SelectBoomerang()
    {
        EnableWeapon(boomerangScript);
        SetButtonListener(() => boomerangScript.Attack());

        EvolutionManager.instance.SetStarterWeapon(ItemType.Scythe);
    }
    public void SelectNet()
    {
        EnableWeapon(netScript);
        SetButtonListener(() => netScript.Attack());

        EvolutionManager.instance.SetStarterWeapon(ItemType.Net);

        

    }

    public void SelectRazor()
    {
        EnableWeapon(razorScript);
        SetButtonListener(() => razorScript.Attack());

        // Evolution Manager'a "Ben Jilet seçtim" diyoruz
        // (Eðer ItemType enum'ýna Razor eklemediysen hata verebilir, eklemeyi unutma)
        // EvolutionManager.instance.SetStarterWeapon(ItemType.Razor); 
    }













    // --- YARDIMCI FONKSÝYONLAR ---


    void DisableAllWeapons()
    {
        if (razorScript) razorScript.enabled = false;
        if (treeScript) treeScript.enabled = false;
        if (nunchakuScript) nunchakuScript.enabled = false;
        if (flailScript) flailScript.enabled = false;
        if (netScript) netScript.enabled = false;
        if (doubleSwordScript) doubleSwordScript.enabled = false;
        if (teslaScript) teslaScript.enabled = false;
        if (iceScript) iceScript.enabled = false;
        if (compositeBowScript) compositeBowScript.enabled = false;
        if (chainScript) chainScript.enabled = false;
        if (boomerangScript) boomerangScript.enabled = false;
        if (whipScript) whipScript.enabled = false;
        if (fireballScript) fireballScript.enabled = false;
        if (scytheScript) scytheScript.enabled = false;
        if (slashScript) slashScript.enabled = false;
        if (spearScript) spearScript.enabled = false;
        if (magicScript) magicScript.enabled = false;
        if (clubScript) clubScript.enabled = false;
        if (bowScript) bowScript.enabled = false;
        if (chaosScript) chaosScript.enabled = false;
        if (greatswordScript) greatswordScript.enabled = false;
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

        // --- EKSÝK OLAN KISIM BURASI ---
        // Hangi modda olduðumuzu kontrol et
        int mode = PlayerPrefs.GetInt("GameMode", 0);

        if (mode == 0) // Battle Royale
        {
            // BR Spawner açýksa ve atanmýþsa baþlat
            if (brSpawnerScript != null && brSpawnerScript.gameObject.activeInHierarchy)
            {
                brSpawnerScript.StartBattle();
            }
        }
        else // Survival
        {
            // Survival Spawner açýksa ve atanmýþsa baþlat
            if (survivalSpawnerScript != null && survivalSpawnerScript.gameObject.activeInHierarchy)
            {
                survivalSpawnerScript.StartBattle();
            }
        }
    }
}