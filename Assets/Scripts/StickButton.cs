using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スティックボタンクラス
/// </summary>
public class StickButton : MonoBehaviour
{
    /// <summary>プレイヤーコントローラー</summary>
    PlayerController m_playerController;

    /// <summary>プレイヤーアタック（クリック）</summary>
    public void OnClickPlayerAttack()
    {
        m_playerController = GameObject.FindObjectOfType<PlayerController>();
        if (m_playerController)
        {
            m_playerController.PlayerAttack();
        }
    }
}