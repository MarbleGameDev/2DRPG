using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGL;

namespace _2DRPG {
	class RotatingTriangle : IRenderObject {
		public float[] arrayPosition = new float[] {
			0.0f, 0.0f,
			0.5f, 1.0f,
			1.0f, 0.0f
		};
		public float[] arrayColor = new float[] {
			1.0f, 0.0f, 0.0f,
			0.0f, 1.0f, 0.0f,
			0.0f, 0.0f, 1.0f
		};

		public float angle = 0;

		public void Render() {
			Gl.Rotate(angle, 0.0f, 0.0f, 1.0f);
			using (MemoryLock vertexArrayLock = new MemoryLock(arrayPosition))
			using (MemoryLock vertexColorLock = new MemoryLock(arrayColor)) {
				// Note: the use of MemoryLock objects is necessary to pin vertex arrays since they can be reallocated by GC
				// at any time between the Gl.VertexPointer execution and the Gl.DrawArrays execution

				Gl.VertexPointer(2, VertexPointerType.Float, 0, vertexArrayLock.Address);
				Gl.EnableClientState(EnableCap.VertexArray);

				Gl.ColorPointer(3, ColorPointerType.Float, 0, vertexColorLock.Address);
				Gl.EnableClientState(EnableCap.ColorArray);

				Gl.DrawArrays(PrimitiveType.Triangles, 0, 3);
			}
		}

		public void ContextUpdate() {
			angle = (angle + 0.1f) % 45.0f;
		}

		public void ContextCreated() {
		}

		public void ContextDestroyed() {
		}
	}
}
