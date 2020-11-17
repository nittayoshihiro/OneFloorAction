using Cinemachine;//Cinemachineを使うため
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float m_playerSpeed = 0.1f;
    public Coroutine myCor;

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
        float v = Input.GetAxisRaw("Vertical");     // 水平方向の入力を取得する
        float h = Input.GetAxisRaw("Horizontal");   // 垂直方向の入力を取得する

        Debug.Log(myCor);
        if (0 < v)
        {
            Vector3 willPos = new Vector3(pos.x, pos.y + 1f, pos.z);
            StartCor(willPos);
        }
        else if (v < 0)
        {
            Vector3 willPos = new Vector3(pos.x, pos.y - 1f, pos.z);
            StartCor(willPos);
        }
        else if (0 < h)
        {
            Vector3 willPos = new Vector3(pos.x + 1f, pos.y, pos.z);
            StartCor(willPos);
        }
        else if (h < 0)
        {
            Vector3 willPos = new Vector3(pos.x - 1f, pos.y, pos.z);
            StartCor(willPos);
        }


    }

    //MoveToをスタートさせるメソッド
    //外部からコルーチンを呼び出すときはこのメソッドを使う
    public void StartCor(Vector3 goal)
    {
        if (myCor == null)
        {
            myCor = StartCoroutine(MoveTo(goal));
        }

    }

    //goalの位置までスムーズに移動する
    public IEnumerator MoveTo(Vector3 goal)
    {
        while (Vector3.Distance(transform.position, goal) > 0.1f)
        {
            Vector3 nextPos = Vector3.Lerp(transform.position, goal, Time.deltaTime * m_playerSpeed);
            transform.position = nextPos;
            yield return null;//ここまでが1フレームの間に処理される
        }
        transform.position = goal;
        myCor = null;
        print("終了");
        yield break;//処理が終わったら破棄する
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
