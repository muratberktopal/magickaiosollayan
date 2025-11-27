using UnityEngine;

public class ProjectileMover : MonoBehaviour
{
    public float speed = 15f; // Mermi Hýzý

    void Update()
    {
        // Her karede kendi baktýðý yöne (ileri) git
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}