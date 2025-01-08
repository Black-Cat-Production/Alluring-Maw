using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lukas.Scripts.Core.Skills;
using Lukas.Scripts.Core.System;
using NaughtyAttributes;
using Newtonsoft.Json;
using UnityEngine;

namespace Lukas.Scripts.Program
{
    public class SaveGameManager : MonoBehaviour
    {
        [SerializeField] SkillTreeNodeRegistry registry;
        [SerializeField] SaveGameSO saveGameSO;

        const string SaveFolder = "ScriptableObjectSaves";
        string savePathSaveGameSO;
        string savePathRegistry;
        public bool SavePathsCreated;

        static SaveGameManager instance;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
                #if UNITY_EDITOR
                savePathSaveGameSO = Path.Combine(Application.persistentDataPath, SaveFolder, "saveGameSO.json");
                savePathRegistry = Path.Combine(Application.persistentDataPath, SaveFolder, "registry.json");
                SavePathsCreated = true;
#else
                savePathSaveGameSO = Path.Combine(Application.dataPath, SaveFolder, "saveGameSO.json");
                savePathRegistry = Path.Combine(Application.dataPath, SaveFolder, "registry.json");
                SavePathsCreated = true;
#endif
            }
            else
            {
                Destroy(gameObject);
            }
        }


        [Button]
        public void Save()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePathSaveGameSO));

            string json = JsonUtility.ToJson(saveGameSO);
            File.WriteAllText(savePathSaveGameSO,json);
            Debug.Log($"Saved ScriptableObject to {savePathSaveGameSO}");
            

            var serializedSkillTreeData = registry.SkillTreeNodesData.Select(JsonUtility.ToJson).ToList();
            json = JsonConvert.SerializeObject(serializedSkillTreeData, Formatting.Indented);
            File.WriteAllText(savePathRegistry,json);
        }
        
        [Button]
        public void Load()
        {
            if (File.Exists(savePathSaveGameSO))
            {
                string json = File.ReadAllText(savePathSaveGameSO);
                JsonUtility.FromJsonOverwrite(json,saveGameSO);
                Debug.Log("Loaded ScriptableObject!");
                
                
                json = File.ReadAllText(savePathRegistry);
                var serializedSkillTreeData = JsonConvert.DeserializeObject<List<string>>(json);
                for (int i = 0; i < serializedSkillTreeData.Count; i++)
                {
                    JsonUtility.FromJsonOverwrite(serializedSkillTreeData[i],registry.SkillTreeNodesData[i]);
                }
            }
            else
            {
                Debug.LogWarning("Save file not found!");
            }
        }
    }
}