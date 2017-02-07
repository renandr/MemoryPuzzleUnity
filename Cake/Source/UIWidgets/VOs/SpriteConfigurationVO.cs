using System;
using com.goodgamestudios.warlands.uiWidgets.enums;
using UnityEngine;

namespace com.goodgamestudios.warlands.uiWidgets.vos
{
	[Serializable]
	public class SpriteConfigurationVO
	{
		[SerializeField]
		private int id;

		[SerializeField]
		private Sprite sprite;

		[SerializeField]
		private string name;

		public int Id
		{
			get { return id; }
			set { id = value; }
		}

		public Sprite Sprite
		{
			get { return sprite; }
			set{ sprite = value;}
		}

		public string Name
		{
			get
			{
				if (string.IsNullOrEmpty(name) && sprite != null)
				{
					name = CheckCurrentStringForProblems(sprite.name);
				}
				return name;
			}
			set { name = CheckCurrentStringForProblems(value); }
		}

		public SpriteProviderType Type
		{
			get
			{
				return (SpriteProviderType) (id < 0 ? Enum.Parse(typeof (SpriteProviderType),Name) : id);
			}
		}

		private static string CheckCurrentStringForProblems(string text)
		{
			if (!string.IsNullOrEmpty(text))
			{
				char[] elements = { '_', ' ', '*', '+', '-', '/', '%', '!', '"', '$', '(', ')', '=', '?', '#', ':', '.', '{', '}', '\\', '[', ']' };
				string[] splits = text.Split(elements, StringSplitOptions.RemoveEmptyEntries);

				text = "";
				foreach (string split in splits)
				{
					text += UppercaseFirst(split);
				}
			}
			
			return text;
		}

		private static string UppercaseFirst(string text)
		{
			// Check for empty string.
			if (string.IsNullOrEmpty(text))
			{
				return string.Empty;
			}
			// Return char and concat substring.
			return char.ToUpper(text[0]) + text.Substring(1);
		}
	}
}
