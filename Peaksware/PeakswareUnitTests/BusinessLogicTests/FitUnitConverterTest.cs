using System;
using NUnit.Framework;
using PeakswareTest.Business_Logic;
using PeakswareTest.Models;

namespace Tests
{
    [TestFixture]
    public class FitUnitConverterTest
    {
        private const double SPEED_MPS_TO_MPH = 2.24;
        private const double DISTANCE_METERS_TO_MILES = .0006214;
        private const double DISTANCE_METERS_TO_FEET = 3.281;
        private const double DOUBLE_COMPARISON_DELTA = 0.000001;
        private Workout workout;

        [SetUp]
        public void Setup()
        {
            GenerateWorkout();
        }

        [Test]
        public void TestEmptyWorkoutDoesNotException()
        {
            Assert.DoesNotThrow(() => FitUnitConverter.ConvertWorkoutToImperial(workout));
        }

        [Test]
        public void TestSessionSpeedIsConverted()
        {
            workout.Session.SessionMetrics.Add("Speed", 1 / SPEED_MPS_TO_MPH);
            FitUnitConverter.ConvertWorkoutToImperial(workout);
            double result = (double)Convert.ToDecimal(workout.Session.SessionMetrics["Speed"]);
            Assert.AreEqual((double)1, result, DOUBLE_COMPARISON_DELTA);
        }

        [Test]
        public void TestSessionSpeedRelatedMetricIsConverted()
        {
            workout.Session.SessionMetrics.Add("OtherSpeedMetric", 1 / SPEED_MPS_TO_MPH);
            FitUnitConverter.ConvertWorkoutToImperial(workout);
            double result = (double)Convert.ToDecimal(workout.Session.SessionMetrics["OtherSpeedMetric"]);
            Assert.AreEqual(1, result, DOUBLE_COMPARISON_DELTA);
        }

        [Test]
        public void TestSessionDistanceIsConverted()
        {
            workout.Session.SessionMetrics.Add("Distance", 1 / DISTANCE_METERS_TO_MILES);
            FitUnitConverter.ConvertWorkoutToImperial(workout);
            double result = (double)Convert.ToDecimal(workout.Session.SessionMetrics["Distance"]);
            Assert.AreEqual(1, result, DOUBLE_COMPARISON_DELTA);
        }

        [Test]
        public void TestSessionDistanceRelatedMetricIsConverted()
        {
            workout.Session.SessionMetrics.Add("OtherDistanceMetric", 1 / DISTANCE_METERS_TO_MILES);
            FitUnitConverter.ConvertWorkoutToImperial(workout);
            double result = (double)Convert.ToDecimal(workout.Session.SessionMetrics["OtherDistanceMetric"]);
            Assert.AreEqual(1, result, DOUBLE_COMPARISON_DELTA);
        }

        [Test]
        public void TestSessionAltitudeIsConverted()
        {
            workout.Session.SessionMetrics.Add("Altitude", 1 / DISTANCE_METERS_TO_FEET);
            FitUnitConverter.ConvertWorkoutToImperial(workout);
            double result = (double)Convert.ToDecimal(workout.Session.SessionMetrics["Altitude"]);
            Assert.AreEqual(1, result, DOUBLE_COMPARISON_DELTA);
        }

        [Test]
        public void TestSessionAltitudeRelatedMetricIsConverted()
        {
            workout.Session.SessionMetrics.Add("OtherAltitudeMetric", 1 / DISTANCE_METERS_TO_FEET);
            FitUnitConverter.ConvertWorkoutToImperial(workout);
            double result = (double)Convert.ToDecimal(workout.Session.SessionMetrics["OtherAltitudeMetric"]);
            Assert.AreEqual(1, result, DOUBLE_COMPARISON_DELTA);
        }

        [Test]
        public void TestLapSpeedIsConverted()
        {
            Lap newLap = new Lap();
            newLap.LapMetrics.Add("Speed", 1 / SPEED_MPS_TO_MPH);
            workout.Laps.Add(newLap);
            FitUnitConverter.ConvertWorkoutToImperial(workout);
            double result = (double)Convert.ToDecimal(workout.Laps[0].LapMetrics["Speed"]);
            Assert.AreEqual((double)1, result, DOUBLE_COMPARISON_DELTA);
        }

        [Test]
        public void TestLapSpeedRelatedMetricIsConverted()
        {
            Lap newLap = new Lap();
            newLap.LapMetrics.Add("OtherSpeedMetric", 1 / SPEED_MPS_TO_MPH);
            workout.Laps.Add(newLap);
            FitUnitConverter.ConvertWorkoutToImperial(workout);
            double result = (double)Convert.ToDecimal(workout.Laps[0].LapMetrics["OtherSpeedMetric"]);
            Assert.AreEqual(1, result, DOUBLE_COMPARISON_DELTA);
        }

        [Test]
        public void TestLapDistanceIsConverted()
        {
            Lap newLap = new Lap();
            newLap.LapMetrics.Add("Distance", 1 / DISTANCE_METERS_TO_MILES);
            workout.Laps.Add(newLap);
            FitUnitConverter.ConvertWorkoutToImperial(workout);
            double result = (double)Convert.ToDecimal(workout.Laps[0].LapMetrics["Distance"]);
            Assert.AreEqual(1, result, DOUBLE_COMPARISON_DELTA);
        }

        [Test]
        public void TestLapDistanceRelatedMetricIsConverted()
        {
            Lap newLap = new Lap();
            newLap.LapMetrics.Add("OtherDistanceMetric", 1 / DISTANCE_METERS_TO_MILES);
            workout.Laps.Add(newLap);
            FitUnitConverter.ConvertWorkoutToImperial(workout);
            double result = (double)Convert.ToDecimal(workout.Laps[0].LapMetrics["OtherDistanceMetric"]);
            Assert.AreEqual(1, result, DOUBLE_COMPARISON_DELTA);
        }

        [Test]
        public void TestLapAltitudeIsConverted()
        {
            Lap newLap = new Lap();
            newLap.LapMetrics.Add("Altitude", 1 / DISTANCE_METERS_TO_FEET);
            workout.Laps.Add(newLap);
            FitUnitConverter.ConvertWorkoutToImperial(workout);
            double result = (double)Convert.ToDecimal(workout.Laps[0].LapMetrics["Altitude"]);
            Assert.AreEqual(1, result, DOUBLE_COMPARISON_DELTA);
        }

        [Test]
        public void TestLapAltitudeRelatedMetricIsConverted()
        {
            Lap newLap = new Lap();
            newLap.LapMetrics.Add("OtherAltitudeMetric", 1 / DISTANCE_METERS_TO_FEET);
            workout.Laps.Add(newLap);
            FitUnitConverter.ConvertWorkoutToImperial(workout);
            double result = (double)Convert.ToDecimal(workout.Laps[0].LapMetrics["OtherAltitudeMetric"]);
            Assert.AreEqual(1, result, DOUBLE_COMPARISON_DELTA);
        }

        [Test]
        public void TestRecordSpeedIsConverted()
        {
            Record newRecord = new Record();
            newRecord.RecordMetrics.Add("Speed", 1 / SPEED_MPS_TO_MPH);
            workout.Records.Add(newRecord);
            FitUnitConverter.ConvertWorkoutToImperial(workout);
            double result = (double)Convert.ToDecimal(workout.Records[0].RecordMetrics["Speed"]);
            Assert.AreEqual((double)1, result, DOUBLE_COMPARISON_DELTA);
        }

        [Test]
        public void TestRecordSpeedRelatedMetricIsConverted()
        {
            Record newRecord = new Record();
            newRecord.RecordMetrics.Add("OtherSpeedMetric", 1 / SPEED_MPS_TO_MPH);
            workout.Records.Add(newRecord);
            FitUnitConverter.ConvertWorkoutToImperial(workout);
            double result = (double)Convert.ToDecimal(workout.Records[0].RecordMetrics["OtherSpeedMetric"]);
            Assert.AreEqual(1, result, DOUBLE_COMPARISON_DELTA);
        }

        [Test]
        public void TestRecordDistanceIsConverted()
        {
            Record newRecord = new Record();
            newRecord.RecordMetrics.Add("Distance", 1 / DISTANCE_METERS_TO_MILES);
            workout.Records.Add(newRecord);
            FitUnitConverter.ConvertWorkoutToImperial(workout);
            double result = (double)Convert.ToDecimal(workout.Records[0].RecordMetrics["Distance"]);
            Assert.AreEqual(1, result, DOUBLE_COMPARISON_DELTA);
        }

        [Test]
        public void TestRecordDistanceRelatedMetricIsConverted()
        {
            Record newRecord = new Record();
            newRecord.RecordMetrics.Add("OtherDistanceMetric", 1 / DISTANCE_METERS_TO_MILES);
            workout.Records.Add(newRecord);
            FitUnitConverter.ConvertWorkoutToImperial(workout);
            double result = (double)Convert.ToDecimal(workout.Records[0].RecordMetrics["OtherDistanceMetric"]);
            Assert.AreEqual(1, result, DOUBLE_COMPARISON_DELTA);
        }

        [Test]
        public void TestRecordAltitudeIsConverted()
        {
            Record newRecord = new Record();
            newRecord.RecordMetrics.Add("Altitude", 1 / DISTANCE_METERS_TO_FEET);
            workout.Records.Add(newRecord);
            FitUnitConverter.ConvertWorkoutToImperial(workout);
            double result = (double)Convert.ToDecimal(workout.Records[0].RecordMetrics["Altitude"]);
            Assert.AreEqual(1, result, DOUBLE_COMPARISON_DELTA);
        }

        [Test]
        public void TestRecordAltitudeRelatedMetricIsConverted()
        {
            Record newRecord = new Record();
            newRecord.RecordMetrics.Add("OtherAltitudeMetric", 1 / DISTANCE_METERS_TO_FEET);
            workout.Records.Add(newRecord);
            FitUnitConverter.ConvertWorkoutToImperial(workout);
            double result = (double)Convert.ToDecimal(workout.Records[0].RecordMetrics["OtherAltitudeMetric"]);
            Assert.AreEqual(1, result, DOUBLE_COMPARISON_DELTA);
        }

        private void GenerateWorkout()
        {
            workout = new Workout
            {
                Session = new Session()
            };

        }
    }
}
