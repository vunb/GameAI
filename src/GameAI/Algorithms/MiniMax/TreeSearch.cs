﻿using GameAI.GameInterfaces;

namespace GameAI.Algorithms.MiniMax
{
    /// <summary>
    /// A method class to select moves in games
    /// that are two-player, back-and-forth,
    /// deterministic, and zero-sum or zero-sum-tie.
    /// </summary>
    public static class TreeSearch<TGame, TMove, TPlayer>
        where TGame : TreeSearch<TGame, TMove, TPlayer>.IGame
    {
        /// <summary>
        /// An interface for Games that wish to use the MiniMax AI.
        /// </summary>
        public interface IGame :
            IDoMove<TMove>,
            IGameOver,
            ILegalMoves<TMove>,
            IUndoMove,
            ICurrentPlayer<TPlayer>,
            IScore<TPlayer>
        { }

        /// <summary>
        /// Returns the best Move found by performing
        /// a full MiniMax gamestate search.
        /// </summary>
        /// <param name="game">The gamestate from which to begin the search.</param>
        public static TMove Search(TGame game)
        {
            int bestScore = int.MinValue;
            TMove bestMove = default(TMove);
            int score;
            foreach (TMove move in game.GetLegalMoves())
            {
                game.DoMove(move);
                score = -NegaMax(game);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }
                game.UndoMove();
            }
            return bestMove;
        }

        private static int NegaMax(TGame game)
        {
            if (game.IsGameOver())
                return game.Score(game.CurrentPlayer);

            int bestScore = int.MinValue;
            int score;
            foreach (TMove move in game.GetLegalMoves())
            {
                game.DoMove(move);
                score = -NegaMax(game);
                if (score > bestScore) bestScore = score;
                game.UndoMove();
            }
            return bestScore;
        }
    }
}
