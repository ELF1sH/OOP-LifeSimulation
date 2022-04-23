using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_LifeSimulation
{
    public class Drawer
    {
        private readonly GameForm gameForm;
        private readonly PictureBox fieldPictureBox;
        private Bitmap bmp;
        private Graphics graph;

        private readonly int fieldSize;
        private int _pictureBoxWidth;
        public int PictureBoxWidth {
            get
            {
                return _pictureBoxWidth;
            }
            internal set 
            {
                _pictureBoxWidth = value;
                cellSize = value / fieldSize;
                bmp = new Bitmap(fieldPictureBox.Width, fieldPictureBox.Height);
                graph = Graphics.FromImage(bmp);
            } 
        }
        private int cellSize;

        private Image seedsImage;
        private Image sproutImage;

        private Image bushImage;
        private Image bushRottedImage;
        private Image carrotImage;
        private Image carrotRottedImage;
        private Image poisonousMushroomImage;
        private Image poisonousMushroomRottedImage;
        private Image appleTreeImage;
        private Image appleTreeRottedImage;
        private Image poisonousBerryTreeImage;
        private Image poisonousBerryTreeRottedImage;
        private Image peasImage;
        private Image peasRottedImage;
        private Image treeImage;

        private Image appleFruitImage;
        private Image peasFruitImage;
        private Image poisonousBerryImage;

        private Image horseImage;
        private Image bearImage;
        private Image rabbitImage;
        private Image elephantImage;
        private Image hedgehogImage;
        private Image leopardImage;
        private Image monkeyImage;
        private Image tigerImage;
        private Image wolfImage;

        private Image manImage;
        private Image womanImage;

        private Image goldMineImage;
        private Image ironOreImage;
        private Image stoneRockImage;

        private Image houseImage;
        private Image warehouseGoldImage;
        private Image warehouseIronImage;
        private Image warehouseStoneImage;
        private Image warehouseWoodImage;
        private Image barnImage;

        private readonly Color snowColor = ColorTranslator.FromHtml("#B9F1FF");
        private readonly Color iceColor = ColorTranslator.FromHtml("#87ABFF");

        private readonly Map map;

        public Drawer(GameForm _gameForm, int _fieldSize, Map _map)
        {
            gameForm = _gameForm;
            fieldPictureBox = gameForm.FieldPictureBox;
            bmp = new Bitmap(fieldPictureBox.Width, fieldPictureBox.Height);
            graph = Graphics.FromImage(bmp);
            map = _map;

            fieldSize = _fieldSize;
            _pictureBoxWidth = fieldPictureBox.Width;
            cellSize = _pictureBoxWidth / fieldSize;

            GetAndResizeImages();
        }

        private void GetAndResizeImages()
        {
            seedsImage = ResizeImage(Image.FromFile("../../../Resources/Plants/Seeds.png"), cellSize, cellSize);
            sproutImage = ResizeImage(Image.FromFile("../../../Resources/Plants/Sprout.png"), cellSize, cellSize);

            bushImage = ResizeImage(Image.FromFile("../../../Resources/Plants/Bush.png"), cellSize, cellSize);
            bushRottedImage = ResizeImage(Image.FromFile("../../../Resources/Plants/BushRotted.png"), cellSize, cellSize);
            carrotImage = ResizeImage(Image.FromFile("../../../Resources/Plants/Carrot.png"), cellSize, cellSize);
            carrotRottedImage = ResizeImage(Image.FromFile("../../../Resources/Plants/CarrotRotted.png"), cellSize, cellSize);
            poisonousMushroomImage = ResizeImage(Image.FromFile("../../../Resources/Plants/PoisonousMushroom.png"), cellSize, cellSize);
            poisonousMushroomRottedImage = ResizeImage(Image.FromFile("../../../Resources/Plants/PoisonousMushroomRotted.png"), cellSize, cellSize);
            appleTreeImage = ResizeImage(Image.FromFile("../../../Resources/Plants/Apple/AppleTree.png"), cellSize, cellSize);
            appleTreeRottedImage = ResizeImage(Image.FromFile("../../../Resources/Plants/Apple/AppleTreeRotted.png"), cellSize, cellSize);
            poisonousBerryTreeImage = ResizeImage(Image.FromFile("../../../Resources/Plants/PoisonousBerry/PoisonousBerryBush.png"), cellSize, cellSize);
            poisonousBerryTreeRottedImage = ResizeImage(Image.FromFile("../../../Resources/Plants/PoisonousBerry/PoisonousBerryBushRotted.png"), cellSize, cellSize);
            peasImage = ResizeImage(Image.FromFile("../../../Resources/Plants/Peas/PeasPlant.png"), cellSize, cellSize);
            peasRottedImage = ResizeImage(Image.FromFile("../../../Resources/Plants/Peas/PeasPlantRotted.png"), cellSize, cellSize);
            treeImage = ResizeImage(Image.FromFile("../../../Resources/Plants/Tree.png"), cellSize, cellSize);

            appleFruitImage = ResizeImage(Image.FromFile("../../../Resources/Plants/Apple/AppleFruit.png"), cellSize, cellSize);
            peasFruitImage = ResizeImage(Image.FromFile("../../../Resources/Plants/Peas/PeasFruit.png"), cellSize, cellSize);
            poisonousBerryImage = ResizeImage(Image.FromFile("../../../Resources/Plants/PoisonousBerry/PoisonousBerry.png"), cellSize, cellSize);

            horseImage = ResizeImage(Image.FromFile("../../../Resources/Animals/Horse.png"), cellSize, cellSize);
            bearImage = ResizeImage(Image.FromFile("../../../Resources/Animals/Bear.png"), cellSize, cellSize);
            rabbitImage = ResizeImage(Image.FromFile("../../../Resources/Animals/Rabbit.png"), cellSize, cellSize);
            elephantImage = ResizeImage(Image.FromFile("../../../Resources/Animals/Elephant.png"), cellSize, cellSize);
            hedgehogImage = ResizeImage(Image.FromFile("../../../Resources/Animals/Hedgehog.png"), cellSize, cellSize);
            leopardImage = ResizeImage(Image.FromFile("../../../Resources/Animals/Leopard.png"), cellSize, cellSize);
            monkeyImage = ResizeImage(Image.FromFile("../../../Resources/Animals/Monkey.png"), cellSize, cellSize);
            tigerImage = ResizeImage(Image.FromFile("../../../Resources/Animals/Tiger.png"), cellSize, cellSize);
            wolfImage = ResizeImage(Image.FromFile("../../../Resources/Animals/Wolf.png"), cellSize, cellSize);
            
            manImage = ResizeImage(Image.FromFile("../../../Resources/Animals/Man.png"), cellSize, cellSize);
            womanImage = ResizeImage(Image.FromFile("../../../Resources/Animals/Woman.png"), cellSize, cellSize);

            goldMineImage = ResizeImage(Image.FromFile("../../../Resources/Resources/Gold.png"), cellSize, cellSize);
            ironOreImage = ResizeImage(Image.FromFile("../../../Resources/Resources/Iron.png"), cellSize, cellSize);
            stoneRockImage = ResizeImage(Image.FromFile("../../../Resources/Resources/Stone.png"), cellSize, cellSize);

            houseImage = ResizeImage(Image.FromFile("../../../Resources/Buildings/House.png"), cellSize, cellSize);
            warehouseGoldImage = ResizeImage(Image.FromFile("../../../Resources/Buildings/Warehouse_gold.png"), cellSize, cellSize);
            warehouseIronImage = ResizeImage(Image.FromFile("../../../Resources/Buildings/Warehouse_iron.png"), cellSize, cellSize);
            warehouseStoneImage = ResizeImage(Image.FromFile("../../../Resources/Buildings/Warehouse_stone.png"), cellSize, cellSize);
            warehouseWoodImage = ResizeImage(Image.FromFile("../../../Resources/Buildings/Warehouse_wood.png"), cellSize, cellSize);
            barnImage = ResizeImage(Image.FromFile("../../../Resources/Buildings/Barn.png"), cellSize, cellSize);
        }

        public void UpdateZoom()
        {
            bmp = new Bitmap(fieldPictureBox.Width, fieldPictureBox.Height);
            graph = Graphics.FromImage(bmp);

            _pictureBoxWidth = fieldPictureBox.Width;
            cellSize = _pictureBoxWidth / fieldSize;

            GetAndResizeImages();
            PaintMap();
        }

        public void PaintMap()
        {
            var cells = map.Cells;
            for (int i = 0; i < fieldSize; i++)
            {
                for (int j = 0; j < fieldSize; j++)
                {
                    PaintCell(i, j, cells[i, j].BiomStatus);
                }
            }
        }

        private Color GetColorByBiomStatus(BiomStatus bs)
        {
            if (bs == BiomStatus.Water && map.CurSeason == Season.Summer) return Color.Blue;
            if (bs == BiomStatus.Water && map.CurSeason == Season.Winter) return iceColor;
            if (bs == BiomStatus.Grass && map.CurSeason == Season.Summer) return Color.Green;
            if (bs == BiomStatus.Grass && map.CurSeason == Season.Winter) return snowColor;
            if (bs == BiomStatus.Desert && map.CurSeason == Season.Summer) return Color.LightGoldenrodYellow;
            if (bs == BiomStatus.Desert && map.CurSeason == Season.Winter) return snowColor;
            if (bs == BiomStatus.Hill) return Color.SaddleBrown;
            return Color.White;
        }

        private void PaintCell(int x, int y, BiomStatus biomStatus)
        {
            var solidBrush = new SolidBrush(GetColorByBiomStatus(biomStatus));
            var rect = new Rectangle(x * cellSize, y * cellSize, cellSize, cellSize);
            graph.FillRectangle(solidBrush, rect);
        }

        public void UpdatePictureBox()
        {
            fieldPictureBox.Image = bmp;
        }



        private Image ResizeImage(Image image, int new_height, int new_width)
        {
            var new_image = new Bitmap(new_width, new_height);
            var g = Graphics.FromImage((Image)new_image);
            g.InterpolationMode = InterpolationMode.High;
            g.DrawImage(image, 0, 0, new_width, new_height);
            return new_image;
        }

        public void DrawPlant(int x, int y, PlantType plantType, PlantStatus plantStatus)
        {
            Image img = seedsImage;
            switch (plantStatus)
            {
                case PlantStatus.Sprout:
                    img = sproutImage;
                    break;
                case PlantStatus.Adult:
                    switch (plantType)
                    {
                        case PlantType.Bush: img = bushImage; break;
                        case PlantType.Carrot: img = carrotImage; break;
                        case PlantType.PoisonousMushroom: img = poisonousMushroomImage; break;
                        case PlantType.AppleTree: img = appleTreeImage; break;
                        case PlantType.PoisonousBerryBush: img = poisonousBerryTreeImage; break;
                        case PlantType.Peas: img = peasImage; break;
                        case PlantType.Tree: img = treeImage; break;
                    }
                    break;
                case PlantStatus.Rotted:
                    switch (plantType)
                    {
                        case PlantType.Bush: img = bushRottedImage; break;
                        case PlantType.Carrot: img = carrotRottedImage; break;
                        case PlantType.PoisonousMushroom: img = poisonousMushroomRottedImage; break;
                        case PlantType.AppleTree: img = appleTreeRottedImage; break;
                        case PlantType.PoisonousBerryBush: img = poisonousBerryTreeRottedImage; break;
                        case PlantType.Peas: img = peasRottedImage; break;
                        case PlantType.Tree: img = treeImage; break;
                    }
                    break;
                default:
                    img = seedsImage;
                    break;
            }
            graph.DrawImage(img, new Point(x * cellSize, y * cellSize));
        }

        public void RedrawPlant(int x, int y, PlantType plantType, PlantStatus plantStatus, BiomStatus biomStatus)
        {
            PaintCell(x, y, biomStatus);
            DrawPlant(x, y, plantType, plantStatus);
        }

        public void DrawFruit(int x, int y, PlantType plantType)
        {
            Image img = null;
            switch (plantType) 
            {
                case PlantType.AppleTree:
                    img = appleFruitImage;
                    break;
                case PlantType.Peas:
                    img = peasFruitImage;
                    break;
                case PlantType.PoisonousBerryBush:
                    img = poisonousBerryImage;
                    break;
            }
            graph.DrawImage(img, new Point(x * cellSize, y * cellSize));
        }

        public void DrawAnimal(int x, int y, AnimalType at, int HP, int SP, Gender gender)
        {
            Image img;
            switch (at)
            {
                case AnimalType.Horse: img = horseImage; break;
                case AnimalType.Elephant: img = elephantImage; break;
                case AnimalType.Rabbit: img = rabbitImage; break;
                case AnimalType.Wolf: img = wolfImage; break;
                case AnimalType.Leopard: img = leopardImage; break;
                case AnimalType.Tiger: img = tigerImage; break;
                case AnimalType.Hedgehog: img = hedgehogImage; break;
                case AnimalType.Monkey: img = monkeyImage; break;
                case AnimalType.Bear: img = bearImage; break;
                case AnimalType.Human:
                    {
                        if (gender == Gender.Male)
                        {
                            img = manImage; break;
                        }
                        else
                        {
                            img = womanImage; break;
                        }
                    } 
                default: img = bearImage; break;
            }
            img = ResizeImage(img, cellSize, cellSize);
            graph.DrawImage(img, new Point(x * cellSize, y * cellSize));
            graph.DrawString(HP.ToString(), new Font("Courier New", cellSize / 2.75F), new SolidBrush(Color.Red), new Point((x) * cellSize, (y - 1) * cellSize));
            graph.DrawString(SP.ToString(), new Font("Courier New", cellSize / 2.75F), new SolidBrush(Color.Orange), new Point((x) * cellSize, (y - 1) * cellSize + 10));
        }

        public void DrawResource(Point p, Type resourceType)
        {
            Image img = null;
            if (resourceType == typeof(StoneRock)) img = stoneRockImage;
            else if (resourceType == typeof(IronOre)) img = ironOreImage;
            else img = goldMineImage;
            graph.DrawImage(img, new Point(p.X * cellSize, p.Y * cellSize));
        }

        public void DrawBuilding(Point p, Type buildingType)
        {
            Image img = null;
            if (buildingType == typeof(Treasury)) img = warehouseGoldImage;
            else if (buildingType == typeof(WoodWarehouse)) img = warehouseWoodImage;
            else if (buildingType == typeof(StoneWarehouse)) img = warehouseStoneImage;
            else if (buildingType == typeof(Smithy)) img = warehouseIronImage;
            else if (buildingType == typeof(Barn)) img = barnImage;
            else img = houseImage;
            graph.DrawImage(img, new Point(p.X * cellSize, p.Y * cellSize));
        }

        private void DrawMaterial(Point p, Type materialType)
        {
            Image img = null;
            if (materialType == typeof(Stone)) img = stoneRockImage;
            else if (materialType == typeof(Iron)) img = ironOreImage;
            else if (materialType == typeof(Gold)) img = goldMineImage;
            else img = treeImage;
            graph.DrawImage(img, new Point(p.X * cellSize, p.Y * cellSize));
        }

        public void AnimalMove(int prevX, int prevY, int x, int y, AnimalType at, Gender gender, int HP, int SP)
        {
            PaintCell(prevX, prevY, map.GetBiomStatusCell(prevX, prevY));

            var animal = map.GetAnimal(new Point(prevX, prevY));
            if (animal != null && !animal.HasBeenEaten && prevX != x && prevY != y)
            {
                DrawAnimal(prevX, prevY, animal.AnimalType, animal.HP, animal.SP, animal.Gender);
            }
            if (map.Cells[prevX, prevY].Resource != null)
            {
                DrawResource(new Point(prevX, prevY), map.Cells[prevX, prevY].Resource.GetType());
            }
            if (map.Cells[prevX, prevY].Building != null)
            {
                DrawBuilding(new Point(prevX, prevY), map.Cells[prevX, prevY].Building.GetType());
            }
            if (map.Cells[prevX, prevY].MaterialsOnConstruction.Count > 0)
            {
                DrawMaterial(new Point(prevX, prevY), map.Cells[prevX, prevY].MaterialsOnConstruction[0].GetType());
            }

            if (prevY - 1 >= 0)
            {
                PaintCell(prevX, prevY - 1, map.GetBiomStatusCell(prevX, prevY - 1));
                /*                animal = map.GetAnimal(new Point(prevX, prevY));
                                if (animal != null && !animal.HasBeenEaten && prevX != x && prevY != y)
                                {
                                    DrawAnimal(prevX, prevY, animal.AnimalType, animal.HP, animal.SP, animal.Gender);
                                }*/
                if (map.Cells[prevX, prevY - 1].Resource != null)
                {
                    DrawResource(new Point(prevX, prevY - 1), map.Cells[prevX, prevY - 1].Resource.GetType());
                }
                if (map.Cells[prevX, prevY - 1].Building != null)
                {
                    DrawBuilding(new Point(prevX, prevY - 1), map.Cells[prevX, prevY - 1].Building.GetType());
                }
                if (map.Cells[prevX, prevY - 1].MaterialsOnConstruction.Count > 0)
                {
                    DrawMaterial(new Point(prevX, prevY - 1), map.Cells[prevX, prevY - 1].MaterialsOnConstruction[0].GetType());
                }
            }
            DrawAnimal(x, y, at, HP, SP, gender);
        }

        public void PaintOverAnimal(Point p)
        {
            PaintCell(p.X, p.Y - 1, map.GetBiomStatusCell(p.X, p.Y));
            PaintCell(p.X, p.Y, map.GetBiomStatusCell(p.X, p.Y));
        }
    }
}
