using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class InteractionMenu : MonoBehaviour {

    public InteractionButton buttonPrefab;

    public Transform buttonContainer;

    public void Init(List<NodeData> nodes) {
        foreach (NodeData node in nodes) {
            InteractionButton button = GameObject.Instantiate(this.buttonPrefab);
            button.Init(node, this);
            button.transform.SetParent(this.buttonContainer);
        }
    }

    public void Close() {
        GameObject.Destroy(this.gameObject);
    }

}
