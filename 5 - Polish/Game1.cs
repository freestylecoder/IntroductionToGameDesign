using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace IntroToGameDesign {
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game {
		public GraphicsDeviceManager graphics;
		public SpriteBatch spriteBatch;

		#region Player
		Player Player1;
		#endregion

		#region Asteroids
		TimeSpan TimeToAsteroid;
		List<Asteroid> Asteroids;
		#endregion

		#region Bullets
		List<Bullet> Bullets;
		#endregion

		#region Polish
		SpriteFont CourierNew12;
		#endregion

		public Game1() {
			graphics = new GraphicsDeviceManager( this );
			Content.RootDirectory = "Content";

			#region Player
			Player1 = new Player( this );
			#endregion

			#region Asteroids
			Asteroids = new List<Asteroid>();
			Components.ComponentAdded += ComponentAdded;
			Components.ComponentRemoved += ComponentRemoved;
			#endregion

			#region Bullets
			Bullets = new List<Bullet>();
			#endregion
		}

		#region Asteroids
		void ComponentAdded( object sender, GameComponentCollectionEventArgs e ) {
			if( e.GameComponent is Asteroid )
				Asteroids.Add( e.GameComponent as Asteroid );
			#region Bullets
			else if( e.GameComponent is Bullet )
				Bullets.Add( e.GameComponent as Bullet );
			#endregion
		}

		void ComponentRemoved( object sender, GameComponentCollectionEventArgs e ) {
			if( e.GameComponent is Asteroid )
				Asteroids.Remove( e.GameComponent as Asteroid );
			#region Bullets
			else if( e.GameComponent is Bullet )
				Bullets.Remove( e.GameComponent as Bullet );
			#endregion
		}
		#endregion

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize() {
			#region Player
			Components.Add( Player1 );
			#endregion

			#region Asteroids
			Components.Add( new Asteroid( this ) );
			TimeToAsteroid = TimeSpan.FromMilliseconds( 500 );
			#endregion

			#region Polish
			for( int i = 0; i < 200; ++i )
				Components.Add( new Star( this ) );
			#endregion

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent() {
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch( GraphicsDevice );

			#region Bullets
			Bullet.Sprite = Content.Load<Texture2D>( "Bullet" );
			#endregion

			#region Polish
			Star.Sprite = Content.Load<Texture2D>( "star" );
			CourierNew12 = Content.Load<SpriteFont>( "ScoreFont" );
			#endregion

			base.LoadContent();
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent() {
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update( GameTime gameTime ) {
			if( GamePad.GetState( PlayerIndex.One ).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown( Keys.Escape ) )
				Exit();

			base.Update( gameTime );

			#region Asteroids
			if( TimeToAsteroid <= TimeSpan.Zero ) {
				Components.Add( new Asteroid( this ) );
				TimeToAsteroid = TimeSpan.FromMilliseconds( 500 );
			} else {
				TimeToAsteroid -= gameTime.ElapsedGameTime;
			}
			#endregion

			#region Collision
			for( int _AsteroidIndex = 0; _AsteroidIndex < Asteroids.Count; ++_AsteroidIndex ) {
				bool _AsteroidDestroyed = false;
				for( int _BulletIndex = 0; _BulletIndex < Bullets.Count; ++_BulletIndex ) {
					if( ContainmentType.Disjoint != Asteroids[_AsteroidIndex].Bounds.Contains( Bullets[_BulletIndex].Bounds ) ) {
						_AsteroidDestroyed = true;
						Components.Remove( Asteroids[_AsteroidIndex--] );
						Components.Remove( Bullets[_BulletIndex--] );

						#region Polish
						Player1.Score += 1000;
						#endregion

						break;
					}
				}

				if( _AsteroidDestroyed )
					continue;

				if( ContainmentType.Disjoint != Asteroids[_AsteroidIndex].Bounds.Contains( Player1.Bounds ) ) {
					Components.Remove( Asteroids[_AsteroidIndex--] );

					#region Polish
					if( 0 == Player1.Lives-- ) {
						Player1.Lives = 0;
						Components.Remove( Player1 );
					} else
					#endregion
						Player1.Position = new Vector2( GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height - Player1.Sprite.Height );
				}
			}
			#endregion
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw( GameTime gameTime ) {
			GraphicsDevice.Clear( Color.Black );
			spriteBatch.Begin();

			#region Finished
			spriteBatch.DrawString( CourierNew12, string.Format( "Score: {0}\nLives: {1}", Player1.Score, Player1.Lives ), new Vector2( 50, 50 ), Color.White );
			#endregion

			base.Draw( gameTime );

			spriteBatch.End();
		}
	}
}
