using System;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using UnityEngine.AI;
using System.Linq;
using RPG.RPGResources;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType cursorType;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] private CursorMapping[] cursorMappings;
        [SerializeField] private float navmeshProjectionDistance = 1;
        
        private Mover mover;
        private Fighter fighter;
        private HealthComponent healthComponent;
        private void Awake()
        {
            healthComponent = GetComponent<HealthComponent>();
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
        }

        private void Update()
        {
            if (healthComponent.AlreadyDied)
            {
                SetCursor(CursorType.None);
                return;
            }
            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;
            // if (InteractWithUI()) return;
            SetCursor(CursorType.None);
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = SortedRaycasts();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.ComponentCursorType);
                        return true;
                    }
                }
            }

            return false;
        }



        private bool InteractWithMovement()
        {
            Vector3 target;
            bool hasHit = RaycastNavmesh(out target);

            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                    mover.StartMoveAction(target);
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private bool InteractWithUI()
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        private RaycastHit[] SortedRaycasts()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseray());
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);
            return hits;
        }

        private bool RaycastNavmesh(out Vector3 target)
        {
            RaycastHit hit;
            NavMeshHit navmeshHit;
            target = Vector3.zero;
            bool hasHit = Physics.Raycast(GetMouseray(), out hit);
            if (!hasHit) return false;

            bool hasCastToNavmesh = NavMesh.SamplePosition(hit.point, out navmeshHit, navmeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastToNavmesh) return false;

            target = navmeshHit.position;
            return mover.CanMoveTo(target);
        }

        private void SetCursor(CursorType cursorType)
        {
            CursorMapping cursorMapping = cursorMappings.First<CursorMapping>(c => c.cursorType == cursorType);
            Cursor.SetCursor(cursorMapping.texture, cursorMapping.hotspot, CursorMode.Auto);
        }


        private void MoveToCursor()
        {
            RaycastHit hit;
            if (Physics.Raycast(GetMouseray(), out hit))
                mover.MoveTo(hit.point);

        }

        private static Ray GetMouseray()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
