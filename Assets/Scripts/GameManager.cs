using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>タイトルキャンバス</summary>
    [SerializeField] GameObject m_titleCanvas;
    [SerializeField] GameObject m_goalCanvas;
    [SerializeField] AutoMappingV3 m_autoMapping;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //スタートボタン
    public void PlayButton()
    {
        m_autoMapping.AutoMapping();
        m_titleCanvas.SetActive(false);
    }

    public void GoalEvent()
    {
        m_goalCanvas.SetActive(true);
    }

    public void HomeButton()
    {
        m_titleCanvas.SetActive(true);
        m_goalCanvas.SetActive(false);
    }

}
