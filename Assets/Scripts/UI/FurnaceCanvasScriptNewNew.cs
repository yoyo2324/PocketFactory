﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceCanvasScriptNewNew : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Activate()
    {
        this.gameObject.SetActive(true);
    }

    public void Activate(FurnaceController furnaceController)
    {

    }

    public void Deactivate()
    {
        this.gameObject.SetActive(false);
    }
}
