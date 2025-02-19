using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Core.SceneHandler
{
    public class SceneChanger : MonoBehaviour

    {
        [SerializeField] SceneLoader sceneLoader;
        [SerializeField] CanvasGroup mainMenuGroup;
        [SerializeField] Transform checkpoint1;
        [SerializeField] Transform checkpoint2;

        AsyncOperation loadRoutine;
        Camera mainMenuCamera;
        Vector3 startPoint;

        void Awake()
        {
            mainMenuCamera = Camera.main;
            startPoint = mainMenuCamera.transform.position;
        }

        public void LoadScene()
        {
            sceneLoader.Load();
        }

        public void LoadWithCameraPathing()
        {
            loadRoutine = SceneManager.LoadSceneAsync((int)EScenes.Game);
            loadRoutine.allowSceneActivation = false;
            StartCoroutine(StartLoadWithCamera());
            mainMenuGroup.gameObject.SetActive(false);
        }

        IEnumerator StartLoadWithCamera()
        {
            float minDuration = 2f;
            float timer = 0f;
            
            while (timer < minDuration || loadRoutine.progress > 0.9f)
            {
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / minDuration);
                mainMenuCamera.transform.position = Vector3.Lerp(startPoint, checkpoint2.transform.position, t);
                
                yield return null;
            }

            loadRoutine.allowSceneActivation = true;
        }
    }
}