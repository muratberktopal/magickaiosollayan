using UnityEngine;
using UnityEngine.UI;

public class WeaponSelector : MonoBehaviour
{
    [Header("UI Elemanlarý")]
    public GameObject selectionPanel; // Açýlýþta çýkan panel
    public Button gameAttackButton;   // Oyun içindeki saldýrý butonu

    [Header("Player Silah Kodlarý")]
    public EffectAttack slashScript;          // Kýlýç
    public PlayerClubController clubScript;   // Sopa (Balyoz)
    public PlayerRopeController ropeScript;   // <-- YENÝ: Ýp (Kýrbaç)

    // Mýzrak scripti burada dursun ama panelden seçtirmeyeceðiz (Ýlerde kartlardan çýkacak)
    public PlayerSpearController spearScript;

    void Start()
    {
        // 1. Oyunu Dondur
        Time.timeScale = 0;
        selectionPanel.SetActive(true);

        // 2. Tüm silahlarý kapalý baþlat
        slashScript.enabled = false;
        clubScript.enabled = false;
        ropeScript.enabled = false;
        spearScript.enabled = false;
    }

    // 1. Buton: KILIÇ
    public void SelectSlash()
    {
        slashScript.enabled = true;
        SetAttackButton(() => slashScript.PerformAttack());
        StartGame();
    }

    // 2. Buton: SOPA
    public void SelectClub()
    {
        clubScript.enabled = true;
        SetAttackButton(() => clubScript.PerformAttack());
        StartGame();
    }

    // 3. Buton: ÝP (Eski Mýzrak Butonu buna baðlanacak)
    public void SelectRope()
    {
        ropeScript.enabled = true;
        SetAttackButton(() => ropeScript.PerformAttack());
        StartGame();
    }

    // Yardýmcý Fonksiyon: Buton baðlama iþini kýsaltýr
    void SetAttackButton(UnityEngine.Events.UnityAction action)
    {
        gameAttackButton.onClick.RemoveAllListeners();
        gameAttackButton.onClick.AddListener(action);
    }

    void StartGame()
    {
        selectionPanel.SetActive(false);
        Time.timeScale = 1;
    }
}