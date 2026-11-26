using KruacentExiled.Map.Others.BlackoutNDoor;
using KruacentExiled.Map.Others.BlackoutNDoor.Exceptions;
using KruacentExiled.Map.Others.BlackoutNDoor.Handlers;
using KruacentExiled.Test.Attributes;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace KruacentExiled.Test.Tests
{
    [TestClass]
    public class PatternTests
    {
        private Pattern _pattern;


        // 1Blackout 1Elevator
        [TestMethod]
        public void Ctor_1B1E_NoException()
        {
            List<MapEvent> mapEvents = new List<MapEvent>
            {
                new Blackout(),
                new ElevatorStuck()
            };


            try
            {
                _pattern = new Pattern(mapEvents);
            }
            catch(Exception)
            {
                Assert.IsTrue(false, "should not send an exception");
            }

        }

        // Nothing
        [TestMethod]
        public void Ctor_0_ThrowNoMapEventException()
        {
            List<MapEvent> mapEvents = new List<MapEvent>
            {

            };


            Assert.Catch<NoMapEventException>(() =>
            {
                _pattern = new Pattern(mapEvents);
            }, "should send an exception");
        }

        //Null
        [TestMethod]
        public void Ctor_Null_ThrowNullMapEventsException()
        {
            List<MapEvent> mapEvents = null;


            Assert.Catch<NullMapEventsException>(() =>
            {
                _pattern = new Pattern(mapEvents);
            }, "should send an exception");

        }




        // 1Blackout 1DoorStuck 1ElevatorStuck ; Return Blackout
        [TestMethod]
        public void SeeNext_1B1D1E_ReturnB()
        {
            List<MapEvent> mapEvents = new List<MapEvent>
            {
                new Blackout(),
                new DoorStuck(),
                new ElevatorStuck()
            };
            _pattern = new Pattern(mapEvents);

            MapEvent result = _pattern.SeeNext();


            Assert.IsTrue(result is Blackout, "Should be Blackout");
        }

        // 1Blackout ; Return Blackout
        [TestMethod]
        public void SeeNext_1B_ReturnB()
        {
            List<MapEvent> mapEvents = new List<MapEvent>
            {
                new Blackout(),
            };
            _pattern = new Pattern(mapEvents);

            MapEvent result = _pattern.SeeNext();

            Assert.IsTrue(result is Blackout, "Should be Blackout");
        }


        // 1Blackout 1DoorStuck 1ElevatorStuck ; Return Blackout Return DoorStuck
        [TestMethod]
        public void GetNext_1B1D1E_ReturnBReturnD()
        {
            List<MapEvent> mapEvents = new List<MapEvent>
            {
                new Blackout(),
                new DoorStuck(),
                new ElevatorStuck()
            };
            _pattern = new Pattern(mapEvents);

            MapEvent result = _pattern.GetNext();

            Assert.IsTrue(result is Blackout, "Should be Blackout");

            result = _pattern.GetNext();

            Assert.IsTrue(result is DoorStuck, "Should be DoorStuck");
        }

        // 1Blackout ; Return Blackout Return Blackout
        [TestMethod]
        public void GetNext_1B_ReturnBReturnB()
        {
            List<MapEvent> mapEvents = new List<MapEvent>
            {
                new Blackout(),
            };
            _pattern = new Pattern(mapEvents);

            MapEvent result1 = _pattern.GetNext();

            Assert.IsTrue(result1 is Blackout, "Should be Blackout");

            MapEvent result2 = _pattern.GetNext();

            Assert.IsTrue(result2 is Blackout, "Should be Blackout");
        }


    }
}
