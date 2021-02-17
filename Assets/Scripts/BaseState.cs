using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクター（敵やプレイヤー用）
/// </summary>
public class BaseState : MonoBehaviour
{
    private int m_hp;
    private int m_attack;

    /// <summary>
    /// ステータスを設定します
    /// </summary>
    /// <param name="hp">ヒットポイント</param>
    /// <param name="attack">攻撃力</param>
    public BaseState(int hp, int attack)
    {
        m_hp = hp;
        m_attack = attack;
    }

    /// <summary>
    /// 攻撃力を返します
    /// </summary>
    public int GetAttack { get { return m_attack; } }

    /// <summary>
    /// ヒットポイントを返します
    /// </summary>
    public int GetHp { get { return m_hp; } }

    /// <summary>
    /// ダメージ計算
    /// </summary>
    /// <param name="attack">攻撃力</param>
    public void DamageCalculation(int attack)
    {
        m_hp -= attack;
    }

    /// <summary>
    /// 回復
    /// </summary>
    /// <param name="recoveryPoint">回復量</param>
    public void Recovery(int recoveryPoint)
    {
        m_hp += recoveryPoint;
    }
}
