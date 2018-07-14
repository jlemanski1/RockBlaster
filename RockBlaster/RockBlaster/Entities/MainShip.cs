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
using RockBlaster.Factories;

namespace RockBlaster.Entities
{
	public partial class MainShip
	{

        Xbox360GamePad gamePad;

        /// <summary>
        /// Initialization logic which is execute only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
		private void CustomInitialize()
		{
            // Temp, will eventually be assigned based off the PlayerIndex var
            gamePad = InputManager.Xbox360GamePads[0];

		}

		private void CustomActivity()
		{
            MovementActivity();
            TurningActivity();
            ShootingActivity();
		}

		private void CustomDestroy()
		{


		}

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }

        /// <summary>
        /// Ship Forward Movement
        /// </summary>
        private void MovementActivity() {
            this.Velocity = this.RotationMatrix.Up * this.MovementSpeed;    // Have the ship always moving forward
        }

        /// <summary>
        /// Ship Rotation
        /// </summary>
        private void TurningActivity() {
            this.RotationZVelocity = -gamePad.LeftStick.Position.X * TurningSpeed;
        }

        /// <summary>
        /// Ship Shooting
        /// </summary>
        private void ShootingActivity() {
            if (gamePad.ButtonPushed(Xbox360GamePad.Button.A)) {
                // Create Right bullet
                Bullet rBullet = BulletFactory.CreateNew();
                rBullet.Position = this.Position;
                rBullet.Position += this.RotationMatrix.Up * 12;

                rBullet.Position += this.RotationMatrix.Right * 6;
                rBullet.RotationZ = this.RotationZ;
                rBullet.Velocity = this.RotationMatrix.Up * rBullet.MovementSpeed;

                //Create Left bullet
                Bullet lBullet = BulletFactory.CreateNew();
                lBullet.Position = this.Position;
                lBullet.Position += this.RotationMatrix.Up * 12;

                lBullet.Position -= this.RotationMatrix.Right * 6;
                lBullet.RotationZ = this.RotationZ;
                lBullet.Velocity = this.RotationMatrix.Up * lBullet.MovementSpeed;
            }
        }
	}
}
