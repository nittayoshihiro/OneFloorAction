using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    PlayerController m_PlayerController;
    TurnStatus m_TurnStatus = TurnStatus.Standby;
    
    void Update()
    {
        TurnCycle();
    }

    /// <summary>セットアップ関数</summary>
    public void SetUp()
    {
        m_PlayerController = FindObjectOfType <PlayerController> ();
    }

    /// <summary>ターンサイクル</summary>
    void TurnCycle()
    {
        switch (m_TurnStatus)
        {
            case TurnStatus.Standby:
                //プレイヤーが存在するとき（playerControllerがある時）
                if (m_PlayerController)
                {
                    m_PlayerController.MoveOn();
                    m_TurnStatus = TurnStatus.PlayerTurn;
                }
                break;
            case TurnStatus.PlayerTurn:
                //プレイヤーが移動し終わったら
                if (!m_PlayerController.MoveNow)
                {
                    m_TurnStatus = TurnStatus.EnemyTurn;
                }
                break;
            case TurnStatus.EnemyTurn:
                m_TurnStatus = TurnStatus.Standby;
                break;
        }
       
        
    }

    enum TurnStatus
    {
        Standby,
        PlayerTurn,
        EnemyTurn
    }
}
