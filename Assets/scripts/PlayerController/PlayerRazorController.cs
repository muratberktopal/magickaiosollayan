using UnityEngine;

public class PlayerRazorController : MonoBehaviour
{
    public GameObject razorPrefab;
    public float attackRate = 5.0f; // Süresi uzun olsun (Stratejik silah)
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
            ThrowRazor();
            nextAttackTime = Time.time + attackRate;
        }
    }

    void ThrowRazor()
    {
        if (razorPrefab == null) return;

        Vector3 spawnPos = transform.position + (Vector3.up * 1.0f);

        GameObject razor = Instantiate(razorPrefab, spawnPos, transform.rotation);

        // Sahibini ata (Hem kýlýca hem aradaki ipe)
        SimpleWeapon[] weapons = razor.GetComponentsInChildren<SimpleWeapon>();
        foreach (var w in weapons)
        {
            w.owner = gameObject;
        }

        if (AudioManager.instance != null) AudioManager.instance.PlaySlash(); // Keskin ses
    }
}