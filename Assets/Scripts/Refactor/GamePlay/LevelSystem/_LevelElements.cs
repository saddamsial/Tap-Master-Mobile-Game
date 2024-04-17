using TMPro;
using UnityEngine;

namespace Core.GamePlay.LevelSystem{
    public class _LevelElements{
        private const string _levelTextFormat = "LEVEL ";

        private Transform _levelItem;
        private TMP_Text _levelText;

        private int _currentLevel;

        public _LevelElements(Transform levelItem)
        {
            _levelText = levelItem.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        }

        public void SetLevel(int level){
            _currentLevel = level;
            _levelText.text = _levelTextFormat + _currentLevel;
            _levelItem.gameObject.SetActive(_currentLevel != -1);
        }

        public void OnClickLevelPlay(){
            Debug.Log("Play Level: " + _currentLevel);
        }
    }
}