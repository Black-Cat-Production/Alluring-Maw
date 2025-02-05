using System;
using UnityEngine;

namespace Scripts.Core.AnimationScripts
{
    public class Door : MonoBehaviour
    {
        Animator animator;

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
            animator.SetTrigger("Close");
        }
    }
}