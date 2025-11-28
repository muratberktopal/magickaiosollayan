using UnityEngine;

public class HealthDropper : MonoBehaviour
{
    [Header("Ayarlar")]
    public GameObject healthPotionPrefab; // Kýrmýzý iksir prefabý
    [Range(0, 100)] public float dropChance = 10f; // %10 Þansla düþsün

    // Bu fonksiyonu HealthSystem çaðýracak
    public void CheckDrop()
    {
        // 0 ile 100 arasýnda sayý tut. Eðer þans deðerinden küçükse düþür.
        if (Random.Range(0f, 100f) <= dropChance)
        {
            if (healthPotionPrefab != null)
            {
                // Yere gömülmemesi için hafif yukarýdan (Y+0.5) oluþtur
                Vector3 spawnPos = transform.position + Vector3.up * 0.5f;
                Instantiate(healthPotionPrefab, spawnPos, Quaternion.identity);
            }
        }
    }
}