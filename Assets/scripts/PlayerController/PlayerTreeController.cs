using UnityEngine;

public class PlayerTreeController : MonoBehaviour
{
    [Header("Gerekli Parçalar")]
    public GameObject treePrefab; // Aðaç Prefabý

    [Header("Ayarlar")]
    public float attackRate = 2.0f;
    public float spawnDistance = 4.0f; // Karakterin 4 metre önüne

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
            SpawnTree();
            nextAttackTime = Time.time + attackRate;
        }
    }

    void SpawnTree()
    {
        if (treePrefab == null) return;

        // 1. POZÝSYON: Karakterin Önü
        // Mesafe ayarý (En az 4 metre)
        float finalDistance = Mathf.Max(spawnDistance, 4.0f);
        Vector3 groundPos = transform.position + (transform.forward * finalDistance);
        groundPos.y = 0f; // Yeri hedefle

        // 2. ROTASYON: YATAY YAPMA (Ýþte burasý eksikti!)
        // Karakterin yönünü al + 90 derece öne yatýr
        Quaternion horizontalRot = transform.rotation * Quaternion.Euler(90, 0, 0);

        // 3. OLUÞTUR
        GameObject tree = Instantiate(treePrefab, groundPos, horizontalRot);

        // Sahibini ata
        SimpleWeapon weapon = tree.GetComponentInChildren<SimpleWeapon>();
        if (weapon != null)
        {
            weapon.owner = gameObject;
        }

        // Ses
        if (AudioManager.instance != null) AudioManager.instance.PlayClub();
    }
}