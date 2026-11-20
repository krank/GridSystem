namespace GridSystem;

public class GridTile(
  int x, int y, int height, bool hasFloor, GridLevel parentGridLevel,
  bool blockN = false,
  bool blockE = false,
  bool blockS = false,
  bool blockW = false
  )
{
  public int X { get; set; } = x;
  public int Y { get; set; } = y;
  public int Height { get; set; } = height;
  public bool HasFloor { get; set; } = hasFloor;
  public GridLevel GridLevel = parentGridLevel;

  public bool[] Blocking { get; set; } = [blockN, blockE, blockS, blockW];

  public GridTile() : this(x: 0, y: 0, height: 1, hasFloor: true, null) { }

  public override string ToString()
  {
    return $"Tile at [x:{X} y:{Y}], " +
      "walls [" +
        $"{(Blocking[(int)Direction.North] ? "N" : "")}" +
        $"{(Blocking[(int)Direction.East] ? "E" : "")}" +
        $"{(Blocking[(int)Direction.South] ? "S" : "")}" +
        $"{(Blocking[(int)Direction.West] ? "W" : "")}" +
      "]";
  }
}