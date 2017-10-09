﻿using Microsoft.Xna.Framework;
using mopacman.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mopacman.Components
{
    class Ghost : Player
    {
        public enum States
        {
            Waiting,
            Chase,
            Scatter,
            Frightened
        }

        public States State
        {
            get { return this.behavior.State; }
        }

        public GhostBehavior Behavior
        {
            get { return this.behavior; }
        }

        public Tuple<MazeSection,MazeSection> Region { get; set; }
               
        public Ghost(MyGame g, String ghostType)
            : base(g, ghostType, new Rectangle(0, 0, Constants.DEFAULT_BLOCK_WIDTH, Constants.DEFAULT_BLOCK_WIDTH))
        {
            this.behavior = new GhostBehavior();
            this.behavior.StateChanged +=behavior_StateChanged;
        }
                                       
        public override void Update(GameTime gameTime)
        {
            this.behavior.Update(gameTime);
            base.Update(gameTime);
        }

        private void behavior_StateChanged(object sender, EventArgs e)
        {
            this.Animation.Velocity = this.behavior.Velocity;
        }
                
        private GhostBehavior behavior;
    }
}
