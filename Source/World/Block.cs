namespace MC.World
{
  public enum BlockType
  {
    Air,
    Stone,
  }

  public class Block
  {
    public BlockType Type { get; set; }
    public static Block Air { get; } = new Block(BlockType.Air);

    public Block(BlockType type)
    {
      Type = type;
    }
  }
}
