using Cinemachine;//Cinemachineを使うため
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float m_playerSpeed = 0.01f;
    /// <summary>プレイヤーの前ポジション</summary>
    Vector3 m_oldPlayerPosition;
    /// <summary>ゴールポジション</summary>
    Vector3 m_goalPosition;
    /// <summary>移動できるか</summary>
    bool m_playerMoveNow;

    // Start is called before the first frame update
    void Start()
    {
        //Virtual Camera がプレイヤーを見るように設定する
        CinemachineVirtualCamera vCam = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        if (vCam)
        {
            vCam.Follow = transform;
        }
        m_playerMoveNow = true;
        //ゴールポジションを取得します
        AutoMappingV3 m_autoMappingV3 = GameObject.FindObjectOfType<AutoMappingV3>();
        m_goalPosition = m_autoMappingV3.GetGoalPosition;
        Debug.Log(m_goalPosition);
        //ポジションを残します
        m_oldPlayerPosition = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = this.gameObject.transform.position;
        if (m_playerMoveNow)
        {
            PlayerMove(pos);
        }
        //ゴール判定
        if (m_oldPlayerPosition.x <= m_goalPosition.x + 0.5f && m_goalPosition.x - 0.5f <= pos.x && m_oldPlayerPosition.y <= m_goalPosition.y + 0.5f && m_goalPosition.y - 0.5f <= pos.y)
        {
            Debug.Log("Destroy");
            Destroy(this.gameObject);
        }
        m_oldPlayerPosition = this.gameObject.transform.position;
    }

    /// <summary>プレイヤー移動</summary>
    void PlayerMove(Vector3 Vector3)
    {
        m_playerMoveNow = false;
        float h = Input.GetAxisRaw("Horizontal");   // 垂直方向の入力を取得する
        float v = Input.GetAxisRaw("Vertical");     // 水平方向の入力を取得する
        if (0 < h)
        {
            this.gameObject.transform.position = new Vector3(Vector3.x + m_playerSpeed, Vector3.y, Vector3.z);
        }
        else if (h < 0)
        {
            this.gameObject.transform.position = new Vector3(Vector3.x - m_playerSpeed, Vector3.y, Vector3.z);
        }
        else if (0 < v)
        {
            this.gameObject.transform.position = new Vector3(Vector3.x, Vector3.y + m_playerSpeed, Vector3.z);
        }
        else if (v < 0)
        {
            this.gameObject.transform.position = new Vector3(Vector3.x, Vector3.y - m_playerSpeed, Vector3.z);
        }
        m_playerMoveNow = true;
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
