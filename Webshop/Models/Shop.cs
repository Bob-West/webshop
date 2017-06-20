using System;
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
            public String name;

            public CartItem()
            {
                ProductID = 0;
                Amount = 0;
                Price = 0;
                name = "";
            }

            public CartItem(int vProductID, float vAmount, decimal vPrice, String name)
            {
                this.ProductID = vProductID;
                this.Amount = vAmount;
                this.Price = vPrice;
                this.name = name;
            }
        }

        public class User
        {
            public int UserID;
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

        public static User loginUser(String email, String passwd)
        {
            SqlCommand vSQLcommand;
            SqlDataReader vSQLreader;
            try
            {
                using (SqlConnection objSQLconn = new SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["shop"].ConnectionString))
                {
                    objSQLconn.Open();
                    vSQLcommand = new SqlCommand("SELECT * FROM tblUsers WHERE user_email=@email AND user_password =@passwd;", objSQLconn);
                    vSQLcommand.Parameters.AddWithValue("@email", email);
                    vSQLcommand.Parameters.AddWithValue("@passwd", Encrypt.Pwd_Encode(passwd));

                    vSQLreader = vSQLcommand.ExecuteReader();
                    vSQLcommand.Dispose();

                    if (vSQLreader.HasRows)
                    {
                        vSQLreader.Read();
                        User loggedInUser = new Shop.User();
                        loggedInUser.UserID = (int)vSQLreader["user_id"];
                        loggedInUser.firstname = (String)vSQLreader["user_firstname"];
                        loggedInUser.lastname = (String)vSQLreader["user_lastname"];
                        loggedInUser.email = (String)vSQLreader["user_email"];
                        loggedInUser.phone = (String)vSQLreader["user_tel"];
                        loggedInUser.bill_street = (String)vSQLreader["user_bill_street"];
                        loggedInUser.bill_city = (String)vSQLreader["user_bill_city"];
                        loggedInUser.bill_country = (String)vSQLreader["user_bill_country"];
                        loggedInUser.bill_zipcode = (String)vSQLreader["user_bill_zipcode"];

                        return loggedInUser;
                    }
                    else
                    {
                        Debug.Print("falsches Login " + email + " " + passwd);
                        return null;
                    }

                }
            }
            catch (Exception vError)
            {
                Debug.Print("DB geht nicht" + vError);
                return null;
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
                    String insertCommand = "INSERT INTO tblUsers (user_salutation,user_title,user_firstname,user_lastname,user_email,user_tel,user_password,user_bill_street,user_bill_zipcode,user_bill_city,user_bill_country,user_delivery_street,user_delivery_zipcode,user_delivery_city,user_delivery_country) " +
                        "VALUES (@user_salutation, @user_title, @user_firstname, @user_lastname,@user_email, @user_tel, @user_password, @user_bill_street, @user_bill_zipcode, @user_bill_city, @user_bill_country, @user_delivery_street, @user_delivery_zipcode, @user_delivery_city, @user_delivery_country) SELECT CAST(scope_identity() AS int)";
                    vSQLcommand = new SqlCommand(insertCommand, objSQLconn);
                    vSQLcommand.Parameters.AddWithValue("@user_salutation", newUser.salutation);
                    vSQLcommand.Parameters.AddWithValue("@user_title", newUser.title);
                    vSQLcommand.Parameters.AddWithValue("@user_firstname", newUser.firstname);
                    vSQLcommand.Parameters.AddWithValue("@user_lastname", newUser.lastname);
                    vSQLcommand.Parameters.AddWithValue("@user_email", newUser.email);
                    vSQLcommand.Parameters.AddWithValue("@user_tel", newUser.phone);
                    vSQLcommand.Parameters.AddWithValue("@user_password", Encrypt.Pwd_Encode(newUser.passwd));
                    vSQLcommand.Parameters.AddWithValue("@user_bill_street", newUser.bill_street);
                    vSQLcommand.Parameters.AddWithValue("@user_bill_zipcode", newUser.bill_zipcode);
                    vSQLcommand.Parameters.AddWithValue("@user_bill_city", newUser.bill_city);
                    vSQLcommand.Parameters.AddWithValue("@user_bill_country", newUser.bill_country);
                    vSQLcommand.Parameters.AddWithValue("@user_delivery_street", newUser.delivery_street);
                    vSQLcommand.Parameters.AddWithValue("@user_delivery_zipcode", newUser.delivery_zipcode);
                    vSQLcommand.Parameters.AddWithValue("@user_delivery_city", newUser.delivery_city);
                    vSQLcommand.Parameters.AddWithValue("@user_delivery_country", newUser.delivery_country);


                    int insertSuccessfull = vSQLcommand.ExecuteNonQuery();

                    if (insertSuccessfull > 0)
                    {
                        var newUserRegisteredID = (int)vSQLcommand.ExecuteScalar();
                        newUser.UserID = newUserRegisteredID;
                        return newUser;
                    }
                    else
                    {
                        Debug.Print("kein User insertSuccessfull falsch " + insertSuccessfull);
                        return null;
                    }
                }
            }
            catch (Exception vError)
            {
                Debug.Print("DB geht nnicht" + vError);
                return null;
            }
        }
        public static bool checkEmail(string email)
        {
            SqlCommand vSQLcommand;
            SqlDataReader vSQLreader;

            try
            {
                using (SqlConnection objSQLconn = new SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["shop"].ConnectionString))
                {
                    objSQLconn.Open();
                    //read
                    vSQLcommand = new SqlCommand("SELECT * FROM tblUsers WHERE user_email=@user_email;", objSQLconn);
                    vSQLcommand.Parameters.AddWithValue("@user_email", email);
                    vSQLreader = vSQLcommand.ExecuteReader();
                    vSQLcommand.Dispose();

                    vSQLreader.Close();
                    return true;
                }
            }
            catch (Exception vError)
            {
                return false;
            }
        }

        public static bool AddtoCartItem(int vProductID, float vAmount)
        {
            SqlCommand vSQLcommand;
            SqlDataReader vSQLreader;
            decimal vProductPrice;
            String vName;
            try
            {
                using (SqlConnection objSQLconn = new SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["shop"].ConnectionString))
                {
                    objSQLconn.Open();
                    //read
                    vSQLcommand = new SqlCommand("SELECT product_id, product_price, product_name FROM tblProducts WHERE product_id=@product_id;", objSQLconn);
                    vSQLcommand.Parameters.AddWithValue("@product_id", vProductID);
                    vSQLreader = vSQLcommand.ExecuteReader();
                    vSQLcommand.Dispose();

                    if (vSQLreader.HasRows)
                    {
                        // read Methode schaltet eins weiter
                        vSQLreader.Read();
                        vProductPrice = (decimal)vSQLreader["product_price"];
                        vName = (String)vSQLreader["product_name"];
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
                    vCart.Items.Add(new CartItem(vProductID, vAmount, vProductPrice, vName));
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
        public class Order
        {
            public int UserID;
            public int OrderID;
            public string timestamp;

            public Order()
            {
                UserID = 0;
                OrderID = 0;
                timestamp = "";
            }
            public Order(int UserID, int OrderID)
            {
                this.UserID = UserID;
                this.OrderID = OrderID;
                this.timestamp = timestamp;
            }
            public static bool save_Order()
            {
                SqlCommand vSQLcommand1;
                SqlCommand vSQLcommand2;
                int userID;

                try
                {

                    using (SqlConnection objSQLconn = new SqlConnection(System.Web.Configuration.WebConfigurationManager.ConnectionStrings["shop"].ConnectionString))
                    {
                        objSQLconn.Open();
                        String insertCommand_order = "INSERT INTO tblOrder (user_id) VALUES (@order_userID); SELECT CAST(scope_identity() AS int)";
                        String insertCommand_order_prd = "INSERT INTO tblOrder_Product (order_id,product_id,product_amount) VALUES (@order_ID, @product_id, @order_product_amount);";

                        vSQLcommand1 = new SqlCommand(insertCommand_order, objSQLconn);

                        Shop.User derUser = HttpContext.Current.Session["User"] as Shop.User;
                        userID = derUser.UserID;
                        vSQLcommand1.Parameters.AddWithValue("@order_userID", userID);

                        Shop.Cart derCart = HttpContext.Current.Session["Cart"] as Shop.Cart;

                        var order_id = (int)vSQLcommand1.ExecuteScalar();
                        //int insertSuccessfull1 = vSQLcommand1.ExecuteNonQuery();
                        int i = 0;
                        foreach (var items in derCart.Items)
                        {
                            vSQLcommand2 = new SqlCommand(insertCommand_order_prd, objSQLconn);
                            vSQLcommand2.Parameters.AddWithValue("@order_ID", order_id);
                            vSQLcommand2.Parameters.AddWithValue("@product_ID", items.ProductID);
                            vSQLcommand2.Parameters.AddWithValue("@order_product_amount", 45);
                            int insertSuccessfull2 = vSQLcommand2.ExecuteNonQuery();
                            if (insertSuccessfull2 > 0)
                            {
                                Debug.Print("Successful Order_Product insert Nr. " + (++i));
                            }
                            else
                            {
                                Debug.Print("insert product fehlgeschlagen nummer " + (++i));
                                return false;
                            }
                        }
                        if (order_id > 0)
                        {
                            HttpContext.Current.Session["Order"] = new Order(userID, order_id);
                            return true;

                        }
                        else
                        {
                            Debug.Print("insert order fehlgeschlagen");
                            return false;
                        }
                        


                    }
                }
                catch (Exception vError)
                {
                    Debug.Print("DB geht nicht - save order fkt" + vError);
                    return false;
                }
            }
        }
    }
}
