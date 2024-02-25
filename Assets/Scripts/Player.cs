using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour, ICharacter
{
    [SerializeField]
    private float health;
    [SerializeField]
    private float moveSpeed = 5f;
    private bool isInvulnerable = false;
    private Rigidbody2D rb;

    public float Health
    {
        get => health;
        set => health = Mathf.Max(value, 0);
    }
    public float MoveSpeed { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 direction)
    {
        Vector2 move = direction.normalized * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + move);
    }

    public void TakeDamage(float amount)
    {
        if (isInvulnerable) return;
        Health -= amount;
        if (Health <= 0)
        {
            Die();
        }
    }

    public void Attack()
    {

    }

    public void AimTowards(Vector2 aimDirection)
    {

    }
    public void Die()
    {
        Debug.Log("Player died.");
    }

    public void SetInvulnerability(bool state, float duration)
    {
        isInvulnerable = state;
        if (state)
        {
            Invoke(nameof(ResetInvulnerability), duration);
        }
    }

    private void ResetInvulnerability()
    {
        isInvulnerable = false;
    }
}