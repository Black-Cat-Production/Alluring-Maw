using UnityEngine;

namespace Lukas.Scripts.Core.SceneHandler
{
    public class SceneChanger : MonoBehaviour

    {
        [SerializeField] SceneLoader sceneLoader;

        public void LoadScene()
        {
            sceneLoader.Load();
        }
    }
}