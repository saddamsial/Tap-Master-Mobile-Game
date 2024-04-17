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

        public _LevelElements(Transform levelItem)
        {
            _levelText = levelItem.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
            _levelItem = levelItem;
            _playButton = _levelItem.GetChild(1).GetComponent<Button>();
            _playButton.onClick.AddListener(OnClickLevelPlay);
            _lockMask = _levelItem.GetChild(2);
        }

        ~_LevelElements(){
            _playButton.onClick.RemoveListener(OnClickLevelPlay);
        }

        public void SetLevel(int level){
            _currentLevel = level;
            _levelText.text = _levelTextFormat + _currentLevel;
            _levelItem.gameObject.SetActive(_currentLevel != -1);
            SetInteractable(_currentLevel <= _PlayerData.UserData.HighestLevel + 1);
            if(_currentLevel == _ConstantGameplayConfig.LEVEL_EASY+1 || _currentLevel == _ConstantGameplayConfig.LEVEL_MEDIUM + _ConstantGameplayConfig.LEVEL_EASY + 1 || _currentLevel == 1){
                SetInteractable(true);
            }
        }

        public void OnClickLevelPlay(){
            Debug.Log("Play Level: " + _currentLevel);
        }

        private void SetInteractable(bool isInteractable){
            _playButton.interactable = isInteractable;
            _lockMask.gameObject.SetActive(!isInteractable);
        }
    }
}