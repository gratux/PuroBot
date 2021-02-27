namespace PuroBot.SpeechSynth
{
	public class Record
	{
		public Record(RecordModes mode, double voice, Frame[] data)
		{
			Mode = mode;
			Voice = voice;
			Data = data;
		}

		public RecordModes Mode { get; }
		public double Voice { get; }
		public Frame[] Data { get; }
	}

	public enum RecordModes
	{
		ChooseRandomly,
		Trill,
		PlayInSequence
	}
}