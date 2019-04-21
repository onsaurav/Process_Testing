using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Data.SqlClient;
using DBConnection;
using Microsoft.VisualBasic;
using DBExecution;
using System.Web;
using System.Web.Mail;


namespace Process_Testing
{
    public partial class Form1 : Form
    {
        
        private CConnection m_oCConnectionToDB = new CConnection();
        SqlConnection m_oSqlConnection = new SqlConnection();
        private Encoding fileEncoding = Encoding.UTF8;
        private CExecutionDB m_oCSQLCommandExecutor = new CExecutionDB();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void SearchExeFile(string c)
        {
            //OMSProcessMail ap = new OMSProcessMail();
            DirectoryInfo dir = new DirectoryInfo(c);
            SqlConnection oSqlConnection = new SqlConnection();
            CConnection m_oCConnectionToDB = new CConnection();
            foreach (FileInfo f in dir.GetFiles())
            {
                String Sender = "";
                String pathname = "C:\\OMS\\Others\\Sender.dbz";
                FileStream fsIn = new FileStream(pathname, FileMode.Open, FileAccess.Read, FileShare.Read);
                using (StreamReader sr = new StreamReader(fsIn, fileEncoding, true))
                {
                    Sender = sr.ReadLine();
                }

                String Mail = "";
                pathname = "C:\\OMS\\Others\\Mail.dbz";
                fsIn = new FileStream(pathname, FileMode.Open, FileAccess.Read, FileShare.Read);
                using (StreamReader sr = new StreamReader(fsIn, fileEncoding, true))
                {
                    Mail = sr.ReadLine();
                }
                oSqlConnection = m_oCConnectionToDB.GetDBConnection();
                DataSet oDataSet = new DataSet();

                //Modified   :   Saurav Biswas Kartik /OCT-21-2009 [Start]
                //Summary    :   To mail to all user [user aleart only]
                //oDataSet = m_oCSQLCommandExecutor.DataAdapterQueryRequest("SELECT  *  FROM SecurityUser WHERE (UsrDepartment = 'Administration') and  UsrLevel= 'Administrator' and UsrActive='Y'", oSqlConnection);
                string strUser = "";
                strUser = f.Name;
                strUser = strUser.Substring(0, strUser.IndexOf("_"));
                oDataSet = m_oCSQLCommandExecutor.DataAdapterQueryRequest("SELECT * FROM SecurityUser WHERE UsrActive='Y' And UsrUserName = '" + strUser + "'", oSqlConnection);
                //Modified   :   Saurav Biswas Kartik /OCT-21-2009 [End]

                if (oDataSet.Tables[0].Rows.Count > 0)
                {
                    File.Copy(f.FullName, "C:\\OMS\\Mail_Mail\\" + f.Name.ToString(), true);
                    File.Delete(f.FullName);
                    for (int k = 0; k <= oDataSet.Tables[0].Rows.Count - 1; k++)
                    {
                        if (oDataSet.Tables[0].Rows[k]["UsrEmailAddress"].ToString() != "")
                        {
                            //Kartik Checking Oct-21-2009 [START]
                            StreamWriter sw = File.AppendText("c:\\Test1.txt");
                            sw.WriteLine(oDataSet.Tables[0].Rows[k]["UsrEmailAddress"].ToString());
                            sw.Close();
                            //Kartik Checking Oct-21-2009 [END]

                            pathname = "C:\\OMS\\Mail_Mail\\" + f.Name.ToString();  // @"D:\TransferPurchase\Buy03022009Purchase.txt";

                            string s;
                            MailMessage mail = new MailMessage();
                            mail.To = oDataSet.Tables[0].Rows[k]["UsrEmailAddress"].ToString();
                            mail.From = Sender;
                            mail.Subject = f.Name.ToString();
                            mail.Body = f.Name.ToString();
                            MailAttachment attachment = new MailAttachment(pathname); //create the attachment
                            mail.Attachments.Add(attachment);	//add the attachment
                            SmtpMail.SmtpServer = Mail;  //your real server goes here
                            SmtpMail.Send(mail);

                        }
                        else
                        {
                            File.Copy(f.FullName, "C:\\OMS\\OMS_Mail_Err\\" + f.Name.ToString(), true);
                            File.Delete(f.FullName);
                        }
                    }
                }


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SearchExeFile("C:\\OMS\\Mail\\");
        }
    }
}
