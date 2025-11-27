using UnityEngine;

public class PlayerNetController : MonoBehaviour
{
    [Header("Gerekli Parçalar")]
    public GameObject netPrefab;   // Elektrik Topu Prefabý

    [Header("Ayarlar")]
    public float attackRate = 1.5f; // Atýþ hýzý
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
            SpawnNet();
            nextAttackTime = Time.time + attackRate;
        }
    }

    void SpawnNet()
    {
        if (netPrefab == null) return;

        // Karakterin önünde ve bel hizasýnda oluþtur
        Vector3 spawnPos = transform.position + (Vector3.up * 1.0f) + (transform.forward * 0.5f);

        // Topu oluþtur
        Instantiate(netPrefab, spawnPos, transform.rotation);

        // Ses (Varsa)
        if (AudioManager.instance != null) AudioManager.instance.PlayMagic();
    }
}