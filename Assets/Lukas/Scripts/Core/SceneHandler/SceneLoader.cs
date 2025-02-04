using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Core.SceneHandler
{
    [CreateAssetMenu(menuName = "Scriptables/Scene/SceneLoader")]
    public class SceneLoader : ScriptableObject
    {
        [SerializeField] EScenes sceneToLoad;


        public void Load()
        {
            SceneManager.LoadScene((int)sceneToLoad);
        }

        public void LoadAsync()
        {
            SceneManager.LoadSceneAsync((int)sceneToLoad);
        }
    }
}