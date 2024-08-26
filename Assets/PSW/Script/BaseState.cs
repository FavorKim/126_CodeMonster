using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState<T>  where T : class
{
    protected T Controller { get; set; }

    public BaseState(T controller)
    {
        this.Controller = controller;
    }

    public abstract void OnEnterState();
    public abstract void OnUpdateState();
    public abstract void OnFixedUpdateState();
    public abstract void OnExitState();
}
