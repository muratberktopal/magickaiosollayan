using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float lifetime = 0.5f; // Animasyon süresi kadar olsun

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}