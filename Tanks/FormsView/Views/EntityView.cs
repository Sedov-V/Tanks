using Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsView.Views
{
    public class EntityView
    {
        private Graphics graphics;
        private List<Entity> entities;
        private Bitmap img;
        private int size;

        public EntityView(Graphics graphics, Entity entity, Bitmap img, int size) : this(graphics, new List<Entity>() { entity }, img, size)
        {
        }

        public EntityView(Graphics graphics, List<Entity> entities, Bitmap img, int size)
        {
            this.graphics = graphics;
            this.entities = entities;
            this.img = img;
            this.size = size;
        }

        public void Draw()
        {
            foreach (Entity entity in entities)
            {
                graphics.DrawImage(RotateImage(new Bitmap(img), entity.Dir), entity.Pos.X, entity.Pos.Y, size, size);
            }
        }

        private Bitmap RotateImage(Bitmap b, Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                    b.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
                case Direction.Down:
                    b.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case Direction.Left:
                    b.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
            }
            return b;
        }
    }
}
