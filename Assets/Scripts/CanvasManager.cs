using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    /// <summary>タイトルキャンバス</summary>
    [SerializeField] GameObject m_titleCanvas;
    [SerializeField] GameObject m_configurationCanvas;
    [SerializeField] GameObject m_moveSensitivitySlider;
    [SerializeField] GameObject m_stickCanvas;
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
        m_stickCanvas.SetActive(true);
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
        //スティックのポジション初期化
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            m_stickCanvas.SetActive(false);
        }
    }

    //ホームボタン
    public void HomeButton()
    {
        m_titleCanvas.SetActive(true);
        m_goalCanvas.SetActive(false);
    }
}
