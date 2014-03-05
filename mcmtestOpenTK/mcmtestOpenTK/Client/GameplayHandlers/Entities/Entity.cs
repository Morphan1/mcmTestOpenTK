using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using mcmtestOpenTK.Client.GraphicsHandlers;
using mcmtestOpenTK.Client.CommonHandlers;
using mcmtestOpenTK.Client.GlobalHandler;

namespace mcmtestOpenTK.Client.GameplayHandlers.Entities
{
    public class Entity: Renderable
    {
        /// <summary>
        /// The precise X/Y/Z location of the entity.
        /// </summary>
        public Vector3 Location = Vector3.Zero;
        /// <summary>
        /// The precise X/Y/Z worldly movement speed.
        /// </summary>
        public Vector3 Velocity = Vector3.Zero;
        /// <summary>
        /// The precise Yaw/Pitch/Roll direction of the entity.
        /// </summary>
        public Vector3 Angle = Vector3.Zero;
        /// <summary>
        /// A fairly unique ID stored as long as the entity is alive.
        /// </summary>
        public long ID;

        public Entity()
        {
            ID = MainGame.NewEntityID();
        }

        /// <summary>
        /// Ticks the entity, including running of basic movement and related handlers.
        /// </summary>
        public virtual void Update()
        {
            Location += Velocity * MainGame.DeltaF;
        }

        /// <summary>
        /// Fully renders the entity.
        /// </summary>
        public override void Draw()
        {
        }
    }
}
