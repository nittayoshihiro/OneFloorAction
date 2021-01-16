using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar1 : MonoBehaviour
{
    AutoMappingV3.TileStatus[,] m_mapStatus;
    Map_Date[,] m_nodeStatuses;
    Map_Date map;
    //Open、Closeノードを配列に保管する
    Map_Date[] m_openNodeStatuses;
    Map_Date[] m_closeNodeStatuses;
    //スタートノードを作る
    Map_Date m_startMap_Date;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MapReset()
    {
        //今のマップデータを取得
        GameObject canvasManager = GameObject.Find("CanvasManager");
        AutoMappingV3 autoMappingV3 = canvasManager.GetComponent<AutoMappingV3>();
        m_mapStatus = autoMappingV3.GetMappingData;
    }

    //ノードを作成する
   public void AstarMove()
    {
        for (int x = 0; x < m_mapStatus.GetLength(1); x++)
        {
            for (int y = 0; y < m_mapStatus.GetLength(2); y++)
            {
                //マップデータを作成
                map.tilePos = new Vector3(x, y, 0);
                if (m_mapStatus[x,y]==AutoMappingV3.TileStatus.Wall)
                {
                    map.nodeStatus = NodeStatus.Wall;
                }
                else
                {
                    map.nodeStatus = NodeStatus.None;
                }
                m_nodeStatuses[x, y] = map;
            }
        }
    }

   　/// <summary>
    /// 検索して最短ルートを探す
    /// </summary>
    /// <param name="nodeStatus"></param>
    public void SerachStart(NodeStatus [,] nodeStatuses,Vector3 startPos,Vector3 goalPos)
    {
        //スタートノードをポジションをいれノードをopenする。
        m_startMap_Date.tilePos = startPos;
        m_startMap_Date.nodeStatus = NodeStatus.Open;
        m_startMap_Date.H = 0;
        m_openNodeStatuses[0]=  m_startMap_Date;
        Search(nodeStatuses,m_openNodeStatuses,m_closeNodeStatuses,goalPos);
    }

    //Openのーどで一番コストが低いあたいから
    void Search(NodeStatus[,] nodeStatus, Map_Date[] openNodeStatuses, Map_Date[] closeNodeStaruses,Vector3 goalPos)
    {
        //コストソートをここでする
        Map_Date map_Date = openNodeStatuses[0];
        //ここで
    }

    public enum NodeStatus
    {
        None,
        Open,
        Close,
        Wall
    }
}

public class Map_Date
{
    public Vector3 tilePos; 　　　　　　　　　　　　//タイルのポジション
    public Astar1.NodeStatus nodeStatus;        //ノードステータス
    public AutoMappingV3.TileStatus tileStatus; //タイルのステータス
    public int C; 　　　　　　　　　　　　　　　　　　//推定コスト
    public int H; 　　　　　　　　　　　　　　　　　　//移動コスト
    public int S; 　　　　　　　　　　　　　　　　　　//合計コスト
    public Map_Date map_date;　　　　　　　　　    //親のMap_Date
} 
