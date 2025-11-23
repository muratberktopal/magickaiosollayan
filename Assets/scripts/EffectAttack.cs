using UnityEngine;

public class EffectAttack : MonoBehaviour
{
    [Header("Gerekli Parçalar")]
    public GameObject slashPrefab;
    public Transform firePoint;

    [Header("Ayarlar")]
    public float attackRate = 0.5f;
    public float effectLifeTime = 0.2f;

    private float nextAttackTime = 0f;

    // Her karede klavyeyi dinlememiz lazým, o yüzden Update kullanýyoruz
    void Update()
    {
        // BÝLGÝSAYAR ÝÇÝN EKLEME: Space tuþuna basýldý mý?
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PerformAttack();
        }
#endif
    }

    // Butona ve Klavyeye baðlý ortak fonksiyon
    public void PerformAttack()
    {
        if (Time.time >= nextAttackTime)
        {
            SpawnSlash();

            // --- SESÝ ÇAL ---
            if (AudioManager.instance != null)
                AudioManager.instance.PlayAttack();
            // ---------------

            nextAttackTime = Time.time + attackRate;
        }
        if (Time.time >= nextAttackTime)
        {
            SpawnSlash();
            nextAttackTime = Time.time + attackRate;
        }
    }

    void SpawnSlash()
    {
        if (slashPrefab == null || firePoint == null) return;

        // Efekti yatay oluþtur
        Quaternion rotasyon = Quaternion.Euler(90, transform.eulerAngles.y, 0);
        GameObject currentSlash = Instantiate(slashPrefab, firePoint.position, rotasyon);

        // Sahibini ata
        SimpleWeapon weaponScript = currentSlash.GetComponent<SimpleWeapon>();
        if (weaponScript != null)
        {
            weaponScript.owner = this.gameObject;
        }

        Destroy(currentSlash, effectLifeTime);
    }
}