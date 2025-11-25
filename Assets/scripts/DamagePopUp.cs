using UnityEngine;
using TMPro; // TextMeshPro kütüphanesi þart

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro textMesh;
    private float disappearTimer = 1f; // 1 saniye yaþasýn
    private Color textColor;
    private Vector3 moveVector;

    public void Setup(int damageAmount)
    {
        textMesh = GetComponent<TextMeshPro>();
        textMesh.text = damageAmount.ToString();

        textColor = textMesh.color;

        // Yukarý ve hafif saða/sola rastgele fýrlasýn (Dinamik dursun)
        moveVector = new Vector3(Random.Range(-1f, 1f), 5f, 0) * 3f;
    }

    void Update()
    {
        // 1. YUKARI HAREKET
        transform.position += moveVector * Time.deltaTime;

        // Hareketi yavaþlat (Yerçekimi varmýþ gibi)
        moveVector -= moveVector * 8f * Time.deltaTime;

        // 2. YAVAÞÇA KAYBOLMA
        if (disappearTimer > 0.5f) // Ýlk yarým saniye net görünsün
        {
            disappearTimer -= Time.deltaTime;
        }
        else // Sonra solmaya baþlasýn
        {
            disappearTimer -= Time.deltaTime;
            textColor.a -= 3f * Time.deltaTime; // Alfa (Þeffaflýk) azalt
            textMesh.color = textColor;

            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    // Yazý hep kameraya baksýn
    void LateUpdate()
    {
        if (Camera.main != null)
            transform.forward = Camera.main.transform.forward;
    }
}