using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class pathFinding_ASTAR : MonoBehaviour
{

	public Transform seeker, target;//those our main characters the start and the end positions

	grid Grid;// this is our plane which contains everything

	void Awake()
	{
		Grid = GetComponent<grid>();//finding the grid
	}

	void Update()
	{
		if (Input.GetButtonDown("Jump"))
		{
			Grid.CreateGrid(); //creating the grid this is inside the update function so that it can compute the location of obstacles if they are dynamic or static
			FindPath(seeker.position, target.position);//this is A* algorithm it takes the start and end positions
		}
	}



	void FindPath(Vector3 startPos, Vector3 targetPos)
	{
		Stopwatch sw = new Stopwatch();//just a stopwatch to keep track of time taken in milli secs
		sw.Start();
		
	// first thing we need to convert the locations to nodes to be able to find the neighbours of it
	 // i used a function called worldpoint_to_node to do this job calculations are explained
		Node startNode = Grid.Worldpoint_to_node(startPos);
		Node targetNode = Grid.Worldpoint_to_node(targetPos);


		//then we need 2 sets one for the nodes that hasn't been visited yet , the other is for the nodes that is already visited
		List<Node> openSet = new List<Node>();
		HashSet<Node> closedSet = new HashSet<Node>();
		// we add the start node to the open set as it's visited by default and has cost value of 0
		openSet.Add(startNode);

		//the next while loop is finding the node that has the lowest Fcost , it's complexity is too high : O(N^2)
		//Considering the worst-case scenario where each node has multiple neighbors and all nodes are unvisited,
		//the overall time complexity can be approximated as O(N^2), where N is the number of nodes in the graph. WE CAN OPTIMIZING IT BY MAKING IT AS A MIN HEAP
		//since the min heap garuantees that the min Fcost is at the top of the heap


		// while loop iterating until the openset is empty 
		while (openSet.Count > 0)
		{
			Node node = openSet[0];
//by other means created a node that is having the same value of the start node


// since i=1 , and in the first iteration openset only contains the start node so it's count is 1 , this for loop
// is not entered at first itteration 

			for (int i = 1; i < openSet.Count; i++)
			{
				if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
				{
					if (openSet[i].hCost < node.hCost)
						node = openSet[i];
				}
			}
			//so we remove this node from the unvisited and add it to the visited
			openSet.Remove(node);
			closedSet.Add(node);
			//the node is not the target node at this case

			if (node == targetNode)
			{
				RetracePath(startNode, targetNode);// if we found the node then draw the path then end the program
				sw.Stop();
				print("it took " + sw.ElapsedMilliseconds + " ms to find the path");
				return;
			}

			//starting to find neighbours of this node , a method called getneighbours is used 
			//for example if we have a node at position(13,0) so it has neighbours (12,0),(14,0),(12,1),(13,1),(14,1) as it can move diagonally and horizontal and vertical
			//explained in the grid class 

			foreach (Node neighbour in Grid.GetNeighbours(node))
			{
				if (!neighbour.walkable || closedSet.Contains(neighbour))//check if the neighbour is not walkable "obstacle" or we already visited it then neglect it
																		// for example the node (12,0) is walkable , not inside the visited set so we are using this node now...
				{
					continue;
				}

				int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);// the new cost to the neighbour is equal to the cost of the previous node +the 
																				   //distence from the node to the neighbour
																				   //since the node is the start node then gcost=0, distance is either 10 or 14 if diagonally it's 14
																				   //if horizontal or vertical it's 10 , so the new neighbour cost=10

				if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))//10 is not less than 10, it's still not explored yet so it's true we enter the if 
				{
					neighbour.gCost = newCostToNeighbour;//now the node (12,0) has a gcost of 10  
					neighbour.hCost = GetDistance(neighbour, targetNode);//h cost of distance between this node and the target node which is calculated to be 306 considering 
																		 //each cell is 1*1 meter 
					neighbour.parent = node;							//then keep refrence to the cell which the neighbour came from by making the parent of the neighbour = node

					if (!openSet.Contains(neighbour))//finally add the neighbour to the to be explored list  
						openSet.Add(neighbour);
				}
			}
		}
	}

	void RetracePath(Node startNode, Node endNode)
	{
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode)//in here we have found the target node , so we itterate backwards to the start node to draw the path
		{
			path.Add(currentNode);
			currentNode = currentNode.parent;// we go back to the parent of every node that's why we kept track of it , just adding the nodes to a list 
		}
		path.Reverse();// then we reverse this list just for the sake of logic 

		Grid.path = path; // finally we have a path just need to assign it to the public variable inside the grid class to be able to draw the path from it

	}

	int GetDistance(Node nodeA, Node nodeB)// the distance between the node (12,0) and the target node (3,27) is equal to : 
	{
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);// 12-3 =9
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);// 0-27 =27 

		if (dstX > dstY)  //since y>x then
			return 14 * dstY + 10 * (dstX - dstY);
		return 14 * dstX + 10 * (dstY - dstX); //return 14*9+10(27-9) =306 
	}
}