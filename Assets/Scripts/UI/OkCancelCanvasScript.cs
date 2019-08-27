﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OkCancelCanvasScript : MonoBehaviour
{
    public Text instructionText;
    public Text instructionText2;
    private Action okButtonAction;
    private Action cancelButtonAction;
    public GameObject Navbar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {
        this.gameObject.SetActive(true);
        this.Navbar.SetActive(false);
    }

    public void Activate(Action okButtonAction, Action cancelButtonAction)
    {
        this.okButtonAction = okButtonAction;
        this.cancelButtonAction = cancelButtonAction;
        this.Activate();
    }

    public void Activate(string instructionText, Action okButtonAction, Action cancelButtonAction)
    {
        UpdateInstructionText(instructionText);
        this.okButtonAction = okButtonAction;
        this.cancelButtonAction = cancelButtonAction;
        this.Activate();
    }

    public void Deactivate()
    {
        this.gameObject.SetActive(false);
        this.Navbar.SetActive(true);
    }

    public void OkButton_Clicked()
    {
        okButtonAction?.DynamicInvoke();
        this.Deactivate();
    }

    public void CancelButton_Clicked()
    {
        cancelButtonAction?.DynamicInvoke();
        this.Deactivate();
    }

    public void UpdateInstructionText(string instructionText)
    {
        this.instructionText.text = instructionText;
    }

    public void UpdateInstructionText2(string instructionText)
    {
        this.instructionText2.text = instructionText;
    }
}