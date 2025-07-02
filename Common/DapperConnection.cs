using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
//using Npgsql;

namespace CommonCMS.Webs.Common
{
    public class DapperConnection
    {
        public static string connectionString;

        public static void ErrorLogEntry(string ErrorDetails, string ControllerName, string ActionName, string DBOrWebsite, string UserDetails)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("ErrorDetails", ErrorDetails);
            dictionary.Add("ControllerName", ControllerName);
            dictionary.Add("ActionName", ActionName);
            dictionary.Add("DBOrWebsite", DBOrWebsite);
            dictionary.Add("UserDetails", UserDetails);

            ExecuteWithoutResultStatic("InsertErrorMaster", CommandType.StoredProcedure, dictionary);
        }

        public IEnumerable<T> GetListResult<T>(string query, CommandType commandType, Dictionary<string, object> dictionary, IDbTransaction dbTransaction = null)
        {
            try
            {
                using (IDbConnection dbConnection = new MySqlConnection(connectionString))
                {
                    if (dbTransaction != null)
                    {
                        using (dbTransaction)
                        {
                            dbConnection.Open();

                            IEnumerable<T> entities = dbConnection.Query<T>(query, new DynamicParameters(dictionary), null, true, 0, commandType);

                            dbConnection.Close();

                            return entities;
                        }
                    }
                    else
                    {
                        dbConnection.Open();

                        IEnumerable<T> entities = dbConnection.Query<T>(query, new DynamicParameters(dictionary), null, true, 0, commandType);

                        dbConnection.Close();

                        return entities;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into query=>" + query + " \r\n CommandType="+commandType.ToString(), ex.ToString(),"","","", true);
                return null;
            }
        }

        public IEnumerable<T> GetListResult<T>(string query, CommandType commandType, IDbTransaction dbTransaction = null)
        {
            try
            {
                using (IDbConnection dbConnection = new MySqlConnection(connectionString))
                {
                    if (dbTransaction != null)
                    {
                        using (dbTransaction)
                        {

                            dbConnection.Open();

                            IEnumerable<T> entities = dbConnection.Query<T>(query, null, null, true, 0, commandType);

                            dbConnection.Close();

                            return entities;
                        }
                    }
                    else
                    {

                        dbConnection.Open();

                        IEnumerable<T> entities = dbConnection.Query<T>(query, null, null, true, 0, commandType);

                        dbConnection.Close();

                        return entities;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into query=>" + query + " \r\n CommandType=" + commandType.ToString(), ex.ToString(), "", "", "", true);
                return null;
            }
        }

        public static bool ExecuteWithoutResultStatic(string storeProcedurename, CommandType commandType, Dictionary<string, object> dictionary)
        {
            bool result = true;
            try
            {
                using (IDbConnection dbConnection = new MySqlConnection(connectionString))
                {
                    dbConnection.Open();
                    dbConnection.Execute(storeProcedurename, dictionary, null, 0, commandType);
                    dbConnection.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into " + storeProcedurename, ex.ToString(), "", "", "", true);
                return false;
            }
            return result;
        }

        public bool ExecuteWithoutResult(string storeProcedurename, CommandType commandType, Dictionary<string, object> dictionary,IDbTransaction dbTransaction=null)
        {
            bool result = true;
            try
            {
                using (IDbConnection dbConnection = new MySqlConnection(connectionString))
                {
                    if (dbTransaction != null)
                    {
                        using (dbTransaction)
                        {
                            dbConnection.Open();
                            dbConnection.Execute(storeProcedurename, dictionary, null, 0, commandType);
                            dbConnection.Close();
                        }
                    }
                    else
                    {
                        dbConnection.Open();
                        dbConnection.Execute(storeProcedurename, dictionary, null, 0, commandType);
                        dbConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into " + storeProcedurename, ex.ToString(), "", "", "", true);
                return false;
            }
            return result;
        }

        public bool ExecuteWithoutResult(string storeProcedurename, CommandType commandType, IDbTransaction dbTransaction = null)
        {
            bool result = true;
            try
            {
                using (IDbConnection dbConnection = new MySqlConnection(connectionString))
                {

                    if (dbTransaction != null)
                    {
                        using (dbTransaction)
                        {
                            dbConnection.Open();
                            dbConnection.Execute(storeProcedurename, null, null, 0, commandType);
                            dbConnection.Close();
                        }
                    }
                    else
                    {
                        dbConnection.Open();
                        dbConnection.Execute(storeProcedurename, null, null, 0, commandType);
                        dbConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into " + storeProcedurename, ex.ToString(), "", "", "", true);
                return false;
            }
            return result;
        }

    }
}
