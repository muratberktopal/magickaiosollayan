using UnityEngine;

public class PlayerIceShardController : MonoBehaviour
{
    [Header("Gerekli Parçalar")]
    public GameObject iceShardPrefab; // Buz Prefabý

    [Header("Ayarlar")]
    public float attackRate = 0.8f;    // Atýþ hýzý
    public float spawnHeight = 1.0f;   // Yerden yükseklik
    public float forwardOffset = 1.0f; // Ne kadar önden çýksýn?
    public float sideSpacing = 0.5f;   // Kýymýklar arasý boþluk (Geniþlik)

    public int damage = 15; // Hasar gücü

    private float nextAttackTime = 0f;

    // PC Testi
    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.Space)) Attack();
#endif
    }

    public void Attack()
    {
        if (Time.time >= nextAttackTime)
        {
            SpawnShards();
            nextAttackTime = Time.time + attackRate;
        }
    }

    void SpawnShards()
    {
        if (iceShardPrefab == null) return;

        // Ses
        if (AudioManager.instance != null) AudioManager.instance.PlayMagic();

        // 3 Tane Oluþturacaðýz: Sol, Orta, Sað
        // -1 (Sol), 0 (Orta), 1 (Sað)
        for (int i = -1; i <= 1; i++)
        {
            // POZÝSYON HESABI (Matematik)
            // Merkez + Yükseklik + Ýleri + (Yana Kaydýrma * i)
            // transform.right = Karakterin sað tarafý demektir.
            Vector3 spawnPos = transform.position
                               + (Vector3.up * spawnHeight)
                               + (transform.forward * forwardOffset)
                               + (transform.right * (i * sideSpacing));

            // Oluþtur (Hepsi dümdüz karþýya baksýn)
            GameObject shard = Instantiate(iceShardPrefab, spawnPos, transform.rotation);

            // Hareketi Baþlat
            ArrowProjectile moveScript = shard.GetComponent<ArrowProjectile>();
            if (moveScript != null) moveScript.enabled = true;

            // Sahibini ve Hasarý Ata
            SimpleWeapon weapon = shard.GetComponent<SimpleWeapon>();
            if (weapon != null)
            {
                weapon.owner = gameObject;
                weapon.damage = this.damage;
            }
        }
    }
}