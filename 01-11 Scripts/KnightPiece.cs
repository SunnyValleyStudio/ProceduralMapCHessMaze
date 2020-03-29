/*
	Made by Sunny Valle Studio
	(https://svstudio.itch.io)
*/
using System.Collections.Generic;
using UnityEngine;

namespace SVS.ChessMaze
{
	public class KnightPiece
	{
		public static List<Vector3> listOfPossibleMoves = new List<Vector3>
		{
			new Vector3(-1,0, 2),
			new Vector3( 1,0, 2),
			new Vector3(-1,0,-2),
			new Vector3( 1,0,-2),
			new Vector3(-2,0,-1),
			new Vector3(-2,0, 1),
			new Vector3( 2,0,-1),
			new Vector3( 2,0, 1)
		};

		private Vector3 position;

		public Vector3 Position { get => position; set => position = value; }

		public KnightPiece(Vector3 position)
		{
			this.Position = position;
		}

		
	}
}

