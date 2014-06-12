#region Shell Copyright.2009
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner 
// 
//Filename: EPCatalogFilter.ascx.cs 
#endregion

using System;
using System.Data;
using System.Collections;
using System.Net;
using System.Web;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using Shell.SharePoint.DREAM.Business.Entities;
using Shell.SharePoint.DREAM.Controller;
using Shell.SharePoint.DREAM.Utilities;
using Telerik.Web.UI;

/// <summary>
/// EPCatalogFilter class
/// </summary>
namespace Shell.SharePoint.DREAM.Site.UI.UserControls
{
    public partial class EPCatalogFilter :UIControlHandler
    {
        #region DECLARATION
        private UISaveSearchHandler objUISaveSearchHandler;
        const string EPCATALOGURL = "/pages/EPCatalogFilterScreen.aspx";
        const string EPCATALOG = "EPCATALOG";
        const string SEARCHRESULTSPAGE = "/pages/EPCatalog.aspx";
        const string ASSET = "Asset";
        const string EPASSETTYPE = "AssetType";
        const string AUTHOR = "Author";
        const string PUBLISHEDDATE = "PublishedDate";
        const string PUBLISHEDSTATUS = "PublishedStatus";
        const string LOGICALFORMAT = "LogicalFormat";
        const string PRODUCTTYPE = "ProductType";

        #endregion

        #region Protected Methods
        /// <summary>
        /// Handles the initialization event of the Page control.
        /// </summary>
        protected void Page_Init()
        {
            try
            {
                objUtility.RenderAjaxBusyBox(this.Page); //rendering busy box.
                //**LoadChapter();
                //**LoadProductTypes();
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(HttpContext.Current.Request.Url.ToString(), ex);
            }
        }
        /// <summary>
        /// Handles Page Load 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                objUISaveSearchHandler = new UISaveSearchHandler();
                if(!string.IsNullOrEmpty(HttpContext.Current.Request.Form["hidSelectedCriteriaName"]))
                    hidColumnName.Value = HttpContext.Current.Request.Form["hidSelectedCriteriaName"].ToString().TrimEnd();
                if(!string.IsNullOrEmpty(Page.Request.QueryString[ASSETTYPE]))
                    hidAssetType.Value = Page.Request.QueryString[ASSETTYPE].ToString();
                if(!string.IsNullOrEmpty(HttpContext.Current.Request.Form["hidSelectedRows"]))
                {
                    hidIdentifiers.Value = HttpContext.Current.Request.Form["hidSelectedRows"].ToString();
                }
                BindTooltipTextToControls();
                //Added in DREAM 4.0 For EPCatalog Product Type Filter Options
                //Start
                AssignAttributes();
                //Populating list boxes on first load
                LoadDefaultProdTypes();
                //End
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
            finally
            {
                objUtility.CloseAjaxBusyBox(this.Page);
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the cboChapter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        /*   protected void cboChapter_SelectedIndexChanged(object sender, EventArgs e)
           {
               if(cboChapter.SelectedIndex != 0)
               {
                   DataTable dtChapter = null;
                   objMossController = objFactory.GetServiceManager("MossService");
                   string strListName = "EPCatalog Product Type Mapping";
                   string strCamlQuery = "<Where><Eq><FieldRef Name='Title' /><Value Type='Text'>" + cboChapter.SelectedValue + "</Value></Eq></Where>";
                   string strViewFields = @"<FieldRef Name='Title'/><FieldRef Name='Product_x0020_Type'/>";
                   try
                   {
                       dtChapter = ((MOSSServiceManager)objMossController).ReadList(strCurrSiteUrl, strListName, strCamlQuery, strViewFields);
                       string strProductType = string.Empty;
                       if(dtChapter != null && dtChapter.Rows[0]["Product_x0020_Type"] != null)
                       {
                           strProductType = (string)dtChapter.Rows[0]["Product_x0020_Type"];
                       }
                       if(!string.IsNullOrEmpty(strProductType))
                       {
                           string[] arrProductType = strProductType.Split(";#".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                           ArrayList arrlstProductType = new ArrayList();
                           for(int intProductTypeCounter = 0; intProductTypeCounter < arrProductType.Length; intProductTypeCounter += 2)
                           {
                               if(!string.IsNullOrEmpty(arrProductType[intProductTypeCounter]))
                               {
                                   arrlstProductType.Add(arrProductType[intProductTypeCounter]);
                               }
                           }
                          //** LoadProductType(arrlstProductType);
                       }
                   }
                   finally
                   {
                       if(dtChapter != null)
                       {
                           dtChapter.Dispose();
                       }
                   }
               }
               else
               {
                   //**LoadProductTypes();
               }

           }*/
        /// <summary>
        /// Reloads the page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdReset_Click(object sender, EventArgs e)
        {
            Response.Redirect(Page.Request.Url.AbsolutePath, false);
        }

        /// <summary>
        /// Creates the Search request XML and redirects to ContextSearchResults page 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdSearch_Click(object sender, EventArgs e)
        {
            try
            {
                objRequestInfo = SetDataObject();
                objUISaveSearchHandler.DisplayResults(Page, objRequestInfo, EPCATALOG);
                System.Web.UI.ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "LoadEPSearchDetails", "<Script language='javascript'>LoadSearchResultsPopup('" + SEARCHRESULTSPAGE + "','" + EPCATALOG + "','" + hidAssetType.Value.TrimEnd() + "');</Script>", false);
            }
            catch(Exception ex)
            {
                CommonUtility.HandleException(strCurrSiteUrl, ex);
            }
            finally
            {
                if(objUISaveSearchHandler != null)
                    objUISaveSearchHandler.Dispose();
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Constructs the RequestInfo object
        /// </summary>
        /// <returns></returns>
        private RequestInfo SetDataObject()
        {
            objRequestInfo.Entity = SetEntity();
            return objRequestInfo;
        }
        /// <summary>
        /// Binds the tooltip text to controls.
        /// </summary>
        private void BindTooltipTextToControls()
        {
            DataTable dtFilterTooltip = null;
            try
            {
                dtFilterTooltip = AssignToolTip();
                imgPublishedDate.ToolTip = GetTooltip(dtFilterTooltip, GetControlID(imgPublishedDate.ID));
                imgAuthorName.ToolTip = GetTooltip(dtFilterTooltip, GetControlID(imgAuthorName.ID));
                imgSelectedProdType.ToolTip = GetTooltip(dtFilterTooltip, GetControlID(imgSelectedProdType.ID));
            }
            finally
            {
                if(dtFilterTooltip != null)
                {
                    dtFilterTooltip.Dispose();
                }
            }
        }
        /// <summary>
        /// Constructs the Entity object
        /// </summary>
        /// <returns></returns>
        private Entity SetEntity()
        {
            Entity objEntity;
            objEntity = new Entity();
            objEntity.ResponseType = TABULAR;
            objEntity.AttributeGroups = new ArrayList();

            AttributeGroup objAttributeGrp = new AttributeGroup();
            objAttributeGrp.Operator = ANDOPERATOR;
            objAttributeGrp.Attribute = new ArrayList();
            if(!string.IsNullOrEmpty(txtAUTHORNAME.Text))
            {
                Attributes objAuthorAttribute = new Attributes();
                objAuthorAttribute.Name = AUTHOR;
                objAuthorAttribute.Operator = EQUALSOPERATOR;
                objAuthorAttribute.Value = new ArrayList();

                Value objAuthorVal = new Value();
                objAuthorVal.InnerText = txtAUTHORNAME.Text;
                objAuthorAttribute.Value.Add(objAuthorVal);
                objAttributeGrp.Attribute.Add(objAuthorAttribute);
            }
            //Commented in DREAM 4.0if (IsOptionSelected(lstProductTypes))
            if(lstSelectedProdType.Items.Count > 0)
            {
                // ** code for adding group of product types to added**//
                Attributes objProductType = new Attributes();
                objProductType.Name = PRODUCTTYPE;
                objProductType.Operator = INOPERATOR;
                objProductType.Value = new ArrayList();
                Value objLogiVal = null;
                for(int intProductTypeCounter = 0; intProductTypeCounter < lstSelectedProdType.Items.Count; intProductTypeCounter++)
                {
                    //Commented in DREAM 4.0 if(lstProductTypes.Items[intProductTypeCounter].Selected)
                    {
                        objLogiVal = new Value();
                        objLogiVal.InnerText = lstSelectedProdType.Items[intProductTypeCounter].Value;
                        objProductType.Value.Add(objLogiVal);
                    }
                }
                objAttributeGrp.Attribute.Add(objProductType);
            }
            //adding asset value               
            if(!string.IsNullOrEmpty(hidIdentifiers.Value))
            {
                string strPattern = @"\r\n";
                Regex fixMe = new Regex(strPattern);
                string strTrimmedMyAssetValues = fixMe.Replace(hidIdentifiers.Value, string.Empty);

                string[] arrAssetVal = strTrimmedMyAssetValues.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                Attributes objAssetValue = new Attributes();
                objAssetValue.Name = ASSET;
                objAssetValue.Operator = EQUALSOPERATOR;
                objAssetValue.Value = new ArrayList();

                foreach(string str in arrAssetVal)
                {
                    Value objVal = new Value();
                    objVal.InnerText = str.Trim();
                    objAssetValue.Value.Add(objVal);
                }
                objAttributeGrp.Attribute.Add(objAssetValue);
            }
            //Commented for Dream 3.1 fix
            //Attributes objAssetTypeAttribute = new Attributes();
            //objAssetTypeAttribute.Name = EPASSETTYPE;
            //objAssetTypeAttribute.Operator = EQUALSOPERATOR;
            //objAssetTypeAttribute.Value = new ArrayList();

            //Value objAssetTypeVal = new Value();
            //if (hidAssetType.Value.ToLowerInvariant().Equals("wellbore"))
            //{
            //    hidAssetType.Value = "well";
            //}
            //objAssetTypeVal.InnerText = hidAssetType.Value;
            //objAssetTypeVal.InnerText = hidAssetType.Value;
            //objAssetTypeAttribute.Value.Add(objAssetTypeVal);
            //objAttributeGrp.Attribute.Add(objAssetTypeAttribute);

            AddCountryAttribute(objAttributeGrp.Attribute);//DREAM 4.0

            objEntity.AttributeGroups.Add(objAttributeGrp);
            //adding published date criteria
            if((!string.IsNullOrEmpty(txtStartDate.Text)) && (!string.IsNullOrEmpty(txtEndDate.Text)))
            {
                AddPublishDateCriteria(objEntity);
            }
            return objEntity;
        }

        /// <summary>
        /// Adds the publish date criteria.
        /// </summary>
        /// <param name="requestEntity">The request entity.</param>
        private void AddPublishDateCriteria(Entity requestEntity)
        {
            requestEntity.Criteria = new Criteria();
            requestEntity.Criteria.Value = "[PublishDate] >=" + objDateTimeConvertorService.GetDateInFormat(txtStartDate.Text, WEBSERVICEDATEFORMAT) + " And  [PublishDate] <=" + objDateTimeConvertorService.GetDateInFormat(txtEndDate.Text, WEBSERVICEDATEFORMAT);
            requestEntity.Criteria.Operator = GetOperator("*");
        }
        /// <summary>
        /// Loads product type list boxes from sharepoint list.
        /// </summary>
        /// <param name="listBox">The list box.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="columnName">Name of the column.</param>
        private void PopulateProdTypeLstBox(RadListBox listBox, string listName, string columnName)
        {
            DataTable dtListItems = null;
            objMossController = objFactory.GetServiceManager("MossService");
            string strCamlQuery = "<OrderBy><FieldRef Name=\"" + columnName + "\"/></OrderBy>";
            string strViewFields = @"<FieldRef Name='" + columnName + "'/>";
            try
            {
                dtListItems = ((MOSSServiceManager)objMossController).ReadList(strCurrSiteUrl, listName, strCamlQuery, strViewFields);
                if((dtListItems != null) && (dtListItems.Rows.Count > 0))
                {
                    listBox.DataTextField = columnName;
                    listBox.DataValueField = columnName;//dtListItems.Columns[columnName].ColumnName;
                    listBox.DataSource = dtListItems;
                    listBox.DataBind();
                }
            }
            finally
            {
                if(dtListItems != null)
                {
                    dtListItems.Dispose();
                }

            }
        }

        /// <summary>
        /// Loads the default prod types.
        /// </summary>
        private void LoadDefaultProdTypes()
        {
            PopulateProdTypeLstBox(lstGrpOfProdType, "EPCatalog Product Type", "Title");
            PopulateProdTypeLstBox(lstRgnGrpOfProdType, "EPCatalog Regional Group Of Product Types", "Title");
            PopulateProdTypeLstBox(lstAdditonalProdType, "EPCatalog Additonal Product Types", "Title");
            txtBxSrchGrpOfProdType.Text = string.Empty;
            txtBxSrchRgnGrpOfProdType.Text = string.Empty;
            txtBxSrchAdditonalProdType.Text = string.Empty;
        }
        /// <summary>
        /// Assigns the attributes.
        /// </summary>
        private void AssignAttributes()
        {
            cmdSearch.Attributes.Add("onclick", "return ValidateEPCatalogDates();");
            lstGrpOfProdType.Attributes.Add("ListName", "EPCatalog Product Type");
            lstGrpOfProdType.Attributes.Add("ColumnName", "Title");
            lstRgnGrpOfProdType.Attributes.Add("ListName", "EPCatalog Regional Group Of Product Types");
            lstRgnGrpOfProdType.Attributes.Add("ColumnName", "Title");
            lstAdditonalProdType.Attributes.Add("ListName", "EPCatalog Additonal Product Types");
            lstAdditonalProdType.Attributes.Add("ColumnName", "Title");
            //Added to keep track of last searchtype selection to 
            //aviod repetetion of task on selection of same search type multiple times.
            rbLstEPCatalogSrchType.Attributes.Add("LastSelection", "ProductType");
        }
        /// <summary>
        /// Loads the type of the product mapped to particular chapter.
        /// </summary>
        /// <param name="productType">Type of the product.</param>
        /*  private void LoadProductType(ArrayList productType)
          {
              if(productType != null)
              {
                  productType.Sort();
                  lstProductTypes.Items.Clear();
                  foreach(string strProductType in productType)
                  {
                      if((!string.IsNullOrEmpty(strProductType)) && (lstProductTypes.Items.FindByText(strProductType) == null))
                      {
                          lstProductTypes.Items.Add(strProductType);
                      }
                  }
              }
          }*/
        /// <summary>
        /// Loads the chapter.
        /// </summary>
        /*  private void LoadChapter()
          {
              objMossController = objFactory.GetServiceManager("MossService");
              string strListName = "EPCatalog Product Type Mapping";
              string strCamlQuery = "<OrderBy><FieldRef Name='Title' Ascending='True' /></OrderBy>";
              string strViewFields = @"<FieldRef Name='Title'/>";
              DataTable dtChapter = null;
              try
              {
                  dtChapter = ((MOSSServiceManager)objMossController).ReadList(strCurrSiteUrl, strListName, strCamlQuery, strViewFields);

                  if((dtChapter != null) && (dtChapter.Rows.Count > 0))
                  {
                      DataView dvChapter = new DataView(dtChapter);
                      dtChapter = dvChapter.ToTable(true, "Title");
                      cboChapter.Items.Clear();
                      cboChapter.DataTextField = dtChapter.Columns["Title"].ColumnName;
                      cboChapter.DataValueField = dtChapter.Columns["Title"].ColumnName;
                      cboChapter.DataSource = dtChapter;
                      cboChapter.DataBind();
                  }
                  cboChapter.Items.Insert(0, "--Show All--");
              }
              finally
              {
                  if(dtChapter != null)
                  {
                      dtChapter.Dispose();
                  }
              }
          }*/
        private void AddCountryAttribute(ArrayList attributes)
        {
            if(!string.IsNullOrEmpty(Page.Request.QueryString["country"]) && !Page.Request.QueryString["country"].Trim().Equals("0"))
            {
                attributes.Add(AddAttribute("Country", "EQUALS", new string[] { Page.Request.QueryString["country"].Trim() }));
            }
        }
        #endregion

    }
}