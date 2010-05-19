using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.IO;
using System.Diagnostics;

using System.Xml;
using System.Xml.Serialization;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using GameObjects;

using Drawables.Textures;
using Utilities.Paths;
using Math3D;

namespace KingsReign
{
    public class AnimConfigItem
    {
        public UnitRenderer.AnimationState State = UnitRenderer.AnimationState.Idle;
        public int FPS = 4;

        public AnimConfigItem()
        {}

        public AnimConfigItem (  UnitRenderer.AnimationState s, int fps )
        {
            State = s;
            FPS = fps;
        }
    }

    public class AnimConfig
    {
        public int DefaultFPS = 4;
        public List<AnimConfigItem> AnimFPS = new List<AnimConfigItem>();

        public void LoadDefaults ()
        {
            AnimFPS.Add(new AnimConfigItem(UnitRenderer.AnimationState.Idle, DefaultFPS));
            AnimFPS.Add(new AnimConfigItem(UnitRenderer.AnimationState.Moving, DefaultFPS));
            AnimFPS.Add(new AnimConfigItem(UnitRenderer.AnimationState.Attacking, DefaultFPS));
            AnimFPS.Add(new AnimConfigItem(UnitRenderer.AnimationState.Defending, DefaultFPS));
            AnimFPS.Add(new AnimConfigItem(UnitRenderer.AnimationState.RangeAttacking, DefaultFPS));
            AnimFPS.Add(new AnimConfigItem(UnitRenderer.AnimationState.RangeDefending, DefaultFPS));
            AnimFPS.Add(new AnimConfigItem(UnitRenderer.AnimationState.Dead, DefaultFPS));
        }
    }

    public class UnitRenderer
    {
        protected static Texture GenericDead;

        public static float UnitScale = 1.0f;

        public enum AnimationState
        {
            Idle,
            Moving,
            Attacking,
            Defending,
            RangeAttacking,
            RangeDefending,
            Dead,
        }

        public RealmType Rrealm = RealmType.Unknown;

        protected Dictionary<AnimationState, Dictionary<PlayerColor,List<Texture>>> AnimationFrames = new Dictionary<AnimationState, Dictionary<PlayerColor,List<Texture>>>();
        protected Dictionary<AnimationState, int> FPSMap = new Dictionary<AnimationState, int>();

         public UnitRenderer ( string dir )
        {
            AddAnimFrames(AnimationState.Idle, FindTextures(dir, "idle"));
            AddAnimFrames(AnimationState.Moving, FindTextures(dir, "move"));
            AddAnimFrames(AnimationState.Attacking, FindTextures(dir, "attack"));
            AddAnimFrames(AnimationState.Defending, FindTextures(dir, "defend"));
            AddAnimFrames(AnimationState.RangeAttacking, FindTextures(dir, "ranged"));
            AddAnimFrames(AnimationState.RangeDefending, FindTextures(dir, "ranged_defend"));
            AddAnimFrames(AnimationState.Dead, FindTextures(dir, "die"));

            if (GenericDead == null)
                GenericDead = TextureSystem.system.GetTexture(ResourceManager.FindFile("images/units/generic/die.png"));

            // look for an anim config, if not save one
            AnimConfig config = new AnimConfig();

            FileInfo file = new FileInfo(Path.Combine(ResourceManager.FindDirectory(dir), "anims.xml"));
            if (file.Exists)
            {
                FileStream fs = file.OpenRead();
                config = (AnimConfig) new XmlSerializer(typeof(AnimConfig)).Deserialize(fs);
                fs.Close();
            }
            else
            {
                config.LoadDefaults();
                FileStream fs = file.OpenWrite();
                new XmlSerializer(typeof(AnimConfig)).Serialize(fs,config);
                fs.Close();
            }

            foreach (AnimConfigItem item in config.AnimFPS)
                FPSMap.Add(item.State, item.FPS);
        }

        protected List<Texture> FindTexturesForColor ( string dir, string name )
        {
            List<Texture> textures = new List<Texture>();

            string fname = ResourceManager.FindFile(Path.Combine(dir, name + ".png"));

            if (fname != string.Empty)
                textures.Add(TextureSystem.system.GetTexture(fname));
            else
            {
                // see if there is one with a number
                int i = 1;
                while (true)
                {
                    fname = ResourceManager.FindFile(Path.Combine(dir, name + "_" + i.ToString() + ".png"));

                    if (fname != string.Empty)
                    {
                        textures.Add(TextureSystem.system.GetTexture(fname));
                        i++;
                    }
                    else
                        break;
                }
            }

            if (textures.Count == 0)
                return null;

            return textures;
        }

        protected string GetColorDirName ( PlayerColor color, string root )
        {
            return root + "/" + color.ToString().ToLower();
        }

        protected List<Texture> GetColoredTextures (PlayerColor color, string root, string name )
        {
            List<Texture> anims = FindTexturesForColor(GetColorDirName(color, root), name);
            if (anims == null || anims.Count == 0)
                anims = FindTexturesForColor(root, name);

            return anims;
        }

        protected void AddColorFrames ( Dictionary<PlayerColor, List<Texture>> anims, PlayerColor color, List<Texture> textures )
        {
            if (textures != null && textures.Count > 0)
                anims.Add(color, textures);
        }

        protected Dictionary<PlayerColor,List<Texture>> FindTextures ( string dir, string name )
        {
            Dictionary<PlayerColor, List<Texture>> anims = new Dictionary<PlayerColor, List<Texture>>();

            AddColorFrames(anims,PlayerColor.Black, GetColoredTextures(PlayerColor.Black, dir, name));
            AddColorFrames(anims, PlayerColor.Blue, GetColoredTextures(PlayerColor.Blue, dir, name));
            AddColorFrames(anims, PlayerColor.Brown, GetColoredTextures(PlayerColor.Brown, dir, name));
            AddColorFrames(anims, PlayerColor.Green, GetColoredTextures(PlayerColor.Green, dir, name));
            AddColorFrames(anims, PlayerColor.Orange, GetColoredTextures(PlayerColor.Orange, dir, name));
            AddColorFrames(anims, PlayerColor.Purple, GetColoredTextures(PlayerColor.Purple, dir, name));
            AddColorFrames(anims, PlayerColor.Red, GetColoredTextures(PlayerColor.Red, dir, name));
            AddColorFrames(anims, PlayerColor.Teal, GetColoredTextures(PlayerColor.Teal, dir, name));
            AddColorFrames(anims, PlayerColor.White, GetColoredTextures(PlayerColor.White, dir, name));

            return anims;
        }

        void AddAnimFrames ( AnimationState state, Dictionary<PlayerColor,List<Texture>> textures )
        {
            if (textures != null && textures.Count > 0)
                AnimationFrames.Add(state, textures);
        }

        protected Texture GetFrame ( AnimationState state, PlayerColor color, double time )
        {
            List<Texture> l = AnimationFrames[state][color];

            if (l.Count == 1)
                return l[0];

            int FPS = FPSMap[state];

            double seqTime = (double)l.Count / (double)FPS;

            int fullframes = (int)(time / seqTime);
            double remainder = time - (fullframes * seqTime);

            double frameTime = 1.0 / FPS;

            int frame = (int)((remainder / seqTime) * l.Count);

            if (frame >= l.Count || frame < 0)
                throw new Exception("Bad Frame Math: WTF!");

            return l[frame];
        }

        public void Render ( AnimationState state, PlayerColor color, double time )
        {
            Texture t;
            if (AnimationFrames.ContainsKey(state))
                t = GetFrame(state, color, time);
            else
            {
                if (state == AnimationState.Dead)
                    t = GenericDead;
                else if (state == AnimationState.RangeDefending && AnimationFrames.ContainsKey(AnimationState.Defending))
                    t = GetFrame(AnimationState.Defending, color, time);
                else if (state == AnimationState.RangeAttacking && AnimationFrames.ContainsKey(AnimationState.Attacking))
                    t = GetFrame(AnimationState.Attacking, color, time);
                else
                    t = GetFrame(AnimationState.Idle, color, time);
            }

            RenderUtils.DrawImageCentered(t, UnitScale);
        }
    }

    public class UnitGraphicInstance
    {
        public UnitInstance Unit = null;
        public UnitRenderer Renderer;

        protected Stopwatch timer = new Stopwatch();

        public UnitGraphicInstance ( UnitInstance unit, UnitRenderer rendrer )
        {
            Unit = unit;
            Renderer = rendrer;
            timer.Start();
        }

        public void Draw ()
        {
            UnitRenderer.AnimationState state = UnitRenderer.AnimationState.Idle;

            if (Unit.Position != Unit.Desination)
                state = UnitRenderer.AnimationState.Moving;
            if (Unit.Damage >= 1.0f)
                state = UnitRenderer.AnimationState.Dead;

            GL.PushMatrix();
            GL.Translate(Unit.Position.X, Unit.Position.Y, 0);
            if (Unit.Player != null)
                Renderer.Render(state, Unit.Player.Color, timer.ElapsedMilliseconds / 1000.0);
            else
                Renderer.Render(state, PlayerColor.White, timer.ElapsedMilliseconds / 1000.0);

            GL.PopMatrix();
        }
    }

    public class MapVisual
    {
        public List<Texture> WorldMapTextures = new List<Texture>();
        public List<Vector2> WorldMapTexturePositions = new List<Vector2>();

        public MapVisual ( GameObjects.Map map )
        {
            int i = 0;

            Vector2 offset = new Vector2(0,0);

            foreach ( FileInfo file in map.ImageMaps )
            {
                Texture t = TextureSystem.system.GetTexture(file.FullName);
                t.Clamp = true;
              //  t.mipmap = false;
                WorldMapTextures.Add(t);

                int x = i/map.XImages;
                int y = i - (x * map.YImages);

                WorldMapTexturePositions.Add(new Vector2(offset.X,offset.Y));

                if (y == map.YImages-1)
                {
                    offset.Y = 0;
                    offset.X += t.Width;
                }
                else
                    offset.Y += t.Height;

                i++;
            }
        }
    }

    public class GameVisual
    {
        protected GLControl Control;
        protected GameClient Client;

        protected MapVisual Map = null;

        public Point CameraPos = new Point(100, 100);

        public Point SelectPoint = new Point(200, 200);

        protected Texture Selction;

        protected Texture BackgroundWater;
        protected Texture Logo;
        protected Dictionary<Map.TerrainType,Texture> TerrainFlagImages = new Dictionary<Map.TerrainType,Texture>();
        protected Texture MineImage;
        protected Color MineRingColor = Color.Goldenrod;

        protected Dictionary<RealmType, Texture> CastleImages = new Dictionary<RealmType, Texture>();
        protected Dictionary<RealmType, Texture> CampImages = new Dictionary<RealmType, Texture>();
        protected Dictionary<RealmType, Color> RealmColors = new Dictionary<RealmType, Color>();

        protected Stopwatch GraphicsTimer = new Stopwatch();

        protected List<Point> FlagMarkers = new List<Point>();

        protected Dictionary<string, UnitRenderer> UnitRenderers = new Dictionary<string, UnitRenderer>();

        public GameVisual ( GLControl ctl )
        {
            Control = ctl;

            GraphicsTimer.Start();

            SetupGL();
            Control.Paint += new System.Windows.Forms.PaintEventHandler(Control_Paint);
            Control.Resize += new EventHandler(Control_Resize);
            LoadResources();
        }

        protected void LoadResources ()
        {
            BackgroundWater = TextureSystem.system.GetTexture(ResourceManager.FindFile("images/places/water.png"));
            Logo = TextureSystem.system.GetTexture(ResourceManager.FindFile("images/logo_color_600.png"));

            Selction = TextureSystem.system.GetTexture(ResourceManager.FindFile("images/misc/illuminates-aura.png"));
            MineImage = TextureSystem.system.GetTexture(ResourceManager.FindFile("images/places/mine.png"));

            RealmColors.Add(RealmType.Arlan, Color.OrangeRed);
            CastleImages.Add(RealmType.Arlan, TextureSystem.system.GetTexture(ResourceManager.FindFile("images/places/human_castle_2.png")));
            CampImages.Add(RealmType.Arlan, TextureSystem.system.GetTexture(ResourceManager.FindFile("images/places/small_camp_2.png")));

            RealmColors.Add(RealmType.Glastonburry, Color.CornflowerBlue);
            CastleImages.Add(RealmType.Glastonburry, TextureSystem.system.GetTexture(ResourceManager.FindFile("images/places/dwarven_castle_1.png")));
            CampImages.Add(RealmType.Glastonburry, TextureSystem.system.GetTexture(ResourceManager.FindFile("images/places/fancy_camp_2.png")));

            RealmColors.Add(RealmType.Unknown, Color.DarkSlateGray);
            CastleImages.Add(RealmType.Unknown, TextureSystem.system.GetTexture(ResourceManager.FindFile("images/places/human_castle_1.png")));
            CampImages.Add(RealmType.Unknown, TextureSystem.system.GetTexture(ResourceManager.FindFile("images/places/small_camp_1.png")));

            TerrainFlagImages.Add(GameObjects.Map.TerrainType.eUnpassable, TextureSystem.system.GetTexture(ResourceManager.FindFile("images/misc/flag-red.png")));
            TerrainFlagImages.Add(GameObjects.Map.TerrainType.eForrest, TextureSystem.system.GetTexture(ResourceManager.FindFile("images/misc/flag-forest-green.png")));
            TerrainFlagImages.Add(GameObjects.Map.TerrainType.eNormal, TextureSystem.system.GetTexture(ResourceManager.FindFile("images/misc/flag-light-green.png")));
            TerrainFlagImages.Add(GameObjects.Map.TerrainType.eSand, TextureSystem.system.GetTexture(ResourceManager.FindFile("images/misc/flag-yellow.png")));
            TerrainFlagImages.Add(GameObjects.Map.TerrainType.eRiver, TextureSystem.system.GetTexture(ResourceManager.FindFile("images/misc/flag-teal.png")));
            TerrainFlagImages.Add(GameObjects.Map.TerrainType.eSea, TextureSystem.system.GetTexture(ResourceManager.FindFile("images/misc/flag-blue.png")));
            TerrainFlagImages.Add(GameObjects.Map.TerrainType.eIce, TextureSystem.system.GetTexture(ResourceManager.FindFile("images/misc/flag-white.png")));
            TerrainFlagImages.Add(GameObjects.Map.TerrainType.eMountains, TextureSystem.system.GetTexture(ResourceManager.FindFile("images/misc/flag-gray.png")));            
        }

        public void SetClient ( GameClient client )
        {
            Client = client;
            if (Client != null)
            {
                Client.MapLoaded += new GameClient.MapLoadedEvent(Client_MapLoaded);
                Client.Updated += new GameClient.UpdateEvent(Client_Updated);
                Client.UnitAdded += new GameClient.UnitChangeEvent(Client_UnitAdded);
            }
        }

        void Client_UnitAdded(GameClient sender, Player player, UnitInstance unit)
        {
            string unitGraphic = unit.Descriptor.GraphicType;
            if (!UnitRenderers.ContainsKey(unitGraphic))
                UnitRenderers.Add(unitGraphic,new UnitRenderer("images/units/" + unitGraphic));

            UnitRenderer renderer = UnitRenderers[unitGraphic];
            UnitGraphicInstance graphics = new UnitGraphicInstance(unit, renderer);
            unit.Tag = graphics;
        }

        public void IdleUpdate ()
        {
            Draw();
        }

        void Client_Updated(GameClient sender)
        {
            Draw();
        }

        void Client_MapLoaded(GameClient sender, GameObjects.Map map)
        {
            Map = new MapVisual(map);
        }

        void Control_Resize(object sender, EventArgs e)
        {
            Control.MakeCurrent();
            SetViewPort();
            Control.Invalidate();
        }

        void Control_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Draw();
        }

        protected void Redraw()
        {
            Control.Invalidate();
        }

        protected virtual void SetupGL()
        {
            Control.MakeCurrent();
            GL.ClearColor(Color.SkyBlue);

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Cw);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.LineSmooth);
            GL.Enable(EnableCap.DepthTest);

            // setup light 0
            Vector4 lightInfo = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);
            GL.Light(LightName.Light0, LightParameter.Ambient, lightInfo);

            lightInfo = new Vector4(0.7f, 0.7f, 0.7f, 1.0f);
            GL.Light(LightName.Light0, LightParameter.Diffuse, lightInfo);
            GL.Light(LightName.Light0, LightParameter.Specular, lightInfo);

            SetViewPort();
        }

        protected virtual void SetViewPort()
        {
            Control.MakeCurrent();

            GL.Viewport(0, 0, Control.Width, Control.Height); // Use all of the glControl painting area  
            SetOrthographic();
        }

        protected void SetOrthographic()
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, Control.Width, 0, Control.Height, 0, 100);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        protected void FillScreen(Texture texture, Vector2 UVShift, float UVScale )
        {
            float screenCenterX = Control.Width / 2.0f;
            float screenCenterY = Control.Height / 2.0f;

            texture.Bind();

            GL.Begin(BeginMode.Quads);

            GL.Normal3(0, 0, 1);

            GL.TexCoord2((-screenCenterX / texture.Width) * UVScale + UVShift.X, (-screenCenterY / texture.Height) * UVScale + UVShift.Y);
            GL.Vertex2(-screenCenterX, -screenCenterY);

            GL.TexCoord2((screenCenterX / texture.Width) * UVScale + UVShift.X, (-screenCenterY / texture.Height) * UVScale + UVShift.Y);
            GL.Vertex2(screenCenterX, -screenCenterY);

            GL.TexCoord2((screenCenterX / texture.Width) * UVScale + UVShift.X, (screenCenterY / texture.Height) * UVScale + UVShift.Y);
            GL.Vertex2(screenCenterX, screenCenterY);

            GL.TexCoord2((-screenCenterX / texture.Width ) * UVScale + UVShift.X, (screenCenterY / texture.Height) * UVScale + UVShift.Y);
            GL.Vertex2(-screenCenterX, screenCenterY);

            GL.End();
        }

        protected void DrawCenterMark ()
        {
            GL.Disable(EnableCap.Texture2D);
            GL.Color4(Color.Red);
            GL.Begin(BeginMode.Lines);

            GL.Vertex3(-100, 0, 0);
            GL.Vertex3(100, 0, 0);

            GL.Vertex3(0, -100, 0);
            GL.Vertex3(0, 100, 0);
            GL.End();
            GL.Enable(EnableCap.Texture2D);
        }

        protected void DrawSeletionMark ()
        {
            GL.PushMatrix();
            GL.Translate(SelectPoint.X, SelectPoint.Y, 0);
            GL.Rotate(GraphicsTimer.ElapsedMilliseconds / 5f, 0f, 0f, 1f);
            float scale = ((float)Math.Sin(GraphicsTimer.ElapsedMilliseconds / 500f) + 1.0f) * 0.5f;
            scale = 0.5f + (scale * 0.125f);
            RenderUtils.DrawImageCentered(Selction, scale);

            GL.PopMatrix();
        }


        public void MouseClick ( Point loc )
        {
            FlagMarkers.Add(loc);
        }

        protected void DrawPlace ( Point loc, Texture building, Color ring )
        {
            float radius = building.Width;
            float border = 5f;

            GL.PushMatrix();
            GL.Translate(loc.X, loc.Y, 0);

            GL.Disable(EnableCap.Texture2D);

            RenderUtils.ColorWithAlpha(ring, 0.5f);
          //  GL.Translate(0, 0, 0.1f);
            GL.Begin(BeginMode.Polygon);
            RenderUtils.CircleVerts(radius);
            GL.End();

            RenderUtils.ScaleColorWithAlpha(ring, 0.4f, 0.5f);
            GL.Begin(BeginMode.Polygon);
            RenderUtils.CircleVerts(radius + border);
            GL.End();

            GL.Translate(0, 0, 0.25f);

            GL.Color4(Color.White);
            GL.Enable(EnableCap.Texture2D);
            RenderUtils.DrawImageCentered(building);

            GL.PopMatrix();
        }

        protected void DrawCastle ( Castle castle)
        {
            if (!CastleImages.ContainsKey(castle.Realm))
                DrawPlace(castle.Location, CastleImages[RealmType.Unknown], RealmColors[RealmType.Unknown]);

            DrawPlace(castle.Location, CastleImages[castle.Realm], RealmColors[castle.Realm]);
        }

        protected void DrawPlaces ()
        {
            foreach (Mine mine in Client.WorldMap.Mines)
                DrawPlace(mine.Location, MineImage, MineRingColor);

            foreach (Castle castle in Client.WorldMap.Capitals)
                DrawCastle(castle);
        }

        protected void DrawFlags ()
        {
            foreach (Point p in FlagMarkers)
            {
                GL.PushMatrix();
                Texture t = TerrainFlagImages[Client.WorldMap.GetTerrain(p)];
                GL.Translate(p.X, p.Y-t.Height, 0);

                RenderUtils.DrawImage(t);

                GL.PopMatrix();
            }
        }

        protected void DrawArmies ()
        {
            foreach (Player player in Client.Players)
            {
                foreach (KeyValuePair<UnitType, List<UnitInstance>> unitList in player.DeployedUnits)
                {
                    foreach (UnitInstance unit in unitList.Value)
                    {
                        UnitGraphicInstance graphics = unit.Tag as UnitGraphicInstance;
                        if (graphics != null)
                            graphics.Draw();
                    }
                }
            }
        }

        protected void DrawWorld ()
        {
            GL.Translate(0, Control.Height, 0);
            GL.Scale(1, -1, 1);
            
            float screenCenterX = Control.Width / 2.0f;
            float screenCenterY = Control.Height / 2.0f;
            float tide = GraphicsTimer.ElapsedMilliseconds / 2000f;
            tide = (float)Math.Sin(tide) * 0.025f + 1.0f;

            GL.PushMatrix();
                GL.Translate(screenCenterX, screenCenterY, -25);
                Vector2 uvShift = new Vector2(CameraPos.X / (float)BackgroundWater.Width, CameraPos.Y / (float)BackgroundWater.Height);

                FillScreen(BackgroundWater, uvShift, tide);
            GL.PopMatrix();

            if (Map != null)
            {
                GL.Translate(-CameraPos.X + screenCenterX, -CameraPos.Y + screenCenterY, -20);

                // matrix is now set for the world

                GL.Color4(Color.White);
                for ( int i = 0; i < Map.WorldMapTextures.Count; i++ )
                {
                    GL.PushMatrix();
                        GL.Translate(Map.WorldMapTexturePositions[i].X, Map.WorldMapTexturePositions[i].Y, 0);
                        RenderUtils.DrawImage(Map.WorldMapTextures[i]);
                    GL.PopMatrix();
                }

                // places layer
                GL.Translate(0, 0, 1);
                DrawPlaces();

                // army layer
                GL.Translate(0, 0, 1);
                DrawArmies();

                // effects layer;
                GL.Translate(0, 0, 1);
                DrawFlags();

                // selectionLayer
                GL.Translate(0, 0, 1);
                DrawSeletionMark();
            }
            else
            {
                GL.Translate(screenCenterX, screenCenterY, -19);
                RenderUtils.DrawImageCentered(Logo);
            }
        }

        protected void DrawOverlay()
        {

        }

        protected void Draw ()
        {
            Control.MakeCurrent();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            SetOrthographic();

            GL.PushMatrix();

            GL.Enable(EnableCap.Texture2D);
            GL.Color4(Color.White);

            GL.Disable(EnableCap.Lighting);
//             GL.Enable(EnableCap.Light0);
//             Vector4 lightPos = new Vector4(10, 20, 20, 0);
//             GL.Light(LightName.Light0, LightParameter.Position, lightPos);

            DrawWorld();

            GL.PopMatrix();

            DrawOverlay();

            Control.SwapBuffers();
        }
    }

    public class RenderUtils
    {
        public static void ColorWithAlpha(Color color, float alpha)
        {
            GL.Color4(color.R / 256f, color.G / 256f, color.B / 256f, alpha);
        }

        public static void ScaleColorWithAlpha(Color color, float scale, float alpha)
        {
            GL.Color4(color.R / 256f * scale, color.G / 256f * scale, color.B / 256f * scale, alpha);
        }

        public static void DrawImage(Texture texture)
        {
            texture.Bind();
            GL.Begin(BeginMode.Quads);

            GL.Normal3(0, 0, 1);
            GL.TexCoord2(0, 0);
            GL.Vertex2(0, 0);

            GL.TexCoord2(1, 0);
            GL.Vertex2(texture.Width, 0);

            GL.TexCoord2(1, 1);
            GL.Vertex2(texture.Width, texture.Height);

            GL.TexCoord2(0, 1);
            GL.Vertex2(0, texture.Height);

            GL.End();
        }

        public static void CircleVerts(float radius)
        {
            int segments = 32;
            for (int i = 0; i < segments; i++)
                GL.Vertex2(VectorHelper2.FromAngle(i * (360f / segments), radius));
        }


        public static void DrawImageCentered(Texture texture)
        {
            texture.Bind();
            GL.Begin(BeginMode.Quads);

            GL.Normal3(0, 0, 1);
            GL.TexCoord2(0, 0);
            GL.Vertex2(-texture.Width / 2, -texture.Height / 2f);

            GL.TexCoord2(1, 0);
            GL.Vertex2(texture.Width / 2, -texture.Height / 2f);

            GL.TexCoord2(1, 1);
            GL.Vertex2(texture.Width / 2f, texture.Height / 2f);

            GL.TexCoord2(0, 1);
            GL.Vertex2(-texture.Width / 2, texture.Height / 2f);

            GL.End();
        }

        public static void DrawImageCentered(Texture texture, float scale)
        {
            texture.Bind();
            GL.Begin(BeginMode.Quads);

            float width = texture.Width / 2f * scale;
            float height = texture.Height / 2f * scale;

            GL.Normal3(0, 0, 1);
            GL.TexCoord2(0, 0);
            GL.Vertex2(-width, -height);

            GL.TexCoord2(1, 0);
            GL.Vertex2(width, -height);

            GL.TexCoord2(1, 1);
            GL.Vertex2(width, height);

            GL.TexCoord2(0, 1);
            GL.Vertex2(-width, height);

            GL.End();
        }
    }
}
