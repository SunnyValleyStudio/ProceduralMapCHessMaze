/*
	Made by Sunny Valle Studio
	(https://svstudio.itch.io)
*/
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SVS.ChessMaze
{
	public class CandidateMap
	{
		private MapGrid grid;
		private int numberOfPieces = 0;
		private bool[] obstaclesArray = null;
		private Vector3 startPoint, exitPoint;
		private List<KnightPiece> knightPiecesList;


		public MapGrid Grid { get => grid; }
		public bool[] ObstaclesArray { get => obstaclesArray;}

		public CandidateMap(MapGrid grid, int numberOfPieces)
		{
			this.numberOfPieces = numberOfPieces;
			
			this.grid = grid;
		}

		public void CreateMap(Vector3 startPosition, Vector3 exitPosition, bool autoRepair = false)
		{
			this.startPoint = startPosition;
			this.exitPoint = exitPosition;
			obstaclesArray = new bool[grid.Width * grid.Length];
			this.knightPiecesList = new List<KnightPiece>();
			RandomlyPlaceKnightPieces(this.numberOfPieces);

			PlaceObstacles();
		}

		private bool CheckIfPositionCanBeObstacle(Vector3 position)
		{
			if(position == startPoint || position == exitPoint)
			{
				return false;
			}
			int index = grid.CalculateIndexFromCoordinates(position.x, position.z);

			return obstaclesArray[index] == false;
		}

		private void RandomlyPlaceKnightPieces(int numbeOfPieces)
		{
			var count = numberOfPieces;
			var knighPlacementTryLimit = 100;
			while (count > 0 && knighPlacementTryLimit>0)
			{
				var randomIndex = Random.Range(0, obstaclesArray.Length);
				if (obstaclesArray[randomIndex] == false)
				{
					var coordinates = grid.CalculateCoordinatesFromIndex(randomIndex);
					if(coordinates==startPoint || coordinates == exitPoint)
					{
						continue;
					}
					obstaclesArray[randomIndex] = true;
					knightPiecesList.Add(new KnightPiece(coordinates));
					count--;

				}
				knighPlacementTryLimit--;
			}
		}

		private void PlaceObstaclesForThisKnight(KnightPiece knight)
		{
			foreach (var position in KnightPiece.listOfPossibleMoves)
			{
				var newPosition = knight.Position + position;
				if(grid.IsCellValid(newPosition.x,newPosition.z) && CheckIfPositionCanBeObstacle(newPosition))
				{
					obstaclesArray[grid.CalculateIndexFromCoordinates(newPosition.x, newPosition.z)] = true;
				}
			}
		}

		private void PlaceObstacles()
		{
			foreach (var knight in knightPiecesList)
			{
				PlaceObstaclesForThisKnight(knight);
			}
		}

		public MapData ReturnMapData()
		{
			return new MapData
			{
				obstacleArray = this.obstaclesArray,
				knightPiecesList = knightPiecesList,
				startPosition = startPoint,
				exitPosition = exitPoint
			};
		}
	}
}

