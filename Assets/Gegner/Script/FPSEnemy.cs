using UnityEngine;

public class FPSEnemy : MonoBehaviour
{
    public GameObject bullet;
    public GameObject player;
    public Transform muzzlePoint;
    
    //bewegung
    public float moveSpeed = 3f;
    public float stopDistance = 5f;
    
    private bool canShoot = true;
    private float cooldown = 0;

    public float idleSpeed = 1f;
    public float idleAmount = 0.3f;
    private Vector3 startPosition;

    public float activationDistance = 10f; // Abstand ab dem der Enemy
    private bool isActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
        /*
        Arten player zu Finden wenn er nicht als game object referenced werden kann
        player = GameObject.Find("Player");
        player = GameObject.FindWithTag("Player");
        
        spawnPoint = transform.GetChild(0).gameObject;
        */
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);      
        if (distanceToPlayer <= activationDistance) isActive = true;

        if (isActive)
        {
            LookAtPlayer();
            MoveTowardsPlayer();
        
            cooldown += Time.deltaTime;
            if (canShoot)
            {
                if (cooldown > 0)
                {
                    shoot();              
                }
            }
        }

        else IdleBehavior();       
    }

    void MoveTowardsPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        
        if (distance > stopDistance)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            
            direction.y = 0;
            
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    void LookAtPlayer ()
    {
        float x = player.transform.position.x;
        float z = player.transform.position.z;
        Vector3 target = new Vector3(x, transform.position.y, z); //mit transform.position.y schaut man auf sein eigenes y und nicht das des players
        transform.LookAt(target);
    }

    void shoot ()
    {
        GameObject obj = Instantiate(bullet, muzzlePoint.position, muzzlePoint.rotation);
        obj.transform.LookAt(player.transform);
        cooldown = -5;
    }

    void IdleBehavior()
    {
        Vector3 movement = new Vector3(0, 0, 1);
        float idleMove = Mathf.Sin(Time.time * idleSpeed) * idleAmount;
        transform.position = startPosition + movement * idleMove;
    }
}
