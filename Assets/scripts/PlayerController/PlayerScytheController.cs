using UnityEngine;

public class PlayerScytheController : MonoBehaviour
{
    public GameObject scythePrefab;
    public float attackRate = 0.7f;
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
            SpawnScythe();
            nextAttackTime = Time.time + attackRate;
        }
    }

    void SpawnScythe()
    {
        if (scythePrefab == null) return;

        // Bel hizasýnda oluþtur
        Vector3 spawnPos = transform.position + (Vector3.up * 1.0f);

        // Týrpaný Player'ýn çocuðu olarak oluþtur
        GameObject scythe = Instantiate(scythePrefab, spawnPos, transform.rotation, transform);

        // Sahibini ata
        SimpleWeapon weapon = scythe.GetComponentInChildren<SimpleWeapon>();
        if (weapon != null) weapon.owner = gameObject;

        // Ses
        if (AudioManager.instance != null) AudioManager.instance.PlayScythe();
    }
}