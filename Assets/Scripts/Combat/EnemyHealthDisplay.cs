using System.Collections;
using System;
using UnityEngine;
using RPG.RPGResources;
namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI enemyHealthVal;
        private Fighter playerFighter;

        private void Awake()
        {
            playerFighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            HealthComponent currentEnemyHealth = playerFighter.GetTarget();
            if (currentEnemyHealth)
                enemyHealthVal.text = String.Format("{0:0}/{1:0}", currentEnemyHealth.Health, currentEnemyHealth.MaxHealth);
            else
                enemyHealthVal.text = "NA";
        }
    }
}
