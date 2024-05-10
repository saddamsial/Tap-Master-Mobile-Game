using Core.Data;
using Core.GamePlay;
using MyTools.Generic;
using PopupSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.ExtendPopup{
    public class _SettingPopup : BasePopup{
        [Header("Popup Elements")]
        [SerializeField] private Transform _soundSettingButtonRoot;
        [SerializeField] private Transform _musicSettingButtonRoot;
        [SerializeField] private Transform _vibrationSettingButtonRoot;

        private TwoStateElement _soundSettingButton;
        private TwoStateElement _musicSettingButton;
        private TwoStateElement _vibrationSettingButton;
        private bool _isInit = false;

        public void Show(){
            base.Show();
            _GameManager.Instance.GamePlayManager.IsGameplayInteractable = false;
            InitButton();
            ShowSettingState();
        }

        public void OnClickClose(){
            _MySoundManager.Instance.PlaySound(_SoundType.ClickUIButton);
            base.Hide(
                () => {
                    _GameManager.Instance.GamePlayManager.IsGameplayInteractable = true;
                }
            );
        }

        public override void OnDestroy(){
            base.OnDestroy();
            _soundSettingButtonRoot.GetComponent<Button>().onClick.RemoveAllListeners();
            _musicSettingButtonRoot.GetComponent<Button>().onClick.RemoveAllListeners();
            _vibrationSettingButtonRoot.GetComponent<Button>().onClick.RemoveAllListeners();
        }

        private void InitButton(){
            if(_isInit) return;
            _soundSettingButton = new TwoStateElement(_soundSettingButtonRoot);
            _musicSettingButton = new TwoStateElement(_musicSettingButtonRoot);
            _vibrationSettingButton = new TwoStateElement(_vibrationSettingButtonRoot);
            
            _soundSettingButtonRoot.GetComponent<Button>().onClick.AddListener( SetStateSoundButton);
            _musicSettingButtonRoot.GetComponent<Button>().onClick.AddListener( SetStateMusicButton);
            _vibrationSettingButtonRoot.GetComponent<Button>().onClick.AddListener( SetStateVibrationButton);

            _isInit = true;
        }

        private void ShowSettingState(){
            _soundSettingButton.SetState(_PlayerData.UserData.IsTurnOnSound);
            _musicSettingButton.SetState(_PlayerData.UserData.IsTurnOnMusic);
            _vibrationSettingButton.SetState(_PlayerData.UserData.IsTurnOnVibration);
        }

        private void SetStateSoundButton(){
            SetStateButton(SettingType.Sound);
        }

        private void SetStateMusicButton(){
            SetStateButton(SettingType.Music);
        }

        private void SetStateVibrationButton(){
            SetStateButton(SettingType.Vibration);
        }

        private void SetStateButton(SettingType type){
            switch (type){
                case SettingType.Sound:
                    bool isTurnOnSound = _PlayerData.UserData.IsTurnOnSound;
                    _PlayerData.UserData.IsTurnOnSound = !isTurnOnSound;
                    Debug.Log("Sound: " + _PlayerData.UserData.IsTurnOnSound);
                    _soundSettingButton.SetState(!isTurnOnSound);
                    break;
                case SettingType.Music:
                    bool isTurnOnMusic = _PlayerData.UserData.IsTurnOnMusic;
                    _PlayerData.UserData.IsTurnOnMusic = !isTurnOnMusic;
                    _MySoundManager.Instance.StopMusic();
                    _musicSettingButton.SetState(!isTurnOnMusic);
                    break;
                case SettingType.Vibration:
                    bool isTurnOnVibration = _PlayerData.UserData.IsTurnOnVibration;
                    _PlayerData.UserData.IsTurnOnVibration = !isTurnOnVibration;
                    _vibrationSettingButton.SetState(!isTurnOnVibration);
                    break;
            }
        }
    }

    public enum SettingType{
        Sound,
        Music,
        Vibration
    }
}