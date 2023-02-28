using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";
        [SerializeField] private float fadeInTime = 1;
        private SavingSystem savingSystem;
        private void Awake()
        {
            savingSystem = GetComponent<SavingSystem>();
        }

        IEnumerator Start()
        {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return savingSystem.LoadLastScene(defaultSaveFile);
            fader.FadeOut(fadeInTime);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
                Load();
            if (Input.GetKeyDown(KeyCode.S))
                Save();
            if (Input.GetKeyDown(KeyCode.Delete))
                Delete();
        }

        public void Load()
        {
            savingSystem.Load(defaultSaveFile);
        }

        public void Save()
        {
            savingSystem.Save(defaultSaveFile);
        }

        public void Delete()
        {
            savingSystem.Delete(defaultSaveFile);
        }
    }
}
