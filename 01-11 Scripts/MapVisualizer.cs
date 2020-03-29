/*
	Made by Sunny Valle Studio
	(https://svstudio.itch.io)
*/
using System;
using UnityEngine;

namespace SVS.ChessMaze
{
	public class MapVisualizer : MonoBehaviour
	{
		private Transform parent;
		public Color startColor, exitColor;

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
			element.transform.position = position + new Vector3(.5f,.5f,.5f);
			element.transform.parent = parent;
			var renderer = element.GetComponent<Renderer>();
			renderer.material.SetColor("_Color", color);
		}
	}
}

