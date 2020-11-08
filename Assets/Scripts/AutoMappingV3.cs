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
        int randomRoadX = Random.Range(minimumRoomSize, mapSizeX - minimumRoomSize);
        //X軸で区切る
        for (int i = 0; i < mapSizeX - 1; i++)
        {
            for (int j = 0; j < mapSizeY - 1; j++)
            {
                if (i == randomRoadX)
                {
                    mapStatus[i, j] = TileStatus.Road;
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
