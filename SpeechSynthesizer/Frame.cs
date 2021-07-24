namespace SpeechSynthesizer
{
	public class Frame
	{
		public Frame(double gain, double[] coefficients)
		{
			Gain = gain;
			Coefficients = coefficients;
		}

		public double Gain { get; }
		public double[] Coefficients { get; }
	}
}