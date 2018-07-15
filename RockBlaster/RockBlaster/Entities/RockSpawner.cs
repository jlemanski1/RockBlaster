using System;
using System.Collections.Generic;
using System.Text;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;
using FlatRedBall.Math.Geometry;
using Microsoft.Xna.Framework;
using RockBlaster.Factories;

namespace RockBlaster.Entities
{
	public partial class RockSpawner
	{

        double lastSpawnTime;
        bool isTimeToSpawn {
            get {
                float spawnFrequency = 1 / RocksPerSecond;
                return FlatRedBall.Screens.ScreenManager.CurrentScreen.PauseAdjustedSecondsSince(lastSpawnTime) > spawnFrequency;
            }
        }

        /// <summary>
        /// Initialization logic which is execute only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
		private void CustomInitialize()
		{


		}

		private void CustomActivity()
		{
            if (isTimeToSpawn) {
                SpawnRock();
            }

            // Increase the spawnRate of the rocks to increase difficulty over time
            this.RocksPerSecond += TimeManager.SecondDifference * this.SpawnRateIncrease;
		}

		private void CustomDestroy()
		{


		}

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }


        /// <summary>
        /// Creates and spawns a rock with a random speed & velocity
        /// </summary>
        private void SpawnRock() {
            Vector3 position = GetRandomRockPosition();
            Vector3 velocity = GetRandomRockVelocity(position);

            Rock rock = RockFactory.CreateNew();
            rock.Position = position;
            rock.Velocity = velocity;

            lastSpawnTime = FlatRedBall.Screens.ScreenManager.CurrentScreen.PauseAdjustedCurrentTime;
        }


        /// <summary>
        /// Returns a vector3 with a random positoin on the edge of the screen
        /// </summary>
        private Vector3 GetRandomRockPosition() {
            // Pcik a side to spawn on (top, right, bottom, left)
            int randomSide = FlatRedBallServices.Random.Next(4);

            // Pcik a random point on the side & set values according to randomSide
            float topEdge = SpriteManager.Camera.AbsoluteTopYEdgeAt(0);
            float bottomEdge = SpriteManager.Camera.AbsoluteBottomYEdgeAt(0);
            float leftEdge = SpriteManager.Camera.AbsoluteLeftXEdgeAt(0);
            float rightEdge = SpriteManager.Camera.AbsoluteRightXEdgeAt(0);

            float minX = 0;
            float maxX = 0;
            float minY = 0;
            float maxY = 0;

            switch (randomSide) {
                case 0:     // Top
                    minX = leftEdge;
                    maxX = rightEdge;
                    minY = topEdge;
                    maxY = topEdge;
                    break;
                case 1:     // Right
                    minX = rightEdge;
                    maxX = rightEdge;
                    minY = bottomEdge;
                    maxY = topEdge;
                    break;
                case 2:     // Bottom
                    minX = leftEdge;
                    maxX = rightEdge;
                    minY = bottomEdge;
                    maxY = bottomEdge;
                    break;
                case 3:     // Left
                    minX = leftEdge;
                    maxX = leftEdge;
                    minY = bottomEdge;
                    maxY = topEdge;
                    break;
            }

            // Select random point based off min & max values, move the point offscreen
            float offScreenX = minX + (float)(FlatRedBallServices.Random.NextDouble() * (maxX - minX));
            float offScreenY = minY + (float)(FlatRedBallServices.Random.NextDouble() * (maxY - minY));

            float amountToMoveBy = 64;      // half the width of the biggest rock
            switch (randomSide) {
                case 0:     // Top
                    offScreenY += amountToMoveBy;
                    break;
                case 1:     //  Right
                    offScreenX += amountToMoveBy;
                    break;
                case 2:     // Bottom
                    offScreenY -= amountToMoveBy;
                    break;
                case 3:     // Left
                    offScreenX -= amountToMoveBy;
                    break;
            }


            return new Vector3(offScreenX, offScreenY, 0);
        }


        /// <summary>
        /// Returns the velocity that the rock should move at. All rocks will start outside the edge
        /// of the screen and will moving towards the center. A later implementation will perhaps randomize
        /// the direction of travel
        /// </summary>
        private Vector3 GetRandomRockVelocity(Vector3 position) {
            Vector3 screenCenter = new Vector3(SpriteManager.Camera.X, SpriteManager.Camera.Y, 0);

            Vector3 directionToCenter = screenCenter - position;
            directionToCenter.Normalize();

            float speed = MinVelocity + (float)(FlatRedBallServices.Random.NextDouble() * (MaxVelocity - MinVelocity));

            return speed * directionToCenter;
        }
	}
}
