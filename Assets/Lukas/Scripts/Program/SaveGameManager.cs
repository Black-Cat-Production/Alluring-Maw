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

        static SaveGameManager instance;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }


        [Button]
        public void Save()
        {
            string savePath = Path.Combine(Application.persistentDataPath, SaveFolder, $"saveGameSO.json");
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));

            string json = JsonUtility.ToJson(saveGameSO);
            File.WriteAllText(savePath,json);
            Debug.Log($"Saved ScriptableObject to {savePath}");

            savePath = Path.Combine(Application.persistentDataPath, SaveFolder, $"registry.json");

            var serializedSkillTreeData = registry.SkillTreeNodesData.Select(JsonUtility.ToJson).ToList();
            json = JsonConvert.SerializeObject(serializedSkillTreeData, Formatting.Indented);
            File.WriteAllText(savePath,json);
        }
        
        [Button]
        public void Load()
        {
            string savePath = Path.Combine(Application.persistentDataPath, SaveFolder, $"saveGameSO.json");
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                JsonUtility.FromJsonOverwrite(json,saveGameSO);
                Debug.Log("Loaded ScriptableObject!");
                
                savePath = Path.Combine(Application.persistentDataPath, SaveFolder, $"registry.json");
                json = File.ReadAllText(savePath);
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