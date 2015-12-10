using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Crash
{
    class Mapa
    {
        private int anchoMosaico;
        public int AnchoMosaico
        {
            get
            {
                return anchoMosaico;
            }
            set
            {
                anchoMosaico = value;
            }
        }

        private int altoMosaico;
        public int AltoMosaico
        {
            get
            {
                return altoMosaico;
            }
            set
            {
                altoMosaico = value;
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

        public List<Mosaico> mosaicos = new List<Mosaico>();

        public Mapa(int anchoMosaico, int altoMosaico)
        {
            this.anchoMosaico = anchoMosaico;
            this.altoMosaico = altoMosaico;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Mosaico m in mosaicos)
            {
                m.Draw(spriteBatch);
            }
        }

        public void Generar(string nomMapa)
        {
            int longitudLinea;
            List<string> lineas = new List<string>();

            /* Leemos los caracteres del mapa definidos en un archivo de texto */
            using (Stream fStream = TitleContainer.OpenStream(@"Content\" + nomMapa))
            {
                using (StreamReader reader = new StreamReader(fStream))
                {
                    //Leemos la primera linea y vemos cuantos caracteres tiene
                    string linea = reader.ReadLine();
                    longitudLinea = linea.Length;

                    while (linea != null)
                    {
                        lineas.Add(linea);
                        //Si una linea tiene tamanio diferente de las otras lanzamos un error
                        if (linea.Length != longitudLinea)
                            throw new Exception(String.Format("La longitud de la linea {0} es diferente de las otras.", lineas.Count));
                        linea = reader.ReadLine();
                    }
                }
            }

            //Iteramos para cargar los mosaicos y agregarlos a la lista
            int yPos = 0; //-8
            foreach (string s in lineas)
            {
                int xPos = 0; //-10
                foreach (char c in s)
                {
                    Mosaico m;
                    //Creamos un mosaico de acuerdo al simbolo definido en el mapa
                    switch (c)
                    {
                        //Mosaico vacio
                        case '0':
                            break;
                        case '1':
                            m = new Mosaico("mosaico_" + c,
                                             new Rectangle(xPos * anchoMosaico, yPos * altoMosaico, anchoMosaico, altoMosaico),
                                             adminContenido, '1');
                            mosaicos.Add(m);
                            break;                            
                        case '2':
                            m = new Mosaico("mosaico_" + c.ToString(),
                                             new Rectangle(xPos * anchoMosaico, yPos * altoMosaico, anchoMosaico, altoMosaico),
                                             adminContenido, '2');
                            mosaicos.Add(m);
                            break;
                           
                        case '3':
                            m = new Mosaico("mosaico_" + c.ToString(),
                                             new Rectangle(xPos * anchoMosaico, yPos * altoMosaico, anchoMosaico, altoMosaico),
                                             adminContenido, '3');
                            mosaicos.Add(m);
                            break;
                        case '4':
                        
                            m = new Mosaico("mosaico_" + c.ToString(),
                                             new Rectangle(xPos * anchoMosaico, yPos * altoMosaico, anchoMosaico, altoMosaico),
                                             adminContenido, '4');
                            mosaicos.Add(m);
                            break;
                        case '5':

                            m = new Mosaico("mosaico_" + c.ToString(),
                                             new Rectangle(xPos * anchoMosaico + 8, yPos * altoMosaico +15, 15, 15),
                                             adminContenido, '5');
                            mosaicos.Add(m);
                            break;
                        case '6':

                            m = new Mosaico("mosaico_" + c.ToString(),
                                             new Rectangle(xPos * anchoMosaico, yPos * altoMosaico, anchoMosaico, altoMosaico),
                                             adminContenido, '6');
                            mosaicos.Add(m);
                            break;
                        case '7':

                            m = new Mosaico("mosaico_" + c.ToString(),
                                             new Rectangle(xPos * anchoMosaico, yPos * altoMosaico, anchoMosaico, altoMosaico),
                                             adminContenido, '7');
                            mosaicos.Add(m);
                            break;
                        case '8':
                            m = new Mosaico("mosaico_" + c.ToString(),
                                             new Rectangle(xPos * anchoMosaico, yPos * altoMosaico, anchoMosaico, altoMosaico),
                                             adminContenido, '8');
                            mosaicos.Add(m);
                            break;
                        case '9':
                            m = new Mosaico("mosaico_" + c.ToString(),
                                             new Rectangle(xPos * anchoMosaico, yPos * altoMosaico, anchoMosaico, altoMosaico),
                                             adminContenido, '9');
                            mosaicos.Add(m);
                            break;
                        case 'a':
                            m = new Mosaico("mosaico_" + c.ToString(),
                                             new Rectangle(xPos * anchoMosaico, yPos * altoMosaico, anchoMosaico, altoMosaico),
                                             adminContenido, 'a');
                            mosaicos.Add(m);
                            break;
                        case 'b':
                            m = new Mosaico("mosaico_" + c.ToString(),
                                             new Rectangle(xPos * anchoMosaico, yPos * altoMosaico, anchoMosaico, altoMosaico),
                                             adminContenido, 'b');
                            mosaicos.Add(m);
                            break;

                        default:
                            throw new Exception(String.Format("Error en la carga de mosaicos."));
                    }
                    xPos++;
                }
                yPos++;
            }
        }
    }
}
