Unity Hexagonal Puzzle Game

Version control of the code

<p align="center">
  <img width="600px" height="388" src="https://github.com/dupitydumb/HexagonalPuzzle/assets/37872714/953929ba-e7ec-47c4-a5e8-8ee704da3860)">
</p>

![image](https://github.com/dupitydumb/HexagonalPuzzle/assets/37872714/82b98678-5dcd-4297-8b3b-782c5e7a3735)

![image](https://github.com/dupitydumb/HexagonalPuzzle/assets/37872714/4acb1ae3-00a1-43f6-856c-179bfe84ff40)

## Script: `HexagonBlock.cs`

This script defines the behavior of the hexagon blocks in the game. It includes methods for moving, rotating, and destroying the blocks.

### Dependencies

- Unity Engine
- `GridData.cs`: This script provides the data structure for the game grid.

### Usage

Attach this script to a hexagon block object in the Unity editor. You can then call its methods to manipulate the block.

```csharp
HexagonBlock block = GetComponent<HexagonBlock>();
block.Move(new Vector2(1, 0));
```
