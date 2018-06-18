using System;
using System.Collections.Generic;
using System.Linq;

namespace WordCount.Utils
{
    public class KWayMerge<T>
    {
        private readonly Func<T, T, bool> _greatherThanFunc;
        private readonly List<IEnumerator<T>> _sortedEnumerators;
        private bool _mergeCalled = false;

        public KWayMerge(IEnumerator<T>[] sortedEnumerables, Func<T, T, bool> greatherThanFunc)
        {
            if (sortedEnumerables == null) throw new ArgumentNullException(nameof(sortedEnumerables));
            if (sortedEnumerables.Length == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(sortedEnumerables));
            _greatherThanFunc = greatherThanFunc ?? throw new ArgumentNullException(nameof(greatherThanFunc));

            _sortedEnumerators = sortedEnumerables
                .Where(en => en.MoveNext()) // only non-empty enumerators
                .ToList();

            BuildMaxHeap();
        }

        public IEnumerable<T> Execute()
        {
            if (_mergeCalled) throw new InvalidOperationException($"{nameof(Execute)} method can only be called once");
            _mergeCalled = true;

            while (_sortedEnumerators.Count != 0)
            {
                // at position 0 there's always a maximum item thanks to the max-heap main property
                var maxEnumerator = _sortedEnumerators[0];

                yield return maxEnumerator.Current;

                if (maxEnumerator.MoveNext())
                {
                    Heapify(0);
                }
                else
                {
                    _sortedEnumerators.Remove(maxEnumerator);
                    // rebuild heap, because we have deleted an item from it
                    BuildMaxHeap();
                }
            }
        }

        private void BuildMaxHeap()
        {
            for (var i = _sortedEnumerators.Count / 2; i >= 0; i--)
            {
                Heapify(i);
            }
        }

        private void Heapify(int index)
        {
            int indexOfLeft = 2 * index + 1;
            int indexOfRight = 2 * index + 2;

            int indexOfLargest = index;

            if (indexOfLeft < _sortedEnumerators.Count && GreaterThan(indexOfLeft, indexOfLargest))
            {
                indexOfLargest = indexOfLeft;
            }

            if (indexOfRight < _sortedEnumerators.Count && GreaterThan(indexOfRight, indexOfLargest))
            {
                indexOfLargest = indexOfRight;
            }

            if (indexOfLargest != index)
            {
                Swap(index, indexOfLargest);
                Heapify(indexOfLargest);
            }
        }

        private void Swap(int left, int right)
        {
            var tmp = _sortedEnumerators[left];
            _sortedEnumerators[left] = _sortedEnumerators[right];
            _sortedEnumerators[right] = tmp;
        }

        private bool GreaterThan(int left, int right)
        {
            return _greatherThanFunc(_sortedEnumerators[left].Current, _sortedEnumerators[right].Current);
        }
    }
}
