using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.IO;

using Simulation;

using Drawables.Textures;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Project23
{
    class HudRenderer
    {
        OpenTK.Graphics.TextPrinter printer = new OpenTK.Graphics.TextPrinter();
        Font StatusFont = new Font(FontFamily.GenericSansSerif,24);

        Font ChatHeaderFont = new Font(FontFamily.GenericSansSerif, 12);
        Font ChatFont = new Font(FontFamily.GenericSerif, 8);

        Font PlayerFont = new Font(FontFamily.GenericSansSerif, 18);
      
        Font PlayerListHeaderFont = new Font(FontFamily.GenericSansSerif, 12);
        Font PlayerListFont = new Font(FontFamily.GenericSerif, 8);

        Font LagstatsFont = new Font(FontFamily.GenericMonospace, 6);

        Game game;

        Texture Pilot;

        Texture RadarFrame;
        Texture RadarBackground;
        Texture RadarSweep;

        float radarAngle = 0;

        public static float RadarSize = 128;
        public static float RadarSpeed = 25f;

        public static float RadarOpacity = 0.5f;

        public static float ChatWidth = 300;
        public static float ChatHeight = 120;
        public static float ChatHeaderHeight = 24;
        public static float ChatLineHeight = 18;
        public static float ChatLogLineHeight = 12;
        public static float ChatStartLineOffset = 18;

        public static float PlayerInfoHeight = 24;
        public static float PlayerInfoBorder = 5;
        public static float PlayerInfoPicture = 128;
        public static float PlayerInfoNameHegith = 32;

        public static float PlayerListWidth = 150;
        public static float PlayerListFlySpeed = 250;
        public static float PlayerListHeaderHeight = 24;
        public static float PlayerListItemHeight = 18;

        public bool ShowTimestamps = true;

        public bool ChatMode = false;

        float NameWidth = 0;

        string statusText = string.Empty;

        enum PlayerListStatus
        {
            Holding,
            Activating,
            Deactivating,
        }
        PlayerListStatus playerListStatus = PlayerListStatus.Holding;
        float playerListPosition = 0;

        public HudRenderer (Game g)
        {
            game = g;

            RadarFrame = TextureSystem.system.GetTexture(ResourceManager.FindFile("ui/radar/overlay.png"));
            RadarSweep = TextureSystem.system.GetTexture(ResourceManager.FindFile("ui/radar/sweep.png"));
            RadarBackground = TextureSystem.system.GetTexture(ResourceManager.FindFile("ui/radar/background.png"));
        }

        public void Resize(int width, int height)
        {
            if (width > 1024)
                RadarSize = 256;
            else
                RadarSize = 128;
        }

        public void SetStatusText ( string text )
        {
            statusText = text;
        }

        protected void PrintText ( string text, Font font, Color color, float x, float y, float w, float h )
        {
            printer.Print(text, font, color, new RectangleF(x, game.Height-y, w, h));
        }

        protected void PrintTextCentered(string text, Font font, Color color, float x, float y, float w, float h)
        {
            printer.Print(text, font, color, new RectangleF(x, game.Height - y, w, h), OpenTK.Graphics.TextPrinterOptions.Default, OpenTK.Graphics.TextAlignment.Center);
        }

        protected void PrintTextRight (string text, Font font, Color color, float x, float y, float w, float h)
        {
            printer.Print(text, font, color, new RectangleF(x, game.Height - y, w, h), OpenTK.Graphics.TextPrinterOptions.Default, OpenTK.Graphics.TextAlignment.Far);
        }

        public void SetPlayerData ( Player player )
        {
            Pilot = TextureSystem.system.GetTexture(ResourceManager.FindFile(Path.Combine("pilots", player.Pilot + ".png")));
            if (Pilot == null)
                Pilot = TextureSystem.system.GetTexture(ResourceManager.FindFile(Path.Combine("pilots", "Pilot0u.png")));

            NameWidth = printer.Measure(game.Client.ThisPlayer.Callsign, PlayerFont).BoundingBox.Width;
        }

        void DrawChatWindow ()
        {
            string[] channels = game.Chat.GetChannelNames();

            GL.Disable(EnableCap.Texture2D);

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

                pos = ChatLineHeight + ChatStartLineOffset;
                for (int i = chatChannel.ChatMessages.Count - 1; i >= 0; i--)
                {
                    string message = string.Empty;
                    if (ShowTimestamps)
                        message = chatChannel.ChatMessages[i].TimeStamp.ToString() + " ";
                    message += chatChannel.ChatMessages[i].From + ": " + chatChannel.ChatMessages[i].Message;
                    PrintText(message, ChatFont, Color.White, buffer, pos, ChatWidth - 2 - buffer, 0);
                    pos += ChatLogLineHeight;
                    if (pos > ChatHeight - ChatHeaderHeight)
                        i = -1;
                }

                printer.End();

            }

            if (ChatMode)
                GL.Color3(Color.Yellow);

            GL.Begin(BeginMode.Lines);
            GL.Vertex3(0, ChatLineHeight, 0.1f);
            GL.Vertex3(ChatWidth + 1, ChatLineHeight, 0.1f);
            GL.End();

            if (game.OutgoingChatString != string.Empty)
            {
                printer.Begin();
                PrintText(game.OutgoingChatString, ChatFont, Color.LightGoldenrodYellow, buffer, ChatLineHeight, ChatWidth - 2 - buffer, 0);
                printer.End();
            }
        }

        protected void DrawLagstats()
        {
            float TotalHeight = PlayerInfoPicture + PlayerInfoBorder;
            printer.Begin();
            string lagLine = "L:" + (game.Client.LastLatency * 1000).ToString("F2") + "ms AL:" + (game.Client.AverageLatency * 1000).ToString("F2") + "ms J:" + (game.Client.Jitter * 1000).ToString("F2") + "ms PL:" + game.Client.Packetloss.ToString("F2") + "%";

 //           if (game.Client.ThisPlayer != null)
 //               lagLine += " F:" + game.Client.ThisPlayer.forAftSpeed.ToString("F2") + " I:" + game.Client.ThisPlayer.intendedSpeed.X.ToString("F2");
            PrintText(lagLine, LagstatsFont, Color.White, 0, game.Height - TotalHeight, PlayerInfoPicture + PlayerInfoBorder + PlayerInfoBorder, PlayerInfoHeight + 10);
            printer.End();
        }

        protected void DrawInfoWidget()
        {
            if (Pilot == null)
                return;

            float TotalHeight = PlayerInfoPicture + PlayerInfoHeight + PlayerInfoBorder + PlayerInfoBorder;

            GL.Color4(0,0,0,0.25f);
            GL.PushMatrix();
            GL.Translate(0, game.Height - TotalHeight, -0.5f);

            float pictureBoxWidth = PlayerInfoPicture + PlayerInfoBorder + PlayerInfoBorder;

            GL.Disable(EnableCap.Texture2D);
            GL.Begin(BeginMode.Quads);
                GL.Vertex2(0, 0);
                GL.Vertex2(pictureBoxWidth, 0);
                GL.Vertex2(pictureBoxWidth, TotalHeight);
                GL.Vertex2(0, TotalHeight);

                GL.Vertex2(pictureBoxWidth, TotalHeight - PlayerInfoNameHegith);
                GL.Vertex2(pictureBoxWidth + NameWidth + 10, TotalHeight - PlayerInfoNameHegith);
                GL.Vertex2(pictureBoxWidth + NameWidth + 10, TotalHeight);
                GL.Vertex2(pictureBoxWidth, TotalHeight);

            GL.End();

            GL.Enable(EnableCap.Texture2D);
            GL.Translate(PlayerInfoBorder, PlayerInfoHeight + PlayerInfoBorder, 0.1f);
            GL.Color4(Color.White);
            Pilot.DrawAtWidth(PlayerInfoPicture);
            GL.PopMatrix();

            printer.Begin();
            PrintText(game.Client.ThisPlayer.Callsign, PlayerFont, Color.White, pictureBoxWidth, game.Height, NameWidth + 20, 0);
            printer.End();

            DrawLagstats();
        }

        public void SetChatMode(bool mode)
        {
            ChatMode = mode;
        }

        public void ActivatePlayerList (bool val )
        {
            if (val) // want to turn it on
                playerListStatus = PlayerListStatus.Activating;
            else
                playerListStatus = PlayerListStatus.Deactivating;
        }

        public void TogglePlayerList ( )
        {
            if (playerListStatus == PlayerListStatus.Deactivating)
                playerListStatus = PlayerListStatus.Activating;
            else if (playerListStatus == PlayerListStatus.Activating)
                playerListStatus = PlayerListStatus.Deactivating;
            else if (playerListPosition > 0)
                playerListStatus = PlayerListStatus.Deactivating;
            else
                playerListStatus = PlayerListStatus.Activating;
        }

        protected void DrawPlayerList(double time)
        {
            // figure out the position
            if (playerListStatus == PlayerListStatus.Activating)
                playerListPosition += (float)time * PlayerListFlySpeed;
            else if (playerListStatus == PlayerListStatus.Deactivating)
                playerListPosition -= (float)time * PlayerListFlySpeed;
            else if (playerListStatus == PlayerListStatus.Holding && playerListPosition > 0)
                playerListPosition = PlayerListWidth;

            if (playerListPosition >= PlayerListWidth)
            {
                playerListPosition = PlayerListWidth;
                playerListStatus = PlayerListStatus.Holding;
            }
            else if (playerListPosition <=0)
            {
                playerListPosition = 0;
                playerListStatus = PlayerListStatus.Holding;
            }

            float headerHeight = game.Height - 140;
            if (playerListPosition > 0)
            {
                GL.Disable(EnableCap.Texture2D);

                int playerCount = game.Client.sim.Players.Count;

                float listHeight = PlayerListHeaderHeight + PlayerListItemHeight * playerCount;

                GL.PushMatrix();
                GL.Translate(playerListPosition, headerHeight, -0.5f);

                GL.Color4(0, 0, 0, 0.5f);

                GL.Begin(BeginMode.Quads);
                    GL.Vertex2(-PlayerListWidth, -listHeight);
                    GL.Vertex2(0, -listHeight);
                    GL.Vertex2(0, 0);
                    GL.Vertex2(-PlayerListWidth, 0);
                GL.End();

                GL.Translate(0, 0, 0.1f);
                GL.Color4(1f, 1f, 1f, 1f);
                GL.Begin(BeginMode.LineLoop);
                    GL.Vertex2(-PlayerListWidth, -listHeight);
                    GL.Vertex2(0, -listHeight);
                    GL.Vertex2(0, 0);
                    GL.Vertex2(-PlayerListWidth, 0);
                GL.End();

                GL.Begin(BeginMode.Lines);
                    GL.Vertex2(-PlayerListWidth + 5, -PlayerListHeaderHeight);
                    GL.Vertex2(-5, -PlayerListHeaderHeight);

                    for (int i = 1; i < playerCount; i++)
                    {
                        GL.Vertex2(-PlayerListWidth + 50, -PlayerListHeaderHeight - (PlayerListItemHeight*i));
                        GL.Vertex2(-50, -PlayerListHeaderHeight - (PlayerListItemHeight * i));
                    }
                GL.End();

                printer.Begin();
                PrintText("Players", PlayerListHeaderFont, Color.White, playerListPosition - PlayerListWidth + 2, headerHeight, PlayerListWidth, 0);

                for (int i = 0; i < playerCount; i++)
                {
                    Player player = game.Client.sim.Players[i];
                    PrintText(player.Callsign + " (" + player.Score.ToString() +")", PlayerListFont, Color.White, playerListPosition - PlayerListWidth + 10, headerHeight - PlayerListHeaderHeight - (PlayerListItemHeight * i), PlayerListWidth, 0);
                }
                printer.End();

                GL.PopMatrix();
            }
        }

        void DrawStatusText ()
        {
            if (statusText == string.Empty)
                return;

            GL.PushMatrix();
            GL.Disable(EnableCap.Texture2D);
            GL.Translate(0, 0, -0.25f);
            GL.Color4(0, 0, 0, 0.5f);

            GL.Begin(BeginMode.Quads);
                GL.Vertex2(50, game.Height/2-24);
                GL.Vertex2(game.Width - 50, game.Height / 2 - 24);
                GL.Vertex2(game.Width - 50, game.Height / 2 + 24);
                GL.Vertex2(50, game.Height / 2 + 24);
            GL.End();

            printer.Begin();
            printer.Print(statusText, StatusFont, Color.White, new RectangleF(0, game.Height / 2 - 24, game.Width - 10, 48), OpenTK.Graphics.TextPrinterOptions.Default, OpenTK.Graphics.TextAlignment.Center);

            printer.End();
            GL.PopMatrix();
        }

        protected void DrawRadar ( double time )
        {
            GL.Color4(1f, 1f, 1f, RadarOpacity);
            GL.Enable(EnableCap.Texture2D);
            GL.PushMatrix();
            GL.Translate(game.Width - RadarSize, game.Height - RadarSize, -0.5f);

            RadarBackground.DrawAtWidth(RadarSize);
            GL.Translate(0,0, 0.01f);
            GL.PushMatrix();

            radarAngle += RadarSpeed * (float)time;
            if (radarAngle > 360f)
                radarAngle = radarAngle - 360f;
            GL.Translate(RadarSize / 2f, RadarSize / 2f, 0);
            GL.Rotate(radarAngle, 0, 0, 1f);
            GL.Translate(-RadarSize / 2f, -RadarSize / 2f, 0);
            RadarSweep.DrawAtWidth(RadarSize);

            GL.PopMatrix();
            GL.Translate(0, 0, 0.01f);
            RadarFrame.DrawAtWidth(RadarSize);

            GL.PopMatrix();
        }

        protected void GameHud(double time)
        {
            DrawChatWindow();

            DrawInfoWidget();

            DrawPlayerList(time);

            DrawRadar(time);
        }

        public void Render ( double time )
        {
            GL.Disable(EnableCap.Lighting);

            DrawStatusText();

            if (game.Joined)
                GameHud(time);
        }
    }
}
