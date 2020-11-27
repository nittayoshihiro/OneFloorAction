using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    /// <summary>タイトルキャンバス</summary>
    [SerializeField] GameObject m_titleCanvas;
    [SerializeField] GameObject m_configurationCanvas;
    [SerializeField] GameObject m_moveSensitivitySlider;
    [SerializeField] GameObject m_stickCanvas;
    [SerializeField] GameObject m_crossButtonCanvas;
    [SerializeField] GameObject m_goalCanvas;
    [SerializeField] AutoMappingV3 m_autoMapping;
    float m_sensitiveity;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
        m_autoMapping.AutoMapping();
        m_titleCanvas.SetActive(false);
        if (false)
        {
            Instantiate(m_stickCanvas);
        }
        else if (true)
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
        m_goalCanvas.SetActive(true);
        if (false)
        {
            GameObject stickCanvas = GameObject.Find("StickCanvas(Clone)");
            Destroy(stickCanvas);
        }
        else if (true)
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
    }
}
