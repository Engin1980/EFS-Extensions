﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace FailuresModule.Model.Sim
//{
//    //TODO get rid of this, probably too complicated
//    public abstract class SimConPoint
//    {
//        public abstract string SimPointName { get; }
//    }

//    public class EventSimConPoint : SimConPoint
//    {
//        public string SimEvent { get; }
//        public override string SimPointName => SimEvent;

//        public EventSimConPoint(string simEvent)
//        {
//            SimEvent = simEvent;
//        }
//    }

//    public class VarSimConPoint : SimConPoint
//    {
//        public string SimVar { get; }
//        public override string SimPointName => SimVar;

//        public VarSimConPoint(string simVar)
//        {
//            SimVar = simVar;
//        }
//    }
//}
