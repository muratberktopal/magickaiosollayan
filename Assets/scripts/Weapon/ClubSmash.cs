using System.Collections;
using UnityEngine;

public class ClubSmash : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    public float smashSpeed = 10f;   // Ýndirme hýzý
    public float targetAngle = 90f;  // Kaç dereceye insin? (90 = Yere paralel)

    [Header("Efektler")]
    public GameObject explosionVfx;  // Yere vurunca çýkacak efekt
    public Transform hitPoint;       // Efektin çýkacaðý nokta (Sopanýn ucu)

    private bool hasHit = false;

    void Start()
    {
        // Vuruþ hareketini baþlat
        StartCoroutine(SmashRoutine());

        // Güvenlik için 1 saniye sonra yok ol (Eðer Destroy(club) yoksa)
        Destroy(gameObject, 1f);
    }

    IEnumerator SmashRoutine()
    {
        // Hedef açýya ulaþana kadar döndür
        while (transform.localEulerAngles.x < targetAngle)
        {
            // X ekseninde döndür (Aþaðý indir)
            transform.Rotate(Vector3.right * smashSpeed * Time.deltaTime * 100);
            yield return null;
        }

        // --- YERE ÇARPTIÐI AN ---
        if (!hasHit)
        {
            hasHit = true;
            SpawnExplosion();

            // Ýstersen kamera titretme (Camera Shake) kodunu buraya çaðýrabilirsin
        }
    }

    void SpawnExplosion()
    {
        if (explosionVfx != null)
        {
            // Efekti sopanýn ucunda (veya merkezinde) oluþtur
            Vector3 spawnPos = transform.position;

            // Eðer özel bir uç noktasý belirlediysek orada oluþtur
            if (hitPoint != null) spawnPos = hitPoint.position;

            // Efekti yere paralel oluþtur (Quaternion.Euler(90, 0, 0))
            Instantiate(explosionVfx, spawnPos, Quaternion.identity);
        }
    }
}