using System.Collections.Generic;
using Coffee.UIExtensions;
using UnityEngine;

namespace MyTools.ParticleSystem
{
    public class _ParticleSystemManager : SingletonMonoBehaviour<_ParticleSystemManager>
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private _BaseMyParticles[] _uiParticles;
        [SerializeField] private string _particlePath;
        [SerializeField] private Camera _uiCamera;

        private Dictionary<_ParticleTypeEnum, _BaseMyParticles> _particleDict = new Dictionary<_ParticleTypeEnum, _BaseMyParticles>();

#if UNITY_EDITOR
        [ContextMenu("LoadPopupPrefabs")]
        void LoadPopupPrefabs()
        {
            var lstPrefabs = new List<_BaseMyParticles>();
            var lstNames = System.IO.Directory.GetFiles($"{_particlePath}",
                "*.prefab", System.IO.SearchOption.AllDirectories);
            foreach (var itName in lstNames)
            {
                var obj = UnityEditor.AssetDatabase.LoadAssetAtPath<_BaseMyParticles>($"{itName}");
                if (obj == null) continue;
                lstPrefabs.Add(obj);
            }

            _uiParticles = lstPrefabs.ToArray();
            UnityEditor.EditorUtility.SetDirty(gameObject);
        }
#endif

        public void PreLoad()
        {
            foreach (var particle in _uiParticles)
            {
                var go = Instantiate(particle.gameObject, Vector3.zero, Quaternion.identity, _canvas.transform);
                go.SetActive(false);
                _particleDict.Add(particle.ParticleType, go.GetComponent<_BaseMyParticles>());
            }
        }

        public void ShowParticle(_ParticleTypeEnum typeEnum, Vector3 pos)
        {
            if (_particleDict.ContainsKey(typeEnum) == false)
            {
                PreLoad();
            }
            _particleDict[typeEnum].RectTransform.position = _uiCamera.WorldToScreenPoint(pos);
            _particleDict[typeEnum].gameObject.SetActive(true);
            _particleDict[typeEnum].Play();
        }

        public void HideParticle(_ParticleTypeEnum type)
        {
            _particleDict[type].gameObject.SetActive(false);
        }

        public Camera UICamera {
            get => _uiCamera;
            set => _uiCamera = value;
        }
    }
}