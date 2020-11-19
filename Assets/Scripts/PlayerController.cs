using Cinemachine;//Cinemachineを使うため
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float m_playerSpeed = 0.1f;
    public Coroutine m_myCor;
    AutoMappingV3.TileStatus[,] m_mapStatus;

    // Start is called before the first frame update
    void Start()
    {
        //Virtual Camera がプレイヤーを見るように設定する
        CinemachineVirtualCamera m_vCam = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        if (m_vCam)
        {
            m_vCam.Follow = transform;
        }
        AutoMappingV3 m_autoMapping = GameObject.FindObjectOfType<AutoMappingV3>();
        if (m_autoMapping)
        {
            m_mapStatus = m_autoMapping.GetMappingData;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = this.gameObject.transform.position;
        float v = Input.GetAxisRaw("Vertical");     // 水平方向の入力を取得する
        float h = Input.GetAxisRaw("Horizontal");   // 垂直方向の入力を取得する

        if (0 < v)
        {
            if (m_mapStatus[(int)pos.x, (int)pos.y + 1] != AutoMappingV3.TileStatus.Wall)
            {
                Vector3 willPos = new Vector3(pos.x, pos.y + 1f, pos.z);
                StartCor(willPos);
            }
        }
        else if (v < 0)
        {
            if (m_mapStatus[(int)pos.x, (int)pos.y - 1] != AutoMappingV3.TileStatus.Wall)
            {
                Vector3 willPos = new Vector3(pos.x, pos.y - 1f, pos.z);
                StartCor(willPos);
            }
        }
        else if (0 < h)
        {
            if (m_mapStatus[(int)pos.x + 1, (int)pos.y] != AutoMappingV3.TileStatus.Wall)
            {
                Vector3 willPos = new Vector3(pos.x + 1f, pos.y, pos.z);
                StartCor(willPos);
            }
        }
        else if (h < 0)
        {
            if (m_mapStatus[(int)pos.x - 1, (int)pos.y] != AutoMappingV3.TileStatus.Wall)
            {
                Vector3 willPos = new Vector3(pos.x - 1f, pos.y, pos.z);
                StartCor(willPos);
            }
        }

        if (m_mapStatus[(int)pos.x, (int)pos.y] == AutoMappingV3.TileStatus.Goal)
        {
            Debug.Log("ゴール");
            Destroy(this.gameObject);
        }

    }

    //MoveToをスタートさせるメソッド
    //外部からコルーチンを呼び出すときはこのメソッドを使う
    public void StartCor(Vector3 goal)
    {
        if (m_myCor == null)
        {
            m_myCor = StartCoroutine(MoveTo(goal));
        }

    }

    //goalの位置までスムーズに移動する
    public IEnumerator MoveTo(Vector3 goal)
    {
        while (Vector3.Distance(transform.position, goal) > 0.1f)
        {
            Vector3 nextPos = Vector3.Lerp(transform.position, goal, Time.deltaTime * m_playerSpeed);
            transform.position = nextPos;
            //ここまでが1フレームの間に処理される
            yield return null;
        }
        //ポジションを修正
        transform.position = goal;
        m_myCor = null;
        print("終了");
        //処理が終わったら破棄する
        yield break;
    }



    enum PlayerMoveStatus
    {
        /// <summary>上移動</summary>
        Up,
        /// <summary>下移動</summary>
        Down,
        /// <summary>左移動</summary>
        Lift,
        /// <summary>右移動</summary>
        Right
    }
}
