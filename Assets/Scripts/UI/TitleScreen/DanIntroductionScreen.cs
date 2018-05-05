using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DanIntroductionScreen : MonoBehaviour {

    [SerializeField, TextArea]
    private string danTextFormatter = "Hi, I'm {0} but you can just call me DAN!";

    [SerializeField]
    private Text danTextDisplay = null;

    /// <summary>
    /// Handles the dan name.
    /// </summary>
    /// <param name="danName">Dan name.</param>
    public void InitializeDanScreen(string danName) {
        this.danTextDisplay.text = string.Format(this.danTextFormatter, danName);
    }
}
