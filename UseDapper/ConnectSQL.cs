using Dapper;
using Microsoft.Data.SqlClient;
using QLTV_DangHaNhuThien.Components.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseDapper;

string connecStr = "Data Source=DESKTOP-UI5VDNS;Initial Catalog=Library_data;Integrated Security=True;Trust Server Certificate=True";
var da = new DataAccess(connecStr);
using (var connection = new SqlConnection(connecStr))
{
    connection.Open();
    var products = connection.Query<Book>("SELECT * FROM Book");

    foreach (var product in products)
    {
        Console.WriteLine(product.Title);
    }
}

