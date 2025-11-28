using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    public Transform player;

    void LateUpdate()
    {
        // Player'ý otomatik bul (Eðer atamazsan)
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        if (player != null)
        {
            // Kameranýn Yüksekliðini (Y) koru, X ve Z'de Player'a git
            Vector3 newPos = player.position;
            newPos.y = transform.position.y; // Kendi yüksekliðinde kal
            transform.position = newPos;

            // (Ýsteðe Baðlý) Harita karakterle dönmesin istiyorsan:
            // transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
    }
}