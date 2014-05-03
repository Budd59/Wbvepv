using UnityEngine;
using System.Collections.Generic;
namespace Generator
{
	public class RoomGenerator
	{
		private static List<int> tempGrid;
		private static Map map;

		public static void Generate(Map map)
		{
			RoomGenerator.map = map;

			tempGrid = new List<int>();
			for (int index = 0; index < map.grid.Count; index++) {
				tempGrid.Add(index);
			}

			GenerateStartAndFinish();

			int numberRooms = 0;
			while (tempGrid.Exists(i => i > -1) && numberRooms < map.maxNumberOfRooms)
			{
				List<int> list = tempGrid.FindAll(i => i > -1);
				int roomindex = list[Random.Range(0, list.Count-1)];
				map.grid[roomindex] = CellType.ROOMCENTER;
				tempGrid[roomindex] = -1;
				GenerateRoom(roomindex);
				numberRooms++;
			}

			RoomGenerator.map = null;
		}


		private static void GenerateRoom(int index)
		{
			foreach (int subindex in map.mapAnalyzer.GetNeighbours (map.mapAnalyzer.GetXFromIndex(index), map.mapAnalyzer.GetYFromIndex(index)))
			{
				map.grid[subindex]=CellType.ROOM;
				tempGrid[subindex] = -1;
				GenerateRoomWalls(3, subindex);
			}
		}
		
		private static void GenerateRoomWalls(int level, int index)
		{
			foreach (int subindex in map.mapAnalyzer.GetNeighbours (map.mapAnalyzer.GetXFromIndex(index), map.mapAnalyzer.GetYFromIndex(index))) {
					tempGrid [subindex] = -1;
					if (level > 0) {
							GenerateRoomWalls (level - 1, subindex);
					}
			}
		}

		private static void GenerateStartAndFinish()
		{
			int startindex = Random.Range(1, map.width - 1);
			map.grid[startindex] = CellType.START;
			GenerateRoom (startindex);
			
			int finishindex = Random.Range(1 + (map.height - 1) * map.width, map.width * map.height - 2);
			map.grid[finishindex] = CellType.FINISH;
			GenerateRoom (finishindex);
		}
	}
}


