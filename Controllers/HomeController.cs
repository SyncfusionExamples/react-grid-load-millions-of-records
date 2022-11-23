using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;
using Syncfusion.EJ2.Grids;
using Syncfusion.EJ2.GridExport;
using System.Collections;
using System.Diagnostics;

namespace ReactGrid.Controllers
{
    public class HomeController : Controller
    {
        public static List<OrdersDetails> orddata = new List<OrdersDetails>();
        public IActionResult Index()
        {
            ViewBag.dataSource = BigData.GetAllRecords(); 
            return View();
        }        
        public ActionResult ExcelExport([FromForm] string gridModel)
        {
            GridExcelExport exp = new GridExcelExport();
            Grid gridProperty = ConvertGridObject(gridModel);
            return exp.ExcelExport<OrdersDetails>(gridProperty, orddata);
        }
        public ActionResult PdfExport([FromForm] string gridModel)
        {
            GridPdfExport exp = new GridPdfExport();
            Grid gridProperty = ConvertGridObject(gridModel);
            return exp.PdfExport<OrdersDetails>(gridProperty, orddata);
        }

        private Grid ConvertGridObject(string gridProperty)
        {
            Grid GridModel = (Grid)Newtonsoft.Json.JsonConvert.DeserializeObject(gridProperty, typeof(Grid));
            GridColumnModel cols = (GridColumnModel)Newtonsoft.Json.JsonConvert.DeserializeObject(gridProperty, typeof(GridColumnModel));
            GridModel.Columns = cols.columns;
            return GridModel;
        }

        public class GridColumnModel
        {
            public List<GridColumn> columns { get; set; }
        }

        public class TestDm : DataManagerRequest // define the params field
        {
            public ParamObject Params { get; set; }
            public int externalPageSkip { get; set; }
        }
        public class ParamObject   // define the required field as you want
        {
            public int externalPageSkip { get; set; }
        }


        public IActionResult UrlDataSource([FromBody] TestDm dm)
        {

            IQueryable<BigData> DataSource = BigData.GetAllRecords().AsQueryable();
            QueryableOperation operation = new QueryableOperation();
            //IEnumerable<BigData> DataSource = BigData.GetAllRecords();
            //DataOperations operation = new DataOperations();

            if (dm.Search != null && dm.Search.Count > 0)
            {
                DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            }


            if (dm.Where != null && dm.Where.Count > 0) //Filtering
            {
                DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
            }

            if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting
            {
                DataSource = operation.PerformSorting(DataSource, dm.Sorted);
            }

            int count = DataSource.Cast<BigData>().Count();

            if (dm.Skip != 0)
            {
                DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            }
            if (dm.Take != 0)
            {
                DataSource = operation.PerformTake(DataSource, dm.Take);
            }
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }
        public class BigData
        {
            public static List<BigData> order = new List<BigData>();
            public BigData()
            {

            }
            public BigData(int OrderID, DateTime? OrderDate, string EntryTime, int N1, int N2, string CustomerID, string Status, int? Rating, int? Software, string Trustworthiness, string EmailID, int EmployeeID, double Freight, bool Verified, string ShipCity, string ShipName, string ID, string ShipCountry, DateTime? ShippedDate, string ShipAddress)
            {
                this.OrderID = OrderID;
                this.EntryTime = EntryTime;
                this.N1 = N1;
                this.N2 = N2;
                this.CustomerID = CustomerID;
                this.Status = Status;
                this.Rating = Rating;
                this.Software = Software;
                this.Trustworthiness = Trustworthiness;
                this.EmailID = EmailID;
                this.EmployeeID = EmployeeID;
                this.Freight = Freight;
                this.ShipCity = ShipCity;
                this.Verified = Verified;
                this.OrderDate = OrderDate;
                this.ShipName = ShipName;
                this.ID = ID;
                this.ShipCountry = ShipCountry;
                this.ShippedDate = ShippedDate;
                this.ShipAddress = ShipAddress;
            }
            public static List<BigData> GetAllRecords()
            {
                if (order.Count() == 0)
                {
                    int code = 0;
                    for (int i = 1; i < 200001; i++)
                    {
                        order.Add(new BigData(code + 1, new DateTime(2022, 04, 04), "1000", 15, 10, "Alfki", "Active", 1, 80, "Sufficient", "nancy@domain.com", 4, 12.3 * i, false, "#ff00ff", "Simons bistro", "1", "Denmark", new DateTime(2022, 4, 7), "Kirchgasse 6"));
                        order.Add(new BigData(code + 2, new DateTime(2022, 04, 04), "1100", 20, 8, "Anatr", "Inactive", 2, 30, "Insufficient", "anatr@domain.com", 2, 3.3 * i, true, "#ffee00", "Queen Cozinha", "2", "Brazil", new DateTime(2022, 4, 8), "Avda. Azteca 123"));
                        order.Add(new BigData(code + 3, new DateTime(2022, 11, 30), "1515", 22, 15, "Anton", "Active", 3, 98, "Sufficient", "anton@domain.com", 1, 4.3 * i, true, "#110011", "Frankenversand", "3", "Germany", new DateTime(2022, 4, 9), "Carrera 52 con Ave. Bolívar #65-98 Llano Largo"));
                        order.Add(new BigData(code + 4, new DateTime(2022, 10, 22), "0900", 18, 11, "Blomp", "Inactive", 4, 70,"Perfect", "blonp@domain.com", 3, 5.3 * i, false, "#ff5500", "Ernst Handel", "4", "Austria", new DateTime(2022, 4, 10), "Magazinweg 7"));
                        order.Add(new BigData(code + 5, new DateTime(2022, 02, 18), "2030", 26, 13, "Bolid", "Active", 5, 10, "Insufficient", "bolid@domain.com", 4, 6.3 * i, true, "#aa0088", "Hanari Carnes", "5", "Switzerland", new DateTime(2022, 4, 11), "1029 - 12th Ave. S."));
                        order.Add(new BigData(code + 6, new DateTime(2022, 05, 10), "1000", 15, 10, "Hanar", "Active", 2, 70, "Insufficient", "hanar@domain.com", 5, 12.3 * i, true, "#ff00ff", "Simons bistro", "1", "France", new DateTime(2022, 4, 7), "Kirchgasse 6"));
                        order.Add(new BigData(code + 7, new DateTime(2022, 06, 14), "1100", 20, 8, "Thomas", "Inactive", 5, 90, "Perfect", "thomas@domain.com", 3, 3.3 * i, true, "#ffee00", "Queen Cozinha", "2", "Itali", new DateTime(2022, 4, 8), "Avda. Azteca 123"));
                        order.Add(new BigData(code + 8, new DateTime(2022, 06, 05), "1515", 22, 15, "Jack", "Active", 4, 60, "Insufficient", "jack@domain.com", 2, 4.3 * i, false, "#110011", "Frankenversand", "3", "Austria", new DateTime(2022, 4, 9), "Carrera 52 con Ave. Bolívar #65-98 Llano Largo"));
                        order.Add(new BigData(code + 9, new DateTime(2022, 06, 10), "0900", 18, 11, "Alfki", "Inactive", 1, 30, "Sufficient", "alfki@domain.com", 4, 5.3 * i, true, "#ff5500", "Ernst Handel", "4", "USA", new DateTime(2022, 4, 10), "Magazinweg 7"));
                        order.Add(new BigData(code + 10, new DateTime(2022, 05, 30), "2030", 26, 13, "Hanar", "Active", 3, 50, "Perfect", "hanar@domain.com", 1, 6.3 * i, false, "#aa0088", "Hanari Carnes", "5", "Belgium", new DateTime(2022, 4, 11), "1029 - 12th Ave. S."));
                        code += 10;
                    }
                }
                return order;
            }
            public int? OrderID { get; set; }
            public string EntryTime { get; set; }
            public int? N1 { get; set; }
            public int? N2 { get; set; }
            public string CustomerID { get; set; }
            public string Status { get; set; }
            public int? Rating { get; set; }
            public int? Software { get; set; }
            public string Trustworthiness { get; set; }
            public string EmailID { get; set; }
            public int? EmployeeID { get; set; }
            public double? Freight { get; set; }
            public string ShipCity { get; set; }
            public bool Verified { get; set; }
            public DateTime? OrderDate { get; set; }
            public string ShipName { get; set; }
            public string ID { get; set; }
            public string ShipCountry { get; set; }
            public DateTime? ShippedDate { get; set; }
            public string ShipAddress { get; set; }
        }
        public class OrdersDetails
        {
            public OrdersDetails()
            {

            }
            public OrdersDetails(int OrderID, EmployeeData Employee, string CustomerId, int EmployeeId, double Freight, bool Verified, DateTime OrderDate, string ShipCity, string ShipName, string ShipCountry, DateTime ShippedDate, string ShipAddress)
            {
                this.OrderID = OrderID;
                this.Employee = Employee;
                this.CustomerID = CustomerId;
                this.EmployeeID = EmployeeId;
                this.Freight = Freight;
                this.ShipCity = ShipCity;
                this.Verified = Verified;
                this.OrderDate = OrderDate;
                this.ShipName = ShipName;
                this.ShipCountry = ShipCountry;
                this.ShippedDate = ShippedDate;
                this.ShipAddress = ShipAddress;
            }

            public int? OrderID { get; set; }

            public EmployeeData Employee { get; set; }
            public string CustomerID { get; set; }
            public int? EmployeeID { get; set; }
            public double? Freight { get; set; }
            public string ShipCity { get; set; }
            public bool Verified { get; set; }
            public DateTime OrderDate { get; set; }

            public string ShipName { get; set; }

            public string ShipCountry { get; set; }

            public DateTime ShippedDate { get; set; }
            public string ShipAddress { get; set; }
        }

        public class EmployeeData
        {
            public EmployeeData()
            {

            }
            public EmployeeData(string FirstName, string LastName)
            {
                this.FirstName = FirstName;
                this.LastName = LastName;

            }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
    }
    
}
