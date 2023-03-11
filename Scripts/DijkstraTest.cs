using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DijkstraTest : MonoBehaviour
{
    public float speed = 4.0f;

    private List<Node> nodes;
    private List<Node> path;

    public Node startNode;
    public Node endNode;


    void Start()
    {
        nodes = new List<Node>(GameObject.FindObjectsOfType<Node>());

        path = Dijkstra(startNode, endNode);
        path.Reverse();
    }

    void Update()
    {
        if (path == null || path.Count == 0)
        {
            return;
        }

        Node nextNode = path[0];
        Vector3 direction = nextNode.transform.position - transform.position;
        transform.position += direction.normalized * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, nextNode.transform.position) < 0.1f)
        {
            path.RemoveAt(0);
        }
    }

    Node GetClosestNode(Vector3 position)
    {
        Node closestNode = null;
        float closestDistance = Mathf.Infinity;

        foreach (Node node in nodes)
        {
            float distance = Vector3.Distance(position, node.transform.position);
            if (distance < closestDistance)
            {
                closestNode = node;
                closestDistance = distance;
            }
        }

        return closestNode;
    }

    List<Node> Dijkstra(Node start, Node end)
    {
        List<Node> unvisitedNodes = new List<Node>(nodes);
        Dictionary<Node, float> distance = new Dictionary<Node, float>();
        Dictionary<Node, Node> previous = new Dictionary<Node, Node>();

        foreach (Node node in unvisitedNodes)
        {
            distance[node] = Mathf.Infinity;
            previous[node] = null;
        }

        distance[start] = 0.0f;

        while (unvisitedNodes.Count > 0)
        {
            Node currentNode = null;
            float smallestDistance = Mathf.Infinity;
            foreach (Node node in unvisitedNodes)
            {
                if (distance[node] < smallestDistance)
                {
                    currentNode = node;
                    smallestDistance = distance[node];
                }
            }

            if (currentNode == end)
            {
                break;
            }

            unvisitedNodes.Remove(currentNode);

            foreach (Node neighbor in currentNode.neighbors)
            {
                //float altDistance = distance[currentNode] + Vector3.Distance(currentNode.transform.position, neighbor.transform.position);
                float altDistance = distance[currentNode] + neighbor.weight;

                if (altDistance < distance[neighbor])
                {
                    distance[neighbor] = altDistance;
                    previous[neighbor] = currentNode;
                }
            }
        }

        List<Node> path = new List<Node>();
        Node currentNode2 = end;
        while (currentNode2 != null)
        {
            path.Add(currentNode2);
            currentNode2 = previous[currentNode2];
        }

        return path;
    }
}