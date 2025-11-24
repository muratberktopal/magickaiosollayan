using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro textMesh;
    private float disappearTimer = 1f; // 1 saniye sonra yok olsun
    private Color textColor;
    private Vector3 moveVector;

    // Bu fonksiyonu yazý oluþurken çaðýracaðýz
    public void Setup(int damageAmount)
    {
        textMesh = GetComponent<TextMeshPro>();
        textMesh.text = damageAmount.ToString();

        textColor = textMesh.color;

        // Yukarý ve hafif saða/sola rastgele hareket etsin
        moveVector = new Vector3(Random.Range(-1f, 1f), 5f, 0) * 2f;
    }

    void Update()
    {
        // 1. HAREKET (Yukarý)
        transform.position += moveVector * Time.deltaTime;

        // Hareketi yavaþlat (Sürtünme etkisi)
        moveVector -= moveVector * 3f * Time.deltaTime;

        // 2. SÜRE VE SOLMA (Fade Out)
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            // Rengi þeffaflaþtýr
            float fadeSpeed = 3f;
            textColor.a -= fadeSpeed * Time.deltaTime;
            textMesh.color = textColor;

            // Tamamen görünmez olunca yok et
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    // Yazýnýn hep kameraya bakmasý için (Billboard)
    void LateUpdate()
    {
        if (Camera.main != null)
            transform.forward = Camera.main.transform.forward;
    }
}