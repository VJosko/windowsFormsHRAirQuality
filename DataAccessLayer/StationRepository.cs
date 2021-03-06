﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Common;
using DataAccessLayer.Entities;

namespace DataAccessLayer
{
    public class StationRepository
    {
        public string connectionString = "Data Source=193.198.57.183; Initial Catalog = DotNet;User ID = vjezbe; Password = vjezbe";

        public List<Station> GetStations()
        {
            var Stations = new List<Station>();
            using (DbConnection connection = new SqlConnection(connectionString))
            using (DbCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM KvalitetaZraka_MjernaMjesta";
                connection.Open();
                using (DbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Stations.Add(new Station()
                        {
                            id = (int)reader["ID"],
                            name = (string)reader["NAZIV"]
                        });
                    }
                }
            }
            return Stations;
        }

        public List<string> GetStationsBase()
        {
            var mjestaId = new List<int>();
            using (DbConnection connection = new SqlConnection(connectionString))
            using (DbCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM [KvalitetaZraka_Mjesta-Polutanti]";
                connection.Open();
                using (DbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        mjestaId.Add((int)reader["GRAD_ID"]);
                    }
                }
            }
            mjestaId = mjestaId.Distinct().ToList();
            List<Station> Stations = GetStations();
            var stationsBase = new List<string>();
            for(int i = 0; i < Stations.Count(); i++)
            {
                for(int j = 0; j < mjestaId.Count(); j++)
                {
                    if(Stations[i].id == mjestaId[j])
                    {
                        stationsBase.Add(Stations[i].name);
                    }
                }
            }
            return stationsBase;
        }
        
        public List<string> GetStationNames()
        {
            var names = new List<string>();
            var stations = new List<Station>();
            stations = GetStations();
            names = stations.Where(x => !string.IsNullOrEmpty(x.name)).Select(x => x.name).ToList();
            return names;
        }
    }
}
