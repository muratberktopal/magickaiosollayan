using UnityEngine;

public class LootDropper : MonoBehaviour
{
    [Header("Ayarlar")]
    public GameObject xpGemPrefab; // Düþecek olan o mavi taþ (Prefab)
    public int dropCount = 3;      // Kaç tane düþsün?
    public float spread = 1.5f;    // Ne kadar uzaða saçýlsýn?

    // Bu fonksiyonu HealthSystem çaðýracak
    public void DropLoot()
    {
        for (int i = 0; i < dropCount; i++)
        {
            // Düþmanýn olduðu yerden biraz yukarýda ve rastgele etrafýnda pozisyon seç
            Vector3 randomPos = transform.position + new Vector3(
                Random.Range(-spread, spread),
                1f, // Yerden 1 birim yukarýda doðsun
                Random.Range(-spread, spread)
            );

            // Taþý oluþtur
            Instantiate(xpGemPrefab, randomPos, Quaternion.identity);
        }
    }
}