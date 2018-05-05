using System.Collections.Generic;

public class NodeData {

    public string id;

    public string title;

    public List<string> flagsRequired = new List<string>();

    public NodeData parent;

    public string parentNodeName;

    public List<NodeData> children = new List<NodeData>();

    public NodeData GetChild(string childId) {
        foreach (NodeData child in this.children) {
            if (child.id == childId) {
                return child;
            }
        }
        return null;
    }

    public override string ToString() {
        return string.Format("id:{0} title:{1} parent:{2} kids:{3}", this.id, this.title, this.parent != null ? this.parent.id : "", this.children.Count);
    }

}