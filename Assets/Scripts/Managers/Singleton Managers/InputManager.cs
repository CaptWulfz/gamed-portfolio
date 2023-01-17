using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>, ISingleton
{
    private Controls controls = null;
    private bool allowInput;
    public bool AllowInput
    {
        get { return this.allowInput; }
    }

    private bool isDone = false;
    public bool IsDone
    {
        get { return this.isDone; }
    }

    public void Initialize()
    {
        if (controls == null)
            controls = new Controls();

        this.allowInput = true;
        this.isDone = true;
    }

    public Controls GetControls()
    {
        if (this.controls == null)
            Initialize();

        return this.controls;
    }

    public void ToggleDevice(InputDevice device, bool active)
    {
        if (active)
            InputSystem.EnableDevice(device);
        else
            InputSystem.DisableDevice(device);
    }

    public void ToggleAllowInput(bool active)
    {
        this.allowInput = active;
    }

    public void CreateInputDelay(float inputDelay, Action onFinish = null)
    {
        StartCoroutine(CreateInputDelayCoroutine(inputDelay, onFinish));
    }

    private IEnumerator CreateInputDelayCoroutine(float inputDelay, Action onFinish = null)
    {
        this.allowInput = false;
        yield return new WaitForSeconds(inputDelay);
        this.allowInput = true;
        onFinish?.Invoke();
    }
}
