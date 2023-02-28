
using UnityEngine;
namespace RPG.RPGResources
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private RectTransform foreground;

        [SerializeField] private HealthComponent healthComponent;
        private void Awake()
        {
            healthComponent = GetComponentInParent<HealthComponent>();
        }
        private void OnEnable()
        {
            healthComponent.UpdateHealthBar += UpdateHealthBar;
        }
        private void OnDisable()
        {
            healthComponent.UpdateHealthBar -= UpdateHealthBar;
        }

        private void UpdateHealthBar()
        {
            Vector3 scale = foreground.localScale;
            scale.x = healthComponent.GetFraction();
            foreground.localScale = scale;
            if (Mathf.Approximately(scale.x, 0)) gameObject.SetActive(false);
        }


    }
}
