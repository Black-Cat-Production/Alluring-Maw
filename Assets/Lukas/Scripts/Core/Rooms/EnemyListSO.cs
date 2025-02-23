using System.Collections.Generic;
using Scripts.Core.Modules;
using UnityEngine;

namespace Scripts.Core.Rooms
{
    [CreateAssetMenu(menuName = "Scriptables/Lists/EnemyListSO")]
    public class EnemyListSO : ScriptableObject
    {
        [SerializeField] List<EnemyAIModule> enemyPrefabs;
        [SerializeField] List<EnemyAIModule> fallenAdventurerPrefabList;

        public EnemyAIModule GetRandomModuleFromList()
        {
            var list = GetRandomList();
            return list[Random.Range(0, list.Count)];
        }

        List<EnemyAIModule> GetRandomList()
        {
            if (fallenAdventurerPrefabList.Count == 0) return enemyPrefabs;
            var randomInt = Random.Range(0, 2);
            return randomInt == 0 ? enemyPrefabs : fallenAdventurerPrefabList;
        }
    }
}