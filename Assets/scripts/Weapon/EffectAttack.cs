using UnityEngine;

public class EffectAttack : MonoBehaviour
{
    [Header("Gerekli Parçalar")]
    public GameObject slashPrefab;
    public Transform firePoint;

    [Header("Ayarlar")]
    public float attackRate = 0.5f; // Ýsim bu olduðu için UpgradeManager'da bunu kullandýk
    public float effectLifeTime = 0.2f;
    public int damage = 10;
    private float nextAttackTime = 0f;

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PerformAttack();
        }
#endif
    }

    public void PerformAttack()
    {
        // Eski kodunda burasý iki kere yazýlmýþtý, düzelttim.
        if (Time.time >= nextAttackTime)
        {
            SpawnSlash();

            if (AudioManager.instance != null) AudioManager.instance.PlaySlash();

            nextAttackTime = Time.time + attackRate;
        }
    }

    void SpawnSlash()
    {
        if (slashPrefab == null || firePoint == null) return;

        Quaternion rotasyon = Quaternion.Euler(90, transform.eulerAngles.y, 0);
        GameObject currentSlash = Instantiate(slashPrefab, firePoint.position, rotasyon);

        // --- BURAYI GÜNCELLEDÝM ---
        // Oluþan efektin (SimpleWeapon) hasarýný da güncellememiz lazým
        // Yoksa level atlayýnca hasar artar ama vuruþ deðiþmez.

        SimpleWeapon weaponScript = currentSlash.GetComponent<SimpleWeapon>();
        if (weaponScript != null)
        {
            weaponScript.owner = this.gameObject;
            weaponScript.damage = this.damage; // Hasarý aktar!
        }

        Destroy(currentSlash, effectLifeTime);
    }
}