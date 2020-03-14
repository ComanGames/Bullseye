using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

namespace Visuals{
    public class ScorePanel : MonoBehaviour{

        public TextMeshPro Text;
        public Color HitColor;
        public Color MissColor;
        public float ScoreTimeot = .3f;
        public float ButtonTimeout = .3f;


        public IEnumerator ShowScore(int number){
            Show();
            Text.color = HitColor;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Score");
            sb.AppendLine(number.ToString());
            Text.text = sb.ToString();
            yield return new WaitForSeconds(ButtonTimeout);
            Hide();
        }

        public void Hide(){
            Text.gameObject.SetActive(false);
        }
        public void Show(){
            Text.gameObject.SetActive(true);
        }
        public void Reset(){
            Text.text = "";
        }

        public IEnumerator YouMissed(){
            Show();
            Text.color = MissColor;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("YOU");
            sb.AppendLine("MISS");
            Text.text = sb.ToString();
            yield return new WaitForSeconds(ButtonTimeout);
            Hide();

        }
    }
}