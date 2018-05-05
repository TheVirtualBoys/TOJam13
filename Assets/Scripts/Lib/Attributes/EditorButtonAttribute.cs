using UnityEngine;

/// <summary>
/// Stick [EditorButton] above a method defintion to make a button appear in the component inspector
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Method)]
public class EditorButtonAttribute : PropertyAttribute
{
    public EditorButtonAttribute() {
    }
}
