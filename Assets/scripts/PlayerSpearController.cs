using UnityEngine;

public class PlayerSpearController : MonoBehaviour
{
    [Header("Gerekli Parçalar")]
    public GameObject spearPrefab; // Oluþturacaðýmýz Mýzrak

    [Header("Saldýrý Ayarlarý")]
    public float attackRate = 0.8f; // Saldýrý hýzý
    public float spawnHeight = 1.0f; // Yerden ne kadar yüksekte çýksýn? (Bel hizasý)
    public float forwardOffset = 0.5f; // Karakterin ne kadar önünde çýksýn?

    private float nextAttackTime = 0f;

    // Butonla çaðýrmak için
    public void Attack()
    {
        if (Time.time >= nextAttackTime)
        {
            SpawnSpear();
            nextAttackTime = Time.time + attackRate;
        }
    }

    // Bilgisayarda Space ile denemek için
    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
#endif
    }

    void SpawnSpear()
    {
        if (spearPrefab == null) return;

        // --- MATEMATÝKSEL POZÝSYON HESABI ---
        // Karakterin olduðu yer + Biraz Yukarý + Biraz Ýleri
        Vector3 spawnPos = transform.position + (Vector3.up * spawnHeight) + (transform.forward * forwardOffset);

        // Mýzraðý oluþtur (Rotasyon karakterle ayný olsun)
        GameObject currentSpear = Instantiate(spearPrefab, spawnPos, transform.rotation);

        // Mýzraða sahibini tanýt
        SpearLogic logic = currentSpear.GetComponent<SpearLogic>();
        if (logic != null)
        {
            logic.owner = this.gameObject;
        }
    }
}