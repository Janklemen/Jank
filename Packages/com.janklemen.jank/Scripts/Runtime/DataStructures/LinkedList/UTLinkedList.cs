using System.Collections.Generic;

namespace Jank.DataStructures.LinkedList
{
    /// <summary>
    /// Utilities that facilitate common operations on LinkedList DataStructures
    /// </summary>
    public static class UtLinkedList
    {
        /// <summary>
        /// Move first occurence of an element towards the head of the list a number of steps. 
        /// If steps is greater than steps to head, move to head. 
        /// </summary>
        /// <typeparam name="T">Type of list</typeparam>
        /// <param name="list">The list containing elements</param>
        /// <param name="obj">The objet to move up</param>
        /// <param name="steps">The number of steps to move</param>
        /// <returns>Number of steps taken, or -1 if object does not exist in list</returns>
        public static int StepTowardsHead<T>(this LinkedList<T> list, T obj, int steps)
        {
            // if object not in list return error
            LinkedListNode<T> node = list.Find(obj);
            if (node == null)
            {
                return -1;
            }

            // count the amount of steps that can be taken until first element or steps is hit
            int totalSteps = 0;
            for (; totalSteps < steps; totalSteps++)
            {
                if (node.Previous == null)
                {
                    break;
                }

                node = node.Previous;
            }

            // if no step was taken, return 0
            if (totalSteps == 0)
            {
                return 0;
            }

            // Remove object and put it before node (which is now the node steps away)
            list.Remove(obj);
            list.AddBefore(node, obj);

            // return number of steps taken
            return totalSteps;
        }

        /// <summary>
        /// Move first occurence of an element towards the tail of the list a number of steps. 
        /// If steps is greater than steps to tail, move to tail. 
        /// </summary>
        /// <typeparam name="T">Type of list</typeparam>
        /// <param name="list">The list containing elements</param>
        /// <param name="obj">The objet to move down</param>
        /// <param name="steps">The number of steps to move</param>
        /// <returns>Number of steps taken, or -1 if object does not exist in list</returns>
        public static int StepTowardsTail<T>(this LinkedList<T> list, T obj, int steps)
        {
            // if object not in list return error
            LinkedListNode<T> newPreivous = list.Find(obj);
            if (newPreivous == null)
            {
                return -1;
            }

            // count the amount of steps that can be taken until last element or steps is hit
            int totalSteps = 0;
            for (; totalSteps < steps; totalSteps++)
            {
                if (newPreivous.Next == null)
                {
                    break;
                }

                newPreivous = newPreivous.Next;
            }

            // if no step was taken, return 0
            if (totalSteps == 0)
            {
                return 0;
            }

            // Remove object and put it before new_next
            list.Remove(obj);
            list.AddAfter(newPreivous, obj);

            // return number of steps taken
            return totalSteps;
        }


        /// <summary>
        /// Moves the obj to the head of the list
        /// </summary>
        /// <typeparam name="T">Type of list</typeparam>
        /// <param name="list">The list containing elements</param>
        /// <param name="obj">The objet to move to head</param>
        /// <returns>Returns false if object wasn't in list, true otherwise</returns>
        public static bool JumpToHead<T>(this LinkedList<T> list, T obj)
        {
            if (list.Find(obj) == null)
            {
                return false;
            }

            list.Remove(obj);
            list.AddFirst(obj);

            return true;
        }

        /// <summary>
        /// Moves the obj to the tail of the list
        /// </summary>
        /// <typeparam name="T">Type of list</typeparam>
        /// <param name="list">The list containing elements</param>
        /// <param name="obj">The objet to move to tail</param>
        /// <returns>Returns false if object wasn't in list, true otherwise</returns>
        public static bool JumpToTail<T>(this LinkedList<T> list, T obj)
        {
            if (list.Find(obj) == null)
            {
                return false;
            }

            list.Remove(obj);
            list.AddLast(obj);

            return true;
        }
    }
}