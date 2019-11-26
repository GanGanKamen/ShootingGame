using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FK_CLI;

namespace Final
{
    class Bullet
    {
        public fk_Model model = new fk_Model();
        public fk_Vector direction;
        private int lifeTime = 40;
        public int progressTime = 0;
        public bool isActive = true;
        private int speed = 2;
        public int damege = 1;

        public Bullet(float size, fk_Vector position, fk_Vector _direction,fk_Material color,int _speed, int _damege,fk_AppWindow appWindow)
        {
            model.Shape = new fk_Sphere(4, size);
            model.Material = color;
            model.GlMoveTo(position);
            model.SmoothMode = true;
            model.BMode = fk_BoundaryMode.SPHERE;
            model.AdjustSphere();
            direction = _direction;
            speed = _speed;
            damege = _damege;
            appWindow.Entry(model);
        }
        public void Shoot(fk_AppWindow window)
        {
            model.GlTranslate(direction * speed);
            progressTime ++; 
            if(progressTime == lifeTime)
            {
                window.Remove(model);
                isActive = false;
            }
        }
    }
}
