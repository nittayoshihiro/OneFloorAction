using Cinemachine;//Cinemachineを使うため
using System.Collections;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] float m_playerSpeed = 0.1f;
    //操作感度
    public float m_moveSensitivity;
    //移動判定
    bool m_X, m_Y;
    Coroutine m_myCor;
    FloatingJoystick m_joystick;
    AutoMappingV3.TileStatus[,] m_mapStatus;
    CanvasManager m_canvasManager;
    Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        //Virtual Camera がプレイヤーを見るように設定する
        CinemachineVirtualCamera m_vCam = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        if (m_vCam)
        {
            m_vCam.Follow = transform;
        }
        m_canvasManager = GameObject.FindObjectOfType<CanvasManager>();
        m_joystick = GameObject.FindObjectOfType<FloatingJoystick>();
        AutoMappingV3 m_autoMapping = GameObject.FindObjectOfType<AutoMappingV3>();
        if (m_autoMapping)
        {
            m_mapStatus = m_autoMapping.GetMappingData;
        }
        m_moveSensitivity = 0.1f * m_canvasManager.MoveSensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        m_X = true;
        m_Y = true;
        pos = this.gameObject.transform.position;
        // float y = Input.GetAxisRaw("Vertical");     // 水平方向の入力を取得する
        // float x = Input.GetAxisRaw("Horizontal");   // 垂直方向の入力を取得する
        if (false)
        {
            float y = m_joystick.Vertical;     // 水平方向の入力を取得する
            float x = m_joystick.Horizontal;   // 垂直方向の入力を取得する

            //傾きが大きい方に進む
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                DirectionGoX(pos, x, y);
            }
            else
            {
                DirectionGoY(pos, x, y);
            }
        }
       

        //自分のポジションがゴールポジションだったら
        if (m_mapStatus[(int)pos.x, (int)pos.y] == AutoMappingV3.TileStatus.Goal)
        {
            Debug.Log("ゴール");
            m_canvasManager.GoalEvent();
            Destroy(this.gameObject);
        }

    }

    /// <summary>
    /// ボタン入力用
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void ButtonMove(float x, float y)
    {
        if (true)
        {
            DirectionGoX(pos, x, y);
            DirectionGoY(pos, x, y);
        }
    }

    /// <summary>
    /// x軸方向の移動
    /// </summary>
    /// <param name="position"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void DirectionGoX(Vector3 position, float x, float y)
    {
        m_X = false;
        if (m_moveSensitivity < x)
        {
            //行き先が壁か判断する
            if (m_mapStatus[(int)position.x + 1, (int)position.y] != AutoMappingV3.TileStatus.Wall)
            {
                Vector3 willPos = new Vector3(position.x + 1f, position.y, position.z);
                StartCor(willPos);
            }
            //壁だった場合
            else if (m_mapStatus[(int)position.x + 1, (int)position.y] == AutoMappingV3.TileStatus.Wall)
            {
                if (m_Y)
                {
                    //y軸に移動する
                    DirectionGoY(position, x, y);
                }
            }
        }
        else if (x < -m_moveSensitivity)
        {
            //行き先が壁か判断する
            if (m_mapStatus[(int)position.x - 1, (int)position.y] != AutoMappingV3.TileStatus.Wall)
            {
                Vector3 willPos = new Vector3(position.x - 1f, position.y, position.z);
                StartCor(willPos);
            }
            //壁だった場合
            else if (m_mapStatus[(int)position.x - 1, (int)position.y] == AutoMappingV3.TileStatus.Wall)
            {
                if (m_Y)
                {
                    //y軸に移動する
                    DirectionGoY(position, x, y);
                }
            }

        }
    }

    /// <summary>
    /// y軸方向の移動
    /// </summary>
    /// <param name="position"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void DirectionGoY(Vector3 position, float x, float y)
    {
        m_Y = false;
        if (m_moveSensitivity < y)
        {
            //行き先が壁か判断する
            if (m_mapStatus[(int)position.x, (int)position.y + 1] != AutoMappingV3.TileStatus.Wall)
            {
                Vector3 willPos = new Vector3(position.x, position.y + 1f, position.z);
                StartCor(willPos);
            }
            //壁だった場合
            else if (m_mapStatus[(int)position.x, (int)position.y + 1] == AutoMappingV3.TileStatus.Wall)
            {
                if (m_X)
                {
                    //x軸に移動する
                    DirectionGoX(position, x, y);
                }
            }
        }
        else if (y < -m_moveSensitivity)
        {
            //行き先が壁か判断する
            if (m_mapStatus[(int)position.x, (int)position.y - 1] != AutoMappingV3.TileStatus.Wall)
            {
                Vector3 willPos = new Vector3(position.x, position.y - 1f, position.z);
                StartCor(willPos);
            }
            //壁だった場合
            else if (m_mapStatus[(int)position.x, (int)position.y - 1] == AutoMappingV3.TileStatus.Wall)
            {
                if (m_X)
                {
                    //x軸に移動する
                    DirectionGoX(position, x, y);
                }
            }
        }
    }

    //MoveToをスタートさせるメソッド
    //外部からコルーチンを呼び出すときはこのメソッドを使う
    void StartCor(Vector3 goal)
    {
        if (m_myCor == null)
        {
            m_myCor = StartCoroutine(MoveTo(goal));
        }

    }

    //goalの位置までスムーズに移動する
    IEnumerator MoveTo(Vector3 goal)
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
