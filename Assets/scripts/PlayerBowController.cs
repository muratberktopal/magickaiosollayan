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
        // --- HATA KONTROL� ---
        if (arrowPrefab == null)
        {
            Debug.LogError("HATA: 'Arrow Prefab' kutusu bo�! PlayerBowController scriptine ok prefab�n� s�r�klemedin.");
            return;
        }
        // ---------------------

        // Pozisyon: Omuz hizas� ve biraz �n�
        Vector3 spawnPos = transform.position + (Vector3.up * spawnHeight) + (transform.forward * 0.8f);

        // Oku olu�tur
        GameObject arrow = Instantiate(arrowPrefab, spawnPos, transform.rotation);
        Debug.Log("OK OLU�TURULDU!"); // KONTROL 3
        Collider arrowCol = arrow.GetComponent<Collider>();
        if (arrowCol != null) arrowCol.enabled = false; // İlk salise kapalı kalsın
        StartCoroutine(EnableCollider(arrowCol)); // Birazdan aç
        // Oku f�rlat (Script kapal�ysa uyand�r)
        FireballProjectile proj = arrow.GetComponent<FireballProjectile>();
        if (proj != null)
        {
            proj.enabled = true;
        }
        else
        {
            // E�er script yoksa manuel f�rlat (Yedek plan)
            Rigidbody rb = arrow.GetComponent<Rigidbody>();
            
        }

        // Sahibini ata
        SimpleWeapon weapon = arrow.GetComponent<SimpleWeapon>();
        if (weapon != null) weapon.owner = gameObject;

        // Ses
        if (AudioManager.instance != null) AudioManager.instance.PlayMagic();
    }


    System.Collections.IEnumerator EnableCollider(Collider col)
    {
        yield return new WaitForSeconds(0.1f); // 0.1 saniye bekle
        if (col != null) col.enabled = true;   // Sonra aç
    }
}