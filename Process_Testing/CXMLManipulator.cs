/****************************************
 * Project: ServiceProvider  
 * Class:   CXMLManipulator
 * Author:  ha
 * Version: 1.0 
 * Created: 01/24/2007 17:00:24
 * 
 * Copyright Year: 2007  
 ****************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;


namespace Utility.XML
{
    /// <summary>
    /// Description of CXMLManipulator
    /// </summary>
    public class CXMLManipulator
    {
        #region Members
        #endregion
        #region Properties
        #endregion
        #region Constructors


        /// <summary>
        /// Default Constructor
        /// </summary>
        public CXMLManipulator()
        {

        }
        #endregion

        #region Methods


        /// <summary>
        /// Class Methods
        /// </summary>
        /// 

        public object DeserializeCollectionFromFile(Type tObjectType, string sFileName, string sRootElementName)
        {

            object oRetval = null;

            try
            {

                if (File.Exists(sFileName))
                {

                    XmlRootAttribute ra = new XmlRootAttribute();

                    ra.Namespace = string.Empty;

                    ra.ElementName = sRootElementName;

                    System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(tObjectType, ra);

                    XmlReaderSettings oReaderSettings = new XmlReaderSettings();

                    XmlReader oXmlReader = XmlReader.Create(sFileName, oReaderSettings);

                    if (xmlSerializer.CanDeserialize(oXmlReader))
                    {

                        oRetval = xmlSerializer.Deserialize(oXmlReader);

                        oXmlReader.Close();

                        oXmlReader = null;

                        oReaderSettings = null;

                    }

                    else
                    {

                        throw (new Exception(string.Format("File '{0}' not found.", sFileName)));

                    }

                }

            }

            catch (Exception exp)
            {

                throw (new Exception(string.Format("Error Loading object from file: {0}", exp.Message), exp));

            }

            return oRetval;           

        }
    }


        #endregion

}//CXMLManipulator

