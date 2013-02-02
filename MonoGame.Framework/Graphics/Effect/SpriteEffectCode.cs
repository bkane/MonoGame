using System;
using System.Text;

namespace Microsoft.Xna.Framework.Graphics
{
	internal static class SpriteEffectCode
	{
		internal static string SpriteEffectFragmentCode() {
			
			StringBuilder sb = new StringBuilder();
			sb.Append("uniform sampler2D tex;\n");
			sb.Append("void main(){\n");
			sb.Append("gl_FragColor = gl_Color * texture2D(tex, gl_TexCoord[0].xy);}");
			return sb.ToString();
			
		}

		internal static string SpriteEffectBrownFragmentCode() {
				return "uniform sampler2D tex;\n"
				+ "void main(){\n"
				+ "vec4 current = texture2D(tex, gl_TexCoord[0].xy).rgba;\n"
				+ "vec4 color = current * gl_Color.rgba;\n"
				+ "float desaturation = 0.5;\n"
				+ "vec3 grayXfer = vec3(0.3, 0.59, 0.11);\n"
				+ "vec3 gray = vec3(dot(grayXfer, color.rgb));\n"
				+ "gl_FragColor = vec4(vec3(mix(color.rgb, gray, desaturation)), color.a);"
				+ "\n}";
		}
	}
}

