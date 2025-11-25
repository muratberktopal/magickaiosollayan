using UnityEngine;

public class PlayerWhipController : MonoBehaviour
{
    // "public" yazmazsa Inspector'da göremezsin!
    [Header("Gerekli Parçalar")]
    public GameObject whipPrefab; // <-- Kýrbaç Prefabýný buraya sürükleyeceksin

    [Header("Ayarlar")]
    public float attackRate = 0.7f; // Saldýrý hýzý
    private float nextAttackTime = 0f;

    // Bilgisayarda Space ile test etmek için
    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
#endif
    }

    // Bu fonksiyonu WeaponSelector (Buton) çaðýracak
    public void Attack()
    {
        if (Time.time >= nextAttackTime)
        {
            SpawnWhip();
            nextAttackTime = Time.time + attackRate;
        }
    }

    void SpawnWhip()
    {
        if (whipPrefab == null)
        {
            Debug.LogError("HATA: Whip Prefab kutusu boþ! Player'a týkla ve sürükle.");
            return;
        }

        // 1. POZÝSYON: Karakterin bel hizasýnda (Y=1) oluþtur
        Vector3 spawnPos = transform.position + (Vector3.up * 1.0f);

        // 2. OLUÞTUR: Karakterin baktýðý yönde
        GameObject whip = Instantiate(whipPrefab, spawnPos, transform.rotation);

        // 3. CHILD YAP: Karakter yürürse o da gelsin
        whip.transform.SetParent(transform);

        // 4. SAHÝPLÝK ATA: (Tip/Uç kýsýmdaki hasar scriptini bulur)
        SimpleWeapon weapon = whip.GetComponentInChildren<SimpleWeapon>();
        if (weapon != null)
        {
            weapon.owner = gameObject;
        }

        // 5. SES ÇAL
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayWhip();
        }
    }
}