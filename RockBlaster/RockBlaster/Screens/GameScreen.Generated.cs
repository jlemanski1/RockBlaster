#if ANDROID || IOS || DESKTOP_GL
#define REQUIRES_PRIMARY_THREAD_LOADING
#endif
using Color = Microsoft.Xna.Framework.Color;
using FlatRedBall;
using System;
using System.Collections.Generic;
using System.Text;
using FlatRedBall.Math;
namespace RockBlaster.Screens
{
    public partial class GameScreen : FlatRedBall.Screens.Screen
    {
        #if DEBUG
        static bool HasBeenLoadedWithGlobalContentManager = false;
        #endif
        
        private FlatRedBall.Math.PositionedObjectList<RockBlaster.Entities.Bullet> BulletList;
        private FlatRedBall.Math.PositionedObjectList<RockBlaster.Entities.Rock> RockList;
        private FlatRedBall.Math.PositionedObjectList<RockBlaster.Entities.MainShip> MainShipList;
        private RockBlaster.Entities.EndGameUI EndGameUIInstance;
        private RockBlaster.Entities.Hud HudInstance;
        private RockBlaster.Entities.MainShip MainShipInstance;
        public GameScreen () 
        	: base ("GameScreen")
        {
        }
        public override void Initialize (bool addToManagers) 
        {
            LoadStaticContent(ContentManagerName);
            BulletList = new FlatRedBall.Math.PositionedObjectList<RockBlaster.Entities.Bullet>();
            BulletList.Name = "BulletList";
            RockList = new FlatRedBall.Math.PositionedObjectList<RockBlaster.Entities.Rock>();
            RockList.Name = "RockList";
            MainShipList = new FlatRedBall.Math.PositionedObjectList<RockBlaster.Entities.MainShip>();
            MainShipList.Name = "MainShipList";
            EndGameUIInstance = new RockBlaster.Entities.EndGameUI(ContentManagerName, false);
            EndGameUIInstance.Name = "EndGameUIInstance";
            HudInstance = new RockBlaster.Entities.Hud(ContentManagerName, false);
            HudInstance.Name = "HudInstance";
            MainShipInstance = new RockBlaster.Entities.MainShip(ContentManagerName, false);
            MainShipInstance.Name = "MainShipInstance";
            
            
            PostInitialize();
            base.Initialize(addToManagers);
            if (addToManagers)
            {
                AddToManagers();
            }
        }
        public override void AddToManagers () 
        {
            Factories.BulletFactory.Initialize(ContentManagerName);
            Factories.BulletFactory.AddList(BulletList);
            EndGameUIInstance.AddToManagers(mLayer);
            HudInstance.AddToManagers(mLayer);
            MainShipInstance.AddToManagers(mLayer);
            base.AddToManagers();
            AddToManagersBottomUp();
            CustomInitialize();
        }
        public override void Activity (bool firstTimeCalled) 
        {
            if (!IsPaused)
            {
                
                for (int i = BulletList.Count - 1; i > -1; i--)
                {
                    if (i < BulletList.Count)
                    {
                        // We do the extra if-check because activity could destroy any number of entities
                        BulletList[i].Activity();
                    }
                }
                for (int i = RockList.Count - 1; i > -1; i--)
                {
                    if (i < RockList.Count)
                    {
                        // We do the extra if-check because activity could destroy any number of entities
                        RockList[i].Activity();
                    }
                }
                for (int i = MainShipList.Count - 1; i > -1; i--)
                {
                    if (i < MainShipList.Count)
                    {
                        // We do the extra if-check because activity could destroy any number of entities
                        MainShipList[i].Activity();
                    }
                }
                EndGameUIInstance.Activity();
                HudInstance.Activity();
                MainShipInstance.Activity();
            }
            else
            {
            }
            base.Activity(firstTimeCalled);
            if (!IsActivityFinished)
            {
                CustomActivity(firstTimeCalled);
            }
        }
        public override void Destroy () 
        {
            base.Destroy();
            Factories.BulletFactory.Destroy();
            
            BulletList.MakeOneWay();
            RockList.MakeOneWay();
            MainShipList.MakeOneWay();
            for (int i = BulletList.Count - 1; i > -1; i--)
            {
                BulletList[i].Destroy();
            }
            for (int i = RockList.Count - 1; i > -1; i--)
            {
                RockList[i].Destroy();
            }
            for (int i = MainShipList.Count - 1; i > -1; i--)
            {
                MainShipList[i].Destroy();
            }
            if (EndGameUIInstance != null)
            {
                EndGameUIInstance.Destroy();
                EndGameUIInstance.Detach();
            }
            if (HudInstance != null)
            {
                HudInstance.Destroy();
                HudInstance.Detach();
            }
            if (MainShipInstance != null)
            {
                MainShipInstance.Destroy();
                MainShipInstance.Detach();
            }
            BulletList.MakeTwoWay();
            RockList.MakeTwoWay();
            MainShipList.MakeTwoWay();
            FlatRedBall.Math.Collision.CollisionManager.Self.Relationships.Clear();
            CustomDestroy();
        }
        public virtual void PostInitialize () 
        {
            bool oldShapeManagerSuppressAdd = FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue;
            FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = true;
            FlatRedBall.Math.Geometry.ShapeManager.SuppressAddingOnVisibilityTrue = oldShapeManagerSuppressAdd;
        }
        public virtual void AddToManagersBottomUp () 
        {
            CameraSetup.ResetCamera(SpriteManager.Camera);
            AssignCustomVariables(false);
        }
        public virtual void RemoveFromManagers () 
        {
            for (int i = BulletList.Count - 1; i > -1; i--)
            {
                BulletList[i].Destroy();
            }
            for (int i = RockList.Count - 1; i > -1; i--)
            {
                RockList[i].Destroy();
            }
            for (int i = MainShipList.Count - 1; i > -1; i--)
            {
                MainShipList[i].Destroy();
            }
            EndGameUIInstance.RemoveFromManagers();
            HudInstance.RemoveFromManagers();
            MainShipInstance.RemoveFromManagers();
        }
        public virtual void AssignCustomVariables (bool callOnContainedElements) 
        {
            if (callOnContainedElements)
            {
                EndGameUIInstance.AssignCustomVariables(true);
                HudInstance.AssignCustomVariables(true);
                MainShipInstance.AssignCustomVariables(true);
            }
        }
        public virtual void ConvertToManuallyUpdated () 
        {
            for (int i = 0; i < BulletList.Count; i++)
            {
                BulletList[i].ConvertToManuallyUpdated();
            }
            for (int i = 0; i < RockList.Count; i++)
            {
                RockList[i].ConvertToManuallyUpdated();
            }
            for (int i = 0; i < MainShipList.Count; i++)
            {
                MainShipList[i].ConvertToManuallyUpdated();
            }
            EndGameUIInstance.ConvertToManuallyUpdated();
            HudInstance.ConvertToManuallyUpdated();
            MainShipInstance.ConvertToManuallyUpdated();
        }
        public static void LoadStaticContent (string contentManagerName) 
        {
            if (string.IsNullOrEmpty(contentManagerName))
            {
                throw new System.ArgumentException("contentManagerName cannot be empty or null");
            }
            #if DEBUG
            if (contentManagerName == FlatRedBall.FlatRedBallServices.GlobalContentManager)
            {
                HasBeenLoadedWithGlobalContentManager = true;
            }
            else if (HasBeenLoadedWithGlobalContentManager)
            {
                throw new System.Exception("This type has been loaded with a Global content manager, then loaded with a non-global.  This can lead to a lot of bugs");
            }
            #endif
            RockBlaster.Entities.EndGameUI.LoadStaticContent(contentManagerName);
            RockBlaster.Entities.Hud.LoadStaticContent(contentManagerName);
            RockBlaster.Entities.MainShip.LoadStaticContent(contentManagerName);
            CustomLoadStaticContent(contentManagerName);
        }
        public override void PauseThisScreen () 
        {
            base.PauseThisScreen();
        }
        public override void UnpauseThisScreen () 
        {
            base.UnpauseThisScreen();
        }
        [System.Obsolete("Use GetFile instead")]
        public static object GetStaticMember (string memberName) 
        {
            return null;
        }
        public static object GetFile (string memberName) 
        {
            return null;
        }
        object GetMember (string memberName) 
        {
            return null;
        }
    }
}
