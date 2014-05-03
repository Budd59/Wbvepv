using System.Collections.Generic;
using UnityEngine;
namespace Generator
{
	public class MapGenerator
	{
		private static Map map;

		public static void Generate(Map map)
		{
			MapGenerator.map = map;
			InitMap();
			GenerateMaze();
			RoomGenerator.Generate (map);
			Lissage(map.lissage);
			MapGenerator.map = null;
		}
			
		private static void InitMap()
		{			
			for(int y=0; y < map.height; y++)
			{
				for(int x=0; x < map.width; x++)
				{
					map.grid.Add(CellType.WALL);
				}
			}
		}
			
		private static void GenerateMaze()
		{
			// Création de la liste des murs
			List<int> walllist = new List<int>();
				
			// Tirage aléatoire de la première cellule
			int x = Random.Range(0,map.width);
			int y = Random.Range(0,map.height);
			walllist.Add(map.mapAnalyzer.GetIndexFromXY(x,y));
			do
			{
				// randomize the walllist
				int maxindex = walllist.Count-1;
				int randomindex = Random.Range(0, maxindex);

				int cellindex = walllist[randomindex];
				walllist[randomindex] = walllist[maxindex];
				walllist[maxindex] = cellindex;

				int cellx = map.mapAnalyzer.GetXFromIndex(cellindex);
				int celly = map.mapAnalyzer.GetYFromIndex(cellindex);
					
				if(map.mapAnalyzer.HasNoNeighbourCorridor(cellx, celly))
				{
					map.grid[cellindex] = CellType.CORRIDOR;

					if(cellx -1 >= 0 && map.grid[cellindex - 1] == 0)
					{
						walllist.Add(map.mapAnalyzer.GetIndexFromXY(cellx-1, celly));
					}

					if(cellx +1 < map.width && map.grid[cellindex +1 ] == 0)
					{
						walllist.Add(map.mapAnalyzer.GetIndexFromXY(cellx+1, celly));
					}

					if(celly -1 >= 0 && map.grid[cellindex - map.width] == 0)
					{
						walllist.Add(map.mapAnalyzer.GetIndexFromXY(cellx, celly-1));
					}

					if(celly +1 < map.height && map.grid[cellindex + map.width] == 0)
					{
						walllist.Add(map.mapAnalyzer.GetIndexFromXY(cellx, celly+1));
					}
				}						
				walllist.RemoveAt(maxindex);
			} while(walllist.Count > 0);
		}

		private static void Lissage(int level)
		{
			if (level <= 0) {
				return;
			}

			List<int> forLissage = new List<int>();
			for(int index=0; index < map.grid.Count; index++)
			{
				if (map.mapAnalyzer.isAWallOrBorder(map.mapAnalyzer.GetXFromIndex(index), map.mapAnalyzer.GetYFromIndex(index)))
				{
					continue;
				}
				if (findCellForLissage(index) >= 3)
				{
					forLissage.Add(index);
				}
			}

			foreach (int index in forLissage)
			{
				map.grid[index] = CellType.WALL;
			}

			Lissage(level-1);
		}

		private static int findCellForLissage(int index)
		{
			int counter = 0;
			if (map.mapAnalyzer.isAWallOrBorder (map.mapAnalyzer.GetXFromIndex (index) - 1, map.mapAnalyzer.GetYFromIndex (index)))
			{
				counter = counter + 1;
			}
			if (map.mapAnalyzer.isAWallOrBorder (map.mapAnalyzer.GetXFromIndex (index) + 1, map.mapAnalyzer.GetYFromIndex (index)))
			{
				counter = counter + 1;
			}
			if (map.mapAnalyzer.isAWallOrBorder (map.mapAnalyzer.GetXFromIndex (index), map.mapAnalyzer.GetYFromIndex (index) - 1))
			{
				counter = counter + 1;
			}
			if (map.mapAnalyzer.isAWallOrBorder (map.mapAnalyzer.GetXFromIndex (index), map.mapAnalyzer.GetYFromIndex (index) + 1))
			{
				counter = counter + 1;
			}
			return counter;
		}
	}
}


