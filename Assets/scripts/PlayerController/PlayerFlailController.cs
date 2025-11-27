using UnityEngine;

public class PlayerFlailController : MonoBehaviour
{
    [Header("Gerekli Parçalar")]
    public GameObject flailPrefab; // Fizik Topu Prefabý
    private GameObject currentFlail; // Sahnedeki topumuz

    // Silah Seçilince Topu Yarat
    void OnEnable()
    {
        if (flailPrefab != null && currentFlail == null)
        {
            // Topu Player'ýn içine (Child) OLUÞTURMA! Dünyaya oluþtur.
            // (Child olursa savrulamaz, karakterle beraber döner)
            Vector3 spawnPos = transform.position - (transform.forward * 2f); // Arkada doðsun

            currentFlail = Instantiate(flailPrefab, spawnPos, Quaternion.identity);

            // Topa "Sahibin benim" de
            FlailPhysics physicsScript = currentFlail.GetComponent<FlailPhysics>();
            if (physicsScript != null)
            {
                physicsScript.target = this.transform; // Hedef benim
            }

            // Hasar sistemine sahibini tanýt
            SimpleWeapon weapon = currentFlail.GetComponent<SimpleWeapon>();
            if (weapon != null) weapon.owner = gameObject;
        }
    }

    // Silah deðiþirse topu yok et
    void OnDisable()
    {
        if (currentFlail != null) Destroy(currentFlail);
    }

    // Bu silahta "Attack" tuþuna basmaya gerek yok, pasif çalýþýr.
    // Ama WeaponSelector hata vermesin diye boþ fonksiyon býrakýyoruz.
    public void Attack()
    {
        // Boþ (Saldýrý otomatiktir)
    }
}