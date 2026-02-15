using System.Diagnostics.CodeAnalysis;

namespace IndexTree
{
    /// <summary>
    /// Index tree based on balanced binary search tree.
    /// 
    /// In addition to the AddKey, RemoveKey, and ContainsKey methods you would expect from an indexed container, 
    /// it also provides index-based access via GetKeyAtIndex and GetIndexOfKey methods. 
    /// The tree is always perfectly balanced, so the depth of the tree is always O(log n), where n is the number of keys in the tree. 
    /// This means that all search operations have O(log n) time complexity.
    /// 
    /// </summary>
    /// <typeparam name="K">The type of the keys in the index tree.</typeparam>
    public class IndexTree<K> where K : IComparable<K>
    {
        internal sealed class IndexTreeNode
        {
            public required K Key;
            public IndexTreeNode? Left;
            public IndexTreeNode? Right;
        }

        private IndexTreeNode? root;
        private int size;
        readonly Stack<IndexTreeNode> walkingStack = new(32);

        public int Size => size;            // Total number of keys in the tree 

        /// <summary>
        /// Initialize the tree from a sorted array of keys. The keys in the array must be unique.
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <exception cref="System.InvalidOperationException"></exception>
        public void InitFromArray(K[] keys, int start = -1, int count = -1)
        {
            if (Size > 0)
                throw new InvalidOperationException("Tree is already initialized.");
            if (start == -1)
                start = 0;
            if (count == -1)
                count = keys.Length - start;
            size = count;
            if (count > 0)
            {
                root = SubtreeFromArrayRange(keys, start, start + count - 1, count);
            }
            else
            {
                root = null;
            }
        }

        /// <summary>
        /// Returns the current index of a key in the tree, if found. Otherwise, throws an exception.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetIndexOfKey(K key)
        {
            if (root == null)
                ThrowKeyNotFoundError(key);

            var res = GetIndexOfKey(root, Size, key);

            if (res == -1)
                ThrowKeyNotFoundError(key);

            return res;
        }

        /// <summary>
        /// Returns true if the tree contains the specified key, false otherwise.
        /// </summary>
        /// <param name="key">The key to search for.</param>
        /// <returns>True if the key is found, false otherwise.</returns>
        public bool ContainsKey(K key)
        {
            if (root == null)
                return false;
            return ContainsKey(root, key);
        }

        /// <summary>
        /// Returns a key at the given index in the tree. If index is out of range, throws an exception.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public K GetKeyAtIndex(int index)
        {
            if (index < 0 || index >= Size)
                throw new ArgumentOutOfRangeException($"Index {index} is out of range.");
            int left = 0;
            IndexTreeNode? node = root;
            int nodesInSubtree = Size;
            while (node != null)
            {
                int leftSubtreeSize = (nodesInSubtree - 1) / 2;

                if (index < left + leftSubtreeSize)
                {
                    node = node.Left;
                    nodesInSubtree = leftSubtreeSize;
                }
                else if (index < left + leftSubtreeSize + 1)
                {
                    // found
                    return node.Key;
                }
                else
                {
                    int rightSubtreeSize = leftSubtreeSize + 1 - nodesInSubtree % 2;
                    left += leftSubtreeSize + 1;
                    node = node.Right;
                    nodesInSubtree = rightSubtreeSize;
                }
            }
            ThrowInconsistentStateError();
            return default; // to satisfy compiler
        }

        /// <summary>
        /// Finds the index of the specified key or, if the key is not present, the index of the largest key that is
        /// less than the specified key.
        /// </summary>
        /// <param name="key">The key to search for. If the key is not found, the method returns the index of the largest key that is less
        /// than this value.</param>
        /// <returns>The zero-based index of the specified key or the largest key less than the specified key. Returns -1 if the
        /// collection is empty or if no such key exists.</returns>
        public int GetIndexOfKeyOrSmaller(K key)
        {
            if (root == null)
                return -1;

            var res = GetIndexOfKeyOrSmaller(root, Size, key);
            return res;
        }

        /// <summary>
        /// Finds the index of the specified key or, if the key is not present, the index of the smallest key that is
        /// greater than the specified key.
        /// </summary>
        /// <param name="key">The key to search for. If the key is not found, the method returns the index of the largest key that is less
        /// than this value.</param>
        /// <returns>The zero-based index of the specified key or the smallest key greater than the specified key. Returns -1 if the
        /// collection is empty or if no such key exists.</returns>
        public int GetIndexOfKeyOrGreater(K key)
        {
            if (root == null)
                return -1;

            var res = GetIndexOfKeyOrGreater(root, Size, key);
            if (res == -1)
                return -1;

            // Inner method, GetIndexOfKeyOrGreater, returns offset from right-hand side of tree
            return Size - 1 - res;
        }

        /// <summary>
        /// Adds a key to the tree. If the key already exists, throws an exception.
        /// </summary>
        /// <param name="key">The key to add to the tree.</param>
        public void AddKey(K key)
        {
            root = InsertKey(root, size, key);
            size++;
        }

        /// <summary>
        /// Removes the specified key from the collection.
        /// </summary>
        /// <remarks>If the specified key does not exist in the collection, an exception is thrown.
        /// <param name="key">The key to remove from the collection.</param>
        public void RemoveKey(K key)
        {
            if (root == null)
                ThrowKeyNotFoundError(key);
            root = DeleteKey(root, size, key);
            size--;
        }

        private IndexTreeNode? DeleteKey(IndexTreeNode root, int nodesInSubtree, K key)
        {
            int nodesInLeftChild = (nodesInSubtree - 1) / 2;
            int nodesInRightChild = nodesInLeftChild + 1 - nodesInSubtree % 2;
            bool skew = nodesInSubtree % 2 == 0;

            var cmp = key.CompareTo(root.Key);
            if (cmp == 0)
            {
                if (root.Right != null)
                {
                    if (skew)
                    {
                        root.Key = GetLowestKey(root.Right);
                        root.Right = DeleteKey(root.Right, nodesInRightChild, root.Key);
                    }
                    else
                    {
                        root.Key = GetHighestKey(root.Left!);
                        root.Left = DeleteKey(root.Left!, nodesInLeftChild, root.Key);
                    }
                }
                else
                {
                    // leaf node
                    if (root.Left != null)
                        ThrowInconsistentStateError();

                    return null;
                }
                return root;
            }

            if (cmp < 0)
            {
                if (root.Left == null)
                {
                    ThrowKeyNotFoundError(key);
                }

                if (skew)
                {
                    if (root.Right == null)
                        ThrowInconsistentStateError();

                    IndexTreeNode? previousNode = ShiftKeysGreaterThanToTheLeft(root.Left, key);
                    if (previousNode == null)
                        ThrowKeyNotFoundError(key);
                    previousNode.Key = root.Key;
                    root.Key = GetLowestKey(root.Right);
                    root.Right = DeleteKey(root.Right, nodesInRightChild, root.Key);
                }
                else
                {
                    root.Left = DeleteKey(root.Left, nodesInLeftChild, key);
                }
            }
            else 
            {
                // key > root.Key

                if (skew)
                {
                    if (root.Right == null)
                        ThrowInconsistentStateError();

                    root.Right = DeleteKey(root.Right, nodesInRightChild, key);
                }
                else
                {
                    if (root.Right == null)
                        ThrowKeyNotFoundError(key);

                    IndexTreeNode? previousNode = ShiftKeysSmallerThanToTheRight(root.Right, key);
                    if (previousNode == null)
                        ThrowKeyNotFoundError(key);
                    previousNode.Key = root.Key;
                    if (root.Left == null)
                        ThrowInconsistentStateError();
                    root.Key = GetHighestKey(root.Left);
                    root.Left = DeleteKey(root.Left, nodesInLeftChild, root.Key);                    
                }
            }

            return root;
        }

        private IndexTreeNode? InsertKey(IndexTreeNode? root, int nodesInSubtree, K key)
        {
            if (root == null)
            {
                IndexTreeNode newNode = new()
                {
                    Key = key,
                };
                return newNode;
            }

            int nodesInLeftChild = (nodesInSubtree - 1) / 2;
            int nodesInRightChild = nodesInLeftChild + (1 - nodesInSubtree % 2);
            bool skew = nodesInSubtree % 2 == 0;

            var cmp = key.CompareTo(root.Key);

            if (cmp == 0)
                ThrowDuplicateKeyError(key);

            if (cmp < 0)
            {
                if (skew)
                {
                    root.Left = InsertKey(root.Left, nodesInLeftChild, key);
                }
                else
                {
                    root.Right = InsertKey(root.Right, nodesInRightChild, root.Key);

                    if (root.Left == null)
                    {
                        root.Key = key;
                    }
                    else
                    {
                        var highLeft = GetHighestKey(root.Left);
                        if (key.CompareTo(highLeft) > 0)
                        {
                            root.Key = key;
                        }
                        else
                        {
                            root.Key = highLeft;
                            ShiftKeysToTheRightAndInsert(root.Left, key);
                        }
                    }
                }
            }
            else
            {
                // key > root.Key1
                if (!skew)
                {
                    root.Right = InsertKey(root.Right, nodesInRightChild, key);
                }
                else
                {
                    if (root.Right == null)
                        ThrowInconsistentStateError();

                    root.Left = InsertKey(root.Left, nodesInLeftChild, root.Key);

                    var lowRight = GetLowestKey(root.Right);
                    if (key.CompareTo(lowRight) < 0)
                    {
                        root.Key = key;
                    }
                    else
                    {
                        root.Key = lowRight;
                        ShiftKeysToTheLeftAndInsert(root.Right, key);
                    }
                }
            }
            return root;
        }

        private static int GetIndexOfKeyOrSmaller(IndexTreeNode node, int nodesInSubtree, K key)
        {
            int leftSkip = 0;
            do
            {
                int nodesInLeftSubtree = (nodesInSubtree - 1) / 2;
                var cmp1 = key.CompareTo(node.Key);
                if (cmp1 == 0)
                {
                    return leftSkip + nodesInLeftSubtree;
                }
                if (cmp1 < 0)
                {
                    if (node.Left == null)
                        return leftSkip - 1;
                    node = node.Left;
                    nodesInSubtree = nodesInLeftSubtree;
                }
                else
                {
                    if (node.Right == null)
                        return leftSkip + nodesInLeftSubtree;
                    node = node.Right;
                    leftSkip += nodesInLeftSubtree + 1;
                    nodesInSubtree -= nodesInLeftSubtree + 1;
                }
            } while (true);
        }

        private static int GetIndexOfKeyOrGreater(IndexTreeNode node, int nodesInSubtree, K key)
        {
            int rightSkip = 0;
            do
            {
                int nodesInRightSubtree = (nodesInSubtree - 1) / 2 + 1 - nodesInSubtree % 2;
                var cmp1 = key.CompareTo(node.Key);
                if (cmp1 == 0)
                {
                    return rightSkip + nodesInRightSubtree;
                }
                if (cmp1 > 0)
                {
                    if (node.Right == null)
                        return rightSkip - 1;
                    node = node.Right;
                    nodesInSubtree = nodesInRightSubtree;
                }
                else
                {
                    if (node.Left == null)
                        return rightSkip + nodesInRightSubtree;
                    node = node.Left;
                    rightSkip += nodesInRightSubtree + 1;
                    nodesInSubtree -= nodesInRightSubtree + 1;
                }
            } while (true);
        }

        private static int GetIndexOfKey(IndexTreeNode node, int nodesInSubtree, K key)
        {
            int leftSkip = 0;
            do
            {
                int nodesInLeftSubtree = (nodesInSubtree - 1) / 2;
                var cmp1 = key.CompareTo(node.Key);
                if (cmp1 == 0)
                {
                    return leftSkip + nodesInLeftSubtree;
                }
                if (cmp1 < 0)
                {
                    if (node.Left == null)
                        return -1;
                    node = node.Left;
                    nodesInSubtree = nodesInLeftSubtree;
                }
                else
                {
                    if (node.Right == null)
                        return -1;
                    node = node.Right;
                    leftSkip += nodesInLeftSubtree + 1;
                    nodesInSubtree -= nodesInLeftSubtree + 1;
                }
            } while (true);
        }

        private static bool ContainsKey(IndexTreeNode node, K key)
        {
            do
            {
                var cmp1 = key.CompareTo(node.Key);
                if (cmp1 == 0)
                {
                    return true;
                }
                if (cmp1 < 0)
                {
                    if (node.Left == null)
                        return false;
                    node = node.Left;
                }
                else
                {
                    if (node.Right == null)
                        return false;
                    node = node.Right;
                }
            } while (true);
        }

        private static IndexTreeNode? SubtreeFromArrayRange(K[] keys, int left, int right, int length)
        {
            int mid = (left + right) / 2;
            IndexTreeNode result = new()
            {
                Key = keys[mid],
            };
            if (length > 1)
            {
                int nodesInLeftChild = (length - 1) / 2;
                if (nodesInLeftChild > 0)
                {
                    result.Left = SubtreeFromArrayRange(keys, left, mid - 1, nodesInLeftChild);
                }
                int nodesInRightChild = nodesInLeftChild + (1 - length % 2);
                if (nodesInRightChild > 0)
                {
                    result.Right = SubtreeFromArrayRange(keys, mid + 1, right, nodesInRightChild);
                }
            }
            return result;
        }

        private void ShiftKeysToTheLeftAndInsert(IndexTreeNode node, K keyToInsert)
        {
            IndexTreeNode? prev = null;
            IndexTreeNode? curr = node;

            while (curr != null)
            {
                walkingStack.Push(curr);
                curr = curr.Left;
            }

            while (walkingStack.Count > 0)
            {
                curr = walkingStack.Pop();

                if (prev != null)
                {
                    var cmp = keyToInsert.CompareTo(curr.Key);
                    if (cmp == 0)
                        ThrowDuplicateKeyError(keyToInsert);

                    if (cmp > 0)
                    {
                        prev.Key = curr.Key;
                    }
                    else
                    {
                        prev.Key = keyToInsert;
                        walkingStack.Clear();
                        return;
                    }
                }

                prev = curr;

                var right = curr.Right;
                if (right != null)
                {
                    curr = right;
                    while (curr != null)
                    {
                        walkingStack.Push(curr);
                        curr = curr.Left;
                    }
                }
            }

            walkingStack.Clear();
            prev!.Key = keyToInsert;
        }

        private void ShiftKeysToTheRightAndInsert(IndexTreeNode node, K keyToInsert)
        {
            IndexTreeNode? prev = null;
            IndexTreeNode? curr = node;

            while (curr != null)
            {
                walkingStack.Push(curr);
                curr = curr.Right;
            }

            while (walkingStack.Count > 0)
            {
                curr = walkingStack.Pop();

                if (prev != null)
                {
                    var cmp = keyToInsert.CompareTo(curr.Key);
                    if (cmp == 0)
                        ThrowDuplicateKeyError(keyToInsert);

                    if (cmp < 0)
                    {
                        prev.Key = curr.Key;
                    }
                    else
                    {
                        prev.Key = keyToInsert;
                        walkingStack.Clear();
                        return;
                    }
                }

                prev = curr;

                var left = curr.Left;
                if (left != null)
                {
                    curr = left;
                    while (curr != null)
                    {
                        walkingStack.Push(curr);
                        curr = curr.Right;
                    }
                }
            }

            walkingStack.Clear();
            prev!.Key = keyToInsert;
        }

        private IndexTreeNode? ShiftKeysGreaterThanToTheLeft(IndexTreeNode node, K key)
        {
            IndexTreeNode? curr = node;
            IndexTreeNode? match = null;
            while (walkingStack.Count > 0 || curr != null)
            {
                while (curr != null)
                {
                    walkingStack.Push(curr);
                    if (curr.Key.CompareTo(key) < 0)
                        break;
                    curr = curr.Left;
                }
                curr = walkingStack.Pop();
                if (match != null)
                {
                    match.Key = curr.Key;
                    match = curr;
                }
                else
                {
                    if (curr.Key.Equals(key))
                        match = curr;
                }
                curr = curr.Right;
            }
            walkingStack.Clear();
            return match;
        }

        private IndexTreeNode? ShiftKeysSmallerThanToTheRight(IndexTreeNode node, K key)
        {
            IndexTreeNode? curr = node;
            IndexTreeNode? match = null;
            while (walkingStack.Count > 0 || curr != null)
            {
                while (curr != null)
                {
                    walkingStack.Push(curr);
                    if (curr.Key.CompareTo(key) > 0)
                        break;
                    curr = curr.Right;
                }
                curr = walkingStack.Pop();
                if (match != null)
                {
                    match.Key = curr.Key;
                    match = curr;
                }
                else
                {
                    if (curr.Key.Equals(key))
                        match = curr;
                }
                curr = curr.Left;
            }
            walkingStack.Clear();
            return match;
        }

        private static K GetHighestKey(IndexTreeNode subtree)
        {
            while (subtree.Right != null)
            {
                subtree = subtree.Right;
            }
            return subtree.Key;
        }

        private static K GetLowestKey(IndexTreeNode subtree)
        {
            while (subtree.Left != null)
            {
                subtree = subtree.Left;
            }
            return subtree.Key;
        }

        [DoesNotReturn]
        private static void ThrowInconsistentStateError()
        {
            throw new InvalidOperationException("Inconsistent tree state.");
        }

        [DoesNotReturn]
        private static void ThrowKeyNotFoundError(K key)
        {
            throw new ArgumentException($"Key {key} not found in the tree.");
        }

        [DoesNotReturn]
        private static void ThrowDuplicateKeyError(K key)
        {
            throw new System.ArgumentException($"Key {key} already exists in the tree.");
        }
    }
}
