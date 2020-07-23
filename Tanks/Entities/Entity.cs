using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public interface Entity
    {
        Point Pos
        {
            get;
        }
        Direction Dir
        {
            get;
        }
        void Update();
    }
}
