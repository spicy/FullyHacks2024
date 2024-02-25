using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public partial class BaseEnemy : MonoBehaviour, ICharacter
{
    [SerializeField] private float health;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float fadeDuration = 2f;

    private Rigidbody2D rb;
    public float Health
    {
        get => health;
        set => health = Mathf.Max(value, 0);
    }
    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = Mathf.Max(value, 0);
    }

    [SerializeField] private float rotationSpeed;
    internal IPlayerAwareness playerAwareness;

    internal void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            Debug.LogError("Couldnt find Rigidbody2D component");

        playerAwareness = GetComponent<IPlayerAwareness>();

        if (playerAwareness == null)
            Debug.LogError("Couldnt find IPlayerAwareness component");
    }

    private void FixedUpdate()
    {
        if (playerAwareness.IsAwareOfPlayer)
        {
            Vector2 targetDirection = playerAwareness.DirectionToPlayer;
            RotateTowardsTarget(targetDirection);
            Move(targetDirection);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void RotateTowardsTarget(Vector2 targetDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, targetDirection);
        rb.rotation = Quaternion.RotateTowards(Quaternion.Euler(0, 0, rb.rotation), targetRotation, rotationSpeed * Time.fixedDeltaTime).eulerAngles.z;
    }

    public void Attack()
    {
        throw new System.NotImplementedException();
    }

    public void Move(Vector2 direction)
    {
        Vector2 move = direction.normalized * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + move);
    }

    public void TakeDamage(float amount)
    {
        Health -= amount;
        if (Health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        // Kill the enemy and add to the score manager ? Invoke death event??
        Debug.Log("Enemy Died");
        StartCoroutine(FadeOutAndDestroy());
    }

    private IEnumerator FadeOutAndDestroy()
    {
        float currentTime = 0;
        Material material = GetComponent<Renderer>().material;
        Color startColor = material.color;

        while (currentTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / fadeDuration);
            material.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

}
