using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Astar1 : MonoBehaviour
{
    Vector3[] m_vector3s;
    AutoMappingV3.TileStatus[,] m_mapStatus;
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
    Map_Date[,] AstarMove()
    {
        Map_Date map = new Map_Date();
        Map_Date[,] nodeStatuses = new Map_Date[m_mapStatus.GetLength(1),m_mapStatus.GetLength(2)];
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
                nodeStatuses[x, y] = map;
            }
        }
        return nodeStatuses;
    }

   　/// <summary>
    /// 検索して最短ルートを探す
    /// </summary>
    /// <param name="nodeStatus"></param>
    public void SearchStart(Vector3 startPos,Vector3 goalPos)
    {
        //ノードの初期化
        Map_Date[,] nodeStatuses = AstarMove();
        //スタートノードをポジションをいれノードをopenする。
        m_startMap_Date.tilePos = startPos;
        m_startMap_Date.nodeStatus = NodeStatus.Open;
        m_startMap_Date.C = 0;
        //sqrt(平方根)Pow(x,y乗)
        m_startMap_Date.H = (int)Math.Sqrt(Math.Pow(startPos.x-goalPos.x,2)+Math.Pow(startPos.y - goalPos.y,2));
        m_startMap_Date.S = m_startMap_Date.C + m_startMap_Date.H;
        Array.Resize(ref m_openNodeStatuses, 1);
        m_openNodeStatuses[0]=  m_startMap_Date;
        Search(nodeStatuses,m_openNodeStatuses,m_closeNodeStatuses,goalPos);
    }

    //Openのーどで一番コストが低いあたいから
    void Search(Map_Date[,] nodeStatus, Map_Date[] openNodeStatuses, Map_Date[] closeNodeStaruses,Vector3 goalPos)
    {
        BubbleSort(openNodeStatuses);
        Map_Date map_DateNow = openNodeStatuses[0];
        openNodeStatuses = FirstArrayRemove(openNodeStatuses);

        //ここで上下左右にノードOpenする
        if (nodeStatus[(int)map_DateNow.tilePos.x++, (int)map_DateNow.tilePos.y].nodeStatus == NodeStatus.None)
        {
            nodeStatus[(int)map_DateNow.tilePos.x++, (int)map_DateNow.tilePos.y].nodeStatus = NodeStatus.Open;
            Map_Date map_DateUp = nodeStatus[(int)map_DateNow.tilePos.x++, (int)map_DateNow.tilePos.y];
            map_DateUp.map_date = map_DateNow;
            map_DateUp.C = map_DateNow.C++;
            map_DateUp.H = (int)Math.Sqrt(Math.Pow(map_DateUp.tilePos.x - goalPos.x, 2) + Math.Pow(map_DateUp.tilePos.y - goalPos.y, 2));
            map_DateUp.S = map_DateUp.C + map_DateUp.H;
            Array.Resize(ref openNodeStatuses, openNodeStatuses.Length + 1);
            openNodeStatuses[openNodeStatuses.Length - 1] = map_DateUp;
        }
        if (nodeStatus[(int)map_DateNow.tilePos.x--, (int)map_DateNow.tilePos.y].nodeStatus == NodeStatus.None)
        {
            nodeStatus[(int)map_DateNow.tilePos.x--, (int)map_DateNow.tilePos.y].nodeStatus = NodeStatus.Open;
            Map_Date map_DateDown = nodeStatus[(int)map_DateNow.tilePos.x--, (int)map_DateNow.tilePos.y];
            map_DateDown.map_date = map_DateNow;
            map_DateDown.C = map_DateNow.C++;
            map_DateDown.H = (int)Math.Sqrt(Math.Pow(map_DateDown.tilePos.x - goalPos.x, 2) + Math.Pow(map_DateDown.tilePos.y - goalPos.y, 2));
            map_DateDown.S = map_DateDown.C + map_DateDown.H;
            Array.Resize(ref openNodeStatuses, openNodeStatuses.Length + 1);
            openNodeStatuses[openNodeStatuses.Length - 1] = map_DateDown; 

        }
        if (nodeStatus[(int)map_DateNow.tilePos.x, (int)map_DateNow.tilePos.y--].nodeStatus == NodeStatus.None)
        {
            nodeStatus[(int)map_DateNow.tilePos.x, (int)map_DateNow.tilePos.y--].nodeStatus = NodeStatus.Open;
            Map_Date map_DateLeft = nodeStatus[(int)map_DateNow.tilePos.x, (int)map_DateNow.tilePos.y--];
            map_DateLeft.map_date = map_DateNow;
            map_DateLeft.C = map_DateNow.C++;
            map_DateLeft.H = (int)Math.Sqrt(Math.Pow(map_DateLeft.tilePos.x - goalPos.x, 2) + Math.Pow(map_DateLeft.tilePos.y - goalPos.y, 2));
            map_DateLeft.S = map_DateLeft.C + map_DateLeft.H;
            Array.Resize(ref openNodeStatuses,openNodeStatuses.Length + 1);
            openNodeStatuses[openNodeStatuses.Length - 1] = map_DateLeft;
        }
        if (nodeStatus[(int)map_DateNow.tilePos.x, (int)map_DateNow.tilePos.y++].nodeStatus == NodeStatus.None)
        {
            nodeStatus[(int)map_DateNow.tilePos.x, (int)map_DateNow.tilePos.y--].nodeStatus = NodeStatus.Open;
            Map_Date map_DateRight = nodeStatus[(int)map_DateNow.tilePos.x, (int)map_DateNow.tilePos.y--];
            map_DateRight.map_date = map_DateNow;
            map_DateRight.C = map_DateNow.C++;
            map_DateRight.H = (int)Math.Sqrt(Math.Pow(map_DateRight.tilePos.x - goalPos.x, 2) + Math.Pow(map_DateRight.tilePos.y - goalPos.y, 2));
            map_DateRight.S = map_DateRight.C + map_DateRight.H;
            Array.Resize(ref openNodeStatuses, openNodeStatuses.Length + 1);
            openNodeStatuses[openNodeStatuses.Length - 1] = map_DateRight;
        }

        //ゴールに辿り着いてない場合
        if (nodeStatus[(int)map_DateNow.tilePos.x++, (int)map_DateNow.tilePos.y].tilePos != goalPos
            || nodeStatus[(int)map_DateNow.tilePos.x--, (int)map_DateNow.tilePos.y].tilePos != goalPos
            || nodeStatus[(int)map_DateNow.tilePos.x, (int)map_DateNow.tilePos.y--].tilePos != goalPos
            || nodeStatus[(int)map_DateNow.tilePos.x, (int)map_DateNow.tilePos.y++].tilePos != goalPos)
        {
            //ノードステータスを閉じてcloseNodeStarusesに入れる
            map_DateNow.nodeStatus = NodeStatus.Close;
            Array.Resize(ref closeNodeStaruses, closeNodeStaruses.Length + 1);
            closeNodeStaruses[closeNodeStaruses.Length - 1] = map_DateNow;

            //再起関数
            Search(nodeStatus, openNodeStatuses, closeNodeStaruses, goalPos);
        }
        else
        {
            Rewind(nodeStatus,goalPos);
        }
    }

    /// <summary>
    /// 先頭の要素を抜いてサイズ変更した配列を返します 
    /// </summary>
    /// <param name="_array"></param>
    /// <returns></returns>
    Map_Date[] FirstArrayRemove(Map_Date[] _array)
    {
        Map_Date[] swapArray= new Map_Date[_array.Length-1];
        for (int i = 0; i < swapArray.Length; i++)
        {
            swapArray[i] = _array[i++];
        }
        return swapArray;
    }

    /// <summary>
    /// スコアに応じてソートします
    /// </summary>
    /// <param name="_array"></param>
    /// <returns></returns>
    Map_Date[] BubbleSort(Map_Date[] _array)
    {
        //配列の回数分回す
        for (int i = 0; i < _array.Length; i++)
        {
            //配列の回数分回す
            for (int j = 0; j < _array.Length; j++)
            {
                //比較元より大きければ入れ替え
                if (_array[i].S < _array[j].S)
                {
                    Map_Date x = _array[j];
                    _array[j] = _array[i];
                    _array[i] = x;
                }
            }
        }

        //Sortした結果を返す
        return _array;
    }

    /// <summary>
    /// サーチ巻き戻し
    /// </summary>
    void Rewind(Map_Date[,] nodeStatus, Vector3 goalPos)
    {
        Array.Resize(ref m_vector3s,2);
        m_vector3s[0] = nodeStatus[(int)goalPos.x, (int)goalPos.y].tilePos;
        Map_Date map_DateParent = nodeStatus[(int)goalPos.x, (int)goalPos.y].map_date;
        m_vector3s[1] = map_DateParent.tilePos; 
        for (int i = 2; ; i++)
        {
            //親がいる場合記録する
            if (map_DateParent.map_date!=null)
            {
                map_DateParent = map_DateParent.map_date;
                Array.Resize(ref m_vector3s, i++);
                m_vector3s[i] = map_DateParent.tilePos;
            }
            else
            {
                break;
            }
            
        }

    }

    public enum NodeStatus
    {
        None,
        Open,
        Close,
        Wall
    }

    /// <summary>
    /// 探索ルート
    /// </summary>
     public Vector3[] GetRoute
    {
        get { return m_vector3s; }
    }
}

public class Map_Date
{
    public Vector3 tilePos; 　　　　　　　　　　　　//タイルのポジション
    public Astar1.NodeStatus nodeStatus;        //ノードステータス
    public AutoMappingV3.TileStatus tileStatus; //タイルのステータス
    public int C; 　　　　　　　　　　　　　　　　　　//移動コスト
    public int H; 　　　　　　　　　　　　　　　　　　//推定コスト
    public int S; 　　　　　　　　　　　　　　　　　　//合計コスト
    public Map_Date map_date;　　　　　　　　　    //親のMap_Date
} 
