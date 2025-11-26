using UnityEngine;

public class SpawnerSurvival : MonoBehaviour
{
    public GameObject survivalEnemyPrefab;
    public float spawnRadius = 15f;
    public float spawnRate = 2f;

    // Ýþte uyarý veren o deðiþken (Artýk kullanýyoruz)
    private float nextSpawnTime = 0f;

    private Transform playerTarget;

    void Update()
    {
        // 1. PLAYER BULMA
        if (playerTarget == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) playerTarget = p.transform;
            else return;
        }

        // 2. ZAMANLAMA KONTROLÜ (Deðiþkeni burada kullanýyoruz!)
        // Þu anki zaman, belirlediðimiz "Sonraki Doðum Zamaný"ný geçtiyse:
        if (Time.time >= nextSpawnTime)
        {
            Spawn();

            // Bir sonraki doðum zamanýný ileriye at
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void Spawn()
    {
        Vector2 rnd = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 pos = playerTarget.position + new Vector3(rnd.x, 0, rnd.y);
        pos.y = 0.5f;
        Instantiate(survivalEnemyPrefab, pos, Quaternion.identity);
    }
}