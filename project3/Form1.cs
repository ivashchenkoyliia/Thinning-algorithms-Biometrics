using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace project3
{
    public partial class Form1 : Form
    {
        Bitmap c;
        public Form1()
        {
            InitializeComponent();
            c = new Bitmap(pictureBox1.BackgroundImage);
        }


        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        int[,] deleteTable = {
                           { 3, 5, 7, 12, 13, 14, 15, 20 },
                           { 21, 22, 23, 28, 29, 30, 31, 48 }, //delete table. If summary from pixels = any record from 
                           { 52, 53, 54, 55, 56, 60, 61, 62 }, //that table, pixel that is "4" = "0";
                           { 63, 65, 67, 69, 71, 77, 79, 80 },
                           { 81, 83, 84, 85, 86, 87, 88, 89 },
                           { 91, 92,93, 94, 95, 97, 99, 101 },
                           { 103, 109, 111, 112, 113, 115, 116, 117 },
                           { 118, 119, 120, 121, 123, 124, 125, 126 },
                           { 127, 131, 133, 135, 141, 143, 149, 151 },
                           { 157, 159, 181, 183, 189, 191, 192, 193 },
                           { 195, 197, 199, 205, 207, 208, 209, 211 },
                           { 212, 213, 214, 215, 216, 217, 219, 220 },
                           { 221, 222, 223, 224, 225, 227, 229, 231 },
                           { 237, 239, 240, 241, 243, 244, 245, 246 },
                           { 247, 248, 249, 251, 252, 253, 254, 255 }
                         };

        int[,] compareTable = {
                            { 128, 1, 2 },
                            { 64, 0, 4 }, // 0 is a middle pixel, the rest are neighbourhood
                            { 32, 16, 8 } //of this pixel
                          };
        int[,] delT0 = {
            { 3, 6, 7, 12, 14, 15, 24, 28 },
            { 30, 31, 48, 56, 60, 62, 63, 96 },
            { 112, 120, 124, 126, 127, 129, 131, 135 },
            { 143, 159, 191, 192, 193, 195, 199, 207 },
            { 223, 224, 225, 227, 231, 239, 240, 241 },
            { 243, 247, 248, 249, 251, 252, 253, 254 } };
        int[,] delT1 = { { 7, 14, 28, 56 },{ 112, 131, 193, 224 } };
        int[,] delT2 = { 
            {7, 14, 15, 28, 30, 56, 60, 112 },
            { 120, 131, 135, 193, 195, 224, 225, 240 } };
        int[,] delT3 = { { 7, 14, 15, 28, 30, 31, 56, 60 },
            { 62, 112, 120, 124, 131, 135, 143, 193 },
            { 195, 199, 224, 225, 227, 240, 241, 248 } };
        int[,] delT4 = { { 7, 14, 15, 28, 30, 31, 56, 60 },
            { 62, 63, 112, 120, 124, 126, 131, 135 },
            { 143, 159, 193, 195, 199, 207, 224, 225 },
            { 227, 231, 240, 241, 243, 248, 249, 252 } };
        int[,] delT5 = { { 7, 14, 15, 28 }, {30, 31, 56, 60 },
            { 62, 63, 112, 120 }, {124, 126, 131, 135 },
            { 143, 159, 191, 193 }, {195, 199, 207, 224 },
            { 225, 227, 231, 239 },{ 240, 241, 243, 248 },
            { 249, 251, 252, 254 } };
        int t;
        public int FindEdges(int[,] pixelArray, int compareSize, int x, int y)
        {
            int yArray, xArray, maskY, maskX;
            int check = 0;
            for (maskY = 0; maskY < compareSize; maskY++)
            {
                for (maskX = 0; maskX < compareSize; maskX++)
                {
                    yArray = (y + maskY - 1);
                    if (yArray < 0)
                        yArray = 0;
                    if (yArray > c.Height - 1)
                        yArray = c.Height - 1;

                    xArray = (x + maskX - 1);
                    if (xArray < 0)
                        xArray = 0;
                    if (xArray > c.Width - 1)
                        xArray = c.Width - 1;

                    if (pixelArray[yArray, xArray] == 0) //countnig whites that pixel is stick to
                        check++;
                }
            }
            if (check == 6 | check == 7 | check == 0) //if check == 6 or 7 it is probably the one pixel wide line, 0 = pixel inside wider line
                return pixelArray[y, x]; //we are returning 1 as black pixel
            return 2; //if check is diffrent than 6, 7 or 0 it is probably the edge pixel
        }

        public int FindStickEdges(int[,] pixelArray, int compareSize, int x, int y)
        {
            int yArray, xArray, maskY, maskX;
            for (maskY = 0; maskY < compareSize; maskY++)
            {
                for (maskX = 0; maskX < compareSize; maskX++)
                {
                    yArray = (y + maskY - 1);
                    if (yArray < 0)
                        yArray = 0;
                    if (yArray > c.Height - 1)
                        yArray = c.Height - 1;

                    xArray = (x + maskX - 1);
                    if (xArray < 0)
                        xArray = 0;
                    if (xArray > c.Width - 1)
                        xArray = c.Width - 1;

                    if ((maskY == 0 & maskX == 1) | (maskY == 1 & maskX == 0) | (maskY == 2 & maskX == 1) | (maskY == 1 & maskX == 2))
                    {
                        //in this step i marked all corners on my compare mask, when pixel is stick to the BG with corners, == 3, 
                        if (pixelArray[yArray, xArray] == 0)
                            return 3;
                    }

                }
            }
            return pixelArray[y, x]; //if that pixel is not stick to any corner to white, it is still 1
        }

        public int CheckNeighbourhood(int[,] pixelArray, int compareSize, int x, int y)
        {
            int yArray, xArray, maskY, maskX;
            int check = 0;
            int sum = 0;
            for (maskY = 0; maskY < compareSize; maskY++)
            {
                for (maskX = 0; maskX < compareSize; maskX++)
                {
                    yArray = (y + maskY - 1);
                    if (yArray < 0)
                        yArray = 0;
                    if (yArray > c.Height - 1)
                        yArray = c.Height - 1;

                    xArray = (x + maskX - 1);
                    if (xArray < 0)
                        xArray = 0;
                    if (xArray > c.Width - 1)
                        xArray = c.Width - 1;

                    if (pixelArray[yArray, xArray] != 0)
                    {
                        sum += compareTable[maskY, maskX];
                        check++; //counting neighbours for that pixel
                    }
                }
            }

            if (check == 2 | check == 3 | check == 4)
                return 4;

            return pixelArray[y, x]; //if we not find any "4"
        }

        public int CheckNeighbourhoodWithDelete(int[,] pixelArray, int compareSize, int x, int y)
        {
            int yArray, xArray, maskY, maskX;
            int sum = 0;
            for (maskY = 0; maskY < compareSize; maskY++)
            {
                for (maskX = 0; maskX < compareSize; maskX++)
                {
                    yArray = (y + maskY - 1);
                    if (yArray < 0)
                        yArray = 0;
                    if (yArray > c.Height)
                        yArray = c.Height - 1;

                    xArray = (x + maskX - 1);
                    if (xArray < 0)
                        xArray = 0;
                    if (xArray > c.Width)
                        xArray = c.Width - 1;

                    if (pixelArray[yArray, xArray] != 0)
                    {
                        sum += compareTable[maskY, maskX];
                    }
                }
            }

            

            switch(t)
            {
                case 0:
                    for (int i = 0; i < 6; i++)
                        for (int j = 0; j < 8; j++)
                            if (sum == delT0[i, j])
                                return 0;
                    break;
                case 1:
                    for (int i = 0; i < 2; i++)
                        for (int j = 0; j < 4; j++)
                            if (sum == delT1[i, j])
                                return 0;
                    break;
                case 2:
                    for (int i = 0; i < 2; i++)
                        for (int j = 0; j < 8; j++)
                            if (sum == delT2[i, j])
                                return 0;
                    break;
                case 3:
                    for (int i = 0; i < 3; i++)
                        for (int j = 0; j < 8; j++)
                            if (sum == delT3[i, j])
                                return 0;
                    break;
                case 4:
                    for (int i = 0; i < 4; i++)
                        for (int j = 0; j < 8; j++)
                            if (sum == delT4[i, j])
                                return 0;
                    break;
                case 5:
                    for (int i = 0; i < 15; i++)
                        for (int j = 0; j < 8; j++)
                            if (sum == deleteTable[i, j])
                                return 0;
                    break;
                default:
                    for (int i = 0; i < 9; i++)
                     for (int j = 0; j < 4; j++)
                        if (sum == delT5[i, j])
                            return 0;
                    break;


            }
            return 1;
        }

        public void KMM(Bitmap bmp, Bitmap newImage)
        {
            int compareSize = 3; //size of compare table
            int x, y;
            Color tempPixel;
            int[,] pixelArray = new int[bmp.Height, bmp.Width]; // one record on this array = one pixel
            int N = 2;
            int test = 0;
            for (y = 0; y < bmp.Height; y++)
                for (x = 0; x < bmp.Width; x++)
                {
                    tempPixel = bmp.GetPixel(x, y);

                    if (tempPixel.R < 128) //if color of pixel is black = 1
                        pixelArray[y, x] = 1;
                    else
                        pixelArray[y, x] = 0; //if color of pixel is white = 0
                }

            for (test = 0; test < 20; test++)

            {
                for (y = 0; y < bmp.Height; y++)
                {
                    for (x = 0; x < bmp.Width; x++)
                        if (pixelArray[y, x] == 1)
                            pixelArray[y, x] = FindEdges(pixelArray, compareSize, x, y); 
                    //we are looking for edges of image here
                }

                for (y = 0; y < bmp.Height; y++)
                {
                    for (x = 0; x < bmp.Width; x++)
                        if (pixelArray[y, x] == 1)
                            pixelArray[y, x] = FindStickEdges(pixelArray, compareSize, x, y);
                    //we are looking for "1", that is sticking to "0" in corners and set it to "3"
                }

                for (y = 0; y < bmp.Height; y++)
                {
                    for (x = 0; x < bmp.Width; x++)
                        if (pixelArray[y, x] == 2)
                            pixelArray[y, x] = CheckNeighbourhood(pixelArray, compareSize, x, y);
                    //we are looking for edge pixel, that summary of neighbourhood = deleteTable
                }

                for (y = 0; y < bmp.Height; y++)
                {
                    for (x = 0; x < bmp.Width; x++)
                        if (pixelArray[y, x] == 4)
                            pixelArray[y, x] = 0;
                    //deleting all "4", setting it to "0"
                }

                while (N <= 3)
                {
                    for (y = 0; y < bmp.Height; y++)
                    {
                        for (x = 0; x < bmp.Width; x++)
                            if (pixelArray[y, x] == N)
                                pixelArray[y, x] = CheckNeighbourhoodWithDelete(pixelArray, compareSize, x, y);
                        //deleting all "2" with neighbourhood compared to deleteTable
                    }
                    N++;
                }
                N = 2;
            }

            for (y = 0; y < bmp.Height; y++)
            {
                for (x = 0; x < bmp.Width; x++)
                {
                    if (pixelArray[y, x] != 0)
                        newImage.SetPixel(x, y, Color.Black);
                    else
                        newImage.SetPixel(x, y, Color.White);
                }

            }

            pictureBox1.BackgroundImage = newImage;
        }

        public void K3M(Bitmap bmp, Bitmap newImage)
        {
            int compareSize = 3; //size of compare table
            int x, y;
            Color tempPixel;
            int[,] pixelArray = new int[bmp.Height, bmp.Width]; // one record on this array = one pixel
            int N = 2;
            int test = 0;
            for (y = 0; y < bmp.Height; y++)
                for (x = 0; x < bmp.Width; x++)
                {
                    tempPixel = bmp.GetPixel(x, y);

                    if (tempPixel.R < 128) //if color of pixel is black = 1
                        pixelArray[y, x] = 1;
                    else
                        pixelArray[y, x] = 0; //if color of pixel is white = 0
                }

            for (test = 0; test < 20; test++)

            {
                for (y = 0; y < bmp.Height; y++)
                {
                    for (x = 0; x < bmp.Width; x++)
                        if (pixelArray[y, x] == 1)
                            pixelArray[y, x] = FindEdges(pixelArray, compareSize, x, y); 
                    //we are looking for edges of image here
                }

                for (y = 0; y < bmp.Height; y++)
                {
                    for (x = 0; x < bmp.Width; x++)
                        if (pixelArray[y, x] == 1)
                            pixelArray[y, x] = FindStickEdges(pixelArray, compareSize, x, y);
                    //we are looking for "1", that is sticking to "0" in corners and set it to "3"
                }

                for (y = 0; y < bmp.Height; y++)
                {
                    for (x = 0; x < bmp.Width; x++)
                        if (pixelArray[y, x] == 2)
                            pixelArray[y, x] = CheckNeighbourhood(pixelArray, compareSize, x, y);
                    //we are looking for edge pixel, that summary of neighbourhood = deleteTable
                }

                for (y = 0; y < bmp.Height; y++)
                {
                    for (x = 0; x < bmp.Width; x++)
                        if (pixelArray[y, x] == 3)
                        {
                            t = 1;
                            pixelArray[y, x] = CheckNeighbourhoodWithDelete(pixelArray, compareSize, x, y);
                        }//delete pixels that have 3 neighbors sticking to each other
                }
                for (y = 0; y < bmp.Height; y++)
                {
                    for (x = 0; x < bmp.Width; x++)
                        if (pixelArray[y, x] == 3 && pixelArray[y,x] ==4)
                        {
                            t = 2;
                            pixelArray[y, x] = CheckNeighbourhoodWithDelete(pixelArray, compareSize, x, y);
                        }//delete pixels that have 3 or 4 neighbors sticking to each other
                }
                for (y = 0; y < bmp.Height; y++)
                {
                    for (x = 0; x < bmp.Width; x++)
                        if (pixelArray[y, x] == 3 && pixelArray[y, x] == 4 && pixelArray[y,x] ==5)
                        {
                            t = 3;
                            pixelArray[y, x] = CheckNeighbourhoodWithDelete(pixelArray, compareSize, x, y);
                        }//delete pixels that have 3,4 or 5 neighbors sticking to each other
                }
                for (y = 0; y < bmp.Height; y++)
                {
                    for (x = 0; x < bmp.Width; x++)
                        if (pixelArray[y, x] == 3 && pixelArray[y, x] == 4 && pixelArray[y, x] == 5 &&
                            pixelArray[y, x] == 6)
                        {
                            t = 4;
                            pixelArray[y, x] = CheckNeighbourhoodWithDelete(pixelArray, compareSize, x, y);
                        }//delete pixels that have 3,4,5 or 6 neighbors sticking to each other
                }
                for (y = 0; y < bmp.Height; y++)
                {
                    for (x = 0; x < bmp.Width; x++)
                        if (pixelArray[y, x] == 3 && pixelArray[y, x] == 4 && pixelArray[y, x] == 5 &&
                            pixelArray[y, x] == 6 && pixelArray[y,x] == 7)
                        {
                            t = 5;
                            pixelArray[y, x] = CheckNeighbourhoodWithDelete(pixelArray, compareSize, x, y);
                        }//delete pixels that have 3,4,5,6 or 7 neighbors sticking to each other
                }
                while (N <= 3)
                {
                    t = N;
                    for (y = 0; y < bmp.Height; y++)
                    {
                        for (x = 0; x < bmp.Width; x++)
                            if (pixelArray[y, x] == N)
                                pixelArray[y, x] = CheckNeighbourhoodWithDelete(pixelArray, compareSize, x, y);
                    }
                    N++;
                }
                N = 2;
                t = N;
            }

            for (y = 0; y < bmp.Height; y++)
            {
                for (x = 0; x < bmp.Width; x++)
                {
                    if (pixelArray[y, x] != 0)
                        newImage.SetPixel(x, y, Color.Black);
                    else
                        newImage.SetPixel(x, y, Color.White);
                }

            }

            pictureBox1.BackgroundImage = newImage;
        }
        Bitmap d1, d2;
        private void button1_Click(object sender, EventArgs e)
        {
            d1 = new Bitmap(c.Width, c.Height);
            KMM(c, d1);        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            d2 = new Bitmap(c.Width, c.Height);
            K3M(c, d2);
        }
    }
}
