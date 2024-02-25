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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.TryDash();
        }
    }
}
