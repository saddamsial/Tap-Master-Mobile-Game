using UnityEngine;

namespace Core.SystemGame{
    public class _InputSystem{
        private static _InputSystem _instance;
        
        public static _InputSystem Instance { 
            get { 
                if (_instance == null)
                {
                    _instance = new _InputSystem();
                }
                return _instance;
            }
        }

        public float Timer { get; set; }

        public bool CheckSpread(){
            if((Input.touchCount == 2 && Input.GetTouch(0).phase != TouchPhase.Ended && Input.GetTouch(1).phase != TouchPhase.Ended) || Input.GetAxis("Mouse ScrollWheel") != 0){
                return true;
            }
            return false;
        }

        public bool CheckSelect(){
            if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended){
                return true;
            }
            return Input.GetMouseButtonUp(0);
        }

        public bool CheckSelectDown(){
            if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began){
                return true;
            }
            return Input.GetMouseButtonDown(0);
        }

        public bool CheckHold(){
            if(Input.GetMouseButton(0) || Input.touchCount == 1 && Input.GetTouch(0).phase != TouchPhase.Began){
                return true;
            }
            else{
                Timer = 0;
                return false;
            }
        }

        public bool GetLastPosStopFromSpread(out Vector3 pos){
            if(Input.touchCount == 2){
                if(Input.GetTouch(0).phase == TouchPhase.Ended){
                    pos = Input.GetTouch(1).position;
                    return true;
                }
                if(Input.GetTouch(1).phase == TouchPhase.Ended){
                    pos = Input.GetTouch(0).position;
                    return true;
                }
            }
            pos = Vector3.positiveInfinity;
            return false;
        }

        public Vector3 GetInputPositionInWorld(){
            if(Input.GetMouseButtonUp(0)){
                Vector3 inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //inputPos.z = -30;
                return inputPos;
            }

            if(Input.touchCount == 1){
                Vector3 inputPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                inputPos.z = -30;
                return inputPos;
            }

            return Vector3.positiveInfinity;
        }

        public float GetZoomValue(){
            if(Input.touchCount == 2){
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                return currentMagnitude - prevMagnitude;
            }
            else{
                return Input.GetAxis("Mouse ScrollWheel") * 10;
            }
        }
    }
}