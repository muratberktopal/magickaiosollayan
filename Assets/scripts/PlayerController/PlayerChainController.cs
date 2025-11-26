using UnityEngine;

public class PlayerChainController : MonoBehaviour
{
    [Header("Gerekli Parçalar")]
    public GameObject chainPrefab; // Zincir Prefabý

    [Header("Ayarlar")]
    public float attackRate = 1.0f;
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
            SpawnChain();
            nextAttackTime = Time.time + attackRate;
        }
    }

    void SpawnChain()
    {
        if (chainPrefab == null) return;

        // Bel hizasýnda oluþtur
        Vector3 spawnPos = transform.position + (Vector3.up * 1.0f);

        // Karakterin içine (Child) oluþtur ki karakterle dönsün
        GameObject chain = Instantiate(chainPrefab, spawnPos, transform.rotation, transform);

        // Sahibini ata
        SimpleWeapon weapon = chain.GetComponentInChildren<SimpleWeapon>();
        if (weapon != null) weapon.owner = gameObject;

        // Ses (Zincir/Metal sesi varsa süper olur)
        if (AudioManager.instance != null) AudioManager.instance.PlaySlash();
    }
}