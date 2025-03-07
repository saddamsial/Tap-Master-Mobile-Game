using System.Collections.Generic;
using Core.GamePlay.Block;
using Core.ResourceGamePlay;
using Extensions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using DG.Tweening;
using Core.SystemGame;
using Core.Data;
using System.Threading.Tasks;

namespace Core.GamePlay.BlockPool
{
    public class _BlockPool
    {

        private const int sizeX = 30;
        private const int sizeY = 30;
        private const int sizeZ = 30;

        private bool _isLogicInit;
        private bool[][][] _blockLogicPool;
        private List<_BlockController> _blockObjectPool;
        private GameObject _blockContainer;
        private bool _isInitPool = false;
        private GameObject _blockPrefab;
        private Material _movingMaterial;
        private Material _blockedMaterial;
        private Material _idleMaterial;

        public _BlockPool()
        {
            InitLogicPool(sizeX, sizeY, sizeZ);
            //_GameEvent.OnUseBoosterHint += UseBoosterHint;
        }

        ~_BlockPool()
        {
            //_GameEvent.OnUseBoosterHint -= UseBoosterHint;
        }

        public async Task InitPool(LevelData levelData)
        {
            _blockObjectPool ??= new List<_BlockController>();
            DeSpawnAllBlocks();
            ClearLogicPool();
            _blockContainer ??= new GameObject("BlockContainer");
            _blockContainer.transform.position = Vector3.zero;
            _blockPrefab ??= await AddressablesManager.LoadAssetAsync<GameObject>(_KeyPrefabResources.KeyBlock);
            _movingMaterial ??= await AddressablesManager.LoadAssetAsync<Material>(_KeyMaterialResources.KeyMovingMaterial);
            _blockedMaterial ??= await AddressablesManager.LoadAssetAsync<Material>(_KeyMaterialResources.KeyBlockedMaterial);
            _idleMaterial ??= await AddressablesManager.LoadAssetAsync<Material>(_KeyMaterialResources.KeyIdleMaterial);
            int minX = 0;
            int minY = 0;
            int minZ = 0;
            for (int i = 0; i < levelData.blockStates.Count; i++)
            {
                //var block = SimplePool.Spawn(_blockPrefab, Vector3.zero, Quaternion.identity);
                var block = GameObject.Instantiate(_blockPrefab, Vector3.zero, Quaternion.identity);
                block.name = "Block" + i;
                //Debug.Log("Block " + i + " Is Tweening: " + DOTween.IsTweening(block.transform));
                block.transform.SetParent(_blockContainer.transform);
                block.SetActive(true);
                block.GetComponent<_BlockController>().InitBlock(_idleMaterial, _movingMaterial, _blockedMaterial, levelData.blockStates[i].rotation, levelData.blockStates[i].color, levelData.isSetColor);
                _blockObjectPool.Add(block.GetComponent<_BlockController>());
                block.transform.position = levelData.blockStates[i].pos;
                Vector3Int logicPos = _NormalizingVector3.LogicPos(block.transform.position);
                minX = Mathf.Min(minX, logicPos.x);
                minY = Mathf.Min(minY, logicPos.y);
                minZ = Mathf.Min(minZ, logicPos.z);
            }

            for (int i = 0; i < levelData.blockStates.Count; i++)
            {
                Vector3Int logicPos = _NormalizingVector3.LogicPos(_blockObjectPool[i].transform.position);
                SetStateElementBlockInPool(logicPos.x - minX, logicPos.y - minY, logicPos.z - minZ, true);
                _blockObjectPool[i].LogicPos = new Vector3Int(logicPos.x - minX, logicPos.y - minY, logicPos.z - minZ);
            }
            var containerPos = _blockContainer.transform.position;
            containerPos -= levelData.size / 2 - new Vector3(0.5f, 0.5f, 0.5f);
            _blockContainer.transform.position = containerPos;
        }

        public void SpawnSpecialBlock()
        {
            var block = _blockObjectPool[0];
            block.SetCurrentTypeBlock(_BlockTypeEnum.GoldReward);
        }

        public void SpawnSpecialBlockInCameraView(Camera camera)
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
            int randomIndex = Random.Range(0, _blockObjectPool.Count);
            for (int k = 0; k < _blockObjectPool.Count; k++)
            {
                int i = randomIndex;
                if (_blockObjectPool[i].CheckObjectVisible(planes))
                {
                    //Debug.Log("Find block in camera view");
                    if (_blockObjectPool[i].CurrentType != _BlockTypeEnum.GoldReward && _blockObjectPool[i].CurrentType != _BlockTypeEnum.PuzzleReward)
                    {
                        int tmp = Random.Range(0, 100);
                        if (tmp < 20 && !_GameManager.Instance.GamePlayManager.IsSpawnCollectionBlock){
                            _blockObjectPool[i].SetCurrentTypeBlock(_BlockTypeEnum.PuzzleReward);
                            _MySoundManager.Instance.PlaySound(_SoundType.Spawn);
                            _GameManager.Instance.GamePlayManager.IsSpawnCollectionBlock = true;
                        }
                        else{
                            _blockObjectPool[i].SetCurrentTypeBlock(_BlockTypeEnum.GoldReward);
                            _MySoundManager.Instance.PlaySound(_SoundType.Spawn);
                        }
                        return;
                    }
                }
                i = (i + 1) % _blockObjectPool.Count;
            }
            throw new System.Exception("No block in camera view");
        }


        public void SetStateElementBlockInPool(int x, int y, int z, bool value)
        {
            _blockLogicPool[x][y][z] = value;
        }


        public bool CheckCanEscape(_BlockController block)
        {
            Vector3 direction = _NormalizingVector3.ConvertToVector3Int(-block.transform.right);
            Vector3 tempLogicPos = block.LogicPos + direction;
            for (int i = 0; i < sizeX; i++)
            {
                //neu logicPos nam ngoai kich thuoc cua pool
                if (tempLogicPos.x < 0 || tempLogicPos.x >= sizeX) return true;
                if (tempLogicPos.y < 0 || tempLogicPos.y >= sizeY) return true;
                if (tempLogicPos.z < 0 || tempLogicPos.z >= sizeZ) return true;

                //neu tai vi tri logicPos co block
                if (_blockLogicPool[(int)tempLogicPos.x][(int)tempLogicPos.y][(int)tempLogicPos.z])
                {
                    block.ObstacleLogicPos = _NormalizingVector3.IgnoreDecimalPart(tempLogicPos);
                    return false;
                }
                tempLogicPos += direction;
            }
            //neu logicPos nam trong kich thuoc cua pool va khong co block
            return true;
        }

        public void DeSpawnAllBlocks()
        {
            foreach (var block in _blockObjectPool)
            {
                //DOTween.Kill(block.transform);
                //block.OnBlockReturnToPool();
                //ObjectPooling._ObjectPooling.Instance.ReturnToPool(ObjectPooling._TypeGameObjectEnum.Block, block.gameObject);
                if(block == null) continue;
                if(block.gameObject == null) continue;
                SimplePool.Despawn(block.gameObject);
            }
            _blockObjectPool.Clear();
        }

        public void DespawnBlock(_BlockController block)
        {
            DOTween.Kill(block.transform);
            block.OnBlockReturnToPool();
            //bjectPooling._ObjectPooling.Instance.ReturnToPool(ObjectPooling._TypeGameObjectEnum.Block, block.gameObject);
            SimplePool.Despawn(block.gameObject);
            _blockObjectPool.Remove(block);
        }

        public List<_BlockController> GetNeighborBlock(int r, _BlockController block){
            List<_BlockController> neighborBlocks = new List<_BlockController>();
            for(int i = -r; i <= r; i++){
                for(int j = -r; j <= r; j++){
                    for(int k = -r; k <= r; k++){
                        if(i == 0 && j == 0 && k == 0) continue;
                        Vector3Int logicPos = block.LogicPos + new Vector3Int(i, j, k);
                        if(logicPos.x < 0 || logicPos.x >= sizeX) continue;
                        if(logicPos.y < 0 || logicPos.y >= sizeY) continue;
                        if(logicPos.z < 0 || logicPos.z >= sizeZ) continue;
                        if(_blockLogicPool[logicPos.x][logicPos.y][logicPos.z]){
                            _BlockController neighborBlock = GetBlock(logicPos);
                            neighborBlocks.Add(neighborBlock);
                        }
                    }
                }
            }
            return neighborBlocks;
        }

        public void ExplodeBlocks(List<_BlockController> blocks, _BlockController block){
            // for(int i = 0; i <= blocks.Count - 2; i++){
            //     blocks[i].SetCurrentTypeBlock(_BlockTypeEnum.InExplosion);
            // }
            // blocks[blocks.Count - 1].SetCurrentTypeBlock(_BlockTypeEnum.InExplosion);
            _GameManager.Instance.CameraController.ShakedCamera();
            _MySoundManager.Instance.PlaySound(_SoundType.Explode);
            _MySoundManager.Instance.Vibrate();
            block.SetCurrentTypeBlock(_BlockTypeEnum.InExplosion);
            foreach(var b in blocks){
                //b.gameObject.SetActive(false);
                b.SetCurrentTypeBlock(_BlockTypeEnum.InExplosion);
            }
        }

        public _BlockController GetBlock(Vector3Int logicPos)
        {
            return _blockObjectPool.Find(block => block.LogicPos.Equals(logicPos));
        }

        private void InitLogicPool(int sizeX, int sizeY, int sizeZ)
        {
            if (_isLogicInit) return;
            _blockLogicPool = new bool[sizeX][][];
            for (int i = 0; i < sizeX; i++)
            {
                _blockLogicPool[i] = new bool[sizeY][];
                for (int j = 0; j < sizeY; j++)
                {
                    _blockLogicPool[i][j] = new bool[sizeZ];
                }
            }
            _isLogicInit = true;
        }

        private void InitObjectPooling()
        {
            _blockObjectPool ??= new List<_BlockController>();
            _blockObjectPool.Clear();
        }

        private void ClearLogicPool()
        {
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    for (int k = 0; k < sizeZ; k++)
                    {
                        _blockLogicPool[i][j][k] = false;
                    }
                }
            }
        }

        private void UseBoosterHint(){
            //if(_blockObjectPool.Count < _ConstantGameplayConfig.MIN_BLOCKS_TO_BE_REMOVED_WHEN_HINT) return;
            int blockToRemove = _blockObjectPool.Count / 10;
            blockToRemove = Mathf.Min(blockToRemove, _ConstantGameplayConfig.MAX_BLOCKS_TO_BE_REMOVED_WHEN_HINT);
            blockToRemove = Mathf.Max(blockToRemove, _ConstantGameplayConfig.MIN_BLOCKS_TO_BE_REMOVED_WHEN_HINT);
            blockToRemove = Mathf.Min(blockToRemove, _blockObjectPool.Count);
            int count = 0;
            int totalBlockToRemove = 0;
            for (int i = 0; i < blockToRemove; i++)
            {
                int randomIndex = Random.Range(0, _blockObjectPool.Count);
                if (_blockObjectPool[randomIndex].CurrentType != _BlockTypeEnum.Moving)
                {
                    i--;
                    count += 1;
                    if(count >= _blockObjectPool.Count)
                        break;
                    continue;
                }
                totalBlockToRemove += 1;
                _blockObjectPool[randomIndex].SetCurrentTypeBlock(_BlockTypeEnum.MovingSpecial);
                _blockObjectPool.RemoveAt(randomIndex);
                //DespawnBlock(_blockObjectPool[randomIndex]);
                //_GamePlayManager.Instance.OnBlockSelected(_blockObjectPool[randomIndex], true, false, 1);
            }
            //_GamePlayManager.Instance.OnBlockSelected(null, totalBlockToRemove);
            //_GamePlayManager.Instance.OnBlockSelected(, true, false, blockToRemove);
        }

        public List<_BlockController> BlockObjectPool => _blockObjectPool;
    }
}