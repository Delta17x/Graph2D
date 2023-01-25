using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCS
{
    public abstract class MGraphicsObject
    {
        private static List<MGraphicsObject> _objects = new List<MGraphicsObject>();
        public static List<MGraphicsObject> GraphicsObjects { get { return _objects; } private set { _objects = value; } }
        private int index;
        public MWindow Window { get; set; }   
        /// <summary>
        /// Where the position of whatever is drawn is relative to (i.e. MAnchor.Right would cause a rectangle centered at (0, 0) to be drawn at the right edge of the screen.)
        /// </summary>
        public MAnchor Anchor { get; set; }
        public int AnchorBorder { get; set; }
        public virtual MColor DrawColor { get; set; }
        private bool visible = true;
        public bool Visible { get => visible; set { visible = value; } }
        public List<MGraphicsObject> Children { get; set; }

        public MGraphicsObject(MWindow window, MAnchor anchor, int anchorBorder, MColor drawColor)
        {
            Window = window;
            Anchor = anchor;
            AnchorBorder = anchorBorder;
            DrawColor = drawColor;
            index = _objects.Count;
            _objects.Add(this);
            Children = new List<MGraphicsObject>();
        }

        public MGraphicsObject(MWindow window)
        {
            Window = window;
            index = _objects.Count;
            _objects.Add(this);
            Children = new List<MGraphicsObject>();
        }

        public void Destroy()
        {
            _objects.RemoveAt(index);
            for (int i = index; i < _objects.Count; i++)
            {
                _objects[i].index--;
            }
            index = -1;
        }

        protected Vector2i AnchorPosition()
        {
            return Window.PositionOfAnchor(Anchor, AnchorBorder);
        }
        public abstract void Draw();
    }
}
