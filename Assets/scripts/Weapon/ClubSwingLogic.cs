using UnityEngine;

public class ClubSwingLogic : MonoBehaviour
{
    private float rotationSpeed;
    private float totalAngleToSwing; // Toplam kaç derece dönecek?
    private float currentAngleMoved = 0f; // Þu ana kadar ne kadar döndü?
    private bool isSwinging = false;

    // Ayarlarý dýþarýdan alýyoruz
    public void Setup(GameObject owner, float speed, float angle)
    {
        // 1. Karaktere yapýþ
        transform.SetParent(owner.transform);

        // 2. Pozisyonu karakterin içine getir (Sopayý ClubBase ile ötelediðin için sorun yok)
        transform.localPosition = Vector3.up * 1.0f; // Bel hizasý

        // 3. BAÞLANGIÇ AÇISI: Sopayý geriye al (Örn: 120 derece dönecekse, -60'tan baþlasýn)
        // Böylece vuruþ tam karakterin önünde ortalanmýþ olur.
        float startAngle = -angle / 2f;
        transform.localRotation = Quaternion.Euler(0, startAngle, 0);

        rotationSpeed = speed;
        totalAngleToSwing = angle;
        isSwinging = true;
    }

    void Update()
    {
        if (isSwinging)
        {
            // Bu karedeki (frame) dönüþ miktarýný hesapla
            float step = rotationSpeed * Time.deltaTime;

            // Döndür
            transform.Rotate(Vector3.up, step);

            // Ne kadar döndüðümüzü kaydet
            currentAngleMoved += step;

            // 4. BÝTÝÞ KONTROLÜ: Eðer hedeflenen açýyý geçtiysek yok et
            if (currentAngleMoved >= totalAngleToSwing)
            {
                Destroy(gameObject);
            }
        }
    }
}