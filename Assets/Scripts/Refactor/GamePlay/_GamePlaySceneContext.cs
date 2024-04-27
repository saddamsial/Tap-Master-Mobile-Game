using System.Threading.Tasks;
using Core.Data;
using Core.GamePlay.BlockPool;
using Core.GamePlay.Collection;
using Core.GamePlay.Shop;
using Core.ResourceGamePlay;
using Core.SystemGame;
using DG.Tweening;
using MyTools.ParticleSystem;
using MyTools.ScreenSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.GamePlay
{
    public class _GamePlaySceneContext : MonoBehaviour
    {
        [SerializeField] private LevelDatas _levelTest;
        [SerializeField] private _ShopElementDatas _shopDatas;
        [SerializeField] private _ItemPriceDatas _itemPriceDatas;
        [SerializeField] private _CollectionElementDatas _CollectionElementDatas;

        private GameObject _cameraGamePlay;

        private void Awake()
        {
            Init();
            //DontDestroyOnLoad();
        }

        // Start is called before the first frame update
        async void Start()
        {
            _ScreenManager.Instance.ShowScreen(_ScreenTypeEnum.GamePlay);
            _cameraGamePlay = await SetUpCamera();
            InitGame();
            _GameManager.Instance.StartLevel();
        }

        private void OnApplicationQuit()
        {
            //Debug.Log("OnApplicationQuit");
            _PlayerData.SaveUserData();
        }

        private void OnApplicationPause(bool pause)
        {
            //Debug.Log("OnApplicationPause");
            if (pause)
            {
                _PlayerData.SaveUserData();
            }
        }


        private void Init()
        {
            DOTween.Init();
            Application.targetFrameRate = 60;
        }

        private async Task<GameObject> SetUpCamera()
        {
            var cameraRotation = await AddressablesManager.LoadAssetAsync<GameObject>(_KeyPrefabResources.KeyCameraRotation);
            var cameraObject = (GameObject)Instantiate(cameraRotation);
#if UNITY_EDITOR
            gameObject.name = cameraRotation.Value.name;
#endif     
            return cameraObject;
        }

        private void InitGame()
        {
            _GameManager.Instance.InitGame(_levelTest, _cameraGamePlay.GetComponentInChildren<Camera>());
            _ParticleSystemManager.Instance.UICamera = _cameraGamePlay.GetComponentInChildren<Camera>();
            _GameManager.Instance.BlockElementDatas = _shopDatas;
            _GameManager.Instance.ItemPriceDatas = _itemPriceDatas;
            _GameManager.Instance.CollectionElementDatas = _CollectionElementDatas;
            // _GameManager.Instance.BlockPool = _blockPool;
            // //_LevelSystem.Instance.BlockPool = _blockPool;
            // _LevelSystem.Instance.InitLevelSystem(_levelTest);
        }
    }
}