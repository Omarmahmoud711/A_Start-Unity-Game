using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class grid : MonoBehaviour
{
	public bool onlydisplayPath;
	public Transform plane;
	public LayerMask unwalkableMask;
	private Vector2 gridWorldSize;
	private float nodeRadius =0.5f;
	Node[,] Grid;
	public List<Node> path;

	float nodeDiameter;
	int gridSizeX, gridSizeY;

	void Awake()
	{
		float planeWidth = plane.localScale.x*10;
		float planeLength = plane.localScale.z*10;

		// Set the gridWorldSize based on the plane's dimensions
		gridWorldSize = new Vector2(planeWidth, planeLength);

		nodeDiameter = nodeRadius * 2;

		gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
		CreateGrid();
	}

	public int MaxSize() {
		return gridSizeX * gridSizeY;
	}


    public void CreateGrid()
	{
		Grid = new Node[gridSizeX, gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

		for (int x = 0; x < gridSizeX; x++)
		{
			for (int y = 0; y < gridSizeY; y++)
			{
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
				Grid[x, y] = new Node(walkable, worldPoint, x, y);
			}
		}
	}
	//this is the part where we compute the neighbours so there is a function that takes a node and return list of the neighbours for this node
	public List<Node> GetNeighbours(Node node)
	{
		List<Node> neighbours = new List<Node>();//initiallization 
		//since the max number of neighbours for a node is 3*3 =9 including the node itself so we iterate by 3*3
		for (int x = -1; x <= 1; x++)
		{
			for (int y = -1; y <= 1; y++)
			{
				if (x == 0 && y == 0)//in this case we are on the node itself so we neglect it and don't take it as a neighbour
					continue;

				int checkX = node.gridX + x; //created a variable called checkX to make sure this neighbour we can take it or not ,for example in the first iteration 13+(-1)=12
				int checkY = node.gridY + y;//(0+-1)=-1

				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)//12 is greater than 0,also less than 30 , but y is less than 0 so we don't
																						   //add it as a neighbour, in the second itteration x=-1, y=0 so checkx=12,checkY=0
																						   //so we take it
																						   //and so on......
				{
					neighbours.Add(Grid[checkX, checkY]);// add the grid node to be a neighbour
				}
			}
		}

		return neighbours;//finally returns all the neighbours for that node
	}


	public Node Worldpoint_to_node(Vector3 worldPosition)//this is the function that changes worldpoint to a node
	{

//let's assume that the start object is at location (-1.4,0,-15) that means it's on the lower edge since our plane is 30*30 and it's center is 0,0
//then percentX=(the object's X position + the plane Width/2)/the plane width
//then percentage x is equal to (-1.4 + 30/2)/30 =(-1.4+15)/30 =0.45
//then we clamp this value to 0,1 of the value is below 0 we consider it a zero , if it's above 1 , then we consider it a one , if it's between them nothing happens
//so the location of the object is at 45 percent of the width of the plane 
		float percentX =Mathf.Clamp01 ((worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x);
		float percentY =Mathf.Clamp01( (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y);

//then we multiply this percent by gridsizeX-1 to prevent index being outbound of the array 
		int x = Mathf.RoundToInt((gridSizeX-1 ) * percentX);
		int y = Mathf.RoundToInt((gridSizeY-1 ) * percentY);
	// then we return a Node called Grid that has X ,Y
		return Grid[x, y];
	}



	void OnDrawGizmos()// this function just to show the grid on the plane for ease
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
		if (onlydisplayPath)
		{
			if (path != null)
			{
				foreach (Node n in path)
				{
					Gizmos.color = Color.green;//color it green
					Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .3f));

				}
			}
		}
		else
		{
			if (Grid != null)
			{
				foreach (Node n in Grid)
				{

					Gizmos.color = (n.walkable) ? Color.black : Color.red;//if it's wackable then black , else red
					if (path != null)// if path exists
						if (path.Contains(n))//path contains the current node
							Gizmos.color = Color.green;//color it green
					Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .3f));

				}
			}
		}
	}
}