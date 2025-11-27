using UnityEngine;

public class PlayerTeslaController : MonoBehaviour
{
    [Header("Gerekli Parçalar")]
    public GameObject teslaPrefab; // TeslaBall Prefabý
    public Transform firePoint;    // MagicSpawnPoint kullanabilirsin

    [Header("Ayarlar")]
    public float attackRate = 2.0f; // Güçlü olduðu için yavaþ dolsun
    private float nextAttackTime = 0f;

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
            SpawnTesla();
            nextAttackTime = Time.time + attackRate;
        }
    }

    void SpawnTesla()
    {
        if (teslaPrefab == null) return;

        // Topu oluþtur
        Instantiate(teslaPrefab, firePoint.position, firePoint.rotation);

        // Ses (Varsa elektrik sesi)
        if (AudioManager.instance != null) AudioManager.instance.PlayMagic();
    }
}