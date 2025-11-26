using System.Collections;
using UnityEngine;

public class ChaosBladeSkill : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    public float duration = 1.0f;   // Tam turu ne kadar sürede tamamlasýn?
    public float expandSpeed = 10f; // Dýþarý açýlma hýzý
    public float maxRadius = 6f;    // Menzil

    [Header("Parçalar (Buralarý Elle Doldur!)")]
    public Transform leftBladeObj;
    public Transform rightBladeObj;
    public LineRenderer leftChain;
    public LineRenderer rightChain;

    void Start()
    {
        // Zincir ayarý
        if (leftChain) leftChain.positionCount = 2;
        if (rightChain) rightChain.positionCount = 2;

        // Dönme iþlemini baþlat
        StartCoroutine(SpinRoutine());

        // Ýþ bitince yok et (Süreden biraz sonra)
        Destroy(gameObject, duration + 0.2f);
    }

    IEnumerator SpinRoutine()
    {
        float elapsed = 0f;

        // Baþlangýç açýsý (Mevcut açý)
        Quaternion startRot = transform.rotation;

        // Bitiþ açýsý (360 derece sonrasý)
        // Not: Unity'de tam 360 dereceye Lerp yapmak bazen 0'a dönmek gibi algýlanýr.
        // O yüzden manuel olarak açýyý artýrarak döndüreceðiz.

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / duration; // 0 ile 1 arasý oran

            // --- 1. DÖNME (360 Derece) ---
            // Lerp yerine doðrudan açýyý hesaplýyoruz (Daha garantidir)
            float currentAngle = Mathf.Lerp(0, 360, percent);
            transform.localRotation = Quaternion.Euler(0, currentAngle, 0);

            // --- 2. GENÝÞLEME (Spiral) ---
            if (leftBladeObj != null && leftBladeObj.localPosition.z < maxRadius)
            {
                // Býçaklarý ileri it (Z ekseninde)
                // Lerp ile yumuþakça sona kadar itiyoruz
                float currentDist = Mathf.Lerp(0, maxRadius, percent);

                // Sol Býçaðý Pozisyonla
                leftBladeObj.localPosition = new Vector3(leftBladeObj.localPosition.x, 0, currentDist);

                // Sað Býçaðý Pozisyonla (O da ileri gidiyor ama kendi ekseninde)
                rightBladeObj.localPosition = new Vector3(rightBladeObj.localPosition.x, 0, currentDist);
            }

            // --- 3. ZÝNCÝRLERÝ GÜNCELLE ---
            UpdateChains();

            yield return null;
        }
    }

    void UpdateChains()
    {
        if (leftChain != null && leftBladeObj != null)
        {
            leftChain.SetPosition(0, transform.position);
            leftChain.SetPosition(1, leftBladeObj.position);
        }

        if (rightChain != null && rightBladeObj != null)
        {
            rightChain.SetPosition(0, transform.position);
            rightChain.SetPosition(1, rightBladeObj.position);
        }
    }
}