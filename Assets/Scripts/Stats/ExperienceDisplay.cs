using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI xpVal;
        private Experience experience;
        private void Awake() {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update() {
            xpVal.text = System.String.Format("{0:0}",experience.ExperiencePoints);
        }
    }
}
