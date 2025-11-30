using System.Collections;
using UnityEngine;

public class PlayerExcaliburController : MonoBehaviour
{
    [Header("Gerekli Parçalar")]
    public GameObject greatswordPrefab; // Kýlýç Prefabý

    [Header("Ayarlar")]
    public float duration = 5f;     // Ekranda kalma süresi
    public float attackRate = 6f;   // Soðuma süresi
    public float spawnDistance = 1.5f;

    [Header("Nerf (Yavaþlatma) Ayarlarý")]
    public float slowSpeed = 2f;    // Kýlýç varken hýzýn kaça düþsün?

    private float nextAttackTime = 0f;
    private GameObject currentSword;
    private PlayerMovement playerMovement; // Hýzýna eriþmek için
    private float originalSpeed; // Eski hýzýný hafýzada tutmak için

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.Space)) Attack();
#endif
    }

    public void Attack()
    {
        // Eðer zaten kýlýç varsa tekrar basýlmasýn
        if (currentSword != null) return;

        if (Time.time >= nextAttackTime)
        {
            // Ýþlemi baþlat (Coroutine)
            StartCoroutine(PerformGreatswordAttack());
            nextAttackTime = Time.time + attackRate;
        }
    }

    // --- YENÝ SÝSTEM: ZAMANLAYICI FONKSÝYON ---
    IEnumerator PerformGreatswordAttack()
    {
        if (greatswordPrefab == null) yield break;

        // 1. HIZI DÜÞÜR (NERF)
        if (playerMovement != null)
        {
            originalSpeed = playerMovement.moveSpeed; // Þu anki hýzý kaydet
            playerMovement.moveSpeed = slowSpeed;     // Hýzý düþür (Örn: 2)
        }

        // 2. KILICI OLUÞTUR
        Vector3 spawnPos = transform.position + (transform.forward * spawnDistance) + (Vector3.up * 1f);
        currentSword = Instantiate(greatswordPrefab, spawnPos, transform.rotation);

        // Yatay döndür ve karaktere yapýþtýr
        currentSword.transform.Rotate(90, 0, 0);
        currentSword.transform.SetParent(transform);

        // Sahibini ata
        SimpleWeapon weapon = currentSword.GetComponent<SimpleWeapon>();
        if (weapon != null) weapon.owner = gameObject;

        // Ses
        if (AudioManager.instance != null) AudioManager.instance.PlaySlash();

        // 3. BEKLE (Kýlýç süresi kadar)
        yield return new WaitForSeconds(duration);

        // 4. BÝTÝR (Kýlýcý yok et ve Hýzý düzelt)
        if (currentSword != null) Destroy(currentSword);

        if (playerMovement != null)
        {
            playerMovement.moveSpeed = originalSpeed; // Hýzý eski haline getir
        }
    }
}