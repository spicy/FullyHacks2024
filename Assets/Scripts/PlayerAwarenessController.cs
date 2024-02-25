using UnityEngine;

public interface IPlayerAwareness
{
    bool IsAwareOfPlayer { get; }
    Vector2 DirectionToPlayer { get; }
}

public class PlayerAwarenessController : MonoBehaviour, IPlayerAwareness
{
    [SerializeField] private float playerAwarenessDistance;
    private Transform player;

    public bool IsAwareOfPlayer { get; private set; }
    public Vector2 DirectionToPlayer { get; private set; }

    private void Start()
    {
        player = FindFirstObjectByType<Player>().transform;
    }

    void Update()
    {
        Vector2 enemyToPlayerVector = player.position - transform.position;
        DirectionToPlayer = enemyToPlayerVector.normalized;
        IsAwareOfPlayer = enemyToPlayerVector.magnitude <= playerAwarenessDistance;
    }
}