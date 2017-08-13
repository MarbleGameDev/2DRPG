using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2DRPG {
	class DictionaryConverter : JsonConverter {

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			var dictionary = (Dictionary<Input.KeyInputs, Keys>)value;

			writer.WriteStartObject();

			foreach (KeyValuePair<Input.KeyInputs, Keys> pair in dictionary) {
				writer.WritePropertyName(pair.Key.ToString());
				writer.WriteValue(pair.Value.ToString());
			}

			writer.WriteEndObject();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
			var jObject = JObject.Load(reader);

			var dict = existingValue as Dictionary<Input.KeyInputs, Keys>;

			foreach (JProperty p in jObject.Properties().ToList()) {
				Input.KeyInputs k = (Input.KeyInputs)Enum.Parse(typeof(Input.KeyInputs), p.Name);
				Keys l = (Keys)Enum.Parse(typeof(Keys), p.Value.ToString());
				if (dict.ContainsKey(k))
					dict[k] = l;
				else
					dict.Add(k, l);
			}

			return existingValue;
		}

		public override bool CanConvert(Type objectType) {
			return typeof(Dictionary<Input.KeyInputs, Keys>) == objectType;
		}
	}
}
