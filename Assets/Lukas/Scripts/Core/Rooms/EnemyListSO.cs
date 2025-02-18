using System.Collections.Generic;
using Scripts.Core.Modules;
using UnityEngine;

namespace Scripts.Core.Rooms
{
    [CreateAssetMenu(menuName = "Scriptables/Lists/EnemyListSO")]
    public class EnemyListSO : ScriptableObject
    {
        [SerializeField] List<EnemyAIModule> enemyPrefabs;

        public EnemyAIModule GetRandomModuleFromList()
        {
            return enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        }
    }
}