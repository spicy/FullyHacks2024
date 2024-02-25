using UnityEngine;
using System.Collections;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.UIElements;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    private Player player;
    private Vector2 movementInput;
    private Vector2 aimDirection;

    private void Awake()
    {
        player = GetComponent<Player>();

        if (player == null) Debug.LogError("Could not find player!");
    }

    private void Update()
    {
        ProcessInputs();
    }

    private void FixedUpdate()
    {
        player.Move(movementInput);
    }

    private void ProcessInputs()
    {
        movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        aimDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        aimDirection.Normalize();

        if (Input.GetButtonDown("Fire1"))
        {
            player.AimTowards(aimDirection);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.TryDash();
        }
    }
}
