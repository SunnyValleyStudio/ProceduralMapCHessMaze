/*
	Made by Sunny Valle Studio
	(https://svstudio.itch.io)
*/
using System.Collections.Generic;
using UnityEngine;

namespace SVS.ChessMaze
{
    public struct MapData
	{
		public bool[] obstacleArray;
		public List<KnightPiece> knightPiecesList;
		public Vector3 startPosition;
		public Vector3 exitPosition;
		public List<Vector3> path;
	}
}

