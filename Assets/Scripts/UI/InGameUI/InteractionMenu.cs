using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class InteractionMenu : MonoBehaviour {

    public InteractionButton buttonPrefab;

    public Transform buttonContainer;

    private List<InteractionButton> buttons = new List<InteractionButton>();

    public void Init(List<NodeData> nodes) {
        this.Clear();
        foreach (NodeData node in nodes) {
            InteractionButton button = GameObject.Instantiate(this.buttonPrefab);
            button.Init(node, this);
            button.transform.SetParent(this.buttonContainer);
            this.buttons.Add(button);
        }
    }

    public void Close() {
        GameObject.Destroy(this.gameObject);
    }

    public void Clear() {
        foreach(InteractionButton button in this.buttons) {
            GameObject.Destroy(button.gameObject);
        }
    }

}
