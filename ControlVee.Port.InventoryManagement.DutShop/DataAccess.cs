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
        private readonly string storedProc_CreateBatchRecord = "CreateBatchRecord";
        private readonly string storedProc_GetallBatches = "GetAllBatches";
        private readonly string storedProc_MoveToInventory = "MoveToInventory";
        private readonly string storedProc_GetAllOnHandInventory = "GetAllOnHandInventory";
        private readonly string storedProc_GetInventoryTotalsByType = "GetOnHandInventoryTotalsByType";
        private readonly string storedProc_DeleteFromInventory = "DeleteFromInventory";
        private List<BatchModel> batches;
        private BatchModel batch;

        public DataAccess()
        {
            batches = new List<BatchModel>();
        }

        public DataAccess(System.Data.IDbConnection connection)
        { 
            this.connection = connection;
        }

        #region DbActions
        internal List<BatchModel> CreateBatchRecordFromDb(string nameOf, int? total)
        {
            batches = new List<BatchModel>();

            AssuredConnected();
            using (System.Data.IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = storedProc_CreateBatchRecord;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                // Add input parameter.
                SqlParameter parameterNameOf = new SqlParameter();
                parameterNameOf.ParameterName = "@nameOf";
                parameterNameOf.SqlDbType = SqlDbType.NVarChar;
                parameterNameOf.Direction = ParameterDirection.Input;
                parameterNameOf.Value = nameOf;
                // Add input parameter.
                SqlParameter parameterTotalMade = new SqlParameter();
                parameterTotalMade.ParameterName = "@totalMade";
                parameterTotalMade.SqlDbType = SqlDbType.Int;
                parameterTotalMade.Direction = ParameterDirection.Input;
                parameterTotalMade.Value = total;

                command.Parameters.Add(parameterNameOf);
                command.Parameters.Add(parameterTotalMade);

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

        public List<BatchModel> GetAllBatchesFromDb()
        {
            batches = new List<BatchModel>();

            AssuredConnected();
            using (System.Data.IDbCommand command = connection.CreateCommand())
            {
                string text = storedProc_GetallBatches;
                command.CommandText = text;
                command.CommandType = System.Data.CommandType.StoredProcedure;

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

        public List<InventoryOnHandModel> GetAllOnHandInventoryFromDb()
        {
            List<InventoryOnHandModel> invOnHand = new List<InventoryOnHandModel>();

            AssuredConnected();
            using (System.Data.IDbCommand command = connection.CreateCommand())
            {
                string text = storedProc_GetAllOnHandInventory;
                command.CommandText = text;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                using (System.Data.IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        invOnHand.Add(MapInventoryOnHandFromDb(reader));
                    }
                }
            }

            return invOnHand;
        }

        public void MoveToInventoryDb(int id)
        {
            AssuredConnected();
            using (System.Data.IDbCommand command = connection.CreateCommand())
            {
                string text = storedProc_MoveToInventory;
                command.CommandText = text;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                // Add input parameter.
                SqlParameter prarameterIdOf = new SqlParameter();
                prarameterIdOf.ParameterName = "@batchId";
                prarameterIdOf.SqlDbType = SqlDbType.Int;
                prarameterIdOf.Direction = ParameterDirection.Input;
                prarameterIdOf.Value = id;

                command.Parameters.Add(prarameterIdOf);

                using (command.ExecuteReader())
                {
                   // If command.numberOfRowsAffected?
                }
            }
        }

        public void DeleteFromInventoryDb(int id)
        {
            AssuredConnected();
            using (System.Data.IDbCommand command = connection.CreateCommand())
            {
                string text = storedProc_DeleteFromInventory;
                command.CommandText = text;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                // Add input parameter.
                SqlParameter prarameterIdOf = new SqlParameter();
                prarameterIdOf.ParameterName = "@batchId";
                prarameterIdOf.SqlDbType = SqlDbType.Int;
                prarameterIdOf.Direction = ParameterDirection.Input;
                prarameterIdOf.Value = id;

                command.Parameters.Add(prarameterIdOf);

                using (command.ExecuteReader())
                {
                    // If command.numberOfRowsAffected?
                }
            }
        }

        public List<InventoryOnHandModelByType> GetInventoryTotalsByTypeFromDb()
        {
            List<InventoryOnHandModelByType> inv = new List<InventoryOnHandModelByType>();

            // TODO.
            AssuredConnected();
            using (System.Data.IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = storedProc_GetInventoryTotalsByType;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                using (System.Data.IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        inv.Add(MapTotalOnHandInvetoryAllByTypeToDb(reader));
                    }
                }
            }

            return inv;
        }
        #endregion

        #region Db Mappings
        public BatchModel MapBatchesFromDb(System.Data.IDataReader reader)
        {
            // TODO.
            batch = new BatchModel();
            batch.ID = (string)reader["ID"];
            batch.NameOf = (string)reader["nameOf"];
            batch.Total = (int)reader["total"];
            batch.Started = (DateTime)reader["started"];

            return batch;
        }

        public InventoryOnHandModel MapInventoryOnHandFromDb(System.Data.IDataReader reader)
        {
            InventoryOnHandModel invOnHand = new InventoryOnHandModel();
            // TODO.
            invOnHand = new InventoryOnHandModel();
            invOnHand.ID = (int)reader["ID"];
            invOnHand.NameOf = (string)reader["nameOf"];
            invOnHand.Total = (int)reader["total"];
            invOnHand.Completion = (DateTime)reader["completion"];
            invOnHand.Expiration = (DateTime)reader["expire"];
            invOnHand.BatchId = (int)reader["batchId"];

            return invOnHand;
        }

        public InventoryOnHandModelByType MapTotalOnHandInvetoryAllByTypeToDb(System.Data.IDataReader reader)
        {
            // TODO.
            InventoryOnHandModelByType inv = new InventoryOnHandModelByType();

            inv = new InventoryOnHandModelByType();
            inv.NameOf = (string)reader["nameOf"];
            inv.Total = (int)reader["total"];

            return inv;
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
