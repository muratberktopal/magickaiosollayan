using System.Collections;
using System.Collections.Generic; // List için gerekli
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Temel Ayarlar")]
    public GameObject enemyPrefab;
    public Transform player;
    public float spawnRadius = 15f;

    [Header("Survival Modu")]
    public float spawnRate = 2f;
    private float nextSpawnTime = 0f;

    // --- YENÝ EKLENEN KISIM: BOSS LÝSTESÝ ---
    [System.Serializable]
    public struct BossWave
    {
        public string name;           // Editörde karýþmasýn diye isim
        public float spawnTime;       // Kaçýncý saniyede gelsin? (Örn: 60, 120, 180)
        public GameObject bossPrefab; // Hangi Boss?
        [HideInInspector] public bool spawned; // Doðdu mu kontrolü
    }
    public List<BossWave> bossWaves; // Inspector'dan dolduracaksýn
    // ----------------------------------------

    private int gameMode = 0;
    private int spawnedCount = 0;
    private int maxEnemies = 10;
    private float survivalStartTime; // Oyunun baþlama zamaný

    void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        gameMode = PlayerPrefs.GetInt("GameMode", 0);

        if (gameMode == 0) // BATTLE ROYALE
        {
            if (BattleRoyaleManager.instance != null)
                maxEnemies = BattleRoyaleManager.instance.totalEnemies;
            StartCoroutine(SpawnBattleRoyaleWave());
        }
        else // SURVIVAL
        {
            survivalStartTime = Time.time; // Zamaný baþlat
        }
    }

    void Update()
    {
        if (gameMode == 0) return;

        // --- SURVIVAL MANTIÐI ---
        if (player != null)
        {
            // 1. Normal Düþman Doðumu
            if (Time.time >= nextSpawnTime)
            {
                SpawnEnemy(enemyPrefab); // Normal düþman
                nextSpawnTime = Time.time + spawnRate;
            }

            // 2. BOSS KONTROLÜ (YENÝ)
            CheckBossSpawns();
        }
    }

    void CheckBossSpawns()
    {
        float timeElapsed = Time.time - survivalStartTime;

        // Listeyi kontrol et
        for (int i = 0; i < bossWaves.Count; i++)
        {
            // Zamaný geldiyse VE daha önce doðmadýysa
            if (!bossWaves[i].spawned && timeElapsed >= bossWaves[i].spawnTime)
            {
                // Boss'u oluþtur
                SpawnEnemy(bossWaves[i].bossPrefab);

                // Struct olduðu için listeyi güncellememiz lazým:
                BossWave updateWave = bossWaves[i];
                updateWave.spawned = true;
                bossWaves[i] = updateWave;

                Debug.Log("BOSS GELDÝ: " + updateWave.name);

                // Boss geldiðinde "UYARI" sesi veya efekti ekleyebilirsin
                if (AudioManager.instance != null) AudioManager.instance.PlayLevelUp(); // Þimdilik level sesi çalsýn
            }
        }
    }

    // Fonksiyonu parametre alacak þekilde güncelledim
    void SpawnEnemy(GameObject prefabToSpawn)
    {
        if (prefabToSpawn == null) return;

        Vector2 randomPoint = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = player.position + new Vector3(randomPoint.x, 0, randomPoint.y);
        spawnPos.y = 0.5f;

        Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
    }

    // Eski Battle Royale kodu uyumlu kalsýn diye overload yaptým
    void SpawnEnemy()
    {
        SpawnEnemy(enemyPrefab);
    }

    IEnumerator SpawnBattleRoyaleWave()
    {
        while (spawnedCount < maxEnemies)
        {
            SpawnEnemy();
            spawnedCount++;
            if (spawnedCount >= maxEnemies) yield break;
            yield return new WaitForSeconds(0.2f);
        }
    }
}