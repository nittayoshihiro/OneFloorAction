using UnityEngine;
using UnityEngine.Tilemaps;
public class AutoMappingV3 : MonoBehaviour
{
    [SerializeField] int m_mapSizeX = 30;
    [SerializeField] int m_mapSizeY = 20;
    [SerializeField] int m_minimumRoomSize = 4;
    [SerializeField] Tilemap m_tilemap;
    [SerializeField] Tile m_wallTile;
    [SerializeField] Tile m_roadTile;
    Vector3Int m_vector3Int = new Vector3Int(0, 0, 0);


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AutoMappingButtun()
    {
        Debug.Log("Mapping");
        m_tilemap.ClearAllTiles();
        AutoMapping(m_mapSizeX, m_mapSizeY, m_minimumRoomSize);
    }

    void AutoMapping(int mapSizeX, int mapSizeY, int minimumRoomSize)
    {
        //全部壁にする
        TileStatus[,] mapStatus = new TileStatus[mapSizeX, mapSizeY];
        for (int i = 0; i < mapSizeX - 1; i++)
        {
            for (int j = 0; j < mapSizeY - 1; j++)
            {
                mapStatus[i, j] = TileStatus.Wall;
            }
        }
        //部屋に壁の厚さ２を入れる
        int randomRoadX = Random.Range(minimumRoomSize + 4, mapSizeX - (minimumRoomSize + 5));
        int randomRoadY = Random.Range(minimumRoomSize + 4, mapSizeY - (minimumRoomSize + 5));
        int randomRoomRoadX = Random.Range(2, mapSizeY - 3);
        int randomRoomRoadY = Random.Range(randomRoadX + 3, mapSizeX - 3);
        int randomRoomRoadY2 = Random.Range(2, randomRoadX - 3);
        int randomRoomRoadLastX = Random.Range(randomRoadY + 3, mapSizeY - 3);
        int randomRoomRoadLastY = Random.Range(randomRoadX + 3, mapSizeX - 3);
        int randomRoomRoadLastY2 = Random.Range(3, randomRoadX - 3);
        //X軸で区切る
        for (int i = 0; i < mapSizeX - 1; i++)
        {
            for (int j = 0; j < mapSizeY - 1; j++)
            {
                if (i == randomRoadX)
                {
                    mapStatus[i, j] = TileStatus.Road;
                }
                //小さいほうに部屋を作る
                if (randomRoadX < mapSizeX / 2)
                {
                    if (2 <= i && i < randomRoadX - 2 && 2 <= j && j <= mapSizeY - 2 * 2)
                    {
                        mapStatus[i, j] = TileStatus.Road;
                    }
                    //道作成
                    if (randomRoomRoadX == j && randomRoadX - 2 <= i && i < randomRoadX)
                    {
                        mapStatus[i, j] = TileStatus.Road;
                    }
                    //Y軸で区切る
                    if (randomRoadY == j && randomRoadX < i)
                    {
                        mapStatus[i, j] = TileStatus.Road;
                    }
                    //Y軸上部屋
                    if (randomRoadX + 2 < i && i <= mapSizeX - 2 * 2 && 2 <= j && j < randomRoadY - 2)
                    {
                        mapStatus[i, j] = TileStatus.Road;
                    }
                    //道を作成
                    if (randomRoomRoadY == i && randomRoadY < j && j <= randomRoadY + 2)
                    {
                        mapStatus[i, j] = TileStatus.Road;
                    }
                    //Y軸下部屋
                    if (randomRoadX + 2 < i && i <= mapSizeX - 2 * 2 && randomRoadY + 2 < j && j <= mapSizeY - 2 * 2)
                    {
                        mapStatus[i, j] = TileStatus.Road;
                    }
                    //最後は２本道作成
                    if (randomRoomRoadLastX == j && randomRoadX < i && i <= randomRoadX + 2)
                    {
                        mapStatus[i, j] = TileStatus.Road;
                    }
                    if (randomRoomRoadLastY == i && randomRoadY - 2 <= j && j < randomRoadY)
                    {
                        mapStatus[i, j] = TileStatus.Road;
                    }
                }
                else
                {
                    if (randomRoadX + 2 < i && i <= mapSizeX - 2 * 2 && 2 <= j && j <= mapSizeY - 2 * 2)
                    {
                        mapStatus[i, j] = TileStatus.Road;
                    }
                    //道作成
                    if (randomRoomRoadX == j && randomRoadX < i && i <= randomRoadX + 2)
                    {
                        mapStatus[i, j] = TileStatus.Road;
                    }
                    //Y軸で区切る
                    if (j == randomRoadY && i < randomRoadX)
                    {
                        mapStatus[i, j] = TileStatus.Road;
                    }
                    //Y軸上部屋
                    if (2 <= i && i < randomRoadX - 2 && 2 <= j && j < randomRoadY - 2)
                    {
                        mapStatus[i, j] = TileStatus.Road;
                    }
                    //道を作成
                    if (randomRoomRoadY2 == i && randomRoadY < j && j <= randomRoadY + 2)
                    {
                        mapStatus[i, j] = TileStatus.Road;
                    }
                    //Y軸下部屋
                    if (2 <= i && i < randomRoadX - 2 && randomRoadY + 2 < j && j <= mapSizeY - 2 * 2)
                    {
                        mapStatus[i, j] = TileStatus.Road;
                    }
                    //最後は２本道作成
                    if (randomRoomRoadLastX == j && randomRoadX - 2 <= i && i < randomRoadX)
                    {
                        mapStatus[i, j] = TileStatus.Road;
                    }
                    if (randomRoomRoadLastY2 == i && randomRoadY - 2 <= j && j < randomRoadY)
                    {
                        mapStatus[i, j] = TileStatus.Road;
                    }
                }
            }
        }
        //X軸の道を消す
        for (int i = 0; i < mapSizeX - 1; i++)
        {
            if (randomRoadX == i)
            {
                for (int j = 0; j < mapSizeY - 1; j++)
                {
                    if (mapStatus[i + 1, j] == TileStatus.Road || mapStatus[i - 1, j] == TileStatus.Road)
                    {
                        break;
                    }
                    mapStatus[i, j] = TileStatus.Wall;
                }
                for (int j = mapSizeY - 1; 0 < j; j--)
                {
                    if (mapStatus[i + 1, j] == TileStatus.Road || mapStatus[i - 1, j] == TileStatus.Road)
                    {
                        break;
                    }
                    mapStatus[i, j] = TileStatus.Wall;
                }
            }
        }
        //Y軸の道を消す
        for (int j = 0; j < mapSizeY - 1; j++)
        {
            if (randomRoadY == j)
            {
                if (randomRoadX < mapSizeX / 2)
                {
                    for (int i = mapSizeX - 1; 0 < i; i--)
                    {
                        if (mapStatus[i, j + 1] == TileStatus.Road || mapStatus[i, j - 1] == TileStatus.Road)
                        {
                            break;
                        }
                        mapStatus[i, j] = TileStatus.Wall;
                    }
                }
                else
                {
                    for (int i = 0; i < mapSizeX - 1; i++)
                    {
                        if (mapStatus[i, j + 1] == TileStatus.Road || mapStatus[i, j - 1] == TileStatus.Road)
                        {
                            break;
                        }
                        mapStatus[i, j] = TileStatus.Wall;
                    }

                }
            }
        }

        //タイルを置く
        for (int i = 0; i < mapSizeX - 1; i++)
        {
            for (int j = 0; j < mapSizeY - 1; j++)
            {
                m_vector3Int = new Vector3Int(i, j, 0);
                switch (mapStatus[i, j])
                {
                    case TileStatus.Wall:
                        m_tilemap.SetTile(m_vector3Int, m_wallTile);
                        break;
                    case TileStatus.Road:
                        m_tilemap.SetTile(m_vector3Int, m_roadTile);
                        break;
                }

            }
        }
    }

    /// <summary>tileステータス</summary>
    enum TileStatus
    {
        Wall,
        Road
    }
}
