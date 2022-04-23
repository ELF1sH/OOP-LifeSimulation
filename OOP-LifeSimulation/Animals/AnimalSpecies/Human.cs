using System;
using System.Collections.Generic;
using System.Drawing;

namespace OOP_LifeSimulation
{
    public class Human : OmnivorousAnimal
    {
        public HumanInventory HumanInventory { get; } = new HumanInventory();

        public bool IsWaitingForTamedAnimal { get; private set; } = false;
        public ITamable TamedAnimal { get; private set; } = null;
        public List<int> TamableAnimalsId { get; } = new List<int>();
        private int tamingAge;

        public Human Partner { get; set; } = null;
        public int PartnerId { get; set; } = -1;

        private readonly MoveToGoal__veryfast moveToGoalAlgo = new MoveToGoal__veryfast();

        public Tool Tool { get; }

        public Point constructionPos = new Point(-1, -1);
        public House House { get; private set; } = null;

        public readonly int itemsToBuild = 7;
        public Type CurResourceType { get; private set; } = null;
        public Type CurMaterialType { get; private set; } = null;
        private bool isBuildingHouse = false;

        private bool IsVillager { get; set; } = false;
        private Role Role { get; set; }
        private IWork Work { get; set; } = null;

        public bool CommingHomeToReproduce { get; private set; } = false;

        public bool IsWaitingForFemale { get; set; } = false;

        public Human(int _x, int _y, Map _map, int _animalId, bool isVillager) : base(_x, _y, _map, _animalId)
        {
            AnimalType = AnimalType.Human;
            Tool = new Random().Next(0, 3) switch
            {
                0 => new Axe(),
                1 => new Drill(),
                2 => new Pickaxe(),
                _ => new Pickaxe()
            };
            IsVillager = isVillager;
            timeNoReproduce = 20;
        }

        public override AnimalResult Update()
        {
            AnimalResult animalResult;
            animalResult = UpdateSPHP();

            if (IsVillager)
            {
                if (CurPath == CurPath.BarnToGetFood && path.Count == 0)
                {
                    GetFoodFromBarn();
                }
                Work.Execute();
                if (CurPath == CurPath.None && timeNoReproduce == 0 && Partner != null && Partner.timeNoReproduce == 0)
                {
                    ComeHomeToReproduce();
                }
                if (timeNoReproduce > 0) timeNoReproduce--;
                return animalResult;
            }

            if (HP + SP > 100 && TamedAnimal == null && !IsWaitingForTamedAnimal && !IsWaitingForFemale && CurPath == CurPath.None)
            {
                if (Partner == null && new Random().Next(0, 2) == 0)
                {
                    // searching for female
                    if (Gender == Gender.Male && timeNoReproduce == 0 && !IsWaitingForTamedAnimal && Partner == null)
                    {
                        SearchFor(SearchGoal.Female);
                        if (path.Count >= 30)
                        {
                            path.Clear();
                            CurPath = CurPath.None;
                        }
                    }
                }
                else if (timeNoReproduce == 0)
                {
                    ComeHomeToReproduce();
                }
                /*else if (new Random().Next(0, 2) == 1)
                {
                    // searching for materials to build smth
                    if (SearchFor(SearchGoal.Materials, tool: Tool))
                    {
                        CurPath = CurPath.Materials;
                    }
                }*/
            }

            if (CurPath != CurPath.None && path.Count > 0)
            {
                // move to goal (cause there is a path)
                if (!IsWaitingForTamedAnimal && !IsWaitingForFemale)
                {
                    MoveToGoal();
                }
                // sometimes some goals move, thus we need to update the path
                if (path.Count == initialPathLength / 2 && path.Count > 5)
                {
                    if (CurPath == CurPath.Food) SearchFor(SearchGoal.Food);
                    else if (CurPath == CurPath.Female) SearchFor(SearchGoal.Female);
                }
                
                // when human reaches a material resource
                if (CurPath == CurPath.Materials && path.Count == 0)
                {
                    HandleMaterial();
                }

                // when human reaches a construction
                if (CurPath == CurPath.Construction && path.Count == 0)
                {
                    if (!TryToBuild())
                    {
                        if (SearchFor(SearchGoal.Materials, resourceType: CurResourceType, tool: Tool))
                        {
                            CurPath = CurPath.Materials;
                        }
                        else CurPath = CurPath.None;
                    }
                    else
                    {
                        CurPath = CurPath.None;
                    }
                }
                // when human reaches a warehouse
                if (CurPath == CurPath.Warehouse && path.Count == 0)
                {
                    CurPath = CurPath.None;
                    AddItemsToWarehouse();
                }
                //when human reaches a home for reproduction
                if (CurPath == CurPath.Home && path.Count == 0)
                {
                    IsWaitingForFemale = true;
                    CurPath = CurPath.None;
                }
            }
            else if (!IsWaitingForTamedAnimal && !IsWaitingForFemale)
            {
                RandomMove();
            }


            if (TamedAnimal != null)
            {
                HandleTamedAnimal();
            }
            if (IsWaitingForTamedAnimal && age - tamingAge >= 15)
            {
                IsWaitingForTamedAnimal = false;
                TamableAnimalsId.Clear();
                TamedAnimal = null;
            }
            if (IsWaitingForFemale && Partner == null)
            {
                IsWaitingForFemale = false;
            }
            age++;
            if (timeNoReproduce > 0) timeNoReproduce--;
            return animalResult;
        }

        private void AddItemsToWarehouse()
        {
            var building = map.Cells[Coords.X, Coords.Y].Building;
            int size = HumanInventory.RemoveMaterials().Count;
            if (building is Smithy)
            {
                for (int i = 0; i < size; i++) (building as Smithy).Storage.Items.Add(new Iron());
            }
            else if (building is WoodWarehouse)
            {
                for (int i = 0; i < size; i++) (building as WoodWarehouse).Storage.Items.Add(new Wood());
            }
            else if (building is StoneWarehouse)
            {
                for (int i = 0; i < size; i++) (building as StoneWarehouse).Storage.Items.Add(new Stone());
            }
            else if (building is Treasury)
            {
                for (int i = 0; i < size; i++) (building as Treasury).Storage.Items.Add(new Gold());
            }
        }

        private bool TryToBuild()
        {
            map.Cells[Coords.X, Coords.Y].MaterialsOnConstruction.AddRange(HumanInventory.RemoveMaterials());
            if (map.Cells[Coords.X, Coords.Y].MaterialsOnConstruction.Count >= itemsToBuild)
            {
                map.Build(Coords, map.Cells[Coords.X, Coords.Y].MaterialsOnConstruction[0].GetType(), isBuildingHouse);
                map.Cells[Coords.X, Coords.Y].MaterialsOnConstruction.Clear();
                if (isBuildingHouse)
                {
                    House = map.Cells[constructionPos.X, constructionPos.Y].Building as House;
                }
                constructionPos.X = -1;
                constructionPos.Y = -1;
                if (isBuildingHouse)
                {
                    (map.Cells[Coords.X, Coords.Y].Building as House).AddOwner(this);
                    (map.Cells[Coords.X, Coords.Y].Building as House).AddOwner(Partner);
                    GenerateRole();
                    IsVillager = true;
                    if (Partner != null) 
                    { 
                        Partner.GenerateRole();
                        Partner.House = House;
                        Partner.IsVillager = true;
                    }
                }
                isBuildingHouse = false;
                return true;
            }
            return false;
        }

        private void ComeHomeToReproduce()
        {
            if (Gender == Gender.Male && House != null && Partner != null && Partner.HP + Partner.SP > 100)
            {
                ComeHome();
                Partner.ComeHome();
                CommingHomeToReproduce = true;
            }
        }

        public void GetItemsFromResource()
        {
            Resource resource = null;
            if (map.Cells[Coords.X, Coords.Y].Resource != null)
            {
                resource = map.Cells[Coords.X, Coords.Y].Resource;
                if (resource is StoneRock)
                {
                    HumanInventory.Storage.AddItems(
                        (resource as StoneRock).Storage.RemoveItems(HumanInventory.GetAvailableSpace())
                    );
                    if ((resource as StoneRock).Storage.Items.Count == 0)
                    {
                        map.Cells[Coords.X, Coords.Y].Resource = null;
                    }
                    CurResourceType = typeof(StoneRock);
                    CurMaterialType = typeof(Stone);
                }
                else if (resource is GoldMine)
                {
                    HumanInventory.Storage.AddItems(
                        (resource as GoldMine).Storage.RemoveItems(HumanInventory.GetAvailableSpace())
                    );
                    if ((resource as GoldMine).Storage.Items.Count == 0)
                    {
                        map.Cells[Coords.X, Coords.Y].Resource = null;
                    }
                    CurResourceType = typeof(GoldMine);
                    CurMaterialType = typeof(Gold);
                }
                else if (resource is IronOre)
                {
                    HumanInventory.Storage.AddItems(
                        (resource as IronOre).Storage.RemoveItems(HumanInventory.GetAvailableSpace())
                    );
                    if ((resource as IronOre).Storage.Items.Count == 0)
                    {
                        map.Cells[Coords.X, Coords.Y].Resource = null;
                    }
                    CurResourceType = typeof(IronOre);
                    CurMaterialType = typeof(Iron);
                }
            }
            else if (map.Cells[Coords.X, Coords.Y].Plant != null && map.Cells[Coords.X, Coords.Y].Plant is Tree)
            {
                Plant resourcePlant = null;
                resourcePlant = map.Cells[Coords.X, Coords.Y].Plant;
                HumanInventory.Storage.AddItems(
                    (resourcePlant as Tree).Storage.RemoveItems(HumanInventory.GetAvailableSpace())
                );
                if ((resourcePlant as Tree).Storage.Items.Count == 0)
                {
                    map.Cells[Coords.X, Coords.Y].Resource = null;
                }
                CurResourceType = typeof(Tree);
                CurMaterialType = typeof(Wood);
            }
        }

        private void GetFoodFromBarn()
        {
            Console.WriteLine("getting food from barn");
            if (map.Cells[Coords.X, Coords.Y].Building is Barn) 
            {
                var food = (map.Cells[Coords.X, Coords.Y].Building as Barn).GetFood();
                if (food != null)
                {
                    HumanInventory.Storage.Items.Add(food);
                }
            }
        }

        private void HandleTamedAnimal()
        {
            if (IsWaitingForTamedAnimal)
            {
                if (TamedAnimal as Animal is Horse && (TamedAnimal as Animal).Coords.X == Coords.X && (TamedAnimal as Animal).Coords.Y == Coords.Y)
                {
                    IsWaitingForTamedAnimal = false;
                    TamedAnimal.IsTamed = true;
                    TamedAnimal.Owner = this;
                    FeedTamedAnimal();
                }
            }
            else if (TamedAnimal != null && (TamedAnimal as Animal).SP <= 50)
            {
                FeedTamedAnimal();

                if ((TamedAnimal as Animal).SP == 0)
                {
                    TamedAnimal.IsTamed = false;
                    TamedAnimal.Owner = null;
                    TamedAnimal.OwnerId = -1;
                    TamableAnimalsId.Remove((TamedAnimal as Animal).AnimalId);
                    TamedAnimal = null;
                    IsWaitingForTamedAnimal = false;
                }
            }
        }

        public override void MoveToGoal()
        {
            if (TamedAnimal != null && TamedAnimal as Animal is Horse)
            {
                SetXY(path[path.Count - 1].X, path[path.Count - 1].Y);
                moveToGoalAlgo.MoveToGoal(ref path, ref CurPath);
            }
            else
            {
                base.MoveToGoal();
            }
        }

        private void FeedTamedAnimal()
        {
            foreach (var food in HumanInventory.Storage.Items)
            {
                if ((TamedAnimal as Animal is HerbivorousAnimal || TamedAnimal as Animal is OmnivorousAnimal)
                    && (food is Plant || food is Fruit))
                {
                    if (food is Plant && food is IEatable)
                    {
                        if ((food as IEatable).IsPlantPoisonous) (TamedAnimal as Animal).UpdateHP__PoisonousPlant();
                        else (TamedAnimal as Animal).UpdateHP__HealthyFood();
                    }
                    else if (food is Fruit)
                    {
                        if ((food as Fruit).IsPoisonous) (TamedAnimal as Animal).UpdateHP__PoisonousPlant();
                        else (TamedAnimal as Animal).UpdateHP__HealthyFood();
                    }
                    HumanInventory.Storage.Items.Remove(food);
                    break;
                }
                if ((TamedAnimal as Animal is CarnivorousAnimal || TamedAnimal as Animal is OmnivorousAnimal)
                    && food is Animal)
                {
                    (TamedAnimal as Animal).UpdateHP__HealthyFood();
                    HumanInventory.Storage.Items.Remove(food);
                    break;
                }
            }
        }

        private void HandleMaterial()
        {
            GetItemsFromResource();

            var warehousePos = FindClosest(Coords, SearchGoal.Warehouse, true, materialType: CurMaterialType);
            if (!isBuildingHouse && warehousePos.X != -1)
            {
                CurPath = CurPath.Warehouse;
                SearchFor(SearchGoal.Construction, x: warehousePos.X, y: warehousePos.Y);
            }
            else
            {
                if (constructionPos.X == -1)
                {
                    if (isBuildingHouse)
                    {
                        var closestBuildingPos = FindClosest(Coords, SearchGoal.ClosestBuilding, true);
                        if (closestBuildingPos.X != -1)
                        {
                            constructionPos = map.GetFreeCloseCell(closestBuildingPos, new BiomStatus[] { BiomStatus.Grass, BiomStatus.Desert }, 7);
                        }
                        else
                        {
                            constructionPos = map.GetFreeCloseCell(Coords, new BiomStatus[] { BiomStatus.Grass, BiomStatus.Desert }, 10);
                        }
                    }
                    else
                    {
                        constructionPos = map.GetFreeCloseCell(House.Coords, new BiomStatus[] { BiomStatus.Grass, BiomStatus.Desert }, 5);
                    }
                }
                SearchFor(searchGoal: SearchGoal.Construction, x: constructionPos.X, y: constructionPos.Y);
                CurPath = CurPath.Construction;
            }
        }

        public void RemoveTamedAnimal()
        {
            TamedAnimal = null;
            TamableAnimalsId.Clear();
            IsWaitingForTamedAnimal = false;
        }

        public override AnimalResult TryToEat()
        {
            if (IsWaitingForTamedAnimal || IsWaitingForFemale) return AnimalResult.Nothing;

            if (IsVillager && CurPath == CurPath.None && HumanInventory.Storage.Items.Count < HumanInventory.InventorySize && 
                House.Village.DoesHaveBarn() && House.Village.GetBarn().Storage.Items.Count > 0)
            {
                SearchFor(SearchGoal.ConcretePos, x: House.Village.GetBarn().Coords.X, y: House.Village.GetBarn().Coords.Y);
                CurPath = CurPath.BarnToGetFood;
            }

            if (CurPath == CurPath.None && HumanInventory.Storage.Items.Count < HumanInventory.InventorySize)
            {
                SearchFor(SearchGoal.Food);
            }

            // collect food in inventory
            var food = map.DoesHaveFood(Coords, FeedType, AnimalType, TamableAnimalsId);
            AnimalResult animalResult = CheckFood(food);
            if (animalResult != AnimalResult.Nothing)
            {
                HumanInventory.Storage.Items.Add(food);
            }

            // eating food from inventory
            if (SP < 50)
            {
                if (HumanInventory.Storage.Items.Count > 0)
                {
                    Unit foodToEat = HumanInventory.GetFood();
                    if (foodToEat != null)
                    {
                        if (foodToEat is Plant && foodToEat is IEatable)
                        {
                            if ((foodToEat as IEatable).IsPlantPoisonous)
                            {
                                UpdateHP__PoisonousPlant();
                            }
                            else
                            {
                                UpdateHP__HealthyFood();
                            }
                        }
                        else if (foodToEat is Fruit)
                        {
                            if ((foodToEat as Fruit).IsPoisonous)
                            {
                                UpdateHP__PoisonousPlant();
                            }
                            else
                            {
                                UpdateHP__HealthyFood();
                            }
                        }
                        else if (foodToEat is Animal)
                        {
                            UpdateHP__HealthyFood();
                        }
                    }
                }
            }

            return animalResult;
        }

        public override Animal TryToReproduce()
        {
            if (Gender == Gender.Male && !IsWaitingForTamedAnimal)
            {
                if (map.DoesHaveFemale(Coords, AnimalType))
                {
                    if (Partner == null)
                    {
                        Partner = map.ExchangePartnersInfo(Coords, AnimalId, this);
                        PartnerId = Partner.AnimalId;

                        if (SearchFor(SearchGoal.Materials, tool: Tool))
                        {
                            isBuildingHouse = true;
                            CurPath = CurPath.Materials;
                        }
                    }
                    else if (House != null && Coords == House.Coords)
                    {
                        IsWaitingForFemale = false;
                        Partner.IsWaitingForFemale = false;
                        timeNoReproduce = 20;
                        var newObj = Activator.CreateInstance(GetType(), Coords.X, Coords.Y, map, new Random().Next(2000, 100000), true);
                        (newObj as Human).GenerateRole();
                        (newObj as Human).House = House;
                        return newObj as Animal;
                    }
                }
                else if (CurPath == CurPath.None && SP > 0)
                {
                    if (PartnerId == -1) SearchFor(SearchGoal.Female);
                }
            }
            return null;
        }

        public void GenerateRole()
        {
            Role = new Random().Next(0, 2) switch
            {
                0 => Role.Builder,
                1 => Role.Collector,
                _ => Role.Builder
            };
            SetWork(Role);
        }

        private void ComeHome()
        {
            if (House != null)
            {
                SearchFor(SearchGoal.ConcretePos, x: House.Coords.X, y: House.Coords.Y);
                CurPath = CurPath.Home;
            }
        }

        private void SetWork(Role CurRole)
        {
            Work = CurRole switch
            {
                Role.Builder => new Build(this),
                Role.Collector => new Collect(this),
                _ => new Build(this)
            };
        }
    }
}