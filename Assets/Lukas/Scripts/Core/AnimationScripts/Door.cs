using System.Collections;
using UnityEngine;
using Event = AK.Wwise.Event;

namespace Scripts.Core.AnimationScripts
{
    public class Door : MonoBehaviour
    {
        [SerializeField] float closeDelay;
        [SerializeField] bool isStartDoor;
        [SerializeField] Event doorOpenSoundEvent;
        [SerializeField] Event doorCloseSoundEvent;
        Animator animator;

        bool isClosed;

        void Awake()
        {
            animator = GetComponent<Animator>();
            if (!isStartDoor)
            {
                Open();
                isClosed = false;
            }
            else
            {
                isClosed = true;
            }
        }

        public void Open()
        {
            animator.SetTrigger("Open");
            AkSoundEngine.PostEvent(doorOpenSoundEvent.Name, gameObject);
        }

        public void Close()
        {
            if (isClosed) return;
            StartCoroutine(CloseDoorWithDelay(closeDelay));
        }

        IEnumerator CloseDoorWithDelay(float _delay)
        {
            yield return new WaitForSeconds(_delay);
            animator.SetTrigger("Close");
            isClosed = true;
            AkSoundEngine.PostEvent(doorCloseSoundEvent.Name, gameObject);
        }
    }
}