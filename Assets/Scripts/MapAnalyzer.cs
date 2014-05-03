using System.Collections.Generic;
namespace Generator
{
	public class MapAnalyzer
	{
		Map map;

		public MapAnalyzer(Map map) {
			this.map = map;
		}

		public bool isAWallOrBorder(int x, int y)
		{
			if (IsCellInMap(x,y))
			{
				if (map.grid[GetIndexFromXY(x,y)] == CellType.WALL)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			return true;
		}

		public bool IsCellInMap(int x, int y)
		{
			return x >= 0 && x < map.width && y >= 0 && y < map.height;
		}
		
		public int GetXFromIndex(int index)
		{
			return index % map.width;
		}
		
		public int GetYFromIndex(int index)
		{
			return index / map.width;
		}
		
		public int GetIndexFromXY(int x, int y)
		{
			return x + y * map.width;
		}
		
		public bool HasNoNeighbourCorridor(int cellx, int celly)
		{
			int adjacentcorridor = 0;
			foreach (int neighbourindex in GetNeighbours(cellx,celly))
			{
				CellType neighbourcell = map.grid[neighbourindex];
				int cellindex = GetIndexFromXY(cellx, celly);
				if (IsDiagonalNeighbour(cellindex ,neighbourindex))
				{
					int neighbourcellx = GetXFromIndex(neighbourindex);
					int neighbourcelly = GetYFromIndex(neighbourindex);
					CellType adjacentcell1 = map.grid[GetIndexFromXY(cellx, neighbourcelly)];
					CellType adjacentcell2 = map.grid[GetIndexFromXY(neighbourcellx, celly)];
					
					if ( adjacentcell1 == CellType.WALL && adjacentcell2 == CellType.WALL && neighbourcell == CellType.CORRIDOR )
					{
						return false;
					}
				}
				else if (neighbourcell == CellType.CORRIDOR)
				{
					adjacentcorridor++;
				}	
			}
			return adjacentcorridor <= 1;
		}
		
		public List<int> GetNeighbours (int cellx, int celly)
		{
			List<int> neighbours = new List<int>();
			for (int x=-1; x<=1; x++)
			{
				for (int y=-1; y<=1; y++)
				{
					if (x==0 && y==0) continue;
					if (IsCellInMap(cellx + x, celly + y))
					{
						neighbours.Add(GetIndexFromXY(cellx + x, celly + y));
					}
				}
			}
			return neighbours;
		}
		
		public bool IsDiagonalNeighbour(int cellindex, int neighbourindex)
		{
			return cellindex - 1 - map.width == neighbourindex
				|| cellindex + 1 - map.width == neighbourindex
					|| cellindex - 1 + map.width == neighbourindex
					|| cellindex + 1 + map.width == neighbourindex;
		}
	}
}

