using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
namespace RPG.Cinematics
{
    
    public class CinematicTrigger : MonoBehaviour
    {
        private bool playedOnce;

        private void OnTriggerEnter(Collider other) {
            if(other.gameObject.CompareTag("Player") && !playedOnce)
            {
                playedOnce = true;                
                GetComponent<PlayableDirector>().Play();
            }
        }
    }
}
