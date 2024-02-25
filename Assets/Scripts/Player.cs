using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour, ICharacter
{
    [SerializeField] private float health;
    [SerializeField] private float moveSpeed = 5f;
    private bool isInvulnerable = false;
    private Rigidbody2D rb;

    [SerializeField] private float dashPower = 5.0f;
    [SerializeField] private float dashCooldown = 2.0f;
    private bool canDash = true;
    [SerializeField] private float invulnerabilityDuration = 1.5f;
    private float dashTime = 1f;

    public float Health
    {
        get => health;
        set => health = Mathf.Max(value, 0);
    }
    public float MoveSpeed { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            Debug.LogError("Could not find Rigidbody2D");
    }

    public void Move(Vector2 direction)
    {
        Vector2 moveVelocity = direction.normalized * moveSpeed;
        rb.velocity = moveVelocity;
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

    public void TryDash(Vector2 aimDirection)
    {
        StartCoroutine(Dash(aimDirection));
    }

    private IEnumerator Dash(Vector2 aimDirection)
    {
        canDash = false;
        SetInvulnerability(true, invulnerabilityDuration);

        rb.velocity = new Vector2(aimDirection.x * transform.localScale.x * dashPower, aimDirection.y * transform.localScale.y * dashPower);
        yield return new WaitForSeconds(dashTime);
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

}