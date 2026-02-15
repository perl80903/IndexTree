# IndexTree

A compact, well-performing, index tree implementation based on a balanced binary search tree in C#.

## Overview

IndexTree provides an indexed container that combines the benefits of:
- Fast O(log n) search operations
- Index-based access (GetKeyAtIndex, GetIndexOfKey)
- Very limited use of recursion for better performance and reduced stack usage
- Memory efficient design with minimal overhead per node (no balance information stored)

## Features

- **AddKey(K key)**: Add a key to the tree
- **RemoveKey(K key)**: Remove a key from the tree
- **ContainsKey(K key)**: Check if a key exists
- **GetKeyAtIndex(int index)**: Retrieve the key at a specific index
- **GetIndexOfKey(K key)**: Get the index of a specific key
- **GetIndexOfKeyOrSmaller(K key)**: Find the index of the key or the largest key smaller than it
- **GetIndexOfKeyOrGreater(K key)**: Find the index of the key or the smallest key greater than it
- **InitFromArray(K[] keys)**: Initialize the tree from a sorted array

## Requirements

- .NET 10
- C# 14.0

## Usage

```csharp
using IndexTree;

// Create an index tree for integers
var tree = new IndexTree<int>();

// Add keys
tree.AddKey(10);
tree.AddKey(5);
tree.AddKey(15);

// Get key by index
int key = tree.GetKeyAtIndex(1); // Returns 10

// Get index of key
int index = tree.GetIndexOfKey(15); // Returns 2

// Check if key exists
bool exists = tree.ContainsKey(5); // Returns true

// Initialize from sorted array
var sortedKeys = new[] { 1, 2, 3, 4, 5 };
tree.InitFromArray(sortedKeys);
```

## Performance

All search operations have O(log n) time complexity due to the perfectly balanced tree structure.

## License

Public Domain
