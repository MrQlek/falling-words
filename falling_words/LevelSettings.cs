using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace falling_words
{
    public class LevelSettings
    {
        public readonly int StartSpeed;
        public readonly int EndSpeed;
        public readonly int WordsLength;
        public readonly int GameTime;

        public LevelSettings(int startSpeed, int endSpeed, int wordsLength, int gameTime)
        {
            if(startSpeed < 100 ||
                endSpeed < 100 ||
                wordsLength < 3 ||
                gameTime < 30 ||
                startSpeed > 1000 ||
                endSpeed > 1000 ||
                wordsLength > 7 ||
                gameTime > 300 ||
                endSpeed < startSpeed)
            {
                throw new ArgumentOutOfRangeException();
            }

            StartSpeed = startSpeed;
            EndSpeed = endSpeed;
            WordsLength = wordsLength;
            GameTime = gameTime;
        }
    }
}
