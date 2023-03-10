using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        private CanvasGroup canvasGroup;
        private Coroutine currentlyActiveFade;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 0;
        }
        public Coroutine FadeIn(float time)
        {
             return Fade(0,time);
        }

        public Coroutine FadeOut(float time)
        {
             return Fade(1,time);
        }
        private Coroutine Fade(float target, float time)
        {
            if (currentlyActiveFade != null) StopCoroutine(currentlyActiveFade);
            currentlyActiveFade = StartCoroutine(FadeRoutine(target,time));
             return currentlyActiveFade;
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, target))
            {
                canvasGroup.alpha += Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }
       
    }
}
