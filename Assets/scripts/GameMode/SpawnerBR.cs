using UnityEngine;

public class SpawnerBR : MonoBehaviour
{
    [Header("Düþman Havuzu")]
    public GameObject[] enemyPrefabs; // Rastgele seçilecekler
    public GameObject bossPrefab;     // Sonuncu (Boss)

    [Header("Ayarlar")]
    public float spawnRadius = 30f;   // Harita geniþliðine göre ayarla

    // Harita deðiþince MapSwitcher burayý güncelleyecek
    [HideInInspector] public Vector3 currentMapCenter;

    public void StartBattle()
    {
        SpawnAllEnemiesInstant();
    }

    void SpawnAllEnemiesInstant()
    {
        // 1. Sayýyý Manager'dan Al
        int total = 10;
        if (BattleRoyaleManager.instance != null)
            total = BattleRoyaleManager.instance.totalEnemies;

        Debug.Log("BR BAÞLADI! " + total + " düþman sahneye atýlýyor.");

        // 2. Döngü (Bekleme Yok, Hepsini Sýrayla Bas)
        for (int i = 0; i < total; i++)
        {
            GameObject toSpawn = null;

            // --- KÝM DOÐACAK? ---
            // Sonuncu sýradaki BOSS olsun
            if (i == total - 1 && bossPrefab != null)
            {
                toSpawn = bossPrefab;
            }
            // Diðerleri rastgele asker olsun
            else if (enemyPrefabs.Length > 0)
            {
                int randomIndex = Random.Range(0, enemyPrefabs.Length);
                toSpawn = enemyPrefabs[randomIndex];
            }

            // --- DOÐUR ---
            if (toSpawn != null)
            {
                SpawnUnit(toSpawn);
            }
        }
    }

    void SpawnUnit(GameObject prefab)
    {
        // Rastgele konum belirle
        Vector2 rnd = Random.insideUnitCircle * spawnRadius;

        // Eðer MapSwitcher merkezi belirlemediyse (Ýlk açýlýþ), (0,0,0) kabul et
        if (currentMapCenter == Vector3.zero)
        {
            // Güvenlik için Player'ý bulup onun etrafýna da atabilirsin ama
            // Battle Royale'de genelde harita merkezine göre daðýtýlýr.
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) currentMapCenter = p.transform.position;
        }

        Vector3 spawnPos = currentMapCenter + new Vector3(rnd.x, 0, rnd.y);

        // Yükseklik: Havadan býrak (5 metre)
        spawnPos.y = 1f;

        Instantiate(prefab, spawnPos, Quaternion.identity);
    }
}