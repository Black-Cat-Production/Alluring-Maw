using System;
using System.Collections;
using System.Collections.Generic;
using Dan.Main;
using Lukas.Scripts.Core.Events;
using Lukas.Scripts.Core.System;
using TMPro;
using UnityEngine;

namespace Lukas.Scripts.Core.UI
{
    public class LeaderboardUI : MonoBehaviour
    {
        [SerializeField] List<TextMeshProUGUI> names;
        [SerializeField] List<TextMeshProUGUI> scores;
        [SerializeField] TextMeshProUGUI header;
        [SerializeField] Canvas leaderboardCanvas;
        
        public void ShowUI()
        {
            leaderboardCanvas.gameObject.SetActive(true);
            DisplayLeaderboard(ELeaderboardType.TimeToComplete);
        }

        public void HideUI()
        {
            leaderboardCanvas.gameObject.SetActive(false);
        }
        
        void ResetTexts()
        {
            foreach (var userName in names)
            {
                userName.text = "";
            }

            foreach (var score in scores)
            {
                score.text = "";
            }
        }

        public void DisplayLeaderboard(ELeaderboardType _type)
        {
            ResetTexts();
            switch (_type)
            {
                case ELeaderboardType.TimeToComplete:
                    GetLeaderboard(LeaderboardManager.PublicLeaderboardKeyTime);
                    break;
                case ELeaderboardType.DamageTaken:
                    GetLeaderboard(LeaderboardManager.PublicLeaderboardKeyDamage);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_type), _type, null);
            }

            UpdateHeader(_type);
        }

        void GetLeaderboard(string _leaderboardKey)
        {
            LeaderboardCreator.GetLeaderboard(_leaderboardKey, ((_msg) =>
            {
                int loopLength = (_msg.Length < names.Count) ? _msg.Length : names.Count;
                for (int i = 0; i < loopLength; i++)
                {
                    names[i].text = _msg[i].Username;
                    scores[i].text = _leaderboardKey switch
                    {
                        LeaderboardManager.PublicLeaderboardKeyTime => BuildTimeScoreDisplay(_msg[i].Score),
                        LeaderboardManager.PublicLeaderboardKeyDamage => _msg[i].Score.ToString(),
                        _ => scores[i].text
                    };
                }
            }));
        }

        void UnpackTimeSpan(TimeSpan _timeSpan, out int _minutes, out int _seconds, out int _milliseconds)
        {
            _minutes = _timeSpan.Minutes;
            _seconds = _timeSpan.Seconds;
            _milliseconds = _timeSpan.Milliseconds;
        }

        string BuildTimeScoreDisplay(float _milliseconds)
        {
            Debug.Log(_milliseconds);
            var timeSpan = TimeSpan.FromMilliseconds(_milliseconds);
            UnpackTimeSpan(timeSpan, out int min, out int sec, out int mil);
            return $"{min}m : {sec}s : {mil}ms";
        }

        void UpdateHeader(ELeaderboardType _type)
        {
            header.text = _type switch
            {
                ELeaderboardType.TimeToComplete => "Leaderboard (Time)",
                ELeaderboardType.DamageTaken => "Leaderboard (Damage)",
                _ => throw new ArgumentOutOfRangeException(nameof(_type), _type, null)
            };
        }
    }
}