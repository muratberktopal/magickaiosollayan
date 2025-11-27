using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MapSwitcher : MonoBehaviour
{
    [Header("Harita Listesi (S�rayla S�r�kle)")]
    public GameObject[] mapParents; // Map1, Map2, Map3
    public Transform[] startPoints; // Ba�lang�� noktalar�

    [Header("Y�neticiler")]
    public BattleRoyaleManager brManager;
    public GameObject spawnerObject; // Spawner_BR
    public Transform player;

    // Zone (Gaz) de�i�kenini sildik!

    private int currentMapIndex = 0;

    public void GoToNextLevel()
    {
        // 1. �u anki haritay� kapat
        if (currentMapIndex < mapParents.Length)
        {
            mapParents[currentMapIndex].SetActive(false);
        }

        // S�radaki haritaya ge�
        currentMapIndex++;

        // 2. E�ER BA�KA HAR�TA VARSA -> A�
        if (currentMapIndex < mapParents.Length)
        {
            Debug.Log("HAR�TA " + (currentMapIndex + 1) + "'e ge�iliyor...");

            // Yeni haritay� a�
            mapParents[currentMapIndex].SetActive(true);

            // Player'� o haritan�n ba�lang�� noktas�na ���nla
            if (player != null && startPoints[currentMapIndex] != null)
            {
                // Fizi�i kilitle ki ���nlanma bozulmas�n
                Rigidbody rb = player.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                    rb.linearVelocity = Vector3.zero;
                }

                // I��nla
                player.position = startPoints[currentMapIndex].position;

                // Fizi�i a�
                if (rb != null) rb.isKinematic = false;
                Physics.SyncTransforms();
            }

            // S�STEMLER� RESETLE
            if (brManager) brManager.ResetRound();

            // Zone resetleme kodunu sildik!

            // SPAWNER'I G�NCELLE VE RESETLE
            if (spawnerObject != null)
            {
                SpawnerBR spawnerScript = spawnerObject.GetComponent<SpawnerBR>();

                // Yeni haritan�n merkezini Spawner'a bildir
                if (spawnerScript != null && startPoints[currentMapIndex] != null)
                {
                    spawnerScript.currentMapCenter = startPoints[currentMapIndex].position;
                }

                spawnerObject.SetActive(false); // Kapat
                Invoke("ReactivateSpawner", 0.1f); // A� (Resetlensin diye)
            }
        }
        // 3. E�ER HAR�TA B�TT�YSE -> OYUN B�TT�
        else
        {
            Debug.Log("OYUN B�TT�! �AMP�YONSUN!");
            Time.timeScale = 1;
            SceneManager.LoadScene(0); // Men�ye d�n
        }
    }
    // Parantez hatas� buradayd�, art�k fonksiyon burada bitiyor.

    // --- YEN� FONKS�YON DI�ARIDA ---
    void ReactivateSpawner()
    {
        if (spawnerObject != null) spawnerObject.SetActive(true);
    }
}