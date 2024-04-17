using PopupSystem;
using UIS;
using UnityEngine;

namespace Core.GamePlay.LevelSystem
{
    public class _LevelPopup : BasePopup
    {
        /// <summary>
        /// Link to list
        /// </summary>
        [SerializeField]
        Scroller List = null;

        /// <summary>
        /// Items count
        /// </summary>
        [SerializeField]
        int Count = 100;

        public void Show()
        {
            base.Show();
            Start();
        }

        /// <summary>
        /// Init
        /// </summary>
        void Start()
        {
            Debug.Log("Start");
            List.OnFill += OnFillItem;
            List.OnHeight += OnHeightItem;
            List.InitData(10);
        }

        /// <summary>
        /// Callback on fill item
        /// </summary>
        /// <param name="index">Item index</param>
        /// <param name="item">Item object</param>
        void OnFillItem(int index, GameObject item)
        {
            //item.GetComponentInChildren<TextMeshProUGUI>().text = index.ToString();
            item.GetComponent<_LevelElementsContainer>().SetLevelInLine(index, 30);
        }

        /// <summary>
        /// Callback on request item height
        /// </summary>
        /// <param name="index">Item index</param>
        /// <returns>Current item height</returns>
        int OnHeightItem(int index)
        {
            return 360;
        }

        /// <summary>
        /// Load next demo scene
        /// </summary>
        /// <param name="index">Scene index</param>
        public void SceneLoad(int index)
        {
            //SceneManager.LoadScene(index);
        }
    }
}