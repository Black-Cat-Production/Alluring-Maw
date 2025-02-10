using System;
using UnityEngine;

namespace Scripts.Core.AnimationScripts
{
    public class Door : MonoBehaviour
    {
        Animator animator;

        bool isClosed;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void Open()
        {
            animator.SetTrigger("Open");
        }

        public void Close()
        {
            if (isClosed) return;
            animator.SetTrigger("Close");
            isClosed = true;
        }
    }
}