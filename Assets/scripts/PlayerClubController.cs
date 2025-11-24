using UnityEngine;

public class PlayerClubController : MonoBehaviour
{
    [Header("Gerekli Parçalar")]
    public GameObject clubPrefab; // Sopa Prefabý (Küt ve geniþ bir küp yap)

    [Header("Ayarlar")]
    public float attackRate = 1.0f;    // Sopa aðýrdýr, yavaþ vursun
    public float spawnHeight = 1.0f;   // Bel hizasý
    public float forwardOffset = 0.8f; // Biraz ileride çýksýn

    private float nextAttackTime = 0f;

    // PC Testi
    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
#endif
    }

    // --- ÝÞTE EKSÝK OLAN FONKSÝYON BU ---
    // WeaponSelector scripti burayý arýyor!
    public void Attack()
    {
        if (Time.time >= nextAttackTime)
        {
            SpawnClub();
            nextAttackTime = Time.time + attackRate;
        }
    }
    // ------------------------------------

    void SpawnClub()
    {
        if (AudioManager.instance != null) AudioManager.instance.PlayClub();
        if (clubPrefab == null) return;

        // Karakterin önünde oluþtur
        Vector3 spawnPos = transform.position + (Vector3.up * spawnHeight) + (transform.forward * forwardOffset);

        // Sopayý oluþtur
        GameObject club = Instantiate(clubPrefab, spawnPos, transform.rotation);

        // Sahibini ata
        SimpleWeapon weapon = club.GetComponent<SimpleWeapon>();
        if (weapon != null) weapon.owner = gameObject;

        

        // Sopa vurduktan hemen sonra yok olsun
        Destroy(club, 0.3f);
    }
}