#region Shell Copyright.2008
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//
//Filename: Constants.cs
#endregion

/// <summary> 
/// This class contains the list of constanats used.
/// </summary> 

namespace Shell.SharePoint.DREAM.Controller
{
    /// <summary>
    /// This constanats class
    /// </summary>
    public abstract class Constants
    {
        #region CONSTANTS DECLARATION
        protected const string XMLFILENAME = "Request.xml";
        protected const string REQUESTINFO = "requestinfo";
        protected const string ENTITY = "entity";
        protected const string RESPONSETYPE = "responsetype";
        protected const string PROPERTY = "property";
        protected const string ATTRIBUTE = "attribute";
        protected const string ATTRIBUTEGROUP = "attributegroup";
        protected const string PARAMETER = "parameter";
        protected const string CRITERIA = "criteria";
        protected const string NAME = "name";
        protected const string OPERATOR = "operator";
        protected const string LIKE = "LIKE";
        protected const string EQUALS = "EQUALS";
        protected const string IN = "IN";
        protected const string VALUE = "value";
        protected const string XML = "xml";
        protected const string XMLVERSION = "version='1.0'";
        protected const string ENCODING = "encoding='UTF-8'";
        protected const string TYPE = "type";
        protected const string LEVEL = "level";
        protected const string SAVESEARCHREQUESTS = "saveSearchRequests";
        protected const string SAVESEARCHREQUEST = "saveSearchRequest";
        protected const string ORDER = "order";
        protected const string LABEL = "label";
        protected const string CHECKED = "checked";
        protected const string SHARED = "shared";
        protected const string ID = "requestid";
        protected const string SESSIONID = "sessionid";
        protected const string SKIPRECORDS = "skiprecords";
        protected const string MAXPAGERECORDS = "maxpagerecords";
        protected const string BOOKMARKS = "BookMarks";
        protected const string BOOKMARK = "BookMark";
        protected const string IDENTIFIERNAME = "IdentifierName";
        protected const string MYASSETLIB = "My Assets";
        protected const string MYTEAMLIB = "My Team Assets";
        protected const string ASSETWELL = "well";
        protected const string ASSETWELLBORE = "wellbore";
        protected const string ASSETBASIN = "basin";
        protected const string ASSETFIELD = "field";
        protected const string ASSETPARS = "project_archives";
        protected const string ASSETLOGSBYFIELD = "logs_by_field_depth";
        protected const string QUERY = "query";
        protected const string DISPLAY = "display";
        protected const string USERNAME = "UserName";
        protected const string REQUESTID = "See";
        protected const string INCLUDEMETADATA = "IncludeMetadata";
        protected const string TVDSS = "tvdss";
        protected const string PROJECTNAME = "project";
        #region SRP
        protected const string ISRANGEAPPLICABLE = "IsRangeApplicable";
        protected const string ASSETRESERVOIR = "reservoir";
        #endregion
        #region EP-Catalog CONSTANTS
        protected const string FUZZY = "Fuzzy";
        protected const string OBSOLETE = "Obsolete";
        protected const string SPARSE = "Sparse";
        #endregion
        #region QueryBuilder
        protected const string DATASOURCE = "DataSource";
        protected const string DATASOURCENAME = "DataSourceName";
        protected const string DATAPROVIDERNAME = "DataProvider";
        protected const string DATATABLE = "TableName";
        protected const string DATASOURCELISTNAME = "Query Search Data Source";
        protected const string DATAPROVIDERLISTNAME = "Query Search Data Provider";
        #endregion

        #region Response XML Interceptor
        protected const string RECORDXPATH = "response/report/record";
        protected const string ATTRIBUTEXPATH = "response/report/record/attribute";
        protected const string ATTRIBUTEXPATHWITHNAME = "response/report/record/attribute[@{0}]";
        protected const string ATTRIBUTEXPATHWITHNAMEVALUE = "response/report/record/attribute[@{0}='{1}']";
        protected const string BALLYATTRIBUTEXPATHTABULAR = "Tectonic Setting Bally";
        protected const string KLEMMEATTRIBUTEXPATHTABULAR = "Tectonic Setting Klemme";
        protected const string DATASHEETVIEW = "datasheet";
        protected const string TABULARVIEW = "tabular";
        protected const string MOSSSERVICE = "MossService";
        protected const string TECTONICSETTINGLIST = "Tectonic Setting";
        protected const string BASINNAMTATTRIBUTE = "Basin Name";
        protected const string TITLE = "Title";
        #endregion

        #region Site URL

        #endregion
        #endregion
    }
}
