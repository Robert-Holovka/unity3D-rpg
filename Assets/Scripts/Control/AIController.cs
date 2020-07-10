using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Stats;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [Header("Detection parameters")]
        [SerializeField] float detectionRadius = 5f;
        [SerializeField] Color detectionRadiusColor = Color.blue;
        [Header("Patroling")]
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellingTime = 3f;
        [Range(0, 1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;

        // Cached components
        private GameObject player;
        private Fighter fighter;
        private Health health;
        private Mover mover;
        private ActionScheduler actionScheduler;

        // Patroling
        private Vector3 initialPosition;
        private int currentWaypointIndex;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinceArrivedAtWaypoint = Mathf.Infinity;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        private void Start()
        {
            initialPosition = transform.position;
        }

        private void Update()
        {
            if (health.IsDead()) return;

            bool playerInRange = Vector3.Distance(transform.position, player.transform.position) <= detectionRadius;
            if (playerInRange && fighter.CanAttack(player))
            {
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = initialPosition;
            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            if (timeSinceArrivedAtWaypoint > waypointDwellingTime)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            timeSinceArrivedAtWaypoint = 0;
            currentWaypointIndex = patrolPath.GetNextWaypointIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void SuspicionBehaviour()
        {
            actionScheduler.CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = detectionRadiusColor;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }
}
