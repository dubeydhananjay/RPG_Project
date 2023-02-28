using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] private float experiencePoints;
        public float ExperiencePoints { get { return experiencePoints; } }

        public event System.Action onExperienceGained;


        public void GainExperience(float expPoint)
        {
            experiencePoints += expPoint;
            onExperienceGained?.Invoke();
        }

        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
    }
}
