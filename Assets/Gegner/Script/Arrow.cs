using UnityEngine;

public class Arrow : MonoBehaviour
{
    [Header("Pfeil-Eigenschaften")]
    public float speed = 40f;
    public float lifetime = 5f;
    public float damage = 20f;
    
    [Header("Kollision")]
    public LayerMask hitLayers;  // Was kann getroffen werden
    private bool hasHit = false;
    
    private AudioSource myAudio;
    private Vector3 lastPosition;
    
    void Start()
    {
        myAudio = GetComponent<AudioSource>();
        
        if (myAudio != null) 
            myAudio.Play();
        
        lastPosition = transform.position;
        Destroy(gameObject, lifetime);
    }
    
    void Update()
    {
        if (hasHit) return;
        
        // Raycast zwischen letzter und aktueller Position (für schnelle Objekte!)
        Vector3 direction = transform.forward * speed * Time.deltaTime;
        float distance = direction.magnitude;
        
        RaycastHit hit;
        if (Physics.Raycast(lastPosition, direction.normalized, out hit, distance + 0.5f, hitLayers))
        {
            OnHit(hit.collider.gameObject, hit.point);
            return;
        }
        
        lastPosition = transform.position;
        transform.position += transform.forward * speed * Time.deltaTime;
    }
    
    void OnHit(GameObject target, Vector3 hitPoint)
    {
        if (hasHit) return;
        hasHit = true;
        
        Debug.Log($"Arrow traf: {target.name}");
        
        // Versuche Schaden zu machen
        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            Debug.Log($"Arrow macht {damage} Schaden an Player!");
        }
        
        // Falls Player ein Parent-Objekt ist
        if (playerHealth == null)
        {
            playerHealth = target.GetComponentInParent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log($"Arrow macht {damage} Schaden an Player (Parent)!");
            }
        }
        
        // Bewege Pfeil zur Trefferstelle
        transform.position = hitPoint;
        
        // Zerstöre nach kurzer Zeit
        Destroy(gameObject, 0.1f);
    }
    
    // Fallback für normale Collider (z.B. Wände)
    void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;
        
        // Ignoriere Projektile
        if (other.CompareTag("bullet") || other.CompareTag("arrow"))
            return;
        
        OnHit(other.gameObject, transform.position);
    }
}
