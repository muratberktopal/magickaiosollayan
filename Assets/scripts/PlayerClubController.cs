using UnityEngine;

public class PlayerClubController : MonoBehaviour
{
    [Header("Gerekli Parçalar")]
    public GameObject clubPrefab; // ClubBase Prefab'i

    [Header("Saldýrý Ayarlarý")]
    public float attackRate = 1.0f;

    [Tooltip("Sopa kaç derecelik yay çizecek? (Örn: 120, 150)")]
    public float swingAngle = 140f;

    [Tooltip("Dönme Hýzý")]
    public float swingSpeed = 800f;

    [Header("Güç Ayarlarý")]
    public int damageAmount = 40;

    [Tooltip("Düþmaný ne kadar uzaða fýrlatsýn?")]
    public float knockbackForce = 10f; // <-- YENÝ: Ayarlanabilir Geri Tepme Gücü

    private float nextAttackTime = 0f;

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PerformAttack();
        }
#endif
    }

    public void PerformAttack()
    {
        if (Time.time >= nextAttackTime)
        {
            StartSwing();

            if (AudioManager.instance != null)
                AudioManager.instance.PlayAttack();

            nextAttackTime = Time.time + attackRate;
        }
    }

    void StartSwing()
    {
        if (clubPrefab == null) return;

        // Prefab'i oluþtur
        GameObject currentClub = Instantiate(clubPrefab, transform.position, transform.rotation);

        // --- 1. HAREKET AYARI ---
        ClubSwingLogic logic = currentClub.GetComponent<ClubSwingLogic>();
        if (logic == null) logic = currentClub.AddComponent<ClubSwingLogic>();

        // Hýz ve Açý ile kurulum yap
        logic.Setup(this.gameObject, swingSpeed, swingAngle);


        // --- 2. HASAR VE KNOCKBACK AYARI ---
        // SimpleWeapon scriptini bul (Child objelerde olabilir, o yüzden GetComponentInChildren kullanýyoruz)
        SimpleWeapon weapon = currentClub.GetComponentInChildren<SimpleWeapon>();

        if (weapon != null)
        {
            weapon.owner = this.gameObject;     // Vuran benim
            weapon.damage = damageAmount;       // Hasar bu kadar
            weapon.knockback = knockbackForce;  // <-- YENÝ: Ýtme gücünü buraya aktarýyoruz
        }
    }
}