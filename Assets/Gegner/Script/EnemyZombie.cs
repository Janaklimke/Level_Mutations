using UnityEngine;

public class EnemyZombie : MonoBehaviour
{
    public GameObject player;
    public Animator animator; // Für spätere Animationen
    private CharacterController controller;

    public float moveSpeed = 3f;
    public float attackDistance = 2f; // Abstand für Nahkampf-Angriff
    public float activationDistance = 20f; // Abstand ab dem der Zombie den Spieler bemerkt
    public float rotationSpeed = 5f; // Wie schnell sich Zombie dreht
    public float gravity = -9.81f; // Schwerkraft
    
    public float attackDamage = 10f;
    public float attackCooldown = 1.5f; // Zeit zwischen Angriffen
    
    private float attackTimer = 0;
    private bool isActive = false;
    private Vector3 verticalVelocity;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        
    }
    
    void Update()
    {
    
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        
        if (distanceToPlayer <= activationDistance)
        {
            isActive = true;
        }
        
        if (isActive)
        {

            LookAtPlayer();
            ApplyGravity();
            
            // Timer erhöhen
            attackTimer += Time.deltaTime;
            
            // Prüfen ob Zombie nah genug zum Angreifen ist
            if (distanceToPlayer <= attackDistance)
            {
                // Angreifen wenn Cooldown vorbei ist
                if (attackTimer >= attackCooldown)
                {
                    Attack();
                    attackTimer = 0;
                    
                    // ANIMATION: Idle/Attack wechseln
                    if (animator != null)
                    {
                        animator.SetBool("IsWalking", false);
                    }
                }
            }
            else
            {
                // Zum Spieler laufen wenn zu weit weg
                MoveTowardsPlayer();
                
                // ANIMATION: Laufen
                if (animator != null)
                {
                    animator.SetBool("IsWalking", true);
                }
            }
        }
        else
        {
            // ANIMATION: Idle wenn inaktiv
            if (animator != null)
            {
                animator.SetBool("IsWalking", false);
            }
            
            ApplyGravity(); // Schwerkraft auch wenn inaktiv
        }
    }
    
    void LookAtPlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0; // Nur horizontal drehen
        
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    void MoveTowardsPlayer()
    {
        if (controller == null)
        {
            Debug.LogError("Controller ist null!");
            return;
        }
        
        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0; // Keine Bewegung auf Y-Achse
    
        
        // Bewegung mit CharacterController
        Vector3 movement = direction * moveSpeed * Time.deltaTime;
        controller.Move(movement);
    }
    
    void ApplyGravity()
    {
        if (controller == null) return;
        
        // Schwerkraft anwenden
        if (controller.isGrounded)
        {
            verticalVelocity.y = -2f; // Kleine negative Kraft um am Boden zu bleiben
        }
        else
        {
            verticalVelocity.y += gravity * Time.deltaTime; // Fallgeschwindigkeit erhöhen
        }
        
        controller.Move(verticalVelocity * Time.deltaTime);
    }
    
    void Attack()
    {
        Debug.Log("Zombie greift an! Schaden: " + attackDamage);
        
        // ANIMATION: Angriff
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
        
        // Schaden am Spieler verursachen
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }
    
    // CharacterController nutzt OnControllerColliderHit statt OnCollisionEnter
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject == player)
        {
            // Optional: Sofort angreifen bei Berührung
            if (attackTimer >= attackCooldown)
            {
                Attack();
                attackTimer = 0;
            }
        }
    }
}