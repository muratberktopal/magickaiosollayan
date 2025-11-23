using UnityEngine;
using UnityEngine.UI;

public class WeaponSelector : MonoBehaviour
{
    [Header("UI Elemanlarý")]
    public GameObject selectionPanel; // Açýlýþta çýkan panel
    public Button gameAttackButton;   // Oyun içindeki (Sað alttaki) saldýrý butonu

    [Header("Player Silah Kodlarý")]
    public EffectAttack slashScript;       // Kýlýç Kodu
    public PlayerSpearController spearScript; // Mýzrak Kodu
    public PlayerClubController clubScript; //anet was here
    void Start()
    {
        // 1. Oyunu Dondur
        Time.timeScale = 0;
        selectionPanel.SetActive(true);

        // 2. Her ihtimale karþý silahlarý kapalý baþlat
        slashScript.enabled = false;
        spearScript.enabled = false;
        clubScript.enabled = false;
    }

    // Sol Kutuyu Seçince
    public void SelectSlash()
    {
        // Kýlýç kodunu aç
        slashScript.enabled = true;

        // Saldýrý butonunu KILIÇ koduna baðla (Otomatik Kablolama)
        gameAttackButton.onClick.RemoveAllListeners(); // Eski baðlantýyý sil
        gameAttackButton.onClick.AddListener(() => slashScript.PerformAttack());

        StartGame();
    }

    // Sað Kutuyu Seçince
    public void SelectSpear()
    {
        // Mýzrak kodunu aç
        spearScript.enabled = true;

        // Saldýrý butonunu MIZRAK koduna baðla
        gameAttackButton.onClick.RemoveAllListeners();
        gameAttackButton.onClick.AddListener(() => spearScript.Attack());

        StartGame();
    }

    // Yeni Kutuyu Seçince
    public void SelectClub() // UI butonuna bu fonksiyonu baðlayýn
    {
        // Sopa kodunu aç
        clubScript.enabled = true;

        // Diðerlerini kapatmanýza gerek yok çünkü Start'ta zaten kapalý
        // Saldýrý butonunu SOPA koduna baðla
        gameAttackButton.onClick.RemoveAllListeners();
        gameAttackButton.onClick.AddListener(() => clubScript.PerformAttack());

        StartGame();
    }



    void StartGame()
    {
        // Paneli kapat ve zamaný baþlat
        selectionPanel.SetActive(false);
        Time.timeScale = 1;
    }
}