#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//
//Filename: DataOwnerXMLGenerator.cs
#endregion

/// <summary> 
/// This class is used to create DataOwner XML to display details of dataowners.
/// </summary> 
using System;
using System.Data;
using System.Xml;
using System.Xml.XPath;
using System.Reflection;
using Shell.SharePoint.DREAM.Business.Entities;

namespace Shell.SharePoint.DREAM.Controller
{
    /// <summary>
    /// DataOwnerXMLGenerator class to generate the DataOwner XML.
    /// </summary>
    public class DataOwnerXMLGenerator
    {

        #region Declaration
        const string DATAOWNERS = "Users";
        const string DATAOWNER = "User";
        const string NAME = "name";
        const string DISPLAYNAME = "Name";//for display purpose
        const string DETAIL = "Detail";
        const string VALUE = "Value";
        const string XMLVERSION = "1.0";
        const string ENCODING = "UTF-8";
        //delclaring details display names
        const string USERDISPLAYNAME = "displayName";
        const string DATASOURCE = "dataSource";
        const string DEPARTMENT = "Department";
        const string DEPARTMENTCODE = "Department Code";
        const string COMPANY = "Company";
        const string LOCATION = "Location";
        const string PHONE = "Phone";
        const string TELEPHONENO = "Telephone";
        const string MOBILENO = "Mobile";
        const string EXTNNO = "Extension";
        const string OTHERNO = "OtherTelephone";
        const string EMAIL = "E-Mail";
        const string COUNTRY = "Country";
        const string ROLE = "Role";
        const string POSTALCODE = "Postal code";
        private AbstractController objMossController;
        private ServiceProvider objFactory = new ServiceProvider();
        string strCurrSiteUrl = string.Empty;
        string strListName = "Data Owner";
        #endregion

        #region Constructor
        public DataOwnerXMLGenerator(string siteURL)
        {
            strCurrSiteUrl = siteURL;
        }
        #endregion

        #region Public Method

        /// <summary>
        /// Gets the data owner XML.
        /// </summary>
        /// <param name="users">The users.</param>
        /// <returns></returns>
        public XmlDocument GetDataOwnerXML(Users users, string[] dataSourceNames)
        {
            XmlDocument objResultXml = new XmlDocument();
            XmlElement xmlelementDataOwners;
            XmlElement xmlelementDataOwner;
            XmlElement xmlelementPhoneDetail;
            XmlAttribute xmlattributeName;
            XmlAttribute xmlattributeDataSource;
            XmlAttribute xmlattributeDetailName;
            try
            {
                XmlDeclaration xmlDeclaration = objResultXml.CreateXmlDeclaration(XMLVERSION, ENCODING, string.Empty);
                objResultXml.AppendChild(xmlDeclaration);
                xmlelementDataOwners = objResultXml.CreateElement(DATAOWNERS);
                objResultXml.AppendChild(xmlelementDataOwners);
                for (int intCount = 0; intCount < users.Count; intCount++)
                {
                    xmlelementDataOwner = objResultXml.CreateElement(DATAOWNER);
                    xmlattributeName = objResultXml.CreateAttribute(USERDISPLAYNAME);
                    xmlattributeName.Value = users[intCount].Name;
                    xmlelementDataOwner.Attributes.Append(xmlattributeName);
                    //Adding datasource attribute
                    xmlattributeDataSource = objResultXml.CreateAttribute(DATASOURCE);
                    // xmlattributeDataSource.Value = GetDataSourceName(users[intCount].UserID);
                    xmlattributeDataSource.Value = dataSourceNames[intCount];
                    xmlelementDataOwner.Attributes.Append(xmlattributeDataSource);
                    //adding phone detail element
                    xmlelementPhoneDetail = objResultXml.CreateElement(DETAIL);
                    xmlattributeDetailName = objResultXml.CreateAttribute(NAME);
                    xmlattributeDetailName.Value = GetDisplayName("phone");
                    xmlelementPhoneDetail.Attributes.Append(xmlattributeDetailName);

                    AddDetail(users[intCount], ref objResultXml, ref  xmlelementDataOwner, ref xmlelementPhoneDetail);
                    xmlelementDataOwners.AppendChild(xmlelementDataOwner);

                }
            }
            catch
            {
                throw;
            }
            return objResultXml;
        }
        #endregion

        #region Private Mehtod

        /// <summary>
        /// Adds the detail.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="dataOWnerXML">The data O wner XML.</param>
        /// <param name="dataOwner">The data owner.</param>
        /// <param name="phoneElement">The phone element.</param>
        private void AddDetail(User user, ref XmlDocument dataOWnerXML, ref XmlElement dataOwner, ref XmlElement phoneElement)
        {
            XmlElement xmlelementDetail;
            XmlElement xmlelementDetailValue;
            XmlAttribute xmlattributeDetailName;
            XmlAttribute xmlattributeDetailValueName;
            bool blnAddPhoneNode = true;

            try
            {
                Type type = user.GetType();
                PropertyInfo[] propertyinfoCol = type.GetProperties();
                foreach (PropertyInfo propertyInfo in propertyinfoCol)
                {
                    if ((!propertyInfo.Name.ToLowerInvariant().Equals("userid")) && (!propertyInfo.Name.ToLowerInvariant().Equals("name")))
                    {
                        if ((propertyInfo.Name.ToLowerInvariant().Equals("telephonenumber")) || (propertyInfo.Name.ToLowerInvariant().Equals("mobilenumber")) || (propertyInfo.Name.ToLowerInvariant().Equals("othertelephonenumber")) || (propertyInfo.Name.ToLowerInvariant().Equals("extnnumber")))
                        {
                            for (int count = 0; count < dataOwner.ChildNodes.Count; count++)
                            {

                                if (dataOwner.ChildNodes.Item(count).Attributes[NAME].ToString().Equals("Phone"))
                                {
                                    blnAddPhoneNode = false;
                                    break;
                                }

                            }
                            if (blnAddPhoneNode)
                            {
                                dataOwner.AppendChild(phoneElement);
                            }
                            xmlelementDetailValue = dataOWnerXML.CreateElement(VALUE);
                            xmlattributeDetailValueName = dataOWnerXML.CreateAttribute(NAME);
                            xmlattributeDetailValueName.Value = GetDisplayName(propertyInfo.Name.ToLowerInvariant());
                            xmlelementDetailValue.Attributes.Append(xmlattributeDetailValueName);

                            xmlelementDetailValue.InnerText = propertyInfo.GetValue(user, null).ToString();
                            phoneElement.AppendChild(xmlelementDetailValue);
                        }
                        else
                        {


                            xmlelementDetail = dataOWnerXML.CreateElement(DETAIL);
                            xmlattributeDetailName = dataOWnerXML.CreateAttribute(NAME);

                            xmlattributeDetailName.Value = GetDisplayName(propertyInfo.Name.ToLowerInvariant());
                            xmlelementDetail.Attributes.Append(xmlattributeDetailName);

                            xmlelementDetailValue = dataOWnerXML.CreateElement(VALUE);
                            xmlattributeDetailValueName = dataOWnerXML.CreateAttribute(NAME);
                            xmlattributeDetailValueName.Value = string.Empty;
                            xmlelementDetailValue.Attributes.Append(xmlattributeDetailValueName);

                            xmlelementDetailValue.InnerText = propertyInfo.GetValue(user, null).ToString();
                            xmlelementDetail.AppendChild(xmlelementDetailValue);
                            dataOwner.AppendChild(xmlelementDetail);

                        }

                    }
                }
            }
            catch
            {
                throw;
            }

        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        private string GetDisplayName(string propertyName)
        {
            //property name of user class are mapped here
            switch (propertyName)
            {
                case "name":
                    return DISPLAYNAME;
                case "department":
                    return DEPARTMENT;
                case "departmentcode":
                    return DEPARTMENTCODE;
                case "company":
                    return COMPANY;
                case "location":
                    return LOCATION;
                case "role":
                    return ROLE;
                case "postalcode":
                    return POSTALCODE;
                case "phone":
                    return PHONE;
                case "telephonenumber":
                    return TELEPHONENO;
                case "mobilenumber":
                    return MOBILENO;
                case "othertelephonenumber":
                    return OTHERNO;
                case "extnnumber":
                    return EXTNNO;
                case "email":
                    return EMAIL;
                case "country":
                    return COUNTRY;
                default:
                    return string.Empty;
            }

        }
        /// <summary>
        /// Gets the active data owner list.
        /// </summary>
        /// <returns></returns>
        public DataTable GetActiveDataOwnerList()
        {
            string strCamlQuery = string.Empty;
            string strFoldrName = string.Empty;
            DataTable dtblDataOwnerList = null;
            DataTable dtblDataOwnerNames = null;
            strCamlQuery = "<Where><Eq><FieldRef Name=\"Active\"/><Value Type=\"Boolean\">1</Value></Eq></Where>";
            try
            {
                objMossController = objFactory.GetServiceManager("MossService");
                dtblDataOwnerList = ((MOSSServiceManager)objMossController).ReadList(strCurrSiteUrl, strListName, strCamlQuery);
                if ((dtblDataOwnerList != null) && (dtblDataOwnerList.Rows.Count > 0))
                {
                    strFoldrName = dtblDataOwnerList.Rows[0]["Title"].ToString();
                    dtblDataOwnerNames = ((MOSSServiceManager)objMossController).ReadFolderList(strCurrSiteUrl, strListName, strFoldrName);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (dtblDataOwnerList != null)
                {
                    dtblDataOwnerList.Dispose();
                }

            }
            return dtblDataOwnerNames;
        }

        #endregion
    }
}


