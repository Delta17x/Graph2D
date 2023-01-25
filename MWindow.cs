using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Reflection.Metadata.Ecma335;
using static MathCS.MWindow;

namespace MathCS
{
    // Uses GPU a lot when not visible??
    /// <summary>
    /// 
    /// </summary>
    public class MWindow : GameWindow
    {
        private int texID;

        private int fboID;

        public MColor[] screenBuffer;

        public MColor Background { get; set; } = MColor.White;

        public MWindow(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title })
        {
            screenBuffer = new MColor[width * height];
        }

        public MColor this[int i, int j]
        {
            get
            {
                return screenBuffer[j * Size.X + i];
            }
            set
            {
                screenBuffer[j * Size.X + i] = value;
            }
        }

        private void HandleMGraphicObjects()
        {
            for (int i = 0; i < MGraphicsObject.GraphicsObjects.Count; i++)
            {              
                var obj = MGraphicsObject.GraphicsObjects[i];
                if (obj.Visible)
                    obj.Draw();
            }
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            float[] temp = new float[Size.X * Size.Y * 3];
            for (int i = 0; i < Size.X * Size.Y; i++)
            {
                temp[3 * i] = Background.R;
                temp[3 * i + 1] = Background.G;
                temp[3 * i + 2] = Background.B;
            }

            texID = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texID);
            
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, Size.X, Size.Y, 0, PixelFormat.Rgb, PixelType.Float, 0);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            fboID = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, fboID);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);
            GL.FramebufferTexture2D(FramebufferTarget.ReadFramebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, texID, 0);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, Size.X, Size.Y, 0, PixelFormat.Rgb, PixelType.Float, temp);
            GL.FramebufferTexture2D(FramebufferTarget.ReadFramebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, texID, 0);
            GL.BlitFramebuffer(0, 0, Size.X, Size.Y, 0, 0, Size.X, Size.Y, ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Nearest);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            HandleMGraphicObjects();

            unsafe 
            {
                fixed (MColor* p = &screenBuffer[0])
                {
                    float* p2 = (float*)p;
                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, Size.X, Size.Y, 0, PixelFormat.Rgb, PixelType.Float, (IntPtr)p2);
                }
            }
            //GL.FramebufferTexture2D(FramebufferTarget.ReadFramebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, texID, 0);
            GL.BlitFramebuffer(0, 0, Size.X, Size.Y, 0, 0, Size.X, Size.Y, ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Nearest);
            Array.Fill(screenBuffer, Background);
            SwapBuffers();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
            screenBuffer = new MColor[Size.X * Size.Y];
            for (int i = 0; i < screenBuffer.Length; i++)
            {
                screenBuffer[i] = Background;
            }
        }

        public Vector2i PositionOfAnchor(MAnchor anchor, int anchorBorder)
        {
            int x = Size.X;
            int y = Size.Y;
            return anchor switch
            {
                MAnchor.BottomLeft => new Vector2i(anchorBorder, anchorBorder),
                MAnchor.Bottom => new Vector2i(x / 2, anchorBorder),
                MAnchor.BottomRight => new Vector2i(x - anchorBorder, anchorBorder),
                MAnchor.Left => new Vector2i(anchorBorder, y / 2),
                MAnchor.Center => new Vector2i(x / 2, y / 2),
                MAnchor.Right => new Vector2i(x - anchorBorder, y / 2),
                MAnchor.TopLeft => new Vector2i(anchorBorder, y - anchorBorder),
                MAnchor.Top => new Vector2i(x / 2, y - anchorBorder),
                MAnchor.TopRight => new Vector2i(x - anchorBorder, y - anchorBorder),
                _ => new Vector2i(0, 0),
            };
        }

        public bool IsOnScreen(int x, int y)
        {
            return x > 0 && y > 0 && x < Size.X && y < Size.Y;
        }

        public bool IsOnScreen(float x, float y)
        {
            return x > 0 && y > 0 && x < Size.X && y < Size.Y;
        }

        public bool IsOnScreen(Vector2i p)
        {
            return p.X > 0 && p.Y > 0 && p.X < Size.X && p.Y < Size.Y;
        }
    }
}