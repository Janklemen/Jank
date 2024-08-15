using System;

namespace Jank.Utilities.Calculators
{
    public interface IGridCalculator
    {
        int Width { get; }
        int Height { get; }
        int TotalCells { get; }
        bool ValidIndex(int x, int y);
        bool ValidIndex((int x, int y) coord);
        bool ValidIndex(int flatIndex);
        int FlatIndex(int x, int y);
        int FlatIndex((int x, int y) coord);
        (int, int) UnpackIndex(int index);
        int StepFromIndex(int origin, GridCalculator.EManhattan4Direction direction, int steps);

        /// <summary>
        /// Returns to reflection of an index around a center. This is useful when you want to do calculations related
        /// to surrounding pieces.
        /// </summary>
        int GetReflection(int toMirror, int centre);

        int[] NeighboursY<T>(T[] grid, int intersection, Func<int, T, bool> block = null);
        int[] NeighboursX<T>(T[] grid, int intersection, Func<int, T, bool> block = null);

        /// <summary>
        /// Returns the indices for neighbouring indices of an index if they are valid and not blocked
        /// </summary>
        /// <param name="grid">target grid</param>
        /// <param name="intersection">intersection index</param>
        /// <param name="block">if returns true, blocks index. (index, value) => result </param>
        /// <typeparam name="T">grid type</typeparam>
        /// <returns></returns>
        int[] Manhattan4Neighbours<T>(T[] grid, int intersection, Func<int, T, bool> block = null);

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
        int[][] OrthogonalIntersectionSeparate<T>(T[] grid, int intersection, int range = int.MaxValue, Func<int, T, bool> block = null);

        /// <summary>
        /// Returns lines horizontal and vertical that run through a point on a grid. The block function will stop the
        /// step if the predicate is met. All unique indices are returned as a single array.
        /// </summary>
        /// <remarks>
        /// Order should not be relied on.
        /// </remarks>
        int[] OrthogonalIntersection<T>(T[] grid, int intersection, int range = int.MaxValue, Func<int, T, bool> block = null);

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
        void LineStepFunction<T>(T[] grid, int origin, (int x, int y) step, Action<int, T> act, int range = int.MaxValue, Func<int, T, bool> block = null);

        /// <summary>
        /// If the target is a straight line in a <see cref="GridCalculator.EManhattan4Direction"/> from the origin, returns the
        /// direction and the steps required. Otherwise, returns steps = -1. If target is origin, return (NoDirection,
        /// 0)
        /// </summary>
        (GridCalculator.EManhattan4Direction direction, int steps) GetManhattan4Steps(int origin, int target);
    }
}