using UnityEngine;

public class PlayerDoubleSwordController : MonoBehaviour
{
    [Header("Gerekli Parçalar")]
    public GameObject doubleSwordPrefab; // Prefab

    [Header("Ayarlar")]
    public float attackRate = 4.0f;    // Soðuma süresi (Uzun sürdüðü için)
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
            SpawnSwords();
            nextAttackTime = Time.time + attackRate;
        }
    }

    void SpawnSwords()
    {
        if (doubleSwordPrefab == null) return;

        // Bel hizasýnda oluþtur
        Vector3 spawnPos = transform.position + (Vector3.up * 1.0f);

        // Oluþtur ve Karakterin içine (Child) at
        // Böylece karakter yürürse pervane de onunla gelir
        GameObject swords = Instantiate(doubleSwordPrefab, spawnPos, transform.rotation, transform);

        // Sahibini ata (Bütün býçaklara)
        SimpleWeapon[] weapons = swords.GetComponentsInChildren<SimpleWeapon>();
        foreach (var weapon in weapons)
        {
            weapon.owner = gameObject;
        }

        // Ses (Sürekli dönen bir výnlama sesi varsa süper olur)
        if (AudioManager.instance != null) AudioManager.instance.PlaySlash();
    }
}