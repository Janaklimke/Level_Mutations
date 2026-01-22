using UnityEngine;
using TMPro;

public class WeapponUI : MonoBehaviour
{
    public Weappon weappon;
    public PlayerHealth playerHealth;
    
    public TMP_Text ammoText;
    public TMP_Text healthText;

    void Update()
    {
        UpdateAmmoUI();
        UpdateHealthUI();
    }

    void UpdateAmmoUI()
    {
        if (weappon == null) return;
        
        if (weappon.isReloading)
        {
            ammoText.text = "Reloading...";
        }
        else
        {
            ammoText.text = "Ammo: " + weappon.CurrentAmmo + " / " + weappon.MagazineSize;
            if (weappon.CurrentAmmo <= 0)
                ammoText.text = "Press 'R' to reload";
        }
    }

    void UpdateHealthUI()
    {
        if (playerHealth == null) return;

        healthText.text = "Health: " + Mathf.Round(playerHealth.life) + " / " + playerHealth.maxLife;

        if (playerHealth.life <= 25)
        {
            healthText.color = Color.red;
        }
        else if (playerHealth.life <= 50)
        {
            healthText.color = Color.yellow;
        }
        else
        {
            healthText.color = Color.white;
        }
    }
}