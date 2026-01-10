using UnityEngine;
using TMPro;

public class WeapponUI : MonoBehaviour
{
    public Weappon weappon;
    public TMP_Text ammoText;

    void Update()
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
}
