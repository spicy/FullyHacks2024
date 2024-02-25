using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DynamicMeshCutter
{

    [RequireComponent(typeof(LineRenderer))]
    public class MouseBehaviour : CutterBehaviour
    {
        public LineRenderer lineRenderer => GetComponent<LineRenderer>();
        private Vector3 _from;
        private Vector3 _to;
        private bool _isDragging;

        protected override void Update()
        {
            base.Update();

            if (Input.GetMouseButtonDown(0))
            {
                _isDragging = true;

                var mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 6f);
                _from = Camera.main.ScreenToWorldPoint(mousePos);
            }

            if (_isDragging)
            {
                var mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 6f);
                _to = Camera.main.ScreenToWorldPoint(mousePos);
                VisualizeLine(true);
            }
            else
            {
                VisualizeLine(false);
            }

            if (Input.GetMouseButtonUp(0) && _isDragging)
            {
                Slice();
                _isDragging = false;
            }
        }
        
        private void Slice()
        {
            Plane plane = new Plane(_from, _to, Camera.main.transform.position);

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

        void OnCreated(Info info, MeshCreationData cData)
        {
            MeshCreation.TranslateCreatedObjects(info, cData.CreatedObjects, cData.CreatedTargets, Separation);
            foreach (GameObject obj in cData.CreatedObjects)
            {
                float randomDelay = Random.Range(1f, 10f);
                Destroy(obj, randomDelay);
            }
        }

        private void VisualizeLine(bool value)
        {
            if (lineRenderer == null)
                return;

            lineRenderer.enabled = value;

            if (value)
            {
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, _from);
                lineRenderer.SetPosition(1, _to);
            }
        }

    }

}