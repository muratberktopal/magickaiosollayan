using UnityEngine;
public class SelfDestruct : MonoBehaviour
{
    void Start() { Destroy(gameObject, 5f); } // 5 saniye sonra öl
    void OnTriggerEnter(Collider other)
    {
        // Player'a veya Duvara çarparsa yok ol (Boss hariç)
        if (!other.CompareTag("Enemy")) Destroy(gameObject);
    }
}