using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG.GUI {
	class UIBlob : UIButton {

		public Action RenderAction;
		public delegate bool clickDel(float x, float y);
		public clickDel ClickAction;

		public UIBlob(Action button) : base(button) {

		}

		public override void Render() {
			if (RenderAction != null)
				RenderAction.Invoke();
		}

		public override bool CheckClick(float x, float y) {
			if (ClickAction != null)
				return ClickAction.Invoke(x, y);
			return false;
		}
	}
}
