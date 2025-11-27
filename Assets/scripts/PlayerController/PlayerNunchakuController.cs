using UnityEngine;

public class PlayerNunchakuController : MonoBehaviour
{
    [Header("Gerekli Parçalar")]
    public GameObject nunchakuPrefab;

    [Header("Ayarlar")]
    public float attackRate = 0.4f; // Çok seri atýþ
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
            SpawnNunchaku();
            nextAttackTime = Time.time + attackRate;
        }
    }

    void SpawnNunchaku()
    {
        if (nunchakuPrefab == null) return;

        // Pozisyon: Bel hizasý
        Vector3 spawnPos = transform.position + (Vector3.up * 1.0f);

        // Oluþtur
        GameObject nunchaku = Instantiate(nunchakuPrefab, spawnPos, transform.rotation);

        // Karaktere yapýþtýr (Yürürken seninle gelsin)
        nunchaku.transform.SetParent(transform);

        // Sahibini ata
        SimpleWeapon weapon = nunchaku.GetComponentInChildren<SimpleWeapon>();
        if (weapon != null) weapon.owner = gameObject;

        // Ses (Varsa 'Whoosh' sesi)
        if (AudioManager.instance != null) AudioManager.instance.PlaySlash();
    }
}