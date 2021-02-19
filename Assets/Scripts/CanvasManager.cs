using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class CanvasManager : MonoBehaviour
{
    /// <summary>タイトルキャンバス</summary>
    [SerializeField] GameObject m_titleCanvas;
    [SerializeField] GameObject m_configurationCanvas;
    [SerializeField] GameObject m_moveSensitivitySlider;
    [SerializeField] GameObject m_scoreCanvas;
    [SerializeField] GameObject m_stickCanvas;
    [SerializeField] GameObject m_crossButtonCanvas;
    [SerializeField] GameObject m_goalCanvas;
    [SerializeField] AutoMappingV3 m_autoMapping;
    [SerializeField] EnemyGeneration m_enemyGeneration;
    [SerializeField] AudioClip m_titleBgm;
    [SerializeField] AudioClip m_gameBgm;
    ScoreManager m_scoreManager;
    AudioSource m_audioSource;
    float m_sensitiveity;

    private void Start()
    {
        m_scoreManager = GetComponent<ScoreManager>();
        m_audioSource = GetComponent<AudioSource>(); 
    }

    //感度シリンダー
    public void MoveSensitivitySlider()
    {
        Slider slider = m_moveSensitivitySlider.GetComponent<Slider>();
        m_sensitiveity = slider.value;
    }

    /// <summary>
    /// 動きの感度値
    /// </summary>
    public float MoveSensitivity
    {
        get { return m_sensitiveity; }
    }

    //プレイボタン
    public void PlayButton()
    {
        m_audioSource.clip = m_gameBgm;
        m_audioSource.Play();
        m_autoMapping.AutoMapping();
        m_enemyGeneration.EnemyGenerator();
        m_titleCanvas.SetActive(false);
        m_scoreCanvas.SetActive(true);
        
        if (true)
        {
            Instantiate(m_stickCanvas);
        }
        else if (false)
        {
            Instantiate(m_crossButtonCanvas);
        }
       
    }

    //設定ボタン
    public void ConfigurationButton()
    {
        m_configurationCanvas.SetActive(true);
        m_titleCanvas.SetActive(false);
    }

    //閉じるボタン
    public void CloseButton()
    {
        m_titleCanvas.SetActive(true);
        m_configurationCanvas.SetActive(false);
    }

    //ゴールイベント
    public void GoalEvent()
    {
        m_scoreCanvas.SetActive(false);
        m_goalCanvas.SetActive(true);
        if (true)
        {
            m_scoreManager.ResultText();
            Destroy(GameObject.Find("StickCanvas(Clone)"));
        }
        else if (false)
        {
            GameObject crossButtonCanvas = GameObject.Find("CrossButtonCanvas(Clone)");
            Destroy(crossButtonCanvas);
        }
        
    }

    //ホームボタン
    public void HomeButton()
    {
        m_titleCanvas.SetActive(true);
        m_goalCanvas.SetActive(false);
        m_audioSource.clip = m_titleBgm;
        m_audioSource.Play();
    }
}
