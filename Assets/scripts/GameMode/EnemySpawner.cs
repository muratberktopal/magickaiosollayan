using System.Collections;
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

    // --- KONTROL DEÐÝÞKENLERÝ ---
    private int gameMode = 0;
    private int spawnedCount = 0; // Kaç tane doðurduk?
    private int maxEnemies = 10;  // BR Limiti

    void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        // Modu Oku
        gameMode = PlayerPrefs.GetInt("GameMode", 0);
        Debug.Log("SPAWNER BAÞLADI. Mod: " + gameMode); // Konsola bak: 0 mý 1 mi?

        if (gameMode == 0) // BATTLE ROYALE
        {
            // Limiti Manager'dan al
            if (BattleRoyaleManager.instance != null)
            {
                maxEnemies = BattleRoyaleManager.instance.totalEnemies;
            }

            Debug.Log("Hedef Düþman Sayýsý: " + maxEnemies);
            StartCoroutine(SpawnBattleRoyaleWave());
        }
    }

    void Update()
    {
        // BATTLE ROYALE ÝSE UPDATE ÇALIÞMASIN (ÇELÝK KAPI)
        if (gameMode == 0) return;

        // Sadece SURVIVAL (Mod 1) ise burasý çalýþýr
        if (player != null)
        {
            if (Time.time >= nextSpawnTime)
            {
                SpawnEnemy();
                nextSpawnTime = Time.time + spawnRate;
            }
        }
    }

    IEnumerator SpawnBattleRoyaleWave()
    {
        // Sayaç Limite ulaþana kadar döngü
        while (spawnedCount < maxEnemies)
        {
            SpawnEnemy();
            spawnedCount++; // Sayacý artýr

            // 10. düþmaný doðurduysak dur
            if (spawnedCount >= maxEnemies)
            {
                Debug.Log("LÝMÝTE ULAÞILDI. Spawner duruyor.");
                yield break; // Döngüyü kýr ve çýk
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null) return;

        Vector2 randomPoint = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = player.position + new Vector3(randomPoint.x, 0, randomPoint.y);
        spawnPos.y = 0.5f;

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}