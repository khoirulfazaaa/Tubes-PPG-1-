using UnityEngine;

public class BearEnemy : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float attackDistance = 1.5f;
    [SerializeField] private float chaseRadius = 6f;      // Radius untuk mulai ngejar player
    [SerializeField] private Transform player;
    [SerializeField] private Animator animator;

    private Vector3 originalScale;

    private float lastAttackTime = 0f;
    private float attackCooldown = 1.5f;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    private void Update()
    {
        if (player == null)
        {
            animator.SetBool("isWalking", false);
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackDistance)
        {
            // Serang player jika sudah dekat
            animator.SetBool("isWalking", false);

            if (Time.time >= lastAttackTime + attackCooldown)
            {
                AttackPlayer();
                lastAttackTime = Time.time;
            }
        }
        else if (distanceToPlayer <= chaseRadius)
        {
            // Mulai mengejar player
            ChasePlayer();
        }
        else
        {
            // Player terlalu jauh, beruang diam
            animator.SetBool("isWalking", false);
        }
    }

    private void ChasePlayer()
    {
        animator.SetBool("isWalking", true);

        float playerX = player.position.x;

        float newX = Mathf.MoveTowards(transform.position.x, playerX, speed * Time.deltaTime);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        // Flip sprite beruang berdasarkan posisi player
        if (playerX > transform.position.x)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }

    private void AttackPlayer()
    {
        animator.SetBool("attack", true);

        animator.SetBool("attack", true);

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }
}
