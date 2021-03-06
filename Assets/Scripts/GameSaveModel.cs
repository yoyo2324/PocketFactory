﻿using Assets;
using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameSaveModel
{
    public List<MachineModel> PlacedMachineModels;
    public PlayerModel PlayerModel;
    public Dictionary<string, Dictionary<string, Research>> ResearchDatabase;
    public Dictionary<string, object> BlueprintDatabase;
}
