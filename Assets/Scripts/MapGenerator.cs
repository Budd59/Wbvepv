using UnityEngine;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
	public int height;
	public int width;
	public int roomnumber;

	public Transform wallprefab;
	public Transform borderprefab;
	public Transform groundprefab;
	public Transform startprefab;
	public Transform finishprefab;
	public Transform playerprefab;

	private int BORDER = -1;
	private int WALL = 0;
	private int CORRIDOR = 1;
	private int ROOM = 2;
	private int ROOMCENTER = 3;
	private int START = 4;
	private int FINISH = 5;

	private List<int> grid = new List<int>();

	void Awake()
	{
		InitMap(height, width);
		GenerateMaze();
		GenerateRooms();
		GenerateStartAndFinish();
		ConstructMap();
		SpawnPlayer();
	}
		
	private void InitMap(int width, int height)
	{			
		for(int y=0; y < height; y++)
		{
			for(int x=0; x < width; x++)
			{
				grid.Add(WALL);
			}
		}
	}
		
	private void GenerateMaze()
	{
		// Création de la liste des murs
		List<int> walllist = new List<int>();
			
		// Tirage aléatoire de la première cellule
		int x = Random.Range(0,width);
		int y = Random.Range(0,height);
		walllist.Add(GetIndexFromXY(x,y));
		do
		{
			// randomize the walllist
			int maxindex = walllist.Count-1;
			int randomindex = Random.Range(0, maxindex);
				
			int cellindex = walllist[randomindex];
			walllist[randomindex] = walllist[maxindex];
			walllist[maxindex] = cellindex;
				
			int cellx = GetXFromIndex(cellindex);
			int celly = GetYFromIndex(cellindex);
				
			if(HasNoNeighbourCorridor(cellx, celly))
			{
				grid[cellindex] = CORRIDOR;

				if(cellx -1 >= 0 && grid[cellindex - 1] == 0)
				{
					walllist.Add(GetIndexFromXY(cellx-1, celly));
				}

				if(cellx +1 < width && grid[cellindex +1 ] == 0)
				{
					walllist.Add(GetIndexFromXY(cellx+1, celly));
				}

				if(celly -1 >= 0 && grid[cellindex - width] == 0)
				{
					walllist.Add(GetIndexFromXY(cellx, celly-1));
				}

				if(celly +1 < height && grid[cellindex + width] == 0)
				{
					walllist.Add(GetIndexFromXY(cellx, celly+1));
				}
			}						
			walllist.RemoveAt(maxindex);
		} while(walllist.Count > 0);
	}

	private void GenerateRooms ()
	{
		// Création d'une liste de centre de salle
		List<int> roomlist = new List<int>();
		for (int i = 0; i < roomnumber; i++)
		{
			int roomindex = Random.Range(0, grid.Count-1);
			roomlist.Add(roomindex);
			grid[roomindex] = ROOMCENTER;
		}

		// Création de la salle
		foreach (int roomcenter in roomlist)
		{
			int cellx = GetXFromIndex(roomcenter);
			int celly = GetYFromIndex(roomcenter);
			foreach (int neighbourindex in GetNeighbours(cellx, celly))
			{
				if (IsCellInMap(GetXFromIndex(neighbourindex),GetXFromIndex(neighbourindex)))
				{
					grid[neighbourindex] = ROOM;
				}
			}
		}
	}

	private void GenerateStartAndFinish() {
		while (true) {
			int randomindex = Random.Range(0, grid.Count-1);
			if ( grid[randomindex] == ROOMCENTER )
			{
				grid[randomindex] = START;
				break;
			}
		}
		while (true) {
			int randomindex = Random.Range(0, grid.Count-1);
			if ( grid[randomindex] == ROOMCENTER )
			{
				grid[randomindex] = FINISH;
				break;
			}
		}
	}

	private void ConstructMap ()
	{
		for (int cellindex = 0; cellindex < grid.Count ; cellindex ++)
		{
			int cell = grid[cellindex];
			if(cell == WALL)
			{
				Instantiate(wallprefab, new Vector3(GetXFromIndex(cellindex)+1, GetYFromIndex(cellindex)+1, -1), Quaternion.identity);
			}
			else if (cell == CORRIDOR)
			{
				Instantiate(groundprefab, new Vector3(GetXFromIndex(cellindex)+1, GetYFromIndex(cellindex)+1, 0), Quaternion.identity);
			}
			else if (cell == ROOM)
			{
				Instantiate(groundprefab, new Vector3(GetXFromIndex(cellindex)+1, GetYFromIndex(cellindex)+1, 0), Quaternion.identity);
			}
			else if (cell == ROOMCENTER)
			{
				Instantiate(groundprefab, new Vector3(GetXFromIndex(cellindex)+1, GetYFromIndex(cellindex)+1, 0), Quaternion.identity);
			}
			else if (cell == START)
			{
				Instantiate(startprefab, new Vector3(GetXFromIndex(cellindex)+1, GetYFromIndex(cellindex)+1, 0), Quaternion.identity);
			}
			else if (cell == FINISH)
			{
				Instantiate(finishprefab, new Vector3(GetXFromIndex(cellindex)+1, GetYFromIndex(cellindex)+1, 0), Quaternion.identity);
			}
		}
		for (int x=0; x<=width+1; x++)
		{
			Instantiate(borderprefab, new Vector3(x, 0, -1), Quaternion.identity);
			Instantiate(borderprefab, new Vector3(x, height+1, -1), Quaternion.identity);
		}
		for (int y=1; y<=height+1; y++)
		{
			Instantiate(borderprefab, new Vector3(0, y, 0), Quaternion.identity);
			Instantiate(borderprefab, new Vector3(width+1, y, -1), Quaternion.identity);
		}
	}

	private void SpawnPlayer()
	{
		for (int cellindex = 0; cellindex < grid.Count; cellindex ++)
		{
			if ( grid[cellindex] == START )
			{
				Instantiate(playerprefab, new Vector3(GetXFromIndex(cellindex)+1, GetYFromIndex(cellindex)+1, -1), Quaternion.identity);
				return;
			}
		}
	}
		
	private bool IsCellInMap(int x, int y)
	{
		return x >= 0 && x < width && y >= 0 && y < height;
	}

	private int GetXFromIndex(int index)
	{
		return index % width;
	}
	
	private int GetYFromIndex(int index)
	{
		return index / width;
	}
	
	private int GetIndexFromXY(int x, int y)
	{
		return x + y * width;
	}

	private bool HasNoNeighbourCorridor(int cellx, int celly)
	{
		int adjacentcorridor = 0;
		foreach (int neighbourindex in GetNeighbours(cellx,celly))
		{
			int neighbourcell = grid[neighbourindex];
			int cellindex = GetIndexFromXY(cellx, celly);
			if (IsDiagonalNeighbour(cellindex ,neighbourindex))
			{
				int neighbourcellx = GetXFromIndex(neighbourindex);
				int neighbourcelly = GetYFromIndex(neighbourindex);
				int adjacentcell1 = grid[GetIndexFromXY(cellx, neighbourcelly)];
				int adjacentcell2 = grid[GetIndexFromXY(neighbourcellx, celly)];

				if ( adjacentcell1 == WALL && adjacentcell2 == WALL && neighbourcell == CORRIDOR )
				{
					return false;
				}
			}
			else if (neighbourcell == CORRIDOR)
			{
				adjacentcorridor++;
			}	
		}
		return adjacentcorridor <= 1;
	}

	private List<int> GetNeighbours (int cellx, int celly)
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

	private bool IsDiagonalNeighbour(int cellindex, int neighbourindex)
	{
		return cellindex - 1 - width == neighbourindex
						|| cellindex + 1 - width == neighbourindex
						|| cellindex - 1 + width == neighbourindex
						|| cellindex + 1 + width == neighbourindex;
	}
}

