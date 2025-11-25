using System.Collections;
using UnityEngine;

public class SpinAttack : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    public float duration = 0.4f;   // Savrulma hýzý (Saniye)
    public float startAngle = -90f; // Baþlangýç açýsý (Sol taraf)
    public float endAngle = 90f;    // Bitiþ açýsý (Sað taraf)

    void Start()
    {
        StartCoroutine(SwingRoutine());

        // Ýþ bitince yok et (Animasyon süresinden azýcýk sonra)
        Destroy(gameObject, duration + 0.1f);
    }

    IEnumerator SwingRoutine()
    {
        float elapsed = 0f;

        // Baþlangýç ve Bitiþ rotasyonlarýný hesapla
        // Sadece Y ekseninde (Yere paralel) döndürüyoruz
        Quaternion startRot = Quaternion.Euler(0, startAngle, 0);
        Quaternion endRot = Quaternion.Euler(0, endAngle, 0);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            // 0 ile 1 arasýnda bir oran bul (Zaman ilerledikçe artar)
            float percent = elapsed / duration;

            // Lerp: Ýki açý arasýnda yumuþak geçiþ yap
            transform.localRotation = Quaternion.Lerp(startRot, endRot, percent);

            yield return null; // Bir sonraki kareyi bekle
        }
    }
}