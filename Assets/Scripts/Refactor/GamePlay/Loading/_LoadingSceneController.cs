using System.Collections;
using Core.Data;
using Core.ResourceGamePlay;
using MyTools.ScreenSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class _LoadingSceneController : MonoBehaviour
{


    private bool _isLoadedManager = false;
    private bool _isLoadedAsset = false;

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
    }

    

    private IEnumerator LoadLoadingScreen()
    {
        yield return new WaitUntil(() => _isLoadedManager);
        _ScreenManager.Instance.ShowScreen(_ScreenTypeEnum.Loading);
    }

    private void LoadDataUser(){
        _PlayerData.StartGame();
    }
}

