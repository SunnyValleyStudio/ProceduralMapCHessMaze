/*
	Made by Sunny Valle Studio
	(https://svstudio.itch.io)
*/
using SVS.ChessMaze;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SVS.AI
{
	public static class Astar
	{
		public static List<Vector3> GetPath(Vector3 start, Vector3 exit, bool[] obstaclesArray, MapGrid grid)
		{
			VertexPosition startVertex = new VertexPosition(start);
			VertexPosition exitVertex = new VertexPosition(exit);

			List<Vector3> path = new List<Vector3>();

			List<VertexPosition> openedList = new List<VertexPosition>();
			HashSet<VertexPosition> closedList = new HashSet<VertexPosition>();

			startVertex.estimatedCost = ManhattanDistance(startVertex, exitVertex);

			openedList.Add(startVertex);

			VertexPosition currentVertex = null;
			while (openedList.Count > 0)
			{
				openedList.Sort();
				currentVertex = openedList[0];
				if (currentVertex.Equals(exitVertex))
				{
					while (currentVertex != startVertex)
					{
						path.Add(currentVertex.Position);
						currentVertex = currentVertex.previousVertex;
					}
					path.Reverse();
					break;
				}
				var arrayOfNeighbours = FindNeighboursFor(currentVertex, grid, obstaclesArray);
				foreach (var neighbour in arrayOfNeighbours)
				{
					if(neighbour ==null || closedList.Contains(neighbour))
					{
						continue;
					}
					if (neighbour.IsTaken == false)
					{
						var totalCost = currentVertex.totalCost + 1;
						var neighbourEstimatedCost = ManhattanDistance(neighbour, exitVertex);
						neighbour.totalCost = totalCost;
						neighbour.previousVertex = currentVertex;
						neighbour.estimatedCost = totalCost + neighbourEstimatedCost;
						if (openedList.Contains(neighbour) == false)
						{
							openedList.Add(neighbour);
						}
					}
				}
				closedList.Add(currentVertex);
				openedList.Remove(currentVertex);
			}

			return path;
		}

		private static VertexPosition[] FindNeighboursFor(VertexPosition currentVertex, MapGrid grid, bool[] obstaclesArray)
		{
			VertexPosition[] arrayOfNeighbours = new VertexPosition[4];

			int arrayIndex = 0;
			foreach (var possibleNeihgbour in VertexPosition.possibleNeighbours)
			{
				Vector3 position = new Vector3(currentVertex.X + possibleNeihgbour.x, 0, currentVertex.Z + possibleNeihgbour.y);
				if (grid.IsCellValid(position.x, position.z)){
					int index = grid.CalculateIndexFromCoordinates(position.x, position.z);
					arrayOfNeighbours[arrayIndex] = new VertexPosition(position, obstaclesArray[index]);
					arrayIndex++;
				}
			}
			return arrayOfNeighbours;
		}

		private static float ManhattanDistance(VertexPosition startVertex, VertexPosition exitVertex)
		{
			return Mathf.Abs(startVertex.X - exitVertex.X) + Mathf.Abs(startVertex.Z - exitVertex.Z);
		}
	}
}

