using RPG.RPGResources;
using UnityEngine;
using RPG.Control;
namespace RPG.Combat
{
    [RequireComponent(typeof(HealthComponent))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public CursorType ComponentCursorType
        {
            get
            {
                return CursorType.Combat;
            }
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            Fighter fighter = callingController.GetComponent<Fighter>();
            if (!fighter.CanAttack(gameObject)) return false;
            if (Input.GetMouseButton(0))
                fighter.Attack(gameObject);

            return true;
        }
    }
}