﻿using Assets;
using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Newtonsoft;
using Newtonsoft.Json;
using UnityEngine.EventSystems;

public class GameManagerController : MonoBehaviour
{
    public GUIManagerController gUIManagerController;

    private static GameManagerController instance;
    public static GameManagerController Instance
    {
        get
        {
            if (instance is null)
            {
                instance = new GameManagerController();
            }

            return instance;
        }
    }

    private bool isPaused;
    public bool IsPaused
    {
        get => this.isPaused;
    }

    public GameManagerController()
    {
        this.isPaused = false;
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;

        this.LoadSaveFile();
    }

    private void OnApplicationPause(bool isPaused)
    {
        if (isPaused)
        {
            this.Save();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnApplicationQuit()
    {
        this.Save();
    }

    private void Save()
    {
        File.Delete(Path.Combine(Application.persistentDataPath, "PlayerSave.json.bak.old"));

        if (File.Exists(Path.Combine(Application.persistentDataPath, "PlayerSave.json")))
        {
            File.Move(Path.Combine(Application.persistentDataPath, "PlayerSave.json"), Path.Combine(Application.persistentDataPath, "PlayerSave.json.bak.old"));
        }

        GameSaveModel gameSaveModel = new GameSaveModel();
        gameSaveModel.PlacedMachineModels = UnityEngine.Object.FindObjectsOfType<GameObject>().Where(x => x.layer == 8).ToList().ToMachineModelList();
        gameSaveModel.PlayerModel = Player.playerModel;
        gameSaveModel.ResearchDatabase = ResearchDatabase.database;
        gameSaveModel.BlueprintDatabase = BlueprintDatabase.database;

        File.WriteAllText(Path.Combine(Application.persistentDataPath, "PlayerSave.json"), Newtonsoft.Json.JsonConvert.SerializeObject(gameSaveModel, new Newtonsoft.Json.JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore }));

        //Debug.Log($"Ended at {System.DateTime.Now.Ticks} with {GameObject.FindGameObjectsWithTag("Resource").Sum(x => x.GetComponent<ResourceController>().resource.Quantity)} resources");
    }

    private void LoadSaveFile()
    {
        if (File.Exists(Path.Combine(Application.persistentDataPath, "PlayerSave.json")))
        {
            GameSaveModel gameSaveModel = Newtonsoft.Json.JsonConvert.DeserializeObject<GameSaveModel>(File.ReadAllText(Path.Combine(Application.persistentDataPath, "PlayerSave.json")));

            gameSaveModel.PlacedMachineModels.ToGameObjectList();
            Player.playerModel.SetValues(gameSaveModel.PlayerModel);
            ResearchDatabase.OverwriteFromSave(gameSaveModel.ResearchDatabase);
            BlueprintDatabase.OverwriteFromSave(Newtonsoft.Json.JsonConvert.SerializeObject(gameSaveModel.BlueprintDatabase));
        }
        else
        {
            Player.playerModel.SetMoney(ResearchDatabase.GetRemainingResearchCost() + 1000000);
        }
    }

    public bool PlayPause()
    {
        this.isPaused = !this.isPaused;
        return this.isPaused;
    }
}
