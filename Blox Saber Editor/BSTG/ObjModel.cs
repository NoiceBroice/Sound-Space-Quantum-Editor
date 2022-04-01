using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using OpenTK;

namespace Blox_Saber_The_Game
{
	// Token: 0x02000013 RID: 19
	internal class ObjModel
	{
		// Token: 0x06000096 RID: 150 RVA: 0x000053E5 File Offset: 0x000035E5
		private ObjModel()
		{
		}

		// Token: 0x06000097 RID: 151 RVA: 0x0000541C File Offset: 0x0000361C
		public static ObjModel FromFile(string file)
		{
			string[] array = File.ReadAllLines(file);
			ObjModel objModel = new ObjModel();
			foreach (string text in array)
			{
				if (text.StartsWith("mtllib "))
				{
					string text2 = text.Substring(7);
					if (File.Exists(text2))
					{
						objModel.Material = ObjMtl.FromFile(text2);
					}
				}
				else if (text.StartsWith("v "))
				{
					float[] position = (from pos in text.Substring(2).Split(new char[]
					{
						' '
					}).Where(delegate(string pos)
					{
						float num;
						return float.TryParse(pos.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator), out num);
					})
					select float.Parse(pos.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator))).ToArray<float>();
					Vertex item = new Vertex(position);
					objModel._vertexes.Add(item);
				}
				else if (text.StartsWith("vt "))
				{
					float[] array2 = (from pos in text.Substring(3).Split(new char[]
					{
						' '
					}).Where(delegate(string pos)
					{
						float num;
						return float.TryParse(pos.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator), out num);
					})
					select float.Parse(pos.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator))).ToArray<float>();
					if (array2.Length >= 2)
					{
						Vector2 item2 = new Vector2(array2[0], array2[1]);
						objModel._uvs.Add(item2);
					}
				}
				else if (text.StartsWith("vn "))
				{
					float[] array3 = (from pos in text.Substring(3).Split(new char[]
					{
						' '
					}).Where(delegate(string pos)
					{
						float num;
						return float.TryParse(pos.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator), out num);
					})
					select float.Parse(pos.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator))).ToArray<float>();
					if (array3.Length == 3)
					{
						Vector3 item3 = new Vector3(array3[0], array3[1], array3[2]);
						objModel._normals.Add(item3);
					}
				}
				else if (text.StartsWith("f "))
				{
					string[] source = text.Substring(2).Split(new char[]
					{
						' '
					});
					IEnumerable<long> source2 = from fas in source.Where(delegate(string fas)
					{
						string[] array4 = fas.Split(new char[]
						{
							'/'
						});
						long num;
						return array4.Length >= 1 && long.TryParse(array4[0], out num);
					})
					select long.Parse(fas.Split(new char[]
					{
						'/'
					})[0]) - 1L;
					IEnumerable<long> source3 = from fas in source.Where(delegate(string fas)
					{
						string[] array4 = fas.Split(new char[]
						{
							'/'
						});
						long num;
						return array4.Length >= 2 && long.TryParse(array4[1], out num);
					})
					select long.Parse(fas.Split(new char[]
					{
						'/'
					})[1]) - 1L;
					IEnumerable<long> source4 = from fas in source.Where(delegate(string fas)
					{
						string[] array4 = fas.Split(new char[]
						{
							'/'
						});
						long num;
						return array4.Length == 3 && long.TryParse(array4[2], out num);
					})
					select long.Parse(fas.Split(new char[]
					{
						'/'
					})[2]) - 1L;
					Face item4 = new Face(source2.ToArray<long>(), source3.ToArray<long>(), source4.ToArray<long>());
					objModel._faces.Add(item4);
				}
			}
			return objModel;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x0000578C File Offset: 0x0000398C
		public float[] GetVertexes()
		{
			float[] array = new float[this._faces.Count * 3 * 3];
			int num = 0;
			foreach (Face face in this._faces)
			{
				for (int i = 0; i < face.Indices.Length; i++)
				{
					long num2 = face.Indices[i];
					Vertex vertex = this._vertexes[(int)num2];
					for (int j = 0; j < vertex.Positions.Length; j++)
					{
						array[num++] = vertex.Positions[j];
					}
				}
			}
			return array;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x0000584C File Offset: 0x00003A4C
		public float[] GetNormals()
		{
			float[] array = new float[this._faces.Count * 3 * 3];
			int num = 0;
			foreach (Face face in this._faces)
			{
				for (int i = 0; i < face.Normals.Length; i++)
				{
					long num2 = face.Normals[i];
					Vector3 vector = this._normals[(int)num2];
					for (int j = 0; j < 3; j++)
					{
						array[num++] = vector[j];
					}
				}
			}
			return array;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00005904 File Offset: 0x00003B04
		public float[] GetUVs()
		{
			float[] array = new float[this._faces.Count * 3 * 2];
			int num = 0;
			foreach (Face face in this._faces)
			{
				for (int i = 0; i < face.UVs.Length; i++)
				{
					long num2 = face.UVs[i];
					Vector2 vector = this._uvs[(int)num2];
					for (int j = 0; j < 2; j++)
					{
						array[num++] = vector[j];
					}
				}
			}
			return array;
		}

		// Token: 0x0400005B RID: 91
		private readonly List<Face> _faces = new List<Face>();

		// Token: 0x0400005C RID: 92
		private readonly List<Vertex> _vertexes = new List<Vertex>();

		// Token: 0x0400005D RID: 93
		private readonly List<Vector2> _uvs = new List<Vector2>();

		// Token: 0x0400005E RID: 94
		private readonly List<Vector3> _normals = new List<Vector3>();

		// Token: 0x0400005F RID: 95
		public ObjMtl Material;
	}
}
