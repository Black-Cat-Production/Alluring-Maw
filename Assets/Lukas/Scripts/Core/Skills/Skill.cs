using System.Collections;
using System.Collections.Generic;
using Lukas.Scripts.Core.Skills.Effects;
using UnityEngine;

public abstract class Skill : MonoBehaviour, ISkill
{
   public string SkillName;
   public int ShootingSpeed;
   [SerializeField] protected EffectData effectData;

   string ISkill.SkillName
   {
      get => SkillName;
      set => throw new System.NotImplementedException();
   }
}
