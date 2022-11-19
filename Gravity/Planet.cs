using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gravity {
    internal class Planet {
        public Vector position;
        public Vector velocity;
        public int mass;
        public int size;
        public SolidBrush solidBrush;
        public Planet(Vector position, Vector velocity, int mass, int size, SolidBrush solidBrush) {
            this.position = position;
            this.velocity = velocity;
            this.mass = mass;
            this.size = size;
            this.solidBrush = solidBrush;
        }
    }
}
