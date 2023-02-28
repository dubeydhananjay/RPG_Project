using System.Collections;
using System;
using UnityEngine;
namespace RPG.RPGResources
{
    public class HealthHUD : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI healthVal;
        private HealthComponent healthComponent;
        private void Awake()
        {
            healthComponent = GameObject.FindWithTag("Player").GetComponent<HealthComponent>();
        }

        private void Update()
        {
            healthVal.text = String.Format("{0:0}/{1:0}", healthComponent.Health, healthComponent.MaxHealth); //{0:0} means there should be no decimal value
        }
    }
}
