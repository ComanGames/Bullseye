using Settings;
using UnityEngine;

namespace Logic{
    public class AudioManger{


        private SoundSettings _settings;

        public AudioManger(SoundSettings settings){
            _settings = settings;
        }

        public enum Sounds{
            Click,
            Fly,
            Bang,
            Congrats,
        }

        public void PlaySound(Sounds sound){
            AudioSource source = _settings._source;
            switch (sound){
                case Sounds.Click:
                    source.clip = _settings._clickSound;
                    break;
                case Sounds.Bang:
                    source.clip = _settings._arrowHitSound;
                    break;
                case Sounds.Congrats:
                    source.clip = _settings._gameScoreSound;
                    break;
                case Sounds.Fly:
                    source.clip = _settings._arrowFlySound;
                    break;

            }
            source.Play();

        }

    }
}