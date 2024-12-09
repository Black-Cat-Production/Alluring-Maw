using System;
using UnityEngine;
using LL_Unity_Utils.Timers;
namespace Lukas.Scripts.Core
{
    public class Projectile : Skill
    {
        [SerializeField] float despawnTimerDuration;

        Timer despawnTimer;
        
        //Only for debugging
        [SerializeField] int damageValue;
        
        void Awake()
        {
            despawnTimer = new Timer(despawnTimerDuration);
            despawnTimer.StartTimer();
        }

        void FixedUpdate()
        {
            if(despawnTimer.CheckTimer()) Destroy(gameObject);
        }

        void OnCollisionEnter(Collision _hit)
        {
            if(_hit.gameObject.TryGetComponent(out HealthSystem healthSystem))
            {
                healthSystem.TakeDamage(damageValue);
            }
            Destroy(gameObject);
        }
    }
}