using UnityEngine;

public class PlayerChaosController : MonoBehaviour
{
    [Header("Gerekli Parçalar")]
    public GameObject chaosPrefab; // Kaos Býçaklarý Prefabý

    [Header("Ayarlar")]
    public float attackRate = 2.0f; // Güçlü olduðu için yavaþ dolsun
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
            SpawnChaos();
            nextAttackTime = Time.time + attackRate;
        }
    }

    void SpawnChaos()
    {
        if (chaosPrefab == null) return;

        // Karakterin tam merkezinde oluþtur
        Vector3 spawnPos = transform.position + (Vector3.up * 1.0f); // Bel hizasý

        GameObject chaos = Instantiate(chaosPrefab, spawnPos, transform.rotation, transform);

        // Sahiplik Ata (Ýki býçak için de)
        SimpleWeapon[] weapons = chaos.GetComponentsInChildren<SimpleWeapon>();
        foreach (var weapon in weapons)
        {
            weapon.owner = gameObject;
        }

        // Ses (Özel bir zincir sesi varsa onu koy)
        if (AudioManager.instance != null) AudioManager.instance.PlaySlash();
    }
}