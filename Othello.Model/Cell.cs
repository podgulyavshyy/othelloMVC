using System;

namespace Othello.Model
{
    public class Cell
    {
        public Player MarkedByPlayer { get; private set; }

        public bool IsEmpty => MarkedByPlayer == null;

        public Cell()
        {
            MarkedByPlayer = null; // Ensure MarkedByPlayer is initially set to null
        }

        internal void MarkBy(Player player)
        {
            if (!IsEmpty)
            {
                //throw new Exception("Can not mark cell second time");
            }

            MarkedByPlayer = player;
        }

        internal void Reset()
        {
            MarkedByPlayer = null; // Reset MarkedByPlayer to null when needed
        }
    }
}