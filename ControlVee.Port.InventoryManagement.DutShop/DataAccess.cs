using ControlVee.Port.InventoryManagement.DutShop.Models;
using System.Collections.Generic;
using System;
using System.Data.SqlClient;
using System.Data;

namespace ControlVee.Port.InventoryManagement.DutShop
{
    public class DataAccess
    {
        private System.Data.IDbConnection connection;
        private readonly string storedProc_CreateBatchRecord = "SimulateBatches";
        private readonly string storedProc_SimulateSale = "SimulateSale";
        private readonly string storedProc_GetInventoryTotalsByType = "GetTotalSold";
        private List<BatchModel> batches;
        private List<TotalSoldModel> totalSold;
        private BatchModel batch;
        private TotalSoldModel batchTotalSold;

        public DataAccess()
        {
            
        }

        public DataAccess(System.Data.IDbConnection connection)
        { 
            this.connection = connection;
        }

        #region DbActions
        internal bool CreateBatchRecordFromDb()
        {
            bool success = false;

            AssuredConnected();
            using (System.Data.IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = storedProc_CreateBatchRecord;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                using (System.Data.IDataReader reader = command.ExecuteReader())
                {
                    if (reader.RecordsAffected > 0)
                    {
                        success = true;
                    }
                }
            }

            return success;
        }

        public List<BatchModel> GetAllBatchesFromDb()
        {
            batches = new List<BatchModel>();

            AssuredConnected();
            using (System.Data.IDbCommand command = connection.CreateCommand())
            {
                string text = $"select * from dbo.OnHandInventory";
                command.CommandText = text;
                command.CommandType = CommandType.Text;

                using (System.Data.IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        batches.Add(MapBatchesFromDb(reader));
                    }
                }
            }

            return batches;
        }

        internal bool SimulateSaleFromDb()
        {
            bool success = false;

            AssuredConnected();
            using (System.Data.IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = storedProc_SimulateSale;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                using (System.Data.IDataReader reader = command.ExecuteReader())
                {
                    if (reader.RecordsAffected > 0)
                    {
                        success = true;
                    }
                }
            }

            return success;
        }

        internal List<TotalSoldModel> GetTotalSoldFromDb()
        {
            totalSold = new List<TotalSoldModel>();

            AssuredConnected();
            using (System.Data.IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = storedProc_GetInventoryTotalsByType;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                using (System.Data.IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        totalSold.Add(MapTotalSoldFromDb(reader));
                    }
                }
            }

            return totalSold;
        }

        #endregion

        #region Db Mappings
        public BatchModel MapBatchesFromDb(System.Data.IDataReader reader)
        {
            
            batch = new BatchModel();
            batch.ID = (string)reader["ID"];
            batch.NameOf = (string)reader["nameOf"];
            batch.Total = (int)reader["total"];
            batch.Completion = (DateTime)reader["completion"];
            batch.Expire = (DateTime)reader["expire"];
            batch.TotalSold = (int)reader["totalSold"];
            batch.HasExpired = (int)reader["hasExpired"];

            return batch;
        }

        public TotalSoldModel MapTotalSoldFromDb(System.Data.IDataReader reader)
        {

            batchTotalSold = new TotalSoldModel();
            batchTotalSold.NameOf = (string)reader["nameOf"];
            batchTotalSold.Total = (int)reader["total"];
            batchTotalSold.TotalMoneyAmount = (decimal)reader["totalUSD"];
            return batchTotalSold;
        }
    
        #endregion

        private bool AssuredConnected()
        {
            switch (connection.State)
            {
                case (System.Data.ConnectionState.Closed):
                    connection.Open();
                    return false;

                case (System.Data.ConnectionState.Broken):
                    connection.Close();
                    connection.Open();
                    return false;

                default: return true;
            }
        }
    }
}
