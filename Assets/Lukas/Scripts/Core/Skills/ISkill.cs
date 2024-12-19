using System.Collections.Generic;
using Lukas.Scripts.Core.Modules;
using UnityEngine;

public interface ISkill
{
    void Use(Vector3 _startPosition, List<HealthSystemModule> _enemiesInRange);
}
