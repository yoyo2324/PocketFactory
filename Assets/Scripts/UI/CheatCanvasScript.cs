﻿using Assets;
using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CheatCanvasScript : MonoBehaviour
{
    public InputField inputField;

    public Dictionary<string, Action<string[]>> cheatDict = new Dictionary<string, Action<string[]>>(StringComparer.InvariantCultureIgnoreCase);

    private void Awake()
    {
        cheatDict.Add("player.addMoney", (moneyAmount) => {
            Debug.Log(string.Format("Adding {0} money", moneyAmount));
            Player.playerModel.AddMoney(Convert.ToInt64(moneyAmount.FirstOrDefault()));
        });

        cheatDict.Add("player.setMoney", (moneyAmount) =>
        {
            Player.playerModel.Money = Convert.ToInt64(moneyAmount.FirstOrDefault());
        });

        cheatDict.Add("player.unlockAllMachines", (_) =>
        {
            Debug.Log("Unlocking all machines");
            PrefabDatabase.Instance.GetPrefabsForType("Machine").Values.ToList().ForEach(x => x.GetComponent<MachineController>().Machine.IsUnlocked = true);
        });

        cheatDict.Add("ui.recipeCanvas", (_) =>
        {
            GameObject recipeCanvas = PrefabDatabase.Instance.GetPrefab("UI", "Recipe");
            recipeCanvas.GetComponent<RecipesPanelScript>().Activate("Recipe", (recipe) => { });
        });

        cheatDict.Add("resource.spawn", (resourceInfo) =>
        {
            Resource resourceToSpawn = new Resource(ResourceDatabase.GetResource(resourceInfo[0]).id, Convert.ToInt64(resourceInfo[1]));
            Camera.main.GetComponent<PlayerScript>().SpawnResource(resourceToSpawn);
        });

        cheatDict.Add("game.save", (_) =>
        {
            Debug.Log("Saving game");
            File.Delete(Path.Combine(Application.persistentDataPath, "PlayerSave.json.bak.old"));

            if (File.Exists(Path.Combine(Application.persistentDataPath, "PlayerSave.json")))
            {
                File.Move(Path.Combine(Application.persistentDataPath, "PlayerSave.json"), Path.Combine(Application.persistentDataPath, "PlayerSave.json.bak.old"));
            }

            GameSaveModel gameSaveModel = new GameSaveModel();
            gameSaveModel.PlacedMachineModels = UnityEngine.Object.FindObjectsOfType<GameObject>().Where(x => x.layer == 8).ToList().ToMachineModelList();
            //gameSaveModel.PlayerModel = playerScriptableObject.ToPlayerModel();
            gameSaveModel.ResearchDatabase = ResearchDatabase.database;

            File.WriteAllText(Path.Combine(Application.persistentDataPath, "PlayerSave.json"), Newtonsoft.Json.JsonConvert.SerializeObject(gameSaveModel));
        });

        cheatDict.Add("ak", (_) =>
        {
            TouchScreenKeyboard.Open("", TouchScreenKeyboardType.ASCIICapable);
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void XButton_Click()
    {
        this.Deactivate();
    }

    public void Activate()
    {
        this.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        this.gameObject.SetActive(false);
    }

    public void EnterCheatButton_Click()
    {
        string text = this.inputField.text;
        string[] pieces = text.Split(' ');

        if (cheatDict.TryGetValue(pieces[0], out Action<string[]> existingAction))
        {
            existingAction(pieces.Skip(1).ToArray());
        }
        else
        {
            Debug.Log(string.Format("Cheat {0} not found", pieces[0]));
        }
    }
}
