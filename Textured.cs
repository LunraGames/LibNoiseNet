using UnityEngine;
using LunraGames;

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
		public Texture2D Texture { get; set; }
		public TextureChannels Channel { get; set; }
		public Color DefaultColor { get; set; }

		public float GetValue(float x, float y, float z)
		{
			if (Texture == null) return CalculateValue(DefaultColor);

			var pixelX = Mathf.FloorToInt(x) % Texture.width;
			var pixelY = Mathf.FloorToInt(y) % Texture.height;

			return CalculateValue(Texture.GetPixel(pixelX, pixelY));
		}

		float CalculateValue(Color color)
		{
			// You may be tempted to add an error for unrecognized channel, 
			// but restrain yourself, since it will print a million errors.
			switch (Channel)
			{
				case TextureChannels.Luminosity: return color.GetV();
				case TextureChannels.Red: return color.r;
				case TextureChannels.Green: return color.g;
				case TextureChannels.Blue: return color.b;
				case TextureChannels.Alpha: return color.a;
				default: return 0f;
			}
		}
	}
}