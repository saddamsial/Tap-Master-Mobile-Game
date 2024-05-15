using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyTools.ParticleSystem
{
    public class _ParticleSystemManager : SingletonMonoBehaviour<_ParticleSystemManager>
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private List<_BaseMyParticles> _uiParticles;
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

            _uiParticles = lstPrefabs.ToList();
            UnityEditor.EditorUtility.SetDirty(gameObject);
        }
#endif

        public void PreLoad()
        {
            foreach (var particle in _uiParticles)
            {
                // var tmp = SimplePool.Spawn(particle.gameObject, Vector3.zero, Quaternion.identity);
                // tmp.SetActive(false);
                // tmp.transform.SetParent(_canvas.transform);
                // var go = Instantiate(particle.gameObject, Vector3.zero, Quaternion.identity, _canvas.transform);
                // go.SetActive(false);
                // _particleDict.Add(particle.ParticleType, new List<_BaseMyParticles>{go.GetComponent<_BaseMyParticles>()});
                _particleDict.Add(particle.ParticleType, particle);
            }
        }

        public void ShowParticle(_ParticleTypeEnum typeEnum, Vector3 pos)
        {
            // if (_particleDict.ContainsKey(typeEnum) == false)
            // {
            //     PreLoad();
            // }
            // _particleDict[typeEnum][0].RectTransform.position = _uiCamera.WorldToScreenPoint(pos);
            // _particleDict[typeEnum][0].gameObject.SetActive(true);
            // _particleDict[typeEnum][0].Play();
            if (!_particleDict.ContainsKey(typeEnum))
            {
                PreLoad();
            }
            if (!_particleDict.ContainsKey(typeEnum))
            {
                throw new System.Exception("Can't find particle type");
            }
            var particle = SimplePool.Spawn(_particleDict[typeEnum].gameObject, pos, Quaternion.identity).GetComponent<_BaseMyParticles>();
            particle.transform.gameObject.SetActive(false);
            particle.transform.SetParent(_canvas.transform);
            particle.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            particle.RectTransform.position = _uiCamera.WorldToScreenPoint(pos);
            particle.gameObject.SetActive(true);
            particle.Play(() => { SimplePool.Despawn(particle.gameObject); });
        }

        public void ShowParticle(_ParticleTypeEnum typeEnum, Vector3 pos, Action complete = null)
        {
            if (!_particleDict.ContainsKey(typeEnum))
            {
                PreLoad();
            }
            if (!_particleDict.ContainsKey(typeEnum))
            {
                throw new System.Exception("Can't find particle type");
            }
            var particle = SimplePool.Spawn(_particleDict[typeEnum].gameObject, pos, Quaternion.identity).GetComponent<_BaseMyParticles>();
            particle.transform.gameObject.SetActive(false);
            particle.transform.SetParent(_canvas.transform);
            particle.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            particle.RectTransform.position = _uiCamera.WorldToScreenPoint(pos);
            particle.gameObject.SetActive(true);
            particle.Play(() => { 
                complete?.Invoke();
                SimplePool.Despawn(particle.gameObject); 
            });
        }

        public void HideParticle(_ParticleTypeEnum type)
        {
            //_particleDict[type][0].gameObject.SetActive(false);

        }

        public Camera UICamera
        {
            get => _uiCamera;
            set => _uiCamera = value;
        }
    }
}