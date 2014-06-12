/* 
 * *******************************************************************************
 * File Name        :   EPCatalogFilter.js
 * Project Name     :   SHELL_DREAM_4.0
 * Type             :   JavaScript Function  
 * Date_Created     :   Jun 8 2011
 * Version          :   1.0 
 * *******************************************************************************
 */
 
 /***************************************************************
             function to get Search Results
***************************************************************/
function getSearchResults(searchCriteria,lstBxId)
{
     var objLstBxResult= $find(GetObjectID(lstBxId, 'div'));
     var arrAttributes =objLstBxResult.get_attributes();
     var strListName = arrAttributes.getAttribute("ListName");
     var strColumnName = arrAttributes.getAttribute("ColumnName");
     var arrSearchResult = dreamAjaxServiceProxy.GetSearchResults(searchCriteria,strListName,strColumnName,ShowResult,RequestFailedCallback,objLstBxResult);   
}
/***************************************************************
             function to Show Result
***************************************************************/
function ShowResult(result,objLstBxResult,methodName)
{
    var items = objLstBxResult.get_items();
    objLstBxResult.trackChanges();
    items.clear();
    for(index=0;index<result.length;index++)
    {
        var item = new Telerik.Web.UI.RadListBoxItem();
        item.set_text(result[index]);
        //item.set_value(result[index]); //commented for now ,can be used later
        items.add(item);
    }
    objLstBxResult.commitChanges();
}
/***************************************************************
             function to EPCatalog Search Type OnChange
***************************************************************/
function EPCatalogSrchTypeOnChange(rbSearchType,rbLstId)
{
    var tblRbLst = document.getElementById(GetObjectID(rbLstId, 'table'));
    if(tblRbLst.LastSelection != rbSearchType.value)
    {
        tblRbLst.LastSelection = rbSearchType.value;
        if(rbSearchType.value == "ProductType")
        {
            //Populate listboxes
            PopulateProdTypeListBox("txtBxSrchGrpOfProdType","lstGrpOfProdType","EPCatalog Product Type","Title")
            PopulateProdTypeListBox("txtBxSrchRgnGrpOfProdType","lstRgnGrpOfProdType","EPCatalog Regional Group Of Product Types","Title")
            PopulateProdTypeListBox("txtBxSrchAdditonalProdType","lstAdditonalProdType","EPCatalog Additonal Product Types","Title")
            //Changing label
            ChangeListBoxLabel("EP Catalog Group of Product Types","Regional Group of Product Types","Selected Product Types")
            //Show/Hide Listboxes
            ShowHideAddProdType('block');
        }
        else if(rbSearchType.value == "KidType")
        {
            //Populate listboxes
            PopulateProdTypeListBox("txtBxSrchGrpOfProdType","lstGrpOfProdType","EPCatalog Group Of KID Types","Title")
            PopulateProdTypeListBox("txtBxSrchRgnGrpOfProdType","lstRgnGrpOfProdType","EPCatalog Additonal KID Types","Title")
            //Changing label of listboxes
            ChangeListBoxLabel("EP Catalog Group of KID Types","Additional KID Types","Selected KID Types")
            //Show\Hide Listboxes
            ShowHideAddProdType('none');
        }
        else if(rbSearchType.value == "Discipline")
        {
            //Populate listboxes
            PopulateProdTypeListBox("txtBxSrchGrpOfProdType","lstGrpOfProdType","EPCatalog Group Of Disciplines","Title")
            PopulateProdTypeListBox("txtBxSrchRgnGrpOfProdType","lstRgnGrpOfProdType","EPCatalog Additonal Disciplines","Title")
            //Changing label
            ChangeListBoxLabel("EP Catalog Group Of Disciplines","Additonal Disciplines","Selected Disciplines")
            //Show/Hide Listboxes
            ShowHideAddProdType('none');
        }
        //Clearing selection
        EPCatalogClearSelection();
    }
}
/***************************************************************
             function to Populate Product Type ListBox
***************************************************************/
function PopulateProdTypeListBox(searchBoxId,lstBxId,listName,columnName)
{
    var objLstBxResult= $find(GetObjectID(lstBxId, 'div'));
    var objSrchBxId = document.getElementById(GetObjectID(searchBoxId, 'input'));
    var arrAttributes =objLstBxResult.get_attributes();
    //Set textbox value to empty
    objSrchBxId.value = "";
    //Set new attribute
    arrAttributes.setAttribute("ListName", listName);
    arrAttributes.setAttribute("ColumnName", columnName);
    //
    var strListName = arrAttributes.getAttribute("ListName");
    var strColumnName = arrAttributes.getAttribute("ColumnName");
    var arrSearchResult = dreamAjaxServiceProxy.GetSearchResults("",strListName,strColumnName,ShowResult,RequestFailedCallback,objLstBxResult);   
}
/***************************************************************
             function to Change ListBox Label
***************************************************************/
function ChangeListBoxLabel(lblText1,lblText2,lblText3)
{
    var lblProdType1 = document.getElementById('lblProdType1');
    var lblProdType2 = document.getElementById('lblProdType2');
    var lblSelectedTypes = document.getElementById('lblSelectedTypes');
    lblProdType1.innerText = lblText1;
    lblProdType2.innerText = lblText2;
    lblSelectedTypes.innerText = lblText3;
}
/***************************************************************
             function to Show Hide Additional Producut Type
***************************************************************/
function ShowHideAddProdType(display)
{
    var objRow = null;
    var strRowId =null;
    for(counter=1;counter<=3;counter++)
    {
        strRowId = "trAdditonalProdType" + counter.toString();
        objRow = document.getElementById(strRowId);
        if(objRow!=null)
            objRow.style.display = display;
    }
}
/***************************************************************
             function to EPCatalog Clear Selection
***************************************************************/
function EPCatalogClearSelection()
{
    var lstSelectedProdType = $find(GetObjectID('lstSelectedProdType', 'div'));
    lstSelectedProdType.trackChanges();
    lstSelectedProdType.get_items().clear();
    lstSelectedProdType.commitChanges();
}