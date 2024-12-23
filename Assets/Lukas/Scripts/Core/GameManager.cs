using Lukas.Scripts.Core.Modules;
using Lukas.Scripts.Core.Rooms;
using UnityEditor;
using UnityEngine;

namespace Lukas.Scripts.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void RegisterLastRoom(RoomSpawner _lastRoom)
        {
            _lastRoom.OnFinalRoomCleared += TriggerWin;
        }

        void TriggerWin()
        {
            Debug.Log("You won the game!!");
            EditorApplication.ExitPlaymode(); 
        }

        public void TriggerLoss()
        {
            Debug.Log("You lost!");
            EditorApplication.ExitPlaymode();
        }
    }
}