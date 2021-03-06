using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class DataManager : MonoBehaviour {

    public static DataManager Instance;

    public const string JSON_URL = "https://spreadsheets.google.com/feeds/list/1GGtccUoorjkgcKAJTSDRJnsR7t-rHIFHA6gdVChYTdc/od6/public/values?alt=json";
    
    public NodeData rootNode;

    public List<NodeData> allNodes;

    public List<string> allAchievements;

    public event Action onLoaded;

    private bool isLoaded = false;

    public void Awake() {
        if (DataManager.Instance != null) {
            GameObject.Destroy(this.gameObject);
        }
        DataManager.Instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);
        this.LoadData();
    }

    public void DoOnLoaded(Action callback) {
        if (this.isLoaded) {
            callback.Invoke();
        } else {
            this.onLoaded += callback;
        }
    }

    public void LoadData() {
        this.StartCoroutine(this.LoadDataCoroutine());
    }

    private IEnumerator LoadDataCoroutine() {
        char[] SPLIT_FLAGS = new char[] { ',', ' ' };
        WWW www = new WWW(JSON_URL);
        yield return www;
        JsonReader reader = new JsonReader(www.text);
        JsonData data = JsonMapper.ToObject(www.text);

        this.allNodes = new List<NodeData>();
        this.allAchievements = new List<string>();

        foreach (JsonData row in data["feed"]["entry"]) {
            NodeData node; 
            string parentNode = ((string)row["gsx$parentnode"]["$t"]).Trim();
            if (!string.IsNullOrEmpty(parentNode)) {
                string description = ((string)row["gsx$description"]["$t"]).Trim();
                if (!string.IsNullOrEmpty(description)) {
                    InteractionNodeData interaction = new InteractionNodeData();
                    interaction.description         = description;
                    interaction.flagsCreated        = new List<string>(((string)row["gsx$flagscreated"]["$t"]).Split(SPLIT_FLAGS, System.StringSplitOptions.RemoveEmptyEntries));
                    interaction.flagsRemoved        = new List<string>(((string)row["gsx$flagsremoved"]["$t"]).Split(SPLIT_FLAGS, System.StringSplitOptions.RemoveEmptyEntries));
                    interaction.animation           = ((string)row["gsx$animation"]["$t"]).Trim();
                    interaction.achievement         = ((string)row["gsx$achievement"]["$t"]).Trim();
                    if (!string.IsNullOrEmpty(interaction.achievement)) {
                        this.allAchievements.Add(interaction.achievement);
                    }
                    node = interaction;
                } else {
                    node = new NodeData();
                }
                node.parentNodeName = parentNode;
                node.id = ((string)row["gsx$node"]["$t"]).Trim();
                node.title = ((string)row["gsx$title"]["$t"]).Trim();
                node.flagsRequired = new List<string>(((string)row["gsx$flagsrequired"]["$t"]).Split(SPLIT_FLAGS, System.StringSplitOptions.RemoveEmptyEntries));
                this.allNodes.Add(node);
            }
            //Debug.LogFormat("{0} {1} {2}", node.parentNodeName, node.id, node.flagsRequired);
        }

        this.rootNode = new NodeData();
        this.rootNode.id = "root";
        this.allNodes.Add(this.rootNode);

        foreach (NodeData node in this.allNodes) {
            foreach (NodeData otherNode in this.allNodes) {
                if (node.id == otherNode.parentNodeName) {
                    node.children.Add(otherNode);
                    otherNode.parent = node;
                }
            }
        }

        this.isLoaded = true;

        if (this.onLoaded != null) {
            this.onLoaded.Invoke();
        }

        foreach (NodeData node in this.allNodes) {
            Debug.Log(node);
        }
    }

}
