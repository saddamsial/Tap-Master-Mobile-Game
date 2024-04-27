using System.Linq;
using Core.Data;
using Core.GamePlay.Shop;
using Core.SystemGame;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Core.GamePlay
{
    public class _CameraController : MonoBehaviour
    {
        [SerializeField] private float _sensitivity = 1.0f;
        [SerializeField] private float _damping = -1.0f;
        [SerializeField][Range(0.0f, 1.0f)] private float _inertia;
        [SerializeField] private Transform _cameraRotation;

        private Vector3 _remainingDelta;
        private Vector3 _lastRemainingDelta;
        private Vector3 _lastMousePosition;
        private float _zoomCameraValue;
        private float _lastZoomCameraValue;

        private Vector3 _maxSizeZoomCamera;
        private Vector3 _minSizeZoomCamera;
        //private bool _isZooming = false;

        private void Awake()
        {
            _GameEvent.OnGamePlayReset += SetUp;
            _GameEvent.OnSelectShopElement += SetBackgroundColor;
            var camera = this.GetComponent<Camera>().GetUniversalAdditionalCameraData().cameraStack; 
            _BeforeLoadManager.Instance.CameraUI.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Overlay;
            camera.Add(_BeforeLoadManager.Instance.CameraUI);
        }

        private void Start()
        {
            _GameManager.Instance.CameraController = this;
        }

        private void OnDestroy()
        {
            _GameEvent.OnGamePlayReset -= SetUp;
            _GameEvent.OnSelectShopElement -= SetBackgroundColor;
        }

        public void SetUp()
        {
            _cameraRotation.DORotate(new Vector3(-45, 90, 90), 0.5f);
            SetCameraSize();
            SetBackgroundColor(_PlayerData.UserData.RuntimeSelectedShopData[_ShopPage.Color], _ShopPage.Color);
        }

        private void LateUpdate()
        {
            if(!_GameManager.Instance.GamePlayManager.IsGameplayInteractable){
                _lastMousePosition = _lastRemainingDelta = _remainingDelta = Vector3.zero;
                return;
            } 
            if (_InputSystem.Instance.CheckSelectDown())
            {
                //_isZooming = false;
                _lastMousePosition = Input.mousePosition;
            }
            else if (_InputSystem.Instance.CheckHold())
            {
                Vector3 mouseDelta = Input.mousePosition - _lastMousePosition;
                _lastMousePosition = Input.mousePosition;
                _remainingDelta = mouseDelta * _sensitivity * Time.deltaTime;
            }

            if (_InputSystem.Instance.CheckSpread())
            {
                _remainingDelta = Vector3.zero;
                float zoomValue = _InputSystem.Instance.GetZoomValue();
                _zoomCameraValue = zoomValue * Time.deltaTime;
            }
             else if (_InputSystem.Instance.GetLastPosStopFromSpread(out Vector3 lastPos))
            {
                _lastMousePosition = lastPos;
            }

            Vector3 remainTmp = Vector3.Lerp(_lastRemainingDelta, _remainingDelta, _inertia);
            float remainZoomValue = Mathf.Lerp(_zoomCameraValue, 0, _inertia);
            _cameraRotation.Rotate(Vector3.left, remainTmp.y, Space.Self);
            _cameraRotation.Rotate(Vector3.up, remainTmp.x, Space.Self);
            ZoomCamera(remainZoomValue);

            if (_damping > 0.0f)
            {
                _remainingDelta = Vector3.Lerp(_remainingDelta, Vector3.zero, _damping);
                _zoomCameraValue = Mathf.Lerp(_zoomCameraValue, 0, _damping);
            }
            else
            {
                _remainingDelta = Vector3.zero;
                _zoomCameraValue = 0;
            }
            _lastRemainingDelta = _remainingDelta;
            _lastZoomCameraValue = _zoomCameraValue;
        }

        //Set Camera size by modify position.z and position.y of camera
        // private void SetCameraSize(){
        //     var cameraSize = _GameManager.Instance.CameraCanvas.aspect;
        //     if(cameraSize < 0.5 && cameraSize > 0.4)
        //         _GameManager.Instance.CameraCanvas.transform.position = _ConstantCameraSetting._9x21PositionSetting;
        //     if(cameraSize < 0.6 && cameraSize > 0.5)
        //         _GameManager.Instance.CameraCanvas.transform.position = _ConstantCameraSetting._9x16PositionSetting;
        //     Debug.Log(cameraSize);
        // }

        private void ZoomCamera(float zoomValue = 0)
        {
            if (zoomValue > 0)
            {
                if (this.transform.localPosition.z + Vector3.forward.z < _maxSizeZoomCamera.z)
                {
                    this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, _maxSizeZoomCamera, zoomValue);
                }
            }
            else if (zoomValue < 0)
            {
                if (this.transform.localPosition.z - Vector3.forward.z > _minSizeZoomCamera.z)
                    this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, _minSizeZoomCamera, -zoomValue);
            }
        }

        private void SetBackgroundColor(int index, _ShopPage type){
            if(type == _ShopPage.Color)
                this.GetComponent<Camera>().backgroundColor = _GameManager.Instance.BlockElementDatas.colorData.ElementAt(index).Value.backgroundColor;
        }

        private void SetCameraSize()
        {
            //Debug.Log(_GameManager.Instance.Level.size.x);
            //Debug.Log(_ConstantCameraSetting.GetCameraPositionValue((int)_GameManager.Instance.Level.size.x));
            int max = Mathf.Max((int)_GameManager.Instance.Level.size.x, (int)_GameManager.Instance.Level.size.y, (int)_GameManager.Instance.Level.size.z);
            (this.transform.localPosition, _maxSizeZoomCamera, _minSizeZoomCamera) = _ConstantCameraSetting.GetCameraPositionValue(max);
        }

        public float Sensitivity
        {
            get => _sensitivity;
            set => _sensitivity = value;
        }

        public float Damping
        {
            get => _damping;
            set => _damping = value;
        }

        public float Inertia
        {
            get => _inertia;
            set => _inertia = value;
        }

    }
}
