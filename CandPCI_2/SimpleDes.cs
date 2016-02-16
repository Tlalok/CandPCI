using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandPCI_2
{
    class SimpleDes
    {
        private readonly int[] startPermutationTable =
            new int[] { 2, 6, 3, 1, 4, 8, 5, 7 }
            .Select(i => i - 1)
            .ToArray();
        private readonly int[] endPermutationTable = 
            new int[] { 4, 1, 3, 5, 7, 2, 8, 6 }
            .Select(i => i - 1)
            .ToArray();

        

        public SimpleDes()
        {

        }

        public byte[] Encrypt(string message, short key)
        {
            var blocks = Encoding.Unicode.GetBytes(message);
            return Encrypt(blocks, key);
        }

        public byte[] Encrypt(byte[] blocks, short key)
        {
            var encryptedBlocks = new byte[blocks.Length];
            var keys = new KeyGenerator().GetKeys(key);
            Parallel.For(0, blocks.Length, i =>
            {
                encryptedBlocks[i] = ProcessBlock(blocks[i], keys);
            });
            //for (var i = 0; i < blocks.Length; i++)
            //{
            //    encryptedBlocks[i] = ProcessBlock(blocks[i], keys);
            //}
            return encryptedBlocks;
        }

        public byte[] Decrypt(byte[] blocks, short key)
        {
            var decryptedBlocks = new byte[blocks.Length];
            var keys = new KeyGenerator().GetKeys(key).Reverse().ToArray();
            Parallel.For(0, blocks.Length, i =>
            {
                decryptedBlocks[i] = ProcessBlock(blocks[i], keys);
            });
            //for (var i = 0; i < blocks.Length; i++)
            //{
            //    decryptedBlocks[i] = ProcessBlock(blocks[i], keys);
            //}
            return decryptedBlocks;
        }


        private byte ProcessBlock(byte block, byte[] keys)
        {
            var encryptedBlock = StartPermutation(block);

            encryptedBlock = new Mixer().Mix(encryptedBlock, keys);

            encryptedBlock = EndPermutation(encryptedBlock);
            return encryptedBlock;
        }     

        private byte StartPermutation(byte block)
        {
            return PermutateByteBits(block, startPermutationTable);
        }

        private byte EndPermutation(byte block)
        {
            return PermutateByteBits(block, endPermutationTable);
        }

        private static byte PermutateByteBits(byte block, int[] template)
        {
            int[] bitMasks = Enumerable.Range(0, 8).Select(i => 1 << i).Reverse().ToArray();
            return (byte)PermutateBits(block, bitMasks, template);
        }

        private static int PermutateBits(int block, int[] bitMasks, int[] template)
        {
            int permutatedBlock = 0;
            for (var i = 0; i < template.Length; i++)
            {
                var bit = block & bitMasks[template[i]];
                var bitPosition = template.Length - i - 1;
                permutatedBlock = bit != 0 ? permutatedBlock | 1 << bitPosition : permutatedBlock;
            }
            return permutatedBlock;
        }

        private class Mixer
        {
            const int leftSideMask = 0xF0; // 1111 0000
            const int rightSideMask = 0xF; // 0000 1111

            public byte Mix(byte block, byte[] keys)
            {
                var result = FirstRound(block, keys[0]);
                result = SecondRound(result, keys[1]);
                return (byte)result;
            }

            private int FirstRound(int block, byte key)
            {
                var result = Round(block, key);
                return SwapLeftRigthSides(result);
            }

            private int SwapLeftRigthSides(int bock)
            {
                var rightSide = bock & rightSideMask;
                var leftSide = (bock & leftSideMask) >> 4;
                return rightSide << 4 | leftSide; 
            }

            private int SecondRound(int block, byte key)
            {
                return Round(block, key);
            }

            private int Round(int block, byte key)
            {
                var rightSide = block & rightSideMask;
                var admixture = new KeyFunction().Execute(rightSide, key);

                var leftSide = (block & leftSideMask) >> 4;
                leftSide ^= admixture;

                return leftSide << 4 | rightSide;
            }

            private class KeyFunction
            {
                private readonly int[] expandPBlock =
                   new int[] { 4, 1, 2, 3, 2, 3, 4, 1 }
                   .Select(i => i - 1)
                   .ToArray();

                private readonly int[] straightPBlock =
                    new int[] { 2, 4, 3, 1 }
                    .Select(i => i - 1)
                    .ToArray();

                public int Execute(int rightSide, short key)
                {
                    var fourBitsMasks = Enumerable.Range(0, 4).Select(i => 1 << i).Reverse().ToArray();
                    var expandedRightSide = PermutateBits(rightSide, fourBitsMasks, expandPBlock);
                    var transformedBits = expandedRightSide ^ key;
                    transformedBits = new SBlock().Compress(transformedBits);
                    return PermutateBits(transformedBits, fourBitsMasks, straightPBlock);
                }

                private class SBlock
                {
                    private readonly byte[,] leftSideTable = new byte[4, 4]
                    {
                        { 1, 0, 3, 2 },
                        { 3, 2, 1, 0 },
                        { 0, 2, 1, 3 },
                        { 3, 1, 3, 2 }
                    };

                    private readonly byte[,] rightSideTable = new byte[4, 4]
                    {
                        { 0, 1, 2, 3 },
                        { 2, 0, 1, 3 },
                        { 3, 0, 1, 0 },
                        { 2, 1, 0, 3 }
                    };

                    public byte Compress(int expandedBlock)
                    {
                        var leftSide = (expandedBlock & leftSideMask) >> 4;
                        var left2Bits = CompressLeftSide(leftSide);

                        var rightSide = expandedBlock & rightSideMask;
                        var right2Bits = CompressRightSide(rightSide);

                        var result4Bits = left2Bits << 2 | right2Bits;

                        return (byte)result4Bits;
                    }

                    private byte CompressLeftSide(int leftSide)
                    {
                        return Compress4Bits(leftSide, leftSideTable);
                    }

                    private byte CompressRightSide(int rightSide)
                    {
                        return Compress4Bits(rightSide, rightSideTable);
                    }

                    private byte Compress4Bits(int fourBits, byte[,] compressTable)
                    {
                        var marginalBitsMask = 0x9;
                        var marginalBits = fourBits & marginalBitsMask;
                        var rowIndex = marginalBits >> 2 | marginalBits & 1;

                        var threeLastBitsMask = (1 << 3) - 1;
                        var columnIndex = (fourBits & threeLastBitsMask) >> 1;
                        return compressTable[rowIndex, columnIndex];
                    }
                }

            }

        }

        private class KeyGenerator
        {
            private readonly int[] pBlockPermutations = 
                new int[] { 3, 5, 2, 7, 4, 10, 1, 9, 8, 6 }
                .Select(i => i - 1)
                .ToArray();

            private readonly int[] pBlockCompression = 
                new int[] { 6, 3, 7, 4, 8, 5, 10, 9 }
                .Select(i => i - 1)
                .ToArray();

            private readonly int[] bitMasks = Enumerable.Range(0, 10).Select(i => 1 << i).Reverse().ToArray();
                //new int[] { 512, 256, 128, 64, 32, 16, 8, 4, 2, 1 };

            public byte[] GetKeys(short key)
            {
                var keys = new byte[2];

                var permutatedKey = PermutateKey(key);
                var FirstKeyBlank = GetFirstKeyBlank(permutatedKey);
                keys[0] = (byte)CompressKey(FirstKeyBlank);
                var SecondKeyBlank = GetSecondKeyBlank(permutatedKey);
                keys[1] = (byte)CompressKey(SecondKeyBlank);

                return keys;
            }

            private int PermutateKey(short key)
            {
                return PermutateBits(key, bitMasks, pBlockPermutations);
            }

            private int CompressKey(int key)
            {
                return PermutateBits(key, bitMasks, pBlockCompression);
            }

            private int GetFirstKeyBlank(int key)
            {
                return GetKeyBlank(key, 1);
            }

            private int GetSecondKeyBlank(int key)
            {
                return GetKeyBlank(key, 3);
            }

            private int GetKeyBlank(int key, int shift)
            {
                const int leftSideMask = 0x3E0; // 11111 00000
                const int rightSideMask = 0x1F; // 00000 11111

                var leftSide = (key & leftSideMask) >> 5;
                for (var i = 0; i < shift; i++)
                    leftSide = LeftCycleShift(leftSide, 5);

                var rightSide = key & rightSideMask;
                for (var i = 0; i < shift; i++)
                    rightSide = LeftCycleShift(rightSide, 5);

                return leftSide << 5 | rightSide;
            }

            private int LeftCycleShift(int number, int length)
            {
                int firstBitMask = 1 << (length - 1);
                int numberMask = (1 << length) - 1;
                int lastBit = (number & firstBitMask) >> (length - 1);
                return (number << 1 | lastBit) & numberMask;
            }
        }
    }
}
