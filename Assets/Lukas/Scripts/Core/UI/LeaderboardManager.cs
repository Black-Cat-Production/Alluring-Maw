using System;
using System.Collections;
using Dan.Main;
using Lukas.Scripts.Core.Events;
using UnityEngine;

namespace Lukas.Scripts.Core.UI
{
    public class LeaderboardManager : MonoBehaviour
    {
        [SerializeField] NotifyEvent notifyEvent;
        [SerializeField] NotifyEvent nameChanged;
        static LeaderboardManager instance;
        public const string PublicLeaderboardKeyTime = "fd75063a77e518e3f564853366bfcd5a8b34d8457a29f399408f8a7de5df7f69";
        public const string PublicLeaderboardKeyDamage = "4e6ba1477fca70f969b3bed6730f8eec8256bc2ed612da124041a29ae6a14e19";
        string currentName;

        IEnumerator Start()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                while (!GameManager.Instance.FinishedLoading) yield return null;

                currentName = GameManager.Instance.GetPlayerName();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void OnEnable()
        {
            notifyEvent.OnNotify += SetLeaderboards;
            nameChanged.OnNotify += SetNewName;
        }

        void OnDisable()
        {
            notifyEvent.OnNotify -= SetLeaderboards;
            nameChanged.OnNotify -= SetNewName;
        }

        void SetNewName()
        {
            currentName = GameManager.Instance.GetPlayerName();
        }

        void SetLeaderboards()
        {
            GetScoresFromGameManager(out float timeTakenInMil, out int damageTaken);
            int milliseconds = (int)timeTakenInMil;
            SetLeaderboardEntry(currentName, milliseconds, PublicLeaderboardKeyTime);
            SetLeaderboardEntry(currentName, damageTaken, PublicLeaderboardKeyDamage);
        }

        void GetScoresFromGameManager(out float _timeScore, out int _damageTakenScore)
        {
            _timeScore = GameManager.Instance.TimeScore;
            _damageTakenScore = GameManager.Instance.DamageTakenScore;
        }


        void SetLeaderboardEntry(string _name, int _score, string _leaderboardKey)
        {
            LeaderboardCreator.ResetPlayer();
            LeaderboardCreator.SetUserGuid(Guid.NewGuid().ToString());
            LeaderboardCreator.UploadNewEntry(_leaderboardKey, _name, _score);
        }
    }
}