using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using FivePD.API;
using FivePD.API.Utils;

namespace YouTubeLiveCallout
{
    [CalloutProperties("Pursuit YouTube Live", "BGHDDevelopment", "1.0")]
    public class Pursuit : Callout
    {
        private Vehicle car;
        private Ped ped1;
        private string[] carList = { "speedo", "speedo2", "stanier", "stinger", "stingergt", "stratum", "stretch", "taco", "tornado", "tornado2", "tornado3", "tornado4", "tourbus", "vader", "voodoo2", "dune5", "youga", "taxi", "tailgater", "sentinel2", "sentinel", "sandking2", "sandking", "ruffian", "rumpo", "rumpo2", "oracle2", "oracle", "ninef2", "ninef", "minivan", "gburrito", "emperor2", "emperor"};

        public Pursuit()
        {
            
            Random rnd = new Random();
            float offsetX = rnd.Next(100, 700);
            float offsetY = rnd.Next(100, 700);
            InitInfo(World.GetNextPositionOnStreet(Game.PlayerPed.GetOffsetPosition(new Vector3(offsetX, offsetY, 0))));
            ShortName = "Pursuit";
            CalloutDescription = "A fast pursuit";
            ResponseCode = 3;
            StartDistance = 150f;
        }

        public async override void OnStart(Ped player)
        {
            base.OnStart(player);
            ped1 = await SpawnPed(RandomUtils.GetRandomPed(), Location + 2);
            Random random = new Random();
            string cartype = carList[random.Next(carList.Length)];
            VehicleHash Hash = (VehicleHash) API.GetHashKey(cartype);
            car = await SpawnVehicle(Hash, Location);
            ped1.SetIntoVehicle(car, VehicleSeat.Driver);
            ped1.AlwaysKeepTask = true;
            ped1.BlockPermanentEvents = true;
            car.AttachBlip();
            ped1.AttachBlip();
            API.SetDriveTaskMaxCruiseSpeed(ped1.GetHashCode(), 35f);
            API.SetDriveTaskDrivingStyle(ped1.GetHashCode(), 524852);
            ped1.Task.FleeFrom(player);
            FivePD.API.Pursuit.RegisterPursuit(ped1);
        }
        public async override Task OnAccept()
        {
            InitBlip();
            UpdateData();
        }
    }
}