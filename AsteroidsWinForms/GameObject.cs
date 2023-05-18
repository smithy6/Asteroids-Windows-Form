using System.Drawing;

namespace Asteroids {
    public abstract class GameObject {

        // Variables common in all GameObjects
        protected double Velocity, Size;
        public double Direction;
        protected int ScreenWidth, ScreenHeight;

        // Properties for object position and vertices
        public float PosX { get; protected set; }
        public float PosY { get; protected set; }
        public PointF[] Vertices { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="velocity"></param>
        /// <param name="direction"></param>
        /// <param name="size"></param>
        protected GameObject(float posX, float posY, double velocity, double direction, double size, int screenWidth, int screenHeight) {
            PosX = posX;
            PosY = posY;
            Velocity = velocity;
            Direction = direction;
            Size = size;
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
        }
        
        /// <summary>
        /// Drawing function that draws objects in the PictureBox
        /// </summary>
        /// <param name="graphics"></param>
        public void Draw(Graphics graphics) {
            graphics.DrawPolygon(Pens.White, Vertices);
        }
    }
}