using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using RPG.RPGResources;
using UnityEngine;
namespace RPG.Combat
{
    public class PickUpWeapon : MonoBehaviour, IRaycastable
    {
        public CursorType ComponentCursorType
        {
            get
            {
                return CursorType.Pickup;
            }
        }
        [SerializeField] private WeaponConfig weapon;
        [SerializeField] private float respawnTime = 5;
        [SerializeField] private float healthRestore = 0;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                Pickup(other.gameObject);
            }
        }

        private void Pickup(GameObject subject)
        {
            if (weapon != null)
            {
                subject.GetComponent<Fighter>().EquipWeapon(weapon);
            }
            if (healthRestore > 0)
            {
                subject.GetComponent<HealthComponent>().Heal(healthRestore);
            }
            StartCoroutine(HidePickUpForSeconds());
        }

        private void PickUpActivation(bool flag)
        {
            GetComponent<Collider>().enabled = flag;
            foreach (Transform child in transform)
                child.gameObject.SetActive(flag);
        }

        private IEnumerator HidePickUpForSeconds()
        {
            PickUpActivation(false);
            yield return new WaitForSeconds(respawnTime);
            PickUpActivation(true);

        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Pickup(callingController.gameObject);
            }
            return true;
        }
    }
}
