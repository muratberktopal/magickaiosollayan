using UnityEngine;

public class PlayerCompositeBowController : MonoBehaviour
{
    [Header("Gerekli Parçalar")]
    public GameObject arrowPrefab;     // Ok Prefabý
    public GameObject bowVisualPrefab; // Yay Görseli
    private GameObject currentBowVisual;

    [Header("Ayarlar")]
    public float attackRate = 1.0f;    // Çoklu attýðý için biraz yavaþ olsun
    public float spawnHeight = 1.2f;   // Yükseklik
    public float forwardOffset = 1.0f; // Mesafe

    [Header("Yaylým Ayarlarý")]
    public float spreadAngle = 15f;    // Oklar arasýndaki açý (Derece)

    public int damage = 15; // Tek okun hasarý

    private float nextAttackTime = 0f;

    void OnEnable()
    {
        if (bowVisualPrefab != null && currentBowVisual == null)
        {
            currentBowVisual = Instantiate(bowVisualPrefab, transform.position, transform.rotation, transform);
            currentBowVisual.transform.localPosition = new Vector3(0, 1f, 0.5f);
            currentBowVisual.transform.localRotation = Quaternion.identity;
        }
    }

    void OnDisable()
    {
        if (currentBowVisual != null) Destroy(currentBowVisual);
    }

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
            // 1. ÖN CEPHE (3 OK)
            FireVolley(transform.rotation);

            // 2. ARKA CEPHE (3 OK)
            Quaternion backRotation = transform.rotation * Quaternion.Euler(0, 180, 0);
            FireVolley(backRotation);

            if (AudioManager.instance != null) AudioManager.instance.PlayMagic();

            nextAttackTime = Time.time + attackRate;
        }
    }

    // Yaylým ateþi yapan fonksiyon
    void FireVolley(Quaternion baseRotation)
    {
        // Döngü: -1 (Sol), 0 (Orta), 1 (Sað)
        for (int i = -1; i <= 1; i++)
        {
            // Açýyý hesapla (Örn: -15, 0, 15)
            float currentAngle = i * spreadAngle;

            // Ana yöne bu açýyý ekle
            Quaternion arrowRot = baseRotation * Quaternion.Euler(0, currentAngle, 0);

            CreateArrow(arrowRot);
        }
    }

    void CreateArrow(Quaternion rotationDir)
    {
        if (arrowPrefab == null) return;

        // Pozisyonu hesapla (Açýlý yöne doðru ileri git)
        Vector3 spawnPos = transform.position + (Vector3.up * spawnHeight) + (rotationDir * Vector3.forward * forwardOffset);

        GameObject arrow = Instantiate(arrowPrefab, spawnPos, rotationDir);

        ArrowProjectile arrowScript = arrow.GetComponent<ArrowProjectile>();
        if (arrowScript != null) arrowScript.enabled = true;

        SimpleWeapon weapon = arrow.GetComponent<SimpleWeapon>();
        if (weapon != null)
        {
            weapon.owner = gameObject;
            weapon.damage = this.damage;
        }
    }
}