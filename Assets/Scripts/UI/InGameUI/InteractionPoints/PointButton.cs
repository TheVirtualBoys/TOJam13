using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointButton : MonoBehaviour {

    public event System.Action<NodeData> OnNodeDataSelected = null;

    private NodeData buttonNodeData = null;

    [SerializeField]
    private UnityEngine.UI.Text buttonText = null;

    /// <summary>
    /// Intializes the button being pressed.
    /// </summary>
    /// <param name="data">Data.</param>
    public void InitializeButton(NodeData data) {
        this.buttonNodeData = data;

        this.buttonText.text = data.title;
    }

    /// <summary>
    /// All a node button is responsible for is telling it's pointmenu that it was selected.
    /// </summary>
    public void HandleButtonSelected() {
        if (this.OnNodeDataSelected != null) {
            this.OnNodeDataSelected(this.buttonNodeData);
        }
    }
}
