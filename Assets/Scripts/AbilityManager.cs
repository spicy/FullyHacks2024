using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public LineRenderer lineRenderer;
    private int selectedAbility = -1;
    private float[] cooldowns = new float[3];
    private float[] cooldownTimers = new float[3];
    private Vector3 _from;
    private Vector3 _to;
    private bool _isDragging;

    void Update()
    {
        // Ability selection
        if (Input.GetKeyDown(KeyCode.Alpha1) && cooldownTimers[0] <= 0f)
        {
            selectedAbility = 0;
            Debug.Log("Ability 1 selected - Select two points by dragging.");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && cooldownTimers[1] <= 0f)
        {
            selectedAbility = 1;
            Debug.Log("Ability 2 selected");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && cooldownTimers[2] <= 0f)
        {
            selectedAbility = 2;
            Debug.Log("Ability 3 selected");
        }

        // Dragging and ability activation
        if (selectedAbility == 0) // For Ability 1
        {
            if (Input.GetMouseButtonDown(1))
            {
                _isDragging = true;
                var mousePos = Input.mousePosition;
                mousePos.z = 6f;
                _from = Camera.main.ScreenToWorldPoint(mousePos);
            }

            if (_isDragging)
            {
                var mousePos = Input.mousePosition;
                mousePos.z = 6f;
                _to = Camera.main.ScreenToWorldPoint(mousePos);
                VisualizeLine(true);
            }

            if (Input.GetMouseButtonUp(1) && _isDragging)
            {
                ActivateAbility(0);
                _isDragging = false;
            }
        }
        else
        {
            VisualizeLine(false);
        }

        // Update cooldown timers
        for (int i = 0; i < cooldownTimers.Length; i++)
        {
            if (cooldownTimers[i] > 0)
            {
                cooldownTimers[i] -= Time.deltaTime;
            }
        }
    }

    void ActivateAbility(int abilityIndex)
    {
        // Check if the ability is ready (not on cooldown)
        if (cooldownTimers[abilityIndex] > 0)
        {
            Debug.Log($"Ability {abilityIndex + 1} is on cooldown.");
            return;
        }

        // Ability 1
        if (abilityIndex == 0)
        {
            Debug.Log($"Ability {abilityIndex + 1} activated between points {_from} and {_to}.");
            
            // create airstrike between _from and _to

            // Reset line visualization
            VisualizeLine(false);
        }

        // Set cooldown for the ability
        cooldownTimers[abilityIndex] = cooldowns[abilityIndex];
        selectedAbility = -1; // Deselect ability after use
    }

    void Start()
    {
        // Initialize cooldowns for each ability (example values)
        cooldowns[0] = 10f; // Cooldown for ability 1
        cooldowns[1] = 20f; // Cooldown for ability 2
        cooldowns[2] = 30f; // Cooldown for ability 3
    }

    void VisualizeLine(bool isVisible)
    {
        if (!lineRenderer) return; // Safety check

        lineRenderer.enabled = isVisible;
        if (isVisible)
        {
            lineRenderer.SetPosition(0, _from);
            lineRenderer.SetPosition(1, _to);
        }
    }
}
