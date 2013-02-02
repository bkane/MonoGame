// Original code from SilverSprite Project
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

using MonoMac.OpenGL;

namespace Microsoft.Xna.Framework.Graphics
{
	struct GlyphData
    {
        public char CharacterIndex;
        public Rectangle Glyph;
        public Rectangle Cropping;
        public Vector3 Kerning;
		
		public override string ToString ()
		{
			return string.Format("CharacterIndex:{0}, Glyph:{1}, Cropping{2}, Kerning{3}",CharacterIndex,Glyph,Cropping,Kerning);
		}

    }
	
	internal class charEqualityComparer : IEqualityComparer<char>
	{
		public bool Equals (char x, char y)
		{
			return x == y;
		}	
		public int GetHashCode (char obj)
		{
			return obj.GetHashCode();
		}		
	}		
	
    public class SpriteFont
    {
        public int LineSpacing { get; set; }
        public float Spacing { get; set; }
        double size = 11;
        bool bold = false;
        bool italic = false;
        public string AssetName;
		public Texture2D _texture;
        private char? _defaultCharacter;
        internal Dictionary<char, GlyphData> characterData = new Dictionary<char, GlyphData>(new charEqualityComparer());

        public char? DefaultCharacter
        {
            get 
			{ 
				return _defaultCharacter;
			}
			
			set 
			{ 
				if ( _defaultCharacter != value )
				{
					_defaultCharacter = value;
				}
			}
        }

        public Vector2 MeasureString(string text)
        {
			/*
           	Vector2 v = Vector2.Zero;
            float xoffset=0;
            float yoffset=0;

            foreach (char c in text)
            {
                if (c == '\n')
                {
                    yoffset += LineSpacing;
                    xoffset = 0;
                    continue;
                }
                if (characterData.ContainsKey(c) == false) continue;
                GlyphData g = characterData[c];				
                xoffset += g.Kerning.Y + g.Kerning.Z + Spacing;
                float newHeight = g.Cropping.Height + yoffset;
				if ( newHeight > v.Y)
                {
                    v.Y = newHeight;
                }
                if (xoffset > v.X) v.X = xoffset;
            }
            return v;
*/

			Vector2 size = Vector2.Zero;
			size.Y = LineSpacing;
			float maxX = 0f;
			int numLines = 0;
			float z = 0f;
			bool newLine = true;

			for(int i = 0; i < text.Length; i++)
			{
				char c = text[i];

				if (c != '\r')
				{
					if (c == '\n')
					{
						size.X += Math.Max (z, 0f);
						z = 0f;
						maxX = Math.Max (size.X, maxX);
						size = Vector2.Zero; //reset
						size.Y = LineSpacing;
						newLine = true;
						numLines++;
					}
					else
					{
                		GlyphData g = characterData[c];	
						Vector3 kerning = new Vector3(g.Kerning.X, g.Kerning.Y, g.Kerning.Z);

						if (newLine)
						{
							kerning.X = Math.Max(kerning.X, 0f);
						}
						else
						{
							size.X += Spacing + z;
						}

						size.X += kerning.X + kerning.Y;

						z = kerning.Z;

						size.Y = Math.Max(size.Y, (float) g.Cropping.Height);
						newLine = false;
					}
				}
			}

			size.X += Math.Max (z, 0f);
			size.Y += numLines * LineSpacing;
			size.X = Math.Max(size.X, maxX);

			return size;

        }

        public Vector2 MeasureString(StringBuilder text)
        {
            return MeasureString(text.ToString());
		}

        public SpriteFont(Texture2D texture, List<Rectangle>glyphs, List<Rectangle>cropping, List<char>charMap, int lineSpacing, float spacing, List<Vector3>kerning, char? defaultCharacter)
        {
            _texture = texture;
            LineSpacing = lineSpacing;
            Spacing = spacing;
            _defaultCharacter = defaultCharacter;
            for (int i = 0; i < charMap.Count; i++)
            {
                GlyphData g = new GlyphData();
                g.Glyph = glyphs[i];
                g.Cropping = cropping[i];
                g.Kerning = kerning[i];
                g.CharacterIndex = charMap[i];
                characterData.Add(g.CharacterIndex, g);
            }
        }
		
        public double FontSize
        {
            get
            {
                return size;
            }
        }

        public bool Bold
        {
            get
            {
                return bold;
            }
        }

        public bool Italic
        {
            get
            {
                return italic;
            }
        }		
    }
}
