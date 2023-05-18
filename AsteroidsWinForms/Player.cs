using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Asteroids {
    class Player : GameObject {

        // Core variables for player
        private double rotation;
        private Stopwatch timeSinceLastShot = new Stopwatch();
        public List<Shot> shots = new List<Shot>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <param name="compEcra"></param>
        /// <param name="altEcra"></param>
        /// <param name="tamanho"></param>
        /// <param name="rotation"></param>
        /// <param name="velocidade"></param>
        public Player(float posX, float posY, int compEcra, int altEcra, double tamanho, double rotation = -Math.PI/2, double velocidade = 0) : base(posX, posY, velocidade, 0, tamanho, compEcra, altEcra) {
            this.rotation = rotation;
            Vertices = GenerateVertices();
            timeSinceLastShot.Start();
        }

        /// <summary>
        /// Function that defines all vertices
        /// </summary>
        /// <returns></returns>
        private PointF[] GenerateVertices() {
            PointF[] vertices = new PointF[3];

            vertices[0] = new PointF((float)(PosX + Math.Cos(rotation) * Size), (float)(PosY + Math.Sin(rotation) * Size));
            vertices[1] = new PointF((float)(PosX + (Math.Cos(rotation + (4*Math.PI/5)) * Size)), (float)(PosY + (Math.Sin(rotation + (5*Math.PI/6)) * Size)));
            vertices[2] = new PointF((float)(PosX + (Math.Cos(rotation + (6*Math.PI/5)) * Size)), (float)(PosY + (Math.Sin(rotation + (7*Math.PI/6)) * Size)));

            return vertices;
        }

        /// <summary>
        /// Function that calculates new polygon position
        /// </summary>
        public void UpdatePosition() {
            PosX += (float)(Velocity * Math.Cos(rotation));
            PosY += (float)(Velocity * Math.Sin(rotation));

            if(PosX > ScreenWidth+30) {
                PosX = -30;
            }
            if(PosX < -30) {
                PosX = ScreenWidth+30;
            }
            if(PosY > ScreenHeight+30) {
                PosY = -30;
            }
            if(PosY < -30) {
                PosY = ScreenHeight+30;
            }

            Vertices = GenerateVertices();
        }

        /// <summary>
        /// Function that control all shots and checks if they passed their life-span
        /// </summary>
        public void ControlShots() {
            for(int i = 0; i < shots.Count; i++) {
                if(shots[i].aliveTime.ElapsedMilliseconds > 3000) {
                    shots.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Keep track of keyboard through windows
        /// </summary>
        /// <param name="virtKey"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public extern static short GetKeyState(short virtKey);

        /// <summary>
        /// Function that returns the state of a key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static bool IsKeyDown(Keys key) {
            return (GetKeyState(Convert.ToInt16(key)) & 0X80) == 0X80;
        }

        /// <summary>
        /// Function that processes all keyboard info
        /// </summary>
        public void ProcessInput() {
            double angularVelocity = 0;
            // If left or right arrow is pressed, players angular velocity rises
            if(IsKeyDown(Keys.Right) && !IsKeyDown(Keys.Left)) angularVelocity = Math.PI/19;
            else if(IsKeyDown(Keys.Left) && !IsKeyDown(Keys.Right)) angularVelocity = -Math.PI/19;

            // If up arrow or down arrow are pressed, player velocity gets gradually higher or smaller
            if(IsKeyDown(Keys.Up) && !IsKeyDown(Keys.Down)) {
                if(Velocity < 10) Velocity += 1;
            } else if(IsKeyDown(Keys.Down) && !IsKeyDown(Keys.Up)) {
                if(Velocity > -10) Velocity -= 1;
            } else {
                if(Velocity < 0) Velocity += 1;
                if(Velocity > 0) Velocity -= 1;
            }

            // If space is pressed, verify if player can shoot again
            if(IsKeyDown(Keys.Space)) {
                if(timeSinceLastShot.ElapsedMilliseconds > 750) {
                    Shoot();
                    timeSinceLastShot.Restart();
                }
            }

            rotation += angularVelocity;
        }

        /// <summary>
        /// Function that simulates a shot
        /// </summary>
        public void Shoot() {
            shots.Add(new Shot((int)Vertices[0].X, (int)Vertices[0].Y, ScreenWidth, ScreenHeight, 11, rotation, 8));
        }
    }
}
