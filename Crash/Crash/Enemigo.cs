using System;
using System.Collections.Generic;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Crash
{
    class Enemigo
    {
        private String nomCamina;
        private int indiceY;
        private int indiceX;
        private Texture2D texCamina;
        private Rectangle recEnemigo;
        public Vector2 posEnemigo;
        private ContentManager contenido;
        public int altoCorte;
        public int anchoCorte;
        private float posX;
        private float posY;
        public float avance;
        public SpriteEffects espejo = SpriteEffects.None;

        bool caminaDer;
        bool caminaIzq;
        bool movX;
        public bool visible;


        public ContentManager Contenido
        {
            get { return contenido; }
            set { contenido = value; }
        }
        public String NomCamania
        {
            get { return nomCamina; }
            set { nomCamina = value; }
        }

        public Texture2D TexCamina
        {
            get { return texCamina; }
            set { texCamina = value; }
        }
       
        public Vector2 PosEnemigo{
            get {return posEnemigo;}
            set{posEnemigo = value;}
        }

        public Rectangle RecEnemigo{
            get{ return recEnemigo; }
            set { recEnemigo = value; }
        }

        public int AnchoCorte
        {
            get { return anchoCorte; }
            set { anchoCorte = value; }
        }
        public int AltoCorte
        {
            get { return altoCorte; }
            set { altoCorte = value; }
        }
        public float PosX
        {
            get { return posX; }
            set { posX = value; }
        }
        public float PosY
        {
            get { return posY; }
            set { posY = value; }
        }
        public float Avance
        {
            get { return avance; }
            set { avance = value; }
        }
       

        public Enemigo( ContentManager contenido, int altoCorte, int anchoCorte,
                        int indiceX, int indiceY, float avance,
                        float posX, float posY, bool visible)
        {
            nomCamina = "zombie";
            this.contenido = contenido;
            this.avance = avance;
            this.visible = visible;
            this.indiceX = indiceX;
            this.indiceY = indiceY;
            texCamina = contenido.Load<Texture2D>(nomCamina);
            recEnemigo = new Rectangle(indiceX*anchoCorte, indiceY*altoCorte, anchoCorte, altoCorte);
            posEnemigo = new Vector2(posX, posY);

        }

        public void Dibuja(SpriteBatch spriteBatch, float tiempoTrascurrido, float tiempoCuadro)
        {
            //spriteBatch.Draw(texDetenido, posicion, recPlayer, Color.White, 0F, Vector2.Zero, 1f, espejo, 0f);
            if (visible)
            {
                if (caminaDer || caminaIzq)
                {
                    if (tiempoTrascurrido >= tiempoCuadro)
                    {
                        if (indiceX <= 4){
                            indiceX++;
                            recEnemigo.X = indiceX*anchoCorte;
                        }

                        else
                        {
                            indiceX = 0;
                            recEnemigo.X = indiceX*anchoCorte;
                        }
                            
                        tiempoTrascurrido = 0F;
                    }
                }
            }

            if (visible)
            {

             //   spriteBatch.Draw(texCamina, posEnemigo, recEnemigo, Color.White, 0F, Vector2.Zero, 1f, espejo, 0f);
                
                if (caminaDer)
                {
                    spriteBatch.Draw(texCamina, posEnemigo, recEnemigo, Color.White, 0F, Vector2.Zero, .5f, espejo, 0f);
                }
                else
                {
                    spriteBatch.Draw(texCamina, posEnemigo, recEnemigo, Color.White, 0F, Vector2.Zero, .5f, espejo, 0f);
                }
            }
        }

        public void mueve(float tiempoTrascurrido, float tiempoCuadro)
        {
            if (tiempoTrascurrido >= tiempoCuadro)
            {
                if (caminaIzq)
                {
                    posEnemigo.X -= avance;
                    
                }
                if (caminaDer)
                {
                    posEnemigo.Y += avance;
                }
            }

            if (posEnemigo.X <= posX - avance *8 || posEnemigo.X >= posX)
            {
                caminaIzq = !caminaIzq;
                caminaDer = !caminaDer;
            }
        }

    }
}
