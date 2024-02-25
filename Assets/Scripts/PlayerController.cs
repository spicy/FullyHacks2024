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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.TryDash();
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector3 startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            startPos.z = 0;
            endPos.z = 0;

            // Assuming we have the start and end positions of the slice
            Vector2 direction = endPos - startPos;
            float distance = Vector2.Distance(startPos, endPos);

            RaycastHit2D[] hits = Physics2D.RaycastAll(startPos, direction, distance);
            foreach (var hit in hits)
            {
                ISliceable sliceable = hit.collider.GetComponent<ISliceable>();
                if (sliceable != null)
                {
                    // This object can be sliced
                    sliceable.Slice();
                }
            }

        }
    }
    private void VisualizeLine(bool value)
    {
        if (LR == null)
            return;

        LR.enabled = value;

        if (value)
        {
            LR.positionCount = 2;
            LR.SetPosition(0, _from);
            LR.SetPosition(1, _to);
        }
    }
}
