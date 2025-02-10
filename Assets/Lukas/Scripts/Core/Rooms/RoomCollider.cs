using System;
using UnityEngine;

namespace Scripts.Core.Rooms
{
    public class RoomCollider : MonoBehaviour
    {
        [SerializeField] RoomSpawner roomParent;


        void OnTriggerEnter(Collider _collider)
        {
            if (_collider.gameObject.name == "Player")
            {
                roomParent.TriggerRoomEntered();
            }
        }
    }
}