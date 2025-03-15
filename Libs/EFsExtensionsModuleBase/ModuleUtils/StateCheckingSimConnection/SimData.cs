﻿//using Eng.EFsExtensions.EFsExtensionsModuleBase;
//using Eng.EFsExtensions.EFsExtensionsModuleBase.ModuleUtils.StateChecking;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Eng.EFsExtensions.EFsExtensionsModuleBase.ModuleUtils.StateCheckingSimConnection
//{
//    public class SimData : NotifyPropertyChangedBase
//    {
//        private const int SECONDS_PER_MINUTE = 60;

//        public SimData()
//        {
//            IsSimPaused = true;
//            EngineCombustion = new bool[4];
//            for (int i = 0; i < 4; i++)
//            {
//                EngineCombustion[i] = false;
//            }
//        }

//        public int Altitude
//        {
//            get => GetProperty<int>(nameof(Altitude))!;
//            set => UpdateProperty(nameof(Altitude), value);
//        }

//        public double BankAngle
//        {
//            get => GetProperty<double>(nameof(BankAngle))!;
//            set => UpdateProperty(nameof(BankAngle), value);
//        }

//        public string Callsign
//        {
//            get => GetProperty<string>(nameof(Callsign))!;
//            set => UpdateProperty(nameof(Callsign), value);
//        }

//        public bool[] EngineCombustion
//        {
//            get => GetProperty<bool[]>(nameof(EngineCombustion))!;
//            set => UpdateProperty(nameof(EngineCombustion), value);
//        }

//        public int GroundSpeed
//        {
//            get => GetProperty<int>(nameof(GroundSpeed))!;
//            set => UpdateProperty(nameof(GroundSpeed), value);
//        }

//        public int Height
//        {
//            get => GetProperty<int>(nameof(Height))!;
//            set => UpdateProperty(nameof(Height), value);
//        }

//        public int IndicatedSpeed
//        {
//            get => GetProperty<int>(nameof(IndicatedSpeed))!;
//            set => UpdateProperty(nameof(IndicatedSpeed), value);
//        }

//        public bool IsSimPaused
//        {
//            get => GetProperty<bool>(nameof(IsSimPaused))!;
//            set => UpdateProperty(nameof(IsSimPaused), value);
//        }
//        public bool ParkingBrakeSet
//        {
//            get => GetProperty<bool>(nameof(ParkingBrakeSet))!;
//            set => UpdateProperty(nameof(ParkingBrakeSet), value);
//        }
//        public int VerticalSpeed
//        {
//            get => GetProperty<int>(nameof(VerticalSpeed))!;
//            set => UpdateProperty(nameof(VerticalSpeed), value);
//        }

//        public bool PushbackTugConnected
//        {
//            get => GetProperty<bool>(nameof(PushbackTugConnected))!;
//            set => UpdateProperty(nameof(PushbackTugConnected), value);
//        }

//        public double Acceleration
//        {
//            get => GetProperty<double>(nameof(Acceleration))!;
//            set => UpdateProperty(nameof(Acceleration), value);
//        }

//        internal void Update(CommonDataStruct ss)
//        {
//            Callsign = ss.callsign;
//            Altitude = ss.altitude;
//            BankAngle = ss.bankAngle;
//            Height = ss.height;
//            IndicatedSpeed = ss.indicatedSpeed;
//            GroundSpeed = ss.groundSpeed;
//            VerticalSpeed = ss.verticalSpeed * SECONDS_PER_MINUTE;
//            Acceleration = (int)ss.accelerationBodyZ;
//        }

//        internal void Update(RareDataStruct s)
//        {
//            ParkingBrakeSet = s.parkingBrake != 0;
//            EngineCombustion[0] = s.engineOneCombustion != 0;
//            EngineCombustion[1] = s.engineTwoCombustion != 0;
//            EngineCombustion[2] = s.engineThreeCombustion != 0;
//            EngineCombustion[3] = s.engineFourCombustion != 0;
//            PushbackTugConnected = s.pushbackTugConnected != 0;
//        }
//    }
//}
