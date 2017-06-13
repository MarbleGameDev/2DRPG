using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DRPG {
    interface IRenderObject {
		void Render();
		void ContextUpdate();
		void ContextCreated();
		void ContextDestroyed();

    }
}
