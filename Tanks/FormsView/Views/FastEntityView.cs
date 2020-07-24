using Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsView.View
{
    public class FastEntityView
    {
        private Graphics graphics;
        private List<Entity> entities;
        private Bitmap img;
        private int size;

        public FastEntityView(Graphics graphics, Entity entity, Bitmap img, int size) : this(graphics, new List<Entity>() { entity }, img, size)
        {
        }

        public FastEntityView(Graphics graphics, List<Entity> entities, Bitmap img, int size)
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
                graphics.DrawImage(img, entity.Pos.X, entity.Pos.Y, size, size);
            }
        }
    }
}
