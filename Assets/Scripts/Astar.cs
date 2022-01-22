using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
public class Astar : MonoBehaviour
{
    public List<Node> openList = new List<Node>();
    public List<Node> closedList = new List<Node>();

    public bool AStarFunc(Vector2Int startPos, Vector2Int endPos, Tilemap map)
    {
        print("Called A star function");

        openList.Clear(); closedList.Clear();
        //print("openList has " + openList.Count);
        Node currentNode = new Node(0, 0, "", "", startPos);

        Node startNode = new Node(0, calcDist(startPos, endPos, "H"), createTileName(startPos, map), "", startPos);
        openList.Add(startNode);
        bool success = false;
        //print("Created startNode");

        while (openList.Count != 0)
        {
            //print("start of while loop");
            
            /*print("openList has " + openList.Count);
            foreach (var item in openList)
            {
                print("openlist has " + item.nodeName);
            }
            print("setting current Node");*/
            currentNode = returnLeastF(openList);
            
            if (currentNode != null && currentNode.pos == endPos)
            {
                print("Found endPos");
                success = true;
                return success;
            }

            openList.Remove(currentNode);

            closedList.Add(currentNode);

            for (int i = 0; i < 4; i++)
            {
                //print("checking adj node number " + i);
                Vector2Int curPos = new Vector2Int(0, 0);
                switch (i)
                {
                    case 0: curPos = new Vector2Int(currentNode.pos.x - 1, currentNode.pos.y); break;
                    case 1: curPos = new Vector2Int(currentNode.pos.x, currentNode.pos.y + 1); break;
                    case 2: curPos = new Vector2Int(currentNode.pos.x + 1, currentNode.pos.y); break;
                    case 3: curPos = new Vector2Int(currentNode.pos.x, currentNode.pos.y - 1); break;
                    default: break;
                }

                Node adjNode = new Node();
                adjNode.g = calcDist(startPos, curPos, "G");
                adjNode.h = calcDist(endPos, curPos, "H");
                adjNode.f = adjNode.g + adjNode.h;
                adjNode.nodeName = createTileName(curPos, map);
                adjNode.parentName = currentNode.nodeName;
                adjNode.pos = curPos;

                //print("done evaluating adj node number " + i);

                if (adjNode.nodeName.Contains("road") == false) {
                    //print("adjnode is not road or is in closedlist string ");
                } else if (checkContain(closedList, adjNode.nodeName) == true)
                {
                    //print("adj node is in closed list");
                }
                else
                {
                    //print("adjnode is road and not in closedlist");
                    if (checkContain(openList, adjNode.nodeName) == false)
                    {
                        //print("Added " + adjNode.nodeName + " to openList");
                        openList.Add(adjNode);
                        foreach (var item in openList)
                        {
                            //print("openlist has " + item.nodeName);
                        }
                    }
                    else
                    {
                        Node searchedNode = getNode(adjNode.nodeName, openList);
                        if (searchedNode.g < adjNode.g)
                        {
                            //print("Updated old node");
                            searchedNode = adjNode;
                        }
                    }
                }
            }

            if (openList.Count == 0)
            {
                print("No path found");
            }
        }
        //print("Exited while Loop");
        return success;
    }

    public int calcDist(Vector2Int start, Vector2Int sel, string which)
    {
        int deltaX = Mathf.Abs(sel.x - start.x);
        int deltaY = Mathf.Abs(sel.y - start.y);
        //print("calcDist " + which + " is " + 10*(deltaX + deltaY));
        return 10*(deltaX + deltaY);
    }

    public Node returnLeastF(List<Node> paramList)
    {
        List<Node> hereList = paramList.OrderBy(w => w.f).ToList();
        //print("leastF is " + hereList[0].nodeName);
        return hereList[0];
    }

    public string createTileName(Vector2Int pos, Tilemap map)
    {
        string textureName = map.GetTile(new Vector3Int(pos.x, pos.y, 0)).name;
        string s;
        if (textureName.Contains("road"))
        {
            s = "road" + pos.x.ToString() + pos.y.ToString();
        } else
        {
            s = textureName + pos.x.ToString() + pos.y.ToString();
        }
        //print("created tilename " + s);
        return s;
    }

    public bool checkContain(List<Node> list, string s)
    {
        foreach (var item in list)
        {
            if (item.nodeName == s)
            {
                //print("got node: " + item.nodeName);
                return true;
            }
        }
        //print("did not find node in list");
        return false;
    }
    public Node getNode(string s, List<Node> list)
    {
        foreach (var item in list)
        {
            if (item.nodeName == s)
            {
                //print("got node: " + item.nodeName);
                return item;
            }
        }
        return null;
    }
}

public class Node
{
    public int g;
    public int h;
    public int f;
    public string nodeName;
    public string parentName;
    public Vector2Int pos;

    public Node()
    {

    }

    public Node(int g, int h, string nodeName, string parentName, Vector2Int pos)
    {
        this.g = g;
        this.h = h;
        this.f = g + h;
        this.nodeName = nodeName;
        this.parentName = parentName;
        this.pos = pos;
    }
}