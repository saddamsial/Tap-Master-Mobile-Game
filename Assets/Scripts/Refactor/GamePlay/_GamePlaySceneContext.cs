using Core.Data;
using Core.GamePlay.BlockPool;
using Core.ResourceGamePlay;
using Core.SystemGame;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.GamePlay
{
    public class _GamePlaySceneContext : MonoBehaviour
    {
        [SerializeField] private LevelDatas _levelTest;

        private _BlockPool _blockPool;

        private void Awake()
        {
            Init();
            //DontDestroyOnLoad();
        }

        // Start is called before the first frame update
        void Start()
        {
            SetUpCamera();
            InitGame();
            _PlayerData.StartGame();
            _GameManager.Instance.StartLevel();
        }

        private void OnApplicationQuit(){
            Debug.Log("OnApplicationQuit");
            _PlayerData.SaveUserData();
        }

        

        private void Init()
        {
            DOTween.Init();
            Application.targetFrameRate = 60;
        }

        private async void SetUpCamera()
        {
            var cameraRotation = await AddressablesManager.LoadAssetAsync<GameObject>(_KeyPrefabResources.KeyCameraRotation);
            var gameObject = GameObject.Instantiate(cameraRotation);
#if UNITY_EDITOR
            gameObject.name = cameraRotation.Value.name;
#endif     
        }

        private void InitGame()
        {
            _GameManager.Instance.InitGame(_levelTest);
            // _GameManager.Instance.BlockPool = _blockPool;
            // //_LevelSystem.Instance.BlockPool = _blockPool;
            // _LevelSystem.Instance.InitLevelSystem(_levelTest);
        }
    }
}