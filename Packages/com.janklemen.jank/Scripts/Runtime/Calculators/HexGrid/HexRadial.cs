using System;
using System.Collections.Generic;
using System.Linq;

namespace Jank.Calculators.HexGrid
{
    /// <summary>
    /// A hex radial is a structure that stores data using a flat index conversion from SHexCoordinate
    /// based on the assumption that the space is created from rings emitting outwards from an origin,
    /// and enough rings exists to contain all needed coordinates. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HexRadial<T>
    {
        List<T> _values = new();

        /// <summary>
        /// Values of the radial in order
        /// </summary>
        public IReadOnlyList<T> Values => _values;

        /// <summary>
        /// The total number of steps required to get from origin to the highest indexed radial
        /// </summary>
        public int Count => _values.Count;

        /// <summary>
        /// Allows retrieving the data at a hex coordinate. Automatically grows data structure if out of bounds
        /// coordinates are referenced.
        /// </summary>
        public T this[HexCoordinate index]
        {
            get
            {
                int radialIndex = index.RadialIndex();
                
                if (_values.Count < radialIndex)
                    return default;

                return _values[radialIndex];
            }
            set
            {
                int radialIndex = index.RadialIndex();
                
                if (_values.Count < radialIndex)
                {
                    while (_values.Count < radialIndex)
                        _values.Add(default);
                }

                _values[radialIndex] = value;
            }
        }
        
        public void Clear()
        {
            _values.Clear();
        }
    }
}