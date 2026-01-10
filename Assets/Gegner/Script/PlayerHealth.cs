using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxLife = 100;
    public float life = 100;
    
    public GameObject loot;
    
    void Start()
    {
        life = maxLife;
    }

    void OnCollisionEnter(Collision collision)
    {        
        if (collision.gameObject.CompareTag("bullet"))
        {
            Bullet bulletScript = collision.gameObject.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                TakeDamage(bulletScript.damage);
                Destroy(collision.gameObject);
            }
        }
    }
    
    public void TakeDamage(float damage)
    {
        life -= damage;
        Debug.Log("Spieler nimmt " + damage + " Schaden! Leben übrig: " + life);
        
        if (life <= 0)
        {
            Die();
        }
    }
    
    // Für Heilung (optional)
    public void Heal(float amount)
    {
        life += amount;
        if (life > maxLife)
        {
            life = maxLife;
        }
        Debug.Log("Spieler heilt " + amount + "! Leben jetzt: " + life);
    }
    
    void Die()
    {
        Debug.Log("Spieler ist gestorben!");
        
        if (loot != null)
        {
            Instantiate(loot, transform.position, Quaternion.identity);
        }
        
        Destroy(gameObject);
    }
}
