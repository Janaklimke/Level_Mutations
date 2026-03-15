using UnityEngine;

public class FairyMovement : MonoBehaviour
{
    private GameObject player;
    public GameObject arrow;
    public Transform muzzlePoint;
    
    public float activationDistance = 10f;
    public bool isActive = false;
    
    public float movementRadius = 5f;
    public float minHeight = 1f;
    public float maxHeight = 4f;
    
    public float moveSpeed = 2f;
    public float changeDirectionTime = 2f;
    
    public float idleBobSpeed = 1f;
    public float idleBobAmount = 0.3f;
    private Vector3 startPosition;
    
    private Vector3 targetPosition;
    private float timer;

    private bool canShoot = true;
    private float cooldown = 0;
    
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        startPosition = transform.position;
        ChooseNewTargetPosition();
    }
    
    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        
        if (distanceToPlayer <= activationDistance) isActive = true;
        
        if (isActive) ActiveBehavior();
        else IdleBehavior();
    }
    
    void ActiveBehavior()
    {
        if (player == null) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        
        timer += Time.deltaTime;
        
        if (timer >= changeDirectionTime || Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            ChooseNewTargetPosition();
            timer = 0;
        }

        LookAtPlayer();

        cooldown += Time.deltaTime;
        if (canShoot)
        {
            if (cooldown > 0)
            {
                shoot();              
            }
        }
    }
    
    void IdleBehavior()
    {
        float bob = Mathf.Sin(Time.time * idleBobSpeed) * idleBobAmount;
        transform.position = startPosition + Vector3.up * bob;
    }
    
    void ChooseNewTargetPosition()
    {
        if (player == null) return;

        int maxAttempts = 10;
        bool validPositionFound = false;
        
        for (int i = 0; i < maxAttempts && !validPositionFound; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * movementRadius;
            Vector3 newPosition = player.transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
            float randomHeight = Random.Range(minHeight, maxHeight);
            
            RaycastHit hit;
            if (Physics.Raycast(newPosition + Vector3.up * 100f, Vector3.down, out hit, 200f))
            {
                newPosition.y = hit.point.y + randomHeight;
                
                if (!Physics.Linecast(transform.position, newPosition))
                {
                    targetPosition = newPosition;
                    validPositionFound = true;
                }
            }
        }
        
        if (!validPositionFound)
        {
            targetPosition = transform.position + Random.insideUnitSphere * 2f;
        }
    }
    
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            transform.position += Vector3.up * minHeight * 0.1f;
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ChooseNewTargetPosition();
            timer = 0;
        }
    }
    
    void shoot()
    {
        if (player == null) return;

        GameObject obj = Instantiate(arrow, muzzlePoint.position, muzzlePoint.rotation);
        obj.transform.LookAt(player.transform);
        cooldown = -5;
    }

    void LookAtPlayer()
    {
        if (player == null) return;

        float x = player.transform.position.x;
        float z = player.transform.position.z;
        Vector3 target = new Vector3(x, transform.position.y, z);
        transform.LookAt(target);
    }
}