using System;
using System.Diagnostics;
using System.Drawing;

namespace Asteroids {
    class Shot : GameObject {

        // Stopwatch to keep track of how long the shot was shot.
        public Stopwatch aliveTime = new Stopwatch();

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="screenWidth"></param>
        /// <param name="screenHeight"></param>
        /// <param name="velocity"></param>
        /// <param name="direction"></param>
        /// <param name="size"></param>
        public Shot(float posX, float posY, int screenWidth, int screenHeight, double velocity, double direction, double size) : base(posX, posY, velocity, direction, size, screenWidth, screenHeight) {
            aliveTime.Start();
        }

        /// <summary>
        /// Function that generates all polygon vertices.
        /// </summary>
        /// <returns></returns>
        private PointF[] GenerateVertices() {
            PointF[] vertices = new PointF[10];

            for(int i = 0; i < 10; i++) {
                vertices[i] = new PointF((float)(PosX + (Math.Cos(2*Math.PI*i/10) * Size)), (float)(PosY + (Math.Sin(2*Math.PI*i/10) * Size)));
            }

            return vertices;
        }

        /// <summary>
        /// Function that updates polygon position
        /// </summary>
        public void UpdatePosition() {
            PosX += (float)(Velocity * Math.Cos(Direction));
            PosY += (float)(Velocity * Math.Sin(Direction));

            if(PosX > ScreenWidth+15) {
                PosX = -15;
            }
            if(PosX < -15) {
                PosX = ScreenWidth+15;
            }
            if(PosY > ScreenHeight+15) {
                PosY = -15;
            }
            if(PosY < -15) {
                PosY = ScreenHeight+15;
            }

            Vertices = GenerateVertices();
        }
    }
}
