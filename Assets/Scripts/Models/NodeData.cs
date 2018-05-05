using System.Collections.Generic;

public class NodeData {

    public string id;

    public string title;

    public List<string> flagsRequired;

    public NodeData parent;

    public string parentNodeName;

    public List<NodeData> children = new List<NodeData>();

    public override string ToString() {
        return string.Format("{0} {1} {2} {3}", this.id, this.title, this.parent != null ? this.parent.id : "", this.children.Count);
    }

}