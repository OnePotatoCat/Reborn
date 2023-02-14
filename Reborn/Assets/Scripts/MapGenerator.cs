using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;

namespace Reborn
{
    public class MapGenerator : MonoBehaviour
    {
        [Header("Tile Grid")]
        [SerializeField] private int2 _GridSize = new int2(12, 12);
        [SerializeField] private GameObject _TilePrefab;

        public int CellCount => this._GridSize.x * this._GridSize.y;
        public int MovableCellCount => (int)this.CellCount / 2;
        public int2 CenterCell => new int2((int)this._GridSize.x/2, (int)this._GridSize.y/2);

        public enum Tile
        {
            floor,
            wall,
        }

        private void Awake()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}