using RPG.Combat;
using RPG.Movement;
using RPG.Stats;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Fighter figher;
        private Mover mover;
        private Health health;

        private void Awake()
        {
            figher = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            health = GetComponent<Health>();
        }

        private void Update()
        {
            if (health.IsDead()) return;
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                GameObject targetGameObject = target.gameObject;
                if (!GetComponent<Fighter>().CanAttack(targetGameObject)) continue;

                if (Input.GetMouseButton(1))
                {
                    figher.Attack(targetGameObject);
                }
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            bool isHit = Physics.Raycast(GetMouseRay(), out RaycastHit hit);
            if (isHit)
            {
                if (Input.GetMouseButton(1))
                {
                    mover.StartMoveAction(hit.point, 1f);
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}