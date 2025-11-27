using System.Collections;
using UnityEngine;

public class TreeSmash : MonoBehaviour
{
    [Header("Düþüþ Ayarlarý")]
    public float dropHeight = 10f;  // Kaç metre yukarýdan düþsün?
    public float dropSpeed = 40f;   // Ýniþ hýzý
    public float stayTime = 1f;     // Yerde kalma süresi

    [Header("Efektler")]
    public GameObject dustEffect;   // Varsa toz efekti prefabý

    private Vector3 targetPos;      // Hedef (Zemin)
    private bool hasLanded = false;

    void Start()
    {
        // 1. Doðduðu yeri hedef olarak kaydet (Yerde doðuyor çünkü)
        targetPos = transform.position;

        // 2. Kendini havaya ýþýnla
        transform.position += Vector3.up * dropHeight;
    }

    void Update()
    {
        if (!hasLanded)
        {
            // Aþaðý doðru mermi gibi in
            transform.position = Vector3.MoveTowards(transform.position, targetPos, dropSpeed * Time.deltaTime);

            // Yere vardý mý?
            if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            {
                Land();
            }
        }
    }

    void Land()
    {
        hasLanded = true;

        // SES (Varsa çal)
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayClub(); // Tok ses (Sopa sesi uyar)
        }

        // EFEKT (Varsa oluþtur)
        if (dustEffect != null)
        {
            Instantiate(dustEffect, transform.position, Quaternion.identity);
        }

        // Bekle ve Yok Ol
        Destroy(gameObject, stayTime);
    }
}