using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MRFramework
{
    [MonoSingletonPath("MRFramework/Audio")]
    public class AudioManager : MonoSingleton<AudioManager>
    {
        private List<TimeClock> mTimeClockList = new List<TimeClock>();
        
        private GameObject mSoundObject;
        private GameObject mMusicObject;
        private AudioPlayer mMusicPlayer;

        public override void OnSingletonInit()
        {
            SafeObjectPool<AudioPlayer>.Instance.Init(128, 10);
            SafeObjectPool<TimeClock>.Instance.Init(128, 10);

            if (mSoundObject == null)
            {
                mSoundObject = new GameObject("GameSound");
                mSoundObject.transform.SetParent(this.gameObject.transform, false);
            }

            if (mMusicObject == null)
            {
                mMusicObject = new GameObject("GameMusic");
                mMusicObject.transform.SetParent(this.gameObject.transform, false);
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            foreach (var item in mTimeClockList)
            {
                item.Clean();
            }
            mTimeClockList.Clear();
        }

        public void SetAudioPlayerPoolCount(int maxCacheCount)
        {
            SafeObjectPool<AudioPlayer>.Instance.MaxCacheCount = maxCacheCount;
        }
        
        public void SetATimeClockPoolCount(int maxCacheCount)
        {
            SafeObjectPool<TimeClock>.Instance.MaxCacheCount = maxCacheCount;
        }

        #region 背景音乐相关

        /// <summary>
        /// 播放背景音乐 (默认循环播放)
        /// </summary>
        public AudioPlayer PlayMusic(AudioClip clip, Action<AudioPlayer> callBack = null)
        {
            return PlayMusic(clip, true, 1f, callBack);
        }
        
        /// <summary>
        /// 播放背景音乐
        /// </summary>
        public AudioPlayer PlayMusic(AudioClip clip, bool isLoop, float volume = 1f,
            Action<AudioPlayer> callBack = null)
        {
            if (mMusicPlayer == null)
            {
                mMusicPlayer = AudioPlayer.Allocate();
            }
            
            var name = "music" + clip.GetHashCode();
            mMusicPlayer.Setup(mMusicObject, clip, name, isLoop, volume);
            
            mMusicPlayer.AddOnFinishListener((ap) =>
            {
                // 播放完 如果不是循环的 回收到对象池
                if (!ap.IsLoop)
                {
                    ap.Recycle2Cache();
                }

                callBack?.Invoke(ap);
            });

            if (!mMusicPlayer.PlayAudioClip())
            {
                // 播放失败
                mMusicPlayer.Recycle2Cache();
            }

            return mMusicPlayer;
        }

        /// <summary>
        /// 暂停播放背景音乐
        /// </summary>
        public void PauseMusic()
        {
            if (mMusicPlayer == null) return;
            mMusicPlayer.PauseAudioClip();
        }

        /// <summary>
        /// 重新播放背景音乐
        /// </summary>
        public void ResumeMusic()
        {
            if (mMusicPlayer == null) return;
            mMusicPlayer.ResumeAudioClip();
        }

        /// <summary>
        /// 设置背景音乐音量
        /// </summary>
        public void SetMusicVolume(float volume)
        {
            if (mMusicPlayer == null) return;
            mMusicPlayer.SetVolume(volume);
        }

        #endregion

        #region 音效相关
        
        /// <summary>
        /// 播放音效（默认不循环）
        /// </summary>
        public AudioPlayer PlaySound(AudioClip clip, Action<AudioPlayer> callBack = null)
        {
            return PlaySound(clip, false, 1f, callBack);
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        public AudioPlayer PlaySound(AudioClip clip, bool isLoop, float volume = 1f, 
            Action<AudioPlayer> callBack = null)
        {
            var audioPlayer = AudioPlayer.Allocate();
            
            var name = "sound" + clip.GetHashCode();
            audioPlayer.Setup(mSoundObject, clip, name, isLoop, volume);
            
            audioPlayer.AddOnFinishListener((ap) =>
            {
                // 播放完 如果不是循环的 回收到对象池
                if (!ap.IsLoop)
                {
                    ap.Recycle2Cache();
                }

                callBack?.Invoke(ap);
            });

            if (!audioPlayer.PlayAudioClip())
            {
                // 播放失败
                audioPlayer.Recycle2Cache();
            }

            return audioPlayer;
        }

        #endregion

        #region 内部实现

        private void Update()
        {
            for (int i = 0; i < mTimeClockList.Count; i++)
            {
                if (mTimeClockList[i] != null)
                {
                    // 计时完成 回收计时器
                    if (mTimeClockList[i].IsCompleted)
                    {
                        mTimeClockList[i].Recycle2Cache();
                        mTimeClockList.Remove(mTimeClockList[i]);
                    }
                    else
                    {
                        mTimeClockList[i].Update();
                    }
                }
            }
        }
        
        /// <summary>
        /// 部署计时器
        /// </summary>
        /// <param name="time">计时时间</param>
        /// <param name="onComplete">计时完成回调函数</param>
        /// <param name="ignorTimescale">是否忽略缩放影响</param>
        /// <param name="isAutoLoop">是否自动开始循环计时</param>
        /// <returns></returns>
        public TimeClock DeployTimeClock(float time, Action onComplete, bool ignorTimescale = false, bool isAutoLoop = false)
        {
            TimeClock timer = TimeClock.Allocate();
            timer.TimerSetup(time, onComplete, ignorTimescale, isAutoLoop);
            mTimeClockList.Add(timer);
            return timer;
        }
        
        public void RecycleTimeClock(Guid id)
        {
            var timer = mTimeClockList.FirstOrDefault(t => t.ID == id);
            
            if (timer != null)
            {
                timer.Recycle2Cache();
                mTimeClockList.Remove(timer);
            }
        }

        #endregion
    }
}