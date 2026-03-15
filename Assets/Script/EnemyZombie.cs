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
    public float attackDistance = 2f;
    public float activationDistance = 20f;
    public float rotationSpeed = 5f;
    public float gravity = -9.81f;
    
    public float attackDamage = 10f;
    public float attackCooldown = 2f;
    
    private float attackTimer = 0;
    public bool isActive = false;
    private Vector3 verticalVelocity;

    private float timer = 0f;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    
    void Update()
    {  
        if (player == null) return;

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
        if (player == null) return;

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
        if (player == null) return;

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
        if (player == null) return;

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
        if (player == null) return;

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

        // Player might have been destroyed during the 2 second delay
        if (player == null) yield break;

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }
}