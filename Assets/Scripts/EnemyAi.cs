using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;
    public GameObject gunObject;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    public float health = 120;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Patrolling cooldown
    bool isCooldown = false;
    public float patrolCooldownDuration = 3f;
    float patrolCooldownTimer = 0f;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("PlayerObj").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Check if the enemy AI is still alive
        if (health > 0)
        {
            // Check for sight and attack range
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (!playerInSightRange && !playerInAttackRange)
            {
                if (!isCooldown)
                {
                    Patroling();
                    // Trigger walk animation
                    animator.SetBool("IsWalking", true);
                    animator.SetBool("IsAttacking", false);
                    animator.SetBool("IsLookingAround", false);
                    gunObject.SetActive(false);
                }
                else
                {
                    // Start cooldown timer
                    patrolCooldownTimer += Time.deltaTime;
                    if (patrolCooldownTimer >= patrolCooldownDuration)
                    {
                        isCooldown = false;
                        patrolCooldownTimer = 0f;
                    }
                    // Trigger look around animation
                    animator.SetBool("IsWalking", false);
                    animator.SetBool("IsAttacking", false);
                    animator.SetBool("IsLookingAround", true);
                }
            }
            if (playerInSightRange && !playerInAttackRange)
            {
                ChasePlayer();
                // Trigger walk animation
                animator.SetBool("IsWalking", true);
                animator.SetBool("IsAttacking", false);
                animator.SetBool("IsLookingAround", false);
                gunObject.SetActive(false);
            }
            if (playerInAttackRange && playerInSightRange)
            {
                AttackPlayer();
                // Trigger attack animation
                animator.SetBool("IsWalking", false);
                animator.SetBool("IsAttacking", true);
                animator.SetBool("IsLookingAround", false);
                gunObject.SetActive(true);
            }
        }
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
            // Start cooldown
            isCooldown = true;
        }
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // Enable the gun object
            gunObject.SetActive(true);

            ///Attack code here
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Invoke(nameof(DestroyEnemy), 0.5f);
            animator.SetTrigger("Die");
        }
        else
        {
            animator.SetTrigger("Damage");
        }
    }

    private void DestroyEnemy()
    {
        agent.enabled = false; // Disable the NavMeshAgent
        agent.velocity = Vector3.zero; // Stop the agent's velocity
        agent.isStopped = true; // Stop the agent from moving
        animator.enabled = false; // Disable the Animator
        gunObject.SetActive(false); // Disable the gun object
        // Optional: Play death animation or effects

        // Disable this script so it no longer updates
        enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
