using System;

namespace Managers
{
    public interface IScoreService
    {
        public Action<int, int> OnScoreUpdated { get; set; }
        public Action<int> OnMovesLeftUpdated { get; set; }
        public Action OnWinGame { get; set; }
        public Action OnLooseGame { get; set; }

        public void AddScore(int value);
        public int GetScoreGoal();
        public int GetCurrentScore();
        public void DecrementMovesLeft();
        public int GetMovesLeft();
        public void Init(int scoreToWin, int totalMoves);
        public void ResetScoreMultiplier();
        public void IncrementScoreMultiplier();
    }
    
    public class ScoreService : IScoreService
    {
        public Action<int, int> OnScoreUpdated { get; set; }
        public Action<int> OnMovesLeftUpdated { get; set; }
        public Action OnWinGame { get; set; }
        public Action OnLooseGame { get; set; }
        
        private int currentScore = 0;
        private int scoreMultiplier = 1;
        private int scoreToWin;
        private int currentMovesLeft = 0;

        public int GetMovesLeft()
        {
            return currentMovesLeft;
        }

        public void Init(int scoreToWin, int totalMoves)
        {
            this.scoreToWin = scoreToWin;
            this.currentMovesLeft = totalMoves;
            
        }
        
        public void AddScore(int value)
        {
            int previousScore = currentScore;
            int valueToAdd = value * scoreMultiplier;
            
            if(currentScore + valueToAdd > scoreToWin)
            {
                currentScore = scoreToWin;
                OnWinGame?.Invoke();
            }
            else
            {
                currentScore += valueToAdd;
                OnScoreUpdated?.Invoke(previousScore, currentScore);
            }
        }

        public int GetScoreGoal()
        {
            return scoreToWin;
        }

        public int GetCurrentScore()
        {
            return currentScore;
        }

        public void DecrementMovesLeft()
        {
            if(currentMovesLeft > 0)
            {
                currentMovesLeft--;
                OnMovesLeftUpdated?.Invoke(currentMovesLeft);
            }
            else
            {
                OnLooseGame?.Invoke();
            }
        }
        
        public void ResetScoreMultiplier()
        {
            scoreMultiplier = 0;
        }
        
        public void IncrementScoreMultiplier()
        {
            scoreMultiplier++;
        }
    }
}