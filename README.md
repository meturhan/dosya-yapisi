# Dosya Yapısı — File Structure Utilities (Create Index)

**Author:** M. Emre TURHAN (meturhan@hotmail.com) | Student ID: 051213082  
**Year:** ~2008–2009  
**Language:** C# (.NET 2.0)  
**Platform:** Console Application  
**Solution:** Visual Studio 2005/2008 with 2 projects

## Overview

This project contains file structure utilities for working with structured binary data files. It consists of two sub-projects:

1. **C_I** (Create Index) — Builds a binary search index from a data file
2. **ConsoleApplication1** — Empty helper/stub project

The core data structures implement an indexed file access system using a binary search tree (BST) with pointer blocks, enabling efficient indexed retrieval from structured binary data files.

## Project: C_I (Create Index)

The main project that creates an index structure from a structured binary data file. It reads a format specification file and a binary index file, then performs indexed lookups.

### Architecture

The index system uses a **Binary Search Tree (BST)** where each node contains:

| Component | Type | Description |
|-----------|------|-------------|
| `value` | `int` | The key value being indexed |
| `left` | `BinaryTreeNode` | Left child (smaller values) |
| `right` | `BinaryTreeNode` | Right child (larger values) |
| `block` | `PointerBlock` | Linked list of file pointers for this key |

### Core Classes

#### IndexStructure (`IndexStructure.cs`)
- `Add(int val, long place)` — Inserts a key and its file position into the BST
- `Find(int val)` — Searches for a key and returns the associated `PointerBlock`
- `WriteIntoFile(string file)` — Serializes the entire index structure to a binary file using `BinaryFormatter`
- `ReadFromFile(string file)` — Deserializes and reconstructs the index from a binary file

#### BinaryTreeNode (`BinaryTreeNode.cs`)
- `value` — The key value stored in this node
- `left` / `right` — Child node references
- `block` — Associated `PointerBlock` containing file positions

#### PointerBlock (`PointerBlock.cs`)
- `pointerList` — Array of `long` values representing file positions (byte offsets)
- `link` — Reference to an overflow `PointerBlock` (linked list for handling many records with the same key)
- `Insert(long place)` — Adds a file pointer to the block, creating overflow blocks as needed

#### FormatFile (`FormatFile.cs`)
- `readIt(string formatFile)` — Reads a format specification file (CSV-like) and returns a `string[,]` matrix with field definitions (name, type, size, range, pattern)

### Program Flow

The `Program.cs` main method:
1. Reads the format file using `FormatFile.readIt()`
2. Loads the pre-built index using `IndexStructure.ReadFromFile()`
3. Searches the index for a specified key value using `Find()`
4. Reads matching records from the data file using `BinaryReader`
5. Displays each field value (strings and integers) for all matched records
6. Follows overflow pointer blocks (linked list) to retrieve all matching records

### Command-Line Arguments
```
C_I.exe <data_file> <format_file> <index_file> <field_name> <search_value>
```

| Argument | Description |
|----------|-------------|
| `data_file` | Path to the binary data file |
| `format_file` | Path to the format specification file |
| `index_file` | Path to the pre-built index file |
| `field_name` | Name of the field to search on |
| `search_value` | Value to search for in the index |

### Format File (Example)

The format specification is a CSV-like text file where each line defines a field:
```
Gamer,String,10,4-6,"******"
Score,Integer,5,10000-1000000
Date,String,10,"2008****##"
```

Each field has:
- `name` — Field name
- `type` — Data type (String or Integer)
- `size` — Byte size in the record
- `range` — Valid range for random values (or `y-o-k` if not applicable)
- `pattern` — Format pattern (`*` = alphanumeric, `#` = numeric, or literal characters)

## Project: ConsoleApplication1

An empty C# console application stub, likely used as a testing/scratchpad project during development.

## Index Structure Overview

```
                    Binary Search Tree
                    ┌─────────────────┐
                    │   Root Node     │
                    │   val: 42       │──── PointerBlock ───> [ptr1, ptr2, ...] ──> link ──> ...
                    │  /     \        │
                    └─────────────────┘
                    /                 \
             ┌──────────┐        ┌──────────┐
             │ Left: 17 │        │Right: 99 │
             │ ...      │        │ ...      │
             └──────────┘        └──────────┘
```

The index is serialized using .NET's `BinaryFormatter`, allowing the entire tree structure (including linked pointer blocks) to be saved to and restored from disk in one operation.

## How to Build

Open `C_I/C_I.sln` in Visual Studio 2005/2008 or later. Build and run. Target framework: .NET 2.0.

## See Also

- [qd-query-index](https://github.com/meturhan/qd-query-index) — The query/index counterpart that uses the same data structures
- [rafige-random-generator](https://github.com/meturhan/rafige-random-generator) — Generates the data files this index works with
