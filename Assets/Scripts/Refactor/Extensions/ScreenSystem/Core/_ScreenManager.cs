using System.Collections.Generic;
using UnityEngine;

namespace MyTools.ScreenSystem
{
    public class _ScreenManager : SingletonMonoBehaviour<_ScreenManager>
    {
        [SerializeField] private string _screenFolderPath;
        [SerializeField] private _BaseScreen[] _screens;
        [SerializeField] private Transform _screenCanvas;
        private Dictionary<_ScreenTypeEnum, _BaseScreen> _screenDict = new Dictionary<_ScreenTypeEnum, _BaseScreen>();
        private _ScreenTypeEnum _currentScreenType = _ScreenTypeEnum.None;

#if UNITY_EDITOR
        [ContextMenu("LoadPopupPrefabs")]
        void LoadPopupPrefabs()
        {
            var lstPrefabs = new List<_BaseScreen>();
            var lstNames = System.IO.Directory.GetFiles($"{_screenFolderPath}",
                "*.prefab", System.IO.SearchOption.AllDirectories);
            foreach (var itName in lstNames)
            {
                var obj = UnityEditor.AssetDatabase.LoadAssetAtPath<_BaseScreen>($"{itName}");
                if (obj == null) continue;
                lstPrefabs.Add(obj);
            }

            _screens = lstPrefabs.ToArray();
            UnityEditor.EditorUtility.SetDirty(gameObject);
        }
#endif

        public void PreLoad(){
            foreach(var screen in _screens){
                var go = Instantiate(screen.gameObject, Vector3.zero, Quaternion.identity, _screenCanvas);
                go.SetActive(false);
                _screenDict.Add(screen.ScreenType, go.GetComponent<_BaseScreen>());
                go.transform.localPosition = Vector3.zero;
            }
        }

        public void ShowScreen(_ScreenTypeEnum screenType){
            if(_currentScreenType == screenType) return;
            if(_screenDict.ContainsKey(screenType) == false){
                PreLoad();
            }
            if(_currentScreenType != _ScreenTypeEnum.None){
                _screenDict[_currentScreenType].Hide();
            }
            _currentScreenType = screenType;
            _screenDict[_currentScreenType].Show();
        }

        private void Update(){
            if(Input.GetKeyDown(KeyCode.A)){
                _screenDict[_currentScreenType].Hide();
            }
        }
    }
}