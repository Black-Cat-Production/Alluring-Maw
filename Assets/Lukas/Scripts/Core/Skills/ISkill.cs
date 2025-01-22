using System.Collections.Generic;
using Lukas.Scripts.Core.Modules;
using UnityEngine;

namespace Lukas.Scripts.Core.Skills
{
    public interface ISkill
    {
        void Use(Vector3 _startPosition, List<HealthSystemModule> _enemiesInRange);
    }
}