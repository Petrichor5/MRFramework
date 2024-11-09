using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MRFramework.UGUIPro
{
    [Serializable]
    public class ButtonClickEvent : UnityEvent
    {
        public ButtonClickEvent() { }
    }
    
    [Serializable]
    public class ButtonProBase : Button, IUpdateSelectedHandler
    {
        [SerializeField] private ButtonDoubleClickExtend m_ButtonDoubleClickExtend = new ButtonDoubleClickExtend();
        [SerializeField] private ButtonLongPressExtend m_ButtonLongPressExtend = new ButtonLongPressExtend();
        [SerializeField] private ButtonClickEvent m_ButtonClickEvent = new ButtonClickEvent();
        [SerializeField] private ButtonPressScaleExtend m_ButtonScaleExtend = new ButtonPressScaleExtend();
        [SerializeField] private ButtonAudioExtend m_ButtonAudioExtend = new ButtonAudioExtend();

        private Vector2 m_PressPos;
        private bool m_IsPreass;
        private PointerEventData m_PointerEventData;
        public Action OnPointerUpListener;

        public override void OnPointerClick(PointerEventData eventData)
        {
            OnPointerClick();
        }

        public void OnPointerClick()
        {
            if (m_ButtonAudioExtend != null)
            {
                if (m_ButtonAudioExtend.OnButtonClick() && interactable)
                {
                    onClick?.Invoke();
                }
            }
            else
            {
                if (interactable)
                    onClick?.Invoke();
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            m_PressPos = eventData.position;
            m_IsPreass = true;
            m_PointerEventData = eventData;
            m_ButtonLongPressExtend?.OnPointerDown();
            m_ButtonDoubleClickExtend?.OnPointerDown();
            m_ButtonScaleExtend?.OnPointerDown(transform, interactable);
            m_ButtonAudioExtend.OnPointerDown(transform);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            m_PointerEventData = null;
            m_IsPreass = false;
            //判断手指按下时移动的范围，超过一定的范围后不触发按钮事件，如手指移动出按钮所在区域
            if (interactable && Mathf.Abs(Vector2.Distance(m_PressPos, eventData.position)) < 10)
            {
                m_ButtonClickEvent?.Invoke();
                m_ButtonAudioExtend.OnPointerUp(this);
            }

            OnPointerUpListener?.Invoke();
            m_ButtonScaleExtend?.OnPointerUp(transform, interactable);
            EventSystem.current.SetSelectedGameObject(null);
        }

        public void OnUpdateSelected(BaseEventData eventData)
        {
            m_ButtonLongPressExtend?.OnUpdateSelected();
        }

        public void AddButtonLongListener(UnityAction callback, float duration)
        {
            m_ButtonLongPressExtend.AddListener(callback, duration);
        }

        public void AddButtonDoubleClickListener(UnityAction callback, float clickInterval)
        {
            m_ButtonDoubleClickExtend.AddListener(callback, clickInterval);
        }

        public void AddButtonClick(UnityAction callback)
        {
            //....
            m_ButtonClickEvent.AddListener(callback);
        }

        public void RemoveButtonClick(UnityAction callback)
        {
            m_ButtonClickEvent.RemoveListener(callback);
        }

        public void OnApplicationFocus(bool focus)
        {
            if (focus == false)
            {
                // Log.InfoGreen("OnApplicationFocus   mIsPreass:" + m_IsPreass + " mPointerEventData:" + m_PointerEventData);
                if (m_IsPreass && m_PointerEventData != null)
                {
                    OnPointerUp(m_PointerEventData);
                }
            }
        }
        
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (m_ButtonScaleExtend.UsePressScale)
            {
                transition = Transition.None;
            }
        }
#endif
    }
}