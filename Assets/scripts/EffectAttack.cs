using UnityEngine;

public class EffectAttack : MonoBehaviour
{
    [Header("Gerekli Parçalar")]
    public GameObject slashPrefab; // Oluþturulacak Efekt (Prefab)
    public Transform firePoint;    // Nerede çýkacak? (FirePoint)

    [Header("Ayarlar")]
    public float attackRate = 0.5f; // Saniyede kaç vuruþ?
    public float effectLifeTime = 0.2f; // Efekt kaç saniye ekranda kalsýn? (Çok kýsa olmalý)

    private float nextAttackTime = 0f;

    // Butona baðlayacaðýmýz fonksiyon
    public void PerformAttack()
    {
        // Zamaný geldi mi?
        if (Time.time >= nextAttackTime)
        {
            SpawnSlash();
            nextAttackTime = Time.time + attackRate;
        }
    }

    void SpawnSlash()
    {
        if (slashPrefab == null || firePoint == null) return;

        // --- ÝÞTE SÝHÝRLÝ SATIR ---
        // Efektin açýsýný oluþturuyoruz:
        // X = 90 (Yere yatýr)
        // Y = transform.eulerAngles.y (Karakterin baktýðý yöne çevir)
        // Z = 0 (Yan yatmasýn)
        Quaternion rotasyon = Quaternion.Euler(90, transform.eulerAngles.y, 0);

        // 1. Efekti FirePoint noktasýnda, YENÝ ROTASYON ile oluþtur
        GameObject currentSlash = Instantiate(slashPrefab, firePoint.position, rotasyon);

        // --------------------------

        // Hasar Scriptine "Bunun sahibi benim, bana vurma" de.
        SimpleWeapon weaponScript = currentSlash.GetComponent<SimpleWeapon>();
        if (weaponScript != null)
        {
            weaponScript.owner = this.gameObject;
        }

        // 3. Efekti belirli bir süre sonra yok et
        Destroy(currentSlash, effectLifeTime);
    }
}