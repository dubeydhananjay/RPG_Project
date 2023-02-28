using System;
using UnityEngine;
using UnityEngine.UI;
namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] Text damageText;
        public void DestroyDamageText()
        {
            Destroy(gameObject);
        }

        public void SetValue(float val)
        {
            damageText.text = String.Format("{0:0}", val);
        }
    }
}
