using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Stats
{
    public class LevelUpDisplay : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI levelUpVal;
        private BaseStat baseStat;
        private void Awake() {
            baseStat = GameObject.FindWithTag("Player").GetComponent<BaseStat>();
        }

        private void Update() {
            levelUpVal.text = System.String.Format("{0:0}", baseStat.GetLevel());
        }
    }
}
