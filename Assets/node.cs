using UnityEngine;
using System.Collections;


//after making the heap implementation then node is also must be implements heap
public class Node :Heapo<Node>
{

	public bool walkable;//every node is walkable or not
	public Vector3 worldPosition; //each one has world position
	public int gridX;//x and y coordinates
	public int gridY;

	public int gCost;//g cost and herurestic function
	public int hCost;
	public Node parent;//parent of the node
	int heapIndex;

	public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)// the constructor of the class
	{
		walkable = _walkable;
		worldPosition = _worldPos;
		gridX = _gridX;
		gridY = _gridY;
	}

	public int fCost// the total cost 
	{
		get
		{
			return gCost + hCost;
		}
	}

	public int HeapIndex
	{
		get {
			return heapIndex;
		}
		set {
			heapIndex = value;
		}
	}

	public int CompareTo (Node node_to_be_compared){
		int compared = fCost.CompareTo(node_to_be_compared.fCost);
		if (compared == 0) {
			compared = hCost.CompareTo(node_to_be_compared.hCost);

		}
		return compared*(-1);

	}

}