using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float maxLife = 100;
    public float life = 100;
    
    public GameObject loot;
    public GameObject Camera;
    
    void Start()
    {
        Camera.gameObject.SetActive(false);
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
            life = 0;
            Die();
        }
    }

    void Die()
    {
        Camera.gameObject.SetActive(true);

        if (loot != null)
        {
            Instantiate(loot, transform.position, Quaternion.identity);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
