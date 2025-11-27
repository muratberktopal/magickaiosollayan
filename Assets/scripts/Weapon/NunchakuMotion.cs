using UnityEngine;

public class NunchakuMotion : MonoBehaviour
{
    public float spinSpeed = 1500f; // Çok hýzlý dönsün
    public float lifeTime = 0.4f;   // Çok kýsa sürsün

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Y ekseninde (Yere paralel) fýrýl fýrýl dön
        transform.Rotate(0, spinSpeed * Time.deltaTime, 0);
    }
}