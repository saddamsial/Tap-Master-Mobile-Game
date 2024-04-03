using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTools.ScreenSystem
{
    public class _ScreenManager : SingletonMonoBehaviour<_ScreenManager>
    {
        [SerializeField] private string _screenFolderPath;
        [SerializeField] private _BaseScreen[] _screens;
        private Dictionary<_ScreenTypeEnum, _BaseScreen> _screenDict = new Dictionary<_ScreenTypeEnum, _BaseScreen>();
        private _ScreenTypeEnum _currentScreenType = _ScreenTypeEnum.None;


        public void PreLoad(){
            foreach(var screen in _screens){
                var go = Instantiate(screen.gameObject);
                go.SetActive(false);
                _screenDict.Add(screen.ScreenType, go.GetComponent<_BaseScreen>());
            }
        }

        public void ShowScreen(_ScreenTypeEnum screenType){
            if(_currentScreenType != _ScreenTypeEnum.None){
                _screenDict[_currentScreenType].gameObject.SetActive(false);
            }
            _currentScreenType = screenType;
            _screenDict[_currentScreenType].gameObject.SetActive(true);
        }
    }
}