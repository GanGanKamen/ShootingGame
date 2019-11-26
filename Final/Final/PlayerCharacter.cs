using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FK_CLI;

namespace Final
{
    class PlayerCharacter
    {
        public enum ShootType
        {
            normal,
            diffusion,
            bomb
        }
        public ShootType shootType = ShootType.normal;
        public fk_Model[] muzzules = new fk_Model[3];
        public fk_Model playerModel;
        public float shootCount;
        public int hp = 5;
        public int score = 0;

        public PlayerCharacter(fk_Vector _position, fk_AppWindow appWindow)
        {
            playerModel = new fk_Model();
            playerModel.Shape = new fk_Block(20.0, 40.0, 20.0);
            playerModel.Material = fk_Material.Blue;
            playerModel.GlMoveTo(_position);
            playerModel.SmoothMode = true;
            playerModel.BMode = fk_BoundaryMode.AABB;
            playerModel.AdjustAABB();
            var playerHead = new fk_Model();
            playerHead.Shape = new fk_Sphere(4, 10);
            playerHead.Material = fk_Material.Blue;
            playerHead.Parent = playerModel;
            playerHead.GlMoveTo(new fk_Vector(0, 30, 0) + _position);
            var texture = new fk_RectTexture();
            if(texture.ReadPNG("Face.png") == false)
            {
                Console.WriteLine("File Read Error");
            }
            texture.TextureSize = new fk_TexCoord(20.0, 20.0);
            var textureModel = new fk_Model();
            textureModel.Shape = texture;
            textureModel.Material = fk_Material.White;
            textureModel.Parent = playerModel;
            textureModel.GlMoveTo(new fk_Vector(0, 70, 0) + _position);
            textureModel.GlAngle(Math.PI, -Math.PI/2, 0);
            muzzules[0] = new fk_Model();
            muzzules[0].GlMoveTo(new fk_Vector(0, 10, 15) + _position);
            muzzules[0].Parent = playerModel;
            muzzules[1] = new fk_Model();
            muzzules[1].GlMoveTo(new fk_Vector(-20, 10, 15) + _position);
            muzzules[1].Parent = playerModel;
            muzzules[2] = new fk_Model();
            muzzules[2].GlMoveTo(new fk_Vector(20, 10, 15) + _position);
            muzzules[2].Parent = playerModel;
            appWindow.Entry(playerModel);
            appWindow.Entry(playerHead);
            appWindow.Entry(textureModel);

        }
        public void InputCtrl(fk_AppWindow window)
        {
            if (window.GetKeyStatus('w', fk_SwitchStatus.PRESS)&&playerModel.Position.z <1400)
            {
                playerModel.LoTranslate(0, 0, 0.5f);
            }
            else if (window.GetKeyStatus('s', fk_SwitchStatus.PRESS) && playerModel.Position.z > -200)
            {
                playerModel.LoTranslate(0, 0, -0.5f);
            }
            if (window.GetKeyStatus('a', fk_SwitchStatus.PRESS) && playerModel.Position.x < 200)
            {
                playerModel.LoTranslate(0.5f, 0, 0);
            }
            else if (window.GetKeyStatus('d', fk_SwitchStatus.PRESS) && playerModel.Position.x > -200)
            {
                playerModel.LoTranslate(-0.5f, 0, 0);
            }
            if (window.GetSpecialKeyStatus(fk_SpecialKey.F5, fk_SwitchStatus.DOWN))
            {
                hp += 10;
            }
            /*
            if (window.GetKeyStatus(' ', fk_SwitchStatus.DOWN))
            {
                Bullet bullet = new Bullet(8, playerModel.Position, -playerModel.Vec, fk_Material.Yellow, window);
                bullets.Add(bullet);
            }*/
            if (window.GetKeyStatus(' ', fk_SwitchStatus.PRESS))
            {
                shootCount++;
            }
        }

        public void Shoot(List<Bullet> bullets,fk_AppWindow window)
        {
            if(shootCount >= 18)
            {
                shootCount = 0;
                switch (shootType)
                {
                    case ShootType.normal:
                        Bullet bullet = new Bullet(8, muzzules[0].InhPosition, -playerModel.Vec, fk_Material.Yellow,6,1, window);
                        bullets.Add(bullet);
                        break;
                    case ShootType.diffusion:
                        Bullet[] threeBullets = new Bullet[3];            
                        threeBullets[0] = new Bullet(8, muzzules[0].InhPosition, -playerModel.Vec, fk_Material.Pink,6,1, window);
                        threeBullets[1] = new Bullet(8, muzzules[1].InhPosition, -playerModel.Vec, fk_Material.Pink,6,1, window);
                        threeBullets[2] = new Bullet(8, muzzules[2].InhPosition, -playerModel.Vec, fk_Material.Pink,6, 1,window);
                        bullets.AddRange(threeBullets);
                        break;
                    case ShootType.bomb:
                        Bullet bullet2 = new Bullet(16, muzzules[0].InhPosition, -playerModel.Vec, fk_Material.Orange,8,3, window);
                        bullets.Add(bullet2);
                        break;
                }
                
            }
        }
    }
}
