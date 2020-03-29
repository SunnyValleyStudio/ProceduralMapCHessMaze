/*
	Made by Sunny Valle Studio
	(https://svstudio.itch.io)
*/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SVS.ChessMaze
{
	public class MapVisualizer : MonoBehaviour
	{
		private Transform parent;
		public Color startColor, exitColor;

		Dictionary<Vector3, GameObject> dictionaryOfObstacles = new Dictionary<Vector3, GameObject>();

		private void Awake()
		{
			parent = this.transform;
		}

		public void VisualizeMap(MapGrid grid, MapData data, bool visualizeUsingPrefabs)
		{
			if (visualizeUsingPrefabs)
			{

			}
			else
			{
				VisualizeUsingPrimitives(grid, data);
			}
		}

		private void VisualizeUsingPrimitives(MapGrid grid, MapData data)
		{
			PlaceStartAndExitPoints(data);
			for (int i = 0; i < data.obstacleArray.Length; i++)
			{
				if (data.obstacleArray[i])
				{
					var positionOnGrid = grid.CalculateCoordinatesFromIndex(i);
					if(positionOnGrid == data.startPosition || positionOnGrid == data.exitPosition)
					{
						continue;
					}
					grid.SetCell(positionOnGrid.x, positionOnGrid.z, CellObjectType.Obstacle);
					if (PlaceKnightObstacle(data, positionOnGrid))
					{
						continue;
					}
					if (dictionaryOfObstacles.ContainsKey(positionOnGrid) == false)
					{
						CreateIndicator(positionOnGrid, Color.white, PrimitiveType.Cube);
					}
					
				}
			}
		}

		private bool PlaceKnightObstacle(MapData data, Vector3 positionOnGrid)
		{
			foreach (var knight in data.knightPiecesList)
			{
				if (knight.Position == positionOnGrid)
				{
					CreateIndicator(positionOnGrid, Color.red, PrimitiveType.Cube);
					return true;
				}
			}
			return false;
		}

		private void PlaceStartAndExitPoints(MapData data)
		{
			CreateIndicator(data.startPosition, startColor, PrimitiveType.Sphere);
			CreateIndicator(data.exitPosition, exitColor, PrimitiveType.Sphere);
		}

		private void CreateIndicator(Vector3 position, Color color, PrimitiveType sphere)
		{
			var element = GameObject.CreatePrimitive(sphere);
			dictionaryOfObstacles.Add(position, element);
			element.transform.position = position + new Vector3(.5f,.5f,.5f);
			element.transform.parent = parent;
			var renderer = element.GetComponent<Renderer>();
			renderer.material.SetColor("_Color", color);
		}

		public void ClearMap()
		{
			foreach (var obstacle in dictionaryOfObstacles.Values)
			{
				Destroy(obstacle);
			}
			dictionaryOfObstacles.Clear();

		}
	}
}

