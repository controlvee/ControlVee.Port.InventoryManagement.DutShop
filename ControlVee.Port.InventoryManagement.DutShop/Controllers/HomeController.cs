﻿using ControlVee.Port.InventoryManagement.DutShop.Models;
using ControlVee.Port.InventoryManagement.DutShop.Test;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Newtonsoft.Json;
using System;

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
        private List<BatchModel> batches;
        private readonly string cstring = @"Data Source=(localdb)\mssqllocaldb;Database=DutShop;Integrated Security=True";
        private DataAccess context;
        public MasterModel masterModel = new MasterModel();
        public MasterModel MasterModel

        {
            get
            {
                return masterModel;
            }
            set
            {
                masterModel = value;
            }
        }

        public HomeController()
        {

        }

        public IActionResult Index()
        {


            using (var connection = new System.Data.SqlClient.SqlConnection())
            {
                connection.ConnectionString = cstring;

                context = new DataAccess(connection);

                batches = context.GetNewBatchesFromDb();
                masterModel.BatchModels = batches;


            };

            return View(masterModel);
        }

      
        [HttpPost]
        public IActionResult CreateBatchRecord(string data)
        {
            // TODO: Handle unterminated string exc.
            // TODO: Click twice to update batches?
            var createBatchModel = JsonConvert.DeserializeObject<CreateBatchModel>(data);

            using (var connection = new System.Data.SqlClient.SqlConnection())
            {
                connection.ConnectionString = cstring;

                context = new DataAccess(connection);
                

                if (!context.CreateBatchRecordFromDb(createBatchModel.nameOf, createBatchModel.total))
                {
                    // Return to ajax call.
                    throw new System.Exception("Move from batch to inventory failed.");
                }

              

            };

            return Json(JsonConvert.SerializeObject("{ message: \"200\" }"));
        }

       

        [HttpGet]
        // TODO: Return partial view?
        public IActionResult GetAllBatches()
        {
            batches = new List<BatchModel>();
            using (var connection = new System.Data.SqlClient.SqlConnection())
            {
                connection.ConnectionString = cstring;

                context = new DataAccess(connection);
             
                batches = context.GetNewBatchesFromDb();

                masterModel.BatchModels = batches;
            };

            return Json(JsonConvert.SerializeObject(batches));
        }      
    }
}
