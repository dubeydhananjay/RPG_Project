using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Control;
using RPG.Core;
namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        private PlayerController playerController;
        private PlayableDirector playableDirector;

        private void Awake()
        {
            playerController = FindObjectOfType<PlayerController>();
            playableDirector = GetComponent<PlayableDirector>();
        }

        private void OnEnable()
        {
            playableDirector.stopped += OnEnableController;
            playableDirector.played += OnDisableController;
        }

        private void OnDisable()
        {
            playableDirector.stopped -= OnEnableController;
            playableDirector.played -= OnDisableController;
        }

        private void OnEnableController(PlayableDirector playableDirector)
        {
            PlayerControllerActivation(true);
        }

        public void OnDisableController(PlayableDirector playableDirector)
        {
            playerController.GetComponent<ActionScheduler>().CancelCurrentAction();
            PlayerControllerActivation(false);
        }

        private void PlayerControllerActivation(bool flag)
        {
            playerController.enabled = flag;
        }
    }
}
