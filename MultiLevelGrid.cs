namespace GridSystem;

public class MultiLevelGrid
{
  private readonly Grid[] _grids = [];

  public int Width { get; private set; }
  public int Height { get; private set; }

  public Grid[] Grids
  {
    get => _grids;
    set
    {
      value.Sort((g1, g2) => g1.Level.CompareTo(g2.Level));
      for (int i = 0; i < value.Length; i++)
      {
        value[i].Level = i;
      }
    }
  }
  
  public MultiLevelGrid(int width, int height, int levels)
  {
    Width = width;
    Height = height;
    _grids = new Grid[levels];

    for (int i = 0; i < _grids.Length; i++)
    {
      _grids[i] = new Grid(Width, Height);
    }
  }

  public Grid GetGrid(int n) => _grids[n];

  // public Grid AddGrid(Grid grid)
  // {
  //   grid.Level = _grids.Count;
  //   _grids.Add(grid);
  // }

  // public void DeleteGrid(int n)
  // {
  //   if (n < 0 || n > _grids.Count) return;
  //   _grids.RemoveAt(n);
  //   for (int i = n; i < _grids.Count; i++)
  //   {
  //     _grids[i].Level--;
  //   }
  // }
}
