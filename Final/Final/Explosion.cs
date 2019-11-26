using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FK_CLI;

namespace Final
{
    class Explosion
    {
        public fk_Model model = new fk_Model();
        private int lifeTime = 10;
        public int progressTime = 0;
        public bool isActive = true;

        public Explosion(fk_Vector position, fk_AppWindow appWindow)
        {
            model.Shape = new fk_Sphere(4, 16);
            model.Material = fk_Material.DimYellow;
            model.GlMoveTo(position);
            model.SmoothMode = true;
            model.BMode = fk_BoundaryMode.SPHERE;
            model.AdjustSphere();
            appWindow.Entry(model);
        }

        public void Scattering(Enemy enemy, fk_AppWindow window)
        {
            progressTime++;
            if (progressTime == lifeTime)
            {
                window.Remove(model);
                isActive = false;
            }
        }
    }
}
