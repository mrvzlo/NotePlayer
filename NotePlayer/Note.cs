using System;
using System.Runtime.InteropServices;

namespace SequencerDemo
{
    public class Note
    {
        public int Id { get; }
        public int Length { get; }
        public Modifier Mod { get; set; }

        public Note(int id = 0, double length = 0, int speed = 1, Modifier mod = Modifier.Basic)
        {
            Id = id;
            Mod = mod;
            Length = Convert.ToInt32(length * speed);
        }
    }
}
