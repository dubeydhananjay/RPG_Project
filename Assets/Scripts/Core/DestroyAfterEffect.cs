using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        [SerializeField] private GameObject target;
        private void Update()
        {
            if (!GetComponent<ParticleSystem>().IsAlive())
            {
                if (target) Destroy(target);
                else Destroy(gameObject);
            }
        }
    }
}
