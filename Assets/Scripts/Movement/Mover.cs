using System.Collections;
using System.Collections.Generic;

using RPG.RPGResources;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Core;
namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        private NavMeshAgent navMeshAgent;
        private Animator animator;
        private HealthComponent healthComponent;
        [SerializeField] private float maxSpeed = 1f;
        [SerializeField] private float maxNavPathLength = 40;

        [System.Serializable]
        struct MoverData
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        }

        private void Awake()
        {
            healthComponent = GetComponent<HealthComponent>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }


        void Update()
        {
            navMeshAgent.enabled = !healthComponent.AlreadyDied;
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destinationPos)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destinationPos);
        }

        public void MoveTo(Vector3 destinationPos)
        {
            navMeshAgent.SetDestination(destinationPos);
            navMeshAgent.isStopped = false;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            animator.SetFloat("forwardSpeed", speed);
        }

        public void CancelAction()
        {
            navMeshAgent.isStopped = true;
        }

        public void SetNavMeshSpeed(float speedFraction)
        {
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
        }

         public bool CanMoveTo(Vector3 target)
        {
            NavMeshPath navMeshPath = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, navMeshPath);
            if (!hasPath) return false;
            if (navMeshPath.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(navMeshPath) > maxNavPathLength) return false;
            return true;
        }

         private float GetPathLength(NavMeshPath path)
        {
            Vector3[] corners = path.corners;
            float sumOfAllCorners = 0;
            if (corners.Length < 2) return sumOfAllCorners;
            for (int i = 1; i < corners.Length; i++)
            {
                sumOfAllCorners += Vector3.Distance(corners[i], corners[i - 1]);
            }
            return sumOfAllCorners;
        }

        public object CaptureState()
        {
            MoverData data = new MoverData();
            data.rotation = new SerializableVector3(transform.eulerAngles);
            data.position = new SerializableVector3(transform.position);
            return data;
        }

        public void RestoreState(object state)
        {
            MoverData data = (MoverData)state;
            navMeshAgent.Warp(data.position.ToVector());
            transform.eulerAngles = data.rotation.ToVector();
        }
    }

}
