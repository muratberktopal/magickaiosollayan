using UnityEngine;
using System.Collections.Generic; // Listeler için þart

[System.Serializable]
public class BossEvent
{
    public string name;         // Hatýrlatýcý isim (Örn: "Dakika 2 Minotaur")
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

    [Header("3. Elite Düþman Havuzu (YENÝLENMÝÞ)")]
    // Buraya istediðin kadar farklý Elite prefabý ekle
    public List<GameObject> eliteEnemyPool;
    [Range(0, 100)] public float eliteSpawnChance = 5f; // Her spawn'da %5 ihtimalle Elite gelir

    [Header("4. Boss Takvimi")]
    // Zamaný gelince doðacak bosslar listesi
    public List<BossEvent> bossSpawns;

    // Gizli Deðiþkenler
    private float nextSpawnTime = 0f;
    private Transform playerTarget;
    private bool isBattleStarted = false;
    private float survivalTimer = 0f; // Savaþ baþladýðýndan beri geçen süre

    // WeaponSelector bunu çaðýracak
    public void StartBattle()
    {
        isBattleStarted = true;
        survivalTimer = 0f; // Zamaný sýfýrla
    }

    void Update()
    {
        // Savaþ baþlamadýysa dur
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

        // 1. BOSS KONTROLÜ (Listeyi tara)
        CheckBossSpawns();

        // 2. YENÝ DÜÞMAN KÝLÝDÝ AÇMA
        CheckNewEnemies();

        // 3. SPAWN DÖNGÜSÜ
        if (Time.time >= nextSpawnTime)
        {
            SpawnRoutine();

            // Ýstersen zamanla spawn hýzýný artýrabilirsin (Zorluk)
            // spawnRate = Mathf.Max(0.5f, 2f - (survivalTimer / 60f) * 0.1f);

            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnRoutine()
    {
        // Eðer havuz boþsa hata vermesin diye dön
        if (activeEnemyPool.Count == 0) return;

        // Pozisyon Belirle
        Vector2 rnd = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 pos = playerTarget.position + new Vector3(rnd.x, 0, rnd.y);
        pos.y = 0.5f; // Yükseklik

        GameObject prefabToSpawn = null;

        // --- ELITE KONTROLÜ (YENÝ) ---
        // Elite havuzunda eleman varsa VE Zar tutarsa
        if (eliteEnemyPool.Count > 0 && Random.Range(0f, 100f) < eliteSpawnChance)
        {
            // Elite havuzundan rastgele bir tane seç
            int randomEliteIndex = Random.Range(0, eliteEnemyPool.Count);
            prefabToSpawn = eliteEnemyPool[randomEliteIndex];
            // Debug.Log("Elite Düþman Sahneye Ýndi!");
        }
        else
        {
            // --- NORMAL SEÇÝM ---
            // Normal havuzdan rastgele bir düþman seç
            int randomIndex = Random.Range(0, activeEnemyPool.Count);
            prefabToSpawn = activeEnemyPool[randomIndex];
        }

        // Yarat
        if (prefabToSpawn != null)
        {
            Instantiate(prefabToSpawn, pos, Quaternion.identity);
        }
    }

    void CheckBossSpawns()
    {
        // Tüm boss listesini kontrol et
        foreach (var boss in bossSpawns)
        {
            // Eðer daha önce doðmadýysa VE zamaný geldiyse
            if (!boss.spawned && survivalTimer >= boss.spawnTime)
            {
                SpawnBoss(boss.bossPrefab);
                boss.spawned = true; // Tik at, bir daha doðmasýn
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
                Debug.Log("HAVUZA YENÝ DÜÞMAN EKLENDÝ: " + enemyInfo.name);
            }
        }
    }

    void SpawnBoss(GameObject bossPrefab)
    {
        if (bossPrefab == null) return;

        // Boss biraz daha uzakta doðsun (Spawn Radius + 5m)
        Vector2 rnd = Random.insideUnitCircle.normalized * (spawnRadius + 5f);
        Vector3 pos = playerTarget.position + new Vector3(rnd.x, 0, rnd.y);
        pos.y = 0.5f;

        Instantiate(bossPrefab, pos, Quaternion.identity);
    }
}