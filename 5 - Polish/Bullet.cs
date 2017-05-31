using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IntroToGameDesign {
	public class Bullet : DrawableGameComponent {
		#region Bullets
		private Player m_Owner;

		public Vector2 Position;
		public static Texture2D Sprite;
		#endregion

		#region Collision
		private float Radius;
		public BoundingSphere Bounds {
			get {
				return new BoundingSphere( new Vector3( Position.X + Radius, Position.Y + Radius, 0 ), Radius );
			}
		}
		#endregion

		public Bullet( Game p_game, Player p_player ) : base( p_game ) {
			#region Bullets
			m_Owner = p_player;
			#endregion
		}

		public override void Initialize() {
			base.Initialize();
		}

		protected override void LoadContent() {
			#region Bullets
			if( null == Sprite )
				Sprite = Game.Content.Load<Texture2D>( "Bullet" );

			Position = new Vector2( m_Owner.Position.X + ( m_Owner.Sprite.Width / 2 ), m_Owner.Position.Y );
			#endregion

			#region Collision
			Radius = Sprite.Width / 2;
			#endregion

			base.LoadContent();
		}

		public override void Update( GameTime gameTime ) {
			#region Bullets
			Position -= new Vector2( 0, 10 );

			if( Position.Y < ( Sprite.Height * -1 ) ) {
				if( Game.Components.Remove( this ) ) {
					Dispose();
				}
			}
			#endregion

			base.Update( gameTime );
		}

		public override void Draw( GameTime gameTime ) {
			#region Bullets
			( Game as Game1 ).spriteBatch.Draw( Sprite, Position, Color.White );
			#endregion

			base.Draw( gameTime );
		}
	}
}
