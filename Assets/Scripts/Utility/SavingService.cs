using System;
using System.Collections.Generic;
using System.IO;
using Gameplay.Bootstrapping;
using Gameplay.Execution.Models;
using UnityEngine;
using Newtonsoft.Json;
using Zenject;

public class SavingService : IInitializable
{
    private const string fileName = "gameSave.json";
    
    private GameSave gameSave = new GameSave();

    public void Initialize()
    {
        if (File.Exists(Path.Combine(Application.persistentDataPath, fileName)))
        {
            ReadGameSave(File.ReadAllText(Path.Combine(Application.persistentDataPath, fileName)));
        }
    }

    private void ReadGameSave(string json)
    {
        gameSave = JsonConvert.DeserializeObject<GameSave>(json);
    }

    public void SaveGame(GameplayContext gameplayContext)
    {
        gameSave.CampaignSaves[gameplayContext.OpponentConfig.Name] = gameplayContext.GameplayStateModel;
        WriteToFile();
    }

    private void WriteToFile()
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(path, JsonConvert.SerializeObject(gameSave, Formatting.Indented));
        Debug.Log($"Wrote game save to {path}");
    }

    public bool TryGetGameplayStateModel(out GameplayStateModel gameplayStateModel, string opponentName)
    {
        return gameSave.CampaignSaves.TryGetValue(opponentName, out gameplayStateModel);
    }
}

[Serializable]
public class GameSave
{ 
    public Dictionary<string, GameplayStateModel> CampaignSaves = new Dictionary<string, GameplayStateModel>();
}
