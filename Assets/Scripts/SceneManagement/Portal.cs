using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;
using UnityEngine.AI;
namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D
        }
        [SerializeField] private int sceneToLoad = -1;
        [SerializeField] private DestinationIdentifier destinationIdentifier;
        [SerializeField] private float fadeOutTime = 3f;
        [SerializeField] private float fadeInTime = 1f;
        [SerializeField] private float fadeWaitTime = 0.5f;
        private PlayerController playerController;
        public Transform spawnPoint;

        private void Awake()
        {
            GetPlayerController();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError("No scenes to load");
                yield break;
            }
            DontDestroyOnLoad(gameObject);
            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            PlayerControllerActivation(false);
            fader.FadeIn(fadeInTime);
            savingWrapper.Save();
            yield return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneToLoad);
            GetPlayerController();
            PlayerControllerActivation(false);
            savingWrapper.Load();
            UpdatePlayer(GetOtherPortal());
            savingWrapper.Save();
            yield return new WaitForSeconds(fadeWaitTime);
            fader.FadeOut(fadeOutTime);
            PlayerControllerActivation(true);
            Destroy(gameObject);

        }

        private void UpdatePlayer(Portal otherPortal)
        {
            Transform player = GameObject.Find("Player").transform;
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.rotation = spawnPoint.rotation;
        }

        private Portal GetOtherPortal()
        {
            Portal[] portals = FindObjectsOfType<Portal>();
            foreach (Portal portal in portals)
            {
                if (portal == this) continue;
                if (portal.destinationIdentifier != destinationIdentifier) continue;
                return portal;
            }
            return null;
        }

        private void PlayerControllerActivation(bool flag)
        {
            playerController.enabled = flag;
        }

        private void GetPlayerController()
        {
            playerController = FindObjectOfType<PlayerController>();
        }
    }
}
