using UnityEngine;

public class DoubleSpinMotion : MonoBehaviour
{
    [Header("Ayarlar")]
    public float spinSpeed = 720f; // Dönme hýzý (Hýzlý olsun)
    public float duration = 2.0f;  // Kaç saniye dönsün?

    void Start()
    {
        // Süre bitince yok et
        Destroy(gameObject, duration);
    }

    void Update()
    {
        // Y ekseninde (Yere paralel) sürekli dön
        transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
    }
}