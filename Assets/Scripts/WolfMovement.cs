using UnityEngine;

public class WolfEnemy : MonoBehaviour
{
    [Header("Statistik")]
    [SerializeField] private int damage = 15;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float attackDistance = 1.2f;
    [SerializeField] private float chaseRadius = 7f;
    [SerializeField] private float patrolRange = 3f;

    [Header("Referensi")]
    [SerializeField] private Transform player;
    [SerializeField] private Animator animator;

    private Vector3 originalScale;
    private Vector3 startPosition;
    private float leftLimit;
    private float rightLimit;
    private bool movingRight = true;
    private float lastAttackTime;
    private float attackCooldown = 1.5f;

    private void Start()
    {
        originalScale = transform.localScale;
        startPosition = transform.position;
        leftLimit = startPosition.x - patrolRange;
        rightLimit = startPosition.x + patrolRange;
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
            animator.SetBool("isWalking", false);

            if (Time.time >= lastAttackTime + attackCooldown)
            {
                AttackPlayer();
                lastAttackTime = Time.time;
            }
        }
        else if (distanceToPlayer <= chaseRadius)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        animator.SetBool("isWalking", true);

        float targetX = movingRight ? rightLimit : leftLimit;
        float newX = Mathf.MoveTowards(transform.position.x, targetX, speed * Time.deltaTime);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        if (Mathf.Approximately(transform.position.x, targetX))
            movingRight = !movingRight;

        FlipSprite(movingRight);

    }

    private void ChasePlayer()
    {
        animator.SetBool("isWalking", true);

        float direction = Mathf.Sign(player.position.x - transform.position.x);
        float newX = Mathf.MoveTowards(transform.position.x, player.position.x, speed * Time.deltaTime);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        FlipSprite(direction > 0);
    }

    private void AttackPlayer()
    {
        animator.SetTrigger("attack");

        PlayerHealth health = player.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }
    }

    private void FlipSprite(bool facingRight)
    {
        float x = Mathf.Abs(originalScale.x);
        transform.localScale = new Vector3(facingRight ? x : -x, originalScale.y, originalScale.z);
    }
}
