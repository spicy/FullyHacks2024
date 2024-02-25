using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicMeshCutter
{

    [RequireComponent(typeof(LineRenderer))]
    public class TwoDMouseBehaviour : CutterBehaviour
    {
        public LineRenderer LR => GetComponent<LineRenderer>();
        private Vector3 _from;
        private Vector3 _to;
        private bool _isDragging;

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isDragging = true;

                Vector3 mousePos = Input.mousePosition;
                mousePos.z = Camera.main.nearClipPlane; // Set to near clip plane
                _from = Camera.main.ScreenToWorldPoint(mousePos);
                _from.z = 0; // Ensure Z is 0 for 2D
            }

            if (_isDragging)
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = Camera.main.nearClipPlane; // Consistent with _from initialization
                _to = Camera.main.ScreenToWorldPoint(mousePos);
                _to.z = 0; // Ensure Z is 0 for 2D
                VisualizeLine(true);
            }

            if (Input.GetMouseButtonUp(0) && _isDragging)
            {
                _isDragging = false;
                VisualizeLine(false);
                Cut();
            }
        }

        private void Cut()
        {
            Plane plane = new Plane(_from, _to, Camera.main.transform.position);
            
            VisualizePlane((_from + _to) / 2, plane.normal);

            var roots = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var root in roots)
            {
                if (!root.activeInHierarchy)
                    continue;
                var targets = root.GetComponentsInChildren<MeshTarget>();
                foreach (var target in targets)
                {
                    Cut(target, _to, plane.normal, null, OnCreated);
                }
            }
        }
        void VisualizePlane(Vector3 pointOnPlane, Vector3 planeNormal)
        {
            // Draw the normal of the plane
            Debug.DrawRay(pointOnPlane, planeNormal * 5, Color.red, duration: 5f);

            // Create a rotation that looks in the direction of the normal
            Quaternion rotation = Quaternion.LookRotation(planeNormal);

            // Determine the size of the plane visualization
            float planeSize = 5f;
            Vector3 right = rotation * Vector3.right * planeSize;
            Vector3 forward = rotation * Vector3.forward * planeSize;

            // Decrease the step size to draw more lines
            float stepSize = planeSize / 20; // Increase the number of lines by making the step size smaller

            // Draw a denser grid to represent the plane
            for (float i = -planeSize; i <= planeSize; i += stepSize)
            {
                // Lines parallel to the X axis
                Debug.DrawRay(pointOnPlane + right * i - forward * planeSize, forward * 2 * planeSize, Color.blue, 5f);
                // Lines parallel to the Z axis
                Debug.DrawRay(pointOnPlane + forward * i - right * planeSize, right * 2 * planeSize, Color.blue, 5f);
            }
        }

        void OnCreated(Info info, MeshCreationData cData)
        {
            MeshCreation.TranslateCreatedObjects(info, cData.CreatedObjects, cData.CreatedTargets, Separation);
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

}