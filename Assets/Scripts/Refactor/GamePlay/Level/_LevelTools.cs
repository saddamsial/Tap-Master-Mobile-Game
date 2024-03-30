using Core.Extensions;
using Core.GamePlay;
using UnityEditor;
using UnityEngine;

namespace Core.Data{
    public class _LevelTools : MonoBehaviour{
        [SerializeField] private LevelDatas _levelSO;
        [SerializeField] private TextAsset _levelJson;
        [SerializeField] private int _targetLevel;

        public void ReadData(){
            Debug.Log(_levelJson.text);
            _levelSO.datasControllers.Clear();
            //LevelDatas LevelDatas = JsonUtility.FromJson<LevelDatas>((_levelJson.text));
            //_levelSO = LevelDatas;
            string[] res = _levelJson.text.Split( "-----------------------------------" , System.StringSplitOptions.RemoveEmptyEntries);
            foreach (var data in res){
                Debug.Log(data);
                LevelData levelData = JsonUtility.FromJson<LevelData>(data);
                _levelSO.datasControllers.Add(levelData);
            } 
            _levelSO.numberOfLevels = _levelSO.datasControllers.Count;

            #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(_levelSO);
            AssetDatabase.SaveAssets();
            #endif
        }

        public void GotoLevel(){
            if(_targetLevel < _levelSO.datasControllers.Count){
                Debug.Log("Goto Level: " + _targetLevel);
                _PlayerData.UserData.HighestLevel = _targetLevel;
                _GameManager.Instance.StartLevelByTool();
            }
            else{
                Debug.Log("Level not found");
            }
        }
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(_LevelTools))]
    public class _LevelToolsEditor : Editor{
        public override void OnInspectorGUI(){
            DrawDefaultInspector();
            var script = (Core.Data._LevelTools)target;
            if(GUILayout.Button("Read Data")){
                script.ReadData();
            }
            else if(GUILayout.Button("Goto Level")){
                script.GotoLevel();
            }
        }
    }
    #endif
}