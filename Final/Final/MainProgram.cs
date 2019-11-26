using System;
using System.Collections.Generic;
using FK_CLI;

namespace Final
{
    class MainProgram
    {
        static public bool gameclear = false;

        static void Main(string[] args)
        {
            fk_Material.InitDefault();

            // ウィンドウ生成
            var window = new fk_AppWindow();
            window.Size = new fk_Dimension(800, 600);
            var camera = new fk_Model();
            window.CameraModel = camera;
            window.CameraPos = new fk_Vector(0, 400, -10);
            window.CameraFocus = new fk_Vector(0, 0, 50);
            window.Entry(camera);

            PlayerCharacter player = new PlayerCharacter(new fk_Vector(0, 0, 0), window);
            List<Bullet> fieldBullets = new List<Bullet>();
            List<Enemy> fieldEnemys = new List<Enemy>();
            List<Explosion> fieldExceptions = new List<Explosion>();

            for (int i = 0; i < 30; i+=5)
            {
                for (int j = -300; j < 300; j += 50)
                {
                    Enemy enemy = new Enemy(Enemy.Category.Dash, new fk_Vector(j, 0, i * 20 + 800), window);
                    fieldEnemys.Add(enemy);
                }

            }
            for(int i = 0; i < 4; i++)
            {
                for (int j = -200; j < 200; j += 100)
                {
                    Enemy enemy = new Enemy(Enemy.Category.Patroll, new fk_Vector(j, 0, i * 100 + 200), window);
                    fieldEnemys.Add(enemy);
                }
            }
            for (int i = 0; i < 10; i++)
            {
                Enemy enemy = new Enemy(Enemy.Category.Patroll, new fk_Vector(-200 + i*40, 0, 1300), window);
                fieldEnemys.Add(enemy);
            }

                List<Item> fieldItems = new List<Item>();
            Item item = new Item(Item.Type.diffusion, new fk_Vector(-100, 0, 100), window);
            fieldItems.Add(item);
            Item item1 = new Item(Item.Type.bomb, new fk_Vector(150, 0, 200), window);
            fieldItems.Add(item1);
            Item item2 = new Item(Item.Type.bomb, new fk_Vector(0, 0, 600), window);
            fieldItems.Add(item2);
            Item item3 = new Item(Item.Type.diffusion, new fk_Vector(-50, 0, 800), window);
            fieldItems.Add(item3);
            Item item4 = new Item(Item.Type.diffusion, new fk_Vector(-100, 0, 1200), window);
            fieldItems.Add(item4);
            Item item5 = new Item(Item.Type.bomb, new fk_Vector(100, 0, 1200), window);
            fieldItems.Add(item5);

            var hpText = new fk_SpriteModel();
            if (hpText.InitFont("rm1b.ttf") == false)
            {
                Console.Write("Font Init Error");
            }
            window.Entry(hpText);

            var texture = new fk_RectTexture();
            if (texture.ReadJPG("Stage.jpg") == false)
            {
                Console.WriteLine("File Read Error");
            }
            texture.TextureSize = new fk_TexCoord(400.0, 800.0);
            var stage = new fk_Model();
            stage.Shape = texture;
            stage.Material = fk_Material.White;
            stage.GlMoveTo(0, 0, 1000);
            stage.GlFocus(camera.Position);
            stage.GlAngle(0, -Math.PI / 2, 0);
            window.Entry(stage);

            var texture0 = new fk_RectTexture();
            if (texture0.ReadPNG("Stage0.png") == false)
            {
                Console.WriteLine("File Read Error");
            }
            texture0.TextureSize = new fk_TexCoord(400.0, 800.0);
            var stage0 = new fk_Model();
            stage0.Shape = texture0;
            stage0.Material = fk_Material.White;
            stage0.GlMoveTo(0, 0, 200);
            stage0.GlFocus(camera.Position);
            stage0.GlAngle(Math.PI, -Math.PI / 2, 0);
            window.Entry(stage0);

            var texture1 = new fk_RectTexture();
            if (texture1.ReadJPG("goal.jpg") == false)
            {
                Console.WriteLine("File Read Error");
            }
            texture1.TextureSize = new fk_TexCoord(100, 100);
            var goal = new fk_Model();
            goal.Shape = texture1;
            goal.Material = fk_Material.White;
            goal.GlMoveTo(0, 5, 1350);
            goal.GlAngle(Math.PI, -Math.PI / 2, 0);
            goal.SmoothMode = true;
            goal.BMode = fk_BoundaryMode.AABB;
            goal.AABB = new fk_Vector(50,50,50);
            window.Entry(goal);

            window.Open();



            while (window.Update())
            {
                if (player.hp <= 0)
                {
                    return;
                }
                if (gameclear == false)
                {
                    player.InputCtrl(window);
                }
                
                player.Shoot(fieldBullets, window);
                CameraLookAt(player.playerModel, camera);
                BulletCtrl();
                EnemyCtrl();
                ItemCtrl();
                TextCtrl();
                GameClear();
                
            }

            void BulletCtrl()
            {
                for (int i = 0; i < fieldBullets.Count; i++)
                {
                    fieldBullets[i].Shoot(window);
                }
            }

            void EnemyCtrl()
            {
                for (int i = 0; i < fieldBullets.Count; i++)
                {
                    for (int j = 0; j < fieldEnemys.Count; j++)
                    {
                        fieldEnemys[j].DamageCheck(player,fieldBullets[i], window);
                    }

                }
                for (int i = 0; i < fieldEnemys.Count; i++)
                {
                    fieldEnemys[i].CollideAttack(player, window);
                    fieldEnemys[i].Ability(player);
                }
            }

            void ItemCtrl()
            {
                for (int i = 0; i < fieldItems.Count; i++)
                {
                    fieldItems[i].GetItem(player, window);
                }
            }

            void TextCtrl()
            {
                hpText.DrawText("HP:" + player.hp.ToString() +"     Score"+ player.score.ToString() , true);
                hpText.SetPositionLT(-400.0, 300.0);
            }

            void GameClear()
            {
                if(goal.IsInter(player.playerModel) && gameclear == false)
                {
                    gameclear = true;
                    fieldEnemys.Clear();
                    window.Remove(goal);
                    var clearText = new fk_SpriteModel();
                    if (clearText.InitFont("rm1b.ttf") == false)
                    {
                        Console.Write("Font Init Error");
                    }
                    window.Entry(clearText);
                    clearText.DrawText("Game Clear", true);
                    hpText.SetPositionLT(0.0, 0.0);
                }
            }
        }



        static void CameraLookAt(fk_Model lookat, fk_Model camera)
        {
            camera.GlMoveTo(lookat.Position.x, camera.Position.y, lookat.Position.z);
        }
    }
}