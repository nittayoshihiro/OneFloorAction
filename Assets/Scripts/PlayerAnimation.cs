using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator m_animatorm;
    private AnimaState m_animaState;
    private SpriteRenderer m_sprite;

    void Awake()
    {
        m_animatorm = GetComponent<Animator>();
        m_sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        switch (m_animaState)
        {
            case AnimaState.Up:
                m_animatorm.SetBool("Up", true);
                m_animatorm.SetBool("Down", false);
                m_animatorm.SetBool("Left", false);
                m_animatorm.SetBool("Right", false);
                break;
            case AnimaState.Down:
                m_animatorm.SetBool("Up", false);
                m_animatorm.SetBool("Down", true);
                m_animatorm.SetBool("Left", false);
                m_animatorm.SetBool("Right", false);
                break;
            case AnimaState.Left:
                m_animatorm.SetBool("Up", false);
                m_animatorm.SetBool("Down", false);
                m_animatorm.SetBool("Left", true);
                m_animatorm.SetBool("Right", false);
                m_sprite.flipX = false;
                break;
            case AnimaState.Right:
                m_animatorm.SetBool("Up", false);
                m_animatorm.SetBool("Down", false);
                m_animatorm.SetBool("Left", false);
                m_animatorm.SetBool("Right", true);
                m_sprite.flipX = true;
                break;
            default:
                break;
        }
        
    }

    public void ChangeAnimation(AnimaState animaState)
    {
        m_animaState = animaState;
    }

    public enum AnimaState
    {
        Up,
        Down,
        Left,
        Right,
        Done
    }
}
