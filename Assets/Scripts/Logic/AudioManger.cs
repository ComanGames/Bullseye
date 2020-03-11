using Settings;

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

        }

    }
}