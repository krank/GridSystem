namespace GridSystem;

public class Grid
{
  private GridLevel[] _gridLevels = [];

  public int Width { get; set; }
  public int Height { get; set; }

  public GridLevel[] Levels
  {
    get => _gridLevels;
    set
    {
      value.Sort((g1, g2) => g1.LevelNum.CompareTo(g2.LevelNum));
      for (int i = 0; i < value.Length; i++)
      {
        value[i].LevelNum = i;
      }
      _gridLevels = value;
    }
  }

  public Grid() {}

  public Grid(int width, int height, int levels)
  {
    Width = width;
    Height = height;
    _gridLevels = new GridLevel[levels];

    for (int i = 0; i < _gridLevels.Length; i++)
    {
      _gridLevels[i] = new GridLevel(Width, Height, i);
    }
  }

  public GridLevel GetLevel(int n) => _gridLevels[n];
}
