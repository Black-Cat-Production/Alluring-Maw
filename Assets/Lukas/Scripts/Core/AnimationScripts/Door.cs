using System;
using System.Collections;
using UnityEngine;

namespace Scripts.Core.AnimationScripts
{
    public class Door : MonoBehaviour
    {
        [SerializeField] float closeDelay;
        Animator animator;

        bool isClosed;

        void Awake()
        {
            animator = GetComponent<Animator>();
            Open();
            isClosed = false;
        }

        public void Open()
        {
            animator.SetTrigger("Open");
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
        }
    }
}