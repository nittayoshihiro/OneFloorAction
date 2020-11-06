using UnityEngine;
using UnityEngine.Tilemaps;
public class AutoMappingV2 : MonoBehaviour
{
    [SerializeField] Tilemap m_tilemap;
    [SerializeField] int m_mapSizeX = 30;
    [SerializeField] int m_mapSizeY = 20;
    [SerializeField] Vector3Int m_vector3Int = new Vector3Int(0, 0, 0);
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
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AutoMappingButton()
    {
        AutoMapping(m_mapSizeX, m_mapSizeY);
    }
    void AutoMapping(int mapSizeX, int mapSizeY)
    {
        m_tilemap.ClearAllTiles();
        m_vector3Int = new Vector3Int(0, 0, 0);

        //マップのステータスを配列で管理する
        TileStatus[] mapStatus = new TileStatus[mapSizeX * mapSizeY];
        //マップ全体に壁とする
        for (int i = 0; i < mapStatus.Length; i++)
        {
            mapStatus[i] = TileStatus.Wall;
        }
        //マップを分割するX
        System.Random random = new System.Random();
        int randomX = random.Next(8, mapSizeX - 8);//部屋を作るときの最低サイズ（８）
        for (int i = 0; i < mapStatus.Length; i++)
        {
            if (i % mapSizeX == randomX - 1)//配列は0スタートだから-1
            {
                mapStatus[i] = TileStatus.Road;
            }
        }
        int roadCount = 0;
        int randomRoad = random.Next(3, mapSizeY - 2);
        //分割して小さいほうには1：1サイズ部屋を作る
        if (randomX < mapSizeX / 2)
        {
            for (int i = 0; i < mapStatus.Length; i++)
            {
                //初めの2つでY軸に幅を与え、後の2つでX軸に幅を与えてる
                if (mapSizeX * 2 < i && i < mapStatus.Length - mapSizeX * 2 && 2 <= i % mapSizeX && i % mapSizeX <= randomX - 4)
                {
                    mapStatus[i] = TileStatus.Road;
                }
                //道を境目につなげる
                if (i % mapSizeX == randomX - 1)
                {
                    roadCount++;
                    if (roadCount == randomRoad)
                    {
                        mapStatus[i - 1] = TileStatus.Road;
                        mapStatus[i - 2] = TileStatus.Road;
                    }
                }
            }
            //マップを分割するY
            int randomY = random.Next(8, mapSizeY - 8);//部屋を作るときの最低サイズ（８）
            for (int i = 0; i < mapStatus.Length; i++)
            {
                if (i % (mapSizeX * randomY) == 0 && 0 != i)//Y軸を
                {
                    i += randomX - 1;//X軸から伸ばすため
                    while (i % mapSizeX < mapSizeX - 1)//X軸の端まで
                    {
                        i++;
                        mapStatus[i] = TileStatus.Road;
                    }
                    break;
                }

            }
            //小さいほうに1:1の部屋を作る
            if (randomY < mapSizeY / 2)
            {
                for (int i = 0; i < mapStatus.Length; i++)
                {
                    //最初の２でY座標を確立、後の２つでX座標を確立
                    if (mapSizeX * 2 <= i && i <= (randomY - 2) * mapSizeX - 1 && randomX + 2 <= i % mapSizeX && i % mapSizeX <= mapSizeX - 3)
                    {
                        mapStatus[i] = TileStatus.Road;
                    }
                    //もう一つ大きい部屋の場合も部屋を作る
                    if (mapSizeX * (randomY + 3) <= i && i <= (mapSizeY - 2) * mapSizeX - 1 && randomX + 2 <= i % mapSizeX && i % mapSizeX <= mapSizeX - 3)
                    {
                        mapStatus[i] = TileStatus.Road;
                    }
                }
            }
            else
            {
                for (int i = 0; i < mapStatus.Length; i++)
                {
                    //最初の２でY座標を確立、後の２つでX座標を確立
                    if (mapSizeX * 2 <= i && i <= (randomY - 2) * mapSizeX - 1 && randomX + 2 <= i % mapSizeX && i % mapSizeX <= mapSizeX - 3)
                    {
                        mapStatus[i] = TileStatus.Road;
                    }
                    //もう一つ大きい部屋の場合も部屋を作る
                    if (mapSizeX * (randomY + 3) <= i && i <= (mapSizeY - 2) * mapSizeX - 1 && randomX + 2 <= i % mapSizeX && i % mapSizeX <= mapSizeX - 3)
                    {
                        mapStatus[i] = TileStatus.Road;
                    }
                }
            }

        }
        else
        {
            for (int i = 0; i < mapStatus.Length; i++)
            {
                //初めの2つでY軸に幅を与え、後の2つでX軸に幅を与えてる
                if (mapSizeX * 2 < i && i < mapStatus.Length - mapSizeX * 2 && randomX + 2 <= i % mapSizeX && i % mapSizeX <= mapSizeX - 2)
                {
                    mapStatus[i] = TileStatus.Road;
                }
                //道を境目につなげる
                if (i % mapSizeX == randomX - 1)
                {
                    roadCount++;
                    if (roadCount == randomRoad)
                    {
                        mapStatus[i + 1] = TileStatus.Road;
                        mapStatus[i + 2] = TileStatus.Road;
                    }
                }
            }
            //マップを分割するY
            int randomY = random.Next(8, mapSizeY - 8);//部屋を作るときの最低サイズ（８）
            for (int i = 0; i < mapStatus.Length; i++)
            {
                if (i % (mapSizeX * randomY) == 0 && 0 != i)//Y軸を
                {
                    while (mapStatus[i] == TileStatus.Wall)//X軸の壁にに重なるまで
                    {
                        mapStatus[i] = TileStatus.Road;
                        i++;
                    }
                    break;
                }
            }
            //小さいほうに1:1の部屋を作る
            if (randomY < mapSizeY / 2)
            {
                for (int i = 0; i < mapStatus.Length; i++)
                {
                    //最初の２でY座標を確立、後の２つでX座標を確立
                    if (mapSizeX * 2 <= i && i < mapSizeX * (randomY - 2) && 2 <= i % mapSizeX && i % mapSizeX <= randomX - 4)
                    {
                        mapStatus[i] = TileStatus.Road;
                    }
                    //もう一つ大きい部屋の場合も部屋を作る
                    if (mapSizeX * (randomY+3) <= i && i < mapSizeX * (mapSizeY - 2) && 2 <= i % mapSizeX && i % mapSizeX <= randomX - 4)
                    {
                        mapStatus[i] = TileStatus.Road;
                    }
                }
            }
            else
            {
                for (int i = 0; i < mapStatus.Length; i++)
                {
                    //最初の２でY座標を確立、後の２つでX座標を確立
                    if (mapSizeX * 2 <= i && i < mapSizeX * (randomY - 2) && 2 <= i % mapSizeX && i % mapSizeX <= randomX - 4)
                    {
                        mapStatus[i] = TileStatus.Road;
                    }
                    //もう一つ大きい部屋の場合も部屋を作る
                    if (mapSizeX * (randomY + 3) <= i && i < mapSizeX * (mapSizeY - 2) && 2 <= i % mapSizeX && i % mapSizeX <= randomX - 4)
                    {
                        mapStatus[i] = TileStatus.Road;
                    }
                }
            }
        }







        TilePut(mapStatus, mapSizeX, mapSizeY);
    }

    void TilePut(TileStatus[] mapPutStatus, int mapPutSizeX, int mapPutSizeY)
    {
        //タイルを置く
        for (int i = 0; i < mapPutStatus.Length;)
        {
            for (int y = 0; y < mapPutSizeY; y++)
            {
                for (int x = 0; x < mapPutSizeX; x++)
                {
                    switch (mapPutStatus[i])
                    {
                        case TileStatus.Wall:
                            m_tilemap.SetTile(m_vector3Int, m_wallTile[0]);
                            break;
                        case TileStatus.Road:
                            m_tilemap.SetTile(m_vector3Int, m_roadTile[0]);
                            break;
                    }
                    i++;
                    m_vector3Int.x++;
                }
                m_vector3Int.y++;
                m_vector3Int.x -= mapPutSizeX;
            }
        }
    }



    enum TileStatus
    {
        Wall,
        Road
    }
}