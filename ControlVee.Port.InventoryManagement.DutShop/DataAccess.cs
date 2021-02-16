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
        private readonly string storedProc_GetAllBatches = "GetAllBatches";
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
        internal bool CreateBatchRecordFromDb(string nameOf, int? total)
        {
            bool updated = false;

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
                    if (reader.RecordsAffected > 0)
                        updated = true;
                }
            }

            return updated;
        }

        public List<BatchModel> GetNewBatchesFromDb()
        {
            batches = new List<BatchModel>();

            AssuredConnected();
            using (System.Data.IDbCommand command = connection.CreateCommand())
            {
                string text = storedProc_GetAllBatches;
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
        #endregion

        #region Db Mappings
        public BatchModel MapBatchesFromDb(System.Data.IDataReader reader)
        {
            // TODO.
            batch = new BatchModel();
            batch.ID = (int)reader["ID"];
            batch.NameOf = (string)reader["nameOf"];
            batch.Total = (int)reader["total"];
            batch.Started = (DateTime)reader["started"];

            return batch;
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
