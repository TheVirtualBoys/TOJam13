using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameDanScreen : MonoBehaviour {

    public System.Action<string> OnDanNameEntered = null;

    /// <summary>
    /// The dan name field.
    /// </summary>
    [SerializeField]
    private InputField danNameField = null;

    /// <summary>
    /// The handles the submit name being entered for dan.
    /// </summary>
    public void HandleDanNameComplete() {
        
    }
}
