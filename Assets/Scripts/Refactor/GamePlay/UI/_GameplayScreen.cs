using Core.Data;
using MyTools.ScreenSystem;
using TMPro;
using UnityEngine;

namespace Core.UI{
    public class _GameplayScreen : _BaseScreen{
        [Header("Gameplay Screen Elements")]
        [SerializeField] private TMP_Text _levelText;

        protected override void OnStartShowItSelf()
        {
            base.OnCompleteShowItSelf();
            SetupScreen();
        }

        public void SetupScreen(){
            _levelText.text ="Level " + _PlayerData.UserData.HighestLevel;
        }
    }
}