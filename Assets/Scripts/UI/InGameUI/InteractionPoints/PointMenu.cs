using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Point Menu holds a root node and shows all it's child items.
/// </summary>
public class PointMenu : MonoBehaviour {

    [SerializeField]
    private PointButton pointButtonPrefab = null;

    private NodeData currentRootNode = null;

    /// <summary>
    /// The buttons.
    /// </summary>
    private List<PointButton> buttons = new List<PointButton>();

    public void SetRootNode(NodeData rootNode) {
        this.currentRootNode = rootNode;
        List<NodeData> subNodes = AdventureLog.Instance.FilterAvailableNodes(this.currentRootNode.children);
        this.LoadNodeData(subNodes);
    }

    private void LoadNodeData(List<NodeData> nodes) {
        this.ClearMenu();
        foreach (NodeData node in nodes) {
            PointButton button = GameObject.Instantiate(this.pointButtonPrefab);
            button.InitializeButton(node);
            button.transform.SetParent(this.transform);
            this.buttons.Add(button);
            button.OnNodeDataSelected += this.HandleNodeSelected;
        }
    }

    private void HandleNodeSelected(NodeData selectedNodeData) {
        if (selectedNodeData is InteractionNodeData) {
            // leaf node, do the thing
            InteractionNodeData interaction = (InteractionNodeData)selectedNodeData;
            Debug.Log(interaction.description);
            AdventureLog.Instance.UseAction(interaction);
            this.SetRootNode(this.currentRootNode);
        } else {
            this.SetRootNode(selectedNodeData);
        }
    }

    /// <summary>
    /// Clears the menu.
    /// </summary>
    public void ClearMenu() {
        foreach(PointButton button in buttons) {
            button.OnNodeDataSelected -= this.HandleNodeSelected;
            GameObject.Destroy(button.gameObject);
        }

        buttons.Clear();
    }
}
