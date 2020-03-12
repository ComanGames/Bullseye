using System;
using Settings;
using TMPro;
using UnityEngine;

namespace Logic{
    [RequireComponent(typeof(BoxCollider))]
    public class InputButton:MonoBehaviour,IPushable{

        [SerializeField] private LayerMask _hitMask;
        [SerializeField] private Material _colorMat;
        [SerializeField] private TextMeshPro _textMesh;

        public event Action OnPushed;

        private BoxCollider _collider;
        private bool _isPushed;

        public void Awake(){
            _collider = GetComponent<BoxCollider>();
            _isPushed = false;
        }

        private void Update(){
            if (Input.GetMouseButtonDown(0)){
                RaycastHit hit;  
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast( ray,out hit, 100f,_hitMask)){
                    if (hit.collider != null
                        && hit.collider == _collider){
                        _isPushed = true;
                        if (OnPushed != null) 
                            OnPushed.Invoke();
                    }

                }
            }
        }

        public bool IsPushed(){
            return _isPushed;
        }

        public void Reset(){
            _isPushed = false;
        }

        public void ChangeState(ButtonVisuals state){

            _textMesh.text = state.Text;
            _colorMat.color = state.Color;

        }

        public static void Init(){

        }
    }
}