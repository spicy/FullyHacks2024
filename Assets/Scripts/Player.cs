using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour, ICharacter
{
    [SerializeField] private float health;
    [SerializeField] private float moveSpeed = 5f;
    private bool isInvulnerable = false;
    private Rigidbody2D rb;

    [SerializeField] private float dashPower = 5.0f;
    [SerializeField] private float dashCooldown = 2.0f;
    [SerializeField] private float dashTimeInSeconds = 0.25f;

    private bool isDashing = false;
    private bool canDash = true;
    [SerializeField] private float invulnerabilityDuration = 1.5f;

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
        if (isDashing)
        {
            return;
        }

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
        Destroy(gameObject);
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

    public void TryDash()
    {
        if (!canDash) return;

        StartCoroutine(Dash());
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        SetInvulnerability(true, invulnerabilityDuration);

        rb.velocity = new Vector2(transform.localScale.x * rb.velocity.x * dashPower, transform.localScale.y * rb.velocity.y * dashPower);
        yield return new WaitForSeconds(dashTimeInSeconds);
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
 
    /*
    public void getHealth()
    {
        "Health: " + health;
    }
    */

}