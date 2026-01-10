using UnityEngine;

public class Weappon : MonoBehaviour
{
    public GameObject Bullet;
    public KeyCode fireKey;
    public Transform muzzlePoint;

    public FPSPlayer player;
    public float recoilAmount = 2f;
    public float fireRate = 5f;     // Schüsse pro Sekunde
    private float nextFireTime = 0f;

    // Munition
    private int currentAmmo;
    public int CurrentAmmo => currentAmmo;
    private int magazineSize = 6;   // Schüsse pro Magazin
    public int MagazineSize => magazineSize;

    // Reload
    public float reloadTime = 2f;   // Dauer des Nachladens
    public bool isReloading = false;
    public KeyCode reloadKey;
   
    void Start()
    {
        currentAmmo = magazineSize;
    }

    void Update()
    {
        if (isReloading)
            return;

        if (Input.GetKeyDown(reloadKey) && currentAmmo < magazineSize)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetKey(fireKey) && Time.time >= nextFireTime && currentAmmo > 0)
        {
            nextFireTime = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(Bullet, muzzlePoint.position, muzzlePoint.rotation);
        player.AddRecoil(recoilAmount);
        currentAmmo--;
        Debug.Log("Ammo left: " + currentAmmo);
    }

    System.Collections.IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = magazineSize;
        isReloading = false;
        Debug.Log("Reloaded!");
    }
}
