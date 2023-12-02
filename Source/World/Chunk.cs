

using MC.Model;
using OpenTK.Graphics.ES11;
using OpenTK.Mathematics;

namespace MC.World
{
  public class Chunk
  {
    public const int CubeSize = 1;
    public const int ChunkSize = 16;
    public Block[,,] Blocks { get; set; } = new Block[ChunkSize, ChunkSize, ChunkSize];
    public Vector3 Position { get; set; }

    public Chunk(Vector3 position)
    {
      Position = position;

    }

    public static void IterateChunk(Action<int, int, int> iterateCallback)
    {
      for (var x = 0; x < ChunkSize; x++)
      {
        for (var y = 0; y < ChunkSize; y++)
        {
          for (var z = 0; z < ChunkSize; z++)
          {
            iterateCallback(x, y, z);
          }
        }
      }
    }

    public void FillBlocks()
    {
      IterateChunk((x, y, z) =>
      {
        Blocks[x, y, z] = new(y > 3 ? BlockType.Air : BlockType.Stone);
      });
    }

    private static Quad GenerateCubeQuad(CubeFace face, Vector3 position)
    {
      var x = position.X;
      var y = position.Y;
      var z = position.Z;

      return face switch
      {
        CubeFace.Front => new()
        {
          BottomLeft = (x, y, z + CubeSize),
          BottomRight = (x + CubeSize, y, z + CubeSize),
          TopRight = (x + CubeSize, y + CubeSize, z + CubeSize),
          TopLeft = (x, y + CubeSize, z + CubeSize),
        },

        CubeFace.Back => new()
        {
          BottomLeft = (x, y, z),
          BottomRight = (x + CubeSize, y, z),
          TopRight = (x + CubeSize, y + CubeSize, z),
          TopLeft = (x, y + CubeSize, z)
        },

        CubeFace.Left => new()
        {
          BottomLeft = (x, y, z),
          BottomRight = (x, y, z + CubeSize),
          TopRight = (x, y + CubeSize, z + CubeSize),
          TopLeft = (x, y + CubeSize, z)
        },

        CubeFace.Right => new()
        {
          BottomLeft = (x + CubeSize, y, z),
          BottomRight = (x + CubeSize, y, z + CubeSize),
          TopRight = (x + CubeSize, y + CubeSize, z + CubeSize),
          TopLeft = (x + CubeSize, y + CubeSize, z),
        },

        CubeFace.Bottom => new()
        {
          BottomLeft = (x, y, z),
          BottomRight = (x + CubeSize, y, z),
          TopRight = (x + CubeSize, y, z + CubeSize),
          TopLeft = (x, y, z + CubeSize)
        },

        CubeFace.Top => new()
        {
          BottomLeft = (x, y + CubeSize, z),
          BottomRight = (x + CubeSize, y + CubeSize, z),
          TopRight = (x + CubeSize, y + CubeSize, z + CubeSize),
          TopLeft = (x, y + CubeSize, z + CubeSize)
        },

        _ => throw new Exception($"Invalid face={face}")
      };
    }

    public Mesh GenerateMesh()
    {
      var mesh = new Mesh();

      IterateChunk((x, y, z) =>
      {
        var block = Blocks[x, y, z];
        if (block.Type == BlockType.Air) return;

        var leftBlock = x > 0 ? Blocks[x - 1, y, z] : Block.Air;
        var rightBlock = x < 15 ? Blocks[x + 1, y, z] : Block.Air;
        var bottomBlock = y > 0 ? Blocks[x, y - 1, z] : Block.Air;
        var topBlock = y < 15 ? Blocks[x, y + 1, z] : Block.Air;
        var backBlock = z > 0 ? Blocks[x, y, z - 1] : Block.Air;
        var frontBlock = z < 15 ? Blocks[x, y, z + 1] : Block.Air;

        var position = new Vector3(x, y, z);

        void processBlock(Block block, CubeFace face)
        {
          if (block.Type == BlockType.Air) mesh.Quads.Add(GenerateCubeQuad(face, position));
        }

        processBlock(leftBlock, CubeFace.Left);
        processBlock(rightBlock, CubeFace.Right);
        processBlock(bottomBlock, CubeFace.Bottom);
        processBlock(topBlock, CubeFace.Top);
        processBlock(backBlock, CubeFace.Back);
        processBlock(frontBlock, CubeFace.Front);
      });

      return mesh;
    }
  }
}
