using System;
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
    public class Game1 : Game
    {
        enum fase { inicio, nivel, nivel2, fin, win };
        
        const int ANCHO_ESC = 1280;
        const int ALTO_ESC = 800;
        const int ANCHO_MOSAICO = 32;
        const int ALTO_MOSAICO = 32;
        const int anchoCorte = 45;
        const int altoCorte = 55;
        const float tiempoPorCuadro = 0.08F;
        const float tiempoSalto = .10F;
        float tiempoGiro = .05F;
        float avance = 1.20F;
        const int anchoCamara = 640;
        const int altoCamara = 480;
        const int anchoGiro = 70;
        //float limX = 27*ANCHO_MOSAICO-45;
        float avanceSalto = 20F;

        int fases;
        int indiceY = 0;
        int indiceX = 0;
        
        //float piso = ALTO_ESC-107;
        public static int score = 0;
        public static int puntos = 0;
        public static int vidas = 3;
        
       
        bool correDer;
        bool correIzq;
        bool movX;
        bool caer;
        bool salta=true;
        float tiempoTrascurrido;
        bool cayo = false;
        bool muerto;
        bool giro = false;
        bool golpe = false;
        bool ukaVisible = false;
        bool nivel2 = false;
        bool nivel = false;
        
        
        

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Mapa mapa;
        Song musica;
        Song intro;
        Song fin;
        Song n2;
        SoundEffect sGiro;
        SoundEffect sGolpe;
        SoundEffect sMetal;
        SoundEffect sCome;
        Texture2D texCamina;
        Texture2D texMuerte;
        Texture2D texSalta;
        Texture2D texNivel;
        Rectangle recFondo;
        Texture2D texIntro;
        Rectangle recIntro;
        Rectangle uka;
        Texture2D texPlayer;
        Texture2D texGiro;
        Texture2D texGolpe;
        Texture2D texUka;
        Rectangle recPlayer;
        Rectangle recGiro;
        Vector2 posicion;
        SpriteEffects espejo = SpriteEffects.None;
        Matrix camara = new Matrix();
        Vector3 posCam = new Vector3(0, 0, 0);
        Vector2 centroCam = new Vector2(0, 0);

        SpriteFont texto;
        Vector2 posTexto;

       /* Enemigo z1;
        Enemigo z2;
        Enemigo z3;
        Enemigo z4;*/


        public Game1()
           
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";


            graphics.CreateDevice();

            //graphics.PreferredBackBufferWidth = ANCHO_ESC;
            //graphics.PreferredBackBufferHeight = ALTO_ESC;
            graphics.ApplyChanges();
            graphics.IsFullScreen = true;
        }

        
        protected override void Initialize()
        {
            graphics.CreateDevice();
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = anchoCamara;
            graphics.PreferredBackBufferHeight = altoCamara;
            graphics.ApplyChanges();
            



            mapa = new Mapa(ANCHO_MOSAICO, ALTO_MOSAICO);
            mapa.AdminContenido = Content;
           // mapa.Generar("Escenario.txt");

            recIntro = new Rectangle(0, 0, 243, 174);
            recPlayer = new Rectangle(0, 0, 45, 55);
            recGiro = new Rectangle(0, 0, 70, 55);
            posicion = new Vector2(100, 700);
            recFondo = new Rectangle(0,0, ANCHO_ESC, ALTO_ESC); //-320,-240
            
            uka = new Rectangle (1000, 700, 30,40);
            
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            //Caarga de texturas
            texPlayer = Content.Load<Texture2D>("crash");
            texCamina = Content.Load<Texture2D>("caminaCrash");
            texSalta = Content.Load<Texture2D>("saltaCrash");
            texMuerte = Content.Load<Texture2D>("muerte");
            texIntro = Content.Load<Texture2D>("inicio");
            texNivel = Content.Load<Texture2D>("tree");
            texGiro = Content.Load<Texture2D>("giro");
            texGolpe = Content.Load<Texture2D>("golpe");
            texUka = Content.Load<Texture2D>("mosaico_4");
            
            //carga de sonidos
            musica = Content.Load<Song>("nivel1.wav");
            intro = Content.Load<Song>("intro.wav");
            fin = Content.Load<Song>("musica.wav");
            n2 = Content.Load<Song>("nivel2.wav");

            sGiro = Content.Load<SoundEffect>("giro.wav");
            sCome = Content.Load<SoundEffect>("comer.wav");
            sGolpe = Content.Load<SoundEffect>("golpe.wav");
            sMetal = Content.Load<SoundEffect>("metal.wav");
            //texto
            texto = Content.Load<SpriteFont>("spriteFont");
            posTexto = new Vector2(anchoCamara/2 - 100,360);

            //Enemigos
            z1 = new Enemigo(Content, 128, 128, 0, 0, 0.10f, 200, 700, true);
            z2 = new Enemigo(Content, 128, 128, 0, 1, 0.10f, 300, 700, true);
            z3 = new Enemigo(Content, 128, 128, 0, 2, 0.10f, 400, 700, true);
            z4 = new Enemigo(Content, 128, 128, 0, 3, 0.10f, 500, 700, true);
            //*/
        }

        
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

       
        protected override void Update(GameTime gameTime)
        {
            prueba();
            switch (fases)
            {
                case 0:
                    {
                        if (MediaPlayer.State == MediaState.Stopped)
                        {
                            MediaPlayer.Stop();
                            //MediaPlayer.State = MediaState.Playing;
                            MediaPlayer.Play(intro);
                            //MediaPlayer.IsRepeating;
//                            MediaPlayer.IsRepeating();

                            

                        }
                        Update_inicio(gameTime);
                        break;
                    }

                case 1:
                    {
                        
                        if (MediaPlayer.State != MediaState.Playing)
                        {
                            MediaPlayer.Stop();
                            MediaPlayer.Play(musica);
                        }
                        if (!nivel)
                        {
                            nivel = true;
                            mapa.Generar("Escenario.txt");
                        }
                        Update_nivel(gameTime);
                        break;
                    }
                case 2:

                    Update_nivel2(gameTime);
                    if (MediaPlayer.State == MediaState.Stopped)
                    {
                        MediaPlayer.Play(n2);

                    }
                    if (!nivel2)
                    {
                        nivel2 = true;
                        ukaVisible = false;
                        mapa = new Mapa(ANCHO_MOSAICO, ALTO_MOSAICO);
                        mapa.AdminContenido = Content;
                        mapa.Generar("Nivel2.txt");
                        texNivel = Content.Load<Texture2D>("templo");
                    }
                    break;

               case 3:
                    Update_fin(gameTime);
                    if (MediaPlayer.State == MediaState.Paused)
                    {
                        MediaPlayer.Play(fin);

                    }
                    break;
                case 4:
                    Update_win(gameTime);
                    break;
                default:
                    Exit();
                    break;
            }
            

            base.Update(gameTime);
        }

       
        protected override void Draw(GameTime gameTime)
        {           
            switch (fases)
            {
                case 0:
                    Draw_inicio(gameTime);
                    break;
                case 1:
                    Draw_nivel(gameTime);
                    break;
                case 2:
                    Draw_nivel(gameTime);
                    break;
                case 3:
                    Draw_fin(gameTime);
                    break;
                case 4:
                    Draw_win(gameTime);
                    break;
                

            }
            base.Draw(gameTime);
        }

        public void Update_inicio(GameTime gameTime)
        {
            recIntro = new Rectangle(0, 0, 243, 174);
            texIntro = Content.Load<Texture2D>("inicio");
             KeyboardState kbs = Keyboard.GetState();
             if (kbs.IsKeyDown(Keys.Enter))
             {
                 MediaPlayer.Stop();
                 fases = (int)fase.nivel;
             }
                 
                 
        }

        public void Update_nivel(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            


            posCam = new Vector3(0, 0, 0);
            camara = Matrix.CreateTranslation(posCam);

            


            if (posicion.Y > ALTO_ESC)
            {
                muerto = true;
                
               /* if (posicion.Y < 0-recFondo.Y)
                {
                    fases = (int)fase.fin;
                }*/
            }

            else
            {


                ///mapa
                ConstruyeMapa();


                KeyboardState kbs = Keyboard.GetState();
                if (kbs.IsKeyDown(Keys.Space) && kbs.IsKeyDown(Keys.LeftShift)) { golpe = true; }
                else if (kbs.IsKeyDown(Keys.Space)) { giro = true; sGiro.Play(); }
                if (kbs.IsKeyDown(Keys.Up))
                    salta = true;
                if (kbs.IsKeyDown(Keys.Right))
                {
                    if (movX)
                    {
                        correDer = true;
                        posicion.X += avance;
                        espejo = SpriteEffects.None;
                        correIzq = false;
                    }
                    else
                    {
                        correDer = false;
                        correIzq = false;
                    }

                }
                else if (kbs.IsKeyDown(Keys.Left))
                {
                    if (movX)
                    {
                        correIzq = true;
                        posicion.X -= avance;
                        espejo = SpriteEffects.FlipHorizontally;
                        correDer = false;
                    }
                    else
                    {
                        correDer = false;
                        correIzq = false;
                    }
                }
                else
                {
                    correDer = false;
                    correIzq = false;
                }


                if (caer && !salta)
                {
                    posicion.Y += 5;
                }
            }

            CamaraParallax();
            ChecaUka();
            

        }

        public void Update_nivel2(GameTime gameTieme)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();



            posCam = new Vector3(0, 0, 0);
            camara = Matrix.CreateTranslation(posCam);




            if (posicion.Y > ALTO_ESC)
            {
                muerto = true;

                /* if (posicion.Y < 0-recFondo.Y)
                 {
                     fases = (int)fase.fin;
                 }*/
            }

            else
            {


                ///mapa
                ConstruyeMapa();


                KeyboardState kbs = Keyboard.GetState();
                if (kbs.IsKeyDown(Keys.Space) && kbs.IsKeyDown(Keys.LeftShift)) { golpe = true; }
                else if (kbs.IsKeyDown(Keys.Space)) { giro = true; sGiro.Play(); }
                if (kbs.IsKeyDown(Keys.Up))
                    salta = true;
                if (kbs.IsKeyDown(Keys.Right))
                {
                    if (movX)
                    {
                        correDer = true;
                        posicion.X += avance;
                        espejo = SpriteEffects.None;
                        correIzq = false;
                    }
                    else
                    {
                        correDer = false;
                        correIzq = false;
                    }

                }
                else if (kbs.IsKeyDown(Keys.Left))
                {
                    if (movX)
                    {
                        correIzq = true;
                        posicion.X -= avance;
                        espejo = SpriteEffects.FlipHorizontally;
                        correDer = false;
                    }
                    else
                    {
                        correDer = false;
                        correIzq = false;
                    }
                }
                else
                {
                    correDer = false;
                    correIzq = false;
                }


                if (caer && !salta)
                {
                    posicion.Y += 5;
                }
            }

            CamaraParallax();
            ChecaUka();
        }
        public void Update_fin(GameTime gameTime)
        {
            recIntro = new Rectangle(0, 0, 159, 83);
            texIntro = Content.Load<Texture2D>("gameOver");
            KeyboardState kbs = Keyboard.GetState();
            if (kbs.IsKeyDown(Keys.Enter))
                Exit();
        }

        public void Update_win(GameTime gameTime) {
            recIntro = new Rectangle(0, 0, 841, 741);
            texIntro = Content.Load<Texture2D>("winner");
            KeyboardState kbs = Keyboard.GetState();
            if (kbs.IsKeyDown(Keys.Enter))
                Exit();
        }
        public void Draw_inicio(GameTime gameTime)
        {
                      
            GraphicsDevice.Clear(Color.Black);
           
            tiempoTrascurrido += (float)gameTime.ElapsedGameTime.TotalSeconds;
            bool cambia = false;

            
            spriteBatch.Begin();
            spriteBatch.Draw(texIntro, new Vector2(posTexto.X-30, 0) , recIntro, Color.White, 0F, Vector2.Zero, 2F, espejo, 0f);
            if (tiempoTrascurrido > tiempoGiro * 20)
            {
                cambia = !cambia;
                tiempoTrascurrido = 0;
            }
            if (cambia)
            {
                spriteBatch.DrawString(texto, "Press Intro", posTexto, Color.Yellow,0F,Vector2.Zero,3f,SpriteEffects.None,0F);
            }
            else
                spriteBatch.DrawString(texto, "Press Intro", posTexto, Color.OrangeRed,0f,Vector2.Zero,3f,SpriteEffects.None,0F);
            spriteBatch.End();
        }
        public void Draw_fin(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.TransparentBlack);

            tiempoTrascurrido += (float)gameTime.ElapsedGameTime.TotalSeconds;
            spriteBatch.Begin();
            spriteBatch.Draw(texIntro, new Vector2(90,10), recIntro, Color.White, 0F, Vector2.Zero, 4F, espejo, 0f);
            spriteBatch.DrawString(texto, "GAME OVER", posTexto, Color.DarkOrange, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f);
            spriteBatch.End();
        }
        public void Draw_nivel(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CadetBlue);
            tiempoTrascurrido += (float)gameTime.ElapsedGameTime.TotalSeconds;
            spriteBatch.Begin(SpriteSortMode.Deferred,BlendState.AlphaBlend,null,null,null,null,camara);
            spriteBatch.Draw(texNivel, recFondo, Color.White);
            mapa.Draw(spriteBatch);
            //z3.mueve(tiempoTrascurrido, 0.5f);
            //z1.Dibuja(spriteBatch, tiempoTrascurrido, tiempoSalto);
            z2.Dibuja(spriteBatch, tiempoTrascurrido, tiempoSalto);
            //z3.Dibuja(spriteBatch, tiempoTrascurrido, tiempoSalto);
            //z4.Dibuja(spriteBatch, tiempoTrascurrido, tiempoSalto);

            if (!muerto)
            {
                if (giro)
                {
                    if (tiempoTrascurrido > tiempoGiro)
                    {
                        recGiro.X = indiceX * anchoGiro;
                        recGiro.Y = 0;
                        indiceX++;
                        tiempoTrascurrido = 0;
                        if (indiceX == 4)
                        {
                            tiempoGiro = tiempoGiro * 3;
                        }
                        if (indiceX > 4)
                        {
                            giro = false;
                            indiceX = 0;
                            tiempoGiro = tiempoGiro / 3;
                        }
                    }
                }
                
                if (salta)
                {
                    cayo = false;
                    if (tiempoTrascurrido > tiempoSalto)
                    {
                        indiceY++;
                        tiempoTrascurrido = 0F;
                        if (indiceY <= 4)
                        {

                            posicion.Y -= avanceSalto;

                        }
                        else
                        {
                            /*if (posicion.Y >= piso)
                            {
                                cayo = true;
                            }*/
                            // cayo = true; //validar con una colision
                            if (cayo)
                            {
                                salta = false;
                                indiceY = 0;

                            }
                            else
                            {
                                indiceY = 4;
                                posicion.Y += 20;
                            }
                        }
                    }
                }
                else if (correDer || correIzq)
                {
                    if (tiempoTrascurrido >= tiempoPorCuadro)
                    {
                        if (indiceY <= 6)
                            indiceY++;
                        else
                            indiceY = 0;
                        tiempoTrascurrido = 0F;
                    }


                }
                if (!correDer && !correIzq && !salta)
                    indiceY = 0;

                recPlayer.Y = indiceY * altoCorte;

            }
            else
            {
                //posicion.Y = 200;
                
                if (vidas > 0)
                {
                    if (tiempoTrascurrido > 4)
                    {
                        vidas--;
                        posicion.X = 100;
                        posicion.Y = 700;
                        muerto = false;
                    }
                    
                }
                else {
                    MediaPlayer.Stop();
                    fases = (int)fase.fin; 
                }
            }
            if (muerto)
            {
                posicion.Y -= 10;
                spriteBatch.Draw(texMuerte, posicion, recPlayer, Color.White, 0F, Vector2.Zero, 1f, espejo, 0f);

            }
            else if (giro)
            {
                spriteBatch.Draw(texGiro, posicion, recGiro, Color.White, 0F, Vector2.Zero, 1f, espejo, 0f);
            }

            else if (salta)
            {
                
                spriteBatch.Draw(texSalta, posicion, recPlayer, Color.White, 0F, Vector2.Zero, 1f, espejo, 0f);
            }

            else if (correDer){
                spriteBatch.Draw(texCamina, posicion, recPlayer, Color.White, 0F, Vector2.Zero, 1f, espejo, 0f);
            }
            else if (correIzq)
            {
                spriteBatch.Draw(texCamina, posicion, recPlayer, Color.White, 0F, Vector2.Zero, 1f, espejo, 0f);
            }


            else
            {
                spriteBatch.Draw(texPlayer, posicion, recPlayer, Color.White, 0F, Vector2.Zero, 1f, espejo, 0f);
            }
            if(ukaVisible){ spriteBatch.Draw(texUka, uka, Color.White); }

            //dibujando Texto
            spriteBatch.DrawString(texto, "Puntos: " + score.ToString(),new Vector2(centroCam.X+150,centroCam.Y-230), Color.Yellow, 0F, Vector2.Zero, 1f, SpriteEffects.None, 0F);
            spriteBatch.DrawString(texto, "Vidas: x " + vidas.ToString() , new Vector2(centroCam.X + 300, centroCam.Y-230), Color.Yellow, 0F, Vector2.Zero, 1f, SpriteEffects.None, 0F);
            spriteBatch.End();
        }

        public void Draw_nivle2(GameTime gameTime)
        {

        }
        public void Draw_win(GameTime gameTime) 
        {
            GraphicsDevice.Clear(Color.TransparentBlack);

            //tiempoTrascurrido += (float)gameTime.ElapsedGameTime.TotalSeconds;
            spriteBatch.Begin();
            spriteBatch.Draw(texIntro, new Vector2(150, 10), recIntro, Color.White, 0F, Vector2.Zero, .5F, SpriteEffects.None, 0f);
            spriteBatch.DrawString(texto, "   Felicidades!!  "+
                                      "\nScore: " + puntos, posTexto, Color.Yellow, 0F, Vector2.Zero, 1.8f, SpriteEffects.None, 0F);

            spriteBatch.End();
        }

        public void ConstruyeMapa()
        {
            foreach (Mosaico m in mapa.mosaicos)
            {
                BoundingBox boxCrashAbajo = new BoundingBox();
                boxCrashAbajo.Min = new Vector3(posicion.X + 5, posicion.Y, 0);
                boxCrashAbajo.Max = new Vector3(posicion.X + anchoCorte, posicion.Y + altoCorte, 0);

                BoundingBox boxGiro = new BoundingBox();
                boxGiro.Min = new Vector3(posicion.X + 5, posicion.Y, 0);
                boxGiro.Max = new Vector3(posicion.X + anchoGiro, posicion.Y + altoCorte, 0);



                if ( m.TipoMos == '1' || m.TipoMos == 'b' )
                {
                    BoundingBox boxPiso = new BoundingBox();
                    boxPiso.Min = new Vector3(m.Rectangulo.X+18, m.Rectangulo.Y, 0);
                    boxPiso.Max = new Vector3(m.Rectangulo.X + ANCHO_MOSAICO -18, m.Rectangulo.Y + ALTO_MOSAICO, 0);

                    if (boxCrashAbajo.Intersects(boxPiso))
                    {
                        caer = false;
                        posicion.Y = m.Rectangulo.Y - 40;
                        salta = false;
                    }
                    else
                    {
                        if(!salta)
                        caer = true;
                    }



                }

                else
                    if (m.TipoMos == '2' || m.TipoMos == '7')
                {

                    BoundingBox boxMuro = new BoundingBox();
                    boxMuro.Min = new Vector3(m.Rectangulo.X, m.Rectangulo.Y, 0);
                    boxMuro.Max = new Vector3(m.Rectangulo.X + ANCHO_MOSAICO, m.Rectangulo.Y + ALTO_MOSAICO, 0);

                    if (boxCrashAbajo.Intersects(boxMuro))
                    {

                        movX = false;
                        if (correIzq)
                        {
                            posicion.X = m.Rectangulo.X + ANCHO_MOSAICO + 1;
                        }
                        else
                        {
                            posicion.X = m.Rectangulo.X - 46;
                        }
                    }
                    else
                    {
                        movX = true;
                    }
                }


                else if (m.TipoMos == '3' || m.TipoMos == '6' )
                {
                    BoundingBox boxArriba = new BoundingBox();
                    boxArriba.Min = new Vector3(m.Rectangulo.X + 18, m.Rectangulo.Y, 0);
                    boxArriba.Max = new Vector3(m.Rectangulo.X + m.Rectangulo.Width - 18, m.Rectangulo.Y + m.Rectangulo.Height / 2, 0);

                    if (boxCrashAbajo.Intersects(boxArriba))
                    {
                        caer = false;
                        posicion.Y = m.Rectangulo.Y - 45;
                        salta = false;
                    }
                    else
                    {
                        caer = true;
                    } 

                }

                else if (m.TipoMos == '4')
                {
                    BoundingBox boxUka = new BoundingBox();
                    boxUka.Min = new Vector3(m.Rectangulo.X, m.Rectangulo.Y, 0);
                    boxUka.Max = new Vector3(m.Rectangulo.X + m.Rectangulo.Width, m.Rectangulo.Y + m.Rectangulo.Height, 0);
                    if (boxCrashAbajo.Intersects(boxUka) || posicion.Y < -10)
                    {
                        fases = (int)fase.fin;
                    }
                }
                else if (m.TipoMos == '5')
                {
                    BoundingBox boxFruta = new BoundingBox();
                    boxFruta.Min = new Vector3(m.Rectangulo.X, m.Rectangulo.Y, 0);
                    boxFruta.Max = new Vector3(m.Rectangulo.X + m.Rectangulo.Width, m.Rectangulo.Y + m.Rectangulo.Height, 0);
                    if (boxCrashAbajo.Intersects(boxFruta))
                    {
                        if (m.visible)
                        {
                            sCome.Play();
                            m.visible = false;
                            score += 1;
                            puntos += score;
                            if (score >= 100) { vidas++; score = 0; }
                        }

                    }
                }
                else if (m.TipoMos == '8')
                {
                    BoundingBox boxCajaArriba = new BoundingBox();
                    boxCajaArriba.Min = new Vector3(m.Rectangulo.X + 18, m.Rectangulo.Y, 0);
                    boxCajaArriba.Max = new Vector3(m.Rectangulo.X + m.Rectangulo.Width - 18, m.Rectangulo.Y + m.Rectangulo.Height, 0);

                    if (giro)
                    {
                        if (boxGiro.Intersects(boxCajaArriba))
                        {
                            if (m.visible)
                            {
                                m.visible = false;
                                ukaVisible = true;
                                sMetal.Play();
                            }
                        }
                    }
                    else
                    {

                        if (boxCrashAbajo.Intersects(boxCajaArriba))
                        {
                            if (m.visible)
                            {
                                caer = false;
                                posicion.Y = m.Rectangulo.Y - 45;
                                salta = false;

                            }
                            else
                            {
                                caer = true;
                            }
                        }
                    }

                }

                else if (m.TipoMos == '9')
                {
                    BoundingBox boxCajaArriba = new BoundingBox();
                    boxCajaArriba.Min = new Vector3(m.Rectangulo.X + 18, m.Rectangulo.Y, 0);
                    boxCajaArriba.Max = new Vector3(m.Rectangulo.X + m.Rectangulo.Width - 18, m.Rectangulo.Y + m.Rectangulo.Height, 0);

                    if (giro)
                    {
                        if (boxGiro.Intersects(boxCajaArriba))
                        {
                            if (m.visible)
                            {
                                Random rnd = new Random();
                                int a = (int)rnd.Next(1, 5);
                                
                                m.visible = false;
                                score += a;
                                puntos += score;
                                sGolpe.Play();
                                if (score >= 100) { vidas++; score = 0; }
                            }
                        }
                    }
                    else
                    {
                        if (boxCrashAbajo.Intersects(boxCajaArriba))
                        {
                            if (m.visible)
                            {
                                caer = false;
                                posicion.Y = m.Rectangulo.Y - 45;
                                salta = false;
                            }
                            else
                            {
                                caer = true;
                            }
                        }
                    }

                }
            }
        }

        public void CamaraParallax()
        {
            //camara Parallax
            if (posicion.X < anchoCamara / 2)
            {
                centroCam.X = anchoCamara / 2;
            }
            else if (posicion.X > (ANCHO_ESC - anchoCamara / 2) - 160)
            {
                centroCam.X = (ANCHO_ESC - anchoCamara / 2) - 160;
            }
            else
                centroCam.X = posicion.X;

            if (posicion.Y < altoCamara / 2)
            {
                centroCam.Y = altoCamara / 2;
            }
            else if (posicion.Y > (ALTO_ESC - altoCamara / 2))
            {
                centroCam.Y = ALTO_ESC - altoCamara / 2;
            }
            else
            {
                centroCam.Y = posicion.Y;
            }


            posCam.X = -centroCam.X + anchoCamara / 2;
            posCam.Y = -centroCam.Y + altoCamara / 2;
            camara = Matrix.CreateTranslation(posCam);
            //fin camara
        }

        public void ChecaUka()
        {
            Rectangle crash = new Rectangle((int)posicion.X, (int)posicion.Y, recPlayer.Width, recPlayer.Height);
            Rectangle ukaW = new Rectangle(uka.X, uka.Y, uka.Width, uka.Height);
            if (crash.Intersects(ukaW))
            {
                if (ukaVisible)
                {
                    if (nivel2)
                    {
                        fases = (int)fase.win;
                        MediaPlayer.Stop();
                    }

                    else
                    {
                        MediaPlayer.Stop();
                        fases = (int)fase.nivel2;
                    }

                    posicion.X = 100;
                    posicion.Y = 700;
                }
                
            }
        }

        public void prueba()
        {
            KeyboardState kbs = Keyboard.GetState();
            if (kbs.IsKeyDown(Keys.S) && kbs.IsKeyDown(Keys.LeftShift)) { fases +=1;  }
               
        }

       /*/ public void MueveEne(Enemigo zombie, GameTime gameTime)
        {
            if (zombie.visible)
            {
                if (tiempoTrascurrido > 0.5F)
                {
                    rec = indiceX * anchoGiro;
                    recGiro.Y = 0;
                    indiceX++;
                    tiempoTrascurrido = 0;
                    if (indiceX == 4)
                    {
                        tiempoGiro = tiempoGiro * 3;
                    }
                    if (indiceX > 4)
                    {
                        giro = false;
                        indiceX = 0;
                        tiempoGiro = tiempoGiro / 3;
                    }
                }
            }
        }*/
    }
}
