using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG {
	/// <summary>
	/// Interface for rendering visuals to screen
	/// </summary>
    interface IRenderable {
		void Render();
		void ContextUpdate();
		void ContextCreated();
		void ContextDestroyed();

    }
}
