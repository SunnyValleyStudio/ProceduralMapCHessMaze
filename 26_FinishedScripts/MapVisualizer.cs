/*
	Made by Sunny Valle Studio
	(https://svstudio.itch.io)
*/
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SVS.ChessMaze
{
	public class MapVisualizer : MonoBehaviour
	{
		private Transform parent;
		public Color startColor, exitColor;

		public GameObject roadStraight, roadTileCorner, tileEmpty, startTile, exitTile;
		public GameObject[] environmentTiles;

		Dictionary<Vector3, GameObject> dictionaryOfObstacles = new Dictionary<Vector3, GameObject>();

		public bool animate;

		private void Awake()
		{
			parent = this.transform;
		}

		public void VisualizeMap(MapGrid grid, MapData data, bool visualizeUsingPrefabs)
		{
			if (visualizeUsingPrefabs)
			{
				VisualizeUsingPrefabs(grid, data);
			}
			else
			{
				VisualizeUsingPrimitives(grid, data);
			}
		}

		private void VisualizeUsingPrefabs(MapGrid grid, MapData data)
		{
			for (int i = 0; i < data.path.Count; i++)
			{
				var position = data.path[i];
				if (position != data.exitPosition)
				{
					grid.SetCell(position.x, position.z, CellObjectType.Road);
				}
			}
			for (int col = 0; col < grid.Width; col++)
			{
				for (int row = 0; row < grid.Length; row++)
				{
					var cell = grid.GetCell(col, row);
					var position = new Vector3(cell.X,0,cell.Z);

					var index = grid.CalculateIndexFromCoordinates(position.x, position.z);
					if(data.obstacleArray[index] && cell.IsTaken == false)
					{
						cell.ObjectType = CellObjectType.Obstacle;
					}
					Direction previousDirection = Direction.None;
					Direction nextDirection = Direction.None;
					switch (cell.ObjectType)
					{
						case CellObjectType.Empty:
							CreateIndicator(position, tileEmpty);
							break;
						case CellObjectType.Road:
							if (data.path.Count > 0)
							{
								previousDirection = GetDirectionOfPreviousCell(position, data);
								nextDirection = GetDicrectionOfNextCell(position, data);
							}
							if(previousDirection == Direction.Up && nextDirection == Direction.Right || previousDirection == Direction.Right && nextDirection == Direction.Up)
							{
								CreateIndicator(position, roadTileCorner, Quaternion.Euler(0,90,0));
							}
							else if (previousDirection == Direction.Right && nextDirection == Direction.Down || previousDirection == Direction.Down && nextDirection == Direction.Right)
							{
								CreateIndicator(position, roadTileCorner, Quaternion.Euler(0, 180, 0));
							}
							else if (previousDirection == Direction.Down && nextDirection == Direction.Left || previousDirection == Direction.Left && nextDirection == Direction.Down)
							{
								CreateIndicator(position, roadTileCorner, Quaternion.Euler(0, -90, 0));
							}
							else if (previousDirection == Direction.Left && nextDirection == Direction.Up || previousDirection == Direction.Up && nextDirection == Direction.Left)
							{
								CreateIndicator(position, roadTileCorner);
							}
							else if (previousDirection == Direction.Right && nextDirection == Direction.Left || previousDirection == Direction.Left && nextDirection == Direction.Right)
							{
								CreateIndicator(position, roadStraight, Quaternion.Euler(0, 90, 0));
							}
							else
							{
								CreateIndicator(position, roadStraight);
							}

							break;
						case CellObjectType.Obstacle:
							int randomIndex = Random.Range(0, environmentTiles.Length);
							CreateIndicator(position, environmentTiles[randomIndex]);
							break;
						case CellObjectType.Start:
							if (data.path.Count > 0)
							{
								nextDirection = GetDirectionFromVectors(data.path[0], position);
								
							}
							if (nextDirection == Direction.Right || nextDirection == Direction.Left)
							{
								CreateIndicator(position, startTile, Quaternion.Euler(0, 90, 0));
							}
							else
							{
								CreateIndicator(position, startTile);
							}

							break;
						case CellObjectType.Exit:
							if (data.path.Count > 0)
							{
								previousDirection = GetDirectionOfPreviousCell(position, data);
								switch (previousDirection)
								{
									case Direction.Right:
										CreateIndicator(position, exitTile, Quaternion.Euler(0,90,0));
										break;
									case Direction.Left:
										CreateIndicator(position, exitTile, Quaternion.Euler(0, -90, 0));
										break;
									case Direction.Down:
										CreateIndicator(position, exitTile, Quaternion.Euler(0, 180, 0));
										break;
									default:
										CreateIndicator(position, exitTile);
										break;
								}
							}
							
							break;
						default:
							break;
					}
				}
			}
		}

		private Direction GetDicrectionOfNextCell(Vector3 position, MapData data)
		{
			int index = data.path.FindIndex(a => a == position);
			var nextCellPosition = data.path[index + 1];
			return GetDirectionFromVectors(nextCellPosition, position);
		}

		private Direction GetDirectionOfPreviousCell(Vector3 position, MapData data)
		{
			var index = data.path.FindIndex(a => a == position);
			var previousCellPosition = Vector3.zero;
			if(index > 0)
			{
				previousCellPosition = data.path[index - 1];
			}
			else
			{
				previousCellPosition = data.startPosition;
			}
			return GetDirectionFromVectors(previousCellPosition, position);
		}

		private Direction GetDirectionFromVectors(Vector3 positionToGoTo, Vector3 position)
		{
			if(positionToGoTo.x > position.x)
			{
				return Direction.Right;
			}else if(positionToGoTo.x < position.x)
			{
				return Direction.Left;
			}else if(positionToGoTo.z < position.z)
			{
				return Direction.Down;
			}
			return Direction.Up;
		}

		private void CreateIndicator(Vector3 position, GameObject prefab, Quaternion rotation = new Quaternion())
		{
			var placementPosition = position + new Vector3(.5f, .5f, .5f);
			var element = Instantiate(prefab, placementPosition, rotation);
			element.transform.parent = parent;
			dictionaryOfObstacles.Add(position, element);
			if (animate)
			{
				element.AddComponent<DropTween>();
				DropTween.IncreaseDropTime();
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
			if (animate)
			{
				element.AddComponent<DropTween>();
				DropTween.IncreaseDropTime();
			}
		}

		public void ClearMap()
		{
			foreach (var obstacle in dictionaryOfObstacles.Values)
			{
				Destroy(obstacle);
			}
			dictionaryOfObstacles.Clear();

			if (animate)
				DropTween.ResetTime();
		}
	}
}

