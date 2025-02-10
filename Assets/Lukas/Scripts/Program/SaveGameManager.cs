using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NaughtyAttributes;
using Newtonsoft.Json;
using Scripts.Core;
using Scripts.Core.Skills.SkillTree;
using Scripts.Core.System;
using UnityEngine;

namespace Scripts.Program
{
    public class SaveGameManager : MonoBehaviour
    {
        [SerializeField] SkillTreeNodeRegistry registry;
        [SerializeField] SaveGameSO saveGameSO;
        [SerializeField] SaveGameSO defaultSaveGameSO;
        [SerializeField] OptionsSaveSO optionsSaveSO;

        const string SaveFolder = "ScriptableObjectSaves";
        string savePathSaveGameSO;
        string savePathRegistry;
        string savePathOptions;
        [NonSerialized] public bool SavePathsCreated;

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
                savePathOptions = Path.Combine(Application.persistentDataPath, SaveFolder, "options.json");
                SavePathsCreated = true;
#else
                savePathSaveGameSO = Path.Combine(Application.dataPath, SaveFolder, "saveGameSO.json");
                savePathRegistry = Path.Combine(Application.dataPath, SaveFolder, "registry.json");
                savePathOptions = Path.Combine(Application.dataPath, SaveFolder, "options.json"); 
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

            string json = JsonConvert.SerializeObject(saveGameSO);
            File.WriteAllText(savePathSaveGameSO, json);
            Debug.Log($"Saved ScriptableObject to {savePathSaveGameSO}");

            json = JsonConvert.SerializeObject(optionsSaveSO);
            File.WriteAllText(savePathOptions, json);

            var serializedSkillTreeData = registry.SkillTreeNodesData.Select(JsonUtility.ToJson).ToList();
            var testData = registry.SkillTreeNodesData.Select(_nodeDataSO => JsonConvert.SerializeObject(_nodeDataSO, Formatting.Indented)).ToList();
            json = JsonConvert.SerializeObject(testData, Formatting.Indented);
            File.WriteAllText(savePathRegistry, json);
        }

        [Button]
        public void Load()
        {
            if (File.Exists(savePathSaveGameSO))
            {
                string json = File.ReadAllText(savePathSaveGameSO);
                JsonUtility.FromJsonOverwrite(json, saveGameSO);
                Debug.Log("Loaded ScriptableObject!");

                json = File.ReadAllText(savePathOptions);
                JsonUtility.FromJsonOverwrite(json, optionsSaveSO);


                json = File.ReadAllText(savePathRegistry);
                var serializedSkillTreeData = JsonConvert.DeserializeObject<List<string>>(json);
                for (int i = 0; i < serializedSkillTreeData.Count; i++) JsonUtility.FromJsonOverwrite(serializedSkillTreeData[i], registry.SkillTreeNodesData[i]);
            }
            else
            {
                saveGameSO.HasSaved = defaultSaveGameSO.HasSaved;
                saveGameSO.MemoryFragmentsAmount = defaultSaveGameSO.MemoryFragmentsAmount;
                GameManager.Instance.ResetPlayerName();
                Debug.LogWarning("Save file not found!");
            }
        }
    }
}