using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using Drawables.Textures;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Project24
{
    class Hud
    {
        OpenTK.Graphics.TextPrinter printer = new OpenTK.Graphics.TextPrinter();
        Font BigFont = new Font(FontFamily.GenericSansSerif,32);

        Font ChatHeaderFont = new Font(FontFamily.GenericSansSerif, 12);
        Font ChatFont = new Font(FontFamily.GenericSerif, 8);

        Game game;

        Texture Pilot;

        public static float ChatWidth = 300;
        public static float ChatHeight = 120;
        public static float ChatHeaderHeight = 24;

        public Hud (Game g)
        {
            game = g;

            Pilot = TextureSystem.system.GetTexture(ResourceManager.FindFile("pilots/Pilot2m.png"));
        }

        protected void ConnectionScreen ()
        {
            printer.Begin();
            printer.Print("Connecting!", BigFont, Color.White, new RectangleF(0, game.Height / 2, game.Width, 0), OpenTK.Graphics.TextPrinterOptions.Default, OpenTK.Graphics.TextAlignment.Center);
            printer.End();
        }

        protected void PrintText ( string text, Font font, Color color, float x, float y, float w, float h )
        {
            printer.Print(text, font, color, new RectangleF(x, game.Height-y, w, h));
        }

        void DrawChatWindow ()
        {
            string[] channels = game.Chat.GetChannelNames();

            GL.Color4(0.5f,0.5f,0.6f,0.5f);
            GL.Translate(0, 0, -1f);

            GL.Begin(BeginMode.Quads);

            GL.Vertex2(0, 0);
            GL.Vertex2(ChatWidth, 0);
            GL.Vertex2(ChatWidth, ChatHeight);
            GL.Vertex2(0, ChatHeight);

            GL.End();

            GL.Color4(Color.DarkSlateBlue);
            GL.Begin(BeginMode.LineLoop);

            GL.Vertex3(1, 1,0.1f);
            GL.Vertex3(ChatWidth+1, 1,0.1f);
            GL.Vertex3(ChatWidth+1, ChatHeight+1,0.1f);
            GL.Vertex3(1, ChatHeight+1,0.1f);

            GL.End();

            GL.Begin(BeginMode.Lines);
            GL.Vertex3(0, ChatHeight-ChatHeaderHeight, 0.1f);
            GL.Vertex3(ChatWidth + 1, ChatHeight - ChatHeaderHeight, 0.1f);

            GL.End();

            // count the tabs

            float pos = 0;
            int channel = 0;
            float buffer = 5;
            while ( pos < ChatWidth && channel < channels.Length)
            {
                float width = printer.Measure(channels[channel], ChatHeaderFont).BoundingBox.Right;
                if (pos + width + buffer+buffer < ChatWidth)
                {
                    if (game.Chat.CurrentChannel == channels[channel])
                    {
                        GL.Begin(BeginMode.Quads);

                        GL.Vertex3(pos, ChatHeight - ChatHeaderHeight,0.05f);
                        GL.Vertex3(pos + width + buffer + buffer, ChatHeight - ChatHeaderHeight, 0.05f);
                        GL.Vertex3(pos + width + buffer + buffer, ChatHeight, 0.05f);
                        GL.Vertex3(pos, ChatHeight, 0.05f);

                        GL.End();
                    }
                    printer.Begin();
                    PrintText(channels[channel], ChatHeaderFont, Color.White, pos + buffer, ChatHeight, pos + width + 2, 0);
                    printer.End();

                    GL.Begin(BeginMode.Lines);
                    GL.Vertex3(pos + width + buffer + buffer, ChatHeight, 0.1f);
                    GL.Vertex3(pos + width + buffer + buffer, ChatHeight - ChatHeaderHeight, 0.1f);

                    GL.End();
                    pos += width + buffer + buffer + 1;
                    channel++;
                }
                else
                    pos = ChatWidth;
            }

            if (game.Chat.CurrentChannel != string.Empty)
            {
                ChatChannel chatChannel = game.Chat.GetChannel(game.Chat.CurrentChannel);

                printer.Begin();

                pos = ChatHeight - ChatHeaderHeight;
                for ( int i = chatChannel.ChatMessages.Count-1; i >= 0; i-- )
                {
                    string msg = chatChannel.ChatMessages[i].Message;
                    if ( pos > 8)
                    {
                        PrintText(msg, ChatFont, Color.White, buffer, pos, ChatWidth - 2 - buffer, 0);
                        pos -= 10;
                    }
                }
                printer.End();

            }
        }

        protected void DrawInfoWidget()
        {
            if (Pilot == null)
                return;

            GL.Color4(0,0,0,0.25f);
            GL.PushMatrix();
            GL.Translate(game.Width - 136, game.Height - 136, -0.5f);

            GL.Disable(EnableCap.Texture2D);
            GL.Begin(BeginMode.Quads);
                GL.Vertex2(0, 0);
                GL.Vertex2(136, 0);
                GL.Vertex2(136, 136);
                GL.Vertex2(0, 136);

            GL.End();

            GL.Enable(EnableCap.Texture2D);
            GL.Translate(3, 3, 0.1f);
            GL.Color4(Color.White);
            Pilot.Draw(0.5f);
            GL.PopMatrix();
        }

        protected void GameHud ()
        {
            printer.Begin();
            printer.Print("Done!", BigFont, Color.White, new RectangleF(0, game.Height / 2, game.Width, 0), OpenTK.Graphics.TextPrinterOptions.Default, OpenTK.Graphics.TextAlignment.Center);
            printer.End();

            DrawChatWindow();

            DrawInfoWidget();
        }

        public void Render ( double time )
        {
            if (!game.Connected)
                ConnectionScreen();
            else
                GameHud();
        }
    }
}
