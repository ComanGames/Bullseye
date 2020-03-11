using System;
using UnityEngine;

namespace Settings{
    [Serializable]
    public class SoundSettings{
        public AudioSource _source;
        public AudioClip _clickSound;
        public AudioClip _arrowFlySound;
        public AudioClip _arrowHitSound;
        public AudioClip _gameScoreSound;
    

    }
}