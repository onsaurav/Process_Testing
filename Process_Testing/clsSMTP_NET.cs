using System;
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Windows.Forms;

namespace CS_SMTP_NET
{
	#region "Public Enum-Types"
	public enum TEnumBodyFormat 
	{
		HTML = 0,
		PlainText = 1
	}

	public enum TEnumPriority
	{
		NORMAL = 0,
		HIGH = 1,
		LOW = 2
	}
	#endregion

	public class clsSMTP_NET
	{
		#region "Member Variables"
			private struct TRecipient
			{
				public string strEMail;
				public string strName;
				public bool bBlind;
			}


			private string m_sSender;
			private string m_sUser;
			private string m_sSenderName;
			private string m_sRecipient;
			private string m_sRecipientName;
			private string m_sServer;
			private string m_sSubject;
			private string m_sBody;

			private TEnumBodyFormat m_iBodyFormat;
			private TEnumPriority m_iPriority;

			private SortedList m_colCC;
			private SortedList m_colCC_OK;
			private SortedList m_colAttachments;
		#endregion

		#region "Properties"
		public string User
		{
			get
			{
				return m_sUser;
			}
			set
			{
				m_sUser = value;
			}
		}

		public string Subject
		{
			get
			{
				return m_sSubject;
			}
			set 
			{
				m_sSubject = value;
			}
		}

		public string Body
		{
			get
			{
				return m_sBody;
			}
			set
			{
				m_sBody = value;
			}
		}
		public string Sender
		{
			get
			{
				return m_sSender;
			}
			set
			{
				m_sSender = value;
			}
		}
		public string SenderName
		{
			get
			{
				return m_sSenderName;
			}
			set
			{
				m_sSenderName = value;
			}
		}
		public string Recipient
		{
			get
			{
				return m_sRecipient;
			}
			set
			{
				m_sRecipient = value;
			}
		}
		public string RecipientName
		{
			get
			{
				return m_sRecipientName;
			}
			set
			{
				m_sRecipientName = value;
			}
		}
		public string Server
		{
			get
			{
				return m_sServer;
			}
			set
			{
				m_sServer = value;
			}
		}
		public TEnumBodyFormat BodyFormat
		{
			get
			{
				return m_iBodyFormat;
			}
			set
			{
				m_iBodyFormat = value;
			}
		}
		public TEnumPriority Priority
		{
			get
			{
				return m_iPriority;
			}
			set
			{
				m_iPriority = value;
			}
		}
		#endregion
	
		#region "Private Procedures and Functions"
		private void Init()
		{
			m_sBody = "";
			m_sSubject = "";
			m_sSender = "";
			m_sSenderName = "";
			m_sRecipient = "";
			m_sRecipientName = "";
			m_sServer = "";
			
			m_iBodyFormat = TEnumBodyFormat.HTML;
			m_iPriority = TEnumPriority.NORMAL;

			m_colCC = new SortedList();
			m_colCC_OK = new SortedList();
			m_colAttachments = new SortedList();
		}
		#endregion

		#region "Public Methods"
		public clsSMTP_NET()
		{
			Init();
		}

		public void Dispose()
		{
			if (m_colCC != null) 
			{
				while (m_colCC.Count > 0)
				{
					try
					{
						m_colCC.RemoveAt(0);
					}
					catch {}
				}
			}

			if (m_colCC_OK != null) 
			{
				while (m_colCC_OK.Count > 0)
				{
					try
					{
						m_colCC_OK.RemoveAt(0);
					}
					catch {}
				}
			}

			if (m_colAttachments != null) 
			{
				while (m_colAttachments.Count > 0)
				{
					try
					{
						m_colAttachments.RemoveAt(0);
					}
					catch {}
				}
			}

			m_colCC = null;
			m_colCC_OK = null;
			m_colAttachments = null;
		}

		public void Clear()
		{
			Init();
		}

		public bool Add_cc(string strCC_EMail)
		{
		TRecipient objCC;
		
			try
			{
				objCC = new TRecipient();
				objCC.strEMail = strCC_EMail;
				objCC.strName = "";
				objCC.bBlind = false;

				m_colCC.Add(objCC.strEMail,objCC);

				return true;
			}
			catch
			{
				return false;
			}
		}

		public bool Add_cc(string strCC_EMail, string strCC_Name)
		{
		TRecipient objCC;
		
			try
			{
				objCC = new TRecipient();
				objCC.strEMail = strCC_EMail;
				objCC.strName = strCC_Name;
				objCC.bBlind = false;

				m_colCC.Add(objCC.strEMail,objCC);

				return true;
			}
			catch
			{
				return false;
			}
		}

		public bool Add_Bcc(string strCC_EMail)
		{
		TRecipient objCC;

			try
			{
				objCC = new TRecipient();
				objCC.strEMail = strCC_EMail;
				objCC.strName = "";
				objCC.bBlind = true;

				m_colCC.Add(objCC.strEMail,objCC);

				return true;
			}
			catch
			{
				return false;
			}
		}

		public bool Add_Bcc(string strCC_EMail, string strCC_Name)
		{
		TRecipient objCC;

			try
			{
				objCC = new TRecipient();
				objCC.strEMail = strCC_EMail;
				objCC.strName = strCC_Name;
				objCC.bBlind = true;

				m_colCC.Add(objCC.strEMail,objCC);

				return true;
			}
			catch
			{
				return false;
			}
		}

		public bool AddAttachment(string strFilepath)
		{
		bool bRet;

			try
			{
				bRet = System.IO.File.Exists(strFilepath);
				if (bRet)
					m_colAttachments.Add(strFilepath,strFilepath);
		
				return true;
			}
			catch 
			{
				return false;
			}
		}

		public string Send() 
		{
		string sTmp;
		System.Web.Mail.MailMessage objMailMsg;
		System.Web.Mail.MailAttachment objAttachment;

			try 
			{
				//m_sBody = ""
				//m_sSubject = ""
				//m_sServer = ""
				//m_iPort = -1
				//m_iTimeOut = 30
				objMailMsg = new System.Web.Mail.MailMessage();
				if (m_sSenderName != "")
					objMailMsg.From = string.Concat(m_sSenderName, " <", m_sSender, ">");
				else
					objMailMsg.From = m_sSender;
		
				if (m_sRecipientName != "")
					objMailMsg.To = string.Concat(m_sRecipientName, " <", m_sRecipient, ">");
				else
					objMailMsg.To = m_sRecipient;
		
				foreach(TRecipient objCC in m_colCC.Values)
				{
					if (objCC.bBlind)
					{
						sTmp = objMailMsg.Bcc;
		
						if (sTmp != null)
							sTmp = string.Concat(sTmp, "; ");
		
						if (objCC.strName != "")
							sTmp = string.Concat(sTmp, objCC.strName, " <", objCC.strEMail, ">");
						else
							sTmp = string.Concat(sTmp, objCC.strEMail);
					
						objMailMsg.Bcc = sTmp;
						sTmp = "";
					}
					else
					{
						sTmp = objMailMsg.Cc;
						if (sTmp != null)
							sTmp = string.Concat(sTmp, "; ");
		
						if (objCC.strName != "")
							sTmp = string.Concat(sTmp, objCC.strName, " <", objCC.strEMail, ">");
						else
							sTmp = string.Concat(sTmp, objCC.strEMail);
		
						objMailMsg.Cc = sTmp;
						sTmp = "";
					}
				}

				objMailMsg.Subject = m_sSubject;
				objMailMsg.Body = m_sBody;

				foreach(string sFilename in m_colAttachments.Values)
				{
					objAttachment = new System.Web.Mail.MailAttachment(sFilename);
					objMailMsg.Attachments.Add(objAttachment);
					objAttachment = null;
				}

				System.Web.Mail.SmtpMail.SmtpServer = m_sServer;
				
				switch (m_iBodyFormat)
				{
					case TEnumBodyFormat.HTML:
						objMailMsg.BodyFormat = System.Web.Mail.MailFormat.Html;
						break;
					case TEnumBodyFormat.PlainText:
						objMailMsg.BodyFormat = System.Web.Mail.MailFormat.Text;
						break;
				}

				switch (m_iPriority)
				{
					case TEnumPriority.NORMAL:
						objMailMsg.Priority = System.Web.Mail.MailPriority.Normal;
						break;
					case TEnumPriority.HIGH:
						objMailMsg.Priority = System.Web.Mail.MailPriority.High;
						break;
					case TEnumPriority.LOW:
						objMailMsg.Priority = System.Web.Mail.MailPriority.Low;
						break;
				}

				System.Web.Mail.SmtpMail.Send(objMailMsg);
				return "OK";
			}
			catch (Exception ex)
			{
				return ex.ToString();
			}
			finally
			{
				objMailMsg = null;
			}
		}
		#endregion
	}
}
