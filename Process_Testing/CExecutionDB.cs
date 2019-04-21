using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Utility;

using DBConnection;
using DBExecution;

namespace DBExecution
{
    class CExecutionDB
    {
        #region Member
        private CExecutionDB oCExecutionDB ;
        private SqlConnection oSqlConnection ;
        private CConnection m_oCConnectionToDB ;
        private DataSet oDataSet;
 
        #endregion
        #region method
       public DataSet DataAdapterQueryRequest(string sSql, SqlConnection oSqlConnection)
       {
           oDataSet = new DataSet();
            
           SqlDataAdapter oSqlDataAdapter = new SqlDataAdapter(sSql, oSqlConnection);
            try
            {
                oSqlDataAdapter.Fill(oDataSet, "Common");
                 

            }
            catch (Exception ex)
            {
                //oCResult.IsSuccess = false;
                //oCResult.Message = ex.ToString();
            }
            return oDataSet;
        }

        public DataSet  DataAdapterQueryRequest(string sSql)
        {
            m_oCConnectionToDB = new CConnection();
            oSqlConnection = new SqlConnection();
            oDataSet = new DataSet();
            oSqlConnection = m_oCConnectionToDB.GetDBConnection();
            SqlDataAdapter oSqlDataAdapter = new SqlDataAdapter(sSql, oSqlConnection);
            try
            {
                oSqlDataAdapter.Fill(oDataSet, "Common");
                //oCResult.IsSuccess = true;
                //oCResult.Message = "Successfull";
                //oCResult.Data = oDataSet;

            }
            catch (Exception ex)
            {
                //oCResult.IsSuccess = false;
                //oCResult.Message = ex.ToString();
            }
            return oDataSet;
        }
        public string GenerateAutoID(string PrefixString, string TableName, string TableFieldName)
        {
            double ICount;
            oDataSet = new DataSet();
            oDataSet = SQLResult("Select " + TableFieldName + " from " + TableName + " where " + TableFieldName + " like '%" + PrefixString + "%'  order by right(" + TableFieldName + ",5)");
            if (oDataSet.Tables[0].Rows.Count > 0)
            {
                ICount = int.Parse(oDataSet.Tables[0].Rows[oDataSet.Tables[0].Rows.Count - 1][0].ToString().Substring(PrefixString.Length, 5)) + 1;
                PrefixString = PrefixString + ICount.ToString("00000");
            }
            else
            {
                PrefixString = PrefixString + "00001";
            }

            return PrefixString;

        }
        public string GenerateAutoID(string PrefixString, string TableName, string TableFieldName, int StringLength)
        {
            double ICount;
            oDataSet = new DataSet();
            oDataSet = SQLResult("Select " + TableFieldName + " from " + TableName + " where " + TableFieldName + " like '%" + PrefixString + "%'  order by right(" + TableFieldName + "," + StringLength + ")");
            if (oDataSet.Tables[0].Rows.Count > 0)
            {
                ICount = int.Parse(oDataSet.Tables[0].Rows[oDataSet.Tables[0].Rows.Count - 1][0].ToString().Substring(PrefixString.Length, StringLength)) + 1;
                PrefixString = PrefixString + ICount.ToString().PadLeft(StringLength, '0');
            }
            else
            {
                ICount = 1;
                PrefixString = PrefixString + ICount.ToString().PadLeft(StringLength, '0');
            }

            return PrefixString;

        }
        public string GenerateAutoID(string PrefixString, string TableName, string TableFieldName, int StringLength, string AdditionalWhere)
        {
            double ICount;
            oDataSet = new DataSet();
            oDataSet = SQLResult("Select " + TableFieldName + " from " + TableName + " where " + TableFieldName + " like '%" + PrefixString + "%' and " + AdditionalWhere + "order by right(" + TableFieldName + "," + StringLength + ")");

            if (oDataSet.Tables[0].Rows.Count > 0)
            {
                ICount = int.Parse(oDataSet.Tables[0].Rows[oDataSet.Tables[0].Rows.Count - 1][0].ToString().Substring(PrefixString.Length, StringLength)) + 1;
                PrefixString = PrefixString + ICount.ToString().PadLeft(StringLength, '0');
            }
            else
            {
                ICount = 1;
                PrefixString = PrefixString + ICount.ToString().PadLeft(StringLength, '0');
            }

            return PrefixString;

        }
        public DataSet SQLResult(string strSQLString)
        {
            oCExecutionDB = new CExecutionDB();
            oDataSet= oCExecutionDB.DataAdapterQueryRequest(strSQLString);
            return oDataSet;
        }
        public string EncripPassword(string strPassword)
        {
            int pass1;
            int ctr = 1;
            int l;
            string passnew = "";
            pass1 = int.Parse(strPassword.Length.ToString()) - 1;
            ctr = 0;
            do
            {
                char k = Convert.ToChar(strPassword.Substring(ctr, 1));
                l = Convert.ToInt16(k) + 17;
                passnew = passnew.ToString() + (char)l;
                ctr = ctr + 1;
            }
            while (ctr <= pass1);
            return passnew;
        }
        public string EncryptText(string strText, string strPwd)
        {

            int i, c;
            int l;
            string strBuff = "";
            try
            {
                if (strPwd.Length != 0)
                {
                    for (i = 0; i <= strText.Length - 1; i++)
                    {
                        char k = Convert.ToChar(strText.Substring(i, 1));
                        c = Convert.ToInt16(k);                       //c = Asc(Mid$(strText, i, 1))
                        k = Convert.ToChar(strPwd.Substring((i+1) % strPwd.Length , 1));
                        c = c + Convert.ToInt16(k);
                        k = Convert.ToChar(c);
                        strBuff = strBuff + k.ToString();
                    }
                }
                else
                {
                    strBuff = strText;
                }

            }
            catch (Exception ex)
            {
            }
            return strBuff;
        }

        public string DecryptText(string strText, string strPwd)
        {

            int i, c;
            int l;
            string strBuff="";

            if (strPwd.Length != 0)
            {
                for ( i = 0; i <= strText.Length-1; i++)
                {
                    char k = Convert.ToChar(strText.Substring(i, 1));
                    c = Convert.ToInt16(k);                       //c = Asc(Mid$(strText, i, 1))
                    k = Convert.ToChar(strPwd.Substring((i + 1) % strPwd.Length, 1));
                    c = c - Convert.ToInt16(k);
                    k = (char)(c);
                    strBuff = strBuff + k.ToString();
                }
            }
            else
            {
                strBuff = strText;
            }
            return strBuff;
        }

        
        #endregion

 
    }
}
