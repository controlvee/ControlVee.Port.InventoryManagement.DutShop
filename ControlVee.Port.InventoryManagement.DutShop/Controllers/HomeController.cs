﻿using ControlVee.Port.InventoryManagement.DutShop.Models;
using ControlVee.Port.InventoryManagement.DutShop.Test;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Newtonsoft.Json;
using System;
using System.Data.SqlClient;

namespace ControlVee.Port.InventoryManagement.DutShop.Test.Controllers
{
    /// <summary>
    ///  TODO: Handles nulls in Models and Db.
    ///  TODO: Add getallinventory with buttons to delete.
    ///  TODO: Save S_Procs and clean up.
    ///  TODO: S_Procs for all.
    ///  TODO: S_Proc for dbo.Batches in progress to delete
    ///  after x-time to simulate completion.
    ///  TODO: 0 (zero) in S_Proc.
    ///  TODO: Organize references to CSS and .JS.
    ///  TODO: ONHandInventory table foreign key.
    ///  TODO: Handle user input create batch for null or bad values.
    ///  TODO: Disable input on index load with no data.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly string cstring = @"Data Source=(localdb)\MSSQLLocalDB;Database=DutShop;Integrated Security=True";
        private DataAccess context;
        List<BatchModel> batches;
        List<TotalSoldModel> totalSold;
        
        public HomeController()
        {

        }

        [HttpGet]
        [Route("getBatches")]
        public IQueryable<BatchModel> GetBatches()
        {
            batches = new List<BatchModel>();
            using (var connection = new SqlConnection())
            {
                connection.ConnectionString = cstring;

                context = new DataAccess(connection);

                batches = context.GetAllBatchesFromDb();
            };

            return batches.AsQueryable();
        }

        [HttpGet]
        [Route("createBatch")]
        public int CreateBatch()
        {
            int success = 0;
            using (var connection = new SqlConnection())
            {
                connection.ConnectionString = cstring;

                context = new DataAccess(connection);

                if (context.CreateBatchRecordFromDb())
                    success = 1;
            };

            return success;
        }

        [HttpGet]
        [Route("simulateSale")]
        public int SimulateSale()
        {
            int success = 0;
            using (var connection = new SqlConnection())
            {
                connection.ConnectionString = cstring;

                context = new DataAccess(connection);

                if (context.SimulateSaleFromDb())
                    success = 1;
            };

            return success;
        }

        [HttpGet]
        [Route("getTotalSold")]
        public IQueryable<TotalSoldModel> GetTotalSold()
        {
            totalSold = new List<TotalSoldModel>();
            using (var connection = new SqlConnection())
            {
                connection.ConnectionString = cstring;

                context = new DataAccess(connection);

                totalSold = context.GetTotalSoldFromDb();

            };

            return totalSold.AsQueryable();
        }
    }
}
