using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
	// タイルのスケールは1
	public Transform tilePrefab;
	public Transform obstaclePrefab;
    public Vector2 mapSize;

	[Range (0, 1)]
	public float outlinePercent;

	[Range (0, 1)]
	public float obstaclePercent;

	List<Coord> allTileCoords;
	Queue<Coord> shuffledTileCoords;

	public int seed = 10;
	Coord mapCentre;

	void Start ()
	{
        seed = Random.Range(0, 150);
        GenerateMap ();
	}

	public void GenerateMap ()
	{
		allTileCoords = new List<Coord> ();
		for (int x = 0; x < mapSize.x; x++)
        {
			for (int y = 0; y < mapSize.y; y++)
            {
				allTileCoords.Add (new Coord (x, y));
			}
		}
		shuffledTileCoords = new Queue<Coord> (Utility.ShuffleArray (allTileCoords.ToArray (), seed));

		mapCentre = new Coord ((int)mapSize.x / 2, (int)mapSize.y / 2);

		string holderName = "Generated Map";
		if (transform.FindChild (holderName))
        {
			DestroyImmediate (transform.FindChild (holderName).gameObject);
		}

		Transform mapHolder = new GameObject (holderName).transform;
		mapHolder.parent = transform; // Mapオブジェクトを親にする。

		//
		// マップのZY平面タイル作成
		//
		for (int x = 0; x < mapSize.x; x++)
        {
			for (int y = 0; y < mapSize.y; y++)
            {
				// 0.5fはQuadの半分のサイズ
				Vector3 tilePosition = CoordToPosition (x, y);
				// 赤軸90度回転
				Transform newTile = Instantiate (tilePrefab, tilePosition, Quaternion.Euler (Vector3.right * 90)) as Transform;
				newTile.localScale = Vector3.one * (1 - outlinePercent);
				newTile.parent = mapHolder;
			}
		}

		//
		// 壁を個数分作成
		//
		bool[,] obstacleMap = new bool[(int)mapSize.x, (int)mapSize.y];

		int obstacleCount = (int)(mapSize.x * mapSize.y * obstaclePercent);
		int currentObstacleCount = 0;

		for (int i = 0; i < obstacleCount; i++)
        {
			// ランダムに座標を取得
			Coord randomCoord = GetRandomCoord ();
			obstacleMap [randomCoord.x, randomCoord.y] = true;
			currentObstacleCount++;
			if (randomCoord != mapCentre && MapIsFullyAccessible (obstacleMap, currentObstacleCount))
            {
				Vector3 obstaclePosition = CoordToPosition (randomCoord.x, randomCoord.y);
				Transform newObstacle = Instantiate (obstaclePrefab, obstaclePosition + Vector3.up * .5f, Quaternion.identity) as Transform;
				newObstacle.parent = mapHolder;
			}
            else
            {
				obstacleMap [randomCoord.x, randomCoord.y] = false;
				currentObstacleCount--;
			}
		}
	}

	// Flood fill アルゴリズムで、移動できる箇所を調べて、道が塞がれ到達できない場合は、falseを返す。
	bool MapIsFullyAccessible (bool[,] obstacleMap, int currentObstacleCount)
	{
		bool[,] mapFlags = new bool[obstacleMap.GetLength (0), obstacleMap.GetLength (1)];
		Queue<Coord> queue = new Queue<Coord> ();
		// 起点として、センター座標は到達できる状態にしておく
		queue.Enqueue (mapCentre);
		mapFlags [mapCentre.x, mapCentre.y] = true;

		int accessibleTileCount = 1;

		while (queue.Count > 0)
        {
			Coord tile = queue.Dequeue ();

			// check neighbor
			for (int x = -1; x <= 1; x++)
            {
				for (int y = -1; y <= 1; y++)
                {
					int neighbourX = tile.x + x;
					int neighbourY = tile.y + y;
					if (x == 0 || y == 0)
                    {
						// 隣接がマイナスや、マップサイズ以上になっていない場合
						// xは調べるところ、+は中心、
						// xxx
						// x+-
						// x--
						if (neighbourX >= 0 && neighbourX < obstacleMap.GetLength (0) && neighbourY >= 0 && neighbourY < obstacleMap.GetLength (1))
                        {
							// mapFlagは移動できるかのフラグ、壁であるか
							// その隣接XYが移動出来て、障害物じゃない場合、次の起点として使えるようにQueueに追加。
							if (!mapFlags [neighbourX, neighbourY] && !obstacleMap [neighbourX, neighbourY])
                            {
								mapFlags [neighbourX, neighbourY] = true;
								queue.Enqueue (new Coord (neighbourX, neighbourY));
								accessibleTileCount++;
							}
						}
					}
				}
			}
		}

		int targetAccessibleTielCount = (int)(mapSize.x * mapSize.y - currentObstacleCount);
		return targetAccessibleTielCount == accessibleTileCount;
	}

	// 原点を中心にする
	Vector3 CoordToPosition (int x, int y)
	{
		return new Vector3 (-mapSize.x / 2 + 0.5f + x, 0, -mapSize.y / 2 + 0.5f + y);
	}

	public Coord GetRandomCoord ()
	{
		// キューから取り出す。=Dequeue
		Coord randomCoord = shuffledTileCoords.Dequeue ();
		// キューにれる。=Enqueue
		shuffledTileCoords.Enqueue (randomCoord);
		return randomCoord;
	}

	public struct Coord
	{
		public int x;
		public int y;

		public Coord (int _x, int _y)
		{
			x = _x;
			y = _y;
		}

		public static bool operator == (Coord c1, Coord c2)
		{
			return c1.x == c2.x && c1.y == c2.y;
		}

		public static bool operator != (Coord c1, Coord c2)
		{
			// 早速上記の==演算子を使う
			return !(c1 == c2);
		}
	}

   
}
