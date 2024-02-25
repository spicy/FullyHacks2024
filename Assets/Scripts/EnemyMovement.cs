using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    private Rigidbody2D rb;
    private IPlayerAwareness playerAwareness;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAwareness = GetComponent<IPlayerAwareness>();
    }

    private void FixedUpdate()
    {
        if (playerAwareness.IsAwareOfPlayer)
        {
            Vector2 targetDirection = playerAwareness.DirectionToPlayer;
            RotateTowardsTarget(targetDirection);
            SetVelocity(targetDirection);
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

    private void SetVelocity(Vector2 targetDirection)
    {
        rb.velocity = targetDirection.normalized * speed;
    }
}