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

        // Karakterin merkezinden deðil, bel hizasýndan (Y+1) çýkar
        Vector3 spawnPos = transform.position + (Vector3.up * 1.2f) + (transform.forward * 1.0f);

        // Rotasyonu sýfýrla (Karakterin baktýðý yöne baksýn ama eðilmesin)
        Quaternion spawnRot = Quaternion.LookRotation(transform.forward);

        Instantiate(teslaPrefab, spawnPos, spawnRot);

        if (AudioManager.instance != null) AudioManager.instance.PlayMagic();
    }
}