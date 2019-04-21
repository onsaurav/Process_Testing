using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Xml;
using Utility;
using Utility.XML;
namespace DBConnection
{
    class CConnection
    {
        #region Methods

        /// <summary>
        /// Class Methods
        /// </summary>


        /// <summary>
        /// Establish SqlConnection for any insert, update, delete, retrieve operation from Database.
        /// </summary>
        /// <returns>SqlConnection</returns>
        public SqlConnection GetDBConnection()
        {
            SqlConnection oSqlConnection = new SqlConnection(GetConnString());
            try
            {
                if (oSqlConnection.State.ToString() == "Open")
                {
                    oSqlConnection.Close();
                }
                oSqlConnection.Open();
            }
            catch (Exception ex)
            {
                throw new Exception(oSqlConnection.ConnectionString);
            }
            return oSqlConnection;
        }



        /// <summary>
        /// This method read the ConnectionString from a connection related XML file from Base Directory.
        /// </summary>
        /// <returns>string</returns>
        private string GetConnString()
        {
            string sConnectionString = "";
            CXMLManipulator oCXMLManipulator = new CXMLManipulator();
            try
            {
                configuration oconfiguration = (configuration)oCXMLManipulator.DeserializeCollectionFromFile(typeof(configuration), AppDomain.CurrentDomain.BaseDirectory + "ConnectionString.xml", "configuration");
                sConnectionString = oconfiguration.connectionStrings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return sConnectionString;
        }

        #endregion

    }
}
