using System;
using Substrate;
using Substrate.Core;

// This example replaces all instances of one block ID with another in a world.
// Substrate will handle all of the lower-level headaches that can pop up, such
// as maintaining correct lighting or replacing TileEntity records for blocks
// that need them.

// For a more advanced Block Replace example, see replace.cs in NBToolkit.

namespace BlockReplace
{
    class Program
    {
        static void Main (string[] args)
        {
            /*            if (args.Length != 3) {
                            Console.WriteLine("Usage: BlockReplace <world> <before-id> <after-id>");
                            return;
                        }*/

            string dest = @"..\..\Data\1_8_7-survival-updated\"; //args[0];
            //int before = Convert.ToInt32(args[1]);
            //int after = Convert.ToInt32(args[2]);

            // Open our world
            NbtWorld world = NbtWorld.Open(dest);
            
            //This fails because In AnvilWorld.cs ,in LoadLevel(), _level.LoadTreeSafe returns false because the verify function in NBTVerifier.cs takes in an _schema (From Level.cs) and tries to compare 
            //it with the old version of the NBT structure found in the level files, where the newer versions of minecraft use https://minecraft.fandom.com/wiki/Java_Edition_level_format that
            if (world == null)
            {
                Console.WriteLine("World was found to be null");
                return;
            }
            // The chunk manager is more efficient than the block manager for
            // this purpose, since we'll inspect every block
            IBlockManager bm = world.GetBlockManager();
            AlphaBlock b = bm.GetBlock(0, 0, 0);
            Console.WriteLine(b.Info.Name);


            IChunkManager cm = world.GetChunkManager(0);
            foreach (ChunkRef chunk in cm) {
                if(chunk == null)
                {
                    Console.WriteLine("Found null chunk");
                    continue;
                }
                // You could hardcode your dimensions, but maybe some day they
                // won't always be 16.  Also the CLR is a bit stupid and has
                // trouble optimizing repeated calls to Chunk.Blocks.xx, so we
                // cache them in locals
                int xdim = chunk.Blocks.XDim;
                int ydim = chunk.Blocks.YDim;
                int zdim = chunk.Blocks.ZDim;
                // x, z, y is the most efficient order to scan blocks (not that
                // you should care about internal detail)
                for (int x = 0; x < xdim; x++) {
                    for (int z = 0; z < zdim; z++) {
                        for (int y = 0; y < ydim; y++) {

                            // Replace the block with after if it matches before
                            /*if (chunk.Blocks.GetID(x, y, z) == before) {
                                chunk.Blocks.SetData(x, y, z, 0);
                                chunk.Blocks.SetID(x, y, z, after);
                            }*/
                            int currID = chunk.Blocks.GetID(x, y, z);
                            string name = chunk.Blocks.GetInfo(x, y, z).Name;
                            bool reg = chunk.Blocks.GetInfo(x, y, z).Registered;

                            int worldY = y;
                            int worldX = chunk.X * 16 + x;
                            int worldZ = chunk.Z * 16 + z;
                            if (currID != 0 && currID != 1)
                            {
                                //Console.WriteLine("In Chunk @ {0}, {1}", chunk.X, chunk.Z);
                                Console.WriteLine("Block at {0},{1},{2} is {3}", worldX, worldY, worldZ, name);
                            }
                        }
                    }
                }

                // Save the chunk
                //cm.Save();

                Console.WriteLine("Processed Chunk {0},{1}", chunk.X, chunk.Z);
            }
        }
    }
}
