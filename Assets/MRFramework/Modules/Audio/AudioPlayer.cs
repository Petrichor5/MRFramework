using System;
using UnityEngine;

namespace MRFramework
{
    public class AudioPlayer
    {
        private AudioSource m_AudioSource;  // AudioSource 组件，只有在调用 DestroyAudioSource 方法时才会被销毁
        private AudioClip m_AudioClip;      // 音源

        private Action<AudioPlayer> m_OnStartListener;  // 开始播放回调
        private Action<AudioPlayer> m_OnFinishListener; // 播放结束回调

        private int m_PlayCount = 0;        // 播放次数
        private float m_Volume = 1;         // 音量大小
        private bool m_IsLoop = false;      // 是否循环播放
        private bool m_IsPlaying = false;   // 是否在播放

        private long m_AudioPlayerID;       // 唯一ID
        private long m_TimerID;             // 计时器ID

        public AudioSource AudioSource => m_AudioSource;

        public int PlayCount => m_PlayCount;
        public float Volume => m_Volume;
        public bool IsLoop => m_IsLoop;
        public bool IsPlaying => m_IsPlaying;
        public long AudioPlayerID => m_AudioPlayerID;

        public AudioPlayer(long id)
        {
            m_AudioPlayerID = id;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(GameObject root, AudioClip clip, bool isLoop = false, float volume = 1)
        {
            if (clip == null)
            {
                Log.Error("[AudioPlayer] AudioClip 为空！");
                return;
            }

            PauseAudioClip();
            Clear();

            if (m_AudioSource == null)
            {
                m_AudioSource = root.AddComponent<AudioSource>();
            }

            this.m_IsLoop = isLoop;
            this.m_Volume = volume;
            this.m_AudioClip = clip;
        }

        /// <summary>
        /// 播放音源
        /// </summary>
        public void PlayAudioClip()
        {
            if (m_AudioSource == null || m_AudioClip == null) return;

            m_AudioSource.clip = m_AudioClip;
            m_AudioSource.loop = m_IsLoop;
            m_AudioSource.volume = m_Volume;

            m_TimerID = TimerManager.Instance.AddTimer(m_AudioClip.length, OnSoundPlayFinish);
            m_AudioSource.Play();
            m_IsPlaying = true;
            m_OnStartListener?.Invoke(this);
        }

        /// <summary>
        /// 暂停播放音源
        /// </summary>
        public void PauseAudioClip()
        {
            if (m_AudioSource == null || m_AudioClip == null) return;
            TimerManager.Instance.GetTimer(m_TimerID).Pause();
            m_AudioSource.Pause();
            m_IsPlaying = false;
        }

        /// <summary>
        /// 继续播放音源
        /// </summary>
        public void ResumeAudioClip()
        {
            if (m_AudioSource == null || m_AudioClip == null) return;
            TimerManager.Instance.GetTimer(m_TimerID).Resume();
            m_AudioSource.Play();
            m_IsPlaying = true;
        }

        public void AddOnStartListener(Action<AudioPlayer> callback)
        {
            m_OnStartListener += callback;
        }

        public void AddOnFinishListener(Action<AudioPlayer> callback)
        {
            m_OnFinishListener += callback;
        }

        private void OnSoundPlayFinish()
        {
            m_PlayCount++;
            m_IsPlaying = false;
            m_OnFinishListener?.Invoke(this);
        }

        /// <summary>
        /// 设置音量大小
        /// </summary>
        /// <param name="volume">大小：0 ~ 1</param>
        public void SetVolume(float volume)
        {
            this.m_Volume = volume;
            if (m_AudioSource != null)
            {
                m_AudioSource.volume = volume;
            }
        }

        /// <summary>
        /// 设置是否循环
        /// </summary>
        public void SetLoop(bool isLoop)
        {
            this.m_IsLoop = isLoop;
            if (m_AudioSource != null)
            {
                m_AudioSource.loop = isLoop;
            }
        }

        /// <summary>
        /// 销毁 AudioSource 组件
        /// </summary>
        public void DestroyAudioSource()
        {
            GameObject.Destroy(m_AudioSource);
            m_AudioSource = null;
        }

        /// <summary>
        /// 清理设置数据
        /// </summary>
        public void Clear()
        {
            m_AudioPlayerID = 0;
            m_PlayCount = 0;
            m_IsPlaying = false;
            m_OnStartListener = null;
            m_OnFinishListener = null;

            TimerManager.Instance.RemoveTimer(m_TimerID);
            m_TimerID = default;

            if (m_AudioSource)
            {
                if (m_AudioSource.clip == m_AudioClip)
                {
                    m_AudioSource.Stop();
                    m_AudioSource.clip = null;
                }
            }

            m_AudioClip = null;
        }
    }
}
