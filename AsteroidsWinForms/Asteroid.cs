using System;
using System.Drawing;

namespace Asteroids {

    enum AsteroidType { BIG, MEDIUM, SMALL };

    class Asteroid : GameObject {

        // Core variables for an Asteroid
        public AsteroidType type;
        private double rotation = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="screenWidth"></param>
        /// <param name="screenHeight"></param>
        /// <param name="rotation"></param>
        /// <param name="size"></param>
        /// <param name="direction"></param>
        /// <param name="velocity"></param>
        /// <param name="type"></param>
        public Asteroid(float posX, float posY, int screenWidth, int screenHeight, double rotation, double size, double direction, float velocity, AsteroidType type) : base(posX, posY, velocity, direction, size, screenWidth, screenHeight) {
            Vertices = GenerateVertices();
            this.rotation = rotation;
            this.type = type;
        }

        /// <summary>
        /// Function that defines all vertices
        /// </summary>
        /// <returns></returns>
        private PointF[] GenerateVertices() {
            PointF[] vertices = new PointF[7];

            for(int i = 0; i < 7; i++) {
                vertices[i] = new PointF((float)(PosX + (Math.Cos(rotation + (2*Math.PI*i/7)) * Size)), (float)(PosY + (Math.Sin(rotation + (2*Math.PI*i/7)) * Size)));
            }

            return vertices;
        }

        /// <summary>
        /// Function that calculates new polygon position
        /// </summary>
        public void UpdatePosition() {
            PosX += (float)(Velocity * Math.Cos(Direction));
            PosY += (float)(Velocity * Math.Sin(Direction));
            if(PosX > ScreenWidth+50) {
                PosX = -50;
            } 
            if(PosX < -50) {
                PosX = ScreenWidth+50;
            }
            if(PosY > ScreenHeight+50) {
                PosY = -50;
            } 
            if(PosY < -50) {
                PosY = ScreenHeight+50;
            }

            Vertices = GenerateVertices();
        }
    }
}
