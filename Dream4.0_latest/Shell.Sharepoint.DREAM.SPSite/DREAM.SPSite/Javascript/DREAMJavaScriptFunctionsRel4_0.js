
/* 
 * *******************************************************************************
 * File Name        :   DREAMJavaScriptFunctionsRel4_0.js
 * Project Name     :   DREAM 4.0
 * Type             :   JavaScript Function  
 * Date Created     :   January 10 2011
 * Version          :   4.0 
 * Description      :   New javascript functions for DREAM 4.0 release.
 * *******************************************************************************
 */
 
 
/***************************************************************
function to handle Paging click in My Asset, My Team Asset.
***************************************************************/
function MyAssetPaging(URL,pageNumber,maxRecords,recordcount,requestid,sortBy,sortType,searchType)
{
   var strParams ='pagenumber='+pageNumber+'&MaxRecords='+maxRecords+'&RecordCount='+recordcount+'&RequestId='+requestid+'&sortby='+sortBy+'&sorttype='+sortType+'&searchtype='+searchType;  
   try
   {
     
     __doPostBack(GetObjectID('UpdatePanelContentPage','div'),strParams);  
   }  
   catch(Ex)
   {
     alert(Ex.message);
   } 
}

/***************************************************************
function to perform Export All and Export Page options.
***************************************************************/

function ContinueExportOption(reporType)
{
var fileType = SetExportFileType();
var SEARCHNAMEFORUBIWRKSHEET = "wellboreheader|picks|picksdetail|directionalsurvey|directionalsurveydetail|timedepth|timedepthdetail|geopressure|recalllogs|recallcurve|zoneproperties";

/// Check Export Page option is selected
if(document.getElementById(GetObjectID('rdblExportSelectionCurrentPage', 'input')).checked == true)
{
   if(fileType == "excel")
   {
    /// Call Export All functionality
    if(SEARCHNAMEFORUBIWRKSHEET.indexOf(reporType.toLowerCase()) != -1)
    {
        ExportUBIToWorkSheet();
    }
    else if (reporType.toLowerCase() == "edmreport")
    {
      ExportEDMSearchResults('tblSearchResults');
    }
    else if(reporType.toLowerCase() == "mechanicaldata")
    {
    ExportMDSearchResults('tblHole','tblCasings','tblLiners','tblMechanicalcontent','tblFluidscements','tblGrossperforations','tblWellhead');
    }
    else if(reporType.toLowerCase() == "welltestreport")
    {
    ExportWellTestSearchResults('tblgeneraltestdata','tbltestanalysisdata','tbltestformationdata','tbltestflowdata','tbltestintervaldata');
    }
     else if (reporType.toLowerCase() == "paleomarkers")
    {
        ExportTabTabularSearchResults('tblCDSPAL','tblMMSPAM');
    }
    else if (reporType.toLowerCase() == "pressuresurveydata")
    {
        ExportTabTabularSearchResults('tblPressureSurvey','tblReservoir');
    }
    else if (reporType.toLowerCase() == "functionality usage report")
    {
       FURExportToExcel('tblSearchResults');
    }
    else
    {
      ExportToExcelNew();
    }
   }
   else if(fileType == "csv")
   {
     /// Call Export All functionality
     if(SEARCHNAMEFORUBIWRKSHEET.indexOf(reporType.toLowerCase()) != -1)
    {
        ///ExportUBIToWorkSheet();
        ExportToExcelNew();
    }
    else if (reporType.toLowerCase() == "edmreport")
    {
      ExportEDMSearchResults('tblSearchResults');
    }
    else if(reporType.toLowerCase() == "mechanicaldata")
    {
    ExportMDSearchResults('tblHole','tblCasings','tblLiners','tblMechanicalcontent','tblFluidscements','tblGrossperforations','tblWellhead');
    }
    else if(reporType.toLowerCase() == "welltestreport")
    {
    ExportWellTestSearchResults('tblgeneraltestdata','tbltestanalysisdata','tbltestformationdata','tbltestflowdata','tbltestintervaldata');
    }
     else if (reporType.toLowerCase() == "paleomarkers")
    {
        ExportTabTabularSearchResults('tblCDSPAL','tblMMSPAM');
    }
    else if (reporType.toLowerCase() == "pressuresurveydata")
    {
        ExportTabTabularSearchResults('tblPressureSurvey','tblReservoir');
    }
    else if (reporType.toLowerCase() == "functionality usage report")
    {
       FURExportToExcel('tblSearchResults');
    }
    else
    {
      ExportToCSV();
    }
   }
      hide('divExportOptions');
      return false;
} /// Check Export All option is selected
else if (document.getElementById(GetObjectID('rdblExportSelectionAll', 'input')).checked == true)
{
/// Call Export All functionality
 if (reporType.toLowerCase() == "paleomarkers")
    {
        ExportTabTabularSearchResults('tblCDSPAL','tblMMSPAM');
    }
    else if (reporType.toLowerCase() == "pressuresurveydata")
    {
        ExportTabTabularSearchResults('tblPressureSurvey','tblReservoir');
    }
    else
    {
      ExportToExcelAll('/Pages/ExportToExcel.aspx',reporType);
    }

hide('divExportOptions');
  return false;
}
return false;
}

/***************************************************************
function to hide the given control based on id.
***************************************************************/
function hide(controlId) 
{
    document.getElementById(controlId).style.display='none';
    //document.getElementById('tblExportOptions').style.display = 'none';
    return false;
}


/***************************************************************
function to show  the given control based on id.
***************************************************************/
function pop(controlId)
{

    var e = window.event;
    var x = e.clientX;
    var y = e.clientY;
    document.getElementById(controlId).style.display = 'block';
   //  document.getElementById('tblExportOptions').style.display = 'block';
  // document.getElementById(controlId).style.left = 500;
    //document.getElementById(controlId).style.top = 250;
    document.getElementById(controlId).style.left = x - 300;
    document.getElementById(controlId).style.top = y
    return false;
}

/***************************************************************
function to set the default values of Export Options choices.
***************************************************************/
function SetExportOptionDefaults()
{

if (GetHTMLObject(window,'rdblExportFormatExcel', 'input').checked == false)
{
GetHTMLObject(window,'rdblExportFormatExcel', 'input').checked = true;
}

if(GetHTMLObject(window,'rdblExportFormatCSV', 'input').checked == true)
{
 GetHTMLObject(window,'rdblExportFormatCSV', 'input').checked = false;
}

if (GetHTMLObject(window,'rdblExportSelectionCurrentPage', 'input').checked == false)
{
GetHTMLObject(window,'rdblExportSelectionCurrentPage', 'input').checked = true;
}

if(GetHTMLObject(window,'rdblExportSelectionAll', 'input').checked == true)
{
 GetHTMLObject(window,'rdblExportSelectionAll', 'input').checked = false;
}
return false;
}

/********************************************************
function to set the selected file type to hidden field
**********************************************************/
function SetExportFileType()
{
var fileType = "excel";
/// Set the file type selected to Hidden field
if(GetHTMLObject(window,'rdblExportFormatCSV', 'input').checked == true)
{
 GetHTMLObject(window,'hidFileType', 'input').value = "csv";
 fileType = "csv";
}
else if (GetHTMLObject(window,'rdblExportFormatExcel', 'input').checked == true)
{
GetHTMLObject(window,'hidFileType', 'input').value = "excel";
fileType = "excel";
}
else
{ /// Default file type will be Excel.
GetHTMLObject(window,'hidFileType', 'input').value = "excel";
fileType = "excel";
}
return fileType;
}

/***************************************************************
function to validate the selection of Sync lists.
***************************************************************/
function ValidateSyncListOptions(controlId)
{
   var validSelection = false;
   validSelection = ValidateCheckBoxListSelection(controlId);
   if(validSelection)
   {
     return true;
   }
  else
  {
     alert('Please select at least one list to be synchronised');
     return false;
  }
   alert('Please select at least one list to be synchronised');
  return false;
}

/*******************************************************************
Function to validate the selection of Check box list options.
*******************************************************************/
function ValidateCheckBoxListSelection(controlId)
{
var chkblAssetTypeList = document.getElementById(controlId);
var  validSelection =false;

   if(chkblAssetTypeList != null)
   {
     var assets = chkblAssetTypeList.getElementsByTagName("input");
     
      for(var intCounter = 0; intCounter < assets.length; intCounter++ )
      {
         if(assets[intCounter].checked == true)
            {
                validSelection =true;
                /// Atleast one option is selected in Checkboxlist
                break;
            }
      }           
   }
return validSelection;
}

/********************************************************
function to create the Comma Separated Text for Export Page
**********************************************************/
function CreateCSVText(headerRow,dataRows)
{
var arrColTags = $("table#tblSearchResults col");
var rowIndex = 1;
var colStartIndex = 1;
var tempdata = "";
var rowCSV = [dataRows.length + 1];
var columnHeading = "";
var finalText = "";
for (var outerCounter = 0; outerCounter < dataRows.length; outerCounter++)
{   
 
    var column = dataRows[outerCounter].cells;
    if(column[1].innerHTML.indexOf("INPUT") >= 0)//added for report contaning chart to aviod check box row to be exported
    {
        colStartIndex = 2;
    }
    else
    {
        colStartIndex = 1;
    }
    var colIndex =1;
    for (var innerCounter = colStartIndex; innerCounter < column.length; innerCounter++)
    {
        if((arrColTags[innerCounter].className == "hide")||(headerRow[0].cells[innerCounter].innerHTML.indexOf("CHECKED")<0))
        {
            continue;
        }
        if (rowIndex == 1)
        {
           /// Setting the header cells.           
                 if(innerCounter !=  column.length -1)
                 {    
                    columnHeading += headerRow[1].cells[innerCounter].innerText.Trim() +",";
                 }
                else
                {
                columnHeading += headerRow[1].cells[innerCounter].innerText.Trim() +"\n";     
               
                }                                   
        }

        
            
                 if(innerCounter != column.length -1)
                 {    
                    if(column[innerCounter].innerText.indexOf(',') != -1)
                    { /// If the column value contains "," replace with space
                   
                    tempdata += column[innerCounter].innerText.replace(/,/gi," ") +",";
                    }
                    else
                    {
                      tempdata += column[innerCounter].innerText +",";
                    }
                 }
                 
                else
                { /// If the last column add a line break
              
                   if(column[innerCounter].innerText.indexOf(',') != -1)
                    {/// If the column value contains "," replace with space
                  
                   tempdata += column[innerCounter].innerText.replace(/,/gi," ") +"\n";
                    }
                    else
                    {
                      tempdata += column[innerCounter].innerText +"\n";
                    }
                }
                 colIndex++;
        } // column for loop ends       
   rowIndex++;                                        
    }// datarows for loop ends here
    finalText = columnHeading + tempdata;
  
return finalText;
}

/** *****************************************
Export to CSV javascript function
**************************************************/
function ExportToCSV()
{

    var tblMain = document.getElementById("tblSearchResults");

    if(tblMain == null)
    {
        return;
    }
     var arrCheckedRows = $("table#tblSearchResults tbody tr:has(input:checked)");

    if(arrCheckedRows.length>0)
    {
      var arrCheckedColumns = $("table#tblSearchResults thead tr th:has(input:checked)");
      if((arrCheckedColumns.length <= 0)||(arrCheckedColumns[0].parentNode.rowIndex !=0))
       {  
            alert('Please select minimum one column.');
            return;
       }
    }
    else
    {
         alert('Please select minimum one record.');
         return;
    }

    alert('This option will only export the records displayed on the current page. If you have multiple pages you will have to export each page separately.'); 
    document.body.style.cursor = 'wait';
    String.prototype.Trim = function () 
    {
       return this.replace(/\|*\s*$/, "");
    }
    try
    {
       
      
        var arrTblMainRows = tblMain.rows;
          var csvText = CreateCSVText(arrTblMainRows,arrCheckedRows);
alert('Please close the popup after records are exported.');        

/// Set the Comman Separated Text into a hidden field.
/// Open the .aspx page and change the content type and show the CSV file.
/// content type changing and pushing the file is done at server side code.
           $("body").append('<form style="width:400px;height:300px;" id="exportform" action="/_layouts/DREAM/CSVExport.aspx" method="post" target="_blank"><input type="hidden" id="exportdata" name="exportdata" /></form>');       
           $("#exportdata").val(csvText);       
           $("#exportform").submit().remove();       
           
    }
    catch(ex)
    {
      //  alert('Either excel is not installed or Your browser security setting is not allowing to create excel object.');
        alert(ex.message);
    }
    document.body.style.cursor = 'auto';
}

function DeleteStatus(message)
{
  alert(message);
}
/***************************************************************
    Function to set External site pane object instance to a variable
    This method may be used later for capturing onload event for external site pages
***************************************************************/
function OnRadSplitterLoadedExternalSite(sender)
{
    try
    {
       //  if(sender)
          //  sender.getEndPane().set_contentUrl(GetQueryString("externalurl"));
    }
    catch(ex)
    {
        alert(ex.message);
    }
}
/***************************************************************
    Function to get query string from url
***************************************************************/
function GetQueryString(name) 
{
    var strSubString = window.location.search.substring(1);
    var arrQueryString = strSubString.split("&");
    for (index=0; index<arrQueryString.length; index++) 
    {
        strQueryString = arrQueryString[index].split("=");
        if (strQueryString[0] == name) 
        {
            return strQueryString[1];
        }
    }
}
/***************************************************************
            function to Call Validate DateService
***************************************************************/
function CallValidateDateService(txtStartDateId,txtEndDateId)
{
    var strStartDate = document.getElementById(GetObjectID(txtStartDateId, "input")).value
    var strEndDate = document.getElementById(GetObjectID(txtEndDateId, "input")).value
    if(strStartDate == "" || strEndDate == "")
    {
        alert("Select both Start date and End date.");
        return false;
    } 
    //parameter name should be same as webparameter name   Ex startDate,its case sensitive
	var objResponse = ExecuteSynchronously('/_vti_bin/DREAM/DateTimeConvertorService.asmx', 'ValidateDate', { startDate: strStartDate, endDate: strEndDate });
	var result ;
	if(objResponse.d)
	    result =objResponse.d;//response object contains a proerty called 'd' which contians the data
	else
	    return false;
    if((result.length > 0) && (result[0] == "Invalid Date format"))
    {
        if((result.length > 1) && (result[1] != ""))
            alert("Please enter the date in " + result[1] + " format.");
        else
            alert("Please enter the date in proper format."); 
        return false;
    }
    else if((result.length > 0) && (result[0] == "Invalid Date"))
    {
        alert("Please enter a valid date.");
        return false;
    }
    strStartDate = result[0]
    strEndDate = result[1]
    
    var dtToday = new Date();
    var dtStart = GetDateObject(strStartDate, "-");
    var dtEnd= GetDateObject(strEndDate, "-");		 
	if(dtStart > dtEnd)
    {
        alert("Start date cannot be greater than End date.");
        return false;
    }
    else if(dtEnd > dtToday)
    {
        alert("End date cannot be greater than today's date.");
        return false;
    }
    else if(dtStart > dtToday)
    {
        alert("Start date cannot be greater than today's date.");
        return false;
    }
    else
    {
        return(ValidateDate(strStartDate) && ValidateDate(strEndDate))
    }
}
/***************************************************************
            function to Valid Date through webmethod
***************************************************************/
function CallIsValidDateService(strDate)
{

//parameter name should be same as webparameter name   Ex startDate,its case sensitive
	var objResponse = ExecuteSynchronously('/_vti_bin/DREAM/DateTimeConvertorService.asmx', 'IsValidDate', { strDate: strDate});
	var result =objResponse.d;//response object contains a proerty called 'd' which contians the data
	return result;
}
/***************************************************************
            function to Execute ajax request Synchronously
***************************************************************/
function ExecuteSynchronously(url, method, args)
{
    var executor = new Sys.Net.XMLHttpSyncExecutor();

    // Instantiate a WebRequest.
    var request = new Sys.Net.WebRequest();

    // Set the request URL.
    request.set_url(url + '/' + method);

    // Set the request verb.
    request.set_httpVerb('POST');

    // Set request header.
    request.get_headers()['Content-Type'] = 'application/json; charset=utf-8';

    // Set the executor.

    request.set_executor(executor);

    // Serialize argumente into a JSON string.

    request.set_body(Sys.Serialization.JavaScriptSerializer.serialize(args));

    // Execute the request.

    request.invoke();
    if (executor.get_responseAvailable())
    {
        return (executor.get_object());
    }
    return (false);
}
/***************************************************************
            function to Change BusyBox Source html
***************************************************************/
function ChangeBusyBoxSource()
{
    var objFrame = document.getElementById("BusyBoxIFrame");
    if(objFrame != null)
    {
        objFrame.src="/_layouts/dream/AjaxBusybox.htm";
    } 
}
/***************************************************************
            function to Check Uncheck All Column
***************************************************************/
function CheckUncheckAllColumn(checkUncheckBx,tableId)
{
    var arrChkBx = null;
    if(checkUncheckBx.checked)
        arrChkBx = $("table#" + tableId + " tbody tr:gt(0):has(input:enabled):has(input:not(:checked)) input");
    else
        arrChkBx = $("table#" + tableId + " tbody tr:gt(0):has(input:enabled):has(input::checked) input");
    for(index = 0; index < arrChkBx.length ; index++)
    {
        arrChkBx[index].checked = checkUncheckBx.checked;
    }    
}
/***************************************************************
            function to On Reorder ChkBx Check Uncheck
***************************************************************/
function OnReorderChkBxCheckUncheck(objChkBx,tableId)
{
    if(objChkBx.disabled)
        return;
    var chkBxCheckuncheckAll = document.getElementById("chkbxCheckUncheckAll");
    if(objChkBx.checked)
    {
        var arrChkBx = $("table#" + tableId + " tbody tr:gt(0):has(input:enabled):has(input:not(:checked)) input");
        if(arrChkBx.length == 0)
            chkBxCheckuncheckAll.checked = true;
    }
    else
        chkBxCheckuncheckAll.checked = false;
}
/***************************************************************
            function to WellSummary FeetMetre Conversion
***************************************************************/
function WellSummaryFeetMetreConversion(unit)
{
    var objTable = document.getElementById('tblLastHUD');
    var value = "";
    if(objTable == null)
        return;
    var objCells = objTable.rows[1].cells;
    if(unit == 'Feet')
    {
        objCells[0].innerText = objCells[0].innerText.replace('(m)', '(ft)')
        value = objCells[1].innerText
        if(!isNaN(value))
            objCells[1].innerText = (value * 3.28084).toFixed(2);
    }
    else if(unit == 'Metres')
    {
        objCells[0].innerText = objCells[0].innerText.replace('(ft)', '(m)')
        value = objCells[1].innerText
        if(!isNaN(value))
            objCells[1].innerText = (value / 3.28084).toFixed(2);
    }
}
/***************************************************************
            function to WellSummary Temperature Unit Convertion
***************************************************************/
function WellSummaryTemperatureUnitConvertor(objSelectedRadioBtn)
{
    var arrTableId = new Array();
    arrTableId.push("tblLastValidWellTestFTH");
    arrTableId.push("tblLastValidWellTestBH");
    var arrCellIdex = new Array();
    arrCellIdex.push(3);
    arrCellIdex.push(5);
    var strSelectedUnit =  GetRdoBtnLstSelectedItem('rdoTemperatureUnit').value;
    for(var index=0;index<arrTableId.length;index++)
    {
        var table = document.getElementById(arrTableId[index]);
        var headerRowCells = table.rows[0].cells;
        var dataRowCells = table.rows[2].cells;
        if(table == null)
            continue;
        if(headerRowCells[arrCellIdex[index]].innerText.indexOf('degC') >= 0)
        {
            headerRowCells[arrCellIdex[index]].innerHTML = headerRowCells[arrCellIdex[index]].innerHTML.replace('degC','degF');
        }
        else if(headerRowCells[arrCellIdex[index]].innerText.indexOf('degF') >= 0)
        {
            headerRowCells[arrCellIdex[index]].innerHTML = headerRowCells[arrCellIdex[index]].innerHTML.replace('degF','degC');
        }
        dataRowCells[arrCellIdex[index]].innerText = ConvertorToTemperatureUnit(dataRowCells[arrCellIdex[index]].innerText,strSelectedUnit);
    }
}
/***************************************************************
            function to WellSummary Pressure Unit Conversion
***************************************************************/
function WellSummaryPressureUnitConvertor(objSelectedRadioBtn)
{
    var arrTableId = new Array();
    arrTableId.push("tblLastValidWellTestFTH");
    arrTableId.push("tblLastValidWellTestBH");
    var arrCellIdex = new Array();
    arrCellIdex.push(2);
    arrCellIdex.push(4);
    var objSelectedItem = GetRdoBtnLstSelectedItem('rdoPressureUnit');
    var strCurrentUnit = objSelectedItem.parentNode.previousSelectedValue;//radiobutton list top span object
    var strSelectedUnit = objSelectedItem.value;
    for(var index=0;index<arrTableId.length;index++)
    {
        var table = document.getElementById(arrTableId[index]);
        var headerRowCells = table.rows[0].cells;
        var dataRowCells = table.rows[2].cells;
        if(table == null)
            continue;
        if(headerRowCells[arrCellIdex[index]].innerText.indexOf('barA') >= 0)
        {
            headerRowCells[arrCellIdex[index]].innerHTML =  headerRowCells[arrCellIdex[index]].innerHTML.replace('barA',strSelectedUnit);
        }
        else if( headerRowCells[arrCellIdex[index]].innerText.indexOf('kPa') >= 0)
        {
            headerRowCells[arrCellIdex[index]].innerHTML =  headerRowCells[arrCellIdex[index]].innerHTML.replace('kPa',strSelectedUnit);
        }
        else if( headerRowCells[arrCellIdex[index]].innerText.indexOf('psiA') >= 0)
        {
            headerRowCells[arrCellIdex[index]].innerHTML = headerRowCells[arrCellIdex[index]].innerHTML.replace('psiA',strSelectedUnit);
        }
        dataRowCells[arrCellIdex[index]].innerText = ConvertorToPressureUnit(dataRowCells[arrCellIdex[index]].innerText,strCurrentUnit,strSelectedUnit);
    }
    //setting current selected unit to table property 'previousSelectedValue'
	objSelectedRadioBtn.parentNode.previousSelectedValue = objSelectedRadioBtn.value;
}
/***************************************************************
            function to Significant well events
***************************************************************/
function MakeBold(chbBox,chbBoxSpan)
{
    var chbSpan = document.getElementById(GetObjectID(chbBoxSpan,'span'));
    if(chbBox.checked)
    {
        chbSpan.className="makebold";
    }
    else
    {
        chbSpan.className="makenormal";
    }
    return;
}
/***************************************************************
            function to Significant well events
***************************************************************/
//for maintaining check box selection 
function SelectExportFilteredEventsCheckBox(chboxName)
{
    var objchBox = document.getElementById(GetObjectID(chboxName, "input"));
    var objchbHigh = document.getElementById(GetObjectID('chbHigh', "input"));
    var objchbMedium = document.getElementById(GetObjectID('chbMedium', "input"));
    var objchbLow = document.getElementById(GetObjectID('chbLow', "input"));
    var objchbPrioritySpan = document.getElementById(GetObjectID('spnPriority','span'));

    if(objchbHigh.checked || objchbMedium.checked || objchbLow.checked)
    {
        objchBox.checked=true;
        objchbPrioritySpan.className="makebold";
    }
    else
    {
        objchBox.checked=false;
        objchbPrioritySpan.className="makenormal";
    }
}
/***************************************************************
            function to Significant well events
***************************************************************/
function SelectRadCheckBox(chboxName,chbBoxSpan)
{
    var objchBox = document.getElementById(GetObjectID(chboxName, "input"));
    var chbSpan = document.getElementById(GetObjectID(chbBoxSpan,'span'));
    objchBox.checked=true;
    chbSpan.className="makebold";
}
/***************************************************************
            function to Significant well events
***************************************************************/
function CheckFilterValidation()
{
    var objEventGroup = document.getElementById(GetObjectID('cblEventsGroup', "select"));
    var rblEventGroup =  document.getElementById(GetObjectID('cboEventGroup', "input"));
    var objOwnedBy = document.getElementById(GetObjectID('txtOWNEDBY', "input"));
    var rblOwnedBy =  document.getElementById(GetObjectID('cboOwnedBy', "input"));
    var objUpdatedBy = document.getElementById(GetObjectID('txtUPDATEDBY', "input"));
    var rblUpdatedBy =  document.getElementById(GetObjectID('cboUpdatedBy', "input"));
    var objCreatedBy = document.getElementById(GetObjectID('txtCREATEDBY', "input"));
    var rblCreatedBy =  document.getElementById(GetObjectID('cboCreatedBy', "input"));
    var objcblEventsType = document.getElementById(GetObjectID('cblEventsType', "select"));
    var rblEventType =  document.getElementById(GetObjectID('cboEventType', "input"));
    var cboEventPriority =  document.getElementById(GetObjectID('cboEventPriority', "input"));
    var cboHigh =  document.getElementById(GetObjectID('chbHigh', "input"));
    var cboMedium =  document.getElementById(GetObjectID('chbMedium', "input"));
    var cboLow =  document.getElementById(GetObjectID('chbLow', "input"));
    var blnFilter = false;
    var blnFilter2 = false;
    if(rblEventGroup.checked)
    {
        blnFilter=true;
        if(objEventGroup.value=='---Select---')
        {
            alert('Please select a filter value for Event Group.');
            return false;
        }
    }
    if(rblEventType.checked)
    {
        blnFilter=true;
        if(objcblEventsType.value=='---Select---')
        {
            alert('Please select a filter value for Event Type.');
            return false;
        }
    }
    if(rblCreatedBy.checked)
    {
        blnFilter=true;
        if(objCreatedBy.value=='')
        {
            alert('Please select a filter value for Created By.');
            return false;
        }
    }
    if(rblUpdatedBy.checked)
    {
        blnFilter=true;
        if(objUpdatedBy.value=='')
        {
            alert('Please select a filter value for Updated By.');
            return false;
        }
    }
    if(rblOwnedBy.checked)
    {
        blnFilter=true;
        if(objOwnedBy.value=='')
        {
            alert('Please select a filter value for Owner.');
            return false;
        }
    }
}
/***************************************************************
            function to Significant well events
***************************************************************/
function MakeBoldCheckBoxSelected()
{
    for (intCounter=0; intCounter < document.forms[0].elements.length; intCounter++)
    {        
        if (document.forms[0].elements[intCounter].type == 'checkbox')
        {
            if(document.forms[0].elements[intCounter].checked)
            {
                document.forms[0].elements[intCounter].parentElement.className="makebold";
            }
        }
    }
}
/***************************************************************
            function to Significant well events
***************************************************************/
function HideExpandFilter(expImage,divFilter)
 {     
     if(document.getElementById(divFilter) != null)
     {
	 if(document.getElementById(divFilter).style.display == 'block')
	 	{
	 		document.getElementById(divFilter).style.display = 'none';
	 		document.getElementById(expImage).src='/_layouts/DREAM/Images/Plus.gif';
	 	}
	 	else
	 	{
	 		document.getElementById(divFilter).style.display = 'block';
	 		document.getElementById(expImage).src='/_layouts/DREAM/Images/Minus.gif';
	 	}
	 }
 }
 /***************************************************************
            function to Significant well events
***************************************************************/
function FixColWidth(tblID)
{
    var table = document.getElementById(GetObjectID(tblID,"TABLE"));	
    var cTR = table.rows;     
    var tr = table.rows[0];
    //collection of rows
    tr.cells[0].style.width = "1%";
    tr.cells[1].style.width = "8%";
    tr.cells[2].style.width = "24%";
    tr.cells[3].style.width = "1%";
    tr.cells[4].style.width = "15%";
    tr.cells[5].style.width = "45%";
} 
 /***************************************************************
            function to Significant well events
***************************************************************/
function CarriageReturnHandling(colName,tblId)
{
    var tblMain = document.getElementById(tblId);
    if(tblMain != null)
    {
        if(tblMain.rows.length>1)
        {
            var colReqd = 0;
            var colReqdFound = false;
            for(var outerCounter=0;outerCounter<tblMain.rows[0].cells.length;outerCounter++)
            {
                if(tblMain.rows[0].cells[outerCounter].innerText==colName)
                {
                    colReqdFound=true;
                    colReqd=outerCounter;
                    break;
                }
            }
            if(colReqdFound)
            {
                for(var innerCounter=1;innerCounter<tblMain.rows.length;innerCounter++)
                {
                    if(tblMain.rows[innerCounter].cells[colReqd] != null)
                    {
                        tblMain.rows[innerCounter].cells[colReqd].innerText=tblMain.rows[innerCounter].cells[colReqd].innerText.replace(/&#xD;/gi,"\n").replace(/&nbsp;/gi," ");
                    }
                }
            }
        }
    }
}
/***************************************************************
            function to Significant well events
***************************************************************/
function ShowDateWithTime(objDateImg)
{
    var objTab = document.getElementById('tblSearchResults');
    var intCounter;
    if(objDateImg.nameProp == "plus.gif")
    {
        for(intCounter=1;intCounter<objTab.rows.length;intCounter++)
        {
            try
            {
                if(objTab.rows[intCounter].cells[1].children[0] != null)
                    objTab.rows[intCounter].cells[1].children[0].style.display = "block";
                if(objTab.rows[intCounter].cells[1].children[1] != null)
                    objTab.rows[intCounter].cells[1].children[1].style.display = "none";
            }
            catch(ex)
            {
                continue;
            }
        }
        objDateImg.src = "/_layouts/DREAM/images/minus.gif";
    }
    else
    {
        for(intCounter=1;intCounter<objTab.rows.length;intCounter++)
        {
            try
            {
                if(objTab.rows[intCounter].cells[1].children[0] != null)
                    objTab.rows[intCounter].cells[1].children[0].style.display = "none";
                if(objTab.rows[intCounter].cells[1].children[1] != null)
                    objTab.rows[intCounter].cells[1].children[1].style.display = "block";
            }
            catch(ex)
            {
                continue;
            }
        }
        objDateImg.src = "/_layouts/DREAM/images/plus.gif";
    }
}

var gsExpDiv;
/***************************************************************
            Function to hide expand dataowner menu
**************************************************************/
function HideExpandDataOwnerMenu(divOrder)
 {     
     if(document.getElementById("detailDiv_" + divOrder) != null)
     {
	 if(document.getElementById("detailDiv_" + divOrder).style.display == 'block')
	 	{
	 	    key = document.getElementById("rec_img_" + divOrder);
	 		document.getElementById("detailDiv_" + divOrder).style.display = 'none';
	 		key.src='/_layouts/DREAM/Images/Plus.gif';
	 		gsExpDiv=null;
	 	}
	 	else
	 	{
	 	 if(gsExpDiv != null)
          { 
          key=document.getElementById("rec_img_" + gsExpDiv);
          document.getElementById("detailDiv_"+gsExpDiv).style.display = 'none';
	      key.src='/_layouts/DREAM/Images/Plus.gif';
          }
          
            key = document.getElementById("rec_img_" + divOrder);
	 		document.getElementById("detailDiv_" + divOrder).style.display = 'block';
	 		key.src='/_layouts/DREAM/Images/Minus.gif';
	 		gsExpDiv=divOrder;
	 	}
	 	gsDetDiv = document.getElementById("detailDiv_" + divOrder);
	 }
	 
 }
 var objPopWinViewEDMInfo = null;
/***************************************************************
            Function to View Edm Info in SWED reports
**************************************************************/
function ViewEdmInfo(eventid)
{
    var iWidth = 700;
    var iHeight = 600;
    var hdnSwedUrl =  document.getElementById(GetObjectID('hdnSwedUrl', "input"));
    var ileft = parseInt((screen.availWidth/2) - (iWidth/2));
    var itop = parseInt((screen.availHeight/2) - (iHeight/2));
    var strUrl = hdnSwedUrl.value + "/Pages/ViewEDMInfo.aspx?eventid=";
    var sWindowFeatures = "width=" + iWidth + ",height=" + iHeight + ",status=yes,resizable=yes,left=" + ileft + ",top=" + itop + "screenX=" + ileft + ",screenY=" + itop;

    if(objPopWinViewEDMInfo != null)
    {
        try
        {
            objPopWinViewEDMInfo.location = strUrl + eventid;
        }
        catch(ex)
        {
            objPopWinViewEDMInfo = window.open(strUrl + eventid,"EDMInfo", sWindowFeatures); 
        }
    }
    else
    {
        objPopWinViewEDMInfo = window.open(strUrl + eventid,"EDMInfo", sWindowFeatures); 
    }
    objPopWinViewEDMInfo.focus();
}
/***************************************************************
            Function to open live link url in SWED reports
**************************************************************/
function OpenLiveLink(instanceNum,docNum)
{
    window.open("http://knowledge.europe.shell.com/GetDoc?instance=" + instanceNum + "&documentnumber=" + docNum);
}

function CloseWindow()
{
    window.close();
    return true;
}  
/***************************************************************
     Function declaration for ajax implementation start
***************************************************************/
/************************************************************
        function to truncate the junk characters in the ID
************************************************************/
 function ApplyReorder()
 { 
    var tblShowHideColOption = document.getElementById('tblShowHideColOption');
    var tableDnD = new TableDnD();
    tableDnD.init(tblShowHideColOption);
 }
 /************************************************************
        function to truncate the junk characters in the ID
************************************************************/
 function HandleAjaxException(args)
 {
    if (args.get_error() != undefined)
    {
        var errorMessage = args.get_error().message;
        args.set_errorHandled(true);
        alert(errorMessage);
    }
 }
 /************************************************************
        function to GetRadSplitter
************************************************************/
 function GetRadSplitter()
 {
   return $find(GetClientID(window, "splitterMain", "div"));
 }
 /************************************************************
        function to GetContentWindow
************************************************************/
function GetContentWindow()
{
  if(Splitter == null)
    return null;
    
  var iframe = Splitter.getEndPane().getExtContentElement();
  var contentWindow = iframe.contentWindow;
  return  contentWindow
}
/************************************************************
        function to GetContentWindowObject
************************************************************/
function GetContentWindowObject(objectName, TagName)
{
    var contentWindow = GetContentWindow();
    return contentWindow.document.getElementById(GetContentWindowObjectID(objectName,TagName));
}

/************************************************************
        function to GetContentWindowObjectID
************************************************************/
function GetContentWindowObjectID(objectName, TagName)
{
    var contentWindow =GetContentWindow();
    var objectId = "";
    var blnIdFound =false;
    var arrHTMLElements = contentWindow.document.documentElement.getElementsByTagName(TagName);
    for(index = 0; index < arrHTMLElements.length; index++)
    {
        objectId = arrHTMLElements.item(index).id;
        var tempId = objectId.substr(objectId.length - objectName.length);
        if(objectName == tempId )
        {
            blnIdFound =true;
            break;
        }
    }
    if(!blnIdFound)
    {
       objectId = "";
    }
    return objectId;
} 
/************************************************************
        function to GetHTMLObject
************************************************************/
function GetHTMLObject(objWindow, objectName, TagName)
{
    if(objWindow == null)
        objWindow = window;
        
    return objWindow.document.getElementById(GetClientID(objWindow,objectName,TagName));
}
/************************************************************
        function to GetClientID
************************************************************/
function GetClientID(objWindow, objectName, TagName)
{
    var clientId = "";
    var blnIdFound = false;
    var arrHTMLElements = objWindow.document.documentElement.getElementsByTagName(TagName);
    for(index = 0; index < arrHTMLElements.length; index++)
    {
        clientId = arrHTMLElements.item(index).id;
        var tempId = clientId.substr(clientId.length - objectName.length);
        if(objectName == tempId )
        {
            blnIdFound =true;
            break;
        }
    }
    if(!blnIdFound)
    {
       clientId = "";
    }
    return clientId;
}
/************************************************************
        function to QuickSearchOnChange
************************************************************/
function QuickSearchOnChange()
{
    HideShowContextSrchMenu(window,'none');
    var iframe = Splitter.getEndPane().getExtContentElement();
    if(iframe != null)
    {
        Splitter.getEndPane().set_contentUrl('');
    }
    if(Splitter != null)
    {
        var  height = GetHeight()-90;
        SetSplitterHeight(Splitter,height);
    }
}

/************************************************************
        function to OpenPageInContentWindow
************************************************************/
function OpenPageInContentWindow(url)
{
    if(busyBox)
        busyBox.Show();

    DeSelectLeftNavLink();
    HideShowContextSrchMenu(window,'none');
    if(Splitter)
    {
        SetSplitterHeight(Splitter,(GetHeight()-90));
        Splitter.getEndPane().set_contentUrl(url);
    }
}
/************************************************************
        function to Hide Show ContextSrchMenu
************************************************************/
function HideShowContextSrchMenu(window,display)
{
    var contextDiv = window.document.getElementById('divContextSearch');
    var contextTable = window.document.getElementById('tblContextSearch');
    if(contextDiv != null)
        contextDiv.style.display = display;
    if(contextTable != null)
        contextTable.style.display = display;
}
/************************************************************
        function to OpenAdvSearchResultsPage
************************************************************/
 function OpenAdvSearchResultsPage(url,sType)
{
  var splitter = window.parent.Splitter;
  if(splitter)
    splitter.getEndPane().set_contentUrl(url);
  window.parent.__doPostBack(GetClientID(window.parent,'updtPanelLeftNaV','div'),sType);
}
var Splitter;
/************************************************************
        function to OnRadSplitterLoaded
************************************************************/
function OnRadSplitterLoaded(splitter, args)   
{  
    Splitter = GetRadSplitter();              
    SetSplitterHeight(Splitter,(GetHeight()));    
}
/************************************************************
        function to OnContent Window Load
************************************************************/
function OnContentWindowLoad()
{
    var contentWindow = GetContentWindow();
    if(contentWindow)
    {
        var tblSearchResults = contentWindow.document.getElementById('tblSearchResults');
        if(tblSearchResults != null)
        {
             HideShowContextSrchMenu(window,'block');
        }
        else
        {
            HideShowContextSrchMenu(window,'none');
        }
    }
    if(Splitter)
        SetSplitterHeight(Splitter,GetContentWindowHeight(Splitter));   
    SetLeftNavMenu(); 
    HideBusyBox(window); 
}
/************************************************************
        function to SetSplitterHeight
************************************************************/
function SetSplitterHeight(splitter,height)
{
    if(splitter != null)
        if(!isNaN(height))
            splitter.set_height(height);    
}
/************************************************************
        function to GetHeight
************************************************************/
function GetHeight()
{
    var y = 0;
    if (self.innerHeight)
    {
        y = self.innerHeight;
    }
    else if (document.documentElement && document.documentElement.clientHeight)
    {
        y = document.documentElement.clientHeight;
    }
    else if (document.body)
    {
        y = document.body.clientHeight;
    }                    
    return y;
}
/************************************************************
        function to GetContentWindowHeight
************************************************************/
function GetContentWindowHeight(splitter)
{
    var height = 600;
    var objContentWindow = null;
    if(splitter)
    {
        objContentWindow = splitter.getEndPane().getExtContentElement().contentWindow;
        height = objContentWindow.document.body.scrollHeight;
    }
    return height; 
}
/************************************************************
        function to DeSelectLeftNavLink
************************************************************/
function DeSelectLeftNavLink()
{
    var arrRows = $("table#tblLeftNavContainer tr.lvl1over");
    if(arrRows.length > 0)
    {
        for(index = 0; index < arrRows.length; index++)
        {
           arrRows[index].className = "lvl1";
        }
    }   
}
/***************************************************************
      function to RegisterPageRequestManagerEventsForContextSrch
***************************************************************/
function RegisterPageRequestManagerEventsForContextSrch() 
{
    try
    {
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandlerForContextSrch);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandlerForContextSrch);
    }
    catch(ex)
    {
        alert(ex.message);
    }
}
/***************************************************************
             BeginRequestHandlerForContextSrch
***************************************************************/
function BeginRequestHandlerForContextSrch(sender, args)
{
    try
    {
        if(busyBox)
            busyBox.Show();
    }
    catch(ex)
    {
       //alert(ex.message);
    }
}
/***************************************************************
             EndRequestHandlerForContextSrch
***************************************************************/
function EndRequestHandlerForContextSrch(sender, args)
{
    try
    { 
        if(busyBox)
            busyBox.Hide();
        HandleAjaxException(args)
    }
    catch(ex)
    {
       //alert(ex.message);
    }
}
var postBackElement;
/***************************************************************
             RegisterPageRequestManagerEventsForSrchResults
***************************************************************/
function RegisterPageRequestManagerEventsForSrchResults()
{
    try
    {
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandlerSrchResults);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandlerSrchResults);
    }
    catch(ex)
    {
       alert(ex.message);
    }
}
/***************************************************************
             BeginRequestHandlerSrchResults
***************************************************************/
function BeginRequestHandlerSrchResults(sender, args)
{
  try
    {
        postbackElement = args.get_postBackElement();
//        if(window.parent) 
//            if(window.parent.busyBox)
//                window.parent.busyBox.Show();
   if(window.parent) 
            if(window.parent.busyBox)
                if(postbackElement.redirect == 'true')
                    window.parent.searchBusyBox.Show();
                else
                    window.parent.busyBox.Show();
    }
    catch(ex)
    {
        alert(ex.message);
    }
}
/***************************************************************
             EndRequestHandlerSrchResults
***************************************************************/
function EndRequestHandlerSrchResults(sender, args)
{
    try
    { 
        var tblSearchResults = document.getElementById('tblSearchResults');
        if(tblSearchResults != null)
        {
            HideShowContextSrchMenu(window.parent,'block');
        }
        else
        {
            HideShowContextSrchMenu(window.parent,'none');
        }
        if(postbackElement != null)    
            if(postbackElement.resizeWindow == 'true')
            {
                if(parent != null)
                    if(parent.Splitter != null)
                        SetSplitterHeight(parent.Splitter,document.body.scrollHeight);
                var divRadTreeView = document.getElementById('divRadTreeView');
                if(divRadTreeView != null)
                    divRadTreeView.style.height = document.body.scrollHeight-180;
            }
        if(postbackElement) 
            if(postbackElement.redirect != 'true')
                if(window.parent)
                    if(window.parent.busyBox)
                        HideBusyBox(window.parent);
        HandleAjaxException(args)
    }
    catch(ex)
    {
       alert(ex.message);
    }
} 
/***************************************************************
             RegisterPageRequestManagerEventsForLeftNav
***************************************************************/
function RegisterPageRequestManagerEventsForLeftNav()
{
    try
    {
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandlerLeftNav);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandlerLeftNav);
    }
    catch(ex)
    {
        alert(ex.message);
    }
}
/***************************************************************
             BeginRequestHandlerLeftNav
***************************************************************/
function BeginRequestHandlerLeftNav(sender, args)
{
  try
    {
        postbackElement = args.get_postBackElement(); 
        // if(busyBox)
        //   busyBox.Show();
        if(postbackElement != null)
            if(postbackElement.id.indexOf('cboQuickAsset')>=0)
                busyBox.Show();
            else if(postbackElement.id.indexOf('cmdGo')>=0)
                searchBusyBox.Show();    
    }
    catch(ex)
    {
       alert(ex.message);
    }
}
/***************************************************************
             EndRequestHandlerLeftNav
***************************************************************/
function EndRequestHandlerLeftNav(sender, args)
{
    try
    { 
        if(postbackElement != null)
            if(postbackElement.id.indexOf('cboQuickAsset')>=0)
               HideBusyBox(window);
       
        if((postbackElement != null) && (postbackElement.id.indexOf('cboQuickAsset')<0))
        {  
            var contentWindow = GetContentWindow();         
            var tblSearchResults = contentWindow.document.getElementById('tblSearchResults');
            if(tblSearchResults != null)
            {
                HideShowContextSrchMenu(window,'block');
            }
            else
            {
                HideShowContextSrchMenu(window,'none');
            }
        }
        HandleAjaxException(args)
    }
    catch(ex)
    {
        alert(ex.message);
    }
} 
/***************************************************************
     End of function to Register Page Request ManagerEvents
***************************************************************/
/***************************************************************
            function to cancel ajax processing
***************************************************************/
function AjaxCancelFn()
{        
    try
    {		
	    var objcmdSearch = window.parent.document.getElementById(GetObjectIDParentWindow("cmdSearch","input"));	
	    var objcmdSaveSearch = window.parent.document.getElementById(GetObjectIDParentWindow("cmdSaveSearch","input"));
	    var objcmdReset = window.parent.document.getElementById(GetObjectIDParentWindow("cmdReset","input"));		
    		
	    if(objcmdSearch!=null)
		    objcmdSearch.disabled=false;
    		
	    if(objcmdSaveSearch!=null)
		    objcmdSaveSearch.disabled=false;
    		
	    if(objcmdReset!=null)
		    objcmdReset.disabled=false;	
		    
        window.parent.Sys.WebForms.PageRequestManager.getInstance().abortPostBack();
        alert('Postback Cancelled');  
	}
	catch(ex)
	{
	   alert(ex.message);
	}        	
}

/***************************************
    Show the Busy box.
***************************************/
function HideBusyBox(objWindow)
{
    try
    {
        if(objWindow.busyBox != 'undefined' && objWindow.busyBox != null)
            objWindow.busyBox.Hide();
    }
    catch(ex)
    {
    }
    try
    {
        if(objWindow.searchBusyBox != 'undefined' && objWindow.searchBusyBox != null)
            objWindow.searchBusyBox.Hide();
    }
    catch(ex)
    {
    }
}
/***************************************************************
     Function declaration for ajax implementation end
***************************************************************/

/***************************************************************
     Function to Reset File Search Criteria
***************************************************************/
function ResetFileSearchCriteria()
{
    var cboSearchCriteria = GetHTMLObject(window,'cboSearchCriteria', 'select');
    var fileUploader = GetHTMLObject(window,'fileUploader', 'input');
    if(cboSearchCriteria !=null)
        cboSearchCriteria.selectedIndex = 0;
    if(fileUploader !=null)    
        clearFileUpload(fileUploader);
    //calling onchnage of cboSearchCriteria to enable all textboxes.    
    cboSearchCriteria.onchange();
}
/***************************************************************
     Function to Reset File upload control
***************************************************************/
function clearFileUpload(fileField)
{
    // get the file upload parent element
    var parentNode = fileField.parentNode;
    // create new element
    var tmpForm = document.createElement("form");
    //signature of replaceChild(newElement, oldElement); 
    parentNode.replaceChild(tmpForm,fileField);
    tmpForm.appendChild(fileField);
    tmpForm.reset();
    //signature of replaceChild(newElement, oldElement); 
    parentNode.replaceChild(fileField,tmpForm);
}
/***************************************************************
     Function to Check whether number is Whole or not
***************************************************************/
function IsWholeNumber(value)
{
    var blnLegalNumber = isFinite(value);//checks weather it is a legal number
    if(blnLegalNumber)
        {
            if(value>=0)//checking number should be grater than zero i.e. positive number including zero
            {
                if(value.toString().indexOf('.')<0)// check for positive integer
                {	
                    return true;
                }
            }
        }
    return false;
}
function ValidateInterPolatedSearch()
{
    //get the from and to text box value
    var txtbxFrom = GetHTMLObject(window,'txtbxFrom', 'input');
    var  txtbxTo = GetHTMLObject(window,'txtbxTo', 'input'); 
    if( IsWholeNumber(txtbxFrom.value))
    {
        if( IsWholeNumber(txtbxTo.value))
        {
            return true;
        }
        else
        {
            alert("Please enter whole number.");
            return false;
        }
    }
    else
    {
        alert("Please enter whole number.");
        return false;
    }
}
/***************************************************************
   Function to Change Interpolated Depth Label
***************************************************************/

function ChangeInterpolatedDepthLabel(depthUnit)
{
    var lblDepthInterval = GetHTMLObject(window,'lblDepthInterval','span');
    var lblFrom = GetHTMLObject(window,'lblFrom','span');
    var lblTo = GetHTMLObject(window,'lblTo','span');
    if(depthUnit == "Feet")
    {
        if(lblDepthInterval!=null)
            lblDepthInterval.innerText="(ft)";
        lblFrom.innerText="(ft)";
        lblTo.innerText="(ft)";
    }
    else if(depthUnit == "Metres")
    {
        if(lblDepthInterval!=null)
            lblDepthInterval.innerText="(m)";
        lblFrom.innerText="(m)";
        lblTo.innerText="(m)";
    }
}
/***************************************************************
    Function to set textbox value to zero if it is empty
***************************************************************/

function SetToZeroIfEmpty(objTxtBx)
{
    if(objTxtBx.value == "")
    {
        objTxtBx.value = "0";
    }
}
/***************************************************************
   Function to Get AssetName Column
***************************************************************/
function GetAssetNameCol(window)
{
    var strAsset = GetHTMLObject(window,'hidAssetName','input').value;
    var colName = "";
    if(strAsset == "Well")
    {
        colName = "Well Name";
    }
    else if(strAsset == "Wellbore")
    {
        colName = "WellBore Name";
    }
    else if(strAsset == "Field")
    {
        colName = "Field Name";
    }
    else if(strAsset == "Basin")
    {
        colName = "Basin Name";
    }
     else if(strAsset == "Reservoir")
    {
        colName = "Reservoir Name";
    }
    else
    {
        colName = "";
    }
    return colName;
}
/***************************************************************
   Function to Set Selected Asset Names
***************************************************************/
function SetSelectedAssetNames(window)
{
    var hidSelectedAssetNames = GetHTMLObject(window,'hidSelectedAssetNames','input');
    var colName = GetTableAttribute(window,"assetColName");
    var arrCheckedRows = $("table#tblSearchResults tbody tr:has(input:checked)",window.document);
    var arrColumnIndex = $("table#tblSearchResults thead tr th:contains('"+ colName + "')",window.document);
    if(arrColumnIndex.length <= 0)
    {
        return false;
    }
    var columnIndex = arrColumnIndex[0].cellIndex;
    hidSelectedAssetNames.value = "";
    for(intIndex = 0; intIndex < arrCheckedRows.length; intIndex++)
    {
        hidSelectedAssetNames.value += arrCheckedRows[intIndex].cells[columnIndex].innerText + "|";
    }
    return true;
 }
/***************************************************************
    CheckBox On Change eventhandler for tabular results checkboxes
***************************************************************/
function CheckBoxOnChange(objCheckbox,tableId,tbodyOrThead,chkbxSelectAllId,hidSelectedValuesId)
{
    var chkbxSelectAll = GetHTMLObject(window, chkbxSelectAllId,'input');
    var hidSelectedCheckBoxes = GetHTMLObject(window, hidSelectedValuesId,'input');
    var hidSelectedAssetNames = GetHTMLObject(window,'hidSelectedAssetNames','input');
    if(objCheckbox.checked)
    {
    
        if(!ContainsExactMatch(hidSelectedCheckBoxes.value,objCheckbox.value))//unique identifier
        {
            hidSelectedCheckBoxes.value += objCheckbox.value;
        }
        if(objCheckbox.assetName && !ContainsExactMatch(hidSelectedAssetNames.value,objCheckbox.assetName))//asset name,this is only for row checkboxes
        {
             hidSelectedAssetNames.value += objCheckbox.assetName;
        }
        var arrCheckedChkbx = $("table#" + tableId + " " + tbodyOrThead + " :checkbox:visible:[id='" + objCheckbox.id + "']:not(:checked)");
        if(arrCheckedChkbx.length <=0)
            chkbxSelectAll.checked = true;
    }
    else
    {
        hidSelectedCheckBoxes.value = ReplaceExactMatch(hidSelectedCheckBoxes.value,objCheckbox.value);//unique identifier
        if(objCheckbox.assetName)//asset name
        {
             hidSelectedAssetNames.value = ReplaceExactMatch(hidSelectedAssetNames.value,objCheckbox.assetName); //this is only for row checkboxes
        }
        chkbxSelectAll.checked = false;
    }
    //For recall logs report
    //objCheckbox.assetName means its a row checkbox
    if(GetTableAttribute(window,"searchType") == "recalllogs" && objCheckbox.assetName)
        SetRecallLogIdentifiers(objCheckbox.checked);
        
}
/***************************************************************
  Select all CheckBox On Change eventhandler for tabular results checkboxes
***************************************************************/
function SelectAllCheckBoxOnChange(objSelectAllCheckbox,tableId,tbodyOrThead,chkbxId,hidSelectedValuesId)
{
    var arrChbx = $("table#" + tableId + " " + tbodyOrThead + " :checkbox:visible:[id='" + chkbxId + "']")
    var hidSelectedCheckBoxes = GetHTMLObject(window, hidSelectedValuesId,'input');
    var hidSelectedAssetNames = GetHTMLObject(window,'hidSelectedAssetNames','input');
    if(objSelectAllCheckbox.checked)
    {
        for(index = 0; index < arrChbx.length; index++)
        {
            if(!ContainsExactMatch(hidSelectedCheckBoxes.value,arrChbx[index].value))//unique identifier
            {
                hidSelectedCheckBoxes.value += arrChbx[index].value;
            }
            if(arrChbx[index].assetName && !ContainsExactMatch(hidSelectedAssetNames.value,arrChbx[index].assetName))//asset name this is only for row checkboxes
            {
                 hidSelectedAssetNames.value += arrChbx[index].assetName;
            }
            arrChbx[index].checked = true;
        }
    }
    else
    {
        for(index = 0; index < arrChbx.length; index++)
        {
            hidSelectedCheckBoxes.value = ReplaceExactMatch(hidSelectedCheckBoxes.value,arrChbx[index].value);//unique identifier
            if(arrChbx[index].assetName)
            {
                 hidSelectedAssetNames.value = ReplaceExactMatch(hidSelectedAssetNames.value,arrChbx[index].assetName);//asset name this is only for row checkboxes
            }
            arrChbx[index].checked = false;
        }
    }
    //For recall logs report
    //oobjSelectAllCheckbox.id == "chkbxRowSelectAll" means its a row level select all checkbox
    if(GetTableAttribute(window,"searchType") == "recalllogs" && objSelectAllCheckbox.id == "chkbxRowSelectAll")
        SetRecallLogIdentifiers(objSelectAllCheckbox.checked);
        
}
/***************************************************************
 Function to select "SelectAllCheckbox" on page load
***************************************************************/
function SelectSelectAllCheckBox(chkbxSelectAllId,tableId,tbodyOrThead,chkbxId)
{
    var chkbxSelectAll = GetHTMLObject(window, chkbxSelectAllId,'input');
    var arrCheckedChkbx = $("table#" + tableId + " " + tbodyOrThead + " :checkbox:visible:[id='" + chkbxId + "']:not(:checked)");
    if(arrCheckedChkbx.length <=0)
        chkbxSelectAll.checked = true;
}
/***************************************************************
 Function to Get Selected Country in quick search
***************************************************************/
function GetSelectedCountry()
{
    var objCountry = GetHTMLObject(window,"cboQuickCountry", "select");
    var strCountry = "";
    if(objCountry != null)
        strCountry = objCountry.options[objCountry.selectedIndex].value;
    return strCountry;
}
/*************************************************************
Function to set text box value to zero by default in advance search
**************************************************************/
function SetTextBoxValue(objTextBox,value)
{
    if(objTextBox.value == "")
    {
        objTextBox.value = value;
    }
}
/*************************************************************
Function to Replace Exact Match of a string
**************************************************************/
function ReplaceExactMatch(str,strToReplace)
{
    strName  = strToReplace.substr(0,strToReplace.length -1);
    strPattern1 = "\\|" + strName + "\\|";
    pattr1 = new RegExp(strPattern1);
    if(pattr1.test(str))
    {
        return(str.replace(pattr1,"|"));
    }
    strPattern2 = "^" + strName + "\\|";
    pattr2 = new RegExp(strPattern2);
    if(pattr2.test(str))
    {
        return(str.replace(pattr2,""));
    }
    return str;
}
/*************************************************************
Function to check wheather string contains Exact Match
**************************************************************/
function ContainsExactMatch(str,strToSearch)
{
    strName  = strToSearch.substr(0,strToSearch.length -1);
    strPattern1 = "\\|" + strName + "\\|";
    pattr1 = new RegExp(strPattern1);
    strPattern2 = "^" + strName + "\\|";
    pattr2 = new RegExp(strPattern2);
    return (pattr1.test(str) || pattr2.test(str))
}
/***************************************************************
            function to Set Recall Log Identifiers values to hidden field
***************************************************************/
function SetRecallLogIdentifiers(checked)
{    
    var hidUWBI = GetHTMLObject(window,'hidUWBI','input');
    var hidLogService = GetHTMLObject(window,'hidLogService','input');
    var hidLogType = GetHTMLObject(window,'hidLogType','input');
    var hidLogSource = GetHTMLObject(window,'hidLogSource','input');
    var hidLogName = GetHTMLObject(window,'hidLogName','input');
    var hidLogActivity = GetHTMLObject(window,'hidLogActivity','input');
    var hidLogrun = GetHTMLObject(window,'hidLogrun','input');
    var hidLogVersion = GetHTMLObject(window,'hidLogVersion','input');
    var hidProjectName = GetHTMLObject(window,'hidProjectName','input');

    var colIndexUWBI = GetColumnIndex("UWBI");
    var colIndexLogService = GetColumnIndex("Log Service");
    var colIndexLogType = GetColumnIndex("Log Type");
    var colIndexLogSource = GetColumnIndex("Log Source");
    var colIndexLogName = GetColumnIndex("Log Name");
    var colIndexLogActivity = GetColumnIndex("Log Activity");
    var colIndexLogrun = GetColumnIndex("Log Run");
    var colIndexLogVersion = GetColumnIndex("Log Version");
    var colIndexProjectName = GetColumnIndex("Recall Project Name");
    
    var arrCheckedRows = $("table#tblSearchResults tbody tr:has(input:checked)",window.document);
    for(intIndex = 0; intIndex < arrCheckedRows.length; intIndex++)
    {
        objCells = arrCheckedRows[intIndex].cells;
        if(checked)
        {
            if(!ContainsExactMatch(hidUWBI.value,objCells[colIndexUWBI].innerText + "|"))
                hidUWBI.value += objCells[colIndexUWBI].innerText + "|";
            if(!ContainsExactMatch(hidLogService.value,objCells[colIndexLogService].innerText + "|"))    
                hidLogService.value += objCells[colIndexLogService].innerText + "|";
            if(!ContainsExactMatch(hidLogType.value,objCells[colIndexLogType].innerText + "|"))       
                hidLogType.value += objCells[colIndexLogType].innerText + "|";
            if(!ContainsExactMatch(hidLogSource.value,objCells[colIndexLogSource].innerText + "|"))  
                hidLogSource.value += objCells[colIndexLogSource].innerText + "|";
            if(!ContainsExactMatch(hidLogName.value,objCells[colIndexLogName].innerText + "|"))    
                hidLogName.value += objCells[colIndexLogName].innerText + "|";
            if(!ContainsExactMatch(hidLogActivity.value,objCells[colIndexLogActivity].innerText + "|"))
                hidLogActivity.value += objCells[colIndexLogActivity].innerText + "|";
            if(!ContainsExactMatch(hidLogrun.value,objCells[colIndexLogrun].innerText + "|"))
                hidLogrun.value += objCells[colIndexLogrun].innerText + "|";
            if(!ContainsExactMatch(hidLogVersion.value,objCells[colIndexLogVersion].innerText + "|"))
                hidLogVersion.value += objCells[colIndexLogVersion].innerText + "|";
            if(!ContainsExactMatch(hidProjectName.value,objCells[colIndexProjectName].innerText + "|"))
                hidProjectName.value += objCells[colIndexProjectName].innerText + "|";
        }
        else
        {
             hidUWBI.value = ReplaceExactMatch(hidUWBI.value,objCells[colIndexUWBI].innerText + "|");
             hidLogService.value = ReplaceExactMatch(hidLogService.value,objCells[colIndexLogService].innerText + "|");
             hidLogType.value = ReplaceExactMatch(hidLogType.value,objCells[colIndexLogType].innerText + "|");
             hidLogSource.value = ReplaceExactMatch(hidLogSource.value,objCells[colIndexLogSource].innerText + "|");
             hidLogName.value = ReplaceExactMatch(hidLogName.value,objCells[colIndexLogName].innerText + "|");
             hidLogActivity.value = ReplaceExactMatch(hidLogActivity.value,objCells[colIndexLogActivity].innerText + "|");
             hidLogrun.value = ReplaceExactMatch(hidLogrun.value,objCells[colIndexLogrun].innerText + "|");
             hidLogVersion.value = ReplaceExactMatch(hidLogVersion.value,objCells[colIndexLogVersion].innerText + "|");
             hidProjectName.value = ReplaceExactMatch(hidProjectName.value,objCells[colIndexProjectName].innerText + "|");
        }
    }
}
/***************************************************************
            function to Get Table Attribute
***************************************************************/
function GetTableAttribute(objWindow,attributeName)
{
    var tblSearchResults = GetHTMLObject(objWindow,'tblSearchResults','table');
    var strValue = tblSearchResults.getAttribute(attributeName)
    if(strValue != null)
        return strValue;
    else
        return "";
}
/****************************************************************************
 function to Set textbox value in adv search for Geographical Search checkbox 
*****************************************************************************/
function SetGeographicalDefaultValues(objCheckbox)
{
    if(!objCheckbox.checked)
        return true;
    var txtMinLatDeg = GetHTMLObject(window,'txtMinLatDeg','input');
    var strDefaultValue = "0";
    SetTextBoxValue(txtMinLatDeg,strDefaultValue);
    var txtMinLatMin = GetHTMLObject(window,'txtMinLatMin','input');
    SetTextBoxValue(txtMinLatMin,strDefaultValue);
    var txtMinLatSec = GetHTMLObject(window,'txtMinLatSec','input');
    SetTextBoxValue(txtMinLatSec,strDefaultValue);
    var txtMinLonDeg = GetHTMLObject(window,'txtMinLonDeg','input');
    SetTextBoxValue(txtMinLonDeg,strDefaultValue);
    var txtMinLonMin = GetHTMLObject(window,'txtMinLonMin','input');
    SetTextBoxValue(txtMinLonMin,strDefaultValue);
    var txtMinLonSec = GetHTMLObject(window,'txtMinLonSec','input');
    SetTextBoxValue(txtMinLonSec,strDefaultValue);
    var txtMaxLatDeg = GetHTMLObject(window,'txtMaxLatDeg','input');
    SetTextBoxValue(txtMaxLatDeg,strDefaultValue);
    var txtMaxLatMin = GetHTMLObject(window,'txtMaxLatMin','input');
    SetTextBoxValue(txtMaxLatMin,strDefaultValue);
    var txtMaxLatSec = GetHTMLObject(window,'txtMaxLatSec','input');
    SetTextBoxValue(txtMaxLatSec,strDefaultValue);
    var txtMaxLonDeg = GetHTMLObject(window,'txtMaxLonDeg','input');
    SetTextBoxValue(txtMaxLonDeg,strDefaultValue);
    var txtMaxLonMin = GetHTMLObject(window,'txtMaxLonMin','input');
    SetTextBoxValue(txtMaxLonMin,strDefaultValue);
    var txtMaxLonSec = GetHTMLObject(window,'txtMaxLonSec','input');
    SetTextBoxValue(txtMaxLonSec,strDefaultValue);
    
}