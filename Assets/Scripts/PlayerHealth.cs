using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    private Animator animator;
    private PlayerMovement movementScript;
    public GameObject gameOverUI; // Assign UI GameObject “Kamu Mati” dari Inspector

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        movementScript = GetComponent<PlayerMovement>();

        if (gameOverUI != null)
            gameOverUI.SetActive(false); // Sembunyikan di awal
    }

    public void TakeDamage(int damage)
    {

        if (currentHealth <= 0) return; // Hindari overkill

        animator.SetTrigger("Hit");

        currentHealth -= damage;
        Debug.Log("Player kena serang! Sisa HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player Mati!");

        if (animator != null)
            animator.SetTrigger("die");

        if (movementScript != null)
            movementScript.enabled = false;

        if (gameOverUI != null)
            gameOverUI.SetActive(true);
    }
}
