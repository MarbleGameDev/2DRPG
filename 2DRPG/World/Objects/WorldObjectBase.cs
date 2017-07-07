using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace _2DRPG.World.Objects {
	public class WorldObjectBase : TexturedObject {
		public float worldX;
		public float worldY;

		public WorldObjectBase() : base() { }
		public WorldObjectBase(string textureName) : base(textureName) { }
		public WorldObjectBase(float x, float y, string textureName) : base(textureName) {
			SetWorldPosition(x, y);
		}
	
		/// <summary>
		/// Sets the position of the object in the world
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void SetWorldPosition(float x, float y) {
			worldX = x;
			worldY = y;
			arrayPosition[0] = worldX - size / 2;
			arrayPosition[3] = worldX - size / 2;
			arrayPosition[6] = worldX + size / 2;
			arrayPosition[9] = worldX + size / 2;
			arrayPosition[1] = worldY - size / 2;
			arrayPosition[10] = worldY - size / 2;
			arrayPosition[4] = worldY + size / 2;
			arrayPosition[7] = worldY + size / 2;
		}

		public void LoadSavedWorldPosition(string region, int UID) {
			RegionSave s;
			lock (SaveData.RegionData) {
				s = SaveData.RegionData[region];
				if (s == null) {
					s = new RegionSave();
					SaveData.RegionData[region] = s;
				}
			}
			if (s.objectData.ContainsKey(UID) && s.objectData[UID] != null) {
				float[] b = ((JArray)s.objectData[UID]).ToObject<float[]>();
				SetWorldPosition(b[0], b[1]);
			} else {
				s.objectData.Remove(UID);
				s.objectData.Add(UID, new float[] { worldX, worldY });

			}
		}
		public void SaveWorldPosition(string region, int UID) {
			lock (SaveData.RegionData) {
				if (SaveData.RegionData[region] == null) {
					SaveData.RegionData[region] = new RegionSave();
					if (SaveData.RegionData[region].objectData.ContainsKey(UID))
						SaveData.RegionData[region].objectData[UID] = new float[] { worldX, worldY };
					else
						SaveData.RegionData[region].objectData.Add(UID, new float[] { worldX, worldY });
				}
			}
		}
	}
}
