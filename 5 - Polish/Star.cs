using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IntroToGameDesign {
	class Star : DrawableGameComponent {
		public Vector2 Position;
		public static Texture2D Sprite;

		private static Random s_rng;
		static Star() {
			s_rng = new Random();
		}

		public Star( Game p_game ) : base( p_game ) {
		}

		public override void Initialize() {
			base.Initialize();
		}

		private int Place;
		protected override void LoadContent() {
			Position = new Vector2( s_rng.Next( 0, Game.GraphicsDevice.Viewport.Width ), s_rng.Next( 0, Game.GraphicsDevice.Viewport.Height ) );
			Place = s_rng.Next( 0, 9 );
			TimeToChange = TimeSpan.FromMilliseconds( 200 );
			base.LoadContent();
		}

		TimeSpan TimeToChange;
		public override void Update( GameTime gameTime ) {
			if( TimeToChange <= TimeSpan.Zero ) {
				++Place;
				if( Place > 8 )
					Place = 0;
				TimeToChange = TimeSpan.FromMilliseconds( 200 );
			} else {
				TimeToChange -= gameTime.ElapsedGameTime;
			}

			base.Update( gameTime );
		}

		public override void Draw( GameTime gameTime ) {
			Rectangle Slice = new Rectangle( 5 * Place, 0, 5, 5 );
			( Game as Game1 ).spriteBatch.Draw( Sprite, Position, Slice, Color.White );

			base.Draw( gameTime );
		}
	}
}
