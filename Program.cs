using System;
using System.Collections.Generic;
using System.Linq;

/*
 Test Case executed:
 
 Input 1 : {2, 5, 6, 4, 10, 1, 7, 4, 3, 5, 3, 8, 2, 8, 2, 10, 9, 0}
 Output  134
 * 
 * 
 Inout 2 : { 1, 4, 7, 3, 10, 1, 7, 5, 1, 5, 3, 8, 2, 8, 2, 10, 9, 1, 10 }
 Output  : 143
 * 
 * 
 Input 3 : { 2, 5, 7, 3, 10, 1, 7, 5, 1, 4, 3, 8, 2, 8, 2, 10, 10, 10, 10 }
 Output  : 164

 */

namespace Bowling_Game_Score
{
    class Program
    {
        /// <summary>
        /// Declaring dictionary "bowlingGameScore".
        /// Key as FrameNumber
        /// Value as Number of Points earned for each frame
        /// Time Complexity of retreiving the Number of Points from each frame is O(1)
        /// </summary>
        static Dictionary<int, int> bowlingGameScore = new Dictionary<int, int>();

        //Start with frame number 1
        static int frameNumber = 1;

        // Variable to identify if rolled out pin is spare on not
        static bool isSpare = false;

        // Variable to identify if rolled out pin is strike or not.
        // [0] parameter defines strike
        // [1] parameter defines double strike
        static bool[] isStrike = new bool[] { false, false };

        //Variable to keep track of number of rolls in each frame.
        static int numberOfRollsInFrame = 1;


        /// <summary>
        /// Starting point of Bowling Application
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //Input parameter is each roll that user has rolled during the complete time frame
            int[] rolls = new int[] { 2, 5, 7, 3, 10, 1, 7, 5, 1, 4, 3, 8, 2, 8, 2, 10, 10, 10, 10 };

            // Calling method to calculate the points gathered in each roll
            Roll(rolls);

            //Calculate the sum of each frame and return the result
            int totalScore = Score();

            // Print the result on the console application
            Console.WriteLine("Total Score : " +  totalScore);
            Console.ReadLine();
        }

        /// <summary>
        /// Method to calculate the score for each frame
        /// Parameter will be number of pin down for each roll
        /// Calculate based on the rules , whether a strike, spare or a normal pin is scored
        /// </summary>
        /// <param name="numberOfPin"></param>
        private static void Roll(int[] numberOfPin) 
        {
            //Loop through each rolled chance and calculate based on the outcome
            for (int i = 0; i < numberOfPin.Length; i++)
            {
                //Condition if STRIKED in first roll of the frame
                if (numberOfPin[i] == 10 && numberOfRollsInFrame==1)
                {
                    CalculateFrameScoreForStrike(i, numberOfPin);
                }

                //Condition if SPARED in first roll of the frame
                else if (i < numberOfPin.Length-1 && numberOfPin[i] + numberOfPin[i + 1] == 10 && numberOfRollsInFrame == 1)
                {
                    CalculateFrameScoreForSpare(i, numberOfPin);

                    //In case of spare , rolling over 2 pins
                    i++;
                }

                //Condition for neither strike nor spare
                else 
                {
                    CalculateNormalRolling(i, numberOfPin); 
                }
            }
        }

        /// <summary>
        /// This method will calculate the score of the particular frame in case of STRIKE.
        /// If new frame is generated then it will add that to dictionary
        /// Update the previous frame as per STRIKE rules
        /// </summary>
        /// <param name="i"></param>
        /// <param name="numberOfPin"></param>
        private static void CalculateFrameScoreForStrike(int i, int[] numberOfPin)
        {

                    //Add new frame to the bowlingGameScore dictionary along with the score of that frame
                    bowlingGameScore.Add(frameNumber, 10);

                    //Condition in case of Double strike excluding the last bonus roll
                    if (isStrike[1] && frameNumber!=12)
                    {
                        bowlingGameScore[frameNumber - 2] = bowlingGameScore[frameNumber - 2] + 20;
                        isStrike[1] = false;
                    }

                    //Condition for Strike excluding the last bonus roll
                    else if (isStrike[0] && frameNumber!=10)
                    {
                        bowlingGameScore[frameNumber - 1] = bowlingGameScore[frameNumber - 1] + 10;
                        isStrike[1] = true;
                        isStrike[0] = false;
                    }

                    //Condition for Spare
                    else if (isSpare)
                    {
                        bowlingGameScore[frameNumber - 1] = bowlingGameScore[frameNumber - 1] + numberOfPin[i];
                        isSpare = false;
                    }
                    
                    //Redefine the strike conditions
                    isStrike[0] = true;

                    //Move to next frame
                    frameNumber++;
        }

        /// <summary>
        /// This method will calculate the score of the particular frame in case of SPARE.
        /// If new frame is generated then it will add that to dictionary
        /// Update the previous frame as per SPARE rules
        /// </summary>
        /// <param name="i"></param>
        /// <param name="numberOfPin"></param>
        private static void CalculateFrameScoreForSpare(int i, int[] numberOfPin)
        {
            //Add new frame to the bowlingGameScore dictionary along with the score of that frame
            bowlingGameScore.Add(frameNumber, 10);

            //Condition to check for strike
            if (isStrike[0] && frameNumber != 10)
            {
                bowlingGameScore[frameNumber - 1] = bowlingGameScore[frameNumber - 1] + 10;
                isStrike[0] = false;
            }

            //Condition to check for spare
            if (isSpare)
            {
                bowlingGameScore[frameNumber - 1] = bowlingGameScore[frameNumber - 1] + numberOfPin[i];
                isSpare = false;
            }

            //Redfine the spare values
            isSpare = true;
            //Moving to next frame
            frameNumber++;
        }

        /// <summary>
        /// This method will calculate the score of the particular frame in case of neither Strike nor Spare.
        /// If new frame is generated then it will add that to dictionary
        /// </summary>
        /// <param name="i"></param>
        /// <param name="numberOfPin"></param>
        private static void CalculateNormalRolling(int i, int[] numberOfPin)
        {
            //First roll in the frame
            if (numberOfRollsInFrame == 1)
            {
                //Add new frame to the bowlingGameScore dictionary along with the score of that frame
                bowlingGameScore.Add(frameNumber, numberOfPin[i]);
                numberOfRollsInFrame++;
            }
            //Second roll in the frame
            else if (numberOfRollsInFrame == 2)
            {
                //Calculate Spare Condition
                if (isSpare)
                {
                    bowlingGameScore[frameNumber - 1] = bowlingGameScore[frameNumber - 1] + numberOfPin[i];
                    isSpare = false;
                }

                //Updating the current frame with the second roll
                bowlingGameScore[frameNumber] = bowlingGameScore[frameNumber] + numberOfPin[i];

                //Calculate strike condition
                if (isStrike[0])
                {
                    bowlingGameScore[frameNumber - 1] = bowlingGameScore[frameNumber - 1] + bowlingGameScore[frameNumber];
                    isStrike[0] = false;
                }

                //Redefining the number of rolls
                numberOfRollsInFrame = 1;

                //Updating the frame number
                frameNumber++;
            }
        }

        /// <summary>
        /// Method to calculate sum of each frame
        /// LINQ is used to traverse through the dictionary and calculate the sum
        /// Time Complexity O(n)
        /// n is number of key/values in Dictionary
        /// </summary>
        /// <returns>Total Score of each frame</returns>
        private static int Score()
        {
            return bowlingGameScore.Sum(n => n.Value);
        }
    }
}






