
using Cinemachine;//Cinemachineを使うため
using System.Collections;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] int m_attack;
    [SerializeField] int m_hp;
    [SerializeField] float m_playerSpeed = 0.1f;
    [SerializeField] AudioClip m_moveAudio;
    [SerializeField] AudioClip m_attackAudio;
    [SerializeField] AudioClip m_die;
    //操作感度
    public float m_moveSensitivity;
    //移動判定
    bool m_X, m_Y;
    Coroutine m_myCor;
    FloatingJoystick m_joystick;
    AutoMappingV3.TileStatus[,] m_mapStatus;
    CanvasManager m_canvasManager;
    Animator m_anim;
    AudioSource m_audioSource;
    BaseState m_PlayerState;
    ScoreManager m_scoreManager;
    Vector3 pos;
    List<GameObject> m_enemys = null;
    [SerializeField] Sprite[] m_sprites;
    bool m_move = false;
    private int m_moveCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindObjectOfType<ScoreManager>().Initialize();
        //Virtual Camera がプレイヤーを見るように設定する
        CinemachineVirtualCamera m_vCam = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        if (m_vCam)
        {
            m_vCam.Follow = transform;
        }
        m_anim = GetComponent<Animator>();
        m_audioSource = GetComponent<AudioSource>();
        m_scoreManager = GameObject.FindObjectOfType<ScoreManager>();
        m_PlayerState = new BaseState(m_hp, m_attack);
        m_scoreManager.PlayerLife(m_PlayerState.GetHp);
        m_canvasManager = GameObject.FindObjectOfType<CanvasManager>();
        m_joystick = GameObject.FindObjectOfType<FloatingJoystick>();
        AutoMappingV3 m_autoMapping = GameObject.FindObjectOfType<AutoMappingV3>();
        if (m_autoMapping)
        {
            m_mapStatus = m_autoMapping.GetMappingData;
        }
        m_moveSensitivity = 0.1f * m_canvasManager.MoveSensitivity;
        TurnManager turnManager = GameObject.FindObjectOfType<TurnManager>();
        turnManager.SetUpPlayer();
        m_enemys = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        m_enemys.ForEach(e => Debug.Log(e));
    }

    void Update()
    {
        if (m_PlayerState.GetHp <= 0)
        {
            EnemyClear();
            AudioSource.PlayClipAtPoint(m_die,this.transform.position);
            m_canvasManager.GoalEvent();
            Destroy(this.gameObject);
        }
        if (m_move)
        {
            m_X = true;
            m_Y = true;
            pos = this.gameObject.transform.position;
            // float y = Input.GetAxisRaw("Vertical");     // 水平方向の入力を取得する
            // float x = Input.GetAxisRaw("Horizontal");   // 垂直方向の入力を取得する

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
            m_scoreManager.MovePoint(m_moveCount);
            EnemyClear();
            m_canvasManager.GoalEvent();
            Destroy(this.gameObject);
        }

    }

    private void EnemyClear()
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemys != null)
        {
            foreach (var item in enemys)
            {
                Destroy(item);
            }
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
                this.transform.localScale = new Vector3(-8, 8, 1);
                m_anim.Play("PlayerLeft");
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
                this.transform.localScale = new Vector3(8,8,1);
                m_anim.Play("PlayerLeft");
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
                m_anim.Play("PlayerUp");
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
                m_anim.Play("PlayerDown");
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
        if (EnemyPosCheck(goal))
        {
            if (m_myCor == null)
            {
                m_moveCount++;
                m_myCor = StartCoroutine(MoveTo(goal));
            }
        }
    }

    /// <summary>
    /// 行き先に敵がいないか
    /// </summary>
    /// <returns></returns>
    private bool EnemyPosCheck(Vector3 goal)
    {
        Debug.Log(m_enemys.Count);
        m_enemys = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        if (m_enemys.Count != 0)
        {
            foreach (var item in m_enemys)
            {
                if (item != null)
                {
                    if (item.transform.position == goal)
                    {
                        return false;
                    }
                }
                
            }
        }

        return true;
    }

    //goalの位置までスムーズに移動する
    IEnumerator MoveTo(Vector3 goal)
    {
        m_audioSource.PlayOneShot(m_moveAudio);
        while (Vector3.Distance(transform.position, goal) > 0.1f)
        {
            Vector3 nextPos = Vector3.Lerp(transform.position, goal, Time.deltaTime * m_playerSpeed);
            transform.position = nextPos;
            //ここまでが1フレームの間に処理される
            yield return new WaitForSeconds(0.005f);
        }
        //ポジションを修正
        transform.position = goal;
        m_myCor = null;
        m_move = false;
        print("終了");
        //処理が終わったら破棄する
        yield break;
    }

    public Vector3 PlayPos
    {
        get { return transform.position; }
    }

    public float PlayerSpeed
    {
        get { return m_playerSpeed; }
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

   　/// <summary>
    /// プレイヤー攻撃
    /// </summary>
    public void PlayerAttack()
    {
        m_audioSource.PlayOneShot(m_attackAudio);
        List<EnemyController> enemyController = new List<EnemyController>();
        SearchGameObject().ForEach(enemy => enemyController.Add(enemy.GetComponent("EnemyController") as EnemyController));
        enemyController.ForEach(enemyCol => enemyCol.EnemyDamage(m_PlayerState.GetAttack));
        m_moveCount++;
        m_move = false;
    }

    /// <summary>
    /// プレイヤーの受けるダメージ
    /// </summary>
    /// <param name="attack"></param>
    public void PlayerDamage(int attack)
    {
        m_PlayerState.DamageCalculation(attack);
        m_scoreManager.PlayerLife(m_PlayerState.GetHp);
    }

    /// <summary>
    /// 周囲のオブジェクトを検索します。
    /// </summary>
    private List<GameObject> SearchGameObject()
    {
        Debug.Log("検索");
        m_enemys = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        List<GameObject> nextToEnemy = m_enemys;
        for (int i = 0; i < nextToEnemy.Count;)
        {
            if (this.transform.position == new Vector3(m_enemys[i].transform.position.x + 1, m_enemys[i].transform.position.y, 0)
                || this.transform.position == new Vector3(m_enemys[i].transform.position.x - 1, m_enemys[i].transform.position.y, 0)
                || this.transform.position == new Vector3(m_enemys[i].transform.position.x, m_enemys[i].transform.position.y + 1, 0)
                || this.transform.position == new Vector3(m_enemys[i].transform.position.x, m_enemys[i].transform.position.y - 1, 0))
            {
                i++;
            }
            else
            {
                nextToEnemy.RemoveRange(i, 1);
            }
        }
        return nextToEnemy;
    }

    public void MoveOn()
    {
        m_move = true;
    }

   　/// <summary>動いているかどうか</summary>
    public bool MoveNow => m_move;
}