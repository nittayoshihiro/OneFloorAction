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
        float h = Input.GetAxisRaw("Horizontal");   // 垂直方向の入力を取得する
        float v = Input.GetAxisRaw("Vertical");     // 水平方向の入力を取得する
        if (0 < h)
        {
            this.gameObject.transform.position = new Vector3(pos.x + m_playerSpeed, pos.y, pos.z);
        }
        else if (h < 0)
        {
            this.gameObject.transform.position = new Vector3(pos.x - m_playerSpeed, pos.y, pos.z);
        }
        else if (0 < v)
        {
            this.gameObject.transform.position = new Vector3(pos.x, pos.y + m_playerSpeed, pos.z);
        }
        else if (v < 0)
        {
            this.gameObject.transform.position = new Vector3(pos.x, pos.y - m_playerSpeed, pos.z);
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
