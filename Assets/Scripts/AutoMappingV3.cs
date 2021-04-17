using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
///マッピングクラス
/// </summary>
public class AutoMappingV3 : MonoBehaviour
{
    //マップサイズXY
    /// <summary>マップサイズX</summary>
    [SerializeField] int m_mapSizeX = 30;
    /// <summary>マップサイズY</summary>
    [SerializeField] int m_mapSizeY = 20;
    //部屋サイズ
    /// <summary>部屋最低サイズ</summary>
    [SerializeField] int m_minimumRoomSize = 4;
    /// <summary>タイルマップ</summary>
    [SerializeField] Tilemap m_tilemap;
    /// <summary>壁のタイル</summary>
    [SerializeField] Tile m_wallTile;
    /// <summary>道のタイル</summary>
    [SerializeField] Tile m_roadTile;
    /// <summary>プレイヤーのタイル（スタート）</summary>
    [SerializeField] Tile m_playerTile;
    /// <summary>ゴールのタイル</summary>
    [SerializeField] Tile m_goalTile;
    /// <summary>プレイヤーゲームプレハブ</summary>
    [SerializeField] GameObject m_playerPrefab;
    /// <summary></summary>
    Vector3Int m_vector3Int = new Vector3Int(0, 0, 0);
    /// <summary></summary>
    TileStatus[,] m_mapStatus;
    /// <summary>ゴールポジション</summary>
    Vector3 m_goalPosition;

    /// <summary></summary>
    public void AutoMapping()
    {
        Debug.Log("Mapping");
        m_tilemap.ClearAllTiles();
        AutoMapping(m_mapSizeX, m_mapSizeY, m_minimumRoomSize);
    }

    /// <summary>マップステータスを取得</summary>
    public TileStatus[,] GetMappingData
    {
        get { return m_mapStatus; }
    }

    void AutoMapping(int mapSizeX, int mapSizeY, int minimumRoomSize)
    {
        //全部壁にする
        m_mapStatus = new TileStatus[mapSizeX, mapSizeY];
        for (int i = 0; i < mapSizeX - 1; i++)
        {
            for (int j = 0; j < mapSizeY - 1; j++)
            {
                m_mapStatus[i, j] = TileStatus.Wall;
            }
        }
        //区域分割法に乗っ取って部屋を3つに分解する。部屋は道によって分割される。以降で道をどこに引くか計算する
        int randomRoadX = Random.Range(minimumRoomSize + 4, mapSizeX - (minimumRoomSize + 5));//randomRoadXは垂直方向に区切る道のx座標
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
                    m_mapStatus[i, j] = TileStatus.Road;
                }
                //小さいほうに部屋を作る
                if (randomRoadX < mapSizeX / 2)
                {
                    if (2 <= i && i < randomRoadX - 2 && 2 <= j && j <= mapSizeY - 2 * 2)
                    {
                        m_mapStatus[i, j] = TileStatus.Road;
                    }
                    //ゴールを作成
                    if (i == (2 + randomRoadX - 2) / 2 && j == (2 + mapSizeY - 2 * 2) / 2)
                    {
                        m_mapStatus[i, j] = TileStatus.Goal;
                    }
                    //道作成
                    if (randomRoomRoadX == j && randomRoadX - 2 <= i && i < randomRoadX)
                    {
                        m_mapStatus[i, j] = TileStatus.Road;
                    }
                    //Y軸で区切る
                    if (randomRoadY == j && randomRoadX < i)
                    {
                        m_mapStatus[i, j] = TileStatus.Road;
                    }
                    //Y軸下部屋
                    if (randomRoadX + 2 < i && i <= mapSizeX - 2 * 2 && 2 <= j && j < randomRoadY - 2)
                    {
                        m_mapStatus[i, j] = TileStatus.Road;
                    }
                    //プレイヤースポーンタイル
                    if (i == (randomRoadX + 2 + mapSizeX - 2 * 2) / 2 && j == (2 + randomRoadY - 2) / 2)
                    {
                        m_mapStatus[i, j] = TileStatus.Player;
                    }
                    //道を作成
                    if (randomRoomRoadY == i && randomRoadY < j && j <= randomRoadY + 2)
                    {
                        m_mapStatus[i, j] = TileStatus.Road;
                    }
                    //Y軸上部屋
                    if (randomRoadX + 2 < i && i <= mapSizeX - 2 * 2 && randomRoadY + 2 < j && j <= mapSizeY - 2 * 2)
                    {
                        m_mapStatus[i, j] = TileStatus.Road;
                    }
                    //最後は２本道作成
                    if (randomRoomRoadLastX == j && randomRoadX < i && i <= randomRoadX + 2)
                    {
                        m_mapStatus[i, j] = TileStatus.Road;
                    }
                    if (randomRoomRoadLastY == i && randomRoadY - 2 <= j && j < randomRoadY)
                    {
                        m_mapStatus[i, j] = TileStatus.Road;
                    }
                }
                else
                {
                    if (randomRoadX + 2 < i && i <= mapSizeX - 2 * 2 && 2 <= j && j <= mapSizeY - 2 * 2)
                    {
                        m_mapStatus[i, j] = TileStatus.Road;
                    }
                    //ゴールを作成
                    if (i == (randomRoadX + 2 + mapSizeX - 2 * 2) / 2 && j == (2 + mapSizeY - 2 * 2) / 2)
                    {
                        m_mapStatus[i, j] = TileStatus.Goal;
                    }
                    //道作成
                    if (randomRoomRoadX == j && randomRoadX < i && i <= randomRoadX + 2)
                    {
                        m_mapStatus[i, j] = TileStatus.Road;
                    }
                    //Y軸で区切る
                    if (j == randomRoadY && i < randomRoadX)
                    {
                        m_mapStatus[i, j] = TileStatus.Road;
                    }
                    //Y軸下部屋
                    if (2 <= i && i < randomRoadX - 2 && 2 <= j && j < randomRoadY - 2)
                    {
                        m_mapStatus[i, j] = TileStatus.Road;
                    }
                    //道を作成
                    if (randomRoomRoadY2 == i && randomRoadY < j && j <= randomRoadY + 2)
                    {
                        m_mapStatus[i, j] = TileStatus.Road;
                    }
                    //Y軸上部屋
                    if (2 <= i && i < randomRoadX - 2 && randomRoadY + 2 < j && j <= mapSizeY - 2 * 2)
                    {
                        m_mapStatus[i, j] = TileStatus.Road;
                    }
                    //プレイヤースポーンタイル
                    if (i == (2 + randomRoadX - 2) / 2 && j == (2 + randomRoadY - 2) / 2)
                    {
                        m_mapStatus[i, j] = TileStatus.Player;
                    }
                    //最後は２本道作成
                    if (randomRoomRoadLastX == j && randomRoadX - 2 <= i && i < randomRoadX)
                    {
                        m_mapStatus[i, j] = TileStatus.Road;
                    }
                    if (randomRoomRoadLastY2 == i && randomRoadY - 2 <= j && j < randomRoadY)
                    {
                        m_mapStatus[i, j] = TileStatus.Road;
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
                    if (m_mapStatus[i + 1, j] == TileStatus.Road || m_mapStatus[i - 1, j] == TileStatus.Road)
                    {
                        break;
                    }
                    m_mapStatus[i, j] = TileStatus.Wall;
                }
                for (int j = mapSizeY - 1; 0 < j; j--)
                {
                    if (m_mapStatus[i + 1, j] == TileStatus.Road || m_mapStatus[i - 1, j] == TileStatus.Road)
                    {
                        break;
                    }
                    m_mapStatus[i, j] = TileStatus.Wall;
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
                        if (m_mapStatus[i, j + 1] == TileStatus.Road || m_mapStatus[i, j - 1] == TileStatus.Road)
                        {
                            break;
                        }
                        m_mapStatus[i, j] = TileStatus.Wall;
                    }
                }
                else
                {
                    for (int i = 0; i < mapSizeX - 1; i++)
                    {
                        if (m_mapStatus[i, j + 1] == TileStatus.Road || m_mapStatus[i, j - 1] == TileStatus.Road)
                        {
                            break;
                        }
                        m_mapStatus[i, j] = TileStatus.Wall;
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
                switch (m_mapStatus[i, j])
                {
                    case TileStatus.Wall:
                        m_tilemap.SetTile(m_vector3Int, m_wallTile);
                        break;
                    case TileStatus.Road:
                        m_tilemap.SetTile(m_vector3Int, m_roadTile);
                        break;
                    case TileStatus.Player:
                        m_tilemap.SetTile(m_vector3Int, m_playerTile);
                        Instantiate(m_playerPrefab, new Vector3(m_vector3Int.x + 0.5f, m_vector3Int.y + 0.5f, m_vector3Int.z), Quaternion.identity);
                        break;
                    case TileStatus.Goal:
                        m_tilemap.SetTile(m_vector3Int, m_goalTile);
                        m_goalPosition = new Vector3(m_vector3Int.x + 0.5f, m_vector3Int.y + 0.5f, 0);
                        break;
                }

            }
        }
    }

    /// <summary>tileステータス</summary>
    public enum TileStatus
    {
        Wall,
        Road,
        Player,
        Goal
    }
}
