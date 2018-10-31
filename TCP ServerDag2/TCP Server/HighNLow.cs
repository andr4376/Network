using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Server
{
    public static class HighNLow
    {
        public static int numberToGuess = 5;

        public static int guessCount = 0;

        private static bool correctGuess = false;



        public static string EvaluateReply(string message)
        {
            if (correctGuess)
            {

                ResetCount();
                correctGuess = false;
            }

            string result = "Message sent.";
            bool isGuess = false;

            if (message.All(char.IsDigit))
            {
                guessCount++;
                isGuess = true;
                int guess;
                Int32.TryParse( message, out guess);

                if (guess < numberToGuess)
                {
                    result = "Cold....";
                }
                if (guess > numberToGuess)
                {
                    result = "HOT....";
                }
                if (guess == numberToGuess)
                {
                    result = "Correct!";
                    correctGuess = true;


                    Random rnd = new Random();
                    numberToGuess = rnd.Next(100);
                }

            }
            if (message.ToLower() == "help")
            {
                result = "Number is " + numberToGuess;
            }

            if (isGuess)
            {
                result += "           Attempts: " + guessCount;
            }
            return result;

        }


        public static void ResetCount()
        {
            guessCount = 0;
        }
    }

}
