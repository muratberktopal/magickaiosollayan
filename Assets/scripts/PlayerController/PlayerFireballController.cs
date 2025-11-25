using UnityEngine;

public class PlayerFireballController : MonoBehaviour
{
    [Header("Gerekli Parçalar")]
    public GameObject fireballPrefab; // Ateþ Topu Prefabý
    public Transform firePoint;       // Çýkýþ Noktasý (Mevcut MagicSpawnPoint'i kullanabilirsin)

    [Header("Ayarlar")]
    public float attackRate = 1.2f;   // Biraz yavaþ atýlsýn (Güçlü olduðu için)
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
            SpawnFireball();
            nextAttackTime = Time.time + attackRate;
        }
    }

    void SpawnFireball()
    {
        if (fireballPrefab == null) return;

        // Oluþtur
        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);

        // Sahibini ata
        SimpleWeapon weapon = fireball.GetComponent<SimpleWeapon>();
        if (weapon != null) weapon.owner = gameObject;

        // Ses (Buna özel bir patlama sesi eklersen süper olur)
        if (AudioManager.instance != null) AudioManager.instance.PlayMagic();
    }
}