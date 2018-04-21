using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shop2Client
{
    class Shop2Client
    {
        public int CustomerID { get; set; }
        public string CName { get; set; }
        public string CAddress { get; set; }
        public int Phone { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Pname { get; set; }
        public int Price { get; set; }

        public override string ToString()
        {
            return "Customer ID" + CustomerID + "Customer Name " + CName + "Customer Address " + CAddress + "Customer Phone Number " + Phone + " Order ID" + OrderID + "Product ID" + ProductID + "Product Name" + Pname + "Product Price" + Price;
        }
    }
}


