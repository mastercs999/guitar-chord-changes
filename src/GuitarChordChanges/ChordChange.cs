using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuitarChordChanges
{
    public class ChordChange
    {
        public string Chord1 { get; set; }
        public string Chord2 { get; set; }
        public List<ChordChangeRecord> ChordChangeRecords { get; set; }


        public ChordChange()
        {
            ChordChangeRecords = new List<ChordChangeRecord>();
        }
    }
}
