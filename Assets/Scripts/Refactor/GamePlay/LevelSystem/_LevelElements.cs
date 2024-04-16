using TMPro;
using UnityEngine;

namespace Core.GamePlay.LevelSystem{
    public class _LevelElements: MonoBehaviour{
        private const string _levelTextFormat = "LEVEL ";

        [Header("Elements")]
        [SerializeField] private TMP_Text _levelText;
        private int _currentLevel;

        public void SetLevel(int level){
            _currentLevel = level;
            _levelText.text = _levelTextFormat + _currentLevel;
        }

        public void OnClickLevelPlay(){
            Debug.Log("Play Level: " + _currentLevel);
        }
    }
}