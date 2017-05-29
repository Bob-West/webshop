﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Webshop.Models
{
    public class Shop
    {
        public class Cart
        {
            public int UserID;
            public List<CartItem> Items;

            public Cart()
            {
                UserID = 0;
                Items = new List<CartItem>();
            }

            public float getCartSum()
            {
                float sum = 0.0f;
                foreach (var cartItem in this.Items)
                {
                    sum += ((float)cartItem.Amount * (float)cartItem.Price);
                }
                return sum;
            }
        }

        public class CartItem
        {
            public int ProductID;
            public float Amount;
            public decimal Price;

            public CartItem()
            {
                ProductID = 0;
                Amount = 0;
                Price = 0;
            }

            public CartItem(int vProductID, float vAmount, decimal vPrice)
            {
                this.ProductID = vProductID;
                this.Amount = vAmount;
                this.Price = vPrice;
            }
        }

        public class User
        {
            public String salutation;
            public String title;
            public String email;
            public String firstname;
            public String lastname;
            internal String passwd;
            public String phone;

            public String delivery_street;
            public String delivery_zipcode;
            public String delivery_city;
            public String delivery_country;
            
            public String bill_street;
            public String bill_zipcode;
            public String bill_city;
            public String bill_country;

            public String vat_id;

            public User()
            {

            }
        }

        public static bool loginUser(string email, string passwd)
        {
            SqlCommand vSQLcommand;
            try
            {
                using (SqlConnection objSQLconn = new SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["shop"].ConnectionString))
                {
                    objSQLconn.Open();
                    vSQLcommand = new SqlCommand("SELECT user_email, user_password FROM tblUsers WHERE user_email='@email' AND user_password ='@passwd';", objSQLconn);
                    vSQLcommand.Parameters.AddWithValue("@email", email);
                    vSQLcommand.Parameters.AddWithValue("@passwd", passwd);

                    int selectSuccessfull = vSQLcommand.ExecuteNonQuery();
                    vSQLcommand.Dispose();

                    if (selectSuccessfull > 0)
                    {
                        return true;
                    }
                    else
                    {
                        Debug.Print("falsches Login " + email +" "+passwd);
                        return false;
                    }
                }
            }
            catch (Exception vError)
            {
                Debug.Print("DB geht nicht" + vError);
                return false;
            }
        }

        //String register_salutation, String register_title, String register_firstname, String register_lastname, String register_password, String register_telephone, String register_bill_street, String register_bill_zipcode, String register_bill_country, String register_bill_city, String register_delivery_street, String register_delivery_zipcode, String register_delivery_country, String register_delivery_city
        public static User registerUser(User newUser)
        {
            SqlCommand vSQLcommand;
            try
            {
                using (SqlConnection objSQLconn = new SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["shop"].ConnectionString))
                {
                    objSQLconn.Open();
                    String insertCommand = "INSERT INTO tblUsers (user_salutation,user_title,user_firstname,user_lastname,user_email,user_tel,user_password,user_bill_street,user_bill_zipcode,user_bill_city,user_bill_country,user_delivery_street,user_delivery_zipcode,user_delivery_city,user_delivery_country)"
                        + "VALUES ('" + newUser.salutation + "','" + newUser.title + "','" + newUser.firstname + "','" + newUser.lastname + "','"
                        + newUser.email + "','" + newUser.phone + "','" + newUser.passwd + "','" + newUser.bill_street + "','" + newUser.bill_zipcode + "','" + newUser.bill_city + "','" + newUser.bill_country + "','"
                        + newUser.delivery_street + "','" + newUser.delivery_zipcode + "','" + newUser.delivery_city + "','" + newUser.delivery_country + "')";
                    vSQLcommand = new SqlCommand(insertCommand, objSQLconn);
                    int insertSuccessfull = vSQLcommand.ExecuteNonQuery();

                    if(insertSuccessfull > 0)
                    {
                        return newUser;
                    }
                    else
                    {
                        Debug.Print("kein User insertSuccessfull falsch "+insertSuccessfull);
                        return null;
                       
                    }
                }
            }
            catch (Exception vError)
            {
                Debug.Print("DB geht nicht"+vError);
                return null;
            }
        }

        public static bool AddtoCartItem(int vProductID, float vAmount)
        {
            SqlCommand vSQLcommand;
            SqlDataReader vSQLreader;
            decimal vProductPrice;
            try
            {
                using (SqlConnection objSQLconn = new SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["shop"].ConnectionString))
                {
                    objSQLconn.Open();
                    //read
                    vSQLcommand = new SqlCommand("SELECT product_id, product_price FROM tblProducts WHERE product_id=@product_id;", objSQLconn);
                    vSQLcommand.Parameters.AddWithValue("@product_id", vProductID);
                    vSQLreader = vSQLcommand.ExecuteReader();
                    vSQLcommand.Dispose();

                    if (vSQLreader.HasRows)
                    {
                        // read Methode schaltet eins weiter
                        vSQLreader.Read();
                        vProductPrice = (decimal)vSQLreader["product_price"];
                    }
                    else
                    {
                        return false;
                    }
                    vSQLreader.Close();
                }

                Cart vCart;
                if (HttpContext.Current.Session["Cart"] == null)
                {
                    vCart = new Cart();
                }
                else
                {
                    vCart = HttpContext.Current.Session["Cart"] as Cart;
                }

                var dict = vCart.Items.ToDictionary(x => x.ProductID);

                CartItem found;
                if (dict.TryGetValue(vProductID, out found))
                {
                    found.Amount += vAmount;

                }
                else
                {
                    vCart.Items.Add(new CartItem(vProductID, vAmount, vProductPrice));
                }

                HttpContext.Current.Session["Cart"] = vCart;

                return true;
            }
            catch (Exception vError)
            {
                return false;
            }
        }

        public static bool deleteItemfromCart(int vProductID)
        {
            Cart vCart;
            vCart = HttpContext.Current.Session["Cart"] as Cart;

            vCart.Items.RemoveAll(x => x.ProductID == vProductID);

            HttpContext.Current.Session["Cart"] = vCart;
            return true;

        }

        public static bool updateItemAmount(int vProductID, float currentAmount)
        {
            Cart vCart;
            vCart = HttpContext.Current.Session["Cart"] as Cart;
            float amount = currentAmount;

            var dict = vCart.Items.ToDictionary(x => x.ProductID);

            CartItem found;
            if (dict.TryGetValue(vProductID, out found))
            {
                found.Amount = amount;
                return true;

            }
            else
            {
                return false;
            }

        }

        public static float summarizePricebyID(int vProductID)
        {
            float ergebnis = 0;
            Cart vCart;
            vCart = HttpContext.Current.Session["Cart"] as Cart;

            var dict = vCart.Items.ToDictionary(x => x.ProductID);
            CartItem found;
            if (dict.TryGetValue(vProductID, out found))
            {
                decimal preis = found.Price;
                float amount = found.Amount;
                ergebnis = (int)preis * amount;

            }

            return ergebnis;
        }
    }
}
