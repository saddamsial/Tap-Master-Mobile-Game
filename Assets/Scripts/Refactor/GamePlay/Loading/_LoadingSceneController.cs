using System.Collections;
using Core.ResourceGamePlay;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class _LoadingSceneController : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text _loadingText;
    [SerializeField] private int _percent = 100;
    [SerializeField] private Slider _loadingSlider;
    [SerializeField] private float _delayTime = 0.1f;

    // Start is called before the first frame update
    void Awake(){
        DontDestroyOnLoad();
    }
    void Start()
    {
        RunLoading();
    }

    private void RunLoading()
    {
        StartCoroutine(CounterTime.CounterUp(_percent, _delayTime, OnCouter, OnCounterComplete));
    }

    private void OnCouter(int value)
    {
        _loadingText.text = $"{value}%";
        _loadingSlider.value = value / 100f;
    }

    private void OnCounterComplete()
    {
        //await Task.Delay(() => AdsManager.Instance.canLoadAds);
        //SceneManager.LoadScene(Const.SCENE_GAME);
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene(){
        //yield return new WaitUntil( () => AdsManager.Instance.canLoadAds);
        yield return new WaitForEndOfFrame();
        SceneManager.LoadScene(Const.SCENE_GAMEPLAY);
    }

    private async void DontDestroyOnLoad()
        {
            var dontDestroyOnLoad = await AddressablesManager.LoadAssetAsync<GameObject>(_KeyPrefabResources.KeyDontDestroyOnLoad);
            var gameObject = GameObject.Instantiate(dontDestroyOnLoad);
        }
}

