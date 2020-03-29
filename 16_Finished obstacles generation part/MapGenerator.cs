/*
	Made by Sunny Valle Studio
	(https://svstudio.itch.io)
*/
using UnityEngine;

namespace SVS.ChessMaze
{
	public class MapGenerator : MonoBehaviour
	{
		public GridVisualizer gridVisualizer;
		public MapVisualizer mapVisualizer;

		public Direction startEdge, exitEdge;
		public bool randomPlacement;
		[Range(1,10)]
		public int numberOfPieces;

		private Vector3 startPosition, exitPosition;

		[Range(3,20)]
		public int width, length = 11;
		private MapGrid grid;
		private void Start()
		{

			gridVisualizer.VisualizeGrid(width, length);
			GenerateNewMap();
		}

		public void GenerateNewMap()
		{
			mapVisualizer.ClearMap();

			grid = new MapGrid(width, length);

			MapHelper.RandomlyChooseAndSetStartAndExit(grid, ref startPosition, ref exitPosition, randomPlacement, startEdge, exitEdge);
			
			CandidateMap map = new CandidateMap(grid, numberOfPieces);
			map.CreateMap(startPosition, exitPosition);
			mapVisualizer.VisualizeMap(grid, map.ReturnMapData(), false);
		}
	}
}

