using UnityEngine;
using System.Collections;

public class EnemyZombie : MonoBehaviour
{
    public GameObject player;
    public GameObject Drops;
    public Animator animator;
    private CharacterController controller;
    
    string bullet = "bullet";

    public float moveSpeed = 3f;
    public float attackDistance = 2f; //Abstand für Nahkampf-Angriff
    public float activationDistance = 20f; //Abstand ab dem der Zombie den Spieler bemerkt
    public float rotationSpeed = 5f; //Wie schnell sich Zombie dreht
    public float gravity = -9.81f; //Schwerkraft
    
    public float attackDamage = 10f;
    public float attackCooldown = 2f; //Zeit zwischen Angriffen
    
    private float attackTimer = 0;
    private bool isActive = false;
    private Vector3 verticalVelocity;

    private float timer = 0f;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    
    void Update()
    {
        timer += Time.deltaTime;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        
        if (distanceToPlayer <= activationDistance)
        {
            isActive = true;
            animator.SetTrigger("Rage");
        }
        
        if (isActive)
        {

            LookAtPlayer();
            ApplyGravity();
            
            attackTimer += Time.deltaTime;
            
            if (distanceToPlayer <= attackDistance)
            {
                if (attackTimer >= attackCooldown)
                {
                    Attack();
                    attackTimer = 0;
                    
                    if (animator != null)
                    {
                        animator.SetBool("IsWalking", false);
                    }
                }
            }
            else
            {
                MoveTowardsPlayer();
                
                if (animator != null)
                {
                    animator.SetBool("IsWalking", true);
                }
            }

            Damageable damageable = GetComponent<Damageable>();
            if (timer >= 20)
            {
                Drop();
                timer = 0;
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("IsWalking", false);
            }
            
            ApplyGravity();
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            animator.SetTrigger("Hit");
        }
    }
    void LookAtPlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0;
        
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    void MoveTowardsPlayer()
    {
        
        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0;
    
        
        Vector3 movement = direction * moveSpeed * Time.deltaTime;
        controller.Move(movement);
    }
    
    void ApplyGravity()
    {
        if (controller == null) return;
        
        if (controller.isGrounded)
        {
            verticalVelocity.y = -2f;
        }
        else
        {
            verticalVelocity.y += gravity * Time.deltaTime;
        }
        
        controller.Move(verticalVelocity * Time.deltaTime);
    }
    
    void Attack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            StartCoroutine(Delay());
        }
    }
    
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject == player)
        {
            if (attackTimer >= attackCooldown)
            {
                Attack();
                attackTimer = 0;
            }
        }
    }

    void Drop()
    {
        if (Drops != null)
        {
            Instantiate(Drops, transform.position, Quaternion.identity);
            Instantiate(Drops, transform.position, Quaternion.identity);
            Instantiate(Drops, transform.position, Quaternion.identity);
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(2f);
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.TakeDamage(attackDamage);
    }
}