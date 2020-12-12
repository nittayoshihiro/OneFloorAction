using UnityEngine;

public class Astar : MonoBehaviour
{
    AutoMappingV3.TileStatus[,] m_mapStatus;
    NodeStatus[,] m_nodeStatus;
    Vector3[] m_openVector3s;
    Vector3[] m_closeVector3s;

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
            }
        }

        //スタートとゴールをノードに表示する
        m_nodeStatus[(int)startPosition.x, (int)startPosition.y] = NodeStatus.Start;
        m_nodeStatus[(int)goalPosition.x, (int)goalPosition.y] = NodeStatus.Goal;

    }

    void Node(NodeStatus[,] nodeStatuses, Vector3 startPosition, Vector3 goalPosition)
    {
        if (m_nodeStatus[(int)startPosition.x++, (int)startPosition.y++] == NodeStatus.None)
        {
            m_nodeStatus[(int)startPosition.x++, (int)startPosition.y++] = NodeStatus.Open;
        }
        if (m_nodeStatus[(int)startPosition.x++, (int)startPosition.y--] == NodeStatus.None)
        {
            m_nodeStatus[(int)startPosition.x++, (int)startPosition.y--] = NodeStatus.Open;
        }
        if (m_nodeStatus[(int)startPosition.x--, (int)startPosition.y++] == NodeStatus.None)
        {
            m_nodeStatus[(int)startPosition.x--, (int)startPosition.y++] = NodeStatus.Open;
        }
        if (m_nodeStatus[(int)startPosition.x--, (int)startPosition.y--] == NodeStatus.None)
        {
            m_nodeStatus[(int)startPosition.x--, (int)startPosition.y--] = NodeStatus.Open;
        }
    }

    enum NodeStatus
    {
        None,
        Open,
        Close,
        Start,
        Goal
    }
}
