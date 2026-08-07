using System;
using System.Collections;
using Scripts.Core;
using UnityEngine;
using Event = AK.Wwise.Event;
using Random = UnityEngine.Random;

namespace WWISE_Integration_Scripts
{
    public class JumpingAtmoAudio : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] GameObject playerObject;
        [SerializeField] Event playRandomAtmoSoundEvent;

        [Header("Sound Play Settings")]
        [SerializeField] [Range(0, 20)] float minSecondsToWait;
        [SerializeField][Range(0,20)] float maxSecondsToWait;

        [Header("Object Teleport Settings")]
        [SerializeField] [Range(0, 30)] float minSecondsToWaitForJump;
        [SerializeField][Range(0, 30)] float maxSecondsToWaitForJump;
        [SerializeField] int distanceToPlayerInMeter;

        void Start()
        {
            StartCoroutine(PlaySoundsRoutine());
            StartCoroutine(TeleportObject());
        }

        IEnumerator PlaySoundsRoutine()
        {
            while (gameObject.activeInHierarchy)
            {
                float randomWaitTime = Random.Range(minSecondsToWait, maxSecondsToWait);
                yield return new WaitForSeconds(randomWaitTime);
                AkSoundEngine.PostEvent(playRandomAtmoSoundEvent.Name, gameObject);
                yield return null;
            }
        }

        IEnumerator TeleportObject()
        {
            while (gameObject.activeInHierarchy)
            {
                var unitSphere = Random.insideUnitSphere * distanceToPlayerInMeter;
                var randomPoint = new Vector3(unitSphere.x, 0, unitSphere.z);
                randomPoint += playerObject.transform.position;
                transform.position = randomPoint;
                float randomWaitTime = Random.Range(minSecondsToWaitForJump, maxSecondsToWaitForJump);
                yield return new WaitForSeconds(randomWaitTime);
                yield return null;
            }
        }
    }
}