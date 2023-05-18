using System;
using System.Drawing;
using System.Collections.Generic;

namespace Asteroids {
    public enum GameState { Running, Won, Lost }

    public class Game {
        // Core game variables
        private readonly int ScreenWidth, ScreenHeight;
        private List<Asteroid> AsteroidsList;
        private Player player;
        private int InitialAsteroids = 3;
        public int Level = 1;
        public int Score = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="screenWidth"></param>
        /// <param name="screenHeight"></param>
        public Game(int screenWidth, int screenHeight) {
            this.ScreenWidth = screenWidth;
            this.ScreenHeight = screenHeight;
        }

        /// <summary>
        /// Starts the game
        /// </summary>
        public void PlayGame() {
            // Predefined points for asteroids starting location
            Point[] spawnPoints = { new Point(50, 50), new Point(ScreenWidth-50, 50), new Point(ScreenWidth-50, ScreenHeight-50), new Point(50, ScreenHeight-50), new Point(50, ScreenHeight/2), new Point(ScreenWidth - 50, ScreenHeight/2), new Point(ScreenWidth/2, ScreenHeight-50), new Point(ScreenWidth/2, 50) };
            
            // Create asteroids with random positions
            AsteroidsList = new List<Asteroid>();
            List<int> nUsed = new List<int>();
            for(int i = 0; i < InitialAsteroids + Level; i++) {
                Random rand = new Random(DateTime.Now.Millisecond);
                int j = rand.Next(0, spawnPoints.Length);
                if(i>0)
                    while(Has(j, nUsed))
                        j = rand.Next(0, spawnPoints.Length);
                nUsed.Add(j);
                int rotation = rand.Next(-36, 36);
                int direction = rand.Next(-36, 36);
                while(5*direction%90==0)
                    direction = rand.Next(-36, 36);
                AsteroidsList.Add(new Asteroid(spawnPoints[j].X, spawnPoints[j].Y, ScreenWidth, ScreenHeight, rotation, 40, 5 * direction * Math.PI/180, 4, AsteroidType.BIG));
            }

            // Create Player
            player = new Player(ScreenWidth/2, ScreenHeight/2, ScreenWidth, ScreenHeight, 30);
        }

        /// <summary>
        /// Function that verifies collisions
        /// </summary>
        /// <returns></returns>
        public bool VerifyCollisions() {
            List<Asteroid> destroyedAsteroids = new List<Asteroid>();
            List<Shot> destroyedShots = new List<Shot>();

            //Verifica colisão entre os asteroides e tiros
            for(int i = 0; i < AsteroidsList.Count; i++) {
                for(int j = 0; j < player.shots.Count; j++) {
                    if(Colisions.VerifyCollision(AsteroidsList[i], player.shots[j])) {
                        destroyedAsteroids.Add(AsteroidsList[i]);
                        destroyedShots.Add(player.shots[j]);
                        Score += 50;
                    }
                }
            }

            // Verificar colisão entre o jogador e o asteroide
            foreach(Asteroid asteroid in AsteroidsList)
                if(Colisions.VerifyCollision(asteroid, player))
                    return true;

            // Remover os tiros e os asteroides que colidiram na verificação a cima. A remoção é efetuada aqui para não afetar outras verificações
            foreach(Shot shot in destroyedShots)
                player.shots.Remove(shot);

            foreach(Asteroid asteroid in destroyedAsteroids)
                AsteroidsList.Remove(asteroid);

            // Criação de novos asteroides no caso de um Grande ou um Médio ser destruido
            for(int i = 0; i < destroyedAsteroids.Count; i++) {
                if(destroyedAsteroids[i].type == AsteroidType.BIG) {
                    Random rand = new Random(DateTime.Now.Millisecond);
                    int rotation = rand.Next(-18, 18);
                    AsteroidsList.Add(new Asteroid(destroyedAsteroids[i].PosX, destroyedAsteroids[i].PosY, ScreenWidth, ScreenHeight, rotation * Math.PI/180, 30, destroyedAsteroids[i].Direction-Math.PI/7, 6, AsteroidType.MEDIUM));
                    AsteroidsList.Add(new Asteroid(destroyedAsteroids[i].PosX, destroyedAsteroids[i].PosY, ScreenWidth, ScreenHeight, rotation * Math.PI/180, 30, destroyedAsteroids[i].Direction+Math.PI/7, 6, AsteroidType.MEDIUM));
                } else if(destroyedAsteroids[i].type == AsteroidType.MEDIUM) {
                    Random rand = new Random(DateTime.Now.Millisecond);
                    int rotation = rand.Next(-18, 18);
                    AsteroidsList.Add(new Asteroid(destroyedAsteroids[i].PosX, destroyedAsteroids[i].PosY, ScreenWidth, ScreenHeight, rotation * Math.PI/180, 15, destroyedAsteroids[i].Direction-Math.PI/7, 9, AsteroidType.SMALL));
                    AsteroidsList.Add(new Asteroid(destroyedAsteroids[i].PosX, destroyedAsteroids[i].PosY, ScreenWidth, ScreenHeight, rotation * Math.PI/180, 15, destroyedAsteroids[i].Direction+Math.PI/7, 9, AsteroidType.SMALL));
                }
            }

            return false;
        }
        
        /// <summary>
        /// Function to keep track of all the logic in game
        /// </summary>
        /// <returns></returns>
        public GameState Logic() {
            player.ProcessInput();

            player.UpdatePosition();

            player.ControlShots();

            foreach(Shot tiro in player.shots)
                tiro.UpdatePosition();

            foreach(Asteroid asteroide in AsteroidsList)
                asteroide.UpdatePosition();

            if(VerifyCollisions())
                return GameState.Lost;
            if(AsteroidsList.Count == 0)
                return GameState.Won;

            return GameState.Running;
        }

        /// <summary>
        /// Function that gets al the drawing objects on the game
        /// </summary>
        /// <returns></returns>
        public Image GetFrame() {
            Image imagem = new Bitmap(ScreenWidth, ScreenHeight);
            Graphics graficos = Graphics.FromImage(imagem);

            foreach(Asteroid asteroide in this.AsteroidsList)
                asteroide.Draw(graficos);

            foreach(Shot tiro in player.shots)
                tiro.Draw(graficos);

            player.Draw(graficos);

            graficos.Dispose();
            return imagem;
        }

        /// <summary>
        /// Auxiliar function that allows to verify if a number belongs to a list of numbers
        /// </summary>
        /// <param name="num"></param>
        /// <param name="listNums"></param>
        /// <returns></returns>
        public bool Has(int num, List<int> listNums) {
            foreach(int Num in listNums) {
                if(num == Num)
                    return true;
            }
            return false;
        }
    }
}
