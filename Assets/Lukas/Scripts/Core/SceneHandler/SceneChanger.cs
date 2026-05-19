using System.Collections;
using Scripts.Core.UI;
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

        [SerializeField] PresentTexts presentIntroText;

        [SerializeField] AudioClip startButtonSound;
        [SerializeField] AudioClip transitionSound;
        [SerializeField] AudioSource bgmSource;
        //AudioSource audioSource;

        AsyncOperation loadRoutine;
        Camera mainMenuCamera;
        Vector3 startPoint;

        void Awake()
        {
            mainMenuCamera = Camera.main;
            if (mainMenuCamera != null) startPoint = mainMenuCamera.transform.position;
            
        }

        public void LoadScene()
        {
            sceneLoader.Load();
        }

        public void LoadWithCameraPathing()
        {
            
            StartCoroutine(StartLoadWithCamera());
            mainMenuGroup.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        IEnumerator StartLoadWithCamera()
        {
            float minDuration = 2f;
            float timer = 0f;

            while (timer < minDuration)
            {
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / minDuration);
                mainMenuCamera.transform.position = Vector3.Lerp(startPoint, checkpoint1.transform.position, t);
               // bgmSource.volume = Mathf.Lerp(0.5f, 0, t);
            }

            timer = 0f;

            loadRoutine = SceneManager.LoadSceneAsync((int)EScenes.Game);
            loadRoutine.allowSceneActivation = false;
            while (timer < minDuration || loadRoutine.progress > 0.9f)
            {
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / minDuration);
                mainMenuCamera.transform.position = Vector3.Lerp(startPoint, checkpoint2.transform.position, t);
                //if (!audioSource.isPlaying)
                //{
                //    audioSource.clip = transitionSound;
                //    audioSource.Play();
                //}

                yield return null;
            }

            presentIntroText.PresentIntroText(loadRoutine);
        }
    }
}