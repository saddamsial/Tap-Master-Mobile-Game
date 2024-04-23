using Core.Data;
using Core.SystemGame;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.GamePlay.LevelSystem{
    public class _LevelElements{
        private const string _levelTextFormat = "LEVEL ";

        private Transform _levelItem;
        private TMP_Text _levelText;
        private Button _playButton;
        private int _currentLevel;
        private Transform _lockMask;
        private GameObject _selectedIcon;
        private Image _backgroundElement;

        public _LevelElements(Transform levelItem)
        {
            _levelText = levelItem.GetChild(0).GetComponent<TMP_Text>();
            _levelItem = levelItem;
            _playButton = _levelItem.GetComponent<Button>();
            _playButton.onClick.AddListener(OnClickLevelPlay);
            _lockMask = _levelItem.GetChild(1);
            _selectedIcon = _levelItem.GetChild(2).gameObject;
            _GameEvent.OnGamePlayReset += SetSelected;
            _backgroundElement = _levelItem.GetComponent<Image>();
        }

        ~_LevelElements(){
            _playButton.onClick.RemoveListener(OnClickLevelPlay);
            _GameEvent.OnGamePlayReset -= SetSelected;
        }

        public void SetLevel(int level){
            _currentLevel = level;
            _levelText.text = _levelTextFormat + _currentLevel;
            _levelItem.gameObject.SetActive(_currentLevel != -1);
            SetInteractable(_currentLevel <= _PlayerData.UserData.HighestLevelInMode[GetLevelType()]);
            SetSelected();
            if(_currentLevel == _ConstantGameplayConfig.LEVEL_EASY+1 || _currentLevel == _ConstantGameplayConfig.LEVEL_MEDIUM + _ConstantGameplayConfig.LEVEL_EASY + 1 || _currentLevel == 1){
                SetInteractable(true);
            }
        }

        public void SetBackgroundElement(Sprite sprite){
            _backgroundElement.sprite = sprite;
        }

        public void OnClickLevelPlay(){
            PopupSystem.PopupManager.Instance.CloseAllPopup();
            // _GameManager.Instance.GamePlayManager.IsGameplayInteractable = true;
            // Debug.Log("Play Level: " + _currentLevel);
            if(_currentLevel < _GameManager.Instance.LevelSystem.MaxLevelCount){
                // Debug.Log("Goto Level: " + _currentLevel);
                // _PlayerData.UserData.CurrentLevel = _currentLevel - 1;
                // _GameManager.Instance.BlockPool.DeSpawnAllBlocks();
                // _GameManager.Instance.StartLevel();
                //SetSelected(true);
                _GameManager.Instance.StartLevel(_currentLevel - 1);

            }
            else{
                Debug.Log("Level not found");
            }
        }


        private void SetSelected(){
            bool isSelectd = _currentLevel == _PlayerData.UserData.CurrentLevel + 1;
            _selectedIcon.SetActive(isSelectd);
        }

        private void SetInteractable(bool isInteractable){
            _playButton.interactable = isInteractable;
            _lockMask.gameObject.SetActive(!isInteractable);
        }

        private _LevelType GetLevelType(){
            if(_currentLevel <= _ConstantGameplayConfig.LEVEL_EASY){
                return _LevelType.Easy;
            }
            if(_currentLevel <= _ConstantGameplayConfig.LEVEL_EASY + _ConstantGameplayConfig.LEVEL_MEDIUM){
                return _LevelType.Medium;
            }
            return _LevelType.Master;
        }
    }
}