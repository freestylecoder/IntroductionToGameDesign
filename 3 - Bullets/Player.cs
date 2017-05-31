using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace IntroToGameDesign {
	public class Player : DrawableGameComponent {
		#region Player
		public Vector2 Position;
		public Texture2D Sprite;
		#endregion

		#region Bullets
		private GamePadState LastState;
		#endregion

		public Player( Game game ) : base( game ) {
		}

		public override void Initialize() {
			#region Bullets
			LastState = GamePad.GetState( PlayerIndex.One );
			#endregion

			base.Initialize();
		}

		protected override void LoadContent() {
			#region Player
			Sprite = Game.Content.Load<Texture2D>( "Ship" );
			Position = new Vector2( Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height - Sprite.Height );
			#endregion

			base.LoadContent();
		}

		/// <summary>Allows the game component to update itself.</summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update( GameTime gameTime ) {
			#region Player
			#region Move Player
			Position.X += GamePad.GetState( PlayerIndex.One ).ThumbSticks.Left.X * 5f;
			Position.Y -= GamePad.GetState( PlayerIndex.One ).ThumbSticks.Left.Y * 5f;
			#endregion

			#region Keep Player in Viewport
			Position.X = MathHelper.Clamp( Position.X, 0, Game.GraphicsDevice.Viewport.Width - Sprite.Width );
			Position.Y = MathHelper.Clamp( Position.Y, 0, Game.GraphicsDevice.Viewport.Height - Sprite.Height );
			#endregion
			#endregion

			#region Bullets
			#region Fire Bullet
			if( GamePad.GetState( PlayerIndex.One ).IsButtonDown( Buttons.A ) )
				//if( LastState.IsButtonUp( Buttons.A ) )
					Game.Components.Add( new Bullet( Game, this ) );
			#endregion

			LastState = GamePad.GetState( PlayerIndex.One );
			#endregion

			base.Update( gameTime );
		}

		public override void Draw( GameTime gameTime ) {
			#region Player
			( Game as Game1 ).spriteBatch.Draw( Sprite, Position, Color.White );
			#endregion

			base.Draw( gameTime );
		}
	}
}
