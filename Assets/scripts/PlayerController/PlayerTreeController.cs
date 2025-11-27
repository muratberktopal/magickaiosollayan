using UnityEngine;

public class PlayerTreeController : MonoBehaviour
{
    [Header("Gerekli Parçalar")]
    public GameObject treePrefab; // Aðaç Prefabý

    [Header("Ayarlar")]
    public float attackRate = 2.0f;    // Yavaþ ama güçlü
    public float spawnDistance = 3.0f; // Karakterin 3 metre önüne düþsün

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

        // Düþeceði Yer: Karakterin önü + Zemin seviyesi (Y=0 varsayýyoruz)
        Vector3 groundPos = transform.position + (transform.forward * spawnDistance);
        groundPos.y = 0f; // Yere sabitle

        // Aðacý oluþtur (Script onu otomatik havaya kaldýrýp indirecek)
        GameObject tree = Instantiate(treePrefab, groundPos, Quaternion.identity);

        // Sahibini ata (SimpleWeapon çocukta olabilir)
        SimpleWeapon weapon = tree.GetComponentInChildren<SimpleWeapon>();
        if (weapon != null)
        {
            weapon.owner = gameObject;
        }
    }
}