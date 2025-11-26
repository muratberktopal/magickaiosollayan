using System.Collections;
using UnityEngine;

public class SpawnerBR : MonoBehaviour
{
    [Header("Ayarlar")]
    public GameObject enemyPrefab;
    public GameObject bossPrefab;
    public float spawnRadius = 15f;

    // Player'ý gizli tutuyoruz, kod kendi bulacak
    private Transform playerTarget;
    private bool waveStarted = false;

    void OnEnable() // Obje açýldýðý an çalýþýr
    {
        waveStarted = false; // Sýfýrla
        Debug.Log("SPAWNER BR: Aktif oldu, Player aranýyor...");
    }

    void Update()
    {
        // 1. PLAYER BULMA KISMI
        if (playerTarget == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
            {
                playerTarget = p.transform;
                Debug.Log("SPAWNER BR: Player bulundu! Saldýrý baþlýyor.");
            }
            else
            {
                // Player yoksa bekle, hata verme
                return;
            }
        }

        // 2. DALGA BAÞLATMA KISMI
        if (!waveStarted)
        {
            StartCoroutine(SpawnWave());
            waveStarted = true; // Bir daha baþlatma
        }
    }

    IEnumerator SpawnWave()
    {
        int total = 10;
        // Manager varsa sayýyý al, yoksa 10 devam et
        if (BattleRoyaleManager.instance != null)
            total = BattleRoyaleManager.instance.totalEnemies;

        Debug.Log("SPAWNER BR: " + total + " düþman üretilecek.");

        for (int i = 0; i < total; i++)
        {
            // Sonuncu düþman Boss olsun
            GameObject toSpawn = (i == total - 1 && bossPrefab != null) ? bossPrefab : enemyPrefab;

            SpawnUnit(toSpawn);

            yield return new WaitForSeconds(1.5f);
        }

        Debug.Log("SPAWNER BR: Tüm düþmanlar doðdu.");
    }

    void SpawnUnit(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogError("SPAWNER HATASI: Enemy veya Boss prefabý boþ!");
            return;
        }

        Vector2 rnd = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 pos = playerTarget.position + new Vector3(rnd.x, 0, rnd.y);
        pos.y = 0.5f;

        Instantiate(prefab, pos, Quaternion.identity);
    }
}