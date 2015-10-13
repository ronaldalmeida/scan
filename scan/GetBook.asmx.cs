using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace scan
{
    /// <summary>
    /// Summary description for GetBook
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class GetBook : System.Web.Services.WebService
    {
        // Method to Store Read status for the book
        [WebMethod]
        public void ReadStatus(string ISBN)
        {
            string isbn_10;
            Int64 isbn_13;
            Book bk = new Book();
            string cs = ConfigurationManager.ConnectionStrings["DatabaseServices"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                // Cheeking ISBN 
                if (ISBN.Length == 10)
                {
                    isbn_10 = ISBN.ToString();
                    isbn_13 = 0;
                }
                else
                {
                    isbn_13 = Int64.Parse(ISBN);
                    isbn_10 = "0";
                }
                string sql = "Update Book set RDStatus='Read' where isbn_10 =@isbn_10 or isbn_13=@isbn_13";
                SqlCommand cmd2 = new SqlCommand(sql, con);
                SqlParameter parameter = new SqlParameter();
                cmd2.Parameters.Add("@isbn_10", SqlDbType.NVarChar, 10).Value = isbn_10;
                cmd2.Parameters.Add("@isbn_13", SqlDbType.BigInt).Value = isbn_13;
                cmd2.CommandType = CommandType.Text;
                cmd2.ExecuteNonQuery();
                JavaScriptSerializer js = new JavaScriptSerializer();
                SqlDataReader rdr = cmd2.ExecuteReader();
                Context.Response.Write(js.Serialize(bk));
            }
        }
        // Method to store Comment on book
        [WebMethod]
        public void Comment(string ISBN, string comm)
        {
            string isbn_10;
            Int64 isbn_13;
            Book bk = new Book();
            string cs = ConfigurationManager.ConnectionStrings["DatabaseServices"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                if (ISBN.Length == 10)
                {
                    isbn_10 = ISBN.ToString();
                    isbn_13 = 0;
                }
                else
                {
                    isbn_13 = Int64.Parse(ISBN);
                    isbn_10 = "0";
                }
                string sql = "Update Book set comment=@comment where isbn_10 =@isbn_10 or isbn_13=@isbn_13";
                SqlCommand cmd2 = new SqlCommand(sql, con);
                SqlParameter parameter = new SqlParameter();
                cmd2.Parameters.Add("@isbn_10", SqlDbType.NVarChar, 10).Value = isbn_10;
                cmd2.Parameters.Add("@isbn_13", SqlDbType.BigInt).Value = isbn_13;
                cmd2.Parameters.Add("@comment", SqlDbType.NVarChar, 200).Value = comm;
                cmd2.CommandType = CommandType.Text;
                cmd2.ExecuteScalar();
                JavaScriptSerializer js = new JavaScriptSerializer();
                SqlDataReader rdr = cmd2.ExecuteReader();
                Context.Response.Write(js.Serialize(bk));
            }
        }
        //Method to get book details from Google Api and store in database
        public Book GetGoogleBooks(string ISBN)
        {
            string apikey = "AIzaSyATMUsgSHjB4bW5p8UAG_H39REokWM-VJs";
            string requestString = "https://www.googleapis.com/books/v1/volumes?q=isbn:" + ISBN + "&key=" + apikey;
            WebRequest objWebRequest = WebRequest.Create(requestString);
            WebResponse objWebResponse = objWebRequest.GetResponse();
            Stream objWebStream = objWebResponse.GetResponseStream();
            using (StreamReader objStreamReader = new StreamReader(objWebStream))
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                Book bk = js.Deserialize<Book>(objStreamReader.ReadToEnd());
                

                if (bk.totalitems == 0)
                {
                    return bk;

                }
                else
                {


                    string cs = ConfigurationManager.ConnectionStrings["DatabaseServices"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(cs))
                    {
                        con.Open();

                           string auth = "N/A";
                            string isbn_10;
                            string isbn_13;
                            if (bk.items[0].volumeInfo.authors != null)
                            {
                                auth = string.Join(", ", bk.items[0].volumeInfo.authors.ToArray());

                            }
                            if (bk.items[0].volumeInfo.industryIdentifiers[0].identifier.Length == 13)
                            {
                                isbn_13 = bk.items[0].volumeInfo.industryIdentifiers[0].identifier;
                                isbn_10 = bk.items[0].volumeInfo.industryIdentifiers[1].identifier;
                            }
                            else
                            {
                                isbn_13 = bk.items[0].volumeInfo.industryIdentifiers[1].identifier;
                                isbn_10 = bk.items[0].volumeInfo.industryIdentifiers[0].identifier;
                            }

                        
                        string sql = "INSERT INTO book(author,title,pagecount,isbn_10,isbn_13) VALUES(@Author,@Title,@pageCount,@isbn_10,@isbn_13)";
                        SqlCommand cmd2 = new SqlCommand(sql, con);
                        cmd2.Parameters.Add("@Author", SqlDbType.NVarChar, 100).Value = auth;
                        cmd2.Parameters.Add("@Title", SqlDbType.NVarChar, 100).Value = bk.items[0].volumeInfo.title;
                        cmd2.Parameters.Add("@pageCount", SqlDbType.Int).Value = bk.items[0].volumeInfo.pageCount;
                        cmd2.Parameters.Add("@isbn_10", SqlDbType.NVarChar, 10).Value = isbn_10;
                        cmd2.Parameters.Add("@isbn_13", SqlDbType.BigInt).Value = Int64.Parse(isbn_13);
                        cmd2.CommandType = CommandType.Text;
                        cmd2.ExecuteNonQuery();
                        return bk;



                    }
                }

            }
        }

        //Method to look up book details from detabase
        [WebMethod]
        public void GetBookDetails(string ISBN)
        {

            string isbn_10;
            Int64 isbn_13;
            Book bk = new Book();
            string cs = ConfigurationManager.ConnectionStrings["DatabaseServices"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                if (ISBN.Length == 10)
                {
                    isbn_10 = ISBN.ToString();
                    isbn_13 = 0;
                }
                else
                {
                    isbn_13 = Int64.Parse(ISBN);
                    isbn_10 = "0";
                }
                string sql = "Select author,title,pagecount,isbn_10,isbn_13,RDStatus,comment from Book where isbn_10 =@isbn_10 or isbn_13=@isbn_13";
                SqlCommand cmd2 = new SqlCommand(sql, con);
                SqlParameter parameter = new SqlParameter();
                cmd2.Parameters.Add("@isbn_10", SqlDbType.NVarChar, 10).Value = isbn_10;
                cmd2.Parameters.Add("@isbn_13", SqlDbType.BigInt).Value = isbn_13;
                cmd2.CommandType = CommandType.Text;
                cmd2.ExecuteScalar();
                JavaScriptSerializer js = new JavaScriptSerializer();
                SqlDataReader rdr = cmd2.ExecuteReader();
                if (rdr.HasRows)
                {
                    bk.source = true;
                    bk.totalitems = 1;
                    while (rdr.Read())
                    {
                        bk.authors1 = rdr["Author"].ToString();
                        bk.title1 = rdr["Title"].ToString();
                        bk.pageCount1 = Convert.ToInt32(rdr["PageCount"]);
                        bk.ReadStatus = rdr["RDStatus"].ToString();
                        bk.comment = rdr["comment"].ToString();
                        Context.Response.Write(js.Serialize(bk));
                    }
                }
                else
                {
                    //IF not found in database call the GetGoogleBooks function.
                    bk = GetGoogleBooks(ISBN);
                    bk.source = false;
                    Context.Response.Write(js.Serialize(bk));
                }

            }
        }

    }
}
