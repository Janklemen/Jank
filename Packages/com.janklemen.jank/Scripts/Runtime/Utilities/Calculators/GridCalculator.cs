using System;
using System.Collections.Generic;
using System.Linq;
using Jank.DataStructures.Array;
using Jank.DotNet.IEnumerable;

namespace Jank.Utilities.Calculators
{
    /// <summary>
    /// Provides functionality related to grid indexing. It's main use is the consistent translation of indices from
    /// flat space and coordinate space. It also tracks grid size, and can validate the existence of indices.
    /// </summary>
    public class GridCalculator : IGridCalculator
    {
        public int Width { get; }
        public int Height => Width;

        public GridCalculator(int width)
        {
            Width = width;
        }

        public int TotalCells => Width * Width;

        public bool ValidIndex(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Width;
        public bool ValidIndex((int x, int y) coord) => ValidIndex(coord.x, coord.y);
        public bool ValidIndex(int flatIndex) => ValidIndex(UnpackIndex(flatIndex));
        
        public int FlatIndex(int x, int y)
        {
            return (y * Width) + x;
        }

        public int FlatIndex((int x, int y) coord) => FlatIndex(coord.x, coord.y);

        public (int, int) UnpackIndex(int index)
            => (index % Width, index / Width);

        public int StepFromIndex(int origin, EManhattan4Direction direction, int steps)
        {
            (int x, int y) = UnpackIndex(origin);

            switch (direction)
            {
                case EManhattan4Direction.Up:
                    y += steps;
                    break;
                case EManhattan4Direction.Right:
                    x += steps;
                    break;
                case EManhattan4Direction.Down:
                    y -= steps;
                    break;
                case EManhattan4Direction.Left:
                    x -= steps;
                    break;
            }

            return FlatIndex((x, y));
        }
        
        /// <summary>
        /// Returns to reflection of an index around a center. This is useful when you want to do calculations related
        /// to surrounding pieces.
        /// </summary>
        public int GetReflection(int toMirror, int centre)
        {
            (int x, int y) centreU = UnpackIndex(centre);
            (int x, int y) mirrorU = UnpackIndex(toMirror);

            mirrorU.x -= centreU.x;
            mirrorU.y -= centreU.y;

            mirrorU.x *= -1;
            mirrorU.y *= -1;

            mirrorU.x += centreU.x;
            mirrorU.y += centreU.y;

            return FlatIndex(mirrorU);
        }
        
        public int[] NeighboursY<T>(T[] grid, int intersection, Func<int, T, bool> block = null)
        {
            List<int> indicies = new();

            LineStepFunction(grid, intersection, (0, 1), (i, _) => indicies.Add(i), block: block, range:1);
            LineStepFunction(grid, intersection, (0, -1), (i, _) => indicies.Add(i), block: block, range:1);

            return indicies.ToArray();
        }
        
        public int[] NeighboursX<T>(T[] grid, int intersection, Func<int, T, bool> block = null)
        {
            List<int> indicies = new();

            LineStepFunction(grid, intersection, (1, 0), (i, _) => indicies.Add(i), block: block, range:1);
            LineStepFunction(grid, intersection, (-1, 0), (i, _) => indicies.Add(i), block: block, range:1);

            return indicies.ToArray();
        }
        
        /// <summary>
        /// Returns the indices for neighbouring indices of an index if they are valid and not blocked
        /// </summary>
        /// <param name="grid">target grid</param>
        /// <param name="intersection">intersection index</param>
        /// <param name="block">if returns true, blocks index. (index, value) => result </param>
        /// <typeparam name="T">grid type</typeparam>
        /// <returns></returns>
        public int[] Manhattan4Neighbours<T>(T[] grid, int intersection, Func<int, T, bool> block = null)
        {
            int[] values = OrthogonalIntersection(grid, intersection, range: 1, block: block);
            return values.ToList().Where(i =>i != intersection).ToArray();
        }
        
        /// <summary>
        /// Collects the indices that run in all 4 directions from the point. Stops if the <see cref="block"/> predicate
        /// is met. Keeps the lines as 4 separate arrays.
        /// </summary>
        /// <remarks>
        /// Intersection is not included in the result.
        /// 
        /// Line order is always closest to origin to furthest from origin.
        ///
        /// Order of line directions is always up, right, down, left assuming a non-weird 2D basis
        /// </remarks>
        public int[][] OrthogonalIntersectionSeparate<T>(T[] grid, int intersection, int range = int.MaxValue, Func<int, T, bool> block = null)
        {
            List<int>[] validIndices = {new(), new(), new(), new()};

            LineStepFunction(grid, intersection, (0, 1), (i, _) => validIndices[0].Add(i), block: block, range:range);
            LineStepFunction(grid, intersection, (1, 0), (i, _) => validIndices[1].Add(i), block:block, range:range);
            LineStepFunction(grid, intersection, (0, -1), (i, _) => validIndices[2].Add(i), block:block, range:range);
            LineStepFunction(grid, intersection, (-1, 0), (i, _)=> validIndices[3].Add(i), block:block, range:range);

            validIndices.ForEach(l => l.Remove(intersection));
            return new[] { validIndices[0].ToArray(), validIndices[1].ToArray(),validIndices[2].ToArray(),validIndices[3].ToArray()} ;
        }
        
        /// <summary>
        /// Returns lines horizontal and vertical that run through a point on a grid. The block function will stop the
        /// step if the predicate is met. All unique indices are returned as a single array.
        /// </summary>
        /// <remarks>
        /// Order should not be relied on.
        /// </remarks>
        public int[] OrthogonalIntersection<T>(T[] grid, int intersection, int range = int.MaxValue, Func<int, T, bool> block = null)
        {
            return OrthogonalIntersectionSeparate(grid, intersection, range, block).UniqueMerge().ToArray();
        }
        
        /// <summary>
        /// Calls the act function along a line starting from origin following the step. Stops if step out of bounds.
        /// </summary>
        /// <param name="grid">target grid</param>
        /// <param name="origin">origin index on the grid</param>
        /// <param name="step">the step to take</param>
        /// <param name="act">action to perform per step</param>
        /// <param name="range">the max number of steps to take</param>
        /// <param name="block">predicate that determines if step should stop</param>
        /// <typeparam name="T">grid type</typeparam>
        public void LineStepFunction<T>(T[] grid, int origin, (int x, int y) step, Action<int, T> act, int range = int.MaxValue, Func<int, T, bool> block = null)
        {
            (int x, int y) test = UnpackIndex(origin);
            int stepsTaken = 0;
            
            while (ValidIndex(test) && grid.ValidIndex(FlatIndex(test)) && stepsTaken <= range)
            {
                int flatIndex = FlatIndex(test);
                
                if (block != null && block(flatIndex, grid[flatIndex])) 
                    break;
                
                act(flatIndex, grid[flatIndex]);
                test.x += step.x;
                test.y += step.y;

                stepsTaken++;
            }
        }

        /// <summary>
        /// If the target is a straight line in a <see cref="EManhattan4Direction"/> from the origin, returns the
        /// direction and the steps required. Otherwise, returns steps = -1. If target is origin, return (NoDirection,
        /// 0)
        /// </summary>
        public (EManhattan4Direction direction, int steps) GetManhattan4Steps(int origin, int target)
        {
            if (origin == target)
                return (EManhattan4Direction.NoDirection, 0);
            
            (int originX, int originY) = UnpackIndex(origin);
            (int targetX, int targetY) = UnpackIndex(target);

            if (originX != targetX && originY != targetY)
                return (EManhattan4Direction.NoDirection, -1);

            if (originX == targetX)
            {
                int step = targetY - originY;
                int sign = Math.Sign(step);
                return sign == 1 ? (EManhattan4Direction.Up, step) : (EManhattan4Direction.Down, -step);
            }
            else 
            {
                int step = targetX - originX;
                int sign = Math.Sign(step);
                return sign == 1 ? (EManhattan4Direction.Right, step) : (EManhattan4Direction.Left, -step);
            }
        }

        public enum EManhattan4Direction
        {
            NoDirection,
            Up,
            Right, 
            Down,
            Left
        }
    }
}