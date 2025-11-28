using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 25; // Ne kadar can doldursun?

    void OnTriggerEnter(Collider other)
    {
        // Sadece Player toplayabilir
        if (other.CompareTag("Player"))
        {
            HealthSystem playerHealth = other.GetComponent<HealthSystem>();

            if (playerHealth != null)
            {
                // Caný doluysa alma (Ýsteðe baðlý, þimdilik alalým)
                if (playerHealth.currentHealth < playerHealth.maxHealth)
                {
                    playerHealth.Heal(healAmount);

                    // Ses (Varsa)
                    // if(AudioManager.instance) AudioManager.instance.PlayHeal();

                    Destroy(gameObject); // Ýksiri yok et
                }
            }
        }
    }
}