using System.Linq;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.RPGResources;
using UnityEngine;
using GameDevTV.Utils;
namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionTime = 3f;
        [SerializeField] private float aggroCoolDownTime = 3f;
        [SerializeField] private float waypointDwellTime = 3f;
        [Range(0, 1)]
        [SerializeField] private float patrolSpeedFraction = 0.2f;
        [SerializeField] private float alertDistance = 5f;
        [SerializeField] private PatrolPath patrolPath;
        private PlayerController playerController;
        private Fighter fighter;
        private HealthComponent healthComponent;
         public LazyValue<Vector3> guardPosition;
        private Mover mover;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinceAggravated = Mathf.Infinity;
        private int currentWayPointIndex;
        private float waypointDwellingTimer;
        private void Awake()
        {
            healthComponent = GetComponent<HealthComponent>();
            mover = GetComponent<Mover>();
            playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            fighter = GetComponent<Fighter>();
            guardPosition = new LazyValue<Vector3>(() => transform.position);
            SetWayPointDwellingTimer();
        }

        private void Start()
        {
            guardPosition.ForceInit();
        }

        private void Update()
        {
            if (healthComponent.AlreadyDied) return;
            if (IsAggravated() && fighter.CanAttack(playerController.gameObject))
            {
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer <= suspicionTime)
            {
                SuspiciousBehaviour();
            }
            else
                PatrolBehaviour();

            UpdateTimer();

        }

        private void PatrolBehaviour()
        {
            mover.SetNavMeshSpeed(patrolSpeedFraction);
            Vector3 nextPosition = guardPosition.value;
            if (patrolPath)
            {
                if (AtWayPoint())
                    CycleWayPoint();
                nextPosition = GetCurrentWayPoint();
            }
            mover.StartMoveAction(nextPosition);
        }

        private bool AtWayPoint()
        {
            float minDistance = 1f;
            waypointDwellingTimer -= Time.deltaTime;
            return ((transform.position - GetCurrentWayPoint()).sqrMagnitude <= minDistance) && waypointDwellingTimer <= 0;
        }

        private void CycleWayPoint()
        {
            currentWayPointIndex = patrolPath.GetNextIndex(currentWayPointIndex);
            SetWayPointDwellingTimer();
        }

        private Vector3 GetCurrentWayPoint()
        {
            return patrolPath.GetWayPoint(currentWayPointIndex);
        }

        private void SuspiciousBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            mover.SetNavMeshSpeed(1);
            fighter.Attack(playerController.gameObject);
            AlertNearbyEnemies();
        }

        private void AlertNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position,alertDistance,Vector3.up,0);
            var aIControllers = hits.Select(x => x.collider.GetComponent<AIController>()).
            Where(x => x != null);
            foreach (AIController aIController in aIControllers)
            {
                aIController.Aggravated();
            }

        }

        public void Aggravated()
        {
            timeSinceAggravated = 0;
        }

        private bool IsAggravated()
        {
            return (transform.position - playerController.transform.position).sqrMagnitude <= Mathf.Pow(chaseDistance, 2) ||
                    timeSinceAggravated < aggroCoolDownTime;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }


        private void UpdateTimer()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceAggravated += Time.deltaTime;
        }

        private void SetWayPointDwellingTimer()
        {
            waypointDwellingTimer = waypointDwellTime;
        }
    }
}
