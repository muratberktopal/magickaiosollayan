using UnityEngine;

public class PlayerBuzzsawController : MonoBehaviour
{
    public GameObject buzzsawPrefab;
    public float attackRate = 3.5f; // Ýþlem uzun sürdüðü için geç dolsun
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
            ThrowBuzzsaw();
            nextAttackTime = Time.time + attackRate;
        }
    }

    void ThrowBuzzsaw()
    {
        if (buzzsawPrefab == null) return;

        // Bel hizasýnda oluþtur
        Vector3 spawnPos = transform.position + (Vector3.up * 1.0f);

        // Fýrlat (Karakterin baktýðý yöne)
        GameObject saw = Instantiate(buzzsawPrefab, spawnPos, transform.rotation);

        // Sahibini ata
        SimpleWeapon weapon = saw.GetComponentInChildren<SimpleWeapon>();
        if (weapon != null) weapon.owner = gameObject;

        // Ses (Testere sesi varsa efsane olur)
        if (AudioManager.instance != null) AudioManager.instance.PlayClub();
    }
}