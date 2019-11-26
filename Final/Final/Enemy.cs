using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FK_CLI;

namespace Final
{
    class Enemy
    {
        public enum Category
        {
            Dash,
            Patroll
        }

        public Category category;
        public fk_Model model = new fk_Model();
        public int hp = 2;
        public bool isActive = true;
        public bool ability = false;
        private bool preAbility;

        public Enemy(Category _category,fk_Vector position,fk_AppWindow window)
        {
            category = _category;
            switch (category)
            {
                case Category.Dash:
                    model.Shape = new fk_Sphere(4, 12);
                    model.Material = fk_Material.Red;
                    model.GlMoveTo(position);
                    model.SmoothMode = false;
                    model.BMode = fk_BoundaryMode.SPHERE;
                    model.AdjustSphere();
                    window.Entry(model);
                    ability = false;
                    preAbility = ability;
                    hp = 2;
                    break;
                case Category.Patroll:
                    model.Shape = new fk_Sphere(4, 18);
                    model.Material = fk_Material.Red;
                    model.GlMoveTo(position);
                    model.SmoothMode = false;
                    model.BMode = fk_BoundaryMode.SPHERE;
                    model.AdjustSphere();
                    window.Entry(model);
                    ability = false;
                    preAbility = ability;
                    hp = 3;
                    break;
            }

        }

        public void DamageCheck(PlayerCharacter player, Bullet bullet, fk_AppWindow window)
        {
            if (model.IsInter(bullet.model)&&bullet.isActive == true&&isActive == true)
            {
                hp-= bullet.damege;
                window.Remove(bullet.model);
                bullet.isActive = false;
            }
            if(hp <= 0 && isActive == true)
            {
                player.score += 100;
                window.Remove(model);
                isActive = false;
            }
        }

        public void CollideAttack(PlayerCharacter player, fk_AppWindow window)
        {
            if (model.IsInter(player.playerModel) && isActive == true)
            {
                player.hp--;
                window.Remove(model);
                isActive = false;
            }
        }

        public void Ability(PlayerCharacter player)
        {
            
            if (Math.Sqrt(Math.Pow(model.Position.x - player.playerModel.Position.x, 2) 
                + Math.Pow(model.Position.z - player.playerModel.Position.z, 2)) < 200)
            {
                ability = true;
            }
            switch (category)
            {
                case Category.Dash:
                    if(preAbility != ability)
                    {
                        preAbility = ability;
                        model.GlFocus(new fk_Vector(player.playerModel.Position.x, model.Position.y, player.playerModel.Position.z));
                    }
                    if(ability == true)
                    {
                        model.GlTranslate(model.Vec * 0.5f);
                        model.GlFocus(new fk_Vector(player.playerModel.Position.x, model.Position.y, player.playerModel.Position.z));
                    }
                    
                    break;
                case Category.Patroll:
                    if (preAbility != ability)
                    {
                        preAbility = ability;
                        model.GlFocus(new fk_Vector(player.playerModel.Position.x, model.Position.y, player.playerModel.Position.z));
                    }
                    if (ability == true)
                    {
                        model.GlTranslate(model.Vec * 1f);
                    }
                    break;
            }
        }
    }
}
