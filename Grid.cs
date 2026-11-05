namespace GridSystem;

using System.Text.Json.Serialization;

public class Grid
{
  private readonly GridTile[,] _tiles;

  [JsonPropertyName("Tiles")]
  public GridTile[] TilesAs1DArray
  {
    get
    {
      int width = _tiles.GetLength(0);
      int height = _tiles.GetLength(1);
      GridTile[] allTiles = new GridTile[width * height];
      for (int y = 0; y < height; y++)
      {
        for (int x = 0; x < width; x++)
        {
          allTiles[y * width + x] = _tiles[x, y];
        }
      }
      return allTiles;
    }
    set
    {
      foreach (GridTile tile in value)
      {
        _tiles[tile.X, tile.Y] = tile;
      }
    }
  }

  public int Width { get => _tiles.GetLength(0); }
  public int Height { get => _tiles.GetLength(1); }
  public int Level { get; set; }

  public int CostStraight { get; } = 12;
  public int CostDiagonal { get; } = 17;

  private readonly static (int x, int y)[] _offsets = [
      (0,-1), (0,1), (1,0), (-1,0),  // N-S-E-W
      (1,-1), (1,1), (-1,1), (-1,-1), // NE, SE, SW, NW
  ];

  [JsonIgnore]
  public static IReadOnlyCollection<(int x, int y)> Offsets => _offsets.AsReadOnly();

  public Grid(int width, int height)
  {
    _tiles = new GridTile[width, height];
    for (int y = 0; y < height; y++)
    {
      for (int x = 0; x < width; x++)
      {
        _tiles[x, y] = new GridTile(x, y, 1, true);
      }
    }
  }

  public GridTile? Get(int x, int y) => _tiles[x, y];

  public void Set(int x, int y, GridTile tile)
  {
    x = Math.Clamp(x, 0, Width - 1);
    y = Math.Clamp(y, 0, Height - 1);
    tile.X = x;
    tile.Y = y;
    _tiles[x, y] = tile;
  }

  public (int x, int y)[] GetAllCoords()
  {
    (int x, int y)[] coords = new (int x, int y)[Width * Height];

    for (int y = 0; y < Height; y++)
    {
      for (int x = 0; x < Width; x++)
      {
        int i = (Width * y) + x;
        coords[i] = (x, y);
      }
    }

    return coords;
  }

  public bool IsBlockedInDirection(GridTile tile, (int x, int y) offset)
  {
    int vertical = offset.y + 1;
    int horizontal = Math.Abs(offset.x - 1) + 1;

    GridTile? target = GetNeighbor(tile, offset);
    if (target == null) return true;

    GridTile? VerticalNeighbor = null;
    GridTile? horizontalNeighbor = null;

    // Offset has verticality
    if (offset.y != 0)
    {
      // Check ↕ of source to target
      VerticalNeighbor = GetNeighbor(tile, (0, offset.y));

      if (VerticalNeighbor != null)
      {
        if (IsEitherTileBlocking(tile, VerticalNeighbor, (Direction)vertical)) return true;
      }
    }

    // Offset has horizontality
    if (offset.x != 0)
    {
      // Check ↔ of source to target
      horizontalNeighbor = GetNeighbor(tile, (offset.x, 0));
      if (horizontalNeighbor != null)
      {
        if (IsEitherTileBlocking(tile, horizontalNeighbor, (Direction)horizontal)) return true;
      }
    }

    // Offset has both - is diagonal
    if (offset.x != 0 && offset.y != 0)
    {
      // Check ⤱ of source to target
      if (IsEitherTileBlocking(tile, target, (Direction)horizontal)) return true;
      if (IsEitherTileBlocking(tile, target, (Direction)vertical)) return true;

      // Check ↔ and ↕ of neighbors to target
      if (VerticalNeighbor != null && IsEitherTileBlocking(VerticalNeighbor, target, (Direction)horizontal)) return true;
      if (horizontalNeighbor != null && IsEitherTileBlocking(horizontalNeighbor, target, (Direction)vertical)) return true;
    }

    return false;
  }

  private bool IsEitherTileBlocking(GridTile from, GridTile to, Direction direction)
  {
    int numberOfDirections = Enum.GetNames<Direction>().Length;
    int oppositeDirection = ((int)direction + numberOfDirections / 2) % numberOfDirections;

    return from.Blocking[(int)direction]
      || to.Blocking[oppositeDirection];
  }

  private GridTile? GetNeighbor(GridTile tile, (int x, int y) offset)
  {
    // Get the target neighbor coord
    (int x, int y) = (tile.X + offset.x, tile.Y + offset.y);

    // Exit if coord is outside bounds
    if (x < 0 || x > Width - 1
      || y < 0 || y > Height - 1
    ) return null;

    return _tiles[x, y];
  }
}