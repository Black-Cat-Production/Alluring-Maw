using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Core.Rooms
{
    public class RoomCollider : MonoBehaviour
    {
        [SerializeField] RoomSpawner roomParent;
        [SerializeField] List<GameObject> torchFireParentObjects;


        void OnTriggerEnter(Collider _collider)
        {
            if (_collider.gameObject.name == "Player")
            {
                roomParent.TriggerRoomEntered();
                roomParent.SetSideEnteredFrom(this);
            }
        }

        public void DisableTorches()
        {
            foreach (var torchlight in torchFireParentObjects)
            {
                torchlight.gameObject.SetActive(false);
            }
        }
    }
}