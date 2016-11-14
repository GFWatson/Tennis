using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tennisTest
{
    class Program
    {
        //check for win conditions
        //can be game or set dependent on aim
        static bool checkWin(int sOne, int sTwo, int aim)
        {
            if (sOne >= aim && (sOne - sTwo) >= 2)
            {
                return true;
            }
            return false;
        }

        //point scored
        static void score(ref int x, ref int y, ref int xCurrent, ref int yCurrent, ref int xFirst, ref int yFirst, ref int xSecond, ref int ySecond, ref bool cServer, ref bool fSet, ref bool sSet)
        {
            x++;
            //check to see if game is won
            if (checkWin(x, y, 4))
            {
                xCurrent++;

                //change server
                if (cServer)
                    cServer = false;
                else
                    cServer = true;

                //check to see if set is won
                if (checkWin(xCurrent, yCurrent, 6))
                {
                    //save scores, dependent on set number
                    if (!fSet)
                    {
                        fSet = true;
                        xFirst = xCurrent;
                        yFirst = yCurrent;
                        //reset values
                        xCurrent = 0; yCurrent = 0;
                    }
                    else if (!sSet)
                    {
                        sSet = true;
                        xSecond = xCurrent;
                        ySecond = yCurrent;
                        //reset values
                        xCurrent = 0; yCurrent = 0;
                    }

                }
                x = 0; y = 0;
            }

        }

        //return values to 40 or advantage
        static void deuceSolver(ref int a, ref int b)
        {
            if (a > 3 && a < b) //advantage b
            {
                a = 3;
                b = 4;
            }
            else if (b > 3 && b < a) //advantage a
            {
                b = 3;
                a = 4;
            }
            else if (a > 3 && b > 3 && a == b) //deuce
            {
                a = 3;
                b = 3;
            }
        }

        //take number input and return with tennis scoring system
        static string convertScore(int s)
        {
            switch (s)
            {
                case 0:
                    return "0";
                case 1:
                    return "15";
                case 2:
                    return "30";
                case 3:
                    return "40";
                default:
                    return "A";
            }
        }

        static void Main(string[] args)
        {
            //vars
            string line, inputFilePath, outputFilePath, output = "";
            bool currentServerA = true, firstSet = false, secondSet = false;
            int aScore = 0, bScore = 0, aCurrentSet = 0, bCurrentSet = 0, aFirst = 0, bFirst = 0, aSecond = 0, bSecond = 0;

            //get file paths
            Console.WriteLine("Enter input file path:\n");
            inputFilePath = Console.ReadLine();
            Console.WriteLine("\nEnter output file path:\n");
            outputFilePath = Console.ReadLine();

            //open file, read line by line
            System.IO.StreamReader inputFile = new System.IO.StreamReader(inputFilePath);

            while ((line = inputFile.ReadLine()) != null)
            {
                //check each char for who scored
                foreach (char c in line)
                {
                    if (c == 'A')
                    {
                        score(ref aScore, ref bScore, ref aCurrentSet, ref bCurrentSet, ref aFirst, ref bFirst, ref aSecond, ref bSecond, ref currentServerA, ref firstSet, ref secondSet);
                    }
                    else if (c == 'B')
                    {
                        score(ref bScore, ref aScore, ref bCurrentSet, ref aCurrentSet, ref bFirst, ref aFirst, ref bSecond, ref aSecond, ref currentServerA, ref firstSet, ref secondSet);
                    }
                } //end of line loop

                //deal with deuce and advantage scores
                deuceSolver(ref aScore, ref bScore);

                //scores shown in tennis style dependent on current server
                if (currentServerA)
                {
                    if (firstSet)
                    {
                        output += (aFirst + "-" + bFirst + " ");
                    }
                    if (secondSet)
                    {
                        output += (aSecond + "-" + bSecond + " ");
                    }
                    output += (aCurrentSet + "-" +bCurrentSet + " ");
                    if(aScore != 0 || bScore != 0)
                    {
                        output += (convertScore(aScore) + "-" + convertScore(bScore));
                    }
                }
                else
                {
                    if (firstSet)
                    {
                        output += (bFirst + "-" + aFirst + " ");
                    }
                    if (secondSet)
                    {
                        output += (bSecond + "-" + aSecond + " ");
                    }
                    output += (bCurrentSet + "-" + aCurrentSet + " ");
                    if (aScore != 0 || bScore != 0)
                    {
                        output += (convertScore(bScore) + "-" + convertScore(aScore));
                    }
                }
                //add new line
                output += Environment.NewLine;
                //reset values
                firstSet = false; secondSet = false; currentServerA = true;
                aScore = 0; bScore = 0; aCurrentSet = 0; bCurrentSet = 0; aFirst = 0; bFirst = 0; aSecond = 0; bSecond = 0;

            } //end of file loop

            inputFile.Close();

            //write output to file
            System.IO.StreamWriter outputFile = new System.IO.StreamWriter(outputFilePath);
            outputFile.WriteLine(output);
            outputFile.Close();

        }
    }
}
