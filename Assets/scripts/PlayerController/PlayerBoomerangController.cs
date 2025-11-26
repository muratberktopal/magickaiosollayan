using UnityEngine;

public class PlayerBoomerangController : MonoBehaviour
{
    public GameObject boomerangPrefab; // Bumerang Prefabý
    public float attackRate = 1.2f;    // Atýþ sýklýðý
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
            ThrowBoomerang();
            nextAttackTime = Time.time + attackRate;
        }
    }

    void ThrowBoomerang()
    {
        if (boomerangPrefab == null) return;

        // Bel hizasýnda oluþtur
        Vector3 spawnPos = transform.position + (Vector3.up * 1.0f);

        // Bumerangý oluþtur
        GameObject boomerang = Instantiate(boomerangPrefab, spawnPos, transform.rotation);

        // Sahibini ata (Çok önemli, yoksa geri dönemez!)
        SimpleWeapon weapon = boomerang.GetComponent<SimpleWeapon>();
        if (weapon != null) weapon.owner = gameObject;

        // Ses (Varsa)
        if (AudioManager.instance != null) AudioManager.instance.PlaySlash(); // Þimdilik Slash sesi
    }
}