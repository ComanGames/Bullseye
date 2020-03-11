using UnityEngine;

namespace Logic{
    public class WaitForPushable:CustomYieldInstruction{
        private IPushable _pushable;

        public WaitForPushable(IPushable pushable){
            _pushable = pushable;
        }

        public override bool keepWaiting{
            get {
                if (_pushable.IsPushed()){
                    Reset();
                    return false;
                }
                return true;
            }
        }
    }
}