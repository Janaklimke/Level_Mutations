using UnityEngine;
public class Heal : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.life += 25;

            if (playerHealth.life >= 100)
            {
                playerHealth.life = 100;
            }

            Destroy(gameObject);
        }
    }
}