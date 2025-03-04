﻿using Mapbox.CheapRulerCs;
using Mapbox.Unity.Location;

namespace LocationRPG
{
    public class DistanceController
    {
        private CheapRuler _cheapRuler;
        private Location _oldLocation;

        //total session's distance covered in metres [m]
        private double _distanceCovered;

        //distance that device covered since last update
        private double _distance;


        public double Distance => _distance;

        public double DistanceCovered => _distanceCovered;

        public DistanceController(Location oldLocation)
        {
            _cheapRuler = new CheapRuler(oldLocation.LatitudeLongitude.x
                , CheapRulerUnits.Meters);
            _oldLocation = oldLocation;
            Load();
            // _distanceCovered = distanceCovered;
        }

        //count distance 
        public double DistanceUpdate(Location newLocation)
        {
            double[] oldLatlong = {_oldLocation.LatitudeLongitude.x, _oldLocation.LatitudeLongitude.y};
            double[] newLatlong = {newLocation.LatitudeLongitude.x, newLocation.LatitudeLongitude.y};
            double distance = _cheapRuler.Distance(oldLatlong, newLatlong);
            _distance = distance;
            return distance;
        }

        //remembers newLocation as location that player is at at the moment
        //Also adds covered distance to distance counter
        public void AddNewDistance(Location newLocation, double distance)
        {
            _distanceCovered += distance;
            _oldLocation = newLocation;
        }

        public void Save()
        {
            SaveSystem.SaveDistance(this);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void Load()
        {
            DistanceData distanceData = SaveSystem.LoadDistance();
            if (distanceData is null || distanceData.DistanceCovered == 0)
            {
                _distanceCovered = 0;
            }
            else
            {
                _distanceCovered = distanceData.DistanceCovered;
            }
        }
    }
}