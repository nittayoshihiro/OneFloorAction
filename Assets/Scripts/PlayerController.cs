using UnityEngine;
using Cinemachine;//Cinemachineを使うため

public class PlayerController : MonoBehaviour
{
    [SerializeField] float m_playerSpeed = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        //Virtual Camera がプレイヤーを見るように設定する
        CinemachineVirtualCamera vCam = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        if (vCam)
        {
            vCam.Follow = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = this.gameObject.transform.position;
        PlayerMove(pos);
    }

    /// <summary>プレイヤー移動</summary>
    void PlayerMove(Vector3 m_vector3)
    {
        float h = Input.GetAxisRaw("Horizontal");   // 垂直方向の入力を取得する
        float v = Input.GetAxisRaw("Vertical");     // 水平方向の入力を取得する
        if (0 < h)
        {
            this.gameObject.transform.position = new Vector3(m_vector3.x + m_playerSpeed, m_vector3.y, m_vector3.z);
        }
        else if (h < 0)
        {
            this.gameObject.transform.position = new Vector3(m_vector3.x - m_playerSpeed, m_vector3.y, m_vector3.z);
        }
        else if (0 < v)
        {
            this.gameObject.transform.position = new Vector3(m_vector3.x, m_vector3.y + m_playerSpeed, m_vector3.z);
        }
        else if (v < 0)
        {
            this.gameObject.transform.position = new Vector3(m_vector3.x, m_vector3.y - m_playerSpeed, m_vector3.z);
        }
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
