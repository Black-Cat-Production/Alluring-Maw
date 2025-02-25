using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace Scripts.Core.SceneHandler
{
    public class SplashScreen : MonoBehaviour
    {
        [SerializeField] VideoPlayer videoPlayer;

        void Start()
        {
            videoPlayer.loopPointReached += OnVideoFinished;
            videoPlayer.Play();
        }

        void OnVideoFinished(VideoPlayer _source)
        {
           var loadRoutine = SceneManager.LoadSceneAsync((int)EScenes.MainMenu);
           loadRoutine.allowSceneActivation = false;
           StartCoroutine(LoadAsync(loadRoutine));
        }

        IEnumerator LoadAsync(AsyncOperation _loadRoutine)
        {
            while (_loadRoutine.progress < 0.9f)
            {
                yield return null;
            }
            _loadRoutine.allowSceneActivation = true;
        }
    }
}