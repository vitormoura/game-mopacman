﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using MonoGame.Framework;
using mopacman.Components;
using mopacman.Controllers;
using mopacman.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mopacman.Scenes
{
    class MazeScene : GameScene
    {
        public Maze Maze { get; private set; }
                
        public Texture2D Background { get; private set; }

        public Song IntroSong { get; private set; }
               
        public Boolean Ready { get; set; }


        public MazeScene(MyGame g)
            : base(g)
        {
            this.Enabled = false;
            this.Visible = false;
        }
                
        public override void Initialize()
        {
            MyGame game = this.Game as MyGame;

            this.Maze = MazeBuilder.GetDefaultFor(game.Content);

            this.PrepareMazeUI();

            Puckman p = new Puckman(game);
            p.CurrentLocation = this.Maze.GetStartSection();
            p.Initialize();
                        
            KeyboardController keyboard = new KeyboardController(game, p);
            keyboard.Initialize();

            this.Components.Add(p);
            this.Components.Add(keyboard);
            
            ///*
            //Ghost 1
            RegisterNewGhost(@"Sprites\blinky.png", p, this.Maze[1, 4], this.Maze[5, 4]);
            
            //Ghost 2
            RegisterNewGhost(@"Sprites\pinky.png", p, this.Maze[26, 22], this.Maze[29, 22]);

            //Ghost 3
            RegisterNewGhost(@"Sprites\inky.png", p, this.Maze[26, 6], this.Maze[29, 6]);

            //Ghost 2
            RegisterNewGhost(@"Sprites\clyde.png", p, this.Maze[1, 24], this.Maze[5, 24]);
            //*/
          
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.Background = this.Game.Content.Load<Texture2D>("Backgrounds\\maze_template_1.png");
            this.IntroSong = this.Game.Content.Load<Song>("Songs\\pacman_beginning");

            base.LoadContent();
        }

        private void PrepareMazeUI()
        {
            ///* Renderizando paredes como blocos
            foreach (var s in this.Maze)
            {
                if (s.Allowed)
                {
                    Block b = new Block(this.Game as MyGame, s);
                    b.SetPosition(new Point((int)(s.ID.X * b.Bounds.Width), (int)((s.ID.Y * b.Bounds.Height))));
                    b.Initialize();

                    this.Components.Add(b);
                }
            }
            //*/
        }

        private void RegisterNewGhost(String ghostType, Puckman p, MazeSection r1, MazeSection r2)
        {
            Ghost g1 = new Ghost(this.Game as MyGame, ghostType);
            g1.Region = Tuple.Create(r1, r2);
            g1.CurrentLocation = this.Maze.GetGhostLairSection();
            g1.Initialize();

            this.Components.Add(g1);

            GhostAIController iaCtrl1 = new GhostAIController(this.Game as MyGame, g1, p);
            iaCtrl1.Initialize();

            this.Components.Add(iaCtrl1);
        }

        public override void Begin()
        {
            base.Begin();

            this.ready = false;

            songPlayedWaitTime = 4.5f;
            MediaPlayer.Play(this.IntroSong);
        }

        public override void Draw(GameTime gameTime)
        {
            if (this.Visible)
            {
                MyGame.SpriteBatch.Draw(this.Background, destinationRectangle: MyGame.Camera.TranslateToPixelsRect(this.Background.Bounds));

                base.Draw(gameTime);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if ( this.ready && this.Enabled)
            {
                base.Update(gameTime);
                return;
            }
            
            //Aguardando encerramento do som de introdução para considerar a cena pronta
            songPlayedWaitTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (songPlayedWaitTime <= 0.0f)
                this.ready = true;
        }

        private float songPlayedWaitTime;
        private Boolean ready;
    }
}
