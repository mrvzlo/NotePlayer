using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sanford.Multimedia.Midi;

namespace SequencerDemo
{
    public class Layer
    {
        private LayerStatus status;
        private bool loop;
        private const int SPEED = 10;
        private int pos;
        private int baseOctave;
        private int length;

        private List<Note> Sequence { get; }

        public Layer(bool loop = false, int octave = 5)
        {
            pos = 0;
            status = LayerStatus.NotStarted;
            this.loop = loop;
            baseOctave = octave;
            Sequence = new List<Note>();
        }

        public void Shift(OutputDevice outputDevice)
        {
            if (Sequence == null || Sequence.Count == 0 || status == LayerStatus.Finished)
                return;
            if (status == LayerStatus.NotStarted)
            {
                status = LayerStatus.Playing;
                StartNote(pos, outputDevice);
                length = 1;
                return;
            }
             
            var note = Sequence[pos];
            if (length < note.Length)
            {
                length++;
                return;
            }

            StopNote(pos, outputDevice);
            pos++;
            if (pos == Sequence.Count)
            {
                if (!loop)
                {
                    status = LayerStatus.Finished;
                    return;
                }

                pos = 0;
            }

            StartNote(pos, outputDevice);
            length = 1;
        }

        private void StartNote(int id, OutputDevice outputDevice)
        {
            if (id < 0 || id >= Sequence.Count) return;
            var note = Sequence[id];
            if (note.Id != 0) outputDevice.Send(new ChannelMessage(ChannelCommand.NoteOn, 0, note.Id, 127));
        }

        private void StopNote(int id, OutputDevice outputDevice)
        {
            if (id < 0 || id >= Sequence.Count) return;
            var note = Sequence[id];
            if (note.Id != 0) outputDevice.Send(new ChannelMessage(ChannelCommand.NoteOff, 0, note.Id, 0));
        }

        public void AddNote(N note, double length, int octave = 0)
        {
            var octaveDif = baseOctave + octave;
            Sequence.Add(new Note((int)note + octaveDif * 12, length, SPEED));
        }
    }
}
