using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FK_CLI;

namespace Final
{
    class Item
    {
        public enum Type
        {
            diffusion,
            bomb
        }
        public Type type;
        public fk_Model model = new fk_Model();
        public bool isActive = true;
        public Item(Type _type,fk_Vector position,fk_AppWindow window)
        {
            type = _type;
            model.Shape = new fk_Block(15, 15, 15);           
            model.GlMoveTo(position);
            model.SmoothMode = true;
            model.BMode = fk_BoundaryMode.AABB;
            model.AdjustAABB();
            switch (type)
            {
                case Type.diffusion:
                    model.Material = fk_Material.Pink;
                    break;
                case Type.bomb:
                    model.Material = fk_Material.Orange;
                    break;
            }
            window.Entry(model);
        }

        public void GetItem(PlayerCharacter player,fk_AppWindow window)
        {
            if(model.IsInter(player.playerModel)&&isActive == true)
            {
                isActive = false;
                window.Remove(model);
                switch (type)
                {
                    case Type.diffusion:
                        player.shootType = PlayerCharacter.ShootType.diffusion;
                        break;
                    case Type.bomb:
                        player.shootType = PlayerCharacter.ShootType.bomb;
                        break;
                }
            }
        }
    }
}
