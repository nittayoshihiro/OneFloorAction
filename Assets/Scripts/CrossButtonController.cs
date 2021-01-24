using UnityEngine;

public class CrossButtonController : MonoBehaviour
{
    PlayerController m_playerController;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void GetPlayerController(PlayerController playerController)
    {
       GameObject playerGameObject = GameObject.Find("Player(Clone)");
       playerController =playerGameObject.GetComponent<PlayerController>();
    }

    public void UpDownButton(float Y)
    {
        GetPlayerController(m_playerController);
        m_playerController.ButtonMove(0, Y);
    }

    public void RightLeftButton(float X)
    {
        GetPlayerController(m_playerController);
        m_playerController.ButtonMove(X, 0);
    }
}
