﻿using System;

namespace pxdraw.updateprocessor.models
{
    class Board
    {
        private const byte leftmask =  0b00001111;
        private const byte rightmask = 0b11110000;
        private readonly int _boardSize;

        public Byte[] Bitmap { get; private set; }

        public Board(Byte[] board, int boardsize = 1000)
        {
            Bitmap = board;
            _boardSize = boardsize;
        }

        public void InsertPixel(Pixel pixel)
        {
            int offsetIndex = (pixel.X + pixel.Y * _boardSize) / 2;
            bool isLeft = ((pixel.X + pixel.Y * _boardSize) % 2 == 0);
            byte bitmask = isLeft ? leftmask : rightmask;
            byte color = (byte)(pixel.Color % 16);
            if (isLeft) color = (byte)(color << 4);

            byte b = Bitmap[offsetIndex];
            b = (byte)((b & bitmask) + color);
            Bitmap[offsetIndex] = b;
        }

        public override string ToString()
        {
            string board = "";
            for(var y = 0; y < _boardSize; y++)
            {
                for(var x = 0; x < _boardSize; x+=2)
                {
                    int offset = (x + y * _boardSize) / 2;
                    byte b = Bitmap[offset];
                    int left = (b & rightmask) >> 4;
                    int right = b & leftmask;
                    board += $"{left}, {right}, ";
                }
                board += System.Environment.NewLine;
            }
            return board;
        }

        public static Board GenerateBoard(bool random = false, int boardsize = 1000)
        {
            byte[] bitmap = new byte[(boardsize ^ 2) / 2];
            Random rng = new Random();

            if(random)
            {
                for(var i = 0; i < bitmap.Length; i++)
                {
                    bitmap[i] = (byte)rng.Next(0b1111_1111);
                }
            }

            return new Board(bitmap, boardsize: boardsize);
        }
    }
}