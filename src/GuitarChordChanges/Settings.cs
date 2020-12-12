using System;
using System.Collections.Generic;
using System.Text;

namespace GuitarChordChanges
{
    public class Settings
    {
        public string ChangesFile { get; set; }
        public int PracticeCount { get; set; }
        public int AverageLookback { get; set; }
        public string[] KnownChords { get; set; }
    }
}
