using UnityEngine;

public class Damageable : MonoBehaviour
{
    public GameObject loot;
    public GameObject loot2;
    public float life = 100;
    string bullet = "bullet";

    
    void OnCollisionEnter(Collision collision)
    {        
        if (collision.gameObject.tag == bullet)
        {
            Bullet bullet_damage = collision.gameObject.GetComponent<Bullet>();
            life -= bullet_damage.damage;
            Destroy(collision.gameObject);
            if (life <= 0)
            {
                Die();
            }
        }
    }

    void Die ()
    {
        if (loot != null)
        {
            Instantiate(loot, transform.position, Quaternion.identity);
        }
        if (loot2 != null)
        {
            Instantiate(loot2, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
