using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace MRFramework
{
    /// <summary>
    /// UI动画组件
    /// </summary>
    public class UIAnimationComponent
    {
        // public AnimationClip animationClip;
        private Animator m_Animator;
        private Coroutine m_Coroutine;
        
        public void OnInit(GameObject root)
        {
            m_Animator = root.GetComponent<Animator>();

            // 动画事件帧
            // AnimationEvent animEvent = new AnimationEvent();
            // animEvent.functionName = "OnAnimationEvent";
            // animationClip.AddEvent(animEvent);
        }
        
        // 事件触发的方法
        public void OnAnimationEvent()
        {
            Log.Info("Animation event triggered!");
        }

        public void OnDestroy()
        {
            m_Animator = null;
            StopCurrentCoroutine();
        }

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="animName">动画名称</param>
        /// <param name="onComplete">动画结束回调</param>
        public void PlayAnimation(string animName, UnityAction onComplete = null)
        {
            if (m_Animator == null) return;
            
            m_Animator.Play(animName);
            if (onComplete != null)
            {
                m_Coroutine = GlobalManager.Instance.StartCoroutine(CheckAnimationComplete(animName, onComplete));
            }
        }
        
        /// <summary>
        /// 关闭正在播放的动画
        /// </summary>
        public void StopAnimation()
        {
            StopCurrentCoroutine();

            if (m_Animator != null)
            {
                m_Animator.StopPlayback();
            }
        }

        private void StopCurrentCoroutine()
        {
            if (m_Coroutine != null)
            {
                GlobalManager.Instance.StopCoroutine(m_Coroutine);
                m_Coroutine = null;
            }
        }
        
        private IEnumerator CheckAnimationComplete(string animName, UnityAction onComplete)
        {
            // 等待动画播放完毕
            while (true)
            {
                // 检查动画是否还在播放
                if (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                    break;
                
                yield return null;
            }
            
            onComplete?.Invoke();
        }
    }
}