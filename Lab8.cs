using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data.Odbc;

namespace HavestFarm
{
    [Serializable]
    public abstract class Product
    {
        public float Cost { get; set; }
        public float Value { get; set; }
        public DateTime Start { get; set; }
        public TimeSpan Duration { get; set; }
        public float FertilizerCost { get; set; }
        public float WaterCost { get; set; }

        public abstract void Seed();
        public abstract float Harvest();
    }
    [Serializable]
    public class Wheat : Product
    {
        public int NumFertilizer { get; set; }
        public int NumWater { get; set; }

        public int MaxFertilizerPerCycle { get; set; } = 1; // Số lần bón phân mỗi chu kỳ
        public int MaxWaterPerCycle { get; set; } = 2; // Số lần tưới nước mỗi chu kỳ

        public Wheat()
        {
            Cost = 100;
            Value = 300;
            FertilizerCost = 10;
            WaterCost = 5;
            Duration = TimeSpan.FromSeconds(10); // 10 giây chăm bón
        }

        public override void Seed()
        {
            Start = DateTime.Now;
            Console.WriteLine("Gieo trồng lúa mì...");
        }

        public void FeedAutomatically()
        {
            double elapsedSeconds = (DateTime.Now - Start).TotalSeconds;
            int cycles = (int)(elapsedSeconds / Duration.TotalSeconds);

            NumFertilizer = Math.Min(MaxFertilizerPerCycle * cycles, MaxFertilizerPerCycle);
            Console.WriteLine($"Tự động bón phân cho lúa mì. Số lần bón phân: {NumFertilizer}/{MaxFertilizerPerCycle}");
        }

        public void ProvideWaterAutomatically()
        {
            double elapsedSeconds = (DateTime.Now - Start).TotalSeconds;
            int cycles = (int)(elapsedSeconds / Duration.TotalSeconds);

            NumWater = Math.Min(MaxWaterPerCycle * cycles, MaxWaterPerCycle);
            Console.WriteLine($"Tự động tưới nước cho lúa mì. Số lần tưới: {NumWater}/{MaxWaterPerCycle}");
        }

        public override float Harvest()
        {
            FeedAutomatically();
            ProvideWaterAutomatically();

            if (DateTime.Now >= Start.Add(Duration))
            {
                float totalCost = NumFertilizer * FertilizerCost + NumWater * WaterCost;
                float profit = Value - totalCost;
                Console.WriteLine($"Đã thu hoạch lúa mì. Hao phí tổng: {totalCost}. Lợi nhuận: {profit}");
                return profit;
            }
            else
            {
                throw new InvalidOperationException("Chưa đến thời gian thu hoạch.");
            }
        }
    }
    [Serializable]
    public class Tomato : Product
    {
        public int NumFertilizer { get; set; }
        public int NumWater { get; set; }

        public Tomato()
        {
            Cost = 150;
            Value = 400;
            FertilizerCost = 15;
            WaterCost = 7;
            Duration = TimeSpan.FromSeconds(15);
        }

        public override void Seed()
        {
            Start = DateTime.Now;
            Console.WriteLine("Gieo trồng cà chua...");
        }

        public override float Harvest()
        {
            if (DateTime.Now >= Start.Add(Duration))
            {
                float totalCost = Cost + NumFertilizer * FertilizerCost + NumWater * WaterCost;
                float profit = Value - totalCost;
                Console.WriteLine($"Đã thu hoạch {this.GetType().Name}.");
                Console.WriteLine($"Tổng chi phí: {totalCost}, gồm có: ");
                Console.WriteLine($"- Chi phí gieo trồng: {Cost}");
                Console.WriteLine($"- Phí phân bón ({NumFertilizer} lần): {NumFertilizer * FertilizerCost}");
                Console.WriteLine($"- Phí tưới nước ({NumWater} lần): {NumWater * WaterCost}");
                Console.WriteLine("Lợi nhuận: " + profit);
                return profit;
            }
            else
            {
                throw new InvalidOperationException("Chưa đến thời gian thu hoạch.");
            }
        }

        public void Feed()
        {
            NumFertilizer++;
            Console.WriteLine($"Đã bón phân cho {this.GetType().Name}. Chi phí hiện tại cho phân bón: {NumFertilizer * FertilizerCost}");
        }

        public void ProvideWater()
        {
            NumWater++;
            Console.WriteLine($"Đã tưới nước cho {this.GetType().Name}. Chi phí hiện tại cho tưới nước: {NumWater * WaterCost}");
        }

    }
    [Serializable]
    public class Sunflower : Product
    {
        public int NumFertilizer { get; set; }
        public int NumWater { get; set; }

        public Sunflower()
        {
            Cost = 250;
            Value = 450;
            FertilizerCost = 8;
            WaterCost = 4;
            Duration = TimeSpan.FromSeconds(20);
        }

        public override void Seed()
        {
            Start = DateTime.Now;
            Console.WriteLine("Gieo trồng hoa hướng dương...");
        }

        public override float Harvest()
        {
            if (DateTime.Now >= Start.Add(Duration))
            {
                float totalCost = Cost + NumFertilizer * FertilizerCost + NumWater * WaterCost;
                float profit = Value - totalCost;
                Console.WriteLine($"Đã thu hoạch {this.GetType().Name}.");
                Console.WriteLine($"Tổng chi phí: {totalCost}, gồm có: ");
                Console.WriteLine($"- Chi phí gieo trồng: {Cost}");
                Console.WriteLine($"- Phí phân bón ({NumFertilizer} lần): {NumFertilizer * FertilizerCost}");
                Console.WriteLine($"- Phí tưới nước ({NumWater} lần): {NumWater * WaterCost}");
                Console.WriteLine("Lợi nhuận: " + profit);
                return profit;
            }
            else
            {
                throw new InvalidOperationException("Chưa đến thời gian thu hoạch.");
            }
        }

        public void Feed()
        {
            NumFertilizer++;
            Console.WriteLine($"Đã bón phân cho {this.GetType().Name}. Chi phí hiện tại cho phân bón: {NumFertilizer * FertilizerCost}");
        }

        public void ProvideWater()
        {
            NumWater++;
            Console.WriteLine($"Đã tưới nước cho {this.GetType().Name}. Chi phí hiện tại cho tưới nước: {NumWater * WaterCost}");
        }

    }
    [Serializable]
    public class Player
    {
        public string UserName { get; set; }
        public float Reward { get; set; }
        public List<Product> Inventory { get; set; }

        public Player(string userName, float reward)
        {
            UserName = userName;
            Reward = reward;
            Inventory = new List<Product>();
        }

        public void PlantProduct(Product product)
        {
            if (Reward >= product.Cost)
            {
                Reward -= product.Cost;
                Inventory.Add(product);
                product.Seed();
                Console.WriteLine($"Đã gieo trồng {product.GetType().Name}. Số điểm còn lại: {Reward}");
            }
            else
            {
                Console.WriteLine("Không đủ điểm để mua vật phẩm này.");
            }
        }

        public void HarvestProduct(Product product)
        {
            try
            {
                float profit = product.Harvest();
                Reward += profit;
                Inventory.Remove(product);
                Console.WriteLine($"Thu hoạch xong. Ví tiền hiện tại: {Reward}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
    [Serializable]
    class Program
    {
        private const string SaveFileName = "playerdata.bin";
        static void SaveGame(Player player)
        {
            using (FileStream fs = new FileStream(SaveFileName, FileMode.Create))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, player);
            }
            Console.WriteLine("Đã lưu game thành công");
        }
        static Player LoadGame()
        {
            if (File.Exists(SaveFileName))
            {
                using (FileStream fs = new FileStream(SaveFileName, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    return (Player)formatter.Deserialize(fs);
                }
            }
            return null;
        }
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Player player = LoadGame();
            if (player == null)
            {
                Console.Write("Nhập tên người chơi: ");
                string playerName = Console.ReadLine();
                player = new Player(playerName, 500);
            }
            else
            {
                Console.WriteLine($"Chào mừng trở lại, {player.UserName}");
            }
            bool gameRunning = true;

            while (gameRunning)
            {
                //Console.Clear();
                Console.WriteLine("\n--- HarvestFarm Menu ---");
                Console.WriteLine("1. Gieo trồng lúa mì - 10s - 100$");
                Console.WriteLine("2. Gieo trồng cà chua - 15s - 150$");
                Console.WriteLine("3. Gieo trồng hoa hướng dương - 20s - 250$");
                Console.WriteLine("4. Thu hoạch sản phẩm");
                Console.WriteLine("5. Lưu thông tin");
                Console.WriteLine("6. Thoát game");
                Console.Write("Vui lòng chọn một tùy chọn: ");

                int choice;
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            Wheat wheat = new Wheat();
                            player.PlantProduct(wheat); // Thêm vào Inventory qua PlantProduct
                            break;

                        case 2:
                            Tomato tomato = new Tomato();
                            player.PlantProduct(tomato);
                            break;

                        case 3:
                            Sunflower sunflower = new Sunflower();
                            player.PlantProduct(sunflower);
                            break;

                        case 4:
                            Console.WriteLine("Các sản phẩm có thể thu hoạch:");
                            for (int i = 0; i < player.Inventory.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {player.Inventory[i].GetType().Name}");
                            }
                            Console.Write("Chọn sản phẩm để thu hoạch: ");
                            int harvestChoice;
                            if (int.TryParse(Console.ReadLine(), out harvestChoice) && harvestChoice > 0 && harvestChoice <= player.Inventory.Count)
                            {
                                player.HarvestProduct(player.Inventory[harvestChoice - 1]);
                            }
                            else
                            {
                                Console.WriteLine("Lựa chọn không hợp lệ.");
                            }
                            break;

                        case 5:
                            Console.Clear();
                            SaveGame(player);
                            return;
                        case 6:
                            gameRunning = false;
                            Console.WriteLine("Thoát game. Cảm ơn bạn đã chơi!");
                            break;

                        default:
                            Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng thử lại.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Vui lòng nhập một số hợp lệ.");
                }
            }
        }
    }
}

