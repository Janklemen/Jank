using Jank.Utilities.Calculators;
using UnityEngine;

namespace Jank.Utilities
{
    /// <summary>
    /// Spawns the given GameObjects in a grid
    /// </summary>
    public class GameObjectGridSpawner : MonoBehaviour
    {
        [SerializeField] GameObject _prefab;
        [SerializeField] int _gridWidth;
        [SerializeField] int _distanceBetween = 10;

        GridCalculator _grid;
        
        void Start()
        {
            _grid = new GridCalculator(_gridWidth);
            
            int total = _grid.Width * _grid.Width;

            for (int i = 0; i < total; i++)
            {
                (int x, int y) = _grid.UnpackIndex(i);
                Instantiate(_prefab, new Vector3(x * _distanceBetween, 0, y * _distanceBetween), Quaternion.identity);
            }
        }
    }
}
