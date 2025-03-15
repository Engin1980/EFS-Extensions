﻿using ESimConnect;
using ESimConnect.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SimDataCapturer
{
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
  public struct MockPlaneData
  {
    #region Fields
    [DataDefinition(SimVars.Aircraft.Engine.ENG_ON_FIRE__index + "1")]
    public int ENG_ON_FIRE__index;

    [DataDefinition(SimVars.Aircraft.Control.FLAPS_HANDLE_INDEX__index + "1")]
    public int FLAPS_HANDLE_INDEX__index;

    [DataDefinition(SimVars.Aircraft.Control.TRAILING_EDGE_FLAPS_LEFT_INDEX)]
    public int TRAILING_EDGE_FLAPS_LEFT_INDEX;

    [DataDefinition(SimVars.Aircraft.Control.LEADING_EDGE_FLAPS_LEFT_PERCENT)]
    public double LEADING_EDGE_FLAPS_LEFT_PERCENT;

    [DataDefinition(SimVars.Aircraft.Control.TRAILING_EDGE_FLAPS_LEFT_PERCENT)]
    public double TRAILING_EDGE_FLAPS_LEFT_PERCENT;

    [DataDefinition(SimVars.Aircraft.Engine.ENG_COMBUSTION__index + "1")]
    public int engineOneCombustion;

    [DataDefinition(SimVars.Aircraft.Fuel.FUELSYSTEM_TANK_LEVEL__index + "1")]
    public double fuelLevel1;

    [DataDefinition(SimVars.Aircraft.Fuel.FUEL_TANK_LEFT_MAIN_LEVEL)]
    public double fuelLevelLeftMain;

    [DataDefinition(SimVars.Aircraft.Fuel.FUEL_TANK_RIGHT_MAIN_LEVEL)]
    public double fuleLevelRightMain;

    [DataDefinition(SimVars.Aircraft.Fuel.FUEL_TANK_CENTER_LEVEL)]
    public double fuelLevelCenter;

    //[DataDefinition(SimVars.Aircraft.Miscelaneous.PLANE_ALTITUDE, SimUnits.Length.FOOT)]
    //public int altitude;

    //[DataDefinition(SimVars.Aircraft.Miscelaneous.PLANE_ALT_ABOVE_GROUND, SimUnits.Length.FOOT)]
    //public int height;

    //[DataDefinition(SimVars.Aircraft.Miscelaneous.ACCELERATION_BODY_X, SimUnits.Acceleration.FEET_PER_SECOND_SQUARED)]
    //public double accelerationBodyX;

    //[DataDefinition(SimVars.Aircraft.Miscelaneous.ACCELERATION_BODY_Y, SimUnits.Acceleration.FEET_PER_SECOND_SQUARED)]
    //public double accelerationBodyY;

    //[DataDefinition(SimVars.Aircraft.Miscelaneous.ACCELERATION_BODY_Z, SimUnits.Acceleration.FEET_PER_SECOND_SQUARED)]
    //public double accelerationBodyZ;

    //[DataDefinition(SimVars.Aircraft.Miscelaneous.PLANE_BANK_DEGREES, SimUnits.Angle.DEGREE)]
    //public double bankAngle;

    //[DataDefinition(SimVars.Aircraft.Miscelaneous.PLANE_PITCH_DEGREES, SimUnits.Angle.DEGREE)]
    //public double pitchAngle;

    //[DataDefinition(SimVars.Aircraft.Miscelaneous.GROUND_VELOCITY, SimUnits.Speed.KNOT)]
    //public int groundSpeed;

    //[DataDefinition(SimVars.Aircraft.Miscelaneous.AIRSPEED_INDICATED, SimUnits.Speed.KNOT)]
    //public int indicatedSpeed;

    //[DataDefinition(SimVars.Aircraft.Miscelaneous.AIRSPEED_TRUE, SimUnits.Speed.KNOT)]
    //public int trueSpeed;

    //[DataDefinition(SimVars.Aircraft.Miscelaneous.VERTICAL_SPEED, SimUnits.Length.FOOT)]
    //public int verticalSpeed;

    //[DataDefinition(SimVars.Aircraft.Miscelaneous.PLANE_HEADING_DEGREES_MAGNETIC, SimUnits.Angle.DEGREE)]
    //public int planeHeading;

    //[DataDefinition(SimVars.Aircraft.Control.FLAP_POSITION_SET)]
    //public int flapsPositionSet;



    //[DataDefinition(SimVars.Aircraft.RadioAndNavigation.ATC_ID)]
    //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    //public string callsign;




    //[DataDefinition(SimVars.Aircraft.Engine.ENG_COMBUSTION__index + "1")]
    //public int engineOneCombustion;

    //[DataDefinition(SimVars.Aircraft.Engine.ENG_COMBUSTION__index + "2")]
    //public int engineTwoCombustion;

    //[DataDefinition(SimVars.Aircraft.Engine.ENG_COMBUSTION__index + "3")]
    //public int engineThreeCombustion;

    //[DataDefinition(SimVars.Aircraft.Engine.ENG_COMBUSTION__index + "4")]
    //public int engineFourCombustion;



    //[DataDefinition(SimVars.Aircraft.BrakesAndLandingGear.BRAKE_PARKING_POSITION)]
    //public int parkingBrake;

    //[DataDefinition(SimVars.Services.PUSHBACK_ATTACHED)]
    //public int pushbackTugConnected;

    //[DataDefinition(SimVars.Aircraft.FlightModel.G_FORCE)]
    //public double gForce;

    //[DataDefinition(SimVars.Aircraft.FlightModel.STALL_ALPHA)]
    //public double stallAlpha;

    //[DataDefinition(SimVars.Aircraft.FlightModel.DYNAMIC_PRESSURE)]
    //public double dynamicPressure;



    #endregion Fields
  }
}
