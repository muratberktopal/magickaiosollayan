using UnityEngine;

public class PlayerBowController : MonoBehaviour
{
    [Header("Gerekli Par�alar")]
    public GameObject arrowPrefab; // Ok Prefab� (Buraya S�r�kle!)
    public GameObject bowVisualPrefab; // Yay G�rseli
    private GameObject currentBowVisual;

    [Header("Ayarlar")]
    public float attackRate = 0.5f;
    public float spawnHeight = 1.2f;

    public int damage = 15;
    public float fireRate = 1.0f; // DİKKAT: Yay için 'fireRate' dedik


    private float nextAttackTime = 0f;

    // Script A��l�nca (Yay Se�ilince)
    void OnEnable()
    {
        Debug.Log("YAY MODU AKT�F ED�LD�!"); // KONTROL 1

        if (bowVisualPrefab != null && currentBowVisual == null)
        {
            currentBowVisual = Instantiate(bowVisualPrefab, transform.position, transform.rotation, transform);
            currentBowVisual.transform.localPosition = new Vector3(0, 1f, 0.5f);
        }
    }

    void OnDisable()
    {
        if (currentBowVisual != null) Destroy(currentBowVisual);
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.Space)) Attack();
#endif
    }

    public void Attack()
    {
        // KONTROL 1: Teti�e bas�ld� m�?
        Debug.Log("Teti�e bas�ld�. �u anki Zaman: " + Time.time + " | Beklenen Zaman: " + nextAttackTime);

        if (Time.time >= nextAttackTime)
        {
            FireArrow();
            nextAttackTime = Time.time + attackRate;
            Debug.Log("ATE� ED�LD�! Bir sonraki at�� zaman�: " + nextAttackTime);
        }
        else
        {
            Debug.LogWarning("S�LAH SO�UYOR! Beklemen laz�m.");
        }
    }

    void FireArrow()
    {
        if (arrowPrefab == null) return;

        Vector3 spawnPos = transform.position + (Vector3.up * spawnHeight) + (transform.forward * 0.8f);

        // Oku oluştur
        GameObject arrow = Instantiate(arrowPrefab, spawnPos, transform.rotation);

        // --- DEĞİŞEN KISIM BURASI ---
        // Artık Fireball değil ArrowProjectile arıyoruz
        ArrowProjectile arrowScript = arrow.GetComponent<ArrowProjectile>();
        if (arrowScript != null)
        {
            arrowScript.enabled = true; // Zorla çalıştır
        }
        // ----------------------------

        // Sahibini ata
        SimpleWeapon weapon = arrow.GetComponent<SimpleWeapon>();
        if (weapon != null) weapon.owner = gameObject;

        if (AudioManager.instance != null) AudioManager.instance.PlayMagic(); // Veya PlayBow yaparsan onu çağır
    }


    System.Collections.IEnumerator EnableCollider(Collider col)
    {
        yield return new WaitForSeconds(0.1f); // 0.1 saniye bekle
        if (col != null) col.enabled = true;   // Sonra aç
    }
}