using UnityEngine;

public class PlayerMagicCaster : MonoBehaviour
{
    [Header("Gerekli Par�alar")]
    public GameObject magicballPrefab; // Mavi B�y� Prefab� (Buraya s�r�kle)
    public Transform magicSpawnPoint;  // ��k�� Noktas� (Buraya s�r�kle)

    [Header("Ayarlar")]
    public float cooldownTime = 1f;    // At�� h�z�
    private float nextCastTime = 0f;

    // Bilgisayarda test ederken Space tu�uyla �al��mas� i�in
    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
#endif
    }

    // Bu fonksiyonu hem Space tu�u hem de Telefondaki Buton kullan�r
    public void Attack()
    {
        // Zaman kontrol� (Cooldown)
        if (Time.time >= nextCastTime)
        {
            CastMagic();
            nextCastTime = Time.time + cooldownTime;
        }
    }

    void CastMagic()
    {
        // 1. G�VENL�K KONTROL�
        if (magicballPrefab == null)
        {
            Debug.LogError("HATA: PlayerMagicCaster scriptinde 'Magicball Prefab' kutusu bo�!");
            return;
        }
        if (magicSpawnPoint == null)
        {
            Debug.LogError("HATA: PlayerMagicCaster scriptinde 'Spawn Point' kutusu bo�!");
            return;
        }

        // 2. OLU�TURMA (Spawn)
        GameObject magic = Instantiate(magicballPrefab, magicSpawnPoint.position, Quaternion.identity);

        // 3. Y�N AYARI (Karakterin bakt��� y�ne �evir)
        magic.transform.forward = transform.forward;

        // 4. SCRIPT UYANDIRMA (Topun �zerindeki script kapal�ysa zorla a�)
        FireballProjectile projScript = magic.GetComponent<FireballProjectile>();
        if (projScript != null)
        {
            projScript.enabled = true;
        }
        else
        {
            // Script yoksa manuel hareket ekle (Yedek plan)
            Rigidbody rb = magic.GetComponent<Rigidbody>();
            if (rb != null) rb.linearVelocity = transform.forward * 20f;
        }

        // 5. SAH�PL�K (Bana vurmas�n)
        SimpleWeapon weapon = magic.GetComponent<SimpleWeapon>();
        if (weapon != null) weapon.owner = gameObject;

        if (AudioManager.instance != null) AudioManager.instance.PlayMagic();
    }
}