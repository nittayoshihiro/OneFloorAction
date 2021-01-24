using UnityEngine;
using System;

public class Astar : MonoBehaviour
{
    AutoMappingV3.TileStatus[,] m_mapStatus;
    NodeStatus[,] m_nodeStatus;
    Vector3[] m_openVector3s;
    Vector3[] m_closeVector3s;
    Vector3[] m_route;
    int m_moveCount=0;

    Vector3[] RoutePos
    {
        get { return m_route; }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SearchMove(Vector3 startPosition, Vector3 goalPosition)
    {
        //今のマップデータを取得
        GameObject canvasManager = GameObject.Find("CanvasManager");
        AutoMappingV3 autoMappingV3 = canvasManager.GetComponent<AutoMappingV3>();
        m_mapStatus = autoMappingV3.GetMappingData;

        //ノードの基礎データを作成
        m_nodeStatus = new NodeStatus[m_mapStatus.GetLength(0), m_mapStatus.GetLength(1)];
        for (int i = 0; i < m_mapStatus.GetLength(0); i++)
        {
            for (int j = 0; j < m_mapStatus.GetLength(1); j++)
            {
                // 壁じゃない時
                if (m_mapStatus[i, j] != AutoMappingV3.TileStatus.Wall)
                {
                    m_nodeStatus[i, j] = NodeStatus.None;
                }
                else
                {
                    m_nodeStatus[i, j] = NodeStatus.Wall;
                }
            }
        }

        //スタートとゴールをノードに表示する
        m_nodeStatus[(int)startPosition.x, (int)startPosition.y] = NodeStatus.Start;
        m_nodeStatus[(int)goalPosition.x, (int)goalPosition.y] = NodeStatus.Goal;

        //ムーブカウントを初期化してnodeを起動する
        m_moveCount = 0;
        Node(m_nodeStatus,startPosition,goalPosition,m_moveCount);
    }

    void Node(NodeStatus[,] nodeStatuses, Vector3 startPosition, Vector3 goalPosition,int moveCount)
    {
        //実コスト
        int moveUp = 0;
        int moveDown = 0;
        int moveRight = 0;
        int moveLeft = 0;

        //コストを出すための仮Pos
        Vector3 pos;

        //ゴールポジションだったら終了
        if (m_nodeStatus[(int)startPosition.x, (int)startPosition.y++] == NodeStatus.Goal||
            m_nodeStatus[(int)startPosition.x, (int)startPosition.y--] == NodeStatus.Goal||
            m_nodeStatus[(int)startPosition.x++, (int)startPosition.y] == NodeStatus.Goal||
            m_nodeStatus[(int)startPosition.x--, (int)startPosition.y] == NodeStatus.Goal)
        {
             
        }
        else
        {
            //ルートPosのためにリサイズする
            Array.Resize(ref m_route, moveCount);

            //コスト計算
            if (m_nodeStatus[(int)startPosition.x, (int)startPosition.y++] == NodeStatus.None)
            {
                m_nodeStatus[(int)startPosition.x, (int)startPosition.y++] = NodeStatus.Open;
                pos = new Vector3((int)startPosition.x, (int)startPosition.y++, 0);
                moveUp = moveCount + (int)Vector3.Distance(pos, goalPosition);
            }
            if (m_nodeStatus[(int)startPosition.x, (int)startPosition.y--] == NodeStatus.None)
            {
                m_nodeStatus[(int)startPosition.x, (int)startPosition.y--] = NodeStatus.Open;
                pos = new Vector3((int)startPosition.x, (int)startPosition.y--, 0);
                moveDown = moveCount + (int)Vector3.Distance(pos, goalPosition);
            }
            if (m_nodeStatus[(int)startPosition.x++, (int)startPosition.y] == NodeStatus.None)
            {
                m_nodeStatus[(int)startPosition.x++, (int)startPosition.y] = NodeStatus.Open;
                pos = new Vector3((int)startPosition.x++, (int)startPosition.y, 0);
                moveRight = moveCount + (int)Vector3.Distance(pos, goalPosition);
            }
            if (m_nodeStatus[(int)startPosition.x--, (int)startPosition.y] == NodeStatus.None)
            {
                m_nodeStatus[(int)startPosition.x--, (int)startPosition.y] = NodeStatus.Open;
                pos = new Vector3((int)startPosition.x--, (int)startPosition.y, 0);
                moveLeft = moveCount + (int)Vector3.Distance(pos, goalPosition);
            }

            //今のノードを閉める
            m_nodeStatus[(int)startPosition.x, (int)startPosition.y] = NodeStatus.Close;

            //コストが低いのだけ、再起関数
            if (moveUp <= moveDown && moveUp <= moveRight && moveUp <= moveLeft)
            {
                m_route[moveCount] = new Vector3((int)startPosition.x, (int)startPosition.y++, 0);
                Node(nodeStatuses, m_route[moveCount], goalPosition, moveCount++);
            }
            else if (moveDown <= moveUp && moveDown <= moveRight && moveDown <= moveLeft)
            {
                m_route[moveCount] = new Vector3((int)startPosition.x, (int)startPosition.y--, 0);
                Node(nodeStatuses, m_route[moveCount], goalPosition, moveCount++);
            }
            else if (moveRight <= moveUp && moveRight <= moveDown && moveRight <= moveLeft)
            {
                m_route[moveCount] = new Vector3((int)startPosition.x++, (int)startPosition.y, 0);
                Node(nodeStatuses, m_route[moveCount], goalPosition, moveCount++);
            }
            else if (moveLeft <= moveUp && moveLeft <= moveDown && moveLeft <= moveRight)
            {
                m_route[moveCount] = new Vector3((int)startPosition.x--, (int)startPosition.y, 0);
                Node(nodeStatuses, m_route[moveCount], goalPosition, moveCount++);
            }
        }
    }

    enum NodeStatus
    {
        None,
        Open,
        Close,
        Start,
        Goal,
        Wall
    }
}
