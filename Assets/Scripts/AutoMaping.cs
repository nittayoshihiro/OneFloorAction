using UnityEngine;
using UnityEngine.Tilemaps;

public class AutoMaping : MonoBehaviour
{
    [SerializeField] Tilemap m_tilemap;
    /// <summary>部屋の1辺の長さ</summary>
    [SerializeField] int m_roomSize = 5;
    /// <summary>壁のタイル</summary>
    Tile[] m_wallTile;
    /// <summary>道のタイル</summary>
    Tile[] m_roadTile;
    // Start is called before the first frame update
    void Start()
    {
        //Resourcesフォルダーからタイルを読み込む
        m_roadTile = Resources.LoadAll<Tile>("RoadPalette");
        m_wallTile = Resources.LoadAll<Tile>("WallPalette");
        Vector3Int m_vector3Int = new Vector3Int(0, 0, 0);
        RoomMaping(m_wallTile[0], m_roadTile[0], m_vector3Int, m_roomSize);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void RoomMaping(Tile wallTile, Tile roadTile, Vector3Int position, int size)
    {
        int tileIndex = size * size;
        //size×sizeの部屋を想定
        Tile[] tile = new Tile[tileIndex];
        //置くtileを配列に収納
        for (int i = 0; i < tile.Length; i++)
        {
            if (i < size || i % size == 0 || i % size == size - 1 || i > tileIndex - size)
            {
                tile[i] = wallTile;
            }
            else
            {
                tile[i] = roadTile;
            }
        }

        int putIndex = 0;
        //配列からtileを置く
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                position = new Vector3Int(x, y, 0);
                m_tilemap.SetTile(position, tile[putIndex]);
                putIndex++;
            }
        }
    }
}
