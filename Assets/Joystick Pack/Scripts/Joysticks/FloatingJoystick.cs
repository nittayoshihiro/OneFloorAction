using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    PointerEventData m_upEventData;
    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!background.gameObject.activeSelf)
        {
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            background.gameObject.SetActive(true);
            base.OnPointerDown(eventData);
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
        m_upEventData = eventData;
        Invoke("JoistickFalse", 0.5f);
    }

    void JoistickFalse()
    {
        Debug.Log("JoistickFalse");
        background.gameObject.SetActive(false);
        base.OnPointerUp(m_upEventData);
    }
}