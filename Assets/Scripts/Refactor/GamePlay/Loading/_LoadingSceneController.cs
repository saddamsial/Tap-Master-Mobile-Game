 using System.Collections;
using Core.Data;
using Core.ResourceGamePlay;
using Core.SystemGame;
using MyTools.ScreenSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class _LoadingSceneController : MonoBehaviour
{
    private bool _isLoadedManager = false;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad();
        StartCoroutine(LoadLoadingScreen());
        LoadDataUser();
    }

    private async void DontDestroyOnLoad()
    {
        var dontDestroyOnLoad = await AddressablesManager.LoadAssetAsync<GameObject>(_KeyPrefabResources.KeyDontDestroyOnLoad);
        var gameObject = GameObject.Instantiate(dontDestroyOnLoad);
        _isLoadedManager = true;
        Vibration.Init();
    }

    

    private IEnumerator LoadLoadingScreen()
    {
        yield return new WaitUntil(() => _isLoadedManager);
        _BeforeLoadManager.Instance.CameraUI.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Base;
        _ScreenManager.Instance.ShowScreen(_ScreenTypeEnum.Loading);
    }

    private void LoadDataUser(){
        _PlayerData.StartGame();
    }
}

