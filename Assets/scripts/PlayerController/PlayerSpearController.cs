using UnityEngine;

public class PlayerSpearController : MonoBehaviour
{
    [Header("Gerekli Parçalar")]
    public GameObject spearPrefab; // Mýzrak Prefabý

    [Header("Ayarlar")]
    public float attackRate = 0.8f;    // Saldýrý hýzý
    public float spawnHeight = 1.0f;   // Bel hizasý
    public float forwardOffset = 0.5f; // Karakterin ne kadar önünde çýksýn?

    private float nextAttackTime = 0f;

    // PC Testi (Space Tuþu)
    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
#endif
    }

    // Bu fonksiyonu WeaponSelector (Buton) çaðýrýyor
    public void Attack()
    {
        if (Time.time >= nextAttackTime)
        {
            SpawnSpear(); // <--- HATA BURADAYDI, AÞAÐIDAKÝ FONKSÝYONU BULAMIYORDU
            nextAttackTime = Time.time + attackRate;
        }
    }

    // --- ÝÞTE EKSÝK OLAN FONKSÝYON BU ---
    void SpawnSpear()
    {
        if (spearPrefab == null) return;

        // Karakterin önünde ve bel hizasýnda pozisyon belirle
        Vector3 spawnPos = transform.position + (Vector3.up * spawnHeight) + (transform.forward * forwardOffset);

        // Mýzraðý oluþtur
        GameObject currentSpear = Instantiate(spearPrefab, spawnPos, transform.rotation);

        // Mýzraða sahibini tanýt (Hasar sistemi için)
        SimpleWeapon weapon = currentSpear.GetComponent<SimpleWeapon>();
        if (weapon != null)
        {
            weapon.owner = this.gameObject;
        }

        // Ses Çal
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySpear();
        }
    }
}