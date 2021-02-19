using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurnManager : MonoBehaviour
{
    PlayerController m_PlayerController;
    EnemyController[] m_enemyController;
    GameObject[] m_enemys;
    TurnStatus m_TurnStatus = TurnStatus.Standby;
    
    void Update()
    {
        TurnCycle();
    }

    /// <summary>セットアップ関数</summary>
    public void SetUpPlayer()
    {
        m_PlayerController = FindObjectOfType<PlayerController>();
    }

    public void SetUpEnemy()
    {
        m_enemys = GameObject.FindGameObjectsWithTag("Enemy");
        if (m_enemys != null)
        {
            Array.Resize(ref m_enemyController, m_enemys.Length);
            for (int i = 0; i < m_enemyController.Length; i++)
            {
                m_enemyController[i] = m_enemys[i].GetComponent<EnemyController>();
            }
        }
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
                    if (m_enemys != null)
                    {
                        for (int i = 0; i < m_enemyController.Length; i++)
                        {
                            m_enemyController[i].EnemyMoveOn();
                        }
                    }
                    m_TurnStatus = TurnStatus.EnemyTurn;
                }
                break;
            case TurnStatus.EnemyTurn:
                if (m_enemys == null)
                {
                    m_TurnStatus = TurnStatus.Standby;
                }
                else if (m_enemys != null)
                {
                   if (!m_enemyController[0].Enemymove ||m_enemyController[0] == null)
                   {
                    m_TurnStatus = TurnStatus.Standby;
                   }
                }
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
