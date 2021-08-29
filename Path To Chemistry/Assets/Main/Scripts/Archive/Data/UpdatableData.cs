using System;
using UnityEditor;
using UnityEngine;

public class UpdatableData : ScriptableObject
{
    public bool autoUpdate;

    public event Action OnValuesUpdated;

#if UNITY_EDITOR

    protected virtual void OnValidate()
    {
        if (autoUpdate) EditorApplication.update += NotifyOfUpdatedValues;
    }

    public void NotifyOfUpdatedValues()
    {
        EditorApplication.update -= NotifyOfUpdatedValues;
        if (OnValuesUpdated != null) OnValuesUpdated();
    }

#endif
}