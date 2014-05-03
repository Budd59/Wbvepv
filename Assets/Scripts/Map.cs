using UnityEngine;
using System.Collections.Generic;
namespace Generator
{
	public class Map : MonoBehaviour
	{
		public int height;
		public int width;
		public int maxNumberOfRooms;
		public int lissage;
		
		public Transform wallprefab;
		public Transform borderprefab;
		public Transform groundprefab;
		public Transform startprefab;
		public Transform finishprefab;
		public Transform playerprefab;

		public List<CellType> grid = new List<CellType>();

		public MapAnalyzer mapAnalyzer;

		public void Awake ()
		{
			mapAnalyzer = new MapAnalyzer (this);
			MapGenerator.Generate (this);
			ConstructMap();
			SpawnPlayer();
		}

		private void ConstructMap ()
		{
			for (int cellindex = 0; cellindex < grid.Count ; cellindex ++)
			{
				CellType cell = grid[cellindex];
				if(cell == CellType.WALL)
				{
					Transform newwall = Instantiate(wallprefab, new Vector3(mapAnalyzer.GetXFromIndex(cellindex)+1, mapAnalyzer.GetYFromIndex(cellindex)+1, -1), Quaternion.identity) as Transform;
					newwall.transform.parent = gameObject.transform;
				}
				else if (cell == CellType.CORRIDOR)
				{
					Transform newcorridor = Instantiate(groundprefab, new Vector3(mapAnalyzer.GetXFromIndex(cellindex)+1, mapAnalyzer.GetYFromIndex(cellindex)+1, 0), Quaternion.identity) as Transform;
					newcorridor.transform.parent = gameObject.transform;
				}
				else if (cell == CellType.ROOM)
				{
					Transform newroom = Instantiate(groundprefab, new Vector3(mapAnalyzer.GetXFromIndex(cellindex)+1, mapAnalyzer.GetYFromIndex(cellindex)+1, 0), Quaternion.identity) as Transform;
					newroom.transform.parent = gameObject.transform;
				}
				else if (cell == CellType.ROOMCENTER)
				{
					Transform newroomcenter = Instantiate(groundprefab, new Vector3(mapAnalyzer.GetXFromIndex(cellindex)+1, mapAnalyzer.GetYFromIndex(cellindex)+1, 0), Quaternion.identity) as Transform;
					newroomcenter.transform.parent = gameObject.transform;
				}
				else if (cell == CellType.START)
				{
					Transform newstart = Instantiate(startprefab, new Vector3(mapAnalyzer.GetXFromIndex(cellindex)+1, mapAnalyzer.GetYFromIndex(cellindex)+1, 0), Quaternion.identity) as Transform;
					newstart.transform.parent = gameObject.transform;
				}
				else if (cell == CellType.FINISH)
				{
					Transform newfinish = Instantiate(finishprefab, new Vector3(mapAnalyzer.GetXFromIndex(cellindex)+1, mapAnalyzer.GetYFromIndex(cellindex)+1, 0), Quaternion.identity) as Transform;
					newfinish.transform.parent = gameObject.transform;
				}
			}
			for (int x=0; x<=width+1; x++)
			{
				Transform newborder1 = Instantiate(borderprefab, new Vector3(x, 0, -1), Quaternion.identity) as Transform;
				newborder1.transform.parent = gameObject.transform;
				Transform newborder2 = Instantiate(borderprefab, new Vector3(x, height+1, -1), Quaternion.identity) as Transform;
				newborder2.transform.parent = gameObject.transform;
			}
			for (int y=1; y<=height+1; y++)
			{
				Transform newborder3 = Instantiate(borderprefab, new Vector3(0, y, 0), Quaternion.identity) as Transform;
				newborder3.transform.parent = gameObject.transform;
				Transform newborder4 = Instantiate(borderprefab, new Vector3(width+1, y, -1), Quaternion.identity) as Transform;
				newborder4.transform.parent = gameObject.transform;
			}
		}
		
		private void SpawnPlayer()
		{
			for (int cellindex = 0; cellindex < grid.Count; cellindex ++)
			{
				if ( grid[cellindex] == CellType.START )
				{
					Instantiate(playerprefab, new Vector3(mapAnalyzer.GetXFromIndex(cellindex)+1, mapAnalyzer.GetYFromIndex(cellindex)+1, -1), Quaternion.identity);
					return;
				}
			}
		}
	}
}


