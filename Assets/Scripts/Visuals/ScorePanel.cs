using System.Collections;
using System.Text;
using UnityEngine;

namespace Visuals{
    public class ScorePanel : MonoBehaviour{

        public TextMesh Text;
        public float ScoreTimeot = .3f;
        public float ButtonTimeout = .3f;

        public IEnumerator ShowScore(int number){

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Your Score");
            Text.text = sb.ToString();
            yield return new WaitForSeconds(ScoreTimeot);
            sb.AppendLine(number.ToString());
            Text.text = sb.ToString();
            yield return new WaitForSeconds(ButtonTimeout);
        }
        public void Reset(){
            Text.text = "";
        }
    }
}