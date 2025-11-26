using System.Collections;
using UnityEngine;

public class ChainMotion : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    public float duration = 0.5f;   // Saldýrý süresi
    public float maxRange = 5f;     // Ne kadar uzaða açýlsýn?
    public float startAngle = -90f; // Sol taraf
    public float endAngle = 90f;    // Sað taraf (Toplam 120)

    [Header("Parçalar")]
    public Transform tipObject;     // Ucundaki býçak
    public LineRenderer chainLine;  // Zincir çizgisi

    void Start()
    {
        // Otomatik bul
        if (chainLine == null) chainLine = GetComponent<LineRenderer>();
        if (tipObject == null) tipObject = transform.GetChild(0);

        StartCoroutine(SweepRoutine());

        // Ýþ bitince yok et
        Destroy(gameObject, duration + 0.1f);
    }

    void Update()
    {
        // Zinciri her karede güncelle
        if (chainLine != null && tipObject != null)
        {
            chainLine.SetPosition(0, Vector3.zero); // Merkez (0,0,0)
            chainLine.SetPosition(1, tipObject.localPosition); // Uç nokta
        }
    }

    IEnumerator SweepRoutine()
    {
        float elapsed = 0f;

        // Baþlangýç ve Bitiþ rotasyonlarý
        Quaternion startRot = Quaternion.Euler(0, startAngle, 0);
        Quaternion endRot = Quaternion.Euler(0, endAngle, 0);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / duration; // 0 ile 1 arasý

            // 1. DÖNME (Yelpaze hareketi)
            transform.localRotation = Quaternion.Lerp(startRot, endRot, percent);

            // 2. UZAMA (Merkezden dýþarý doðru açýlma)
            // Hareketin ortasýnda (%50) en uzun olsun, sonra geri çekilsin veya sabit kalsýn
            // Biz burada lineer uzama yapalým:
            if (tipObject != null)
            {
                // 1 metreden MaxRange'e kadar uzat
                float currentDist = Mathf.Lerp(1f, maxRange, percent);
                tipObject.localPosition = new Vector3(0, 0, currentDist);
            }

            yield return null;
        }
    }
}