using UnityEngine;
using UnityEngine.UI;

public class Damageable : MonoBehaviour
{
    public GameObject loot;
    public GameObject loot2;
    public float life = 100;
    public Slider healthBar;
    private Image fillImage;
    public float maxLife;
    string bullet = "bullet";

    void Start()
    {
        maxLife = life;
        healthBar.minValue = 0;
        healthBar.maxValue = maxLife;
        healthBar.value = life;
        fillImage = healthBar.fillRect.GetComponent<Image>();
    }

    void Update()
    {
        healthBar.value = life;

        float percentage = life / maxLife;

        if (percentage > 0.5f)
        {
            fillImage.color = Color.green;
        }
        else if (percentage > 0.25f)
        {
            fillImage.color = Color.yellow;
        }
        else
        {
            fillImage.color = Color.red;
        }
    }

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

    void Die()
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