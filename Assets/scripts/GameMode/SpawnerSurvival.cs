using UnityEngine;
using System.Collections.Generic; // Listeler için þart

[System.Serializable]
public class BossEvent
{
    public string name;         // Hatýrlatýcý isim (Örn: "Dakika 5 Bossu")
    public GameObject bossPrefab;
    public float spawnTime;     // Kaçýncý saniyede çýksýn?
    [HideInInspector] public bool spawned = false; // Doðdu mu kontrolü
}

[System.Serializable]
public class TimeBasedEnemy
{
    public string name;
    public GameObject enemyPrefab; // Yeni eklenecek düþman
    public float unlockTime;       // Kaçýncý saniyede havuza eklensin?
    [HideInInspector] public bool added = false;
}

public class SpawnerSurvival : MonoBehaviour
{
    [Header("Temel Ayarlar")]
    public float spawnRadius = 15f;
    public float spawnRate = 2f; // Baþlangýç hýzý

    [Header("1. Standart Düþmanlar (Baþlangýç)")]
    // Oyun baþýnda sadece bu listedekiler doðar
    public List<GameObject> activeEnemyPool = new List<GameObject>();

    [Header("2. Zamanla Açýlan Düþmanlar")]
    // Zamaný gelince yukarýdaki havuza eklenecekler
    public List<TimeBasedEnemy> unlockableEnemies;

    [Header("3. Elite Düþman Ayarý")]
    public GameObject elitePrefab;
    [Range(0, 100)] public float eliteSpawnChance = 5f; // %5 Þans

    [Header("4. Boss Takvimi")]
    // Zamaný gelince doðacak bosslar
    public List<BossEvent> bossSpawns;

    // Gizli Deðiþkenler
    private float nextSpawnTime = 0f;
    private Transform playerTarget;
    private bool isBattleStarted = false;
    private float survivalTimer = 0f; // Savaþ baþladýðýndan beri geçen süre

    public void StartBattle()
    {
        isBattleStarted = true;
        survivalTimer = 0f; // Zamaný sýfýrla
    }

    void Update()
    {
        if (!isBattleStarted) return;

        // Player Bulma
        if (playerTarget == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) playerTarget = p.transform;
            else return;
        }

        // --- ZAMANI ÝLERLET ---
        survivalTimer += Time.deltaTime;

        // 1. BOSS KONTROLÜ
        CheckBossSpawns();

        // 2. YENÝ DÜÞMAN KÝLÝDÝ AÇMA
        CheckNewEnemies();

        // 3. SPAWN DÖNGÜSÜ
        if (Time.time >= nextSpawnTime)
        {
            SpawnRoutine();

            // Ýstersen zamanla spawn hýzýný artýrabilirsin (Zorluk)
            // Örn: Her 60 saniyede spawnRate 0.1 azalsýn (Min 0.5)
            // spawnRate = Mathf.Max(0.5f, 2f - (survivalTimer / 60f) * 0.1f);

            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnRoutine()
    {
        if (activeEnemyPool.Count == 0) return;

        // Pozisyon Belirle
        Vector2 rnd = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 pos = playerTarget.position + new Vector3(rnd.x, 0, rnd.y);
        pos.y = 0.5f;

        GameObject prefabToSpawn = null;

        // --- ELITE KONTROLÜ ---
        // 0 ile 100 arasý zar at, þanstan küçükse Elite doður
        if (elitePrefab != null && Random.Range(0f, 100f) < eliteSpawnChance)
        {
            prefabToSpawn = elitePrefab;
            // Debug.Log("Elite Düþman Doðdu!");
        }
        else
        {
            // --- NORMAL SEÇÝM ---
            // Havuzdan rastgele bir düþman seç
            int randomIndex = Random.Range(0, activeEnemyPool.Count);
            prefabToSpawn = activeEnemyPool[randomIndex];
        }

        if (prefabToSpawn != null)
        {
            Instantiate(prefabToSpawn, pos, Quaternion.identity);
        }
    }

    void CheckBossSpawns()
    {
        foreach (var boss in bossSpawns)
        {
            if (!boss.spawned && survivalTimer >= boss.spawnTime)
            {
                SpawnBoss(boss.bossPrefab);
                boss.spawned = true;
                Debug.Log("BOSS ZAMANI: " + boss.name);
            }
        }
    }

    void CheckNewEnemies()
    {
        foreach (var enemyInfo in unlockableEnemies)
        {
            if (!enemyInfo.added && survivalTimer >= enemyInfo.unlockTime)
            {
                activeEnemyPool.Add(enemyInfo.enemyPrefab);
                enemyInfo.added = true;
                Debug.Log("YENÝ DÜÞMAN EKLENDÝ: " + enemyInfo.name);
            }
        }
    }

    void SpawnBoss(GameObject bossPrefab)
    {
        // Boss biraz daha uzakta doðsun
        Vector2 rnd = Random.insideUnitCircle.normalized * (spawnRadius + 5f);
        Vector3 pos = playerTarget.position + new Vector3(rnd.x, 0, rnd.y);
        pos.y = 0.5f;
        Instantiate(bossPrefab, pos, Quaternion.identity);
    }
}