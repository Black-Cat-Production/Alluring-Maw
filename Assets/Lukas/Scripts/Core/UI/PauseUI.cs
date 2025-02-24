using Scripts.Core.Events;
using UnityEngine;

namespace Scripts.Core.UI
{
    public class PauseUI : MonoBehaviour
    {
        [SerializeField] Canvas pauseUICanvas;
        [SerializeField] NotifyEvent notifyPause;

        public void TogglePauseUI()
        {
            notifyPause.Invoke(!GameManager.Instance.IsPaused);
            pauseUICanvas.gameObject.SetActive(GameManager.Instance.IsPaused);
        }
    }
}