#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: QuickSearchHelper.cs 
#endregion
/// <summary> 
/// This is QuickSearch Helper class
/// </summary>
using System;
using System.Data;
using System.Collections;
using System.Web;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Controller;
using Microsoft.SharePoint;

namespace Shell.SharePoint.WebParts.DREAM.SearchResults
{
    /// <summary>
    /// This is the helper class for search Result webpart 
    /// </summary>
    public class QuickSearchHelper
    {
        #region PrivateMemberDeclaration
        private string strPageNumber = string.Empty;
        private string strRequestId = string.Empty;
        private string strQuickCountry = string.Empty;
        private string strQuickAsset = string.Empty;
        private string strQuickColumn = string.Empty;
        private string strQuickCriteria = string.Empty;
        private string strCurrSiteUrl = string.Empty;
        private string strListSearch = string.Empty;
       // private SkipInfo objSkipInfo;
        //constants                
        private const string ANYASSETQUERYSTRING = "ANYASSET";
        private const string ANDOPERATOR = "AND";
        private const string ANYCOUNTRYQUERYSTRING = "ANYCOUNTRY";
        private const string COUNTRYLIST = "Country";
        private const string COUNTRYNAME = "country";
        private const string BASIN = "basin";
        private const string PROJECTARCHIVES = "Project Archives";
        //constant operators
        private const string EQUALSOPERATOR = "EQUALS";
        private const string LIKEOPERATOR = "LIKE";
        private const string INOPERATOR = "IN";
        private const string STAROPERATOR = "*";
        const string AMPERSANDOPERATOR = "%";
        const string TABULAR = "Tabular";

        AbstractController objMossController;
        ServiceProvider objFactory = new ServiceProvider();
        #endregion
        #region Properties
        /// <summary>
        /// Gets or sets the list.
        /// </summary>
        /// <value>The list.</value>
        public string List
        {
            get
            {
                return strListSearch;
            }
            set
            {
                strListSearch = value;
            }
        }
        /// <summary>
        /// Gets or sets the asset.
        /// </summary>
        /// <value>The asset.</value>
        public string Asset
        {
            get
            {
                return strQuickAsset;
            }
            set
            {
                strQuickAsset = value;
            }
        }
        /// <summary>
        /// Gets or sets the pagenumber.
        /// </summary>
        /// <value>The asset.</value>
        public string pageNumber
        {
            get
            {
                return strPageNumber;
            }
            set
            {
                strPageNumber = value;
            }
        }
        /// <summary>
        /// Gets or sets the requestid.
        /// </summary>
        /// <value>The asset.</value>
        public string RequestId
        {
            get
            {
                return strRequestId;
            }
            set
            {
                strRequestId = value;
            }
        }
        /// <summary>
        /// Gets or sets the criteria.
        /// </summary>
        /// <value>The criteria.</value>
        public string Criteria
        {
            get
            {
                return strQuickCriteria;
            }
            set
            {
                strQuickCriteria = value;
            }
        }
        /// <summary>
        /// Gets or sets the column.
        /// </summary>
        /// <value>The column.</value>
        public string Column
        {
            get
            {
                return strQuickColumn;
            }
            set
            {
                strQuickColumn = value;
            }
        }
        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>The country.</value>
        public string Country
        {
            get
            {
                return strQuickCountry;
            }
            set
            {
                strQuickCountry = value;
            }
        }
        /// <summary>
        /// Gets or sets the curr site URL.
        /// </summary>
        /// <value>The curr site URL.</value>
        public string CurrSiteUrl
        {
            get
            {
                return strCurrSiteUrl;
            }
            set
            {
                strCurrSiteUrl = value;
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Sets the quick Search data objects.
        /// </summary>
        /// <returns></returns>
        public RequestInfo SetQuickDataObjects()
        {
            RequestInfo objRequestInfo = new RequestInfo();
            try
            {
                //this will set the values for basic data Objects.
                // Commented after new pagination implementation
                /* if (pageNumber.Length > 0 || isFetchAll)
                 {
                     Entity objEntity = new Entity();
                     objEntity.SkipInfo = SetSkipInfo(isFetchAll, fetchCount, userPreference);
                     objEntity.RequestID = RequestId;
                     objEntity.ResponseType = RESPONSETYPE;
                     objEntity.Property = true;
                     objRequestInfo.Entity = objEntity;
                 }
                 else*/
                {
                    objRequestInfo.Entity = SetQuickEntity();
                }
            }
            catch(Exception)
            {
                throw;
            }
            return objRequestInfo;
        }
        //commented in new paging implementation
        /// <summary>
        /// set the skip record count, request id and maxfetch record
        /// </summary>
        /// <returns></returns>
      /*  private SkipInfo SetSkipInfo(bool isFetchAll, string fetchCount, UserPreferences userPreference)
        {
            try
            {
                int intMaxRecordPage = 0;
                //Get the value from Userpreferences
                objSkipInfo = new SkipInfo();
                intMaxRecordPage = Convert.ToInt32(userPreference.RecordsPerPage);
                objSkipInfo.SkipRecord = Convert.ToString(Convert.ToInt32(pageNumber) * intMaxRecordPage);
                if(!isFetchAll)
                    objSkipInfo.MaxFetch = userPreference.RecordsPerPage;
                else
                    objSkipInfo.MaxFetch = fetchCount;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return objSkipInfo;
        }*/
        /// <summary>
        /// This function will set the Entity object.
        /// </summary>
        /// <returns></returns>
        private Entity SetQuickEntity()
        {
            Entity objEntity = null;
            //initializes the EntityList object.            
            objEntity = new Entity();
            objEntity.ResponseType = TABULAR;
            objEntity.Property = true;
            Criteria objCriteria = new Criteria();
            if(List.Length > 0)
            {
                ArrayList arlAttribute = new ArrayList();
                arlAttribute = SetQuickAttribute();
                objEntity.Attribute = arlAttribute;
            }
            else
            {
                ArrayList arlAttribute = new ArrayList();
                arlAttribute = SetQuickAttribute();
                //the below condition check for asset value.
                //R5k changes
               /* if(!string.Equals(Asset, "Basin") && !string.Equals(Asset, "Field"))*/
                {
                    if(arlAttribute.Count > 1)
                    {
                        objEntity.AttributeGroups = SetQuickAttributeGroup(arlAttribute);
                    }
                    else
                    {
                        objEntity.Attribute = arlAttribute;
                    }
                    objCriteria = SetQuickCriteria(Criteria);
                    objEntity.Criteria = objCriteria;
                }
                    //R5k changes
                /* else if(string.Equals(Asset, "Basin"))
                 {
                     objEntity.Attribute = arlAttribute;
                     objEntity.Criteria = SetCriteria(Criteria);
                 }
                 else
                {
                    objEntity.Attribute = arlAttribute;
                    objEntity.Criteria = SetCriteria(Criteria);
                }*/
            }
            return objEntity;
        }
        /// <summary>
        /// Sets the quick attribute group.
        /// </summary>
        /// <returns></returns>
        private ArrayList SetQuickAttributeGroup(ArrayList quickAttribute)
        {
            ArrayList arlQuickAttributeGroup = new ArrayList();
            try
            {
                //initializes the attribute group object.
                AttributeGroup objQuickAttributeGroup = new AttributeGroup();
                objQuickAttributeGroup.Operator = GetLogicalOperator();
                objQuickAttributeGroup.Attribute = quickAttribute;
                arlQuickAttributeGroup.Add(objQuickAttributeGroup);
            }
            catch(Exception)
            {
                throw;
            }
            return arlQuickAttributeGroup;
        }
        /// <summary>
        /// Gets the logical operator.
        /// </summary>
        /// <returns></returns>
        private string GetLogicalOperator()
        {
            string strOperator;
            strOperator = ANDOPERATOR;
            //returns the logical operator
            return strOperator;
        }
        /// <summary>
        /// Read active countries from 'Country' SPList and add to arraylist
        /// </summary>
        private ArrayList ReadActiveCountriesList()
        {
            ArrayList arlActiveCountries = new ArrayList();
            DataTable objDtCountryList = new DataTable();
            try
            {
                objMossController = objFactory.GetServiceManager("MossService");
                string strCamlQuery = "<OrderBy><FieldRef Name=\"Title\"/></OrderBy><Where><Eq>" + "<FieldRef Name=\"Active\" /><Value Type=\"Choice\">Yes</Value></Eq></Where>";
                //this will read values from Country Sharepoint list.
                objDtCountryList = ((MOSSServiceManager)objMossController).ReadList(CurrSiteUrl, COUNTRYLIST, strCamlQuery);
                if(objDtCountryList != null)
                {
                    if(objDtCountryList.Rows.Count > 0)
                    {
                        //Loop through the values in country list.
                        foreach(DataRow dtRow in objDtCountryList.Rows)
                        {
                            if(string.Compare(PROJECTARCHIVES, Asset, true) == 0)
                            {
                                arlActiveCountries.Add(SetValue(dtRow["Title"].ToString()));
                            }
                            else
                            {
                                arlActiveCountries.Add(SetValue(dtRow["Country_x0020_Code"].ToString()));
                            }
                        }
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                if(objDtCountryList != null)
                    objDtCountryList.Dispose();
            }
            return arlActiveCountries;
        }
        /// <summary>
        /// Sets the string field as a Value object.
        /// </summary>
        /// <param name="strField">field.</param>
        /// <returns></returns>
        private Value SetValue(string field)
        {
            Value objValue = new Value();
            objValue.InnerText = field;
            //returns the value object.
            return objValue;
        }
        /// <summary>
        /// Gets the query operator for request xml
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private string GetOperator(ArrayList value)
        {
            string strOperator = string.Empty;
            try
            {
                //the below condition validates the value parameter.
                if(value.Count > 1)
                {
                    strOperator = INOPERATOR;
                }
                else
                {
                    //Loop through the values in ArrayList.
                    foreach(Value objValue in value)
                    {
                        //the below condition check for the innerText value.
                        if((objValue.InnerText.Contains(STAROPERATOR)) || (objValue.InnerText.Contains(AMPERSANDOPERATOR)))
                            strOperator = LIKEOPERATOR;
                        else
                            strOperator = EQUALSOPERATOR;
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }
            return strOperator;
        }

        /// <summary>
        /// Sets the attribute node for request xml.
        /// </summary>
        /// <returns></returns>
        private ArrayList SetQuickAttribute()
        {
            ArrayList arlAttribute = new ArrayList();
            try
            {
                ArrayList arlValue = new ArrayList();
                if(List.Length > 0)
                {
                    string strIdentifiedValues = string.Empty;
                    string[] arrIdentifierValue = new string[1];
                    Attributes objAttribute = new Attributes();
                    //this will set the SelectedCriteria name and selected rows values from hidden fields
                    objAttribute.Name = HttpContext.Current.Request.Form["hidSelectedCriteriaName"].ToString().Trim();
                    strIdentifiedValues = HttpContext.Current.Request.Form["hidSelectedRows"].ToString();
                    arrIdentifierValue = strIdentifiedValues.Split('|');
                    //Loop through the Selected Identifier Values
                    foreach(string strAttributeValue in arrIdentifierValue)
                    {
                        int checkDuplicate = 0;
                        //validates the attribute value.
                        if(strAttributeValue.Trim().Length > 0)
                        {
                            foreach(Value objValue in arlValue)
                            {
                                if(string.Equals(strAttributeValue.Trim(), objValue.InnerText.ToString()))
                                {
                                    checkDuplicate++;
                                }
                            }
                            if(checkDuplicate == 0)
                            {
                                arlValue.Add(SetValue(strAttributeValue.Trim()));
                            }
                        }
                    }
                    objAttribute.Value = arlValue;
                    objAttribute.Operator = GetOperator(objAttribute.Value);
                    arlAttribute.Add(objAttribute);
                }
                else
                {
                    if(!string.Equals(Asset, "Basin"))
                    {
                        Attributes objCountry = new Attributes();
                        //the below condition check the selected option for country.
                        if(string.Equals(Country.ToUpper().Trim(), ANYCOUNTRYQUERYSTRING))
                        {
                            objCountry.Name = COUNTRYNAME;
                            arlValue = ReadActiveCountriesList();   //for 'ANYCOUNTRY' read all active countries from SPList
                            objCountry.Value = arlValue;
                            objCountry.Operator = GetOperator(objCountry.Value);
                        }
                        else
                        {
                            objCountry.Name = COUNTRYNAME;
                            arlValue.Add(SetValue(Country));
                            objCountry.Value = arlValue;
                            objCountry.Operator = GetOperator(objCountry.Value);
                        }
                        arlAttribute.Add(objCountry);
                    }
                    else
                    {
                        //**Dream 3.1 fix
                        /*ArrayList arlActiveBasinList = new ArrayList();
                        arlActiveBasinList = GetActiveBasin();
                        Attributes objBasin = new Attributes();

                        objBasin.Name = BASIN;
                        if (arlActiveBasinList.Count != 0)
                        {
                            //Loop through the basin list.
                            foreach (String strActiveBasin in arlActiveBasinList)
                            {
                                arlValue.Add(SetValue(strActiveBasin));
                            }
                        }
                        else
                        {
                            arlValue.Add(SetValue("*"));
                        }
                        objBasin.Value = arlValue;
                        objBasin.Operator = GetOperator(objBasin.Value);
                        arlAttribute.Add(objBasin);*/
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }
            return arlAttribute;
        }
        //commented in dream 3.1 fix
        /// <summary>
        /// Gets the active basin.
        /// </summary>
        /// <returns></returns>
        /*private ArrayList GetActiveBasin()
        {
            DataTable objListData = null;
            DataRow objListRow;

            string strActiveBasin = string.Empty;
            try
            {
                objMossController = objFactory.GetServiceManager("MossService");
                string strListname = "Basin";
                string strCamlQuery = "<OrderBy><FieldRef Name=\"Title\"/></OrderBy><Where><Eq>" + "<FieldRef Name=\"Active\" /><Value Type=\"Choice\">Yes</Value></Eq></Where>";
                string strParentSiteURL = SPContext.Current.Site.Url.ToString();
                //this will read values from Basin Sharepoint list.
                objListData = ((MOSSServiceManager)objMossController).ReadList(strParentSiteURL, strListname, strCamlQuery);
                ArrayList arlActiveBasin = new ArrayList();
                if(objListData != null)
                {
                    //Loop through the values in Basin list.
                    for(int i = 0; i < objListData.Rows.Count; i++)
                    {
                        objListRow = objListData.Rows[i];
                        strActiveBasin = objListRow["Title"].ToString();
                        arlActiveBasin.Add(strActiveBasin);
                    }
                }
                //returns the Active Basin names.
                return arlActiveBasin;
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                if(objListData != null)
                    objListData.Dispose();
            }
        }*/
        /// <summary>
        /// Gets the query operator for request xml
        /// </summary>
        /// <param name="strValue">value.</param>
        /// <returns></returns>
        private string GetOperator(string value)
        {
            string strOperator;
            try
            {
                //the below condition check for the innerText value.
                if((value.Contains(STAROPERATOR)) || (value.Contains(AMPERSANDOPERATOR)))
                    strOperator = LIKEOPERATOR;
                else
                    strOperator = EQUALSOPERATOR;
            }
            catch(Exception)
            {
                throw;
            }
            return strOperator;
        }
        /// <summary>
        /// This function will set search criteria to Criteria object.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        private Criteria SetCriteria(string criteria)
        {
            Criteria objCriteria = new Criteria();
            objCriteria.Value = criteria;
            objCriteria.Operator = GetOperator(objCriteria.Value);
            //returns the search criteria object.
            return objCriteria;
        }

        /// <summary>
        /// This function will set search criteria to Criteria object.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        private Criteria SetQuickCriteria(string criteria)
        {
            Criteria objCriteria = new Criteria();
            if(Column.Length > 0)
                objCriteria.Name = Column;
            objCriteria.Value = criteria;
            objCriteria.Operator = GetOperator(objCriteria.Value);
            //returns the search criteria object.
            return objCriteria;
        }
        #endregion
    }
}
