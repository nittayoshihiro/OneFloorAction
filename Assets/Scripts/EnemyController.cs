using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] int m_attack;
    [SerializeField] int m_hp;
    [SerializeField] int m_point;
    [SerializeField] GameObject m_sliderObj;
    [SerializeField] GameObject m_enemyDie;
    [SerializeField] AudioClip m_destroyAudio;
    Slider m_slider;
    TurnManager m_turnManager;
    AutoMappingV3.TileStatus[,] m_tileStatuses;
    Vector3 m_vector3;
    GameObject m_player;
    ScoreManager m_scoreManager;
    PlayerController m_playerController;
    //差の計算結果
    float x, y;
    bool m_move;
    Coroutine m_Cor;
    internal BaseState m_enemyState;

    void Start()
    {
        m_enemyState = new BaseState(m_hp, m_attack);
        m_tileStatuses = GameObject.FindObjectOfType<AutoMappingV3>().GetMappingData;
        m_turnManager = GameObject.FindObjectOfType<TurnManager>();
        m_turnManager.SetUpEnemy();
        m_player = GameObject.Find("Player(Clone)");
        m_playerController = m_player.GetComponent<PlayerController>();
        m_scoreManager = GameManager.FindObjectOfType<ScoreManager>();
        //スライダー処理
        m_slider = m_sliderObj.GetComponent<Slider>();
        m_slider.maxValue = m_hp;
        m_slider.value = m_hp;
        m_sliderObj.SetActive(false);
    }


    /// <summary>
    /// 敵ダメージ表示ようダメージ計算
    /// </summary>
    /// <param name="attack"></param>
    public void EnemyDamage(int attack)
    {
        m_scoreManager.AddScore(m_point);
        m_enemyState.DamageCalculation(attack);
        m_slider.value = m_enemyState.GetHp;
        if (!m_sliderObj.activeSelf)
        {
            m_sliderObj.SetActive(true);
        }
        //ダメージを受けてたら反撃
        else if (m_sliderObj.activeSelf && m_enemyState.GetHp > 0)
        {
            m_playerController.PlayerDamage(m_enemyState.GetAttack);
            //サウンドをつけたい
        }

    }

    void Update()
    {
        if (m_enemyState.GetHp < 0)
        {
            m_move = false;
            AudioSource.PlayClipAtPoint(m_destroyAudio,this.transform.position);
            Debug.Log("Destroy");
            m_turnManager.SetUpEnemy();
            m_scoreManager.AddScore(m_point);
            //Instantiate(m_enemyDie,this.transform.position,Quaternion.identity);
            Destroy(this.gameObject);
        }
        if (m_move)
        {
            EnemyMove();
        }
    }



    public virtual void EnemyMove()
    {
        //上下左右に移動でプレイヤーに近づくところを探す
        x = this.transform.position.x - m_player.transform.position.x;
        y = this.transform.position.y - m_player.transform.position.y;
        if (x > 0 && m_tileStatuses[(int)this.transform.position.x - 1, (int)this.transform.position.y] == AutoMappingV3.TileStatus.Road
            && new Vector3(this.gameObject.transform.position.x - 1, this.gameObject.transform.position.y, this.gameObject.transform.position.z) != m_player.transform.position)
        {
            m_vector3 = new Vector3(this.transform.position.x - 1, this.transform.position.y, this.transform.position.z);
            StartCor(m_vector3);
        }
        else if (x < 0 && m_tileStatuses[(int)this.transform.position.x + 1, (int)this.transform.position.y] == AutoMappingV3.TileStatus.Road
            && new Vector3(this.gameObject.transform.position.x + 1, this.gameObject.transform.position.y, this.gameObject.transform.position.z) != m_player.transform.position)
        {
            m_vector3 = new Vector3(this.transform.position.x + 1, this.transform.position.y, this.transform.position.z);
            StartCor(m_vector3);
        }
        else if (y > 0 && m_tileStatuses[(int)this.transform.position.x, (int)this.transform.position.y - 1] == AutoMappingV3.TileStatus.Road
            && new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - 1, this.gameObject.transform.position.z) != m_player.transform.position)
        {
            m_vector3 = new Vector3(this.transform.position.x, this.transform.position.y - 1, this.transform.position.z);
            StartCor(m_vector3);
        }
        else if (y < 0 && m_tileStatuses[(int)this.transform.position.x, (int)this.transform.position.y + 1] == AutoMappingV3.TileStatus.Road
            && new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1, this.gameObject.transform.position.z) != m_player.transform.position)
        {
            m_vector3 = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
            StartCor(m_vector3);
        }
        else
        {
            m_move = false;
        }

    }

    //MoveToをスタートさせるメソッド
    //外部からコルーチンを呼び出すときはこのメソッドを使う
    void StartCor(Vector3 goal)
    {
        if (m_Cor == null)
        {
            m_Cor = StartCoroutine(MoveTo(goal));
        }
    }

    //goalの位置までスムーズに移動する
    IEnumerator MoveTo(Vector3 goal)
    {
        while (Vector3.Distance(transform.position, goal) > 0.1f)
        {
            Vector3 nextPos = Vector3.Lerp(transform.position, goal, Time.deltaTime * 10f);
            transform.position = nextPos;
            //ここまでが1フレームの間に処理される
            yield return new WaitForSeconds(0.005f);
        }
        //ポジションを修正
        transform.position = goal;
        m_Cor = null;
        m_move = false;
        print("終了");
        //処理が終わったら破棄する
        yield break;
    }

    public bool Enemymove { get { return m_move; } }

    public void EnemyMoveOn()
    {
        m_move = true;
    }
}