using UnityEngine;

namespace Scripts.Core.UI
{
    public class CreditsUI : MonoBehaviour
    {
        [SerializeField] Canvas creditsCanvas;
        [SerializeField] Animator creditsAnimator;

        public void ShowUI()
        {
            creditsCanvas.gameObject.SetActive(true);
            creditsAnimator.SetTrigger("StartCredits");
        }

        public void HideUI()
        {
            creditsCanvas.gameObject.SetActive(false);
        }
    }
}