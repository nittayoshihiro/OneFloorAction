using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickButton : MonoBehaviour
{
    PlayerController m_playerController;

    public void OnClickPlayerAttack()
    {
        m_playerController = GameObject.FindObjectOfType<PlayerController>();
        if (m_playerController)
        {
            m_playerController.PlayerAttack();
        }
    }
}