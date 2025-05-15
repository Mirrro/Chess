using System.Collections.Generic;

namespace Gameplay.AI
{
    public class TranspositionTable
    {
        private Dictionary<ulong, TranspositionTableEntry> table = new();

        public void Store(ulong hash, TranspositionTableEntry entry)
        {
            table[hash] = entry;
        }

        public bool TryGet(ulong hash, out TranspositionTableEntry entry)
        {
            return table.TryGetValue(hash, out entry);
        }
    }
}