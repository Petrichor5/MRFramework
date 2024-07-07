using System;
using UnityEngine;

namespace MRFramework
{
    public class AudioPlayer : IPoolable, IPoolType
    {
        // AudioSource 组件，只有在调用 DestroyAudioSource 方法时才会被销毁
        private AudioSource audioSource;
        private AudioClip audioClip;
        private Action<AudioPlayer> onStartListener;
        private Action<AudioPlayer> onFinishListener;
        private TimeClock mTimeClock;

        private string name = string.Empty;
        private int playCount = 0; // 播放次数
        private float volume = 1;
        private bool isLoop = false;

        public AudioSource AudioSource { get => audioSource; }

        public string Name { get => name; }
        public int PlayCount { get => playCount; }
        public float Volume { get => volume; }
        public bool IsLoop { get => isLoop; }
        public bool IsRecycled { get; set; }

        public static AudioPlayer Allocate()
        {
            return SafeObjectPool<AudioPlayer>.Instance.Allocate();
        }

        public void Recycle2Cache()
        {
            if (!SafeObjectPool<AudioPlayer>.Instance.Recycle(this))
            {
                Debug.LogWarning("[AudioPlayer] 回收失败，对象池已满！！");
            }
        }

        public void OnRecycled()
        {
            CleanSettingData();
        }

        public void Setup(GameObject root, AudioClip clip, string name, bool isLoop = false, float volume = 1)
        {
            if (clip == null || this.name == name) return;

            if (audioSource == null)
            {
                audioSource = root.AddComponent<AudioSource>();
            }

            PauseAudioClip();
            CleanSettingData();

            this.name = name;
            this.isLoop = isLoop;
            this.volume = volume;
            this.audioClip = clip;
        }

        /// <summary>
        /// 播放音源
        /// </summary>
        public bool PlayAudioClip()
        {
            if (audioSource == null || audioClip == null) return false;

            audioSource.clip = audioClip;
            audioSource.loop = isLoop;
            audioSource.volume = volume;

            mTimeClock = AudioManager.Instance.DeployTimeClock(audioClip.length, OnSoundPlayFinish);

            if (onStartListener != null)
            {
                onStartListener(this);
            }

            audioSource.Play();
            return true;
        }

        /// <summary>
        /// 暂停播放音源
        /// </summary>
        public bool PauseAudioClip()
        {
            if (audioSource == null || audioClip == null) return false;
            
            mTimeClock.Pause();
            if(!mTimeClock.IsPause) return false;
            
            audioSource.Pause();
            return true;
        }

        /// <summary>
        /// 继续播放音源
        /// </summary>
        public bool ResumeAudioClip()
        {
            if (audioSource == null || audioClip == null) return false;

            if (mTimeClock.IsPause)
            {
                mTimeClock.Resume();
                if (mTimeClock.IsPause) return false;

                audioSource.Play();
                return true;
            }

            return false;
        }

        public void AddOnStartListener(Action<AudioPlayer> callback)
        {
            onStartListener += callback;
        }

        public void AddOnFinishListener(Action<AudioPlayer> callback)
        {
            onFinishListener += callback;
        }

        private void OnSoundPlayFinish()
        {
            playCount++;

            if (onFinishListener != null)
            {
                onFinishListener(this);
            }
        }

        /// <summary>
        /// 设置音量大小
        /// </summary>
        /// <param name="volume">大小：0 ~ 1</param>
        public void SetVolume(float volume)
        {
            this.volume = volume;
            if (audioSource != null)
            {
                audioSource.volume = volume;
            }
        }

        /// <summary>
        /// 设置是否循环
        /// </summary>
        public void SetLoop(bool isLoop)
        {
            this.isLoop = isLoop;
            if (audioSource != null)
            {
                audioSource.loop = isLoop;
            }
        }

        /// <summary>
        /// 销毁 AudioSource 组件
        /// </summary>
        public void DestroyAudioSource()
        {
            GameObject.Destroy(audioSource);
            audioSource = null;
        }

        /// <summary>
        /// 清理设置数据
        /// </summary>
        public void CleanSettingData()
        {
            name = null;
            playCount = 0;
            onStartListener = null;
            onFinishListener = null;

            mTimeClock = null;

            if (audioSource)
            {
                if (audioSource.clip == audioClip)
                {
                    audioSource.Stop();
                    audioSource.clip = null;
                }
            }

            audioClip = null;
        }
    }
}
