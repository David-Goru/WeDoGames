using UnityEngine;
using UnityEngine.Events;

public class KeyBinding
{
    KeyCode key;
    UnityAction action;

    public KeyCode Key { get => key; }
    public UnityAction Action { get => action; set => action = value; }

    public KeyBinding(KeyCode key, UnityAction action)
    {
        this.key = key;
        this.action = action;
    }

    public void Check()
    {
        if (Input.GetKeyDown(key)) action();
    }
}