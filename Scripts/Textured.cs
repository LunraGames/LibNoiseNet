using UnityEngine;

namespace LibNoise
{
	public enum TextureChannels
	{
		/// <summary>
		/// The luminosity of the combined RGB channels.
		/// </summary>
		Luminosity,
		Red,
		Green,
		Blue,
		Alpha
	}

	public class Textured : IModule
	{
		public Color[][] Texture { get; set; }
		public TextureChannels Channel { get; set; }
		public Color DefaultColor { get; set; }

		public float GetValue(float x, float y, float z)
		{
			if (Texture == null || Texture.Length == 0 || Texture[0].Length == 0) return CalculateValue(DefaultColor);
			 
			var pixelX = Mathf.FloorToInt(Mathf.Abs(x)) % Texture.Length;
			var pixelY = Mathf.FloorToInt(Mathf.Abs(y)) % Texture[0].Length;

			return CalculateValue(Texture[pixelX][pixelY]);
		}

		float CalculateValue(Color color)
		{
			// You may be tempted to add an error for unrecognized channel, 
			// but restrain yourself, since it will print a million errors.
			switch (Channel)
			{
				case TextureChannels.Luminosity:
					Color.RGBToHSV(color, out _, out _, out var v);
					return v;
				case TextureChannels.Red:
					return color.r;
				case TextureChannels.Green:
					return color.g;
				case TextureChannels.Blue:
					return color.b;
				case TextureChannels.Alpha:
					return color.a;
				default:
					return 0f;
			}
		}
	}
}