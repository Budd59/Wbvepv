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
	private List<int> tempGrid = new List<int>();

	void Awake()
	{
		InitMap(height, width);
		GenerateMaze();
		GenerateStartAndFinish();
		GenerateRooms();
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
				tempGrid.Add(x+y*width);
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

	private void GenerateStartAndFinish()
	{
		int startindex = Random.Range(1, width - 1);
		grid[startindex] = START;
		GenerateRoom (startindex);

		int finishindex = Random.Range(1 + (height - 1) * width, width * height - 2);
		grid[finishindex] = FINISH;
		GenerateRoom (finishindex);
	}

	private void GenerateRoom(int index)
	{
		foreach (int subindex in GetNeighbours (GetXFromIndex(index), GetYFromIndex(index)))
		{
			foreach (int subsubindex in GetNeighbours (GetXFromIndex(subindex), GetYFromIndex(subindex)))
			{
				foreach (int subsubsubindex in GetNeighbours (GetXFromIndex(subsubindex), GetYFromIndex(subsubindex)))
				{
					tempGrid[subsubsubindex] = -1;
				}
				tempGrid[subsubindex] = -1;
			}
			grid[subindex]=ROOM;
			tempGrid[subindex] = -1;
		}
		tempGrid[index] = -1;
	}

	private void GenerateRooms ()
	{
		// Création d'une liste de centre de salle
		//List<int> roomlist = new List<int>();
		//for (int i = 0; i < roomnumber; i++)
		//{
		//	int roomindex = Random.Range(0, grid.Count-1);
		//	roomlist.Add(roomindex);
		//	grid[roomindex] = ROOMCENTER;
		//}

		// Création de la salle
		//foreach (int roomcenter in roomlist)
		//{
		//	int cellx = GetXFromIndex(roomcenter);
		//	int celly = GetYFromIndex(roomcenter);
		//	foreach (int neighbourindex in GetNeighbours(cellx, celly))
		//	{
		//		if (IsCellInMap(GetXFromIndex(neighbourindex),GetXFromIndex(neighbourindex)))
		//		{
		//			grid[neighbourindex] = ROOM;
		//		}
		//	}
		//}

		while (tempGrid.Exists(i => i > -1))
		{
			List<int> list = tempGrid.FindAll(i => i > -1);
			int roomindex = list[Random.Range(0, list.Count-1)];
			grid[roomindex] = ROOMCENTER;
			GenerateRoom(roomindex);
		}
	}

	private void ConstructMap ()
	{
		for (int cellindex = 0; cellindex < grid.Count ; cellindex ++)
		{
			int cell = grid[cellindex];
			if(cell == WALL)
			{
				Transform newwall = Instantiate(wallprefab, new Vector3(GetXFromIndex(cellindex)+1, GetYFromIndex(cellindex)+1, -1), Quaternion.identity) as Transform;
				newwall.transform.parent = gameObject.transform;
			}
			else if (cell == CORRIDOR)
			{
				Transform newcorridor = Instantiate(groundprefab, new Vector3(GetXFromIndex(cellindex)+1, GetYFromIndex(cellindex)+1, 0), Quaternion.identity) as Transform;
				newcorridor.transform.parent = gameObject.transform;
			}
			else if (cell == ROOM)
			{
				Transform newroom = Instantiate(groundprefab, new Vector3(GetXFromIndex(cellindex)+1, GetYFromIndex(cellindex)+1, 0), Quaternion.identity) as Transform;
				newroom.transform.parent = gameObject.transform;
			}
			else if (cell == ROOMCENTER)
			{
				Transform newroomcenter = Instantiate(groundprefab, new Vector3(GetXFromIndex(cellindex)+1, GetYFromIndex(cellindex)+1, 0), Quaternion.identity) as Transform;
				newroomcenter.transform.parent = gameObject.transform;
			}
			else if (cell == START)
			{
				Transform newstart = Instantiate(startprefab, new Vector3(GetXFromIndex(cellindex)+1, GetYFromIndex(cellindex)+1, 0), Quaternion.identity) as Transform;
				newstart.transform.parent = gameObject.transform;
			}
			else if (cell == FINISH)
			{
				Transform newfinish = Instantiate(finishprefab, new Vector3(GetXFromIndex(cellindex)+1, GetYFromIndex(cellindex)+1, 0), Quaternion.identity) as Transform;
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


