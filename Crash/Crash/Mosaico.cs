using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;


namespace Crash
{
    class Mosaico
    {
        private Texture2D textura;
        public Texture2D Textura
        {
            get
            {
                return textura;
            }
            set
            {
                textura = value;
            }
        }

        private Rectangle rectangulo;
        public Rectangle Rectangulo
        {
            get
            {
                return rectangulo;
            }
            set
            {
                rectangulo = value;
            }
        }

        private ContentManager adminContenido;
        public ContentManager AdminContenido
        {
            get
            {
                return adminContenido;
            }
            set
            {
                adminContenido = value;
            }
        }

        private char tipoMos;
        private String nomTex;
        public bool visible;

        public char TipoMos
        {
            get { return tipoMos; }
            set { tipoMos = value; }
        }
        public String NomTex
        {
            get { return nomTex; }
            set { nomTex = value; }
        }
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }


        public Mosaico(string nomMosaico, Rectangle rectangulo, ContentManager adminContenido, char tipoMos)
        {
            this.adminContenido = adminContenido;
            this.textura = this.adminContenido.Load<Texture2D>(nomMosaico);
            this.rectangulo = rectangulo;
            this.tipoMos = tipoMos;
            this.visible = true;
            this.nomTex = nomMosaico;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
             if(visible)
                 spriteBatch.Draw(textura, rectangulo, Color.White);           
        }
    }
}
