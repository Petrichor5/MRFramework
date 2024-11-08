using System;
using System.Collections.Generic;
using UnityEngine;

namespace MRFramework
{
    [MonoSingletonPath("MRFramework/AudioManager")]
    public class AudioManager : MonoSingleton<AudioManager>
    {
        // 音效相关
        private Dictionary<long, AudioPlayer> m_AudioPlayerDic;
        private SimpleObjectPool<AudioPlayer> m_AudioPlayerPool;
        private GameObject m_SoundObject;

        // 背景音乐
        private GameObject m_MusicObject;
        private AudioPlayer m_MusicPlayer;

        // Button点击音效
        private GameObject m_ButtonSoundObject;
        private AudioClip m_ButtonClickClip;
        private AudioSource m_ButtonClickSound;

        private long m_AudioPlayerID = 0;

        public override void OnSingletonInit()
        {
            // 音效相关初始化
            m_AudioPlayerDic = new Dictionary<long, AudioPlayer>();
            m_AudioPlayerPool = new SimpleObjectPool<AudioPlayer>(CreateAudioPlayer, ResetAudioPlayer);
            m_SoundObject = new GameObject("GameSound");
            m_SoundObject.transform.SetParent(this.gameObject.transform, false);

            // 背景音乐初始化
            m_MusicPlayer = new AudioPlayer(-999);
            m_MusicObject = new GameObject("GameMusic");
            m_MusicObject.transform.SetParent(this.gameObject.transform, false);

            // 加载Button音效
            m_ButtonClickClip = Resources.Load<AudioClip>("ButtonClick");
            m_ButtonSoundObject = new GameObject("ButtonSound");
            m_ButtonSoundObject.transform.SetParent(this.gameObject.transform, false);
            m_ButtonClickSound = m_ButtonSoundObject.AddComponent<AudioSource>();
        }

        private void OnDestroy()
        {
            m_AudioPlayerDic.Clear();
            m_AudioPlayerPool.ClearPool();
        }

        private AudioPlayer CreateAudioPlayer()
        {
            m_AudioPlayerID++;
            return new AudioPlayer(m_AudioPlayerID);
        }

        private void ResetAudioPlayer(AudioPlayer ap)
        {
            ap.Clear();
        }

        #region UGUI Pro

        public void PlayButtonClickSound()
        {
            m_ButtonClickSound.PlayOneShot(m_ButtonClickClip);
        }

        #endregion

        #region 背景音乐相关

        /// <summary>
        /// 播放背景音乐 (默认循环播放)
        /// </summary>
        public long PlayMusic(AudioClip clip, Action<AudioPlayer> onPlayEnd = null, Action<AudioPlayer> onPlayStart = null)
        {
            return PlayMusic(clip, true, 1f, onPlayEnd, onPlayStart);
        }
        
        /// <summary>
        /// 播放背景音乐
        /// </summary>
        public long PlayMusic(AudioClip clip, bool isLoop, float volume = 1f, Action<AudioPlayer> callBack = null, Action<AudioPlayer> onPlayStart = null)
        {
            m_MusicPlayer.Init(m_MusicObject, clip, isLoop, volume);
            m_MusicPlayer.AddOnStartListener(onPlayStart);
            m_MusicPlayer.AddOnFinishListener((ap) =>
            {
                if (!ap.IsLoop)
                {
                    // 背景音乐只有一个播放完，如果不是循环的只需要清空缓存
                    ap.Clear();
                }
                else
                {
                    // 再次播放
                    ap.PlayAudioClip();
                }
                callBack?.Invoke(ap);
            });

            // 播放音乐
            m_MusicPlayer.PlayAudioClip();
            if (!m_MusicPlayer.IsPlaying)
            {
                // 播放失败
                m_MusicPlayer.Clear();
                Log.Error("[AudioManager] 播放背景音乐失败！");
            }

            return m_MusicPlayer.AudioPlayerID;
        }

        /// <summary>
        /// 暂停播放背景音乐
        /// </summary>
        public void PauseMusic()
        {
            if (m_MusicPlayer == null) return;
            m_MusicPlayer.PauseAudioClip();
        }

        /// <summary>
        /// 重新播放背景音乐
        /// </summary>
        public void ResumeMusic()
        {
            if (m_MusicPlayer == null) return;
            m_MusicPlayer.ResumeAudioClip();
        }

        /// <summary>
        /// 设置背景音乐音量
        /// </summary>
        public void SetMusicVolume(float volume)
        {
            if (m_MusicPlayer == null) return;
            m_MusicPlayer.SetVolume(volume);
        }

        #endregion

        #region 音效相关
        
        /// <summary>
        /// 播放音效（默认不循环）
        /// </summary>
        public long PlaySound(AudioClip clip, Action<AudioPlayer> onPlayEnd = null, Action<AudioPlayer> onPlayStart = null)
        {
            return PlaySound(clip, false, 1f, onPlayEnd, onPlayStart);
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        public long PlaySound(AudioClip clip, bool isLoop, float volume = 1f, Action<AudioPlayer> onPlayEnd = null, Action<AudioPlayer> onPlayStart = null)
        {
            var audioPlayer = m_AudioPlayerPool.Allocate();
            audioPlayer.Init(m_SoundObject, clip, isLoop, volume);
            audioPlayer.AddOnStartListener(onPlayStart);
            audioPlayer.AddOnFinishListener((ap) =>
            {
                if (!ap.IsLoop)
                {
                    // 播放完 如果不是循环的 回收到对象池
                    m_AudioPlayerDic.Remove(audioPlayer.AudioPlayerID);
                    m_AudioPlayerPool.Recycle(audioPlayer);
                }
                else
                {
                    // 如果是循环的，再次播放
                    ap.PlayAudioClip();
                }
                onPlayEnd?.Invoke(ap);
            });

            // 播放音效
            audioPlayer.PlayAudioClip();
            if (!audioPlayer.IsPlaying)
            {
                // 播放失败
                m_AudioPlayerPool.Recycle(audioPlayer);
                Log.Error("[AudioManager] 播放音效失败！");
            }

            m_AudioPlayerDic.Add(audioPlayer.AudioPlayerID, audioPlayer);
            return audioPlayer.AudioPlayerID;
        }

        /// <summary>
        /// 获取音效播放器
        /// </summary>
        public AudioPlayer GetSoundPlayer(long id)
        {
            if (m_AudioPlayerDic.ContainsKey(id))
            {
                return m_AudioPlayerDic[id];
            }
            return null;
        }

        #endregion
    }
}