/* 
 * *******************************************************************************
 * File Name        :   DREAMJavaScriptFunctionsRel2_1.js
 * Project Name     :   SHELL_DREAM
 * Type             :   JavaScript Function  
 * Date_Created     :   Feb 14 2008
 * Version          :   1.0 
 * *******************************************************************************
 */

/***************************************************************
             function to toggle leftnav Advanced search Menu
***************************************************************/
function showMenuTable(menuID)
{
    var divMenu = document.getElementById(menuID);
    if (divMenu.style.visibility=='visible')
    {
        divMenu.style.visibility='hidden';
        divMenu.style.display = 'none';             
        document.getElementById("divSplSearch").className = "parent";
    }
    else if (divMenu.style.visibility=='hidden')
    {
        divMenu.style.visibility='visible';
        divMenu.style.display = 'block';         
        document.getElementById("divSplSearch").className = "lvl3active1";
    }
}

/***************************************************************
             function to set Left Nav Menu
***************************************************************/
function SetLeftNavMenu()
{   
    var FIELDDEPTHURL = "ADVSEARCHLOGSBYFIELDDEPTH";
    var trLink = null;
    var contentWindow = GetContentWindow();
    var currentURL = "";
    if(contentWindow)
        currentURL = contentWindow.location.href.toUpperCase();
    var divMenu = document.getElementById("tblAdvSearchmenu");
    if(divMenu)
        {
            divMenu.style.visibility='hidden';
            divMenu.style.display = 'none';
        }
    if(currentURL.indexOf(FIELDDEPTHURL)>=0)
    {
        if(divMenu)
        {
            divMenu.style.visibility='visible';
            divMenu.style.display = 'block';
        }
        trLink = document.getElementById("logsFieldTR");
    }
   else if(currentURL.indexOf("QUERYSEARCH")>=0)
    {
        trLink = document.getElementById("querySearchTR"); 
    }
   else if(currentURL.indexOf("SAVE SEARCH")>=0)
    {
        trLink = document.getElementById("standardSearchTR"); 
    }
   else if(currentURL.indexOf("MANAGE SEARCH")>=0)
    {
        trLink = document.getElementById("manageSearchTR");            
    }
    if(trLink)
    {
        trLink.className = "lvl1over";
    }
}



/***************************************************************
       function used for opening a page in a pop up window
***************************************************************/
function OpenPopup(url,name,attributes)
{    
    if(name=="Display")
    {
        window.open(url,name,"width=500, height=100,left="+ screen.width/4 + ",top=" + screen.height/3);
    }
    else if(name=="RegionSettings")
    {
        window.open(url,name,"width=500, height=130,left="+ screen.width/4 + ",top=" + screen.height/3);
    }
    else if(name=="UserPreferences")
    {
        window.open(url,name,"width=800, height=450, left=100, top=100, screenX=100, screenY=100");
    }
     else if(name=="PVTReport")
    {
        window.open(url,name);
    }
   else if(attributes=="")
    {
        window.open(url,name);
    }
    else
    {
        window.open(url,name,attributes);
    }
}

/************************************************************
        function to get the client ID of an object in the parent window.    
************************************************************/
function GetParentObjectID(objectName, TagName)
{
    var objectId = "";
    for(index = 0; index < window.opener.document.documentElement.getElementsByTagName(TagName).length; index++)
    {
        objectId = window.opener.document.documentElement.getElementsByTagName(TagName).item(index).id;
        var tempId = objectId.substr(objectId.length - objectName.length);
        if(objectName == tempId )
        {
            break;
        }
    }
    return objectId;
}
/************************************************************
        function to truncate the junk characters in the ID
************************************************************/
function GetObjectIDParentWindow(objectName, TagName)
{
    var objectId = "";
    for(index = 0; index < window.parent.document.documentElement.getElementsByTagName(TagName).length; index++)
    {
        objectId = window.parent.document.documentElement.getElementsByTagName(TagName).item(index).id;
        var tempId = objectId.substr(objectId.length - objectName.length);
        if(objectName == tempId )
        {
            break;
        }
    }
    return objectId;
}

/***************************************************************
            function to check whether the current page is a 
            print eligible page or not..
***************************************************************/
function IsPrintEligiblePage()
{
    for (i=0; i < document.forms[0].elements.length; i++)
    {
        if (document.forms[0].elements[i].type == 'checkbox')
        {
                return true;         
        }
    }
   var currentURL = location.href;
   var arrPageName = currentURL.split("/");   
   var curPageName = arrPageName[arrPageName.length - 1];
   curPageName = curPageName.toUpperCase();
   if(curPageName.indexOf("REPORT") >=0)
    return true;
    else if(curPageName.indexOf("EPCATALOG") >=0)
    return true;
     else if(curPageName.indexOf("FUNCTIONALITYUSAGE") >=0)
    return true;
    else if(curPageName.indexOf("PROJECTARCHIVESDETAIL") >=0)
    return true;
    else if(curPageName.indexOf("WELLBOREHEADER.ASPX") >=0)
    return true;
    else if(curPageName.indexOf("WELLHISTORY.ASPX") >=0)
    return true;
    else if(curPageName.indexOf("EPCATALOG") >=0)
    return true;
    else if(curPageName.indexOf("DWB") >=0)
    return true;
    else if(curPageName.indexOf("FIELDHEADER.ASPX") >=0)
    return true;
    else if(curPageName.indexOf("RESERVOIRHEADER.ASPX") >=0)
    return true;
    else if(curPageName.indexOf("DREAMWELLSUMMARY.ASPX") >=0)
    return true;
   else
    return false;
}

/***************************************************************
            function to set the Height of the Table in the Detail Page to Auto
***************************************************************/
function setHeight(objTable)
{
    objTable.style.height = "auto";
}

/***************************************************************
       function used for Printer Friendly Version functionality
***************************************************************/
function printContent(tableName, searchName)
{       
 var objWindow;
    if((searchName == 'WellHistory') || (searchName == 'WellSummary'))
    {
        if(IsPrintEligiblePage())
        {
           var isConfirmed = confirm("This option will print the records displayed on the current page.\nIf you have multiple pages of records to print please either print one page at a time or export to Excel and print from there.")
           if(isConfirmed == true)
           {
               var attributes="toolbar=no,location=no,directories=yes,resizable,menubar=yes,";
               attributes+="scrollbars=yes,width=800, height=600, left=100, top=25";
                objWindow = window.open("/_layouts/dream/printWellHistory.htm?title=Standard DWB Page","Print",attributes);
                objWindow.opener =window.self; 
                objWindow.focus();
           }
        }
        else
        {
           alert("Print option is enabled only for search results and details page.");        
        }
    } 
    else if(searchName == 'EDMReport')
    {
        EDMprintContent(tableName);
    }  
    else if(searchName == 'MechanicalData')
    {
       MDRprintContent(tableName);
    }  
    else if((searchName == 'PaleoMarkers') ||(searchName == 'PressureSurveyData'))
    {
       MDRprintContent(tableName);
    }       
    else if(searchName == 'WellTestReport')
    {
       MDRprintContent(tableName);
    }
    else if(searchName == 'DWB')
    {
       DWBResultsRprintContent(tableName);
    }
    else
    {
       //Check whether we are in Search Results page
       var RowNumber = 3;   
       var counter = 0;
       if(IsPrintEligiblePage())
       {   		
           var isConfirmed = confirm("This option will print the records displayed on the current page.\nIf you have multiple pages of records to print please either print one page at a time or export to Excel and print from there.")
           if(isConfirmed == true)
           {
	            for (i=0; i < document.forms[0].elements.length; i++) 
	            {   
		            if (document.forms[0].elements[i].type == 'checkbox')
		            {
			            counter++;				
		            }
		            if(counter>0)
			            break;
	            }
               var attributes="toolbar=no,location=no,directories=yes,resizable,menubar=yes,";
               attributes+="scrollbars=yes,width=800, height=600, left=100, top=25";
               
               if(searchName == 'PreProdRFT')
               {
                     objWindow = window.open("/_layouts/dream/print.htm?title=Standard DWB Page","Print",attributes);
                     objWindow.opener =window.self; 
                     objWindow.focus();
               }
               else
               {
                     objWindow = window.open("/_layouts/dream/print.htm?title=Data Retrieval EPICURE Application","Print",attributes);
                     objWindow.opener =window.self; 
                     objWindow.focus();
               }
           }
        }
        else
        {
           alert("Print option is enabled only for search results and details page.");        
        }
    } 
}

/*******************************************************************
       function used for hiding the check box row in printer page
********************************************************************/
function hideCheckBoxRow(tblID)
{	    
	var table = document.getElementById(tblID);	
	var cTR = table.getElementsByTagName('TR');  //collection of rows	
	for (i = 0; i < cTR.length; i++)
		{		
			var tr = cTR.item(i);
			if(tr.cells[0].innerHTML.indexOf("checkbox") >= 0)
			{
				tr.cells[0].style.display = 'none';			
			}
		}
}   

/***************************************************************
       function used for viewing the page in separate Window
***************************************************************/
function ViewinSeperateWindow()
{
   var attributes="toolbar=yes,location=no,directories=yes,resizable,"; 
   attributes+="scrollbars=yes,width=800, height=600, left=100, top=25"; 
   var printText = document.getElementById("MSO_ContentTable").innerHTML;
  
   var docprint=window.open("","",attributes); 
   docprint.document.open(); 
   docprint.document.write('<html><head>'); 
   docprint.document.write(document.getElementById("ctl00_Head1").innerHTML);
   docprint.document.write('</head><body onLoad="javascript:_spBodyOnLoadWrapper();">');
         
   docprint.document.write(document.body.innerHTML);                 
   docprint.document.write('</body></html>'); 
   docprint.document.close(); 
   docprint.focus(); 
}

/************************************************************
        function to check whether the input is a blank space         
************************************************************/

function isBlankspace(s)
{   
    var blankSpace = " \t\n\r";
    for (var i = 0; i < s.length; i++)
    {        
        var c = s.charAt(i);
        if (blankSpace.indexOf(c) == -1) return false;
    }    
    return true;
}

/************************************************************
        function to validate only if it contains numbers
************************************************************/
function isNumeric(intNumber)
{
   return (( intNumber >="0") && (intNumber <= "9")); 
}

/************************************************************
        function to validate Quick Search UI on leftnav
************************************************************/
function ValidateQuickUI()
{  
    var objText =  GetHTMLObject(window,"txtSearchCriteria", "input");            
    var objCountry = GetHTMLObject(window,"cboQuickCountry", "select");
    var objAsset = GetHTMLObject(window,"cboQuickAsset", "select");
    var objColumn = GetHTMLObject(window,"cboQuickColumn", "select");
    var PARSITEMVAL = "Project Archives";
    if(isBlankspace(objText.value))
    {
        alert("Please enter the search criteria.");
        objText.value = "";
        objText.focus();        
        return false;
    }
    else
    {  
       //3.1 requirement change for appending wildcard
       //start
       if(objText.value.indexOf("*")<0)
        {
            objText.value= objText.value + "*";
        }
       //end
        var Text = objText.value;
        var Asset = objAsset.options[objAsset.selectedIndex].value;
        if(Asset.toString().toLowerCase() == "document")
        {
            PVTContextSearchPopup(window,'/pages/EPCatalog.aspx',encodeURIComponent(Text),Asset)
            return false;
        }
       else
       {
            var Country = objCountry.options[objCountry.selectedIndex].value;
            var Column = "";
            if(objAsset.options[objAsset.selectedIndex].text == PARSITEMVAL)
                if(objCountry.selectedIndex > 0)
                    Country = objCountry.options[objCountry.selectedIndex].text;    
             //Dream 4.0 changes start   
            if(objColumn)
                Column = objColumn.options[objColumn.selectedIndex].value;
            var url = "/Pages/QuickSearchResults.aspx?country=" + Country + "&asset=" + Asset + "&column="+ Column +"&criteria="+ encodeURIComponent(Text);
            OpenPageInContentWindow(url);
            //Dream 4.0 changes end
        }
        return true;              
    }       	
}
/***************************************************************
       function used for validating Access Request Page.
***************************************************************/
 function Validate()
    {        
        var strMessage = "";
        if(CheckIsNull('txtUserAcc','input')) strMessage = strMessage + "- Shell user account \n";
        if(CheckIsNull('txtRegion','input') == true)  strMessage = strMessage + "- Shell region \n";
        if(CheckIsNull('txtPurpose','textarea') == true) strMessage = strMessage + "- Purpose \n";
        
        if(strMessage != "")
        {
            strMessage = "The following field(s) are missing or invalid: \n" + strMessage;
            alert(strMessage);
            return false;
        }
        return true;     
    }
    
/***************************************************************
       function used to trim an input string
***************************************************************/
function TrimString(strParam) 
{
    var temp = strParam;
    return temp.replace(/^\strParam+/,'').replace(/\strParam+$/,'');
}  

/***************************************************************
       function used to Check the specified field has value
***************************************************************/    
function CheckIsNull(objCtrlID,ctrlType)
{
    var strCtrlValue = TrimString(document.getElementById(GetObjectID(objCtrlID, ctrlType)).value);           
	strCtrlValue = strCtrlValue.replace(/\|*\s*$/, "");
	if(strCtrlValue=="")
    {
        document.getElementById(GetObjectID(objCtrlID, ctrlType)).value = "";
        return true;
    }
    else
    {
        return false;
    }
}    

/***************************************************************
       function used to reset all the fields
***************************************************************/    
function ResetFields()
{
    document.getElementById(GetObjectID('txtUserAcc', 'input')).value = "";
    document.getElementById(GetObjectID('txtRegion', 'input')).value = "";
    document.getElementById(GetObjectID('txtPurpose', 'textarea')).value = "";
    return false;
}    
 
/**********************************************************************************
       function used to Check whether the given control has numeric value
**********************************************************************************/  
function IsNumeric(objCtrlID)
{        
    var strValidChars = "0123456789.-";
    var strChar;
    var blnResult = true;
    
    var strCtrlValue = document.getElementById(GetObjectID(objCtrlID, 'input')).value;
    if (strCtrlValue.length == 0) return false;
    
    //check strCtrlValue consists of valid characters listed above
    for (i = 0; i < strCtrlValue.length && blnResult == true; i++)
    {
        strChar = strCtrlValue.charAt(i);
        if (strValidChars.indexOf(strChar) == -1)
        {
            blnResult = false;
        }
    }
    return blnResult;
}


/****************************************************************
     function to hide/display Links table in User Preferences
*****************************************************************/
function HideDisplayLinks(objTable, idImage)  
{    
    if(document.getElementById(objTable).style.display == 'none') 
    {
        document.getElementById(objTable).style.display = "";
        document.getElementById(idImage).src = "/_LAYOUTS/DREAM/images/info_off.gif";
    }
    else 
    {
        document.getElementById(objTable).style.display = "none";
        document.getElementById(idImage).src = "/_LAYOUTS/DREAM/images/info_on.gif";
    }
}

/****************************************************************
       function to validate the user preferences ui
*****************************************************************/
function ValidateUserPreferences()
{       
    if(IsValidURL('txtLinkUrl1') == false)
    {        
        return false;
    }
    if(IsValidURL('txtLinkUrl2') == false)
    {
        return false;
    }
    if(IsValidURL('txtLinkUrl3') == false)
    {
        return false;
    }
    if(IsValidURL('txtLinkUrl4') == false)
    {
        return false;
    }
    return true;
}

/****************************************************************
       function to reset all controls in user preferences
*****************************************************************/
function ResetUserPreferences()
{    
    for(var index = 0; index < document.forms[0].elements.length; index++)
    {
        var element = document.forms[0].elements[index];
        if(element.type == "text")
        {
            element.value = "";
        }            
        else if(element.type == "select-one")
        {
            element.selectedIndex = 0;
        }
    }
    return false;
}

/****************************************************************
       function to validate URL field has proper value
*****************************************************************/
function IsValidURL(objCtrlUrl) 
{ 
    var strUrl = TrimFieldValue(document.getElementById(GetObjectID(objCtrlUrl, 'input')).value)
    var reg = new RegExp(); 
    reg.compile("((https?|ftp|gopher|telnet|file|notes|ms-help):((//)|(\\\\))?[\w\d:#@%/;$()~_?\+-=\\\.&]*)");
    if(strUrl != "")
    {
        if (!reg.test(strUrl)) 
        {               
            alert("The URL should contain http:// or https:// prefixed.\n Enter a valid URL.");   
            return false; 
        }
    }
    return true;
}

/****************************************************************
       function to trim a user input from textbox
*****************************************************************/
function TrimFieldValue(strValue)
{
    var tempValue = strValue;
    return tempValue.replace(/^\strValue+/,'').replace(/\strValue+$/,'');    
} 

var blank = "";
/************************************************************
        function to make an html table visible true
************************************************************/
function resetDateTable()
{
    if((document.getElementById(GetObjectID('rdoRbDate_0', 'input')).checked == true) ||
        (document.getElementById(GetObjectID('rdoRbDate_1', 'input')).checked == true) ||
        (document.getElementById(GetObjectID('txtFrom', 'input')).value != "") ||
        (document.getElementById(GetObjectID('txtTo', 'input')).value != ""))
        {
            document.getElementById(GetObjectID('rdoRbDate_0', 'input')).checked = false;
            document.getElementById(GetObjectID('rdoRbDate_1', 'input')).checked = false;
            document.getElementById(GetObjectID('txtFrom', 'input')).value = "";
            document.getElementById(GetObjectID('txtTo', 'input')).value = "";
       }
}
/************************************************************
        function to reset date criteria for well wellbore advance search
************************************************************/
function resetWellWellboreDateTable()
{  
    var trDates =document.getElementById(GetObjectID('trDates', 'tr'));
    var previousHeight = trDates.parentNode.parentNode.scrollHeight;//Table scroll height after expanding
    document.getElementById(GetObjectID('rdoRbDate_0', 'input')).checked = false;
    document.getElementById(GetObjectID('rdoRbDate_1', 'input')).checked = false;
    document.getElementById(GetObjectID('txtFrom', 'input')).value = "";
    document.getElementById(GetObjectID('txtTo', 'input')).value = "";
    trDates.style.display = 'none';

    //Setting the splitter height during expand collapse
    //sart
    // debugger;
    var currentHeight = trDates.parentNode.parentNode.scrollHeight;//Table scroll height after expanding
    if(previousHeight > currentHeight)//while collapsing
    {
        SetSplitterHeight(window.parent.Splitter,document.body.scrollHeight +(currentHeight - previousHeight)); 
    }
    //end 
}

/************************************************************
        function to make an html table visible true
************************************************************/
function resetPARSDateTable()
{
    if (document.getElementById(GetObjectID('rbSelectDates', 'input')).checked)
    {
        EnableDisableDates('rbSelectDates');
    }
    document.getElementById(GetObjectID('rbLastWeek', 'input')).checked = false;
    document.getElementById(GetObjectID('rbSelectDates', 'input')).checked = false;
    document.getElementById(GetObjectID('rbLastMonth', 'input')).checked = false;
    document.getElementById(GetObjectID('rbLastYear', 'input')).checked = false;
    document.getElementById(GetObjectID('txtStartDate', 'input')).value = "";
    document.getElementById(GetObjectID('txtEndDate', 'input')).value = "";    
}
/************************************************************
        function to make an html table visible true
************************************************************/
function showPARSDateTable()
{     
    try
    {	
	    var objTable = document.getElementById(GetObjectID(theTable,'table'));
	}
	catch (E)
	{
	    var objTable = document.getElementById(GetObjectID(theTable,'TD'));
	}
	
	if ((objTable.style.display == 'none') && (document.getElementById(GetObjectID('chbDates', 'input')).checked == true))
    {
        objTable.style.display = 'block';
        if ((document.getElementById(GetObjectID('rbLastWeek', 'input')).checked == false) && (document.getElementById(GetObjectID('rbLastMonth', 'input')).checked == false) &&
            (document.getElementById(GetObjectID('rbLastYear', 'input')).checked == false) && (document.getElementById(GetObjectID('rbSelectDates', 'input')).checked == false))
        {
            document.getElementById(GetObjectID('rbLastWeek', 'input')).checked = true;
        }
    }
    else
    {
        if((document.getElementById(GetObjectID('rbLastMonth', 'input')).checked == true) ||
            (document.getElementById(GetObjectID('rbLastYear', 'input')).checked == true) ||
            (document.getElementById(GetObjectID('rbSelectDates', 'input')).checked == true) ||
            (document.getElementById(GetObjectID('txtStartDate', 'input')).value != "") ||
            (document.getElementById(GetObjectID('txtEndDate', 'input')).value != ""))
        {
            objTable.style.display = 'none';
            document.getElementById(GetObjectID('rbLastWeek', 'input')).checked = false;
            document.getElementById(GetObjectID('rbSelectDates', 'input')).checked = false;
            document.getElementById(GetObjectID('rbLastMonth', 'input')).checked = false;
            document.getElementById(GetObjectID('rbLastYear', 'input')).checked = false;
            document.getElementById(GetObjectID('txtStartDate', 'input')).value = "";
            document.getElementById(GetObjectID('txtEndDate', 'input')).value = "";
        }
        else
        {
            objTable.style.display = 'none';
            document.getElementById(GetObjectID('rbLastWeek', 'input')).checked = false;
        }       
    }	
}
  //Dream 4.0 changes start
/************************************************************
        function to make an html table visible true
************************************************************/
function showLatLongTable(theTable1,theTable2,theTable3,theTable4,theTable5,theTable6,theTable7,theTable8,assetType)
{    
        var previousHeight = null;
        var currentHeight = null;
    try
    {	
	    var objTable1 = document.getElementById(GetObjectID(theTable1,'TR'));
	    var objTable2 = document.getElementById(GetObjectID(theTable2,'TR'));
	    var objTable3 = document.getElementById(GetObjectID(theTable3,'TR'));
	    var objTable4 = document.getElementById(GetObjectID(theTable4,'TR'));
	    var objTable5 = document.getElementById(GetObjectID(theTable5,'TR'));
	    var objTable6 = document.getElementById(GetObjectID(theTable6,'TR'));
	    var objTable7 = document.getElementById(GetObjectID(theTable7,'TR'));
	    var objTable8 = document.getElementById(GetObjectID(theTable8,'TR'));
	    previousHeight = objTable1.parentNode.parentNode.scrollHeight;//Table scroll height before expanding
	    
	}
	catch (E)
	{	    
	}
	if ((objTable1.style.display == 'none') && (document.getElementById(GetObjectID('chbGeographicalSearch', 'input')).checked))
    {
            objTable1.style.display = 'block';
            
            if(assetType =="Wellbore")
            {
              objTable2.style.display = 'block';
            }
            else
            {
             objTable2.style.display = 'none';
            }
            objTable3.style.display = 'block';
            objTable4.style.display = 'block';
            objTable5.style.display = 'block';
            objTable6.style.display = 'block';
            objTable7.style.display = 'block';
            objTable8.style.display = 'block';
            document.getElementById(GetObjectID('rdoLatLong', 'input')).checked = true;
    }
        else
        {
            if((document.getElementById(GetObjectID('rdoRbLatLon_0', 'input')).checked == true) ||
                (document.getElementById(GetObjectID('rdoRbLatLon_1', 'input')).checked == true) ||
                (document.getElementById(GetObjectID('txtMinLatDeg', 'input')).value != "") ||
                (document.getElementById(GetObjectID('txtMinLatMin', 'input')).value != "") ||
                (document.getElementById(GetObjectID('txtMinLatSec', 'input')).value != "") ||
                (document.getElementById(GetObjectID('txtMinLonDeg', 'input')).value != "") ||
                (document.getElementById(GetObjectID('txtMinLonMin', 'input')).value != "") ||
                (document.getElementById(GetObjectID('txtMinLonSec', 'input')).value != "") ||
                (document.getElementById(GetObjectID('txtMaxLatDeg', 'input')).value != "") ||
                (document.getElementById(GetObjectID('txtMaxLatMin', 'input')).value != "") ||
                (document.getElementById(GetObjectID('txtMaxLatSec', 'input')).value != "") ||
                (document.getElementById(GetObjectID('txtMaxLonDeg', 'input')).value != "") ||
                (document.getElementById(GetObjectID('txtMaxLonMin', 'input')).value != "") ||
                (document.getElementById(GetObjectID('txtMaxLonSec', 'input')).value != "")||
                (document.getElementById(GetObjectID('txtMaxLonEW', 'input')).value != "")||
                (document.getElementById(GetObjectID('txtMinLonEW', 'input')).value != "")||
                (document.getElementById(GetObjectID('txtMinLatNS', 'input')).value != "")||
                (document.getElementById(GetObjectID('txtMaxLatNS', 'input')).value != ""))
                {
                        document.getElementById(GetObjectID('rdoLatLong', 'input')).checked = false;
                        document.getElementById(GetObjectID('rdoRbLatLon_0', 'input')).checked = false;
                        document.getElementById(GetObjectID('rdoRbLatLon_1', 'input')).checked = false;
                        document.getElementById(GetObjectID('txtMinLatDeg', 'input')).value = "";
                        document.getElementById(GetObjectID('txtMinLatMin', 'input')).value = "";
                        document.getElementById(GetObjectID('txtMinLatSec', 'input')).value = "";
                        document.getElementById(GetObjectID('txtMinLonDeg', 'input')).value = "";
                        document.getElementById(GetObjectID('txtMinLonMin', 'input')).value = "";
                        document.getElementById(GetObjectID('txtMinLonSec', 'input')).value = "";
                        document.getElementById(GetObjectID('txtMaxLatDeg', 'input')).value = "";
                        document.getElementById(GetObjectID('txtMaxLatMin', 'input')).value = "";
                        document.getElementById(GetObjectID('txtMaxLatSec', 'input')).value = "";
                        document.getElementById(GetObjectID('txtMaxLonDeg', 'input')).value = "";
                        document.getElementById(GetObjectID('txtMaxLonMin', 'input')).value = "";
                        document.getElementById(GetObjectID('txtMaxLonSec', 'input')).value = "";
                        document.getElementById(GetObjectID('txtMaxLonEW', 'input')).value = "";
                        document.getElementById(GetObjectID('txtMinLonEW', 'input')).value = "";
                        document.getElementById(GetObjectID('txtMinLatNS', 'input')).value = "";
                        document.getElementById(GetObjectID('txtMaxLatNS', 'input')).value = "";
                        objTable1.style.display = 'none';
                        objTable2.style.display = 'none';
                        objTable3.style.display = 'none';
                        objTable4.style.display = 'none';
                        objTable5.style.display = 'none';
                        objTable6.style.display = 'none';
                        objTable7.style.display = 'none';
                        objTable8.style.display = 'none';
            }
            else
            {
                document.getElementById(GetObjectID('rdoLatLong', 'input')).checked = false;
                objTable1.style.display = 'none';
                objTable2.style.display = 'none';
                objTable3.style.display = 'none';
                objTable4.style.display = 'none';
                objTable5.style.display = 'none';
                objTable6.style.display = 'none';
                objTable7.style.display = 'none';
                objTable8.style.display = 'none';
            }
        }  
        //Setting the splitter height during expand collapse
        //sart
       // debugger;
       currentHeight = objTable1.parentNode.parentNode.scrollHeight;//Table scroll height after expanding
       if(currentHeight > previousHeight)//while expanding decrease height by 180
       {
            currentHeight = currentHeight -180;
       }
       SetSplitterHeight(window.parent.Splitter,document.body.scrollHeight +(currentHeight - previousHeight));    
        //end 
    }
      //Dream 4.0 changes end
/************************************************************
        function to make an html table visible true
************************************************************/
function showPARSLatLongTable(Tr1,Tr2,Tr3,Tr4,Tr5,Tr6)
{     
    var previousHeight = null;
    var currentHeight = null;  
    try
    {	
	    var objRow1 = document.getElementById(GetObjectID(Tr1,'TR'));
	    var objRow2 = document.getElementById(GetObjectID(Tr2,'TR'));
	    var objRow3 = document.getElementById(GetObjectID(Tr3,'TR'));
	    var objRow4 = document.getElementById(GetObjectID(Tr4,'TR'));
	    var objRow5 = document.getElementById(GetObjectID(Tr5,'TR'));
	    var objRow6 = document.getElementById(GetObjectID(Tr6,'TR'));
	    previousHeight = objRow1.parentNode.parentNode.scrollHeight;//Table scroll height before expanding
	}
	catch (E)
	{	    
	}
	
	if ((objRow1.style.display == 'none') && (objRow6.style.display == 'none')
	 && (objRow2.style.display == 'none') && (objRow3.style.display == 'none')
	 && (objRow4.style.display == 'none') && (objRow5.style.display == 'none')
	 && (document.getElementById(GetObjectID('chbGeographicalSearch', 'input')).checked == true))
    {
        objRow1.style.display = 'block';
        objRow2.style.display = 'block';
        objRow3.style.display = 'block';
        objRow4.style.display = 'block';
        objRow5.style.display = 'block';
        objRow6.style.display = 'block';
    }
    else
    {
        if( (document.getElementById(GetObjectID('txtMinLatDeg', 'input')).value != "") ||
            (document.getElementById(GetObjectID('txtMinLatMin', 'input')).value != "") ||
            (document.getElementById(GetObjectID('txtMinLatSec', 'input')).value != "") ||
            (document.getElementById(GetObjectID('txtMinLonDeg', 'input')).value != "") ||
            (document.getElementById(GetObjectID('txtMinLonMin', 'input')).value != "") ||
            (document.getElementById(GetObjectID('txtMinLonSec', 'input')).value != "") ||
            (document.getElementById(GetObjectID('txtMaxLatDeg', 'input')).value != "") ||
            (document.getElementById(GetObjectID('txtMaxLatMin', 'input')).value != "") ||
            (document.getElementById(GetObjectID('txtMaxLatSec', 'input')).value != "") ||
            (document.getElementById(GetObjectID('txtMaxLonDeg', 'input')).value != "") ||
            (document.getElementById(GetObjectID('txtMaxLonMin', 'input')).value != "") ||
            (document.getElementById(GetObjectID('txtMaxLonSec', 'input')).value != "")||
            (document.getElementById(GetObjectID('txtMaxLonEW', 'input')).value != "")||
            (document.getElementById(GetObjectID('txtMinLonEW', 'input')).value != "")||
                (document.getElementById(GetObjectID('txtMinLatNS', 'input')).value != "")||
                (document.getElementById(GetObjectID('txtMaxLatNS', 'input')).value != ""))
            {       
                    objRow1.style.display = 'none';
                    objRow2.style.display = 'none';
                    objRow3.style.display = 'none';
                    objRow4.style.display = 'none';
                    objRow5.style.display = 'none';
                    objRow6.style.display = 'none';
                    
                    document.getElementById(GetObjectID('txtMinLatDeg', 'input')).value = "";
                    document.getElementById(GetObjectID('txtMinLatMin', 'input')).value = "";
                    document.getElementById(GetObjectID('txtMinLatSec', 'input')).value = "";
                    document.getElementById(GetObjectID('txtMinLonDeg', 'input')).value = "";
                    document.getElementById(GetObjectID('txtMinLonMin', 'input')).value = "";
                    document.getElementById(GetObjectID('txtMinLonSec', 'input')).value = "";
                    document.getElementById(GetObjectID('txtMaxLatDeg', 'input')).value = "";
                    document.getElementById(GetObjectID('txtMaxLatMin', 'input')).value = "";
                    document.getElementById(GetObjectID('txtMaxLatSec', 'input')).value = "";
                    document.getElementById(GetObjectID('txtMaxLonDeg', 'input')).value = "";
                    document.getElementById(GetObjectID('txtMaxLonMin', 'input')).value = "";
                    document.getElementById(GetObjectID('txtMaxLonSec', 'input')).value = "";
                    document.getElementById(GetObjectID('txtMaxLonEW', 'input')).value = "";
                    document.getElementById(GetObjectID('txtMinLonEW', 'input')).value = "";
                    document.getElementById(GetObjectID('txtMinLatNS', 'input')).value = ""
                    document.getElementById(GetObjectID('txtMaxLatNS', 'input')).value = ""
                    
                }                
        else
        {   
            objRow1.style.display = 'none';
            objRow2.style.display = 'none';
            objRow3.style.display = 'none';
            objRow4.style.display = 'none';
            objRow5.style.display = 'none';
            objRow6.style.display = 'none';
        }
    }
    //Setting the splitter height during expand collapse
    //sart
    // debugger;
    currentHeight = objRow1.parentNode.parentNode.scrollHeight;//Table scroll height after expanding
    if(currentHeight > previousHeight)//while expanding decrease height by 180
    {
        currentHeight = currentHeight -150;
    }
    SetSplitterHeight(window.parent.Splitter,document.body.scrollHeight +(currentHeight - previousHeight));    
    //end 
}
/************************************************************
        function to hide an html table
************************************************************/
function hideTable(TR1,TR2,TR3,TR4,TR5,TR6,TR7,TR8)
{	
	var objTable = document.getElementById(GetObjectID(TR1,'TR'));	
	objTable.style.display = 'none';
	var objTable = document.getElementById(GetObjectID(TR2,'TR'));	
	objTable.style.display = 'none';
	var objTable = document.getElementById(GetObjectID(TR3,'TR'));	
	objTable.style.display = 'none';
	var objTable = document.getElementById(GetObjectID(TR4,'TR'));	
	objTable.style.display = 'none';
	var objTable = document.getElementById(GetObjectID(TR5,'TR'));	
	objTable.style.display = 'none';
	var objTable = document.getElementById(GetObjectID(TR6,'TR'));	
	objTable.style.display = 'none';
	var objTable = document.getElementById(GetObjectID(TR7,'TR'));	
	objTable.style.display = 'none';
	var objTable = document.getElementById(GetObjectID(TR8,'TR'));	
	objTable.style.display = 'none';
}

/************************************************************
        function to validate well/wellbore advanced search ui        
************************************************************/
function ValidateWellboreSearch(isSaveSearchClicked,assetType)
{    
        if(CheckAllControlsWellbore(isSaveSearchClicked,assetType))
        {
            var objUWBIText = GetHTMLObject(window,'txtUnique_Well_Identifier', 'input');            
             var objLeaseIdentiferText =  GetHTMLObject(window,'txtLease_Identifier', 'input'); 
             //Dream 3.1 fix
            if(!ValidateWellListboxSelection())
            {
                return false;
            }
            if(ValidateWellboreDates())
            {   
  
                if(GetHTMLObject(window,'rdoLatLong', 'input').checked == true)
                {
//                    if(GetHTMLObject(window,'rdoRbLatLon_0','input').checked ||
//                                GetHTMLObject(window,'rdoRbLatLon_1','input').checked ||(assetType =="Well"))
//                    {
                        if (!NoLatLongBlankText())
                        {                    
                            if(ValidateLatLongValues() == true)
                            {
                                return true;
                            }                
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            alert("Please enter all Latitude and Longitude values.");
                            return false;
                        }
//                    }
//                    else
//                    {
//                        alert('Please select surface or bottom.');
//                        return false;
//                    }
                }
//                else if(document.getElementById(GetObjectID('rdoCRS', 'input')).checked == true)
//                {
//                    if(ValidateCRSValues() == true)
//                    {
//                        return true;
//                    }
//                    else
//                    {
//                        return false;
//                    }
//                }
                else if ((objLeaseIdentiferText!=null)&&(!isSplCharacter(objLeaseIdentiferText.value)))
                {
                    alert("Plese enter valid Lease Identifier value.");
                    objLeaseIdentiferText.value = "";
                    objLeaseIdentiferText.focus();
                    return false;
                }
                else
                {   	
                    return true;
                }
            }            
        }  
        return false;    
}

/************************************************************
        function to validate the search name has value in 
        wellbore advanced search ui on [Save Search] click        
************************************************************/
function ValidateWellboreSaveSearch(assetType)
{
        if(ValidateWellboreSearch(true,assetType))
        {
		    EnableButton();
            objTextBox = document.getElementById(GetObjectID('txtSaveSearch', 'input'));
            if(isBlankspace(objTextBox.value))
            {            
                alert("Please enter search name.");
                HighlightTextError(objTextBox);            
                return false;
            }
            else if(!isSplCharacter(objTextBox.value))
            {
                alert("Invalid Search Name. Please enter only alphanumeric values and special characters like hyphen or underscore.");
                objTextBox.value = "";
                objTextBox.focus();
                 return false;
            }
            else
            {			
                return true;
            }
        }
        else
        {        
            return false;
        }
}

/************************************************************
        function to loop through all controls in wellbore ui 
        to check whether at least one item has value        
************************************************************/
function CheckAllControlsWellbore(isSaveSearchClicked,assetType)
{
var fileUp =  GetHTMLObject(window,'fileUploader', 'input').value;
 if(!isSaveSearchClicked)
    {
                            if(fileUp == '')
                            {                           

                                if(GetHTMLObject(window,'cboSearchCriteria', 'select').selectedIndex!=0)
                                {
                                 alert("Please select the file to search.");
                                 return false;
                                }
                            }
                            else
                            {

                                    if(GetHTMLObject(window,'cboSearchCriteria', 'select').selectedIndex==0)
                                        {
                                         alert("Please select the required search parameter from ‘Search By’ drop down.");
                                         return false;
                                        }
                     
                                 var extension = fileUp.substring(fileUp.lastIndexOf('.'));
                                    if(extension!='')
                                    {
                                        
                                        
                                        extension = extension.toString().toLowerCase();
                                        if(extension==".txt"||extension==".doc"||extension==".xls" || extension==".docx"||extension==".xlsx")
                                            {
                                            
                                            if(extension==".doc" || extension==".docx"){ReadWordDocument(fileUp,'hidWordContent');}
                                            if(extension==".xls" || extension==".xlsx"){ReadExcelDocument(fileUp,'hidWordContent');}
                                            return true;
                                            }
                                        else
                                        {
                                            alert('Please select a valid file format(*.txt, *.doc,*.docx, *.xls, *.xlsx).');
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                     alert('Please select a valid file format(*.txt, *.doc,*.docx, *.xls, *.xlsx).');
                                            return false;
                                    }
                            }
          }
         else
         {        
          if((GetHTMLObject(window,'cboSearchCriteria', 'select').selectedIndex!=0)||(fileUp != ''))
            {
             alert("Save search is not applicable for file based search.");
             return false;
             }
         }                     
                            
    for(var index = 0; index < document.forms[0].elements.length; index++)
    {
        var element = document.forms[0].elements[index];      
        if(element.type == "text")
        {
                if((element.id !=  GetHTMLObject(window,'txtSaveSearch', 'input').id) &&
                (element.id != GetHTMLObject(window.parent,'txtSearchCriteria', 'input').id)&&
                (element.id != GetHTMLObject(window.parent,'searchString', 'input').id))
            {
                if(!isBlankspace(element.value))
                {
                    return true;
                }
				else
				{
					element.value="";
				}
            }
        }
        else if(element.type == "select-multiple")
        {
            for(var indexSelect = 0; indexSelect < element.options.length; indexSelect++)
            {
 if((GetHTMLObject(window,'cboSavedSearch', 'select') != null && (element.id != GetHTMLObject(window,'cboSavedSearch', 'select').id))               
                && (GetHTMLObject(window.parent,'cboQuickCountry', 'select') != null && (element.id != GetHTMLObject(window.parent,'cboQuickCountry', 'select').id))
                && (GetHTMLObject(window.parent,'cboQuickAsset', 'select') != null && (element.id != GetHTMLObject(window.parent,'cboQuickAsset', 'select').id)))
                {
                    if(element.options[indexSelect].selected == true) 
                    {
                        return true;
                    }
                }
            }
        }
		else if(element.type == "radio")
		{
			if(GetHTMLObject(window,'rdoLatLong', 'input').checked==true)
				{
//				if(GetHTMLObject(window,'rdoRbLatLon_0', 'input').checked || GetHTMLObject(window,'rdoRbLatLon_1', 'input').checked ||(assetType =="Well"))
//                    {
					    if(!ValidateLatLongNoBlank()) 
			            {
			                alert("All Latitude and Longitude values must be entered.");
			                return false;
			            }
//			        }
//			        else
//			        {
//			            alert('Please select surface or bottom.');
//			            return false;
//			        }
		        }
		}
    }
       if(GetHTMLObject(window,'fileUploader', 'input').value == '')
        {
           
         alert("Please enter search criteria.");
            if(GetHTMLObject(window,'txtUnique_Wellbore_Identifier', 'input') != null)
            {           
            HighlightTextError(GetHTMLObject(window,'txtUnique_Wellbore_Identifier', 'input')); 
            }
            else if (GetHTMLObject(window,'txtUnique_Well_Identifier', 'input') != null)
            {
            HighlightTextError(GetHTMLObject(window,'txtUnique_Well_Identifier', 'input')); 
            }
            return false;        
        }
        else
        {
            return true;
        }
 return false;
}

/************************************************************
        function to erase the text in Latitude and Longitude Fields WellBoreAdvanced Search
************************************************************/
function NoLatLongBlankText()
{    
    if ((document.getElementById(GetObjectID('txtMinLatDeg', 'input')).value == blank)&&
    (dodocument.getElementById(GetObjectID('txtMinLatMin', 'input')).value == blank)&&
   (document.getElementById(GetObjectID('txtMinLatSec', 'input')).value == blank)&&
   (document.getElementById(GetObjectID('txtMinLonDeg', 'input')).value == blank)&&
   (document.getElementById(GetObjectID('txtMinLonMin', 'input')).value == blank)&&
   (document.getElementById(GetObjectID('txtMinLonSec', 'input')).value == blank)&&
   (document.getElementById(GetObjectID('txtMaxLatDeg', 'input')).value == blank)&&
   (document.getElementById(GetObjectID('txtMaxLatMin', 'input')).value == blank)&&
   (document.getElementById(GetObjectID('txtMaxLatSec', 'input')).value == blank)&&
   (document.getElementById(GetObjectID('txtMaxLonDeg', 'input')).value == blank)&&
   (document.getElementById(GetObjectID('txtMaxLonMin', 'input')).value == blank)&&
   (document.getElementById(GetObjectID('txtMaxLonSec', 'input')).value == blank)&&
   (document.getElementById(GetObjectID('txtMaxLonEW', 'input')).value == blank)&&
   (document.getElementById(GetObjectID('txtMinLonEW', 'input')).value == blank)&&
   (document.getElementById(GetObjectID('txtMinLatNS', 'input')).value == blank)&&
   (document.getElementById(GetObjectID('txtMaxLatNS', 'input')).value == blank))
    {
        return true;
    }
    else
    {
        return false;
    }  
}

  //Dream 4.0 changes start
/*********************************************************************************************
        function to validate fields related to Latitude & Longitude in wellbre advanced search
**********************************************************************************************/
function ValidateLatLongValues()
{        
        if(!ValidateLatLongBlank()) 
        {
            //true if one or more fields are also blank   
            if(!ValidateLatLongNoBlank()) 
            {
                alert("All Latitude and Longitude values must be entered.");
                return false;
            }
            else
            {
                if(!ValidateLatLongNumeric()) 
                {
                    alert("All Latitude and Longitude values must be numeric.");
                    return false;
                }
                  if(document.getElementById(GetObjectID('txtMinLatNS', 'input')).value != blank)
                {
                    if(!ValidLatitudeNS(document.getElementById(GetObjectID('txtMinLatNS', 'input'))))
                    {
                        alert("Latitude N/S must be 'N, S, n, s'.");
                        return false;
                    }
                }
                if(document.getElementById(GetObjectID('txtMaxLatNS', 'input')).value != blank)
                {
                    if(!ValidLatitudeNS(document.getElementById(GetObjectID('txtMaxLatNS', 'input'))))
                    {
                        alert("Latitude N/S must be 'N, S, n, s'.");
                        return false;
                    }
                }
                 if(document.getElementById(GetObjectID('txtMinLonEW', 'input')).value != blank)
                {
                    if(!ValidLongitudeEW(document.getElementById(GetObjectID('txtMinLonEW', 'input'))))
                    {
                        alert("Longitude E/W must be 'E, W, e, w'.");
                        return false;
                    }
                }
                if(document.getElementById(GetObjectID('txtMaxLonEW', 'input')).value != blank)
                {
                    if(!ValidLongitudeEW(document.getElementById(GetObjectID('txtMaxLonEW', 'input'))))
                    {
                        alert("Longitude E/W must be 'E, W, e, w'.");
                        return false;
                    }
                }
            }
        }
        //Added in DREAM 3.0 to avoid floating point number for Deg and Min
        //start
        //ValidateForInteger added in below if conditions
        //end   
        if(!ValidateForInteger('txtMinLatDeg','txtMaxLatDeg')||!ValidateUpperBound('txtMinLatDeg', 'txtMinLatMin', 'txtMinLatSec', 90)|| !ValidateUpperBound('txtMaxLatDeg', 'txtMaxLatMin', 'txtMaxLatSec', 90)|| !ValidIntRange('txtMinLatDeg', 'input', 0, 90) || !ValidIntRange('txtMaxLatDeg', 'input',0, 90)) 
        {
            alert("Latitude degree must be an integer in the range 0 to 90.");
            return false;
        }
       if(!ValidateForInteger('txtMinLatMin','txtMaxLatMin')|| !ValidIntRange('txtMinLatMin', 'input', 0, 59) || !ValidIntRange('txtMaxLatMin', 'input', 0, 59) )
        {
            alert("Latitude minutes must be an integer in the range 0 to 59.");
            return false;
        }
        if(!ValidFloatRange('txtMinLatSec', 'input', 0, 59.999) || !ValidFloatRange('txtMaxLatSec', 'input', 0, 59.999)) 
        {
            alert("Latitude seconds must be a real number in the range 0 to 59.999...");
            return false;
        }
        
       if(!ValidateForInteger('txtMinLonDeg','txtMaxLonDeg')|| !ValidateUpperBound('txtMinLonDeg', 'txtMinLonMin', 'txtMinLonSec', 180)|| !ValidateUpperBound('txtMaxLonDeg', 'txtMaxLonMin', 'txtMaxLonSec', 180)|| !ValidIntRange('txtMinLonDeg', 'input', 0, 180) || !ValidIntRange('txtMaxLonDeg', 'input', 0, 180)) 
        {
            alert("Longitude degree must be an integer in the range 0 to 180.");
            return false;
        }
        if(!ValidateForInteger('txtMinLonMin','txtMaxLonMin') || !ValidIntRange('txtMinLonMin', 'input', 0, 59) || !ValidIntRange('txtMaxLonMin', 'input', 0, 59)) 
        {
            alert("Longitude minutes must be an integer in the range 0 to 59.");
            return false;
        }
        if(!ValidFloatRange('txtMinLonSec', 'input', 0, 59.999) || !ValidFloatRange('txtMaxLonSec', 'input',0, 59.999)) 
        {
            alert("Longitude seconds must be a real number in the range 0 to 59.999...");
            return false;
        }

        if(!ValidateMinMaxLat())
        {
            return false;
        }
        if(!ValidateMinMaxLong())
        {
            return false;
        }
        return true;
    
}
  //Dream 4.0 changes end
/*******************************************************************************
        function to validate fields related to CRS in wellbre advanced search
********************************************************************************/
function ValidateCRSValues()
{
    if(!ValidateCRSBlank()) 
    {
        if(!ValidateCRSNoBlank()) 
        {
            alert("All CRS fields must be entered.");
            return false;
        }
    }
    if(!ValidateMinMaxCRSX())
    {
        return false;
    }
    if(!ValidateMinMaxCRSY())
    {
        return false;
    }
    return true;
}

/***********************************************************************************
        function to validate CRS fields has not blank in wellbore advanced search
*************************************************************************************/
function ValidateCRSBlank()
{
   if (document.getElementById(GetObjectID('txtMinGridX', 'input')).value != blank) HighlightTextError(document.getElementById(GetObjectID('txtMinGridX', 'input')));
   else if(document.getElementById(GetObjectID('txtMinGridY', 'input')).value != blank) HighlightTextError(document.getElementById(GetObjectID('txtMinGridY', 'input')));
   else if(document.getElementById(GetObjectID('txtMaxGridX', 'input')).value !=blank) HighlightTextError(document.getElementById(GetObjectID('txtMaxGridX', 'input')));
   else if(document.getElementById(GetObjectID('txtMaxGridY', 'input')).value != blank) HighlightTextError(document.getElementById(GetObjectID('txtMinGridY', 'input')));
   else return true;
   return false;
}

/****************************************************************************
        function to validate CRS fields has blank in wellbore advanced search
********************************************************************************/
function ValidateCRSNoBlank()
{
   if (document.getElementById(GetObjectID('txtMinGridX', 'input')).value == blank) HighlightTextError(document.getElementById(GetObjectID('txtMinGridX', 'input')));
   else if(document.getElementById(GetObjectID('txtMinGridY', 'input')).value == blank) HighlightTextError(document.getElementById(GetObjectID('txtMinGridY', 'input')));
   else if(document.getElementById(GetObjectID('txtMaxGridX', 'input')).value == blank) HighlightTextError(document.getElementById(GetObjectID('txtMaxGridX', 'input')));
   else if(document.getElementById(GetObjectID('txtMaxGridY', 'input')).value == blank) HighlightTextError(document.getElementById(GetObjectID('txtMaxGridY', 'input')));
   else return true;
   return false;
}

/************************************************************
        function to validate minimum X value in CRS search is less than 
        maximum X value in wellbore advanced search
************************************************************/
function ValidateMinMaxCRSX()
{
    var intMinX, intMaxX;

    intMinX = parseInt(document.getElementById(GetObjectID('txtMinGridX', 'input')).value,10); 
    intMaxX = parseInt(document.getElementById(GetObjectID('txtMaxGridX', 'input')).value,10);

    if(intMinX > intMaxX) 
    {
        HighlightTextError(document.getElementById(GetObjectID('txtMinGridX', 'input')));
        alert("Minimum Grid X value must be less than maximum Grid X.");
        return false;
    }
    return true;
}

/*************************************************************************
        function to validate minimum Y value in CRS search is less than 
        maximum Y value in wellbore advanced search
************************************************************/
function ValidateMinMaxCRSY()
{
    var intMinY, intMaxY;
    
    intMinY = parseInt(document.getElementById(GetObjectID('txtMinGridY', 'input')).value,10); 
    intMaxY = parseInt(document.getElementById(GetObjectID('txtMaxGridY', 'input')).value,10);

    if(intMinY > intMaxY) 
    {
        HighlightTextError(document.getElementById(GetObjectID('txtMinGridY', 'input')));
        alert("Minimum Grid Y value must be less than maximum Grid Y.");
        return false;
    }
    return true;
}

/************************************************************
        function to check a string contains a given char
************************************************************/
function InStr(strSearch, charSearchFor)
{
    for (i=0; i < strSearch.length; i++)
    {
      if (charSearchFor == Mid(strSearch, i, 1))
      {
            return i;
      }
    }
    return -1;
}

/************************************************************
        function to Highlight & focus textbox on error
************************************************************/
function HighlightTextError(textbox) 
{       
   textbox.select();
   textbox.focus();
}

/************************************************************
        function to validate latitude longitude fields are blank
************************************************************/
function ValidateLatLongBlank()
{
   if (document.getElementById(GetObjectID('txtMinLatDeg', 'input')).value != blank) HighlightTextError(document.getElementById(GetObjectID('txtMinLatDeg', 'input')));
   else if(document.getElementById(GetObjectID('txtMinLatMin', 'input')).value != blank) HighlightTextError(document.getElementById(GetObjectID('txtMinLatMin', 'input')));
   else if(document.getElementById(GetObjectID('txtMinLatSec', 'input')).value !=blank) HighlightTextError(document.getElementById(GetObjectID('txtMinLatSec', 'input')));
   else if(document.getElementById(GetObjectID('txtMinLonDeg', 'input')).value != blank) HighlightTextError(document.getElementById(GetObjectID('txtMinLonDeg', 'input')));
   else if(document.getElementById(GetObjectID('txtMinLonMin', 'input')).value != blank) HighlightTextError(document.getElementById(GetObjectID('txtMinLonMin', 'input')));
   else if(document.getElementById(GetObjectID('txtMinLonSec', 'input')).value != blank) HighlightTextError(document.getElementById(GetObjectID('txtMinLonSec', 'input')));
   else if(document.getElementById(GetObjectID('txtMinLonEW', 'input')).value != blank) HighlightTextError(document.getElementById(GetObjectID('txtMinLonEW', 'input')));
   else if(document.getElementById(GetObjectID('txtMaxLatDeg', 'input')).value != blank) HighlightTextError(document.getElementById(GetObjectID('txtMaxLatDeg', 'input')));
   else if(document.getElementById(GetObjectID('txtMaxLatMin', 'input')).value != blank) HighlightTextError(document.getElementById(GetObjectID('txtMaxLatMin', 'input')));
   else if(document.getElementById(GetObjectID('txtMaxLatSec', 'input')).value != blank) HighlightTextError(document.getElementById(GetObjectID('txtMaxLatSec', 'input')));
   else if(document.getElementById(GetObjectID('txtMaxLonDeg', 'input')).value != blank) HighlightTextError(document.getElementById(GetObjectID('txtMaxLonDeg', 'input')));
   else if(document.getElementById(GetObjectID('txtMaxLonMin', 'input')).value != blank) HighlightTextError(document.getElementById(GetObjectID('txtMaxLonMin', 'input')));
   else if(document.getElementById(GetObjectID('txtMaxLonSec', 'input')).value != blank) HighlightTextError(document.getElementById(GetObjectID('txtMaxLonSec', 'input')));
   else if(document.getElementById(GetObjectID('txtMaxLonEW', 'input')).value != blank) HighlightTextError(document.getElementById(GetObjectID('txtMaxLonEW', 'input')));
   else if(document.getElementById(GetObjectID('txtMinLatNS', 'input')).value != blank) HighlightTextError(document.getElementById(GetObjectID('txtMinLatNS', 'input')));
   else if(document.getElementById(GetObjectID('txtMaxLatNS', 'input')).value != blank) HighlightTextError(document.getElementById(GetObjectID('txtMaxLatNS', 'input')));
   else return true;
   return false;
}

/************************************************************
        function to validate latitude longitude fields are not blank
************************************************************/
function ValidateLatLongNoBlank()
{
   if (document.getElementById(GetObjectID('txtMinLatDeg', 'input')).value == blank) HighlightTextError(document.getElementById(GetObjectID('txtMinLatDeg', 'input')));
   else if(document.getElementById(GetObjectID('txtMinLatMin', 'input')).value == blank) HighlightTextError(document.getElementById(GetObjectID('txtMinLatMin', 'input')));
   else if(document.getElementById(GetObjectID('txtMinLatSec', 'input')).value == blank) HighlightTextError(document.getElementById(GetObjectID('txtMinLatSec', 'input')));
   else if(document.getElementById(GetObjectID('txtMinLonDeg', 'input')).value == blank) HighlightTextError(document.getElementById(GetObjectID('txtMinLonDeg', 'input')));
   else if(document.getElementById(GetObjectID('txtMinLonMin', 'input')).value == blank) HighlightTextError(document.getElementById(GetObjectID('txtMinLonMin', 'input')));
   else if(document.getElementById(GetObjectID('txtMinLonSec', 'input')).value == blank) HighlightTextError(document.getElementById(GetObjectID('txtMinLonSec', 'input')));
   else if(document.getElementById(GetObjectID('txtMinLonEW', 'input')).value == blank) HighlightTextError(document.getElementById(GetObjectID('txtMinLonEW', 'input')));
   else if(document.getElementById(GetObjectID('txtMaxLatDeg', 'input')).value == blank) HighlightTextError(document.getElementById(GetObjectID('txtMaxLatDeg', 'input')));
   else if(document.getElementById(GetObjectID('txtMaxLatMin', 'input')).value == blank) HighlightTextError(document.getElementById(GetObjectID('txtMaxLatMin', 'input')));
   else if(document.getElementById(GetObjectID('txtMaxLatSec', 'input')).value == blank) HighlightTextError(document.getElementById(GetObjectID('txtMaxLatSec', 'input')));
   else if(document.getElementById(GetObjectID('txtMaxLonDeg', 'input')).value == blank) HighlightTextError(document.getElementById(GetObjectID('txtMaxLonDeg', 'input')));
   else if(document.getElementById(GetObjectID('txtMaxLonMin', 'input')).value == blank) HighlightTextError(document.getElementById(GetObjectID('txtMaxLonMin', 'input')));
   else if(document.getElementById(GetObjectID('txtMaxLonSec', 'input')).value == blank) HighlightTextError(document.getElementById(GetObjectID('txtMaxLonSec', 'input')));
   else if(document.getElementById(GetObjectID('txtMaxLonEW', 'input')).value == blank) HighlightTextError(document.getElementById(GetObjectID('txtMaxLonEW', 'input')));
   else if(document.getElementById(GetObjectID('txtMinLatNS', 'input')).value == blank) HighlightTextError(document.getElementById(GetObjectID('txtMinLatNS', 'input')));
   else if(document.getElementById(GetObjectID('txtMaxLatNS', 'input')).value == blank) HighlightTextError(document.getElementById(GetObjectID('txtMaxLatNS', 'input')));
  
   else return true;
   return false;
}

/************************************************************
        function to validate latitude longitude fields are blank
************************************************************/
function ValidateLatLongNumeric()
{
   if (IsNumeric(document.getElementById(GetObjectID('txtMinLatDeg', 'input')).id) == false) HighlightTextError(document.getElementById(GetObjectID('txtMinLatDeg', 'input')));
   else if(IsNumeric(document.getElementById(GetObjectID('txtMinLatMin', 'input')).id) == false) HighlightTextError(document.getElementById(GetObjectID('txtMinLatMin', 'input')));
   else if(IsNumeric(document.getElementById(GetObjectID('txtMinLatSec', 'input')).id) == false) HighlightTextError(document.getElementById(GetObjectID('txtMinLatSec', 'input')));
   else if(IsNumeric(document.getElementById(GetObjectID('txtMinLonDeg', 'input')).id) == false) HighlightTextError(document.getElementById(GetObjectID('txtMinLonDeg', 'input')));
   else if(IsNumeric(document.getElementById(GetObjectID('txtMinLonMin', 'input')).id) == false) HighlightTextError(document.getElementById(GetObjectID('txtMinLonMin', 'input')));
   else if(IsNumeric(document.getElementById(GetObjectID('txtMinLonSec', 'input')).id) == false) HighlightTextError(document.getElementById(GetObjectID('txtMinLonSec', 'input')));
   else if(IsNumeric(document.getElementById(GetObjectID('txtMaxLatDeg', 'input')).id) == false) HighlightTextError(document.getElementById(GetObjectID('txtMaxLatDeg', 'input')));
   else if(IsNumeric(document.getElementById(GetObjectID('txtMaxLatMin', 'input')).id) == false) HighlightTextError(document.getElementById(GetObjectID('txtMaxLatMin', 'input')));
   else if(IsNumeric(document.getElementById(GetObjectID('txtMaxLatSec', 'input')).id) == false) HighlightTextError(document.getElementById(GetObjectID('txtMaxLatSec', 'input')));
   else if(IsNumeric(document.getElementById(GetObjectID('txtMaxLonDeg', 'input')).id) == false) HighlightTextError(document.getElementById(GetObjectID('txtMaxLonDeg', 'input')));
   else if(IsNumeric(document.getElementById(GetObjectID('txtMaxLonMin', 'input')).id) == false) HighlightTextError(document.getElementById(GetObjectID('txtMaxLonMin', 'input')));
   else if(IsNumeric(document.getElementById(GetObjectID('txtMaxLonSec', 'input')).id) == false) HighlightTextError(document.getElementById(GetObjectID('txtMaxLonSec', 'input')));
   else return true;
   return false;
}

/************************************************************
        function to validate the textbox integer value in specified range
************************************************************/
function ValidIntRange(textBoxId, tagName,minInt, maxInt) 
{
    var textBox = document.getElementById(GetObjectID(textBoxId, tagName));
    if (parseInt(textBox.value,10) < minInt || parseInt(textBox.value,10) > maxInt) 
    {
        HighlightTextError(textBox);
        return false;
    }
    else
    { 
        return true;
    }
}

/************************************************************
        function to validate the textbox float value in specified range
************************************************************/
function ValidFloatRange(textBoxId, tagName, minFloat, maxFloat) 
{
    var textBox = document.getElementById(GetObjectID(textBoxId, tagName));
    if (parseFloat(textBox.value) < minFloat || parseFloat(textBox.value) > maxFloat) 
    {
        HighlightTextError(textBox);
        return false;
    }
    else
    { 
        return true;
    }
}

/************************************************************
        function to validate the textbox float value in specified range
************************************************************/
function ValidLongitudeEW(textBox) 
{
   if (textBox.value == 'E' || textBox.value == 'W' || textBox.value == 'e' || textBox.value == 'w') 
   {
      return true;      
   }
   else
   {
      HighlightTextError(textBox);
      return false;
   }
}
  //Dream 4.0 changes start
/************************************************************
        function to validate the textbox float value in specified range
************************************************************/
function ValidLatitudeNS(textBox) 
{
   if (textBox.value == 'N' || textBox.value == 'S' || textBox.value == 'n' || textBox.value == 's') 
   {
      return true;      
   }
   else
   {
      HighlightTextError(textBox);
      return false;
   }
}
//Added in DREAM 3.1 
//Start
/************************************************************
        function to validate upper bound for latitude and longitude
************************************************************/
function ValidateUpperBound(txtDeg, txtMin, txtSec,maxValue)
{
	var textBoxDeg = document.getElementById(GetObjectID(txtDeg, 'input'));
	var textBoxMin = document.getElementById(GetObjectID(txtMin, 'input'));
	var textBoxSec = document.getElementById(GetObjectID(txtSec, 'input'));
	if (parseInt(textBoxDeg.value,10) == maxValue) 
    {
        if(parseInt(textBoxMin.value,10) > 0 || parseInt(textBoxSec.value,10) > 0)
        {
            HighlightTextError(textBoxDeg);
            return false;
        }
        else
        { 
            return true;
        }
    }
    else
    { 
        return true;
    }
}
/************************************************************
        function to set textbox value to zero if user enters -00
************************************************************/
function SetToZero(objTxtBx)
{
    if(parseInt(objTxtBx.value,10)==0)
    {
        objTxtBx.value = "00";
    }
}
//End
/************************************************************
        function to validate deg and sec values for integer
************************************************************/
function ValidateForInteger()
{
   var textBox = null;
   for(var index =0; index < arguments.length; index++)
   {
        textBox = document.getElementById(GetObjectID(arguments[index],'input'));
        if((textBox != null) && (!IsInteger(textBox.value)))
        {
           return false;
        }
   }
   return true;
}
//Dream 4.0 changes end
/************************************************************
        function to validate minimum latitude is not greater than maximum latitude
************************************************************/
function ValidateMinMaxLat() 
{
    var intMinLatDeg, intMinLatMin, floatMinLatSec;
    var intMaxLatDeg, intMaxLatMin, floatMaxLatSec;

    intMinLatDeg = parseInt(document.getElementById(GetObjectID('txtMinLatDeg', 'input')).value,10); 
    intMinLatMin = parseInt(document.getElementById(GetObjectID('txtMinLatMin', 'input')).value,10); 
    floatMinLatSec = parseFloat(document.getElementById(GetObjectID('txtMinLatSec', 'input')).value); 

    intMaxLatDeg = parseInt(document.getElementById(GetObjectID('txtMaxLatDeg', 'input')).value,10);
    intMaxLatMin = parseInt(document.getElementById(GetObjectID('txtMaxLatMin', 'input')).value,10);
    floatMaxLatSec = parseFloat(document.getElementById(GetObjectID('txtMaxLatSec', 'input')).value); 
//Dream 4.0 changes start
//negate latitude values if south pole is selected
    if(document.getElementById(GetObjectID('txtMinLatNS', 'input')).value.toLowerCase() == 's') 
    {
        intMinLatDeg = -intMinLatDeg;
        intMinLatMin = -intMinLatMin;
        floatMinLatSec = -floatMinLatSec;
    }
    if(document.getElementById(GetObjectID('txtMaxLatNS', 'input')).value.toLowerCase() == 's') 
    {
        intMaxLatDeg = -intMaxLatDeg;
        intMaxLatMin = -intMaxLatMin;
        floatMaxLatSec = -floatMaxLatSec;
    }
    //Dream 4.0 changes end
    if(intMinLatDeg > intMaxLatDeg) 
    {
        HighlightTextError(document.getElementById(GetObjectID('txtMinLatDeg', 'input')));
        alert("Minimum latitude must be less than maximum latitude.");
        return false;
    }
    else if(intMinLatDeg == intMaxLatDeg) 
    {
        if(intMinLatMin > intMaxLatMin) 
        {
            HighlightTextError(document.getElementById(GetObjectID('txtMinLatMin', 'input')));
            alert("Minimum latitude must be less than maximum latitude.");
            return false;
        }
        else if(intMinLatMin == intMaxLatMin) 
        {
            if(floatMinLatSec >= floatMaxLatSec) 
            {
                HighlightTextError(document.getElementById(GetObjectID('txtMinLatSec', 'input')));
                alert("Minimum latitude must be less than maximum latitude.");
                return false;
            }
        }
    }
    return true;
}

/************************************************************
        function to validate minimum longitude is not greater than maximum longitude
************************************************************/
function ValidateMinMaxLong() 
{
    var intMinLonDeg, intMinLonMin, floatMinLonSec;
    var intMaxLonDeg, intMaxLonMin, floatMaxLonSec;

    intMinLonDeg = parseInt(document.getElementById(GetObjectID('txtMinLonDeg', 'input')).value,10); 
    intMinLonMin = parseInt(document.getElementById(GetObjectID('txtMinLonMin', 'input')).value,10); 
    floatMinLonSec = parseFloat(document.getElementById(GetObjectID('txtMinLonSec', 'input')).value); 

    intMaxLonDeg = parseInt(document.getElementById(GetObjectID('txtMaxLonDeg', 'input')).value,10);
    intMaxLonMin = parseInt(document.getElementById(GetObjectID('txtMaxLonMin', 'input')).value,10);
    floatMaxLonSec = parseFloat(document.getElementById(GetObjectID('txtMaxLonSec', 'input')).value); 


    //negate longitude values if west of Greenwich
    if(document.getElementById(GetObjectID('txtMinLonEW', 'input')).value.toLowerCase() == 'w') 
    {
        intMinLonDeg = -intMinLonDeg;
        intMinLonMin = -intMinLonMin;
        floatMinLonSec = -floatMinLonSec;
    }
    if(document.getElementById(GetObjectID('txtMaxLonEW', 'input')).value.toLowerCase() == 'w') 
    {
        intMaxLonDeg = -intMaxLonDeg;
        intMaxLonMin = -intMaxLonMin;
        floatMaxLonSec = -floatMaxLonSec;
    }

    if(intMinLonDeg > intMaxLonDeg) 
    {
        HighlightTextError(document.getElementById(GetObjectID('txtMinLonDeg', 'input')));
        alert("Minimum longitude must be west of maximum longitude.");
       // alert("Minimum longitude must be less than maximum longitude.");
        return false;
    }
    else if(intMinLonDeg == intMaxLonDeg) 
    {
        if(intMinLonMin > intMaxLonMin) 
        {
            HighlightTextError(document.getElementById(GetObjectID('txtMinLonMin', 'input')));
            alert("Minimum longitude must be west of maximum longitude.");
            //alert("Minimum longitude must be less than maximum longitude.");
            return false;
        }
        else if(intMinLonMin == intMaxLonMin) 
        {
            if(floatMinLonSec >= floatMaxLonSec) 
            {
                HighlightTextError(document.getElementById(GetObjectID('txtMinLonSec', 'input')));
                alert("Minimum longitude must be west of maximum longitude.");
                 //alert("Minimum longitude must be less than maximum longitude.");
                return false;
            }
        }
    }
    return true;
}

/************************************************************
        function to Enable/Disable the date fields on Date criteria
        in Well wellbore advancesearch
************************************************************/
function EnblDisablWellwellboreAdvSrchDates(showHide)
{ 
    var trDates = document.getElementById(GetObjectID('trDates', 'tr'));
    var previousHeight = trDates.parentNode.parentNode.scrollHeight;//Table scroll height after expanding
    trDates.style.display = showHide;
     //Setting the splitter height during expand collapse
    //sart
    // debugger;
    var currentHeight = trDates.parentNode.parentNode.scrollHeight;//Table scroll height after expanding
    if(currentHeight > previousHeight)//while expanding decrease height by 180
    {
        SetSplitterHeight(window.parent.Splitter,document.body.scrollHeight +(currentHeight - previousHeight));  
    }
    //end 
}


            /***********FOR PARS UI***********/
/************************************************************
        function to Enable/Disable the date fields on Date Archived 
        selection in PARS ui
************************************************************/
function EnableDisableDates(optSelection)
{
    var trDates = document.getElementById(GetObjectID('trDates', 'tr'));
    var previousHeight = trDates.parentNode.parentNode.scrollHeight;//Table scroll height after expanding
    if(optSelection.value == "rbSelectDates")
    {        
        trDates.style.display = 'inline'
    }
    else
    {
        trDates.style.display = 'none'
    }
    //Setting the splitter height during expand collapse
    //sart
    // debugger;
    var currentHeight = trDates.parentNode.parentNode.scrollHeight;//Table scroll height after expanding
    SetSplitterHeight(window.parent.Splitter,document.body.scrollHeight +(currentHeight - previousHeight));  
    //end 
}

/************************************************************
        function to validate PARS ui page
************************************************************/
function ValidatePARS(isSaveSearchClicked)
{
    if(CheckAllControlsPARS(isSaveSearchClicked))
    {
		EnableButton();
        if(ValidatePARSDates() == true)
        {
            if(ValidateLatLongValues() == true)
            {
                return true;
            }			
        }
    }	
    return false;
}
/************************************************************
        function to loop through all controls to check whether 
        at least one item has value in PARS ui page
************************************************************/
function CheckAllControlsPARS(isSaveSearchClicked)
{ 

var fileUp = document.getElementById(GetObjectID('fileUploader','input')).value;
 if(!isSaveSearchClicked)
    {
                            if(fileUp == '')
                            {                           
                                if(document.getElementById(GetObjectID('cboSearchCriteria','select')).selectedIndex!=0)
                                {
                                 alert("Please select the file to search.");
                                 return false;
                                }
                            }
                            else
                            {
                                 if(document.getElementById(GetObjectID('cboSearchCriteria','select')).selectedIndex==0)
                                        {
                                         alert("Please select the required search parameter from ‘Search By’ drop down.");
                                         return false;
                                        }
                                 var extension = fileUp.substring(fileUp.lastIndexOf('.'));
                                    if(extension!='')
                                    {
                                        
                                        
                                        extension = extension.toString().toLowerCase();
                                        if(extension==".txt"||extension==".doc"||extension==".xls" || extension==".docx"||extension==".xlsx")
                                            {
                                            
                                            if(extension==".doc" || extension==".docx"){ReadWordDocument(fileUp,'hidWordContent');}
                                            if(extension==".xls" || extension==".xlsx"){ReadExcelDocument(fileUp,'hidWordContent');}
                                            return true;
                                            }
                                        else
                                        {
                                            alert('Please select a valid file format(*.txt, *.doc,*.docx, *.xls, *.xlsx).');
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                     alert('Please select a valid file format(*.txt, *.doc,*.docx, *.xls, *.xlsx).');
                                            return false;
                                    }
                            }
           }
         else
         {
          if((document.getElementById(GetObjectID('cboSearchCriteria','select')).selectedIndex!=0)||(fileUp != ''))
                                {
                                 alert("Save search is not applicable for file based search.");
                                 return false;
                                 }
         }                    
    var blnIsNotWildCard = false; 
    for(var index = 0; index < document.forms[0].elements.length; index++)
    {
        var element = document.forms[0].elements[index];
        
        if((element.id != document.getElementById(GetObjectID('txtSaveSearch', 'input')).id) &&
            (element.id != GetHTMLObject(window.parent,'txtSearchCriteria', 'input').id)&&
            (element.id != GetHTMLObject(window.parent,'searchString', 'input').id))
        {
			if(element.type == "radio")
			{
                if(element.checked==true)
                {
                    return true;
                }
			}
			else if(element.type == "text")
			{			    
				if(!isBlankspace(element.value))
                {
                    for (var intCount=0; intCount < element.value.length; intCount++)
                    {
                        if(element.value.charAt(intCount) != "*")
                        {
                            if(element.value.charAt(intCount) != "%")
                            {
                                blnIsNotWildCard = true;   
                            }	                            
                        }
                    }                    
                }
				else
				{
					element.value = "";
				}
			}
        }
    }
    if(!blnIsNotWildCard)
    {
        if(document.getElementById(GetObjectID('fileUploader','input')).value == '')
        {
            alert("Please enter a valid search criteria.");
            for(var index = 0; index < document.forms[0].elements.length; index++)
            {
                var element = document.forms[0].elements[index];		        
                
                if((element.id != document.getElementById(GetObjectID('txtSaveSearch', 'input')).id) &&
                (element.id != GetHTMLObject(window.parent,'txtSearchCriteria', 'input').id)&&
                (element.id != GetHTMLObject(window.parent,'searchString', 'input').id))
                {       
			        if(element.type == "text")
			        {			    
				        element.value = "";
			        }
                }
            }
            HighlightTextError(document.getElementById(GetObjectID('txtXMTITLE', 'input')));
            return false;
        }
        else
        {
            return true;
        }
    }
    else
    {
        return true;
    }
    alert("Please enter search criteria.");
    HighlightTextError(document.getElementById(GetObjectID('txtXMTITLE', 'input')));
    return false;
    
      
}

/************************************************************
        function to validate PARS ui page on [Save Search] click
************************************************************/
function ValidateSaveSrchPARS()
{
    if(ValidatePARS(true) == true)
        {
		    EnableButton();
		    var objTextBox  = document.getElementById(GetObjectID('txtSaveSearch', 'input'));
            if(isBlankspace(objTextBox.value))
            {                    
                alert("Please enter search criteria name.");
			    objTextBox.value="";
			    objTextBox.focus();
                return false;
            }
            else if(!isSplCharacter(objTextBox.value))
            {
                alert("Invalid Search Name. Please enter only alphanumeric values and special characters like hyphen or underscore.");
                objTextBox.value = "";
                objTextBox.focus();
            }
            else
            {
                return true;
            }
        }
        return false;
}

        /***********ADDED ON 16 APRIL 08***********/
/***************************************************************
            function to select/unselect all items
***************************************************************/
function SelectAll(CheckBoxControl, btnID) 
{
    if(document.forms[0].elements[CheckBoxControl].checked)
        {
            for (i=0; i < document.forms[0].elements.length; i++) 
            {                
                if (document.forms[0].elements[i].type == 'checkbox')
                {
                    if (document.forms[0].elements[i].id == 'chbSelectID')
                    {
                        document.forms[0].elements[i].checked = true;
                    }                    
                }
            }
        }
        else
        {
            for (i=0; i < document.forms[0].elements.length; i++) 
            {            
                if (document.forms[0].elements[i].type == 'checkbox')
                {
                    if (document.forms[0].elements[i].id == 'chbSelectID')
                    {
                        document.forms[0].elements[i].checked = false;                    
                    }
                }
            }
        } 
} 
 
/***************************************************************
            function to validate atleast one item is selected for view detailed report
***************************************************************/
function ValidateSelect()
{
    for (i=0; i < document.forms[0].elements.length; i++)
    {
        if (document.forms[0].elements[i].type == 'checkbox')
        {
            if(document.forms[0].elements[i].checked == true)
            {
                return true;
            }
        }
    }
    alert('Please select minimum one record.')
    return false;
}
/***************************************************************
           function to restrict users displaying more than
           10 records in DataSheet view
***************************************************************/

function DetailDataSheet()
{
    if(document.getElementById(GetObjectID('rdoViewMode_0','input')).checked)
    {
        var displayType = document.getElementById(GetObjectID('rdoViewMode_0','input')).value;
        var arrSelectedItems;
        arrSelectedItems = document.getElementById(GetObjectID('hidReportSelectRow','input')).value.split("|");
        if(arrSelectedItems.length > 11)
        {            
            document.getElementById(GetObjectID('rdoViewMode_1','input')).checked = true;
            alert('Please select maximum of 100 records for Data Sheet view.');
            return false;
        }
    }
}

/***************************************************************
            function to get Current Report Type
***************************************************************/
function getCurrentReportType()
{
	 // get the current URL
	 var url = window.location.toString();
	 //get the parameters
	 url.match(/\?(.+)$/);
	 var params = RegExp.$1; 
	 // split up the query string and store in an
	 // associative array 	 
	 var queryStringList = {};
	 var params = params.split("&");
	 
	 for(var i=0;i<params.length;i++)
	 {
	 	var tmp = params[i].split("=");
	 	queryStringList[tmp[0]] = tmp[1];
	 }
	  if(unescape(queryStringList["SearchType"]).replace("#",'')!='undefined')
	 {
	    return (unescape(queryStringList["SearchType"]).replace("#",''));	 
	 }
	 else
	 {
	   return (unescape(queryStringList["listSearchType"]).replace("#",''));	 
	 }
}

/***************************************************************
            function to submit Cross Post paging
***************************************************************/
function crossPagePosting(strPageName, strMethod)
{       
    document.forms[0].action=strPageName;
    document.forms[0].method=strMethod;
    document.forms[0].submit();
}

/***************************************************************
            to Lock the Table column.
***************************************************************/
function lockCol(tblID,colNumber)
{
    colNumber = colNumber + 1; 
    var table = document.getElementById(tblID);	
	var cTR = table.getElementsByTagName('TR');     
    if(colNumber == 1)
    {
        SetAlternateColor(tblID);
        for (i = 0; i < cTR.length; i++)
	    {		
		    var tr = cTR.item(i);
            tr.cells[0].className = 'checkLocked'; 
           tr.cells[0].style.width = "15px";           
        }
    }
    else
    {
	    //collection of rows
	    for (i = 0; i < cTR.length; i++)
	    {
		    var tr = cTR.item(i);
           tr.cells[0].style.width = "15px";
		    for(j=0; j<colNumber; j++)
		    {
		        tr.cells[j].className = 'locked';
		    }
	    }
	}
}   


/***************************************************************
            function to create date object from string
***************************************************************/
function GetDateObject(dateString, dateSeperator)
{
    var curPos = 0;
    var cDay, cMonth, cYear;

    //extract month portion
    curPos = dateString.indexOf(dateSeperator);
    cMonth = dateString.substring(0, curPos);
    
    //extract day portion
    endPos = dateString.indexOf(dateSeperator, curPos + 1);			
    cDay = dateString.substring(curPos + 1, endPos);

    //extract year portion			
    curPos = endPos;			
    cYear = dateString.substring(curPos + 1, dateString.length);

    //Create Date Object
    dtObject = new Date(cYear, cMonth - 1, cDay);		
    return dtObject;
}

/***************************************************************
            function to validate dates in well/wellbore search
***************************************************************/
function ValidateWellboreDates()
{
    var strFromDate, strToDate;
    strFromDate = document.getElementById(GetObjectID("txtFrom", "input")).value
    strToDate = document.getElementById(GetObjectID("txtTo", "input")).value
        
    if(document.getElementById(GetObjectID('rdoRbDate_0','input')).checked ||
        document.getElementById(GetObjectID('rdoRbDate_1','input')).checked)
    {   
       return CallValidateDateService("txtFrom","txtTo");
    }
    else
    {
        if(strFromDate != '' || strToDate != '')
        {
            alert('Select the date type.');
            return false;
        }
        else
        {
            return true;
        }
    }    
}

/***************************************************************
            function to validate PARS start and end date
***************************************************************/
function ValidatePARSDates()
{
    if(document.getElementById(GetObjectID('rbSelectDates', 'input')).checked)
    {
        return CallValidateDateService("txtStartDate","txtEndDate");
    }
    else
    {
        return true;
    }
}

/******************** TO VALIDATE DATE ENTRY*******************/
// Declaring valid date character, minimum year and maximum year
var dtCh= "-";
var minYear=1900;
var maxYear=3000;

/***************************************************************
            function to check a value is integer
***************************************************************/
function IsInteger(s){
	var i;
    for (i = 0; i < s.length; i++){   
        // Check that current character is number.
        var c = s.charAt(i);
        if (((c < "0") || (c > "9"))) return false;
    }
    // All characters are numbers.
    return true;
}

/***************************************************************
            function to search through string's characters one by one,
            if character is not in bag, append to returnString
***************************************************************/
function StripCharsInBag(s, bag){
	var i;
    var returnString = "";
    // Search through string's characters one by one.
    // If character is not in bag, append to returnString.
    for (i = 0; i < s.length; i++){   
        var c = s.charAt(i);
        if (bag.indexOf(c) == -1) returnString += c;
    }
    return returnString;
}

/***************************************************************
            function to validate February month
***************************************************************/
function DaysInFebruary (year){
	// February has 29 days in any year evenly divisible by four,
    // EXCEPT for centurial years which are not also divisible by 400.
    return (((year % 4 == 0) && ( (!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28 );
}

/***************************************************************
            function to validate date part from date value
***************************************************************/
function DaysArray(n) {
	for (var i = 1; i <= n; i++) {
		this[i] = 31
		if (i==4 || i==6 || i==9 || i==11) {this[i] = 30}
		if (i==2) {this[i] = 29}
   } 
   return this
}

/***************************************************************
    function to validate user entry is a valid date
***************************************************************/
function IsValidDate(dtStr)
{
	var daysInMonth = DaysArray(12);
	var pos1=dtStr.indexOf(dtCh);
	var pos2=dtStr.indexOf(dtCh,pos1+1);
    var strMonth = dtStr.substring(0,pos1);
	var strDay = dtStr.substring(pos1+1,pos2);
	var strYear=dtStr.substring(pos2+1);
	
	strYr=strYear;
	//newly added
	if((strDay.length>2)||(strMonth.length>2)||(strYr.length>4))
	{
	    alert("Please enter the date in 'yyyy-mm-dd' format.");
		return false
	}
	//
	if (strDay.charAt(0)=="0" && strDay.length>1) strDay=strDay.substring(1)
	if (strMonth.charAt(0)=="0" && strMonth.length>1) strMonth=strMonth.substring(1)
	for (var i = 1; i <= 3; i++) {
		if (strYr.charAt(0)=="0" && strYr.length>1) strYr=strYr.substring(1)
	}
	year=parseInt(strYr)
	month=parseInt(strMonth)
	day=parseInt(strDay)
		
	if (pos1==-1 || pos2==-1){
		alert("Please enter the date in 'yyyy-mm-dd' format.");
		return false
	}
	if (strYear.length != 4 || year==0 || year<minYear){
		alert("Please enter the year greater than 1900.");
		return false
	}
	if (strMonth.length<1 || month<1 || month>12){
		alert("Please enter a valid month.");
		return false
	}
	if (strDay.length<1 || day<1 || day>31 || (month==2 && day>DaysInFebruary(year)) || day > daysInMonth[month]){
		alert("Please enter a valid date.");
		return false
	}
	
	if (dtStr.indexOf(dtCh,pos2+1)!=-1 || IsInteger(StripCharsInBag(dtStr, dtCh))==false){
		alert("Please enter the date in 'yyyy-mm-dd' format.");
		return false
	}
    return true
}

/***************************************************************
            function to cancel processing
***************************************************************/
function CancelFn()
{    
   var sURL = "/DREAM%20HTML/CancelSearch.html";   
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
    	
	    window.parent.document.getElementById("BusyBoxIFrame").style.visibility = "hidden";	
	    window.parent.location.href = sURL;
	}
	catch(E)
	{
	    window.parent.document.getElementById("BusyBoxIFrame").style.visibility = "hidden";	
	    window.parent.location.href = sURL; 
	}        	
}

/***************************************************************
            function to validate date - accepts user entry
***************************************************************/
function ValidateDate(dtValue)
{
    return IsValidDate(dtValue)
}

/***************************************************************
       function used for exporting the page to Excel Sheet.
***************************************************************/

function ExportToExcel(obj)
{
       ExportToExcelNew();
}
/************************************************************
        function for getting the Map Result Table Id
************************************************************/
function GetMapReultTableID()
{
    var objectID = "";
    var resultID = "";
    for(index = 0; index < window.parent.document.documentElement.getElementsByTagName("table").length; index++)
    {
        objectId = window.parent.document.documentElement.getElementsByTagName("table").item(index).id;
        if(objectId.indexOf("tblSearchResults")>=0)
        {           
            resultID = objectId;
            break;
        }
    }
    return resultID;
}
/***************************************************************
        function to set Alternate color in the Results Grid
***************************************************************/
function SetAlternateColor(objTable)
{	
    var table = document.getElementById(objTable);	
	var cTR = table.getElementsByTagName('TR');  //collection of rows	
	for (i = 1; i < cTR.length; i++)
	{		    
	    var cnt = i % 2;	    
	    var tr = cTR.item(i);		    	    

        if( cnt > 0 )
        {            
  	        tr.style.background = '#EFEFEF';				    
	    }
	    else
	    {	     
	        tr.style.background='#FFFFFF';
	    }		
	}
}
/***************************************************************
        function to set Alternate color in the Results Grid
***************************************************************/
function SetAlternateColorForFUR(objTable)
{	
    var table = document.getElementById(objTable);	
	var cTR = table.getElementsByTagName('TR');  //collection of rows	
	for (i = 1; i < cTR.length; i++)
	{		    
	    var cnt = i % 2;	    
	    var tr = cTR.item(i);		    	    

        if( cnt > 0 )
        {          
	       
	         tr.style.background='#FFFFFF';				    
	    }
	    else
	    {	     
	        tr.style.background = '#ECECEC';
	    }		
	}
}

/************************************************************
        function to validate logs by field/depth advanced search ui        
************************************************************/
function ValidateLogByField()
{
        var strMessage = "";    	
	    if(CheckIsNull('txtFieldName', 'input')) strMessage = strMessage + "- Field Name \n";
        if((CheckIsNull('txtCurveTopDepth', 'input') == true) || (IsNumeric('txtCurveTopDepth') == false)) strMessage = strMessage + "- Curve Top Depth \n";
        if((CheckIsNull('txtCurveBottomDepth', 'input') == true) || (IsNumeric('txtCurveBottomDepth') == false)) strMessage = strMessage + "- Curve Bottom Depth \n";
    	
        if(strMessage != "")
        {
            strMessage = "The following field(s) are missing or invalid: \n" + strMessage;
            alert(strMessage);
            return false;
        }
        return true;     	
}


/************************************************************
        function is use to enable the button       
************************************************************/
function EnableButton()
{
	document.getElementById(GetObjectID("cmdSearch","input")).disabled=false;
	document.getElementById(GetObjectID("cmdSaveSearch","input")).disabled=false;	
	document.getElementById(GetObjectID("cmdReset","input")).disabled=false;	
}

/************************************************************
        function to validate the search name has value in 
        logs by field/depth advanced search ui on [Save Search] click        
************************************************************/
function ValidateSaveSrchLogByField()
{
	    var objSaveSearchField = document.getElementById(GetObjectID('txtSaveSearch', 'input'));
        if(ValidateLogByField() == true)
        {
		    EnableButton();
            if(isBlankspace(objSaveSearchField.value))
            {                    
                alert("Please enter search name.");
                objSaveSearchField.value = "";	
                objSaveSearchField.focus();
                return false;
            }
            else if(!isSplCharacter(objSaveSearchField.value))
            {
                alert("Invalid Search Name. Please enter only alphanumeric values and special characters like hyphen or underscore.");
                objSaveSearchField.value = "";
                objSaveSearchField.focus();
            }
            else
            {
                return true;
            }
        }
        return false;
} 
/***************************************************************
            function to validate Special Characters
***************************************************************/
function isSplCharacter(strInput)
{   
    var splChr = "~`!@#$&^|()+=[]\\\';,./{}|\":<>?%*";
    var SpecialChar='No';
    for (var i = 0; i < strInput.length; i++)
    {     
        var c = strInput.charAt(i);        
        if (splChr.indexOf(c) >= 0) 
        {            
            SpecialChar='Yes';
            break;
        }
    }        
    if (SpecialChar == 'Yes')	
    {        
        return false;	
    }	    
    else if (SpecialChar == 'No')	
    {		
        return true;	
    }
}
/************************************************************************
    Function to SET the Logs by well wellbore Search Result table color.
*************************************************************************/
function SetLogResultTableColor(obj)
{    
	var table = document.getElementById(obj);	
	var row = table.rows; 	
	for (i = 1; i < row.length; i++)
	{
	    if(row[i].cells[1].innerText != row[i - 1].cells[1].innerText)
        {
            row[i].style.background = '#DDDDDD';
            row[i].cells[0].className = 'checkLocked';
        }
        else
        {            
            row[i].cells[0].style.background = '#FFFFFF';
            row[i].cells[1].style.color = '#FFFFFF';
            row[i].cells[2].style.color = '#FFFFFF';
            row[i].cells[3].style.color = '#FFFFFF';
            row[i].cells[0].className = 'checkLocked';
        }
    }
}
/************************************************************************
    Function to SET the Logs by Detail Result table color.
************************************************************************/
function SetLogDetailTableColor(obj)
{
	var table = document.getElementById(obj);
	var cTR = table.getElementsByTagName('TR');  //collection of rows	
	for (i = 1; i < cTR.length; i++)
	{   
	    var tr = cTR.item(i);
	    var pTr = cTR.item(i-1);
	    		    	    
        if (tr.cells[1].innerText != pTr.cells[1].innerText||tr.cells[4].innerText != pTr.cells[4].innerText)
        {          
            tr.style.background = '#AAAAAA';         
        }
        else
        {
			var column = tr.cells;
            for (j=0; j < 7; j++)
            {
                tr.cells[j].style.background = '#EEEEEE';         
                tr.cells[j].style.color = '#EEEEEE';         
            }            
        }                
    }
}

/************************************************************************
    Check if Check Box Exist or not.
************************************************************************/
function IsCheckBoxExist()
{
    var blnCheckBoxExist = false;
    for (i=0; i < document.forms[0].elements.length; i++) 
    {   
        if (document.forms[0].elements[i].type == 'checkbox')
        {
            blnCheckBoxExist = true;
            break;
        }
     }
     return blnCheckBoxExist;
}
/*************************************************
    Validating Checkbox List
**************************************************/
function ValidateCheckBoxList() 
{    
        try
         {
           if(event.srcElement.type=='checkbox')
           {
                var nodes = event.srcElement.parentNode.childNodes;
                for(i =0, numberOfElements = childNodes.length; i < numberOfElements; ++i)
                    if(childNodes[i].type=='checkbox')
                        childNodes[i].checked='';
           }
           event.srcElement.checked='checked';
          }
          catch(E)
          {
          }
 }
 
 /************************************************
Function for expanding / collapsing the context menu
*************************************************/
var oldExpContextDiv=null;
function HideExpandCntxtSrch(divOrder)
{     
    var tblContextSearch = document.getElementById('tblContextSearch');
    var previousHeight = tblContextSearch.scrollHeight;//Table scroll height after expanding
    var key = document.getElementById("div_"+divOrder);
    var keyImg = document.getElementById("img_"+divOrder);

    if(document.getElementById("subMenu_"+divOrder).style.visibility=='visible')
    {
        document.getElementById("subMenu_"+divOrder).style.display = 'none';
        document.getElementById("subMenu_"+divOrder).style.visibility='hidden';
        key.className = "parent";
        oldExpContextDiv=null;
    }
    else
    {
        if(oldExpContextDiv != null)
        { 
            key = document.getElementById("div_"+oldExpContextDiv);
            document.getElementById("subMenu_"+oldExpContextDiv).style.display = 'none';
            document.getElementById("subMenu_"+oldExpContextDiv).style.visibility='hidden';
            key.className = "parent";
        }
        key = document.getElementById("div_" + divOrder);
        document.getElementById("subMenu_"+divOrder).style.display = 'block';
        document.getElementById("subMenu_"+divOrder).style.visibility='visible';
        key.className = "lvl3active1";
        oldExpContextDiv =  divOrder;
    }
    //Setting the splitter height during expand collapse
    //sart
    // debugger;
    var currentHeight = tblContextSearch.scrollHeight;//Table scroll height after expanding
    SetSplitterHeight(Splitter,Splitter.getStartPane().get_height() +(currentHeight - previousHeight));    
    //end 	    
}
 
 
 /************************************************
    Hide or Expand Function for Standard Searches.
 *************************************************/
 function hideExpand(divOrder)
 {     
	  key=document.getElementById("dvi_col_exp_"+divOrder);
	  if(document.getElementById("dvi_col_"+divOrder).style.display == 'block')
	 	{
	 		document.getElementById("dvi_col_"+divOrder).style.display = 'none';
	 		key.innerHTML="<b></b><img src='/_layouts/DREAM/images/plus.gif' onClick='hideExpand(" +divOrder+")'>"
	 		
	 	}
	 	else
	 	{
	 		document.getElementById("dvi_col_"+divOrder).style.display = 'block';
	 		key.innerHTML="<b></b><img src='/_layouts/DREAM/images/minus.gif' onClick='hideExpand("+divOrder+")'>"
	 	}
 }
 
 
 /************************************************
    Hide or Expand Function for Standard Searches.
 *************************************************/    
function hideExpand2()
 {
	  key=document.getElementById("dvi_col_exp_2")
	  if(document.getElementById("dvi_col_2").style.display == 'block')
	 	{
	 		document.getElementById("dvi_col_2").style.display = 'none'
	 		key.innerHTML="<b></b><img src='/_layouts/DREAM/images/plus.gif' onClick='hideExpand2()'>"
	 	}
	 	else
	 	{
	 		document.getElementById("dvi_col_2").style.display = 'block'
	 		key.innerHTML="<b></b><img src='/_layouts/DREAM/images/minus.gif' onClick='hideExpand2()'>"
	 	}
 }
   //Dream 4.0 changes start
/***************************************
   opens the standard search page.
***************************************/    
 function openStandardSearches(type)
 {
      OpenPageInContentWindow("/pages/StandardSearch.aspx?Type=" + type);  
 }
/***************************************
    Open Logs Field page.
***************************************/     
 function openLogsField(assetType)
 {
    OpenPageInContentWindow("/pages/AdvSearchlogsbyfielddepth.aspx?asset="+assetType);   
    
 }
/***************************************
    Open Map Search page.
***************************************/
 function openMapSearch(isApplicable)
 {
    if(isApplicable == 'ON')
    {
        OpenPageInContentWindow("/pages/MapSearch.aspx?SearchType=MapSearch")  
    }
    else
    {
        alert('Map Search is not applicable for the current region.');
    }
 }
/***************************************************************************
    Advanced Search Screen will open based on the QuickSearch Asset Type
**************************************************************************/     
 function openAdvSearch(assetType)
 {
            if(assetType == "Well")
            {   
                 OpenPageInContentWindow("/pages/AdvSearchWell.aspx?asset="+assetType)       
            }
            else if(assetType == "Wellbore")
            {   
                 OpenPageInContentWindow("/pages/AdvSearchWellWellbore.aspx?asset="+assetType)       
            }            
            else if(assetType == "Project Archives")
            {   
                 OpenPageInContentWindow("/pages/AdvSearchPARS.aspx?asset="+assetType)        
            }
            else if(assetType == "Field")
            { 
                OpenPageInContentWindow("/pages/AdvSearchField.aspx?asset="+assetType)
            } 
            else if(assetType == "Basin")
            {
               OpenPageInContentWindow("/pages/AdvSearchBasin.aspx?asset=" + assetType)
            }
            else if(assetType == "Reservoir")
            {  
               OpenPageInContentWindow("/pages/AdvSearchReservoir.aspx?asset="+assetType)
            }
            else if(assetType == "")
            {
                alert('Please select an asset.');
            }            
            else
            {
                alert('Advance search facility is not available for '+ assetType +', please use quick search option.');
            } 
 }
   //Dream 4.0 changes end
 /************************************************
function to show or hide the layers
*************************************************/
 function MM_showHideLayers() 
 {
    var i,p,v,obj,args=MM_
    Layers.arguments;
    for (i=0; i<(args.length-2); i+=3) 
        with (document) if (getElementById && ((obj=getElementById(args[i]))!=null)) { v=args[i+2];
    if (obj.style) { obj=obj.style; v=(v=='show')?'visible':(v=='hide')?'hidden':v; }
    obj.visibility=v; }
}
 
/************************************************************
        function to validate Field ui page
************************************************************/
function ValidateField(isSaveSearchClicked)
{
    if(CheckAllControlsField(isSaveSearchClicked))
    {
        return true;
    }	
    return false;
}
/************************************************************
        function to validate save search in ui page
************************************************************/
function ValidateSaveSrchFIELD()
{
	    var objSaveSearchField = document.getElementById(GetObjectID('txtSaveSearch', 'input'));
        if(ValidateField(true))
        {
          //Dream 3.1 fix
            var selectedItemCount = GetSelectedItemCount('lstCountry');
            if(selectedItemCount>999)
            //if(true)
            {
                alert("The maximum number of items per selection criteria is 999.You have exceeded this for \"Country\". Please amend your criteria and try again.");
                return false;
            }
		    EnableButton();
            if(isBlankspace(objSaveSearchField.value))
            {                    
                alert("Please enter search name.");
                objSaveSearchField.value = "";	
                objSaveSearchField.focus();
                return false;
            }
            else if(!isSplCharacter(objSaveSearchField.value))
            {
                alert("Invalid Search Name. Please enter only alphanumeric values and special characters like hyphen or underscore.");
                objSaveSearchField.value = "";
                objSaveSearchField.focus();
            }
            else
            {
                return true;
            }
        }
        return false;
}
/************************************************************
        function to validate save search in ui page
************************************************************/
function ValidateSaveSrchBasin()
{
	    var objSaveSearchField = document.getElementById(GetObjectID('txtSaveSearch', 'input'));
        if(ValidateField(true))
        {
            //Dream 3.1 fix
            var selectedItemCount = GetSelectedItemCount('lstBasin');
            if(selectedItemCount>999)
            {
                alert("The maximum number of items per selection criteria is 999.You have exceeded this for \"Basin\". Please amend your criteria and try again.");
                return false;
            }
		    EnableButton();
            if(isBlankspace(objSaveSearchField.value))
            {                    
                alert("Please enter search name.");
                objSaveSearchField.value = "";	
                objSaveSearchField.focus();
                return false;
            }
            else if(!isSplCharacter(objSaveSearchField.value))
            {
                alert("Invalid Search Name. Please enter only alphanumeric values and special characters like hyphen or underscore.");
                objSaveSearchField.value = "";
                objSaveSearchField.focus();
            }
            else
            {			   
                return true;
            }
        }
        return false;
}

/************************************************************
        function to loop through all controls to check whether 
        at least one item has value in Field ui page
************************************************************/
function CheckAllControlsField(isSaveSearchClicked)
{     

  var fileUp = document.getElementById(GetObjectID('fileUploader','input')).value;
   if(!isSaveSearchClicked)
    {
                            if(fileUp == '')
                            {                           
                                if(document.getElementById(GetObjectID('cboSearchCriteria','select')).selectedIndex!=0)
                                {
                                 alert("Please select the file to search.");
                                 return false;
                                }
                            }
                            else
                            {
                                
                                  
                                 if(document.getElementById(GetObjectID('cboSearchCriteria','select')).selectedIndex==0)
                                        {
                                         alert("Please select the required search parameter from ‘Search By’ drop down.");
                                         return false;
                                        }
                                       
                                 var extension = fileUp.substring(fileUp.lastIndexOf('.'));
                                   if(extension!='')
                                    {
                                        
                                        
                                        extension = extension.toString().toLowerCase();
                                        if(extension==".txt"||extension==".doc"||extension==".xls" || extension==".docx"||extension==".xlsx")
                                            {
                                            
                                            if(extension==".doc" || extension==".docx"){ReadWordDocument(fileUp,'hidWordContent');}
                                            if(extension==".xls" || extension==".xlsx"){ReadExcelDocument(fileUp,'hidWordContent');}
                                            return true;
                                            }
                                        else
                                        {
                                            alert('Please select a valid file format(*.txt, *.doc,*.docx, *.xls, *.xlsx).');
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                     alert('Please select a valid file format(*.txt, *.doc,*.docx, *.xls, *.xlsx).');
                                            return false;
                                    }
                            }
                            
         }
         else
         {
         if((document.getElementById(GetObjectID('cboSearchCriteria','select')).selectedIndex!=0)||(fileUp != ''))
                                {
                                 alert("Save search is not applicable for file based search.");
                                 return false;
                                 }
         }  
    var blnIsNotWildCard = false;   
    for(var index = 0; index < document.forms[0].elements.length; index++)
    {
        var element = document.forms[0].elements[index];		        
        
        if((element.id != GetHTMLObject(window,'txtSaveSearch', 'input').id) &&
            (element.id != GetHTMLObject(window.parent,'txtSearchCriteria', 'input').id)&&
            (element.id != GetHTMLObject(window.parent,'searchString', 'input').id))
        {       
			if(element.type == "radio")
			{
			/// Added for Field Adv Search and Reservoir Advance Search criteria validation.
			/// Selection of only AND or OR radio button should not be considered as valid criteria
			var rdblSearchCondID = GetObjectID('rdblSearchCond','table');
			if((element.id != rdblSearchCondID +"_0") && (element.id != rdblSearchCondID +"_1"))
			{								
                if(element.checked==true)
                {
                    return true;
                }	
             }			
			}
			else if(element.type == "select-multiple")
            {
                for(var indexSelect = 0; indexSelect < element.options.length; indexSelect++)
                {
                    if((element.id != GetHTMLObject(window,'cboSavedSearch', 'select').id) 
                    && (element.id != GetHTMLObject(window.parent,'cboQuickCountry', 'select').id)
                    && (element.id != GetHTMLObject(window.parent,'cboQuickAsset', 'select').id))
                    {
                        if(element.options[indexSelect].selected == true) 
                        {
                            return true;
                        }
                    }
                }
            }
			else if(element.type == "text")
			{				    		    
				if(!isBlankspace(element.value))
                {
                /// Added for RadControls validation
                /// RadComboBox has text element type and selected value is assigned to it.
                /// Selection of "---Select---" should not be considered as valid criteria
                    if(element.value != "---Select---")
                    {
                        for (var intCount=0; intCount < element.value.length; intCount++)
                        {
                            if(element.value.charAt(intCount) != "*")
                            {
                                if(element.value.charAt(intCount) != "%")
                                {
                                    blnIsNotWildCard = true;   
                                }	                            
                            }
                        }
                    }                    
                }
				else
				{
					element.value = "";
				}
			}
        }
    }
    
    if(!blnIsNotWildCard)
    {
        if(document.getElementById(GetObjectID('fileUploader','input')).value == '')
        {
            alert("Please enter a valid search criteria.");
            for(var index = 0; index < document.forms[0].elements.length; index++)
            {
                var element = document.forms[0].elements[index];		        
                
                if((element.id != GetHTMLObject(window,'txtSaveSearch', 'input').id) &&
                    (element.id != GetHTMLObject(window.parent,'txtSearchCriteria', 'input').id)&&
                    (element.id != GetHTMLObject(window.parent,'searchString', 'input').id))
                {       
			        if(element.type == "text")
			        {	
			         /// Added for RadControls validation
                     /// RadComboBox has text element type and selected value is assigned to it.
                     /// Selection of "---Select---" should not be considered as valid criteria
			            if(element.value != "---Select---")
                        {		    
				        element.value = "";
				        }
			        }
                }
            }
         return false;
        }
        else
        {
            return true;
        }
    }
    else
    {
        return true;
    }
    alert("Please enter search criteria.");
    return false;
    
     
    
} 
/**********************************************************************************
Storing the Identifiers for Context Searches and popup windows for Context Search
***********************************************************************************/  
var openPopupWindow = new Array();

/***************************************************************
            function to submit view detailed report
***************************************************************/
function GetLogSelectedRowIdentifiers()
{    
    var hidUWBI = GetHTMLObject(window,'hidUWBI','input');
    if(hidUWBI.value == "")
    {
        alert('Please select minimum one record.');  
        return false;      
    }
    else
    {
        return true;
    }
}

/************************************************
    Function to Set the Map result identifiers.
*************************************************/
function GetAllMapSelectedRowIdentifiers(window,columnIndex)
{ 
    var hidSelectedRows = GetHTMLObject(window,'hidSelectedRows','input');
    var hidSelectedCriteriaName = GetHTMLObject(window,'hidSelectedCriteriaName','input');
    var table = GetHTMLObject(window,'tblSearchResults','table');
    hidSelectedRows.value = '|';
    var row = table.rows;
    var checkBoxCounter = 0;
    var boolChecked = false;
    for(i = 2; i < row.length; i++)
	{
	    if(i == 2)
		{
		    hidSelectedCriteriaName.value = row[i].cells[columnIndex].innerText;
		}
		else
		{
		    if(row[i].cells[0].innerHTML.indexOf("CHECKED")>=0)
			{
			    boolChecked = true;
                hidSelectedRows.value = hidSelectedRows.value + row[i].cells[columnIndex].innerText + '|';
            }
		}
	}	
    if(boolChecked == false)
    {
        alert('Please select minimum one record.');  
        return false;
    }
    else
    {
        return true;
    }
}
/******************************************************************
    Get all the selected rows Checkbox value.
*******************************************************************/
function GetAllSelectedRowIdentifiers(window)
{
    var hidSelectedRows = GetHTMLObject(window,'hidSelectedRows','input');
    var hidSelectedCriteriaName = GetHTMLObject(window,'hidSelectedCriteriaName','input');
    var hidRowSelectedCheckBoxes = GetHTMLObject(window, 'hidRowSelectedCheckBoxes','input');
    if(hidRowSelectedCheckBoxes.value == "")
    {
        alert('Please select minimum one record.');  
        return false;      
    }
    else
    {
        hidSelectedRows.value = hidRowSelectedCheckBoxes.value; 
        hidSelectedCriteriaName.value = GetHTMLObject(window, 'chkbxRowSelectAll','input').value;
        return true;
    }
}
/******************************************************************
    Get  selected rows Checkbox value for EDM Report.
*******************************************************************/
function GetEDMSelectedRowIdentifiers()
{
    var hidSelectedRows = GetContentWindowObject('hidSelectedRows','input');
    var hidSelectedCriteriaName = GetContentWindowObject('hidSelectedCriteriaName','input');
    var table = GetContentWindowObject('tblSearchResults','table'); 
    var checkBoxCounter = 0;
    var arrCheckBoxes = table.getElementsByTagName('input');
    hidSelectedRows.value = "";
    for (i=0; i < arrCheckBoxes.length; i++)
    {        
        if (arrCheckBoxes[i].type == 'checkbox')
        {
            if (arrCheckBoxes[i].id == 'chbSelectID')
            {
                if(arrCheckBoxes[i].checked)
                {
                    checkBoxCounter++; 
                    if(checkBoxCounter >1)
                        break;        
                    hidSelectedRows.value += arrCheckBoxes[i].value;
                }
            }
            else if(arrCheckBoxes[i].name == 'chbSelectAll')
            {
                hidSelectedCriteriaName.value = arrCheckBoxes[i].value;
            }
        }
    }     
   if(checkBoxCounter == 1)
	{
	    return true;
	}
	else if(checkBoxCounter == 0)
	{
	    alert('Please select one record.');
	    return false;
	}
	else
	{
	    alert('Please select only one record.')
	    return false;
	}
}
//Dream 4.0 changes sart
/*************************************************
    Context search for List of Wells or Wellbores
**************************************************/
function DisplayList(url,listSearchType)
{ 
    var contentWindow =GetContentWindow();
    var strAsset = "";
    var quickSearchForm = contentWindow.document.forms[0];
    if(GetContentWindowObject('hidListSearch','input')!=null)
        GetContentWindowObject('hidListSearch','input').value = "True";
    if(GetContentWindowObject('hidListClick','input')!=null)
        GetContentWindowObject('hidListClick','input').value = "True";
    var objCountry = GetHTMLObject(window,"cboQuickCountry", "select");
    if(listSearchType.toLowerCase().indexOf("listofwells")>=0)
        strAsset = "Well";
    else if(listSearchType.toLowerCase().indexOf("listofwellbores")>=0)
        strAsset = "Wellbore";
    else if(listSearchType.toLowerCase().indexOf("listofreservoirs")>=0)
        strAsset = "Reservoir";
    else if(listSearchType.toLowerCase().indexOf("listoffields")>=0)
        strAsset = "Field";    
    if(objCountry!=null)
    {
      
        if(objCountry.selectedIndex==0)
        {
            quickSearchForm.action=url + '?asset=' +strAsset+'&listSearchType=' +listSearchType+'&country=0';
        }
        else
        {
            quickSearchForm.action=url + '?asset=' +strAsset+'&listSearchType=' +listSearchType+'&country='+objCountry.value;
        }
    }
    quickSearchForm.method="post";
    quickSearchForm.submit();  
}
/**************************************
    Context search popup window.
***************************************/
function ContextSearchPopup(url,searchType)
{ 
    var contentWindow = GetContentWindow();
    var quickSearchForm;
    if(contentWindow)
    {
        quickSearchForm = contentWindow.document.forms[0];
    }
    else
    {
        quickSearchForm = window.document.forms[0];
        contentWindow = window;
    }
    if (searchType.toString().match("&") == null)
    {       
        var msgWindow = contentWindow.open("", searchType, 'width=800,height=600,scrollbars=yes,resizable=yes,status=yes,left=100,top=100');
        quickSearchForm.action = url + "?SearchType=" + searchType;
        quickSearchForm.method = "post";
        quickSearchForm.target = searchType;
        quickSearchForm.submit();
    }    
    else
    {
        var title = searchType.toString().split("&")[0];
        var msgWindow = contentWindow.open("", title, 'width=800,height=600,scrollbars=yes,resizable=yes,status=yes,left=100,top=100');
        quickSearchForm.action = url + "?SearchType=" + searchType;
        quickSearchForm.method="post";
        quickSearchForm.target = title;
        quickSearchForm.submit(); 
    }
    quickSearchForm.target="_self";
    quickSearchForm.action= msgWindow.opener.location.href;
    quickSearchForm.method="post"; 
    msgWindow.focus();     
}
//Dream 4.0 changes end
/**************************************
    Function to set the window title.
***************************************/
function setWindowTitle(windowName)
{        
    if(windowName != "")
        window.document.title = windowName;
}
/*******************************************************
    Getting the selected Identifier value for Map Zoom 
********************************************************/
function GetSelectedMapIdentifier(objTable, mapDisplay)
{
    if(mapDisplay == 'ON')
    {
        var hidMapIdentified = document.getElementById(GetObjectID('hidMapIdentified','input'));
        hidMapIdentified.value = "";
        var allSelectedMapIdentifier="";
        var ctr = 0;
        var firstCheckedRow = 0;
        var table = document.getElementById(objTable);	
        var row = table.rows;
        //*added after implementing reorder columns
        var colName = document.getElementById(GetObjectID('hidMapUseColumnName','input')).value
        var columnIndex = $("table#tblSearchResults thead tr th:contains('"+ colName + "')")[0].cellIndex;
        var dataRowIndex = table.tBodies[0].firstChild.rowIndex;
       //** end of reorder changes
        var isRecordSelected = false;
        
        for(i = dataRowIndex; i < row.length; i++)
        {
            if(row[i].cells[0].innerHTML.indexOf("CHECKED") >= 0)
            {
                isRecordSelected = true;        
                if(i == dataRowIndex)
                {
                   hidMapIdentified.value = colName + "|";
                }      
                hidMapIdentified.value = hidMapIdentified.value + row[i].cells[columnIndex].innerText + "|";    
            }
        } 
        if(isRecordSelected) 
        {
            return true;
        }
        else
        {
            alert('Please select minimum one record.');
            return false;
        }
    }
    else
    {
        alert('Map Search is not applicable for the current region.');
        return false;
    }
}

/**********************************************************
    Function to Select all checkboxes of datagrid.
***********************************************************/
function CheckAll(me)
{
    var index = me.name.lastIndexOf('_'); 
    var prefix = me.name.substr(0,index);
    
    for(i=0; i<document.forms[0].length; i++) 
    { 
        var o = document.forms[0][i]; 
        if (o.type == 'checkbox') 
        { 
            if (me.name != o.name) 
            {
                
                var tmp = o.name.substring(0, prefix.length);
                // Replace all occurances of $ with _. The g in the regular expression means globally.
                if(o.name.indexOf("CheckBoxButton")>0)
                {
                    var tmp1 = tmp.replace(/\$/g,"_");
                    if (tmp1 == prefix) 
                    {
                        // Must be this way
                        o.checked = me.checked; 
                        //o.click(); 
                    }
                }
            }
        } 
    } 
}
/***************************************************************
             function used for All Dream search Menu
****************************************************************/
function showAllDREAMMenu(menuID,visibleMode,divStyle,id)
{    
    var divMenu = document.getElementById(menuID);     
        
        divMenu.style.top = "15px";
        var Link = document.getElementById(id);
        if(id=="adminLink")
        var leftPos = getposOffset(Link,"left") - 6; 
        else if(id=="allDreamsLink")
        var leftPos = getposOffset(Link,"left") - 60;
        else if(id=="SystemLink")
        var leftPos = getposOffset(Link,"left") - 60;
        else if(id=="userLink")
        var leftPos = getposOffset(Link,"left") - 60;
        else if(id=="utilityLink")
        var leftPos = getposOffset(Link,"left") - 60;
              
        divMenu.style.left = leftPos; 
        divMenu.style.visibility=visibleMode;
        divMenu.style.display =  divStyle;  
}
var ie4=document.all
var ns6=document.getElementById&&!document.all
/**********************************************************************************
    Function to Get the position for AllDreams Menu.
***********************************************************************************/
function getposOffset(what, offsettype){
var totaloffset=(offsettype=="left")? what.offsetLeft : what.offsetTop;
var parentEl=what.offsetParent;
while (parentEl!=null){
totaloffset=(offsettype=="left")? totaloffset+parentEl.offsetLeft : totaloffset+parentEl.offsetTop;
parentEl=parentEl.offsetParent;
}
return totaloffset;
}

/***********************************************
    Assign the cs class for mapgrid checkboxes
************************************************/
function mapGridCheckBox()
{   
    var objectID = "";
    var resultID = "";
    for(index = 0; index < window.parent.document.documentElement.getElementsByTagName("table").length; index++)
    {
        objectId = window.parent.document.documentElement.getElementsByTagName("table").item(index).id;
        if(objectId.indexOf("tblSearchResults")>=0)
        {           
            resultID = objectId;
            break;
        }
    }
    var table = document.getElementById(resultID);    
    var row = table.rows;
    row[0].cells[0].className = 'maplocked';
    for(i = 1; i < row.length; i++)
	{
	    row[i].cells[0].className = 'maplocked';
	}
}
/***********************************************
    setting the selected Map Identifier.
************************************************/
function setSelectedMapIdentifier(objTable)
{    
    try
    {   
        var arrSelectedMapIdentifier = new Array();
        arrSelectedMapIdentifier = document.getElementById(GetObjectID('hidCheckedColumns','input')).value;
        var columnIndex = parseInt(document.getElementById(GetObjectID('hidMapUseColumnName','input')).value);
        var checkedItems = arrSelectedMapIdentifier.split('|');
        var j = 0;
        var table = document.getElementById(objTable);	
        var row = table.rows;
        if(checkedItems.length > 0)
        {
            while(j < checkedItems.length)
            {
                var ctr = 0;
                for(i = 0; i < row.length; i++)
                {                
                    if(row[i].cells[columnIndex].innerText == checkedItems[j])
                    {
                        row[i].cells[0].innerHTML = row[i].cells[0].innerHTML.replace(/>/i," checked>");
                    }
                }
                j++;
            }
        }
    }
    catch(E)
    {
    }
}

//hides the site action for the normal user
function HideSiteActions()
{
     var objBannerFrame=document.getElementById('divBannerFrame');
     var objBannerText=objBannerFrame.innerText;
     if(objBannerText=="")
     {
        objBannerFrame.style.display='none';
     }
}
/***************************************
    Locking the Recall columns.
***************************************/
function RecalllockCol(tblID,colNumber)
{
    colNumber = colNumber + 1;
    var table = document.getElementById(tblID);
	var cTR = table.getElementsByTagName('TR');
    if(colNumber == 1)
    {
        for (i = 0; i < cTR.length; i++)
	    {
		    var tr = cTR.item(i);
            tr.cells[0].className = 'checkLocked';
            tr.cells[0].style.color = 'EEEEEE';
            tr.cells[0].style.width = "15px";
        }
    }    
}
//validates clicking on reset button if other operation is inprogress
function ValidateAdvSearchReset()
{   
    return true;
}
/***************************************
    Shows the map Unavailibility.
***************************************/
function ShowMapUnavailability()
{    
    alert('Map Search Functionality is currently not available.');
    return false;
}
/***************************************
    Opens the Busy box.
***************************************/
function OpenBusyBox()
{
    var objFrame=document.getElementById("BusyBoxIFrame");
    if(objFrame!=null)
    {
        objFrame.src="/_layouts/dream/Busybox.htm";
        busyBox.Show();
    } 
}
 
    /****************************************************************
       function to reset all controls in Feedback form
*****************************************************************/
function ResetFeedback()
{ 
    for(var index = 0; index < document.forms[0].elements.length; index++)
    {
        var element = document.forms[0].elements[index];
        if(element.type == "text" || element.type == "textarea")
        {
            element.value = "";
        }            
        else if(element.type == "radio" && element.id.indexOf('Rating')!="-1")
        {
            element.status = false;
        }
        else if(element.type == "select-one")
        {
        element.selectedIndex=0;
        }
    }
    return false;
}
/****************************************************************
       function to validate the Feedback ui
*****************************************************************/
function ValidateFeedback()
{
    var objSelectedItem;
    var objPageLevelComment;
    var objReasonForRating;
	for(var index = 0; index < document.forms[0].elements.length; index++)
    {
        var element = document.forms[0].elements[index];
        if((element.type == "text" || element.type == "textarea") && element.id.indexOf('Comment')!="-1")
        {
        objPageLevelComment=element;
        }
        if((element.type == "text" || element.type == "textarea") && element.id.indexOf('ReasonFor')!="-1")
        {
        objReasonForRating=element;
        }
        if(element.type == "radio" && element.id.indexOf('Rating')=="-1")
        {
            if(element.status == true)
            {
            objSelectedItem=element.value;
            }
        }
    }
    //check the condition for type of feedback to validate.
	if (objSelectedItem=="Page Level Feedback")
    {
        //check for pagelevel comment validation.
        if(!isBlankspace(objPageLevelComment.value))
        {
         return true;
        }
        else
        {
            objPageLevelComment.value="";
            alert('You must enter a comment to proceed.');
            objPageLevelComment.focus();
        }
    }
    else
    {
       
        var flag=0;
        for(var index = 0; index < document.forms[0].elements.length; index++)
        {
            var element = document.forms[0].elements[index];
            //this check for atleast one option is selected or not
            if(element.type == "radio" && element.id.indexOf('Rating')!="-1")
            {
                if(element.status == true)
                {
                   //if the option selected is very satisfied or very dissatisfied then check for reason. 
                   if(element.value.indexOf('Very')!="-1")
                   {
                        if(!isBlankspace(objReasonForRating.value))
                        {
                         return true;
                        }
                        else
                        {
                            objReasonForRating.value="";
                            alert('You must give a reason for your rating to proceed.');
                            objReasonForRating.focus();
                            return false;
                        }
                   }
                   else
                   {
                   return true;
                   }
                }
            }
        }
        alert('You must select a rating to proceed.');
    }
    return false;
}

/************************************************************
        function to make an html table visible true
************************************************************/
function showPageGeneralTable(theTable1,theTable2,theTable3,theTable4,theTable5,theTable6,theTable7,theTable8,theTable9)
{  
    try
    {	
	    var objTable1 = document.getElementById(GetObjectID(theTable1,'TR'));
	    var objTable2 = document.getElementById(GetObjectID(theTable2,'TR'));
	    var objTable3 = document.getElementById(GetObjectID(theTable3,'TR'));
	    var objTable4 = document.getElementById(GetObjectID(theTable4,'TR'));
	    var objTable5 = document.getElementById(GetObjectID(theTable5,'TR'));
	    var objTable6 = document.getElementById(GetObjectID(theTable6,'TR'));
	    var objTable7 = document.getElementById(GetObjectID(theTable7,'TR'));
	    var objTable8 = document.getElementById(GetObjectID(theTable8,'TR'));
	    var objTable9 = document.getElementById(GetObjectID(theTable9,'TR'));
	}
	catch (E)
	{	    
	}
	var objSelectedItem;
	for(var index = 0; index < document.forms[0].elements.length; index++)
    {
        var element = document.forms[0].elements[index];
        if(element.type == "radio" && element.id.indexOf('Rating')=="-1")
        {
            if(element.status == true)
            {
            objSelectedItem=element.value;
            }
        }
    }
	if (objSelectedItem=="Page Level Feedback")
    {
            if(objTable1 != null)objTable1.style.display = 'none';
            if(objTable2 != null)objTable2.style.display = 'none';
            if(objTable3 != null)objTable3.style.display = 'none';
            if(objTable4 != null)objTable4.style.display = 'none';
            if(objTable5 != null)objTable5.style.display = 'none';
            if(objTable6 != null)objTable6.style.display = 'none';
            if(objTable7 != null)objTable7.style.display = 'block';
            if(objTable8 != null)objTable8.style.display = 'block'; 
            if(objTable9 != null)objTable9.style.display = 'block'; 
            ResetControls()
    }
    else
    {
            if(objTable1 != null)objTable1.style.display = 'block';
            if(objTable2 != null)objTable2.style.display = 'block';
            if(objTable3 != null)objTable3.style.display = 'block';
            if(objTable4 != null)objTable4.style.display = 'block';
            if(objTable5 != null)objTable5.style.display = 'block';
            if(objTable6 != null)objTable6.style.display = 'block';
            if(objTable7 != null)objTable7.style.display = 'none';
            if(objTable8 != null)objTable8.style.display = 'none'; 
            if(objTable9 != null)objTable9.style.display = 'none'; 
            ResetControls()  
     }
     
  }
    
/************************************************
Hides the Page Level or Application Level FeedBack Options (depreciated)
*************************************************/
   function HidePageLevelFeedbackTable(theTable1,theTable2,theTable3)
   {  
   
    var objTable1 = document.getElementById(GetObjectID(theTable1,'TR'));
	var objTable2 = document.getElementById(GetObjectID(theTable2,'TR'));
	var objTable3 = document.getElementById(GetObjectID(theTable3,'TR'));
	    
   var objSelectedItem;
	for(var index = 0; index < document.forms[0].elements.length; index++)
    {
        var element = document.forms[0].elements[index];
        if(element.type == "radio" && element.id.indexOf('Feedback')!="-1")
        {
            if(element.checked == true)
            {
            objSelectedItem=element.value;
            }
        }
    }
  
	if (objSelectedItem=="Page Level Feedback")
    {
    objTable1.style.display="block";
    objTable2.style.display="block";
    objTable3.style.display = "block";
    }
    else
    {
     objTable1.style.display="none";
     objTable2.style.display="none";
     objTable3.style.display = "none";
    }
   
   }
/***************************************
    Reseting controls for Feedback page.
****************************************/    
function ResetControls()
{ 
    for(var index = 0; index < document.forms[0].elements.length; index++)
    {
        var element = document.forms[0].elements[index];
        if(element.type == "text" || element.type == "textarea")
        {
            element.value = "";
        }            
        else if(element.type == "radio" && element.id.indexOf('Rating')!="-1")
        {
            element.status = false;
        }
    }
    return false;
}
/******************************************
function to open feedback popup window
******************************************/
function OpenFeedback(url,title,wid,hite)
{
    var iWidth = wid;
    var iHeight = hite;
    var ileft = parseInt((screen.availWidth/2) - (iWidth/2));
    var itop = parseInt((screen.availHeight/2) - (iHeight/2));
    var sWindowFeatures = "width=" + iWidth + ",height=" + iHeight + ",status=yes,resizable=yes,left=" + ileft + ",top=" + itop + "screenX=" + ileft + ",screenY=" + itop;
    var currentURL = location.href;
    var arrPageName = currentURL.split("/");   
    var curPageName = arrPageName[arrPageName.length - 1];
    curPageName=(curPageName.split("?")[0]).split(".")[0];
    OpenPopup(url+"?pagename="+curPageName,title,sWindowFeatures);
}
//Dream 4.0 changes start
/***************************************************************
    Standard Search Result (Retrieving Save Search Results).
***************************************************************/
function StandardSearchResults(SiteURL, SearchType, SaveSearchName,AssetType)
{    
    var strUrl = SiteURL + "?SearchType=" + SearchType + "&savesearchname=" + SaveSearchName + "&manage=false&asset="+AssetType;
    window.parent.OpenPageInContentWindow(strUrl);
    window.parent.__doPostBack(GetClientID(window.parent,'updtPanelLeftNaV','div'),AssetType);
}

/***************************************************************
    Standard Search UI (Loading Save Search in UI)
***************************************************************/
function StandardSearchCriteriaDetail(SiteURL, SaveSearchName)
{
    var strUrl = SiteURL + "?savesearchname=" + SaveSearchName + "&manage=false";
    window.parent.OpenPageInContentWindow(strUrl);
}

/***************************************************************
    Standard Search Result (Retrieving Save Search Results).
***************************************************************/
function ManageSearchResults(SiteURL, SearchType, SaveSearchName,AssetType)
{    
    var strUrl = SiteURL + "?SearchType=" + SearchType + "&savesearchname=" + SaveSearchName + "&manage=true&asset=" + AssetType;
    window.parent.OpenPageInContentWindow(strUrl);
    window.parent.__doPostBack(GetClientID(window.parent,'updtPanelLeftNaV','div'),AssetType);
}

/***************************************************************
    Standard Search UI (Loading Save Search in UI)
***************************************************************/
function ManageSearchCriteriaDetail(SiteURL, SaveSearchName)
{
   var strUrl = SiteURL + "?savesearchname=" + SaveSearchName + "&manage=true";
   window.parent.OpenPageInContentWindow(strUrl);
}
//Added by dev for well wellbor advsrch
//Start
/***************************************************************
    Standard Search UI (Loading Save Search in UI) for well wellbore advanced search
***************************************************************/
function ManageWellWellboreSearchCriteriaDetail(SiteURL, SaveSearchName, AssetType)
{    
    var strUrl = SiteURL + "?asset=" + AssetType + "&savesearchname=" + SaveSearchName + "&manage=true";
    window.parent.OpenPageInContentWindow(strUrl);
}
/*****************************************************
    Opening the UI page for modifing saved Searches for well wellbore advanced search
*****************************************************/
function ModifyWellWellboreSaveSearch(SiteURL,SaveSearchName,AssetType)
{
   var strUrl = SiteURL + "?asset="+AssetType +"&savesearchname=" + SaveSearchName + "&operation=modify&manage=true" ;
    window.parent.OpenPageInContentWindow(strUrl);
}
/***************************************************************
    Standard Search UI (Loading Save Search in UI) for well wellbore advanced search
***************************************************************/
function WellWellboreStandardSearchCriteriaDetail(SiteURL, SaveSearchName,AssetType)
{
    var strUrl  = SiteURL +  "?asset="+AssetType + "&savesearchname=" + SaveSearchName + "&manage=false";
    window.parent.OpenPageInContentWindow(strUrl);
}
//Dream 4.0 changes end
/************************************************
function to get Well/WellBore Search Title.
*************************************************/
function GetWellWellboreAdvSrchTitle(rdbLstID)
{ 
	    var strTitle = "Advanced Search - "+ GetSelectedAssetType(rdbLstID);	    
        return strTitle;	  
}

/************************************************
function to get Selected Asset Type
*************************************************/
function GetSelectedAssetType(rdbLstID)
{

	    var rdbLst= document.getElementById(GetObjectID(rdbLstID,'table')); 
	    var rdbLstCollection  =rdbLst.getElementsByTagName("input");	
        var strAssetType = "";
	    for(var i = 0; i < rdbLstCollection.length; i++ )
            {      
                if(rdbLstCollection[i].checked == true)
                    {
                        strAssetType = rdbLstCollection[i].value;
                        break;
                    }
            }        
        return strAssetType;	  
}

/************************************************
function to set Well/WellBore Search Title.
*************************************************/
function SetWellWellboreAdvSearchTitle(tdID,rdbLstID)
{
  var tdTitle =document.getElementById(tdID);
  if(tdTitle!=null)
  {   
    tdTitle.innerText = " " + GetWellWellboreAdvSrchTitle(rdbLstID);
  }
  
}

/************************************************
function to show hide Well/WellBore Search Criteria
*************************************************/
function ShowHideWellboreSearchCriteria(rdbLstID)
{

    var showHide ="";
    if(GetSelectedAssetType(rdbLstID) =="Well")
     {
        showHide ="none";
        document.getElementById(GetObjectID('tdAssetStatus', 'td')).innerText = "Well Status";
     }
     else
     {
        showHide ="block";
        document.getElementById(GetObjectID('tdAssetStatus', 'td')).innerText ="Wellbore Status";
     }
     document.getElementById(GetObjectID('trAPIAreas_Identifier', 'tr')).style.display = showHide; 
     document.getElementById(GetObjectID('trBlock', 'tr')).style.display = showHide; 
     document.getElementById(GetObjectID('trLeaseIdentifier', 'tr')).style.display = showHide; 
     if(IsRdoBtnLstSelected("rdoRbDate"))
      {
        document.getElementById(GetObjectID('trDates', 'tr')).style.display = "inline";
      }   

}

/************************************************
function to check if Radio Button List is selected
*************************************************/
function IsRdoBtnLstSelected(radioBtnLstID)
{
  var rdbLst= document.getElementById(GetObjectID(radioBtnLstID,'table')); 
	    var rdbLstCollection  =rdbLst.getElementsByTagName("input");	
        var isSelected = false;
	    for(var i = 0; i < rdbLstCollection.length; i++ )
            {      
                if(rdbLstCollection[i].checked == true)
                    {
                       isSelected = true;
                        break;
                    }
            }        
        return isSelected;	 
}
//End
//Dream 4.0 changes start
/***************************************************************
            function to redirect to requested page in search results for EDM report
***************************************************************/
function EDMReportPaging(URL,pageNumber,maxRecords,recordcount,requestid,sortBy,sortType,activeDiv)
{
   var strParams ="pagenumber="+pageNumber+"&MaxRecords="+maxRecords+"&RecordCount="+recordcount+"&RequestId="+requestid+"&sortby="+sortBy+"&sorttype="+sortType+ "&activediv="+activeDiv.parentElement.id;
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
            function to redirect to requested page in search results for MDR report
***************************************************************/
function MDRReportPaging(URL,pageNumber,maxRecords,recordcount,requestid,sortBy,sortType,activeDiv)
{
   var strParams ="pagenumber="+pageNumber+"&MaxRecords="+maxRecords+"&RecordCount="+recordcount+"&RequestId="+requestid+"&sortby="+sortBy+"&sorttype="+sortType+ "&activediv="+activeDiv;
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
           On TabTabular Page Link Click eventhandler
***************************************************************/
function OnTabTabularPageLinkClick(pageNumber,sortBy,sortType,activeDiv)
{
   var strParams ="pagenumber=" + pageNumber + "&sortby=" + sortBy + "&sorttype=" + sortType + "&activediv=" + activeDiv;
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
            On TabTabular Sort Link Click eventhandler
***************************************************************/
function OnTabTabularSortLinkClick(sortBy,sortType,activeDiv)
{    
   var str = activeDiv.childNodes[0].rows[0].cells[0].innerText;
   var pageNumber = parseInt(str.substring(str.indexOf('[')+1,str.indexOf(']')))-1 ;
   var strParams ="pagenumber=" + pageNumber + "&sortby=" + sortBy + "&sorttype=" + sortType + "&activediv=" + activeDiv.id;
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
            function to sort results on selected column for EDM report.
***************************************************************/
function PaleoReportSorting(URL,pageNumber,recordcount,requestid,sortBy,sortType,maxRecords,activeDiv)
{    
   var str =activeDiv.childNodes[0].rows[0].cells[0].innerText;
   var pageNumber =parseInt(str.substring(str.indexOf('[')+1,str.indexOf(']')))-1 ;
    var strParams ="pagenumber="+pageNumber+"&RecordCount="+recordcount+"&RequestId="+requestid+"&sortby="+sortBy+"&sorttype="+sortType+"&MaxRecords="+maxRecords + "&activediv="+activeDiv.id;
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
            function to sort results on selected column for EDM report.
***************************************************************/
function EDMReportSorting(URL,pageNumber,recordcount,requestid,sortBy,sortType,maxRecords,activeDiv)
{    
   var strParams ="pagenumber="+pageNumber+"&RecordCount="+recordcount+"&RequestId="+requestid+"&sortby="+sortBy+"&sorttype="+sortType+"&MaxRecords="+maxRecords + "&activediv="+activeDiv.parentElement.id;
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
            function to redirect to requested page in search results.
***************************************************************/
function QuickSearchPaging(URL,pageNumber,maxRecords,recordcount,requestid,sortBy,sortType)
{
   var strParams ='pagenumber='+pageNumber+'&MaxRecords='+maxRecords+'&RecordCount='+recordcount+'&RequestId='+requestid+'&sortby='+sortBy+'&sorttype='+sortType;
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
           On page link click eventhandler for pagination in search results and context searches
***************************************************************/
function OnPageLinkClick(pageNumber,sortBy,sortType)
{
   var strParams ='pagenumber=' + pageNumber + '&sortby=' + sortBy + '&sorttype=' + sortType;
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
            function to sort results on selected column.
***************************************************************/
function QuickSearchSorting(URL,pageNumber,recordcount,requestid,sortBy,sortType,maxRecords)
{    
   var strParams ='pagenumber='+pageNumber+'&MaxRecords='+maxRecords+'&RecordCount='+recordcount+'&RequestId='+requestid+'&sortby='+sortBy+'&sorttype='+sortType;
   try
   { 
     __doPostBack(GetObjectID('UpdatePanelContentPage','div'),strParams);  
   }  
   catch(Ex)
   {
     alert(Ex.message);
   } 
}
//Dream 4.0 changes end
/*******************************************************
    EP-Catalog Functions...
*******************************************************/
function EPCatalogPopup(url)
{        
    if (url == 'Status')
    {
        var attributes="toolbar=yes,location=no,directories=yes,resizable,";
        attributes+="scrollbars=no,width=400, height=300, left=100, top=25";
        window.open("/_layouts/dream/Status.html","Status",attributes);
    }
    if (url == 'Attachment')
    {
        var attributes="toolbar=yes,location=no,directories=yes,resizable,";
        attributes+="scrollbars=yes,width=800, height=550, left=100, top=25";
        window.open("/_layouts/dream/EP-Attachment.html","Attachment",attributes);
    }
}
/*******************************************
    opening attachment page for EP-Catalog.
********************************************/
function openAttachment(url)
{
    var attributes="toolbar=yes,location=no,directories=yes,resizable,";
    attributes+="scrollbars=yes,width=800, height=600, left=100, top=25";
    window.open(url,"attachment",attributes);
}
/*******************************************************
    Context link from well report for EP-Catalog.
********************************************************/
function EPSearchContextLink(contextSearchName, assetType, columnIndex,resultType)
{    
    var contentWindow = GetContentWindow();
    if(resultType == 'MapResults')
    {
        if(GetAllMapSelectedRowIdentifiers(contentWindow,columnIndex))
        {
            EPContextSearchPopup(contentWindow,'/pages/ContextSearchResults.aspx',contextSearchName, assetType);
        }
    }
    else if(resultType == 'PVTReport')
    {
        if(GetAllEPSelectedRowIdentifiers(contentWindow))
        {  
            PVTContextSearchPopup(contentWindow,'/pages/PVTReport.aspx',encodeURIComponent(contextSearchName), assetType);
        }
    }
    else
    {
        if(GetAllEPSelectedRowIdentifiers(contentWindow))
        {        
           if(contextSearchName == "EPCatalogWithoutFilter")
            {
                EPContextSearchPopup(contentWindow,'/pages/EPCatalog.aspx',contextSearchName, assetType);
            }
            else
            {
                EPContextFilterPopup(contentWindow,'/pages/EPCatalogFilterScreen.aspx',contextSearchName, assetType);
            }   
        }
    }
}
function PVTContextSearchPopup(contentWindow,url,searchType,assetType)
{    
    var quickSearchForm = contentWindow.document.forms[0];
    var msgWindow = contentWindow.open('','PVTReport','width=800,height=600,scrollbars=yes,resizable=yes,status=yes,left=100,top=100');
    quickSearchForm.action=url + '?SearchType=' + searchType + '&assetType=' + assetType + '&country=' + GetSelectedCountry();
    quickSearchForm.method="post";
    quickSearchForm.target='PVTReport';
    quickSearchForm.submit();
    msgWindow.focus();
    quickSearchForm.target="_self";
    quickSearchForm.action= msgWindow.opener.location.href;
    quickSearchForm.method="post"; 
}

/**************************************************************
EP catalog results page opening as popup window
 **************************************************************/
function EPContextSearchPopup(contentWindow,url,searchType,assetType)
{    
    var quickSearchForm = contentWindow.document.forms[0];
    var msgWindow = contentWindow.open('',searchType,'width=800,height=600,scrollbars=yes,resizable=yes,status=yes,left=100,top=100');
    quickSearchForm.action=url + '?SearchType=' + searchType + '&assetType=' + assetType + '&country=' + GetSelectedCountry();
    quickSearchForm.method="post";
    quickSearchForm.target=searchType;
    quickSearchForm.submit();
    quickSearchForm.target="_self";
    quickSearchForm.action= msgWindow.opener.location.href;
    quickSearchForm.method="post"; 
    msgWindow.focus();
}



/***************************************
    Print option for EP-Catalog.
***************************************/
function EPprintContent(tableName)
{   
       var counter = 0;
       var isConfirmed = confirm("This option will print the records displayed on the current page.\nIf you have multiple pages of records to print please either print one page at a time or export to Excel and print from there.")
       if(isConfirmed == true)
       {		   
            var attributes="toolbar=yes,location=no,directories=yes,resizable,menubar=yes,";
            attributes+="scrollbars=yes,width=800, height=600, left=100, top=25";
            window.open("/_layouts/dream/print.htm","Print",attributes);       
        }
        else
        {
           alert("Print option is enabled only for search results and details page.");
        }      
}
/**************************************************************
 Export to excel for ep catalog page level
 **************************************************************/
function EPExportToExcel(obj)
{        
    alert('This option will only export the records displayed on the current page. If you have multiple pages you will have to export each page separately.'); 
	document.body.style.cursor = 'wait';
    String.prototype.Trim = function () 
    {
        return this.replace(/\|*\s*$/, "");
    } 
    if (obj.tagName == "TABLE")
	{
	    //Assumes that the obj is THE OBJECT
	    table = obj;
	}
	else
	{
	    table = document.getElementById(obj);
	    sObj = obj.id + "title";
	    ttable = document.getElementById(sObj);
	}
	try
    {
        var xls = new ActiveXObject("Excel.Application");
        
        xls.Workbooks.add();
	    xls.Workbooks(1).WorkSheets(1).Name = "Search Result";
		
		var tempCount = 1;
		var rows=table.rows;
		for(i = 0; i < rows.length; i++)
		{	
            var column = rows[i].cells;

		    for(j = 1; j < column.length; j++)
		    {
    		    		    
			    xls.Cells(i+1, j).FormulaLocal = "'" + column[j].innerText.Trim();					
		        xls.columns.AutoFit();				    
		        if(i == 0)
	            {
	                xls.Cells(i+1, j).Font.Bold = true;				    
		            xls.Cells(i+1, j).Interior.ColorIndex = 15;
	            }					
		    }						
		}		
	    xls.visible = true;
        }
        catch( E )
        {
            alert('Either excel is not installed or Your browser security setting is not allowing to create excel object.');
        }
	    document.body.style.cursor = 'auto';
}
/**************************************************************
 Export to excel for FUR page level
 **************************************************************/
function FURExportToExcel(obj)
{        
    alert('This option will only export the records displayed on the current page.'); 
	document.body.style.cursor = 'wait';
    String.prototype.Trim = function () 
    {
        return this.replace(/\|*\s*$/, "");
    }    
	table = document.getElementById(obj);
	try
    {
       var xls = new ActiveXObject("Excel.Application"); 
        xls.Workbooks.add();
	     var xslSheet = xls.Workbooks(1).WorkSheets(1);
	    xls.Workbooks(1).WorkSheets(1).Name = "Functionality Usage Report";
		
		var tempCount = 1;
		var rows=table.rows;
		for(i = 0; i < rows.length; i++)
		{	
            var column = rows[i].cells;

		    for(j = 0; j < column.length; j++)
		    {
    		    		    
			    if (i == 0)
                   {
                     xslSheet.Cells(i+1, j+1).FormulaLocal = column[j].innerText.Trim();
                     xslSheet.Cells(i+1, j+1).Font.Bold = true;
                     xslSheet.Cells(i+1, j+1).Interior.ColorIndex = 15;                                                   
                   }
               else
                   {
                        xslSheet.Cells(i+1, j+1).Value = column[j].innerText;
                        xslSheet.Cells(i+1, j+1).NumberFormat = "0";
                        if(rows[0].cells[j].innerText.toLowerCase().indexOf("year")>=0)
                        {
                            xslSheet.Cells(i+1, j+1).HorizontalAlignment = -4131;//xlLeft is the constant -4131 
                        }
                   }  
               xslSheet.columns.AutoFit();     				
		    }						
		}		
	    xls.visible = true;
        }
        catch( E )
        {       
            alert('Either excel is not installed or Your browser security setting is not allowing to create excel object.');
        }
	    document.body.style.cursor = 'auto';
}
/*************************************************************************
       function used for opening a export to excel all in new page
*************************************************************************/
function ExportToExcelAll(url,ReportType)
{
    alert('Please close the popup after records are exported.');    
    var requestid = document.getElementById(GetObjectID('hidRequestID','input')).value;
    var searchType= document.getElementById(GetObjectID('hidSearchType','input')).value;
    var strMaxRecord= document.getElementById(GetObjectID('hidMaxRecord','input')).value;
    var fileType = GetHTMLObject(window,'hidFileType','input').value;
    if(searchType == "edmreport")
      {
            var msgWindow=window.open('',ReportType,'width=400,height=300,scrollbars=no,resizable=no,status=no,left=100,top=100');
            document.forms[0].action=url + '?Searchtype=' + searchType+'&Requestid='+requestid+'&MaxRecord='+strMaxRecord + '&ReportType=' + ReportType;
            document.forms[0].method="post";
            document.forms[0].target=ReportType;
            document.forms[0].submit();
            msgWindow.focus();
            document.forms[0].target="_self";
            document.forms[0].action= msgWindow.opener.location.href;
            document.forms[0].method="post"; 
       }
        else if(ReportType == "DirectionalSurveyDetailChartResults")
      {
            var msgWindow=window.open('',ReportType,'width=400,height=300,scrollbars=no,resizable=no,status=no,left=100,top=100');
            document.forms[0].action=url + '?Searchtype=' + searchType+'&Requestid='+requestid+'&MaxRecord='+strMaxRecord+'&ReportType='+ReportType;
            document.forms[0].method="post";
            document.forms[0].target=ReportType;
            document.forms[0].submit();
            msgWindow.focus();
            document.forms[0].target="_self";
            document.forms[0].action= msgWindow.opener.location.href;
            document.forms[0].method="post"; 
       }
           else if(ReportType == "PicksDetailChartResults")
      {
            var msgWindow=window.open('',ReportType,'width=400,height=300,scrollbars=no,resizable=no,status=no,left=100,top=100');
            document.forms[0].action=url + '?Searchtype=' + searchType+'&Requestid='+requestid+'&MaxRecord='+strMaxRecord+'&ReportType='+ReportType;
            document.forms[0].method="post";
            document.forms[0].target=ReportType;
            document.forms[0].submit();
            msgWindow.focus();
            document.forms[0].target="_self";
            document.forms[0].action= msgWindow.opener.location.href;
            document.forms[0].method="post"; 
       }
    else if(GetselectedColumnIdentifiers())
      {            
            var msgWindow=window.open('',ReportType,'width=400,height=300,scrollbars=no,resizable=no,status=no,left=100,top=100');
            document.forms[0].action=url + '?Searchtype=' + searchType+'&Requestid='+requestid+'&MaxRecord='+strMaxRecord+'&FileType='+fileType+'&ReportType='+ReportType;
            document.forms[0].method="post";
            document.forms[0].target=ReportType;
            document.forms[0].submit();
            document.forms[0].target="_self";
            document.forms[0].action= msgWindow.opener.location.href;
            document.forms[0].method="post"; 
            msgWindow.focus();
       }
}

/*************************************************************************
       function used for geting sellected columns from detail reports
*************************************************************************/
function GetselectedColumnIdentifiersDetail()
{   
    document.getElementById(GetObjectID('hidDetailSelectedColumns','input')).value = '';    
    var table = document.getElementById('tblSearchResults');
    var row = table.rows;
    var checkBoxCounter = 0;
    var boolChecked = false;  
    for (i=0; i < document.forms[0].elements.length; i++)
    {        
        if (document.forms[0].elements[i].type == 'checkbox')
        {
            if (document.forms[0].elements[i].id == 'chbSelectColID')
            {
			    checkBoxCounter++;
                if(document.forms[0].elements[i].checked == true)
                {
                    boolChecked = true; 
                    document.getElementById(GetObjectID('hidDetailSelectedColumns','input')).value = document.getElementById(GetObjectID('hidDetailSelectedColumns','input')).value + document.forms[0].elements[i].value;
                }
            }
        }
    }
    if(boolChecked == false)
    {
        alert('Please select minimum one column.');  
        return false;      
    }
    else
    {
        return true;
    } 
}
/***********************************************************************************
       function used for geting sellected columns from search/Context search reports
************************************************************************************/
function GetselectedColumnIdentifiers()
{  
   
    var arrColTags = $("table#tblSearchResults col");
    var arrChbx = $("table#tblSearchResults thead tr input:checked");
    var hidSelectedColumns =  document.getElementById(GetObjectID('hidSelectedColumns','input'));   
    hidSelectedColumns.value = "";
     if(arrChbx.length<=0)
       {  
         alert('Please select minimum one column.');  
         return false;      
       }
       for(var i = 0; i < arrChbx.length; i++)
           {
                if((arrChbx[i].id == "chbSelectColID") && (arrColTags[arrChbx[i].parentNode.cellIndex].className != "hide"))
                {
                  hidSelectedColumns.value  += arrChbx[i].value;
                }
           }
    return true;
}
/***********************************************************************************
       function used for checking the all the columns are selected or not
************************************************************************************/
function IsSellectAllColumns(ReportType)
{
    var table = document.getElementById('tblSearchResults');
    var row = table.rows;
    var boolChecked = false; 
    for (i=0; i < document.forms[0].elements.length; i++)
    {        
        if (document.forms[0].elements[i].type == 'checkbox')
        {
            if (document.forms[0].elements[i].id == 'chkColSelBox')
            {
                if(document.forms[0].elements[i].checked == true)
                {
                    if(!ChkDeselectColumns())
                    {
                        boolChecked = true;
                        if(ReportType == "DetailReport")
                        { 
                            document.getElementById(GetObjectID('hidDetailSelectedColumns','input')).value = "ALLCOLUMNS";
                        }
                        else
                        {
                            document.getElementById(GetObjectID('hidSelectedColumns','input')).value = "ALLCOLUMNS";
                        }
                    }
                    else
                    {
                        boolChecked = false;
                    }
                }
            }
        }
    }
    if(boolChecked == false)
    {
        return false;      
    }
    else
    {
        return true;
    }   
}
/***********************************************************************************
       function used for select all the columns by default on search results page
************************************************************************************/
function SellectAllMapColumns(me)
{
  
    if(document.forms[0].elements['chbColSelectAll'].checked)
        {
            for (i=0; i < document.forms[0].elements.length; i++) 
            {                
                if (document.forms[0].elements[i].type == 'checkbox')
                {
                    if (document.forms[0].elements[i].id == 'chbSelectColID')
                    {
                        document.forms[0].elements[i].checked = true;
                    } 
                }
            }
        }
        else
        {
            for (i=0; i < document.forms[0].elements.length; i++) 
            {            
                if (document.forms[0].elements[i].type == 'checkbox')
                {
                  if (document.forms[0].elements[i].id == 'chbSelectColID')
                    {
                        document.forms[0].elements[i].checked = false;  
                    }                  
                }
            }
        } 
}
/***********************************************************************************
       function used for select all the columns by default on search results page
************************************************************************************/
function SellectAllColumns(CheckBoxControl, cmdID)
{
    if(document.forms[0].elements[CheckBoxControl].checked)
        {
            for (i=0; i < document.forms[0].elements.length; i++) 
            {                
                if (document.forms[0].elements[i].type == 'checkbox')
                {
                    if (document.forms[0].elements[i].id == 'chbSelectColID')
                    {
                        document.forms[0].elements[i].checked = true;
                    } 
                }
            }
        }
        else
        {
            for (i=0; i < document.forms[0].elements.length; i++) 
            {            
                if (document.forms[0].elements[i].type == 'checkbox')
                {
                  if (document.forms[0].elements[i].id == 'chbSelectColID')
                    {
                        document.forms[0].elements[i].checked = false;  
                    }                  
                }
            }
        } 
}

/***********************************************************************************
       function used for check the column are selected or not
************************************************************************************/
function validateColumnCheckedToExport()
{
   var objectID = "";
    var resultID = "";
    for(index = 0; index < window.parent.document.documentElement.getElementsByTagName("table").length; index++)
    {
        objectId = window.parent.document.documentElement.getElementsByTagName("table").item(index).id;
        if(objectId.indexOf("tblSearchResults")>=0)
        {           
            resultID = objectId;
            break;
        }
    }
    var table = document.getElementById(resultID);    
    var row = table.rows;
    var checkBoxCounter = 0;
    var boolChecked = false;
    var column = row[0].cells;
    for(i = 1; i < column.length; i++)
	{
	    if(row[0].cells[i].innerHTML.indexOf("CHECKED")>=0)
		{
			boolChecked = true;             
        }
	}   
    if(boolChecked == false)
    {        
        return false;      
    }
    else
    {
        return true;
    }
}
/***********************************************************************************
       function used for check the column are selected or not
************************************************************************************/
function validateMapColumnCheckedToExport()
{    
   var objectID = "";
    var resultID = "";
    for(index = 0; index < window.parent.document.documentElement.getElementsByTagName("table").length; index++)
    {
        objectId = window.parent.document.documentElement.getElementsByTagName("table").item(index).id;
        if(objectId.indexOf("tblSearchResults")>=0)
        {           
            resultID = objectId;
            break;
        }
    }
    var table = document.getElementById(resultID);    
    var row = table.rows;
    var checkBoxCounter = 0;
    var boolChecked = false;
    var column = row[1].cells;
    for(i = 0; i < column.length; i++)
	{
	    if(row[1].cells[i].innerHTML.indexOf("CHECKED")>=0)
		{
			boolChecked = true;             
        }
	}   
    if(boolChecked == false)
    {        
        return false;      
    }
    else
    {
        return true;
    }
}

/***********************************************************************************
       function used for check any colums are deselected or not
************************************************************************************/
function ChkDeselectColumns()
{
    var boolAnyColChecked = true;
    if(document.forms[0].elements['chbSelectColID'].checked)
        {
            for (i=0; i < document.forms[0].elements.length; i++) 
            {                
                if (document.forms[0].elements[i].type == 'checkbox')
                {
                    if (document.forms[0].elements[i].id == 'chbSelectColID')
                    {
                       if(document.forms[0].elements[i].checked = false)
                       {
                            boolAnyColChecked = false;
                       }
                    } 
                }
            }
        }
        if(boolAnyColChecked == true)
        {
            return true;
        }
        else
        {
            return false;
        }
} 

/***********************************************************
    Export All For EP-Catalog
************************************************************/
function EPExportToExcelAll(url, searchType)
{
    alert('Please close the popup after records are exported.');   
    var msgWindow=window.open('',"popsup",'width=400,height=300,scrollbars=no,resizable=no,status=no,left=100,top=100');
    document.forms[0].action=url + '?Searchtype=' + searchType + '&assetType=Well';
    document.forms[0].method="post";
    document.forms[0].target="popsup";
    document.forms[0].submit();
    msgWindow.focus();
    document.forms[0].target="_self";
    document.forms[0].action= msgWindow.opener.location.href;
    document.forms[0].method="post";
}
//Dream 4.0 changes start
/***************************************
    Paging for EP-Catalog Sorting.
***************************************/
function EPSorting(URL,pageNumber,recordcount,requestid,sortBy,sortType,maxRecords,searchType)
{   
   var strParams ='pagenumber='+pageNumber+'&MaxRecords='+maxRecords+'&RecordCount='+recordcount+'&RequestId='+requestid+'&sortby='+sortBy+'&sorttype='+sortType+'&SearchType='+searchType+'&assetType='+assetType;
   try
   {
     __doPostBack(GetObjectID('UpdatePanelContentPage','div'),strParams);  
   }  
   catch(Ex)
   {
     alert(Ex.message);
   } 
}
/***************************************
    Paging for EP-Catalog Sorting.
***************************************/
function EPOnSortLinkClick(pageNumber,sortBy,sortType)
{   
   var strParams ='pagenumber=' + pageNumber + '&sortby=' + sortBy + '&sorttype=' + sortType ;
   try
   {
     __doPostBack(GetObjectID('UpdatePanelContentPage','div'),strParams);  
   }  
   catch(Ex)
   {
     alert(Ex.message);
   } 
}
/***************************************
    Paging for EP-Catalog Paging.
***************************************/
function EPOnPageLinkClick(pageNumber,sortBy,sortType)
{
   var strParams ='pagenumber=' + pageNumber + '&sortby=' + sortBy + '&sorttype='+sortType ;
   try
   {
     __doPostBack(GetObjectID('UpdatePanelContentPage','div'),strParams);  
   }  
   catch(Ex)
   {
     alert(Ex.message);
   } 
}
/***************************************
    Paging for EP-Catalog Paging.
***************************************/
function EPPaging(URL,pageNumber,recordcount,requestid,sortBy,sortType,maxRecords,searchType,assetType)
{
   var strParams ='pagenumber='+pageNumber+'&MaxRecords='+maxRecords+'&RecordCount='+recordcount+'&RequestId='+requestid+'&sortby='+sortBy+'&sorttype='+sortType+'&SearchType='+searchType+'&assetType='+assetType;
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
    Disabling clicking anywhere on the page while search is on.
****************************************************************/
document.onmousedown = function()
{    
    DisableMouseClick();
}
function DisableMouseClick()
{
    try
    {
        var objIFrame = document.getElementById("BusyBoxIFrame");
        if(objIFrame == null)
        {
            if(document.parent != null)
            {
                objIFrame = document.parent.document.getElementById("BusyBoxIFrame");
            }
        }
        if(objIFrame == null)
             return true;        
        if(objIFrame.style.visibility == "hidden")
        {            
            return true;            
        }
        else
        {
            if(event.button==1)
            {
                alert('This option is disabled when search is on.');
                return false;                
            }
            else
            {
                return true;
            }
        }
    }
    catch(Ex)
    {
        return true;
    }
}
/*************************************************
    Disabling ctl+N on the window.
*************************************************/
document.onkeydown = function()
{    
    if ((event.keyCode == 78) && (event.ctrlKey))
    {
        event.cancelBubble = true;
        event.returnValue = false;
        event.keyCode = false; 
        return false;
    }
    else
    {return true;}
}
//Dream 4.0 changes end
/***********************************************
    Close the window on context search cancel.
************************************************/
function CloseContext()
{    
    try
    {
       window.parent.document.getElementById("BusyBoxIFrame").style.visibility = "hidden";	
	   window.parent.close();
	}
	catch(E)
	{	
	    window.parent.close();
	}
}

/******************************************************************
    Get all the selected rows Checkbox value.
*******************************************************************/
function GetAllEPSelectedRowIdentifiers(window)
{
    var strSearchType = GetHTMLObject(window,'hidAssetName','input').value;
    var colName = GetTableAttribute(window,"assetColName");
    var hidSelectedRows = GetHTMLObject(window,'hidSelectedRows','input');
    var hidSelectedCriteriaName = GetHTMLObject(window,'hidSelectedCriteriaName','input');
    var hidSelectedAssetNames = GetHTMLObject(window,'hidSelectedAssetNames','input');
    if(hidSelectedAssetNames.value == "")
    {
        alert('Please select minimum one record.');  
        return false;      
    }
    else
    {
        hidSelectedRows.value = hidSelectedAssetNames.value; 
        hidSelectedCriteriaName.value = colName;
        return true;
    }
}

/*******************************************************************************
    iWellFile Functions
********************************************************************************/
var striWellSelectionCriteria = "";
function iWellFile(URL, assetType, columnIndex, searchType)
{
    if(ValidateiWellFileSelection("tblSearchResults", columnIndex, searchType))
    {        
        openiWellFilePopup(URL);
    }
}
/*************************************************************
    Validate the report selection for iWellFile.
*************************************************************/
function ValidateiWellFileSelection(tableName, columnIndex ,searchType)
{    
    var table = document.getElementById(tableName);
    var row = table.rows;
    var intCheckBoxCounter = 0;
    for(i = 2; i < row.length; i++)
	{
	    if(row[i].cells[0].innerHTML.indexOf("CHECKED")>=0)
		{
			intCheckBoxCounter++;
			striWellSelectionCriteria = row[i].cells[columnIndex].innerText;
        }
	}
	if(intCheckBoxCounter == 1)
	{
	    return true;
	}
	else if(intCheckBoxCounter == 0)
	{
	    alert('Please select one record.');
	    return false;
	}
	else
	{
	    alert('Please select only one record.')
	    return false;
	}
}

/************************************************************
        Opening the iWellFile Popup Window
************************************************************/
function openiWellFilePopup(URL)
{       
    var url = URL + '?WELLID=' + striWellSelectionCriteria;
    var attributes = 'location=yes,width=1024,height=650,scrollbars=yes,resizable';
    OpenPopup(url,"iWellFile",attributes);
}
/************************************************************
        Opening the iRequest Popup Window
************************************************************/
function OpeniRequest()
{
    var attributes='';    
    OpenPopup("/DREAM%20HTML/EPRequest.html",'Display',attributes);
}
/***************************************
    Open Query Search
***************************************/
 function openQuerySearch()
 {
     OpenPageInContentWindow("/pages/QuerySearch.aspx");
 }
   function openSpecialSearch(url)
 {
    OpenPageInContentWindow(url);
 }
 /***************************************
    Validate Query Search UI.
***************************************/
 function ValidateQuerySearch()
 {    
    var txtSearchText =  document.getElementById(GetObjectID("txtsearchText", "input"));            
    var cboDataSource = document.getElementById(GetObjectID("cboDataSource", "select"));
    var cboDataProvider = document.getElementById(GetObjectID("cboDataProvider", "select"));
    var cboTableName = document.getElementById(GetObjectID("cboTableName", "select"));
    var cboColumnName = document.getElementById(GetObjectID("cboColumnName", "select"));
    
    if(cboTableName.options[0].selected == true)
    {
        alert('Please Select the Table Name.');
        cboTableName.focus();
        return false;
    }
    else if(cboColumnName.options[0].selected == true)
    {
        alert('Please Select the Column Name.');
        cboColumnName.focus();
        return false;
    }
    else if(isBlankspace(txtSearchText.value))
    {
        alert("Please enter the search criteria.");
        txtSearchText.value = "";
        txtSearchText.focus();        
        return false;
    }
    else
    {
        return true;
    }
 }
 /*************************************************************************
    Deselecting the header checkbox when other check boxes are seselected.
***************************************************************************/
 function DeSelectCheck(checkBoxID)
 {    
    document.forms[0].elements[checkBoxID].checked = false;
 }
 /************************************************************************
   To Check Whether all the header columns are selected in Map results.
**************************************************************************/
 function IsSelectAllColumnsMap(tableName)
 {
    try
    {    
        document.getElementById(GetObjectID('hidSelectedColumns','input')).value = "";
        var objectID = "";
        var resultID = "";
        for(index = 0; index < window.parent.document.documentElement.getElementsByTagName("table").length; index++)
        {
            objectId = window.parent.document.documentElement.getElementsByTagName("table").item(index).id;
            if(objectId.indexOf(tableName)>=0)
            {
                resultID = objectId;
                break;
            }
        }
        var table = document.getElementById(resultID);
        var row = table.rows;
        var checkBoxCounter = 0;
        var boolChecked = false;
        var column = row[1].cells;
        if(row[1].cells[0].innerHTML.indexOf("CHECKED")>=0)
	    {
	        document.getElementById(GetObjectID('hidSelectedColumns','input')).value = "ALLCOLUMNS";
		    boolChecked = true;
        }
        else
        {
            for(i = 1; i < column.length; i++)
            {
                if(row[1].cells[i].innerHTML.indexOf("CHECKED")>=0)
	            {
	                document.getElementById(GetObjectID('hidSelectedColumns','input')).value = document.getElementById(GetObjectID('hidSelectedColumns','input')).value + row[2].cells[i].innerText + '|';
		            boolChecked = true;             
                }
            }
	    }
        if(boolChecked == false)
        {        
            return false;      
        }
        else
        {
            return true;
        }
    }
    catch(Ex){}  
}
/*****************************************************
    Confirmation message for Deleting the save Search
*****************************************************/
function DeleteSaveSearch(SearchType, SaveSearchName)
{    
    var isConfirmed = confirm("Do you really want to delete this Save Search?")
    if(isConfirmed == true)
    {
        document.getElementById(GetObjectID('hidDeleteSearchName','input')).value = SaveSearchName;
        document.getElementById(GetObjectID('hidSearchType','input')).value = SearchType;
        return true;
    }
    else
    {
        return false;
    }
}

/*****************************************************
    Opening the UI page for modifing saved Searches
*****************************************************/
function ModifySaveSearch(SiteURL, SaveSearchName)
{
    var strUrl = SiteURL + "?savesearchname=" + SaveSearchName + "&operation=modify&manage=true" ;
    window.parent.OpenPageInContentWindow(strUrl);
}

 /***************************************
    function to set the column width fixed
***************************************/
function SetColumnWidth(tblID)
{    
    var table = document.getElementById(tblID);	
	var cTR = table.getElementsByTagName('TR');     
    
    for (i = 0; i < cTR.length; i++)
	{		
	    var tr = cTR.item(i);        
        tr.cells[0].style.width = "30px";           
    }    
}

 /***************************************
    function to clear the hidden field listClick
***************************************/
function ClearListClick()
{
    if(document.getElementById(GetObjectID('hidListClick','input')) != null)
        document.getElementById(GetObjectID('hidListClick','input')).value = "";
}


 /***************************************
    function to select the identifiers in MAP
***************************************/
function GetAllMapSelectedRowIndex(columnIndex)
{ 
    document.getElementById(GetObjectID('hidGridZoomSelectedItems','input')).value = '|';
    document.getElementById(GetObjectID('hidGridZoomValues','input')).value = '|';    
    var objectID = "";
    var resultID = "";
    for(index = 0; index < window.parent.document.documentElement.getElementsByTagName("table").length; index++)
    {
        objectId = window.parent.document.documentElement.getElementsByTagName("table").item(index).id;
        if(objectId.indexOf("tblSearchResults")>=0)
        {           
            resultID = objectId;
            break;
        }
    }
    var table = document.getElementById(resultID);    
    var row = table.rows;
    var checkBoxCounter = 0;
    var boolChecked = false;
    for(i = 2; i < row.length; i++)
	{
	    if(i == 2)
	    {
            if(row[2].cells[0].innerHTML.indexOf("CHECKED")>=0)
	        {
	            document.getElementById(GetObjectID('hidGridZoomSelectedItems','input')).value = "SELECTALL";	            
	        }
		}
		else
		{
		    if(row[i].cells[0].innerHTML.indexOf("CHECKED")>=0)
			{
			    boolChecked = true;
			    if(document.getElementById(GetObjectID('hidGridZoomSelectedItems','input')).value != "SELECTALL")
	            {
	                document.getElementById(GetObjectID('hidGridZoomSelectedItems','input')).value = document.getElementById(GetObjectID('hidGridZoomSelectedItems','input')).value + i + '|';
	            }	            
                document.getElementById(GetObjectID('hidGridZoomValues','input')).value = document.getElementById(GetObjectID('hidGridZoomValues','input')).value + row[i].cells[columnIndex].innerText + '|';
                
            }
		}
	}
	if(boolChecked == false)
    {
        alert('Please select minimum one record.');  
        return false;
    }
    else
    {
        return true;
    }
}

 /***************************************
    function to zoom the selected records MAP
***************************************/
function ShowGridZoomSelectedRecords()
{    
    var objectID = "";
    var resultID = "";
    for(index = 0; index < window.parent.document.documentElement.getElementsByTagName("table").length; index++)
    {
        objectId = window.parent.document.documentElement.getElementsByTagName("table").item(index).id;
        if(objectId.indexOf("tblSearchResults")>=0)
        {           
            resultID = objectId;
            break;
        }
    }
    var table = document.getElementById(resultID);    
    var row = table.rows;    
    var strZoomSelectedItems = document.getElementById(GetObjectID('hidGridZoomSelectedItems','input')).value;
    if(strZoomSelectedItems == "SELECTALL")
    {
        document.forms[0].elements['HeaderButton'].checked = true;
        CheckAll(document.forms[0].elements['HeaderButton']);
    }
    else
    {
        var arrSelectedItems = strZoomSelectedItems.split("|");
        var index = document.forms[0].elements['HeaderButton'].name.lastIndexOf('_'); 
        var prefix = document.forms[0].elements['HeaderButton'].name.substr(0,index);
        for (i = 0; i < arrSelectedItems.length; i++)
        {            
            if(TrimString(arrSelectedItems[i]).length > 0)
            {
                for(j=0; j<document.forms[0].length; j++) 
                { 
                    var o = document.forms[0][j]; 
                    if (o.type == 'checkbox') 
                    { 
                        if (row[Number(arrSelectedItems[i])].cells[0].innerHTML.indexOf(o.name) > 0) 
                        {                        
                            var tmp = o.name.substring(0, prefix.length);
                            // Replace all occurances of $ with _. The g in the regular expression means globally.
                            if(o.name.indexOf("CheckBoxButton")>0)
                            {
                                var tmp1 = tmp.replace(/\$/g,"_");
                                if (tmp1 == prefix) 
                                {                                
                                    o.checked = true;
                                }
                            }
                        }
                    } 
                } 
            }
        }
    } 
}

 /***************************************
    function to enable criteria - Query Builder
***************************************/
function EnableCriteria(dropdownlist)
{    
try
{
    var objCriteria = dropdownlist.parentElement.parentElement.children[3].children[0];
    
    if(dropdownlist.selectedIndex > 0)
                {
                    objCriteria.disabled = false;
                    objCriteria.focus();
                }
                else
                {
                    objCriteria.value = "";
                    objCriteria.disabled = true;                
                }
   }
   catch(Ex)
   {
   
   }
}

 /***************************************
    function to enable the Query Search Criteria - Query Builder
***************************************/
function EnableQueryCriteria()
{
    var objGridView =document.getElementById(GetObjectID('tblColumnNames','table'));
    var ddlOperatorCollection ;
    if(objGridView!=null)
      {
         ddlOperatorCollection =objGridView.getElementsByTagName("select");                
      }
      else
      {              
        return;
      }

    for(var i=0; i<ddlOperatorCollection.length;i++)
    {             
        if(ddlOperatorCollection[i].selectedIndex == 0)
            {
               ddlOperatorCollection[i].parentElement.nextSibling.childNodes[0].disabled =true;
            } 
            else
            {
            ddlOperatorCollection[i].parentElement.nextSibling.childNodes[0].disabled =false;
            }
    }
}

 /***************************************
    function to check / uncheck the checkboxs - Query Builder 
***************************************/
function CheckUnCheckAllColumns(gridView,headerCheckBox)
{
    var CheckStatus;
    if(headerCheckBox.checked)
       { 
        CheckStatus = true;
        }
    else
        {
        CheckStatus = false;
        }
    var grid = document.getElementById(GetObjectID(gridView,'table'));    
    for(i=0;i<grid.rows.length;i++)
    {
        var Row = grid.rows[i];
        var cellCheckBox = Row.cells[0];  
        
            var chbColumnName = cellCheckBox.firstChild;
            chbColumnName.checked = CheckStatus;    
        
    }
}

 /***************************************
    function to check / uncheck the header checkbox - Query Builder
***************************************/
function UnCheckHeader(gridView,objBox)
{
    var chbHeaderColumn = document.getElementById(GetObjectID('chbHeaderColumn','input')); 
    chbHeaderColumn.checked = false;
}
 
   /************************************************
    Open Dialog for My Asset XML Generation
 *************************************************/
  function openMyAssetWindow()
 {
        var sRptType = getQuickSearchAssetType();
        if(sRptType=='undefined')
            sRptType = getCurrentReportType();
        
        if(sRptType == "Logs By Field Depth" || sRptType =="Project Archives")
           {
                sRptType = sRptType.replace(/\s/g, "_"); 
           }
           else if(sRptType == "ListofWellbores")
           {
                sRptType ="Wellbore";
           }
           else if(sRptType == "ListofWells")
           {
                sRptType ="Well";
           }
           
        if(GetAllSelectedRowIdentifiers(window))
        {
            var iWidth = 350;
            var iHeight = 150;
            var ileft = parseInt((screen.availWidth/2) - (iWidth/2));
            var itop = parseInt((screen.availHeight/2) - (iHeight/2));
            var sWindowFeatures = "width=" + iWidth + ",height=" + iHeight + ",status=no,resizable=no,left=" + ileft + ",top=" + itop + "screenX=" + ileft + ",screenY=" + itop;
            var objPopWin = window.open("","MyAsset", sWindowFeatures); 
            document.forms[0].action="/Pages/MyAssetPopup.aspx?PopUp=yes&operation=add&asset=" + sRptType;
            document.forms[0].method="post";
            document.forms[0].target= "MyAsset";
            document.forms[0].submit();    
            document.forms[0].target="_self";
            document.forms[0].action= objPopWin.opener.location.href;
            document.forms[0].method="post"; 
            objPopWin.focus();  
        }
       
 }
 
  /***************************************
    function to open the My Team Window
***************************************/
 function openTeamWindow()
 {

        var sRptType = getQuickSearchAssetType();
        if(sRptType=='undefined')
            sRptType = getCurrentReportType();
        
        if(sRptType == "Logs By Field Depth" || sRptType =="Project Archives")
           {
            sRptType = sRptType.replace(/\s/g, "_"); 
           }
           else if(sRptType == "ListofWellbores")
           {
                sRptType ="Wellbore";
           }
           else if(sRptType == "ListofWells")
           {
                sRptType ="Well";
           }
        
        if(GetAllSelectedRowIdentifiers(window))
        {
         var iWidth = 350;
         var iHeight = 100;

          var ileft = parseInt((screen.availWidth/2) - (iWidth/2));
          var itop = parseInt((screen.availHeight/2) - (iHeight/2));
          var sWindowFeatures = "width=" + iWidth + ",height=" + iHeight + ",status=no,resizable=no,left=" + ileft + ",top=" + itop + "screenX=" + ileft + ",screenY=" + itop;
          var objPopWin = window.open("",sRptType, sWindowFeatures); 
          
          document.forms[0].action="/Pages/MyTeamPopup.aspx?PopUp=yes&operation=add&asset=" + sRptType;
          document.forms[0].method="post";
          document.forms[0].target=sRptType;
          document.forms[0].submit();    
          document.forms[0].target="_self";
          document.forms[0].action= objPopWin.opener.location.href;
          document.forms[0].method="post"; 
          objPopWin.focus();  
        }
      
 }
 /***************************************************************
            function to open my asset delete window
****************************************************************/
function DeleteSelectedAsset(searchType)
{
    var sRptType = getQuickSearchAssetType();
    if(sRptType=="Project Archives")
    sRptType = "ProjectArchives";
    if(sRptType == null || sRptType == 'undefined')
    sRptType = searchType;                
    if(GetAllSelectedRowIdentifiers(window))
    {        
        var iWidth = 350;
        var iHeight = 100;
        var ileft = parseInt((screen.availWidth/2) - (iWidth/2));
        var itop = parseInt((screen.availHeight/2) - (iHeight/2));
        var sWindowFeatures = "width=" + iWidth + ",height=" + iHeight + ",status=no,resizable=no,left=" + ileft + ",top=" + itop + ",screenX=" + ileft + ",screenY=" + itop;

       //Dream 4.0 changes start
        /// My Asset Paging and Export all functionlity implemented with AJAX postback.
        //***Deleting selected rows
         DeleteSelectedRows();
        //Dream 4.0 changes end
        /// Call the AJAX Postback here
        /// After successful deletion of asset fire the popup from server side
        var strParams = 'PopUp=yes&operation=delete&asset=' + sRptType;//'pagenumber='+pageNumber+'&MaxRecords='+maxRecords+'&RecordCount='+recordcount+'&RequestId='+requestid+'&sortby='+sortBy+'&sorttype='+sortType+'&searchtype='+searchType;  
        try
        {

            __doPostBack(GetObjectID('UpdatePanelContentPage','div'),strParams);  
        }  
        //Dream 4.0 changes end
        catch(Ex)
        {
            alert(Ex.message);
        } 
    }       
}

  /***************************************
    function to delete the selected My Team Asset record
***************************************/
function DeleteSelectedTeamAsset(searchType)
{
    var sRptType = getQuickSearchAssetType();
    if(sRptType=="Project Archives")
    sRptType = "ProjectArchives";

    if(sRptType == null || sRptType == 'undefined')
    sRptType = searchType;  

    if(GetAllSelectedRowIdentifiers(window))
    {
        var iWidth = 350;
        var iHeight = 100;
        var ileft = parseInt((screen.availWidth/2) - (iWidth/2));
        var itop = parseInt((screen.availHeight/2) - (iHeight/2));
        var sWindowFeatures = "width=" + iWidth + ",height=" + iHeight + ",status=no,resizable=no,left=" + ileft + ",top=" + itop + ",screenX=" + ileft + ",screenY=" + itop;
        //Dream 4.0 changes start
        /// My Asset Paging and Export all functionlity implemented with AJAX postback.

        //***Deleting selected rows
         DeleteSelectedRows();
       
        var strParams = 'PopUp=yes&operation=delete&asset=' + sRptType;//'pagenumber='+pageNumber+'&MaxRecords='+maxRecords+'&RecordCount='+recordcount+'&RequestId='+requestid+'&sortby='+sortBy+'&sorttype='+sortType+'&searchtype='+searchType;  
        try
        {

            __doPostBack(GetObjectID('UpdatePanelContentPage','div'),strParams);  
        }  
        //Dream 4.0 changes end
        catch(Ex)
        {
            alert(Ex.message);
        }   
    }
}



  /***************************************
    function to delete the selected records
***************************************/
function DeleteSelectedRows()
{   

    var table = document.getElementById('tblSearchResults');   
    var row = table.rows;    
    for (i= row.length-1; i >=0 ; i--)
    {
        if ((row[i].childNodes[0].childNodes[0].type == 'checkbox')&&(row[i].childNodes[0].childNodes[0].checked == true))
        {
            table.deleteRow(i);
        }
    }
  
    if((table.rows.length==1)&&(table.rows[0].childNodes[0].tagName =="TH"))
    {
        table.deleteRow(0);//deleteing header row when all other rows are deleted
    }
    if(row.length==0)
    {
      var objDiv = document.getElementById(GetObjectID('_minusDiv','div'));
      if(objDiv!=null)
      {
        objDiv.parentNode.parentNode.parentNode.deleteRow(objDiv.parentNode.parentNode.rowIndex);
      }
      HideShowContextSrchMenu(window.parent,'none');
    }
    else
    {
     SetAlternateColor("tblSearchResults");
    }
    //if al the nodes are deleted then show message
    ShowMyAssetMessage();
}
/***************************************************************
            function to show message when all asset are deleted by user
****************************************************************/
function ShowMyAssetMessage()
{
    var objAssetTable = document.getElementById('tblAssetContainer');
    var objTdTitle ;
    if((objAssetTable!=null)&&(objAssetTable.rows.length==0))
    {
        objTdTitle = document.getElementById('tdTitle');
        if(objTdTitle!=null)
        {
            objTdTitle.innerText ="You have not saved any Assets yet.";
            objTdTitle.className = "";           
            objTdTitle.style.color ="red";
            objTdTitle.parentNode.parentNode.parentNode.className ="";
            objAssetTable.parentNode.className ="";
            document.getElementById('divStandardSearch').childNodes[0].className = "tableAdvSrchBorder";           
        }
    }   
}
/***************************************************************
            function to get Quick Search Asset Type
****************************************************************/
function getQuickSearchAssetType()
{
	 // get the current URL
	 var url = window.location.toString();
	 //get the parameters
	 url.match(/\?(.+)$/);
	 var params = RegExp.$1; 
	 // split up the query string and store in an
	 // associative array 	 
	 var queryStringList = {};
	 var params = params.split("&");
	 
	 for(var i=0;i<params.length;i++)
	 {
	 	var tmp = params[i].split("=");
	 	queryStringList[tmp[0]] = tmp[1];
	 }
	 return (unescape(queryStringList["asset"]).replace("#",''));	 
}

/***************************************
   opens the My Assets page 
***************************************/    
 function openMyAssets(sType,objDiv)
 {
	if(document.getElementById(objDiv.id).parentElement.all['minusDetDiv'] != null)
	{
        if(document.getElementById(objDiv.id).parentElement.all['minusDetDiv'].style.display == "none")
        {
            document.getElementById(objDiv.id).parentElement.all['minusDetDiv'].style.display="block";
            document.getElementById(objDiv.id).src = '/_layouts/DREAM/images/minus.gif';
            HideShowContextSrchMenu(window.parent,'block');
        }  
        else
        {
            document.getElementById(objDiv.id).parentElement.all['minusDetDiv'].style.display="none";
            document.getElementById(objDiv.id).src = '/_layouts/DREAM/images/plus.gif';
            HideShowContextSrchMenu(window.parent,'none');
        }
	}
	else
	{
	//Dream 4.0 changes start
	  window.parent.__doPostBack(GetClientID(window.parent,'updtPanelLeftNaV','div'),sType);
	  __doPostBack(GetObjectID('UpdatePanelContentPage','div'),sType);  
	  //Dream 4.0 changes end
	}
	 
 }
 
  /***************************************
    function to open My Team Asset Search results expand / collapse window
***************************************/
function openMyTeamAssets(sType,objDiv)
{
    if(document.getElementById(objDiv.id).parentElement.all['minusDetDiv'] != null)
    {
        if(document.getElementById(objDiv.id).parentElement.all['minusDetDiv'].style.display == "none")
        {
          document.getElementById(objDiv.id).parentElement.all['minusDetDiv'].style.display="block";
          document.getElementById(objDiv.id).src = '/_layouts/DREAM/images/minus.gif';
          HideShowContextSrchMenu(window.parent,'block');
        }  
        else
        {
        document.getElementById(objDiv.id).parentElement.all['minusDetDiv'].style.display="none";
        document.getElementById(objDiv.id).src = '/_layouts/DREAM/images/plus.gif';
        HideShowContextSrchMenu(window.parent,'none');
        }
    }
    else
    {
    //Dream 4.0 changes start
       window.parent.__doPostBack(GetClientID(window.parent,'updtPanelLeftNaV','div'),sType);
	   __doPostBack(GetObjectID('UpdatePanelContentPage','div'),sType); 
	   //Dream 4.0 changes end 
    }
 
}
 /***********************************************************
 Query Builder  Validation Scripts
 /***********************************************************/
 function ValidateCriteria()
 {
    var bCriteriaStatus=false;
    var bDisplayStatus=false;
 
    var objGrid = document.getElementById(GetObjectID('tblColumnNames','table'));   
   
    if(!objGrid.disabled)
    {
        for(i=0;i<objGrid.rows.length;i++)
        {
            var objRow = objGrid.rows[i];
            var objCellCheckBox = objRow.cells[0]; 
            var objCellDropDown = objRow.cells[2];
            var objCellCriteria = objRow.cells[3];
             
           
                var objChbDrpDown = objCellDropDown.firstChild;
                var objChbCheckBox = objCellCheckBox.firstChild;
                var objTxtCriteria = objCellCriteria.firstChild;
                
                if(objChbDrpDown.selectedIndex>0)
                    {
                        if((objTxtCriteria.value.match(/^\s+$/)==null)&&(!isBlankspace(objTxtCriteria.value)))
                        {
                        bCriteriaStatus=true;
                        }
                    }
                    
                if(objChbCheckBox.checked)
                    {
                    bDisplayStatus=true;
                    }
            
         }
        
        if(bCriteriaStatus==false)
        {
            alert('Please select at least one criteria.');
            return false;
        }
        
        else if(bDisplayStatus==false)
        {
            alert('Please select at least one display column name.');
            return false;
        }
        else
        {
          return true;
        }       
    }
    else
    {
        return true;
    }   
    
 }

  /***************************************
    function to validate the query search criteria - query builder 
***************************************/
 function ValidateQuerySearchCriteria()
 {
    if(ValidateCriteria())
        {
            var objGrid = document.getElementById(GetObjectID('tblColumnNames','table'));   
            if(objGrid.disabled)
            {
                var objQueryTextBox = document.getElementById(GetObjectID('txtSQLQuery', 'textarea'));         
                        
                if(isBlankspace(objQueryTextBox.value))
                {            
                alert("Please enter valid SQL query.");
                HighlightTextError(objQueryTextBox);            
                return false;
                }
            }
            else
            {
                return true;
            }
        }
    else
        {
           return false;
        }
 }
 
/***************************************
    function to validate the query save search criteria - query builder 
***************************************/
function ValidateQuerySaveSearchCriteria()
{

    if(ValidateCriteria())
    {
     var objGrid = document.getElementById(GetObjectID('tblColumnNames','table'));   
    if(objGrid.disabled)
    {
        var objQueryTextBox = document.getElementById(GetObjectID('txtSQLQuery', 'textarea'));         
                    
        if(isBlankspace(objQueryTextBox.value))
        {            
            alert("Please enter valid SQL query.");
            HighlightTextError(objQueryTextBox);            
            return false;
        }
    }
     var objTextBox = document.getElementById(GetObjectID('txtSaveSearchName', 'input'));
        if(isBlankspace(objTextBox.value))
        {            
            alert("Please enter search name.");
            HighlightTextError(objTextBox);            
            return false;
        }
        else if(!isSplCharacter(objTextBox.value))
        {
            alert("Invalid Search Name. Please enter only alphanumeric values and special characters like hyphen or underscore.");
            objTextBox.value = "";
            objTextBox.focus();
             return false;
        }
        else
        {	
            return true;
        }
    }
    else
    {
       return false;
    }

}
 
 /***************************************
    function to load the General / SQL Query Search options - Query Builder  
***************************************/
 function SelectQuerySearch(objSelect)
 {
 alert(objSelect.value);
if(objSelect.value=='General Query Search' || objSelect.value=='Sql Query Search')
        {
        return false;
        }
        else
        {
        //Dream 4.0 changes start
           __doPostBack(GetObjectID('cboSavedSearch','select'),'');
           //Dream 4.0 changes end
        }
 }
 
 /***************************************
    function to apply style to query save search combo box - query builder 
***************************************/
function ApplyStyleQrySrchCombo()
{

    var objSelect = document.getElementById(GetObjectID('cboSavedSearch', "select"));
    var sSelectedItem = objSelect.value;
    objSelect.selectedIndex =-1;

    var objGroupGen = document.createElement('optgroup');
         objGroupGen.label="General Query Search";
         
    var objGroupSql = document.createElement('optgroup');
         objGroupSql.label = "Sql Query Search";
    var bGen=false;
    var bSql=false;


    for(i=0;i< objSelect.options.length;i++)
    {
    var objOption = document.createElement('option');
    objOption.value= objSelect.options[i].value;
    objOption.innerText= objSelect.options[i].innerText;


    if(objOption.value=="---Select---")
        continue;
        
    objSelect.options.remove(i);
    i=i-1;

     if(objOption.value=="General Query Search")
        {
        bGen=true;
       
        }
      if(objOption.value=="Sql Query Search")
        {
         bGen=false;
         bSql=true;
        
        }
        
        
            
        if((objOption.value!="Sql Query Search")&&(objOption.value!="General Query Search"))
        {
            if(bGen)
            {
            objGroupGen.appendChild(objOption);
            }
             if(bSql)
            {
            objGroupSql.appendChild(objOption);
            }
        }
    }

    if(objGroupGen.children.length>0)
        objSelect.appendChild(objGroupGen);
    if(objGroupSql.children.length>0)
       objSelect.appendChild(objGroupSql);
       
    objSelect.value = sSelectedItem;
}
 /************************End of Query Builder Module***********************************/
 
 /************************EP CATALOG MODULE ********************************************/
  /***************************************
    function to open the EPCatalogue Filter Pop Up
***************************************/
function EPContextFilterPopup(contentWindow,url,searchType,assetType)
{
    var quickSearchForm = contentWindow.document.forms[0];
    var iWidth = 800;//500;
    var iHeight =600;// 400;
    var ileft = parseInt((screen.availWidth/2) - (iWidth/2));
    var itop = parseInt((screen.availHeight/2) - (iHeight/2));
    var sWindowFeatures = "width=" + iWidth + ",height=" + iHeight + ",scrollbars=yes,resizable=yes,status=yes,left=" + ileft + ",top=" + itop + "screenX=" + ileft + ",screenY=" + itop;
    var gEPCatalogFilterWindow = contentWindow.open('',searchType,sWindowFeatures);
    quickSearchForm.action=url + '?SearchType=' + searchType + '&assetType=' + assetType;
    quickSearchForm.method="post";
    quickSearchForm.target=searchType;
    quickSearchForm.submit();
    quickSearchForm.target="_self";
    quickSearchForm.action= gEPCatalogFilterWindow.opener.location.href;
    quickSearchForm.method="post"; 
    gEPCatalogFilterWindow.focus();
}

  /***************************************
    function to load the EPCatalogue Search Results Pop Up
***************************************/
 function LoadSearchResultsPopup(url,searchType,assetType)
 { 
     
      var iWidth = 800;
      var iHeight = 600;
      var ileft = parseInt((screen.availWidth/2) - (iWidth/2));
      var itop = parseInt((screen.availHeight/2) - (iHeight/2));
      
      setWindowTitle('Document Search Result');
      window.moveTo(ileft,itop); 
      window.resizeTo(iWidth,iHeight); 
      
      document.forms[0].target="_self";
      document.forms[0].action= url + "?SearchType=" + searchType + "&assetType=" + assetType;
      document.forms[0].method="post"; 
      document.forms[0].submit();
 }
 /************************FUNCTIONALITY USAGE REPORT MODULE ********************************************/
 /*Added By dev for functionality usage report
 /************************************************************
        function to deselect date options in functionality usage report
************************************************************/
function resetFunctionalityUsageDateTable()
{
    if (document.getElementById(GetObjectID('rbSelectDates', 'input')).checked == true)
    {
        EnableDisableDates('rbSelectDates');
    }
    document.getElementById(GetObjectID('rbCurrentMonth', 'input')).checked = false;
    document.getElementById(GetObjectID('rbLast6Month', 'input')).checked = false;
    document.getElementById(GetObjectID('rbCurrentYear', 'input')).checked = false;
    document.getElementById(GetObjectID('rbLast1Year', 'input')).checked = false;
    document.getElementById(GetObjectID('rbLast2Year', 'input')).checked = false;
    document.getElementById(GetObjectID('rbSelectDates', 'input')).checked = false;
    document.getElementById(GetObjectID('txtStartDate', 'input')).value = "";
    document.getElementById(GetObjectID('txtEndDate', 'input')).value = "";    
}
/************************************************************
        function to reset  functionality usage report
************************************************************/
function  ResetFunctionalityUsage()
 {
  
    ResetListBox(GetObjectID('lstFURSearchName', 'select'));   
    ResetListBox(GetObjectID('lstUserName', 'select'));
    resetFunctionalityUsageDateTable();
    return false;
 }
 /************************************************************
        function to unselect all for a listbox
************************************************************/
 function ResetListBox(objListBoxID)
 {
 
   var  objListBox =document.getElementById(objListBoxID);
    for (var index = 0; index < objListBox.length; index++)
        {
            objListBox.options[index].selected = false;
        }

    return true;
 
 }
 /************************************************************
        function to Validate search criteria for functionality usage report
************************************************************/
 function ValidateFURSearchCritetia()
 {
    if(IsListBoxItemSelected(GetObjectID('lstFURSearchName', 'select')))
        {
            if (document.getElementById(GetObjectID('rbSelectDates', 'input')).checked)
                {
                    return ValidateFURDates();
                }
            else
                {     
                    
                    return true;
                }
        }
  
    else if(IsListBoxItemSelected(GetObjectID('lstUserName', 'select')))
        {
            
        }
    else if(IsDateSelected())
        {            
           if (document.getElementById(GetObjectID('rbSelectDates', 'input')).checked)
                {
                    return ValidateFURDates();
                }
            else
                {                    
                    return true;
                }
        }
    else
        {
            alert('Select one or more fields!');
            return false;
        }
 }
 
   /***************************************
    function to check if List Box Item is selected.
***************************************/
 function IsListBoxItemSelected(objListBoxID)
 {
  var  objListBox =document.getElementById(objListBoxID);
    for (var index = 0; index < objListBox.length; index++)
        {
            if(objListBox.options[index].selected)
            {
              return true;
            }
        }

    return false;
 }
 
    /***************************************
    function to check if Date is selected
***************************************/
 function IsDateSelected()
 {
  
   if(document.getElementById(GetObjectID('rbCurrentMonth', 'input')).checked)
        {
            return true;
        }
   if(  document.getElementById(GetObjectID('rbLast6Month', 'input')).checked)
        {
            return true;
        }
   if(document.getElementById(GetObjectID('rbCurrentYear', 'input')).checked) 
        {
            return true;
        }
   if(document.getElementById(GetObjectID('rbLast1Year', 'input')).checked)
        {
            return true;
        }
   if(document.getElementById(GetObjectID('rbLast2Year', 'input')).checked)
        {
            return true;
        }
   if (document.getElementById(GetObjectID('rbSelectDates', 'input')).checked)
        {
            return true;
        }
 }
  
/***************************************************************
            function to validate FUR start and end date
***************************************************************/
function ValidateFURDates()
{  
  return CallValidateDateService("txtStartDate","txtEndDate");
}

 /************************END OF FUNCTIONALITY USAGE REPORT MODULE ********************************************/
 
 
 
 /*******************************************Registry entry function****************************/
    
/***************************************
    function to modify the Registry Entry
***************************************/
 function ModifyRegisteryEntry()
        {
       
            var ws = new ActiveXObject("WScript.Shell");
            if (ws != null)
            {
                for (var index = 0; index < 5; index++)
                {
               
                    var key = ws.RegRead("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Zones\\" + index

+ "\\1609");
                    if ((key != null) && (key != 0))
                    {
                        ws.RegWrite("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\Zones\\" +

index + "\\1609", 0, "REG_DWORD");
                    }
                }
            }
        }
        
        
/********************** New functions added for dynamic context search  **********************************************//////////

/***************************************
    Opening Detail Reports from all first level reports
***************************************/
function OpenDetailReport(contextSearchName)
{
    if(contextSearchName == "Recall Logs Report")
    {
       if(GetLogSelectedRowIdentifiers())
        {
            ContextSearchPopup('/pages/RecallCurve.aspx?SearchType=RecallCurve','RecallCurve');
        } 
    }
    else
    {
        if(GetAllSelectedRowIdentifiers(window))
        {     
            if(contextSearchName == "Picks Search Report")
            {
                ContextSearchPopup('/pages/PicksDetail.aspx?SearchType=PicksDetail','PicksDetail');
            }
            else if(contextSearchName == "Directional Survey Report")
            {
                ContextSearchPopup('/pages/DirectionalSurveyDetail.aspx?SearchType=DirectionalSurveyDetail','DirectionalSurveyDetail');
            }
            else if(contextSearchName == "Time Depth Report")
            {
                ContextSearchPopup('/pages/TimeDepthDetail.aspx?SearchType=TimeDepthDetail','TimeDepthDetail');
            }
        }
    }
}
/*Opening Context Search Reports for web services results.*/
function OpenContextReports(contextSearchName,url)
{
    //search names which are applicable for single id at a time
    var contentWindow = GetContentWindow();
    var strSearchNames = "WellHistory|WellTestReport|DailyWellsReporting|MechanicalData|PressureSurveyData|WellSummary|SignificantWellEvents|WellReviews";
   //To get the RecallLogsURL for RecallWebBrowser 
   if(contextSearchName == "RecallWebBrowser")//Dream 4.0 code    
       {
           var hidRecallLogsURL = GetHTMLObject(contentWindow,'hidRecallLogsURL','input');
           window.open(hidRecallLogsURL.value);
       }
    //Reports applicable for one asset at a time,not multiple
//    else if(strSearchNames.indexOf(contextSearchName) >= 0)
//    {
//        if(GetEDMSelectedRowIdentifiers())
//        {
//            ContextSearchPopup(url,contextSearchName);
//        }
//    }  
    else
    { 
        //var contentWindow = GetContentWindow();
        if(GetAllSelectedRowIdentifiers(contentWindow))
        { 
            if((contextSearchName == "ListofWells") || (contextSearchName == "ListofWellbores")||(contextSearchName == "ListOfReservoirs") ||(contextSearchName == "ListofFields"))
            {
            //Dream 4.0 changes start
                __doPostBack(GetObjectID('updtPanelLeftNaV','div'),contextSearchName);
                DisplayList(url,contextSearchName);
                //Dream 4.0 changes end
            }
            else
            {
                ContextSearchPopup(url,contextSearchName);
            }
        }
    }    
}


/**********************Functions For RecallLogs**********************************************//////////

/*Opening recalllogs report.*/
var strRecallLogsURL;
function OpenRecallLogsReport(URL,reportType)
{
strRecallLogsURL =URL;

if(ValidateRecallLogsSelection(reportType))
    {        
         window.open(strRecallLogsURL);
    }
}
/*************************************************************
    Validate the report selection for RecallLogs.
*************************************************************/
function ValidateRecallLogsSelection(reportType)
{   
    var arrCheckedRows = $("table#tblSearchResults tbody tr:has(input:checked)",window.document);
    if(arrCheckedRows.length <= 0)
    {
        alert('Please select one record.');
        return false;
    }
    else if(arrCheckedRows.length > 1)
    {
        alert('Please select only one record.')
        return false;
    }
    CreateExternalRecallURL(arrCheckedRows[0]);
    return true;
}
/************************************************************
        Opening the RecallLogs Popup Window
************************************************************/
function openRecallLogsPopup(URL)
{     
    window.open(URL);
}
//Dream 4.0 changes start
/************************************************************
        Function to get index of a column in a table
************************************************************/
function GetColumnIndex(columnName)
{
    var arrColumn =$("table#tblSearchResults thead tr th:contains('"+ columnName + "')");
    var columnIndex = -1;
    if(arrColumn.length > 0)
        columnIndex = arrColumn[0].cellIndex;
    return columnIndex;
}
/************************************************************
        Function to Create External Recall URL string
************************************************************/
function CreateExternalRecallURL(row)
{
    var patt=/%([a-z]|\s)+%/gi; 
    var arrMatches =  strRecallLogsURL.match(patt);
    if(arrMatches == null)
        return;
    var colIndex = -1;
    for(var index =0;index < arrMatches.length; index++)
    {
        colIndex = GetColumnIndex(arrMatches[index].substring(1, (arrMatches[index].length-1)))
        if(colIndex != -1)
        {
            if(!isNaN(row.cells[colIndex].innerText))
            {
                strRecallLogsURL = strRecallLogsURL.replace(arrMatches[index],parseInt(row.cells[colIndex].innerText));
            }
            else
            {
                strRecallLogsURL = strRecallLogsURL.replace(arrMatches[index],row.cells[colIndex].innerText);
            }
        }
    }
}
//Dream 4.0 changes end
/************************************************************
        Opening the RecallLogs Popup Window
************************************************************/
function CreateRecallLogsURL(row)
{  
    strRecallLogsURL=strRecallLogsURL.replace("%uwbi%",row.cells[1].innerText);
    strRecallLogsURL= strRecallLogsURL.replace("%fldnm%",row.cells[3].innerText);
    strRecallLogsURL=strRecallLogsURL.replace("%projnm%",row.cells[14].innerText);
    //logdetails
    strRecallLogsURL=strRecallLogsURL.replace("%logsrvc%",row.cells[4].innerText);
    strRecallLogsURL=strRecallLogsURL.replace("%logtyp%",row.cells[5].innerText);
    strRecallLogsURL=strRecallLogsURL.replace("%logsrc%",row.cells[6].innerText);
    strRecallLogsURL=strRecallLogsURL.replace("%lognm%",row.cells[7].innerText);
    strRecallLogsURL=strRecallLogsURL.replace("%logact%",row.cells[11].innerText);
    strRecallLogsURL=strRecallLogsURL.replace("%logrn%",row.cells[12].innerText);
    strRecallLogsURL=strRecallLogsURL.replace("%logvrsn%",parseInt(row.cells[13].innerText));
}
/***************************************
    Create Recall Curve Report
***************************************/
function CreateRecallCurveReport(row)
{

    strRecallLogsURL=strRecallLogsURL.replace("%uwbi%",row.cells[1].innerText);
    strRecallLogsURL= strRecallLogsURL.replace("%fldnm%",row.cells[3].innerText);
    strRecallLogsURL=strRecallLogsURL.replace("%projnm%",row.cells[13].innerText);
    //logdetails
    strRecallLogsURL=strRecallLogsURL.replace("%logsrvc%",row.cells[4].innerText);
    strRecallLogsURL=strRecallLogsURL.replace("%logtyp%",row.cells[5].innerText);
    strRecallLogsURL=strRecallLogsURL.replace("%logsrc%",row.cells[6].innerText);
    strRecallLogsURL=strRecallLogsURL.replace("%lognm%",row.cells[7].innerText);
    strRecallLogsURL=strRecallLogsURL.replace("%logact%",row.cells[10].innerText);
    strRecallLogsURL=strRecallLogsURL.replace("%logrn%",row.cells[11].innerText);
    strRecallLogsURL=strRecallLogsURL.replace("%logvrsn%",parseInt(row.cells[12].innerText));
    //Curve Details
     strRecallLogsURL=strRecallLogsURL.replace("%crvnm%",row.cells[14].innerText);
    strRecallLogsURL=strRecallLogsURL.replace("%crvtyp%",row.cells[15].innerText);
    strRecallLogsURL=strRecallLogsURL.replace("%crvsrvc%",row.cells[16].innerText);
    strRecallLogsURL=strRecallLogsURL.replace("%crvvrsn%",parseInt(row.cells[22].innerText));
   
}

/**********************Functions For EDM Report**********************************************//////////

/************************************************************
        populating start date and end date on the basis of time period selection
************************************************************/
function DateFiltersOptionsChange()
{

	var cboDateOptions= document.getElementById(GetObjectID("cboTimePeriod", "select"));
	var cellStartDate = document.getElementById(GetObjectID("txtStartDate", "input"));
	var cellEndDate = document.getElementById(GetObjectID("txtEndDate", "input"));

	if((cboDateOptions != null)&&(cellStartDate != null)&&(cellEndDate != null))
	{
		var dateRange = cboDateOptions.value.split(";");
		if(dateRange!=null)
		{
			if(dateRange.length==2)
			{
				cellStartDate.value = dateRange[0];
				cellEndDate.value = dateRange[1];
			}
			else
			{
			    cellStartDate.value = "";
				cellEndDate.value = "";
			}
		}
		else
		{
		        cellStartDate.value = "";
				cellEndDate.value = "";
		}
	}
}
/************************************************************
       this method enable/disable yes/no option on click of event check box
************************************************************/
function EnableDisabeYesNo()
{
	var tdYesNo= document.getElementById("tdYesNo");
	var tdReportLevel= document.getElementById("tdReportLevel");
	var reprortLevel = tdReportLevel.getElementsByTagName("input");
	var isEventSelected =false;
	for(i=0;i<reprortLevel.length;i++)
	{	
	    if((reprortLevel[i].type=="checkbox")&&(reprortLevel[i].nextSibling.innerText=="Event"))
	    {
	        if(reprortLevel[i].checked)
	        {
	        tdYesNo.disabled = false;
	        }
	        else
	        {
	        tdYesNo.disabled = true;
	        }
	        break;
	    }
	}
	
  
}
/************************************************************
       this method enable/disable report level check box on selection/rejection of tabular result report type
************************************************************/
function EnableDisableReportLevel()
{
    var tdReportLevel= document.getElementById("tdReportLevel");
    if(GetSelectedAssetType("rbLstDisplayFormat")=="Tabular")
    {
        tdReportLevel.disabled =false;
    }
    else
    {
        tdReportLevel.disabled =true;
    }  
}
/***************************************************************
            function to validate search criteria in EDM Report
***************************************************************/
function ValidateEDMReportSearchCriteria()
{
    if(ValidateEDMReportDates())
    {
      if(ValidateEventSelection())
      {
        document.getElementById("tdReportLevel").disabled =false;//enabling before it goes to server
        document.getElementById("tdYesNo").disabled =false;//enabling before it goes to server
        return true;
      }
      else
      {
        alert("Please select atleast one EDM item to view.");
        return false;
      }
    }
    else
    {
        return false;
    }
}
/***************************************************************
            function to validate dates in EDM Report
***************************************************************/
function ValidateEventSelection()
{
        var validSelection =false;
        var chbLstReportLevel= document.getElementById(GetObjectID("chbLstReportLevel", "table"));
	    var reportLevels =chbLstReportLevel.getElementsByTagName("input");
	    	  
	   if(GetSelectedAssetType("rbLstDisplayFormat")=="Tabular")
	   {
	    for(var i = 0; i < reportLevels.length; i++ )
            {      
                if(reportLevels[i].checked == true)
                    {
                        validSelection =true;
                        break;
                    }
            }        
	   }
	   else
	   {
	    validSelection =true;
	   }
	   return validSelection
}
//Dream 4.0 changes start
/***************************************************************
            function to validate dates in EDM Report
***************************************************************/
function ValidateEDMReportDates()
{  
   return CallValidateDateService("txtStartDate","txtEndDate");
}
//Dream 4.0 changes end
/***************************************************************
            function to validate dates in EDM Report
***************************************************************/
function OnSelectedDateChange()
{
 var strFromDate, strToDate;
 var isOptionMatched =false;
    strFromDate = document.getElementById(GetObjectID("txtStartDate", "input")).value
    strToDate = document.getElementById(GetObjectID("txtEndDate", "input")).value
    var cboDateOptions= document.getElementById(GetObjectID("cboTimePeriod", "select"));
   for(i=1;i<cboDateOptions.options.length;i++)
   {
    var dateRange = cboDateOptions.options[i].value.split(";");
		if(dateRange!=null)
		{
			if(dateRange.length==2)
			{
				if((strFromDate== dateRange[0])&&(strToDate== dateRange[1]))
				{
				    cboDateOptions.options[i].selected =true;
				    isOptionMatched =true;
				    break;
				}
			}
		}
   }
   if(!isOptionMatched)
   {
   cboDateOptions.options[0].selected =true;
   }   
 
}
/***************************************************************
            function to highlight/ hide EDM Report Tab
***************************************************************/
 function tabs(divID) 
 { 
    var strDivId ="tab-";
    var strLblId = "spanEDM";
    var objSpan ;
    var objDiv ;
    for(var i=0;i<3;i++)
    {
        strLblId  = "spanEDM" + i ;    
        strDivId  = "tab-" + i ;    
        objSpan  =document.getElementById(strLblId);
        objDiv =document.getElementById(strDivId);   
        if(objDiv.id ==divID)
	    {
	        objDiv.style.display = "block";
	        objSpan.style.fontWeight ="bold";
	        objSpan.style.color ="black";
	         
	    }
	    else
	    {
	        objDiv.style.display = "none";      
	        objSpan.style.fontWeight ="normal";
	        objSpan.style.color ="white";
	        
	    }
    }
   

}

/***************************************
    function to Highlight the Tab
***************************************/
function HighlightTab(spanID)
{
var strID    ="";
var objSpan ;
for(var i=0;i<3;i++)
  {
 	strID  = "spanEDM" + i ;
	objSpan  =document.getElementById(strID);
	if(objSpan.id ==spanID)
	{
	objSpan.style.fontWeight ="bold";
	objSpan.style.color ="black";
	}
	else
	{
	objSpan.style.fontWeight ="normal";
	objSpan.style.color ="white";
	}
  }   
}
/***************************************
    Print option for EDM Report.
***************************************/
function EDMprintContent(tableName)
{   
       var objWindow;
       var counter = 0;
       var isConfirmed = confirm("This option will print the records displayed on the current page.")
       if(isConfirmed == true)
       {		   
            var attributes="toolbar=yes,location=no,directories=yes,resizable,menubar=yes,";
            attributes+="scrollbars=yes,width=800, height=600, left=100, top=25";
            objWindow=window.open("/_layouts/dream/printEDMReport.htm","Print",attributes); 
            objWindow.opener =window.self; 
            objWindow.focus(); 
        }
        else
        {
           alert("Print option is enabled only for search results and details page.");
        }      
}
/***************************************
    Print option for Mechanical data Report.
***************************************/
function MDRprintContent(tableName)
{   
       var objWindow;
       var counter = 0;
       var isConfirmed = confirm("This option will print the records displayed on the current page.")
       if(isConfirmed == true)
       {		   
            var attributes="toolbar=yes,location=no,directories=yes,resizable,menubar=yes,";
            attributes+="scrollbars=yes,width=800, height=600, left=100, top=25";
            objWindow=window.open("/_layouts/dream/printMDReport.htm","Print",attributes);   
            objWindow.opener =window.self; 
            objWindow.focus();     
        }
        else
        {
           alert("Print option is enabled only for search results and details page.");
        }      
}
/************************************************
Exporting Search Results from report service...
*************************************************/
function ExportEDMSearchResults(obj)
  {

    alert('This option will only export the records displayed on the current page. If you have multiple pages you will have to export each page separately.'); 
   
    document.body.style.cursor = 'wait';
    String.prototype.Trim = function () 
       {
         return this.replace(/\|*\s*$/, "");
       }
        var  table = obj;
        var strID    ="";
       var counter =0;
        var arrTable =document.getElementsByTagName("table");	  
            try
            {
              
              var xls = new ActiveXObject("Excel.Application");
                  
              xls.Workbooks.add();      
              xls.Workbooks(1).WorkSheets.add(); 
              xls.Workbooks(1).WorkSheets.add(); 
              var rows ;
               for(var index=0;index<arrTable.length;index++)
	            {
	            
                    if(arrTable[index].id ==obj)
		                {
		                 
		                 strID  = "spanEDM" + counter ;
			             var xslSheet = xls.Workbooks(1).WorkSheets(counter+1);
			             xslSheet.Name = document.getElementById(strID).innerText; 	
			             	counter++;	           			             		           
		                 rows = arrTable[index].rows;
                            for (i = 0; i < rows.length; i++)
                                {
                                
                                     var column = rows[i].cells;

                                            for (j = 0; j < column.length; j++)
                                                {
                                                    if (i == 0)
                                                  {
                                                    xslSheet.Cells(i+1, j+1).FormulaLocal = column[j].innerText.Trim();
                                                    xslSheet.Cells(i+1, j+1).Font.Bold = true;
                                                    xslSheet.Cells(i+1, j+1).Interior.ColorIndex = 15;                                                   
                                                   }
                                                   else
                                                   {
                                                  
					                                 if(rows[0].cells[j].type!="" && rows[0].cells[j].type == "number" )
					                                    {
					                                         xslSheet.Cells(i+1, j+1).NumberFormat = "0.00";
					                                         xslSheet.Cells(i+1, j+1).HorizontalAlignment = -4152;//XlRight is the constant -4152 
					                                    }
					                                 else if(rows[0].cells[j].type!="" && rows[0].cells[j].type == "date" )
					                                     {
					                                         xslSheet.Cells(i+1, j+1).NumberFormat = GetDateFormat();
					                                         xslSheet.Cells(i+1, j+1).HorizontalAlignment = -4131;//xlLeft is the constant -4131 
                					                      
					                                     }
					                                 else
					                                     {
					                                      if(rows[0].cells[j].innerText.indexOf("Well")>=0)
					                                      {
					                                         xslSheet.Cells(i+1, j+1).NumberFormat = "0";
					                                      }
					                                         xslSheet.Cells(i+1, j+1).HorizontalAlignment = -4131;//xlLeft is the constant -4131 
					                                     }     
                                                     xslSheet.Cells(i+1, j+1).Value = column[j].innerText;
                                                         
                                                   }  
                                                   xslSheet.columns.AutoFit();                                                  
                                                }
                                         
                                  }  
                                 
                          }
                   }                    
                xls.visible = true;
            }
            catch (E)
            {
               alert("Either excel is not installed or Your browser security setting is not allowing to create excel object.");
            }
            document.body.style.cursor = 'auto';
 }
/************************************************
Exporting Search Results from report service...
*************************************************/
function ExportMDSearchResults()
  {

    alert('This option will only export the records displayed on the current page. If you have multiple pages you will have to export each page separately.'); 
   
    document.body.style.cursor = 'wait';
    String.prototype.Trim = function () 
       {
         return this.replace(/\|*\s*$/, "");
       }     
        var strTitle    ="";
            try
            { 
              var xls = new ActiveXObject("Excel.Application");
              xls.Workbooks.add();      
              for(var index=0;index<arguments.length-1;index++)
	            {
	              xls.Workbooks(1).WorkSheets.add();      
	            }
              var rows ;
              var counter =1;
               for(var index=0;index<arguments.length;index++)
	            {	
	                     
	             var arrTable =document.getElementById(arguments[index]);	
                    if(arrTable!=null)
		                {	
		                 var xslSheet = xls.Workbooks(1).WorkSheets(counter); 
	                     counter++; 	                   
		                    var arrSpan  = document.getElementsByTagName("span");	
		                    for(var j=0;j<arrSpan.length;j++)
		                        {
		                            if(arrSpan[j].className==arrTable.parentElement.parentElement.id)
		                                {
		                                    strTitle  = arrSpan[j].innerText;
		                                    break;
		                                }
		                        }			                       
			            
			              if(strTitle!="")
			                xslSheet.Name = strTitle; 			           
		                 rows = arrTable.rows;
                            for (i = 0; i < rows.length; i++)
                                {                                
                                    
                                            var column = rows[i].cells;

                                            for (j = 0; j < column.length; j++)
                                                {
                                                    if (i == 0)
                                                  {
                                                    xslSheet.Cells(i+1, j+1).FormulaLocal = column[j].innerText.Trim();
                                                    xslSheet.Cells(i+1, j+1).Font.Bold = true;
                                                    xslSheet.Cells(i+1, j+1).Interior.ColorIndex = 15;                                                   
                                                   }
                                                   else
                                                   {
                                                   if(rows[0].cells[j].type!="" && rows[0].cells[j].type == "number" )
					                                    {
					                                         xslSheet.Cells(i+1, j+1).NumberFormat = "0.00";
					                                         xslSheet.Cells(i+1, j+1).HorizontalAlignment = -4152;//XlRight is the constant -4152 
					                                    }
					                                 else if(rows[0].cells[j].type!="" && rows[0].cells[j].type == "date" )
					                                     {
					                                         xslSheet.Cells(i+1, j+1).NumberFormat = GetDateFormat();
					                                         xslSheet.Cells(i+1, j+1).HorizontalAlignment = -4131;//xlLeft is the constant -4131 
                					                      
					                                     }
					                                 else
					                                     {
					                                         xslSheet.Cells(i+1, j+1).NumberFormat = "@";
					                                         xslSheet.Cells(i+1, j+1).HorizontalAlignment = -4131;//xlLeft is the constant -4131 
					                                     }     
                                                     xslSheet.Cells(i+1, j+1).Value = column[j].innerText;
                                                         
                                                   }  
                                                   xslSheet.columns.AutoFit();                                                  
                                                }
                                         
                                  }        
                          }
                         
                   }                    
                xls.visible = true;
            }
            catch (E)
            {
               
                alert("Either excel is not installed or Your browser security setting is not allowing to create excel object.");
            }
            document.body.style.cursor = 'auto';
 }
 
 
 /********************************
 Function to sort tale column
 /*************************************/
function ExpandCollapse(imgTag)
{
      if(imgTag.parentNode.parentNode.nextSibling.style.display=="none")
      {
        imgTag.parentNode.parentNode.nextSibling.style.display="block";
        imgTag.src="/_LAYOUTS/DREAM/images/Minus.gif";
      }
      else
      {
        imgTag.parentNode.parentNode.nextSibling.style.display="none";
        imgTag.src="/_LAYOUTS/DREAM/images/plus.gif";
      }     
}
  
/***************************************
    function to sort the table
***************************************/
function StringTableSort(tblId,objColmn)
{
	var tblMain = document.getElementById(tblId);
	  var colInd =objColmn.cellIndex;
	    var type ="";
    if((objColmn.innerText.toLowerCase().indexOf("number of searches")>=0)||(objColmn.innerText.toLowerCase().indexOf("year")>=0))
    {
    type = "number";
    }
    else if (objColmn.innerText.toLowerCase().indexOf("month")>=0)
    {
    type ="month";
    }
    else
    {
    type ="string";
    }
	if(tblMain != null)
	{
		if(tblMain.rows.length>2)
		{		    	    
			if(tblMain.rows[0].cells.length>colInd)
			{			   
		        for(var m=0;m<tblMain.rows[0].cells.length;m++)
                {
                    if(m!=colInd && tblMain.rows[0].cells[m].childNodes[1] != null)
                    {
                    tblMain.rows[0].cells[m].childNodes[1].src="/_layouts/dream/images/UP_INACTIVE.GIF";
                    }
                }
        	    var ascending = true;
                if(tblMain.rows[0].cells[colInd].childNodes[1].src.search(/arrupa.gif/i)==-1)
                {
                tblMain.rows[0].cells[colInd].childNodes[1].src="/_layouts/dream/images/arrupa.gif";
                }
                else
                {
                tblMain.rows[0].cells[colInd].childNodes[1].src="/_layouts/dream/images/arrdowna.gif";
                ascending=false;
                }
                   for(var i=1;i<tblMain.rows.length;i++)
				    {
					    var pri=1;
					    var rowNum=tblMain.rows.length-i;
					    for(var j=2;j<rowNum+1;j++)
					    {
						    var priVal = tblMain.rows[pri].cells[colInd].innerText;
						    var curVal =tblMain.rows[j].cells[colInd].innerText;
						    if(type=="month")
						    {
							    var curDate = new Date(curVal.toLowerCase()+"01,1975");
							    var priDate = new Date(priVal.toLowerCase()+"01,1975");
							    if(ascending?(curDate>=priDate):(curDate<=priDate))
							    {
								    pri=j;	
							    }

						    }
						    else if((type=="string")?(ascending?(curVal>=priVal):(curVal<=priVal)):(ascending?(parseInt(curVal)>=parseInt(priVal)):(parseInt(curVal)<=parseInt(priVal))))
						    {
 							    pri=j;								
						    }								
					    }
    					
    					
					    for(var k=0;k<tblMain.rows[0].cells.length;k++)
					    {
						    var temp=tblMain.rows[rowNum].cells[k].innerHTML;
						    tblMain.rows[rowNum].cells[k].innerHTML=tblMain.rows[pri].cells[k].innerHTML;
						    tblMain.rows[pri].cells[k].innerHTML=temp;
					    }
					
				    } // for loop
			}//tblMain.rows[0].cells.length>colInd
		} // tblMain.rows.length>2
	}//tblMain != null
}// function
		
		
/********************************
 Function for Mechanical data report
 /*************************************/
function MDRExpandCollapse(imgTag) 
{ 
	if(imgTag.parentNode.parentNode.nextSibling.style.display=="none") 
	{ 
		imgTag.parentNode.parentNode.nextSibling.style.display="block"; 
		imgTag.src="/_layouts/dream/images/Minus.gif"; 
	} 
	else
	{ 
		imgTag.parentNode.parentNode.nextSibling.style.display="none"; 
		imgTag.src="/_layouts/dream/images/plus.gif"; 
	}
	var tblParent = imgTag.parentNode.parentNode.parentNode.parentNode;
	var parentRow = imgTag.parentNode.parentNode;
	if((parentRow.rowIndex+2)<tblParent.rows.length)
	{
		if(imgTag.src.toLowerCase().indexOf("minus.gif")!=-1)
		{			
			var secndryHeader = tblParent.insertRow(parentRow.rowIndex+2);
			for(var i=0;i<tblParent.rows[0].cells.length;i++)
			{
				if(tblParent.rows[0].cells[i].style.display!="none")
				{
					secndryHeader.insertCell(i);
					secndryHeader.cells[i].className="Header";
					secndryHeader.cells[i].innerHTML = tblParent.rows[0].cells[i].innerHTML;
				}
			}
		}
		else
		{
			tblParent.deleteRow(parentRow.rowIndex+2);
		}
	}
	
}

/***************************************
    function to show/hide the Tab
***************************************/
function TabClick(divId, lblId)
{
	document.getElementById(divId).style.display="";
	var parentCell=document.getElementById("parentCell");
	var srcElmnt =document.getElementById(lblId); 
	var tabCell=srcElmnt.parentNode.parentNode.parentNode.parentNode;
	
    srcElmnt.style.fontWeight="bold";	
	srcElmnt.style.color="black";
    
	for(var i=0;i<parentCell.childNodes.length;i++)
	{
		if(parentCell.childNodes[i].id != divId)
		{
			parentCell.childNodes[i].style.display="none";
		}
	}
	var labels =tabCell.getElementsByTagName("span");	
	
	for(var j=0;j<labels.length;j++)
	{
		if(labels[j]!=srcElmnt)
		{
			labels[j].style.fontWeight="normal";
			labels[j].style.color ="white";
		}
	}	
	
}

/***************************************
    function to show/hide the Tab
***************************************/
function MDRTabClick(divId)
{
	document.getElementById(divId).style.display="";
	var parentCell=document.getElementById("parentCell");
	var srcElmnt =document.getElementById('lbl'+divId); 
	var tabCell=srcElmnt.parentNode.parentNode.parentNode.parentNode;
	
    srcElmnt.style.fontWeight="bold";	
	srcElmnt.style.color="black";
    
	for(var i=0;i<parentCell.childNodes.length;i++)
	{
		if(parentCell.childNodes[i].id != divId)
		{
			parentCell.childNodes[i].style.display="none";
		}
	}
	var labels =tabCell.getElementsByTagName("span");	
	
	for(var j=0;j<labels.length;j++)
	{
		if(labels[j]!=srcElmnt)
		{
			labels[j].style.fontWeight="normal";
			labels[j].style.color ="white";
		}
	}	
	
}

/***************************************
    function to show/hide the column groups
***************************************/
function ShowHideColumnGroups(parentChkId,childChkId,parentGroup,childGroup,tblId)
{
	
	var chkParent = document.getElementById(parentChkId);
	var chkChild = document.getElementById(childChkId);		
	var tblParent = document.getElementById(tblId);
	if((chkParent.checked)&&(chkChild.checked))
	{
		for(var i=0;i<tblParent.rows.length;i++)
		{
			if(tblParent.rows[i].style.display == "none")
			{
				tblParent.rows[i].style.display = "";
			}

			for(var j=0;j<tblParent.rows[i].cells.length;j++)
			{
				if(tblParent.rows[i].cells[j].style.display == "none")
				{
					tblParent.rows[i].cells[j].style.display = "";
				}
			}
		}
	}
	
	else 
	{
		if(chkParent.checked)
		{
			HideGroupMembers(tblId,childGroup);
		}
		else if(chkChild.checked)
		{			    
			ShowNonEmpty(tblId,childGroup);				
		}
		else
		{
			HideGroupMembers(tblId,childGroup);
			HideGroupMembers(tblId,parentGroup);
		}
		RemoveDuplicateRows(tblId)
	}
}

/***************************************
    function to show/hide the column groups
***************************************/
function ShowHideColumnGroupsTest(tblId)
{
	
	var tblParent = window.opener.document.getElementById(tblId);
	
		for(var i=0;i<tblParent.rows.length;i++)
		{
			if(tblParent.rows[i].style.display == "none")
			{
				tblParent.rows[i].style.display = "";
			}

			for(var j=0;j<tblParent.rows[i].cells.length;j++)
			{
				if(tblParent.rows[i].cells[j].style.display == "none")
				{
					tblParent.rows[i].cells[j].style.display = "";
				}
			}
		}
	RemoveDuplicateRowsTest(tblId)
}

/***************************************
    function to remove the duplicate rows
***************************************/
function RemoveDuplicateRowsTest(tblId)
{
	
	var tblParent = window.opener.document.getElementById(tblId);
	for(var i=1;i<tblParent.rows.length;i++)
	{
		if(i<tblParent.rows.length-1)
		{
			if(tblParent.rows[i].style.display!="none")
			{
				for(var j=i+1;j<tblParent.rows.length;j++)
				{	
					var isDuplicate=true;
					for(var k=0;k<tblParent.rows[i].cells.length;k++)
					{
						if((tblParent.rows[i].cells[k].style.display!="none")&&(tblParent.rows[i].cells[k].innerHTML!=tblParent.rows[j].cells[k].innerHTML))
						{
							isDuplicate=false;
							break;
						}
					}
					if(isDuplicate)
					{
						tblParent.rows[j].style.display="none";
					}
				}
			}
		}			
	}
}

/***************************************
    function to hide the group members
***************************************/
function HideGroupMembersTest(tblId,grpName)
{
	var tblParent = window.opener.document.getElementById(tblId);
	var grpIndex=tblParent.rows[0].cells.length;
	
	for(var k=0;k<tblParent.rows[0].cells.length;k++)
	{
		if(tblParent.rows[0].cells[k].group==grpName)
		{
			grpIndex=k;
			break;
		}
	}
	for(var i=0;i<tblParent.rows.length;i++)
	{
		tblParent.rows[i].style.display="";
		for(var j=0;j<tblParent.rows[i].cells.length;j++)
		{
			if(j<grpIndex)
			{
				tblParent.rows[i].cells[j].style.display = "";
			}
			else
			{
				tblParent.rows[i].cells[j].style.display = "none";
			}
		}
	}
}

/***************************************
    function to show non empty table fields
***************************************/
function ShowNonEmpty(tblId,grpName)
{
	var tblParent = document.getElementById(tblId);
	var strtInd = 0;
	var foundStart=false;
	var endInd=tblParent.rows[0].cells.length;
	
	for(var i=0;i<tblParent.rows.length;i++)
		{
			if(tblParent.rows[i].style.display == "none")
			{
				tblParent.rows[i].style.display = "";
			}

			for(var j=0;j<tblParent.rows[i].cells.length;j++)
			{
				if(tblParent.rows[i].cells[j].style.display == "none")
				{
					tblParent.rows[i].cells[j].style.display = "";
				}
			}
		}
	
	for(var i=0;i<tblParent.rows[0].cells.length;i++)
	{
		if((!foundStart)&&(tblParent.rows[0].cells[i].group==grpName))
		{
			strtInd = i;
			foundStart=true;
		}
		else if((foundStart)&&(tblParent.rows[0].cells[i].group!=grpName))
		{
			endInd=i;
			break;
		}
	}

	for(var j=0;j<tblParent.rows.length;j++)
	{
		var isEmpty=true;
		
		for(var k=strtInd;k<endInd;k++)
		{
			if(tblParent.rows[j].cells[k].innerText.replace(/ /gi,"")!="")
			{
				isEmpty=false;					
				break;
			}
		}
		if(isEmpty)
		{
			tblParent.rows[j].style.display="none";
		}
	}
}

/***************************************
    function to hide group members
***************************************/
function HideGroupMembers(tblId,grpName)
{
	var tblParent = document.getElementById(tblId);
	var grpIndex=tblParent.rows[0].cells.length;
	
	for(var k=0;k<tblParent.rows[0].cells.length;k++)
	{
		if(tblParent.rows[0].cells[k].group==grpName)
		{
			grpIndex=k;
			break;
		}
	}
	for(var i=0;i<tblParent.rows.length;i++)
	{
		tblParent.rows[i].style.display="";
		for(var j=0;j<tblParent.rows[i].cells.length;j++)
		{
			if(j<grpIndex)
			{
				tblParent.rows[i].cells[j].style.display = "";
			}
			else
			{
				tblParent.rows[i].cells[j].style.display = "none";
			}
		}
	}
}

/***************************************
    function to remove duplicate rows
***************************************/
function RemoveDuplicateRows(tblId)
{
	
	var tblParent = document.getElementById(tblId);
	for(var i=1;i<tblParent.rows.length;i++)
	{
		if(i<tblParent.rows.length-1)
		{
			if(tblParent.rows[i].style.display!="none")
			{
				for(var j=i+1;j<tblParent.rows.length;j++)
				{	
					var isDuplicate=true;
					for(var k=0;k<tblParent.rows[i].cells.length;k++)
					{
						if((tblParent.rows[i].cells[k].style.display!="none")&&(tblParent.rows[i].cells[k].innerHTML!=tblParent.rows[j].cells[k].innerHTML))
						{
							isDuplicate=false;
							break;
						}
					}
					if(isDuplicate)
					{
						tblParent.rows[j].style.display="none";
					}
				}
			}
		}			
	}
}

/***************************************
    function to fill up empty cells
***************************************/
function FillUpEmptyCells(tblId)
{
	var tblMain = document.getElementById(tblId);		
	for(var i=1;i<tblMain.rows.length;i++)
	{
		while(tblMain.rows[i].cells.length<tblMain.rows[0].cells.length)
		{
			var newCell=tblMain.rows[i].insertCell(tblMain.rows[i].cells.length);
			newCell.innerText=" ";
			newCell.group=tblMain.rows[0].cells(tblMain.rows[i].cells.length-1).group;		
			newCell.style.display="none";	
		}
	}
}

/***************************************
    Feet meter convertion for mechanical data
***************************************/
function FeetMetreConversion(idVal)
{    
    if(unitValue == '')
        userPreferenceValue = document.getElementById(GetObjectID('hidDepthUnit','input')).value;     
    
    if((userPreferenceValue == '') || (userPreferenceValue == 'GO'))
    {
        if (idVal.indexOf(document.getElementById(GetObjectID('hidDepthUnit','input')).value)>=0)
        {           
        }
        else
        {
            userPreferenceValue = 'changed';     
	        if(unitValue == '')
	        {
	            if(idVal == "Metres")
	            {
	                flag = flag + 1;
	                unitValue = idVal;
	            }
	            else
	            {
	                flag = flag + 1;
	                unitValue = idVal;
	            }
	        }
	        else
	        {
	            if(idVal == unitValue)
	            {
	                flag = flag + 1;
	            }
	            else
	            {
	                unitValue = idVal;
	                flag = 1;
	            }
	        }
	        if(flag <= 1)
	        {
	            MechanicalReportFeetMetreConversion();
            }
        }        
    }
    else
    {
        if (idVal.indexOf(userPreferenceValue)>=0)
        {
            userPreferenceValue = document.getElementById(GetObjectID('hidDepthUnit','input')).value;
        }
        else
        {    
            userPreferenceValue = 'changed';     
	        if(unitValue == '')
	        {
	            if(idVal == "Metres")
	            {
	                flag = flag + 1;
	                unitValue = idVal;
	            }
	            else
	            {
	                flag = flag + 1;
	                unitValue = idVal;
	            }
	        }
	        else
	        {
	            if(idVal == unitValue)
	            {
	                flag = flag + 1;
	            }
	            else
	            {
	                unitValue = idVal;
	                flag = 1;
	            }
	        }
	        if(flag <= 1)
	        {
    	        MechanicalReportFeetMetreConversion();
            }
        }
    }
}

function MechanicalReportFeetMetreConversion()
{
    var tableColl = document.getElementsByTagName('table');
	var isExtraHdrRowPresent =true;
	for(var i=0;i<tableColl.length;i++)
	{
     isExtraHdrRowPresent =true;
		var table = tableColl[i];
		if(table.rows.length>0)
		{
			for(var j=0;j<table.rows[0].cells.length;j++)
			{
				if(table.rows[0].cells[j].tagName=='TH')
				{
				    if(table.rows[0].cells[j].innerText.indexOf("(ft)")>=0)
				     {
				      table.rows[0].cells[j].innerText = table.rows[0].cells[j].innerText.replace("(ft)","(m)");
				      ModifyCellValues(table,'ft',j,1);
					isExtraHdrRowPresent =false;
				     }
				    else if (table.rows[0].cells[j].innerText.indexOf("(inches)")>=0)
				     {
				       table.rows[0].cells[j].innerText = table.rows[0].cells[j].innerText.replace("(inches)","(mm)");
				       ModifyCellValues(table,'inches',j,1);
					isExtraHdrRowPresent =false;
				     }
				     else if(table.rows[0].cells[j].innerText.indexOf("(m)")>=0)
				     {
				        table.rows[0].cells[j].innerText = table.rows[0].cells[j].innerText.replace("(m)","(ft)");
					    ModifyCellValues(table,'m',j,1);
					    isExtraHdrRowPresent =false;
				     }
				    else if (table.rows[0].cells[j].innerText.indexOf("(mm)")>=0)
				     {
				       table.rows[0].cells[j].innerText = table.rows[0].cells[j].innerText.replace("(mm)","(inches)");
				       ModifyCellValues(table,'mm',j,1);
					isExtraHdrRowPresent =false;
				     }
					
				} // TH if ends here
				
			} // row[0] for loop ends
		
			if((isExtraHdrRowPresent)&&(table.rows.length>1))
			{
			for(var j=0;j<table.rows[1].cells.length;j++)
			{
						
            if(table.rows[1].cells[j].tagName=='TH')
            {
                if(table.rows[1].cells[j].innerText.indexOf("(ft)")>=0)
				{
				    table.rows[1].cells[j].innerText = table.rows[1].cells[j].innerText.replace("(ft)","(m)");
					ModifyCellValues(table,'ft',j,2);
				}
				else if(table.rows[1].cells[j].innerText.indexOf("(m)")>=0)
				{
				    table.rows[1].cells[j].innerText = table.rows[1].cells[j].innerText.replace("(m)","(ft)");
					ModifyCellValues(table,'m',j,2);
				}
				if(table.rows[1].cells[j].innerText.indexOf("(inches)")>=0)
				{
				    table.rows[1].cells[j].innerText = table.rows[1].cells[j].innerText.replace("(inches)","(mm)");
					ModifyCellValues(table,'inches',j,2);
				}
				else if(table.rows[1].cells[j].innerText.indexOf("(mm)")>=0)
				{
				    table.rows[1].cells[j].innerText = table.rows[1].cells[j].innerText.replace("(mm)","(inches)");
					ModifyCellValues(table,'mm',j,2);
				}
		      } // TH if ends here
			}// row[1] for loop ends here
			
			}	// Extraheader row ends here		
			
		} // table.rows.length > 0 ends
	} // 1st For loop
}
	

/***************************************
    Modify Cell Values
***************************************/
function ModifyCellValues(table, curUnit,colInd,DataRowNo)
{
    
	if(table.rows.length>1)
	{
		for(var i=DataRowNo;i<table.rows.length;i++)
		{		    
		    var isDataRow=true;
		    
		    if(i>1)
			{
			    if(table.rows[i-1].cells[0].getElementsByTagName('img').length!=0)
			    {
			        isDataRow=false;
				}
			}
		    if(i>2)
			{
			    if(table.rows[i-2].cells[0].getElementsByTagName('img').length==1)
				{
				    if(table.rows[i-2].cells[0].getElementsByTagName('img')[0].src.toLowerCase().indexOf('minus.gif')!=-1)
				    {
				        isDataRow=false;
				    }
				}
		    }
			if(isDataRow)
			{
			    if(curUnit=="ft")
			    {			        

	              if(table.rows[i].cells[colInd].innerText!=" ")
	                {					         
		               if(!isNaN(parseFloat(table.rows[i].cells[colInd].innerText)))
		               {
		                table.rows[i].cells[colInd].innerText =(parseFloat(table.rows[i].cells[colInd].innerText)/3.28084).toFixed(2);
		                }		            
		            
		            }

			    }
			    else if(curUnit=="m")
			    {
			     if(table.rows[i].cells[colInd].innerText!=" ")
			            {	
			               if(!isNaN(parseFloat(table.rows[i].cells[colInd].innerText)))
				           {
			                table.rows[i].cells[colInd].innerText = (parseFloat(table.rows[i].cells[colInd].innerText)*3.28084).toFixed(2);			
			                }
			            }		    
			    }
			    else if(curUnit=="inches")
			    {
			     if(table.rows[i].cells[colInd].innerText!=" ")
			            {	
			              if(!isNaN(parseFloat(table.rows[i].cells[colInd].innerText)))
				           {
			                table.rows[i].cells[colInd].innerText = (parseFloat(table.rows[i].cells[colInd].innerText)*25.4).toFixed(2);			
			               }
			            }		    
			    }
			    else if(curUnit=="mm")
			    {
			     if(table.rows[i].cells[colInd].innerText!=" ")
			            {	
			             if(!isNaN(parseFloat(table.rows[i].cells[colInd].innerText)))
				           {
			                table.rows[i].cells[colInd].innerText = (parseFloat(table.rows[i].cells[colInd].innerText)/25.4).toFixed(2);			
			                }
			            }		    
			    }
			}
			
                
            if(table.rows[i].cells[0].tagName == 'TH')
            { 
            for(var k=0;k<table.rows[i].cells.length;k++) 
                {  
                    if(table.rows[i].cells[k].innerText!=" ")
                    {  
                            if(curUnit=="ft")
			                { 
                                if(table.rows[i].cells[k].innerText.indexOf("(ft)")>=0)
				                {
				                    table.rows[i].cells[k].innerText = table.rows[i].cells[k].innerText.replace("(ft)","(m)");					
				                }
				            }
				            else if(curUnit=="m")
			                {
				                if(table.rows[i].cells[k].innerText.indexOf("(m)")>=0)
				                {
				                    table.rows[i].cells[k].innerText = table.rows[i].cells[k].innerText.replace("(m)","(ft)");
				                }
				            }
				            else if(curUnit=="inches")
			                {
				                if(table.rows[i].cells[k].innerText.indexOf("(inches)")>=0)
				                {
				                table.rows[i].cells[k].innerText = table.rows[i].cells[k].innerText.replace("(inches)","(mm)");
				                }
				            }
				            else if(curUnit=="mm")
			                {
				                if(table.rows[i].cells[k].innerText.indexOf("(mm)")>=0)
				                {
				                    table.rows[i].cells[k].innerText = table.rows[i].cells[k].innerText.replace("(mm)","(inches)");
				                }
				            }
				    }
				}
		     }
		}
		
	}
}
	
	
/************************************************************
        function to round value to 4 decimals
************************************************************/ 
function RoundNumber(valToRound) 
{
	var rLength = 2; // The number of decimal places to round to
	var newNumber;
	newNumber = CORE(valToRound, rLength);
	
	return newNumber;
}

/***************************************
    returns the Core value
***************************************/
function CORE(X, N) 
{    
    // X to String with N decimal places
    var int = Math.floor(X), frc = ((X-int)+1.0) * Math.pow(10, N)
    frc = String(IntFrc(frc)) // Put chosen rounding algorithm here
    return (int + +(frc.charAt(0)=="2")) + '.' + frc.substring(1) 
}

/***************************************
    returns the appropriate value of the number. 
***************************************/
function IntFrc(frc) 
{ 
    var CaseNo = 2;
    // often for X > -0.5e-N
    with (Math) switch (CaseNo) {
    case 0 : // Trunc
      return floor(frc)
    case 1 : // Round
      return round(frc)
    case 2 : // Alternate Round
      if (frc%1 >= 0.5) frc++ ; return frc | 0
    case 3 : // Simple Bankers'
      return frc%1 != 0.5 ? round(frc) : 2*round(frc/2)
    case 4 : // Better Bankers'
      return NearHalf(frc) ? 2*round(frc/2) : round(frc)
    case 5 : // Double-round
      if (NearHalf(frc)) frc = round(2*frc)/2 ; return round(frc)
    case 6 : // Ceil
      return ceil(frc)
    case 7 : // Statistical
      return (frc + random()) | 0
    default : return " TypeError" } 
}
    
/***************************************
    returns the rounded off value of the number
***************************************/
function RoundOffNumber(num)
{
    var numDigits=2;
    return Math.round(num*Math.pow(10,numDigits))/Math.pow(10,numDigits).toString();
}

/***************************************
    validates the EDM date fields
***************************************/
function ValidateEDMDates()
{
if(document.getElementById(GetObjectID("txtPUBDATE", "input")).value !=null && document.getElementById(GetObjectID("txtPUBDATE", "input")).value !="")
  {
   if(ValidateDate(document.getElementById(GetObjectID("txtPUBDATE", "input"))) == false)
	{
		return false;
	}
 }
return true;
	
}

/***************************************
    Retains the previous selected value of the combo 
***************************************/
function AddLastSelectedValue()
{
    var ddlDepthRef =document.getElementById(GetObjectID("cboDepthRef", "select"));
    var hdnDepthRef =document.getElementById(GetObjectID('hidDrpValue', 'input'));

if(ddlDepthRef!=null && hdnDepthRef!=null )
    {
        hdnDepthRef.value = ddlDepthRef[ddlDepthRef.selectedIndex].text;
    }
}

/***************************************
    Calculates the field values based on selection whether AH or TVD
***************************************/
function MachanicalDataConversion()
{
	var isExtraHdrRowPresent =false;		
    var tableColl = document.getElementsByTagName('table');
		for(var i=0;i<tableColl.length;i++)
		{
		isExtraHdrRowPresent=true;;	
			var table = tableColl[i];
			if(table.rows.length>0)
			{
				for(var j=0;j<table.rows[0].cells.length;j++)
				{
					if(table.rows[0].cells[j].tagName=='TH')
					{		  
					  if(table.rows[0].cells[j].innerText.indexOf("AH")>=0)
					  {
					    ModifyMachanicalDataCellValues(table,"AH",j,1);	
					   	isExtraHdrRowPresent=false;		    
					  }
					  else  if(table.rows[0].cells[j].innerText.indexOf("TV")>=0)
					  {
					     ModifyMachanicalDataCellValues(table,"TV",j,1);	
					    	isExtraHdrRowPresent=false;		    
					  }					  
						
				    }							
				}
				
				if((isExtraHdrRowPresent)&&(table.rows.length>1))
				{
				
				for(var j=0;j<table.rows[1].cells.length;j++)
				{
					if(table.rows[1].cells[j].tagName=='TH')
					{		  
					  if(table.rows[1].cells[j].innerText.indexOf("AH")>=0)
					  {
					    ModifyMachanicalDataCellValues(table,"AH",j,2);	
					    		    
					  }
					  else  if(table.rows[1].cells[j].innerText.indexOf("TV")>=0)
					  {
					     ModifyMachanicalDataCellValues(table,"TV",j,2);	
					   			    
					  }					  
						
				    }							
				}			
				}
					
			}
		}

AddLastSelectedValue();
	
	//added for header elevation
	 var tblHeader = document.getElementById('tblHeader');	
	 var cboDepthRef = document.getElementById(GetObjectID("cboDepthRef", "select"));
	 if(tblHeader.rows.length>1)	 
			{			
				for(var j=0;j<tblHeader.rows[0].cells.length;j++)
				{
					if(tblHeader.rows[0].cells[j].innerText.indexOf("Elevation")>=0)
					{  
				    if(tblHeader.rows[0].cells[j].innerText.indexOf("(ft)")>=0)
				            {
				               // tblHeader.rows[1].cells[j].innerText= RoundOffNumber(parseFloat(cboDepthRef.options[cboDepthRef.selectedIndex].value));
				                tblHeader.rows[1].cells[j].innerText= parseFloat(cboDepthRef.options[cboDepthRef.selectedIndex].value).toFixed(2);
				              
				            }
				        else  if(tblHeader.rows[0].cells[j].innerText.indexOf("(m)")>=0)
				            {
				                //tblHeader.rows[1].cells[j].innerText = RoundOffNumber((parseFloat(cboDepthRef.options[cboDepthRef.selectedIndex].value)/3.28084).toString());		
				                tblHeader.rows[1].cells[j].innerText = (parseFloat(cboDepthRef.options[cboDepthRef.selectedIndex].value)/3.28084).toFixed(2);			    		    
				            }
					  	      
					  	        break;
				    }							
				}
			}
}

/***************************************
    Calculates the field values based on selection whether Feer or Meter
***************************************/
function ConvertFeetMetre(value,unit)
{
    var convertedValue = value;
    if(!isNaN(parseFloat(value)))
    {
        if(unit =='f' || unit =='F')
        {
            convertedValue = (parseFloat(value)*3.28084).toFixed(2);
        }
        else if(unit =='m' || unit =='M')
        {
            convertedValue = (parseFloat(value)/3.28084).toFixed(2);
        }
    }
    return convertedValue;
}
/***************************************
    Modify the mechanical Data Cell Values
***************************************/
function ModifyMachanicalDataCellValues(table, curUnit,colInd,DataRowNo)
{
 
    if(table.rows.length>1)
		{
			for(var i=DataRowNo;i<table.rows.length;i++)
			{
			    var isDataRow=true;
			    
			    if(i>1)
				{
				    if(table.rows[i-1].cells[0].getElementsByTagName('img').length!=0)
				    {
				        //isDataRow=false;
					}
				}
			    if(i>2)	
				{
				    if(table.rows[i-2].cells[0].getElementsByTagName('img').length==1)
					{
					    if(table.rows[i-2].cells[0].getElementsByTagName('img')[0].src.toLowerCase().indexOf('minus.gif'))
					    {
					        //isDataRow=false;
					    }
					}
			    }
				if((table.rows[i].cells[colInd]!=null)&&!(isNaN(parseFloat(table.rows[i].cells[colInd].innerText)))&&(isDataRow))
				{
				 var cboDepthRef= document.getElementById(GetObjectID("cboDepthRef", "select"));				
				var Text = cboDepthRef.options[cboDepthRef.selectedIndex].text;				
				var Value = cboDepthRef.options[cboDepthRef.selectedIndex].value;
				
				var BF = getGValue('BF');
				var DF = getGValue('DF');
				var GL = getGValue('GL');
				var KB = getGValue('KB');				
				var PDL= getGValue('PDL');	
				var RT = getGValue('RT');			 				
				
				 var m = getGValue(document.getElementById(GetObjectID('hidDrpValue', 'input')).value.toString());
				 //converting unit to meter if cell vlaues are in meter
				  if(table.rows[0].cells[colInd].innerText.indexOf("(m)")>=0)
				  {
				    BF = ConvertFeetMetre(getGValue('BF'),'m');
				    DF = ConvertFeetMetre(getGValue('DF'),'m');
				    GL = ConvertFeetMetre(getGValue('GL'),'m');
				    KB = ConvertFeetMetre(getGValue('KB'),'m');			
				    PDL= ConvertFeetMetre(getGValue('PDL'),'m');
				    RT = ConvertFeetMetre(getGValue('RT'),'m');	
				     m =  ConvertFeetMetre(m,'m'); 
				  }
				 
				    if(Text=="GL")
				    {
				        if(table.rows[i-1].cells[0].getElementsByTagName('img').length==0)
				        {			
				               if(table.rows[i].cells[colInd].innerText!=" ")
				               {	
				                 	    
								        table.rows[i].cells[colInd].innerText = (parseFloat(parseFloat(table.rows[i].cells[colInd].innerText) - parseFloat(m))+parseFloat(GL)).toFixed(2);	
								   
							   }						
					    }
				    }
				    else if(Text=="BF")
				    {
				        if(table.rows[i-1].cells[0].getElementsByTagName('img').length==0)
				        {      
				        if(table.rows[i].cells[colInd].innerText!=" ")
				               {    						
							table.rows[i].cells[colInd].innerText = (parseFloat(parseFloat(table.rows[i].cells[colInd].innerText) - parseFloat(m))+parseFloat(BF)).toFixed(2);						
							}
				        }
				    }
					 else if(Text=="DF")
				    {
				        if(table.rows[i-1].cells[0].getElementsByTagName('img').length==0)
				        {	
				       if(table.rows[i].cells[colInd].innerText!=" ")
				               {			           
							table.rows[i].cells[colInd].innerText = (parseFloat(parseFloat(table.rows[i].cells[colInd].innerText) - parseFloat(m))+parseFloat(DF)).toFixed(2);											
							}
				        }
				    }
				    else if(Text=="KB")
				    {
				        if(table.rows[i-1].cells[0].getElementsByTagName('img').length==0)
				        {	 if(table.rows[i].cells[colInd].innerText!=" ")
				               {			           
							    table.rows[i].cells[colInd].innerText = (parseFloat(parseFloat(table.rows[i].cells[colInd].innerText) - parseFloat(m))+parseFloat(KB)).toFixed(2);											
							    }
				        }
				    }
				    else if(Text=="RT")
				    {
				        if(table.rows[i-1].cells[0].getElementsByTagName('img').length==0)
				        {	
				         if(table.rows[i].cells[colInd].innerText!=" ")
				               {			           
							table.rows[i].cells[colInd].innerText = (parseFloat(parseFloat(table.rows[i].cells[colInd].innerText) - parseFloat(m))+parseFloat(RT)).toFixed(2);												
							}
				        }
				    }
				    else if(Text=="PDL")
				    {
				        if(table.rows[i-1].cells[0].getElementsByTagName('img').length==0)
				        {	
				        if(table.rows[i].cells[colInd].innerText!=" ")
				               {			           
							table.rows[i].cells[colInd].innerText = (parseFloat(parseFloat(table.rows[i].cells[colInd].innerText) - parseFloat(m))+parseFloat(PDL)).toFixed(2);												
							}
				        }
				    }
				}
			}
		}
}

/***************************************
    Get GV Value
***************************************/
function getGValue(obj)
{
    var ddlReport = document.getElementById(GetObjectID("cboDepthRef", "select"));
    for(var i=0; i<ddlReport.length;i++)
    {
        if(ddlReport.options[i].text == obj)
        {          
           return ddlReport.options[i].value;
        }        
    }

}

//***************************************Functions added for My Team, FeedBack, ListView
var objFileUploadDoc= null;
/***************************************
    function to attach uploaded file
***************************************/
function AttachFileToUpload()
{

   if(window.document.getElementById(GetObjectID('TR10','DIV')).style.display == "block")
   {
   alert('Only 1 file can be attached to feedback.');
   return false;
   }

    var iWidth = 500;
    var iHeight = 120;

    var ileft = parseInt((screen.availWidth/2) - (iWidth/2));
    var itop = parseInt((screen.availHeight/2) - (iHeight/2));

    var sWindowFeatures = "width=" + iWidth + ",height=" + iHeight + ",status=yes,resizable=yes,left=" + ileft + ",top=" + itop + "screenX=" + ileft + ",screenY=" + itop;

    if(objFileUploadDoc != null)
     {
       try
       {
        objFileUploadDoc.location = "/Pages/DREAMFileUpload.aspx";
        }
       catch(ex)
       {
        objFileUploadDoc = window.open("/Pages/DREAMFileUpload.aspx","UploadDocument",sWindowFeatures); 
       }
     }
     else
     {
     objFileUploadDoc = window.open("/Pages/DREAMFileUpload.aspx","UploadDocument",sWindowFeatures); 
     }
    objFileUploadDoc.focus();

}
 
/***************************************
    function to check if file is uploaded and refresh parent
***************************************/
function CheckIfUploadedAndRefreshParent()
{

    var lblErrorMessage = document.getElementById(GetObjectID('lblErrorMessage','span'));
    if(lblErrorMessage != null)
    {
        if(lblErrorMessage.innerText == 'Document uploaded successfully')
        {
            var loc = window.opener.location;
            window.opener.location=loc;
            window.opener = top;
            window.close();
        }
    }
}

/***************************************
    function to close the parent without prompt
***************************************/
function CloseWithoutPrompt()
{
    window.opener=top;
    window.close();
}

/***************************************
    function to reload the parent window without prompt
***************************************/
function ReloadParent(msg)
{
    if(msg!='')
    {
        alert(msg);
    }
    var loc = window.opener.location;
    window.opener.location=loc;
    window.opener = top;
    window.close();
}

/***************************************
    function to update the parent window fields - FeedBack 
***************************************/
function UpdateParentControl(objControl,objControlType, objValue, objPath)
{
    var control = window.opener.document.getElementById(GetParentObjectID(objControl,objControlType));
    var fileUp = objPath;
    var extension = fileUp.substring(fileUp.lastIndexOf('.'));
    extension = extension.toString().toLowerCase();
    control.innerText = objValue;
    window.opener.document.getElementById(GetParentObjectID('hidFileName','input')).value = objValue;
    window.opener.document.getElementById(GetParentObjectID('hidFilePath','input')).value = objPath;
    window.opener.document.getElementById(GetParentObjectID('TR10','DIV')).style.display = "block";
    CloseWithoutPrompt();
}

/***************************************
    function to restrict uploaded file
***************************************/
function ValidateUpload()
{


    var fileUp = document.getElementById(GetObjectID('fileUploader',"input"));
    var extension = fileUp.value.substring(fileUp.value.lastIndexOf('.'));

    if(extension!='')
        extension = extension.toString().toLowerCase();

    if(extension==".bmp"||extension==".gif"||extension==".jpg"||extension==".txt"||extension==".doc"||extension==".xls"||extension==".png"||extension==".docx"||extension==".xlsx")
        return true;
    else
    {
        if(extension=='')
        {
        alert('Please select a document to upload.');
        return false;
        }
        else
        {
        alert('Please select a valid file format(*.bmp, *.gif, *.jpg, *.png, *.txt, *.doc, *.xls).');
        return false;
        }
    }

}


//Reads the word document 
function ReadWordDocument(path, hidControlId)
{
document.getElementById(GetObjectID(hidControlId,"input")).value = "";
        var w = new ActiveXObject("Word.Application");
        var docText;
        var obj;
        if (w != null) 
        {
            w.Visible = false;
            obj = w.Documents.Open(path);
            docText = obj.Content;
            document.getElementById(GetObjectID(hidControlId,"input")).value = docText;
            w.Quit();
        }
}

/***************************************
    function to read the excel document
***************************************/
function ReadExcelDocument(path, hidControlId)
{
document.getElementById(GetObjectID(hidControlId,"input")).value="";

var excel;	// Declare the variables
	excel = new ActiveXObject("Excel.Application");	// Create the Excel application object.
	
	    var docText;
        var obj;
        if (excel != null) 
        {
            excel.Visible = false;
            var excel_file = excel.Workbooks.Open(path);
            
            for(var j =1;j<=2000;j++)
            {
            if(excel_file.Worksheets(1).Cells(j,1).Value != null)
               document.getElementById(GetObjectID(hidControlId,"input")).value += excel_file.Worksheets(1).Cells(j,1).Value + "\r\n";
            }
                        
	        excel.Quit();
        }
        
}

/***************************************
    function to open in the same window
***************************************/
function OpenSameWindow(url)
{    
   window.location.href = url;
}

/***************************************
    function to set the column width of the List View report
***************************************/
function ListReportFixColWidth(tblID, reportType,permission)

{

    var table = document.getElementById(tblID);
   
   var cTR = table.getElementsByTagName('TR');  //collection of rows	
		      
	    var tr = cTR.item(0);	
	  
         if(reportType=='TeamRegistration')
         {
             if(permission=="DreamAdmin")
             {
             tr.cells[0].style.width = "35%";
             tr.cells[1].style.width = "20%";    
	         tr.cells[2].style.width = "20%";
             tr.cells[3].style.width = "5%";
             }
             if(permission=="TeamOwner" || permission=="NonRegUser")
             {
             tr.cells[0].style.width = "30%";
             tr.cells[1].style.width = "15%";    
	         tr.cells[2].style.width = "20%";
             tr.cells[3].style.width = "15%";
             }
         }
	
         
    
}

/***************************************
    function to enable paging and sorting in Quick Search 
***************************************/
function QuickSearchPagingListReport(URL,pageNumber,recordcount,sortBy,sortType)
{
    window.location.href = URL+"pagenumber="+pageNumber+"&RecordCount="+recordcount+"&sortby="+sortBy+"&sorttype="+sortType;
}

/***************************************
    function to export to Excel 
***************************************/
function ExportToExcelAdvanced()
{       

    alert('This option will only export the records displayed on the current page. If you have multiple pages you will have to export each page separately.'); 
	document.body.style.cursor = 'wait';
	var reportType =  document.getElementById(GetObjectID('hdnReportType',"input")).value;
    String.prototype.Trim = function () 
    {
        return this.replace(/\|*\s*$/, "");
    }    
	table = document.getElementById('tblSearchResults');
	
	try
    {
        var xls = new ActiveXObject("Excel.Application");
   
        xls.Workbooks.add();
        xls.Workbooks(1).WorkSheets(1).Name = "DREAM2.1";
		
		var tempCount = 1;
		var rows=table.rows;
		var x=1;
		var charA = 65;       
	    var displayLength = 1;
		for(i = 0; i < rows.length; i++)
		{
		    if(i == 0)
		    {	
		        xls.Cells(i+1, 1).FormulaLocal = reportType;					
		        xls.columns.AutoFit();
		        xls.Cells(i+1, 1).Font.Bold = true;		       
		        xls.Cells(i+1, 1).Font.Color = 0;
		        xls.Cells(i+1, 1).Interior.ColorIndex = 16;
		        xls.Cells(i+1, 1).Borders.Weight = 2;	
		         for(k = 1; k <= rows[i].cells.length; k++)
		         {		        
		           var headercolumn = rows[i].cells;		          
		            if((headercolumn[k-1].className != "printerFriendly"))  
		            { 
		              displayLength = displayLength + 1;
		            }
		         }    	        		        
		    }
		    
            var column = rows[i].cells;
            var colLength = column.length;
            
            var updateXValue = false;
		    for(j = 1; j <= colLength; j++)
		    {
		     
		    if((column[j-1].className != "printerFriendly"))  
		    { 
			            xls.Cells(x+1, j).FormulaLocal = "'" + column[j-1].innerText.Trim();					
			            xls.Cells(x+1, j).Borders.Weight = 2;
		                xls.columns.AutoFit();				    
		                if(x == 1)
	                    {
	                        xls.Cells(x+1, j).Font.Bold = true;				    
		                    xls.Cells(x+1, j).Interior.ColorIndex = 15;		                    
	                    }
	                    updateXValue = true;	                    
	         }
	         if(j==colLength)
	            {
	                if(updateXValue)
	                    x=x+1;
	            }
		    }						
		}		
		var endColumn = 66;		
		if(displayLength > 2)
		{
	    endColumn = charA + (displayLength -2);
	    }
		var startIndex = String.fromCharCode(charA);
		var endIndex = String.fromCharCode(endColumn);
		
		xls.Range(startIndex +"1", endIndex+"1").HorizontalAlignment = -4108;
		xls.Range(startIndex +"1", endIndex+"1").MergeCells = true;
	    xls.visible = true;
        }
        catch( E )
        {
            alert('Either excel is not installed or Your browser security setting is not allowing to create excel object.');
        }
	    document.body.style.cursor = 'auto';
}

/***************************************
    function to print the page content
***************************************/
 function PrintContent()
 { 
     var attributes="toolbar=yes,location=no,directories=yes,resizable,menubar=yes,";
     attributes+="scrollbars=yes,width=800, height=600, left=100, top=25";
     window.open("/_layouts/dream/Print.htm","Print",attributes);
 } 
 
 
/***************************************
    function to open the pages 
***************************************/
function OpenPages(id,listType, mode)
{
   if(listType == 'TeamRegistration') 
   {
    if(mode=='managestaff') window.location.href = "/Pages/AdminManageStaff.aspx?idValue="+ id;
    if(mode=='addteam') 
    {
    window.open("/Pages/AddTeam.aspx",name,"width=800, height=450, left=100, top=100, screenX=100, screenY=100");
    }
    if(mode=='editteam') window.location.href = "/Pages/EditDreamTeam.aspx?idValue=" + id;
   }
   return false;
}

/***************************************
    function to delete the record
***************************************/
function DeleteRecord(id,listType)
{

    var ans = window.confirm("Confirm delete?");

    if(ans==true)
    {
   __doPostBack('Delete',id);
    }
    else
    {
    return false;
    }
}

/***************************************
    function to validate the Team Registration
***************************************/
function ValidateAddTeam()
{
if(document.getElementById(GetObjectID('txtTEAMNAME','input')).value == "")
    {
    alert("Please complete all mandatory fields.");
    return false;
    }
    else
    {
    if(!OnlyAlphaNumeric(document.getElementById(GetObjectID('txtTEAMNAME','input')).value))
        {
          alert("Please enter only alpha numeric characters.");
          return false;
        }
    }
    /// Multi Team Owner Implementation
    /// Changed By: Yasotha
    /// Date : 13-Jan-2010
//    if(document.getElementById(GetObjectID('cboTEAMOWNER','select')).value == "")
//    {
//    alert('Please fill all the mandatory fields.');
//    return false;
//    }

  if(document.getElementById(GetObjectID('lstTeamOwner','select')).value == "")
    {
    alert('Please fill all the mandatory fields.');
    return false;
    }
    /// Multi Team Owner Implementation
    /// End
     if(document.getElementById(GetObjectID('cboPROJECTNAME','select')).value == "")
    {
    alert('Please fill all the mandatory fields.');
    return false;
    }
    
    return true;
}

/***************************************
    function to allow only alpha numeric
***************************************/
function OnlyAlphaNumeric(alphane)
{
var inputText  = alphane;

if(inputText.replace(/\s/g,"") == ""){ 
return false;
} 

	var numaric = alphane;
	for(var j=0; j<numaric.length; j++)
		{
		  var alphaa = numaric.charAt(j);
		  var hh = alphaa.charCodeAt(0);
		
		  if((hh > 47 && hh<58) || (hh > 64 && hh<91) || (hh > 96 && hh<123))
		  {
		 
		  }
		else	
		{
             if(hh==32||hh==95)
             {
             
             }
             else
                return false;
		  }
 		}
 return true;
}

/*************************************************Functions for TVDSS calculation*********************/

function IsValidDepthReference(depthRef,ddlDepthRef)
{
    for(var i=0; i < ddlDepthRef.options.length ; i++)
   {
        if(depthRef  == ddlDepthRef.options[i].text)
        {
            return true;
        }
    }
   return false;
}

/***************************************
    function to calcuate the depth reference
***************************************/
function DepthRefCalculator(tableId)
{
    var tblSearchResults =document.getElementById(tableId);
    var ddlDepthRef = document.getElementById(GetObjectID("cboDepthRef", "select"));
    var strSelectedDepthRef =ddlDepthRef.options[ddlDepthRef.selectedIndex].text;
    var strDepthValue;  
    var strDefaultValue;//default depth ref values as well as last selected value;
    var depthRefValueCol= new Array();   
    var depthRefStatusCol = new Array();
    var defaultDepthRefCol = new Array();
    if(tblSearchResults!=null)
    {
       var dataRowIndex = tblSearchResults.tBodies[0].firstChild.rowIndex;
       var arrIndex  = GetDepthRefColIndexes(tblSearchResults, depthRefValueCol, depthRefStatusCol, defaultDepthRefCol);
       if(depthRefValueCol.length == 0)
       {
            return;
       }
    for(var i=dataRowIndex;i<tblSearchResults.rows.length;i++)
    {
        if(ddlDepthRef.selectedIndex == 0)
        {
            if(defaultDepthRefCol.length > 0)
            {
                strSelectedDepthRef = tblSearchResults.rows[i].cells[defaultDepthRefCol[0]].innerText;
            }
            else
            {
                return ;
            }
        }
        //adding slected depth ref value
        //depth ref value column format
        //[KB=85.0;GL=-280.0;DF=85.0;]default|unit
        //[KB=85.0;GL=-280.0;DF=85.0;]KB|feet
        if(depthRefStatusCol.length >0)
            tblSearchResults.rows[i].cells[depthRefStatusCol[0]].innerText  = strSelectedDepthRef ;

        if((defaultDepthRefCol.length > 0) && (tblSearchResults.rows[i].cells[defaultDepthRefCol[0]].innerText.indexOf('No data')>=0))
         {
            continue;
         }
	
    var strDepthRef = tblSearchResults.rows[i].cells[depthRefValueCol[0]].innerText;
    if(strDepthRef.indexOf('[]')>=0)
    {
     continue;
    }
    var strDefault = strDepthRef.substring(strDepthRef.indexOf(']')+1,strDepthRef.indexOf('|'));
    var strDepthRefUnit = strDepthRef.substring(strDepthRef.indexOf('|')+1);
   
    if(!IsValidDepthReference(strSelectedDepthRef,ddlDepthRef))
    {
        continue;
    }
    if((strDepthRef.indexOf(strSelectedDepthRef)>=0)||(strSelectedDepthRef=='PDL'))
    {
        strDepthValue = GetDepthRefValue(strDepthRef,strSelectedDepthRef);
        strDefaultValue = GetDepthRefValue(strDepthRef,strDefault);       
        tblSearchResults.rows[i].cells[depthRefValueCol[0]].innerText = strDepthRef.substring(0,strDepthRef.lastIndexOf(']')+1) +strSelectedDepthRef+"|"+strDepthRefUnit;
    }
    else
    {
        strDepthValue = null;
    }   
        for(var j=0;j<arrIndex.length;j++)
        {
         //added for UAT fixes
            var strCurrentValue ;
            var strCurrentUnit;
            if(tblSearchResults.rows[i].cells[arrIndex[j]].value)
            {
              strCurrentValue = tblSearchResults.rows[i].cells[arrIndex[j]].value ;
            }
            else
            {
               strCurrentValue = tblSearchResults.rows[i].cells[arrIndex[j]].innerText ;
            }
            //current unit
            if(tblSearchResults.rows[i].cells[arrIndex[j]].unit)
            {
              strCurrentUnit = tblSearchResults.rows[i].cells[arrIndex[j]].unit ;
            }
            else
            {
               strCurrentUnit = GetSelectedUnit(); ;
            }
            if(strCurrentValue == 'No data')
                 {
                    continue;
                 }
            if(strDepthValue!=null)
            {
                  tblSearchResults.rows[i].cells[arrIndex[j]].setAttribute('value',CalculateDepthRef(strCurrentValue,strCurrentUnit,strDefaultValue,strDepthValue,strDepthRefUnit));
                  tblSearchResults.rows[i].cells[arrIndex[j]].setAttribute('unit',GetSelectedUnit());
                  tblSearchResults.rows[i].cells[arrIndex[j]].innerText  = tblSearchResults.rows[i].cells[arrIndex[j]].value;
            }
            else
            {
              tblSearchResults.rows[i].cells[arrIndex[j]].setAttribute('value',strCurrentValue); 
              tblSearchResults.rows[i].cells[arrIndex[j]].setAttribute('unit',strCurrentUnit);       
              tblSearchResults.rows[i].cells[arrIndex[j]].innerText  = "No data";      
              tblSearchResults.rows[i].cells[arrIndex[j]].align="right";              
            }  
        }
    }
  }   
}

/***************************************
    function to get the depth reference column indexes
***************************************/
function GetDepthRefColIndexes(table,depthRefValueColIndex,depthRefStatusColIndex,defaultDepthRefColIndex)
{

var arrIndex =new Array();
var hdrRowIndex = table.tHead.lastChild.rowIndex;
for(var i= 0 ; i<table.rows[hdrRowIndex].cells.length ; i++)
{
    if((table.rows[hdrRowIndex].cells[i].tagName=='TH')&&((table.rows[hdrRowIndex].cells[i].innerText.indexOf(" AH ")>=0)||(table.rows[hdrRowIndex].cells[i].innerText.indexOf(" TV ")>=0)))			
    {
      arrIndex.push(i);
    }
    if(table.rows[hdrRowIndex].cells[i].innerText.indexOf('Depth References')>=0)
    {
      depthRefValueColIndex.push(i);
    }
     if(table.rows[hdrRowIndex].cells[i].innerText.indexOf('Selected Depth Reference')>=0)
    {
      depthRefStatusColIndex.push(i);
    }
    if(table.rows[hdrRowIndex].cells[i].innerText.indexOf('Default Depth Reference')>=0)
    {
      defaultDepthRefColIndex.push(i);
    }
}
return arrIndex;
}

/***************************************
    function to get the depth reference value
***************************************/
function GetDepthRefValue(DepthRefValues,SelectedDepthRefValue)
{
    //********Explanation of logic used************///
    //Given string [DF=184.0;GL=-156.0;KB=184.0]
    //Manipulating selected value by first getting substring starting from selected value(GL) and then substring between '=' and ';'
    //Ex if GL is selected then
    //GL=-156.0;KB=184.0;
    //then substring between  = and ; contains required value
    //********Explanation of logic used end************///
    if(SelectedDepthRefValue =='PDL')
    {
        return '0.00';
    }
    //replacing ']' with ';' and substring starting from selected deptf ref from ddl
     var str =DepthRefValues.replace(']',';').substring(DepthRefValues.indexOf(SelectedDepthRefValue));
     var strValue = str.substring(str.indexOf('=')+1,str.indexOf(';')); 
     return strValue;
}

/***************************************
    function to calcuate the depth reference
***************************************/
function CalculateDepthRef(currentCellValue,strCurrentUnit,lastSeclectedValue,CurrentSeclectedValue,depthRefUnit)
{
     var calculatedDepth ;
     if((!(isNaN(parseFloat(currentCellValue))))&&(!(isNaN(parseFloat(lastSeclectedValue))))&&(!(isNaN(parseFloat(CurrentSeclectedValue)))))
     {
        if(GetSelectedUnit() != strCurrentUnit)
        {
            currentCellValue = ConvertFeetMetre(currentCellValue,GetSelectedUnit().substring(0,1));
        }
        if(GetSelectedUnit() != depthRefUnit)
        {  
            lastSeclectedValue =  ConvertFeetMetre(lastSeclectedValue,GetSelectedUnit().substring(0,1));
            CurrentSeclectedValue = ConvertFeetMetre(CurrentSeclectedValue,GetSelectedUnit().substring(0,1));
        }
        calculatedDepth = (parseFloat(currentCellValue) - parseFloat(lastSeclectedValue)) + parseFloat(CurrentSeclectedValue);
        return calculatedDepth.toFixed(2);
     }
     else
     {
        return " ";
     }
}

/***************************************
    function to get the Selected Unit
***************************************/
function GetSelectedUnit()
{
    var rdb= document.getElementById(GetObjectID('rbMeters','input')); 
    if(rdb != null)
    {
        if(rdb.checked)
        {
            return 'meters';
        }
        else
        {
            return 'feet';
        }
    }
}
	 
/***************************************
    function to calcuate the depth reference TST 
***************************************/
function DepthRefDTSTCalculator()
{
    var arrDepthRows =document.getElementsByName('trDepthRef');
    var ddlDepthRef = document.getElementById(GetObjectID("cboDepthRef", "select"));
    var strUnit;
    if(arrDepthRows == null)
    {
        return;
    }
    if(GetSelectedUnit() == "meters")
    {
        strUnit = "(m)";
    }
    else
    {
        strUnit = "(ft)";
    }
    var strSelectedDepthRef = ddlDepthRef.options[ddlDepthRef.selectedIndex].text;
    var strDepthValue;  
    var strDefaultValue;//default depth ref values as well as last selected value;
    for(var i= 0; i<arrDepthRows.length;i++)
    {
        var arrTable =arrDepthRows[i].getElementsByTagName('table');
        var depthTable =null;
        var depthRefTable =null;
        for(var j=0; j<arrTable.length; j++)
        {
            if(arrTable[j].id=='Depths')
            {
                depthTable = arrTable[j];
            }
            if(arrTable[j].id=='Depth References')
            {
                depthRefTable = arrTable[j];
            }               
        }
        if(depthRefTable == null)
        {
            continue;
        }
        var strDepthRef =depthRefTable.rows[0].cells[1].innerText;
        var strDefault = strDepthRef.substring(strDepthRef.indexOf(']')+1,strDepthRef.indexOf('|'));
        var strDepthRefUnit = strDepthRef.substring(strDepthRef.indexOf('|')+1);
        var intSelectedDepthRefColIndex = -1;
        var intDefaultDepthRefColINdex = -1;
        var arrAHTVColIndex = new Array();
        for(var k=0; k<depthTable.rows.length; k++)
        {
            if(depthTable.rows[k].cells[0].innerText.indexOf("Default Depth Reference")>=0)
            {
                intDefaultDepthRefColINdex = k;
                continue;
            }
            if(depthTable.rows[k].cells[0].innerText.indexOf("Selected Depth Reference")>=0)
            {
                intSelectedDepthRefColIndex = k;
                continue;
            }
            if(depthTable.rows[k].cells[0].innerText.indexOf(" AH")>=0||depthTable.rows[k].cells[0].innerText.indexOf(" TV")>=0)			
            {
                arrAHTVColIndex.push(k);
            }
        }
        if(ddlDepthRef.selectedIndex==0)
        {
            if(intDefaultDepthRefColINdex!=-1)
            {
                strSelectedDepthRef = depthTable.rows[intDefaultDepthRefColINdex].cells[1].innerText.replace(/ /g,'');
            }
            else
            {
                return ;
            }
        }
        if(intSelectedDepthRefColIndex!=-1)
            depthTable.rows[intSelectedDepthRefColIndex].cells[1].innerText = strSelectedDepthRef;
        
        if((intDefaultDepthRefColINdex!=-1)&&(depthTable.rows[intDefaultDepthRefColINdex].cells[1].innerText == "No data"))
            continue;
            
        if(strDepthRef.indexOf('[]')>=0)
        {
            continue;
        }    
        if(!IsValidDepthReference(strSelectedDepthRef,ddlDepthRef))
        {
            continue;
        }   
        if((strDepthRef.indexOf(strSelectedDepthRef)>=0)||(strSelectedDepthRef=='PDL'))
        {
            strDepthValue = GetDepthRefValue(strDepthRef,strSelectedDepthRef);
            strDefaultValue = GetDepthRefValue(strDepthRef,strDefault);       
            depthRefTable.rows[0].cells[1].innerText = strDepthRef.substring(0,strDepthRef.lastIndexOf(']')+1) +strSelectedDepthRef+"|"+strDepthRefUnit;
        }
        else
        {
            strDepthValue =null;
        }       
        if(arrAHTVColIndex.length <=0)			
        {
            continue;
        }
        for(var intAHTVColIndexCounter = 0; intAHTVColIndexCounter < arrAHTVColIndex.length; intAHTVColIndexCounter++)
        {
            //added for UAT fixes
            var strCurrentValue ;
            var strCurrentUnit;
            if(depthTable.rows[arrAHTVColIndex[intAHTVColIndexCounter]].cells[1].value)
            {
                strCurrentValue = depthTable.rows[arrAHTVColIndex[intAHTVColIndexCounter]].cells[1].value ;
            }
            else
            {
               strCurrentValue = depthTable.rows[arrAHTVColIndex[intAHTVColIndexCounter]].cells[1].innerText ;
            }
            if(strCurrentValue  == "No data")
            {
                continue;
            }
             //current unit
            if(depthTable.rows[arrAHTVColIndex[intAHTVColIndexCounter]].cells[1].unit)
            {
                strCurrentUnit = depthTable.rows[arrAHTVColIndex[intAHTVColIndexCounter]].cells[1].unit ;
            }
            else
            {
                strCurrentUnit = GetSelectedUnit(); ;
            }
            if(strDepthValue != null)
            {
                depthTable.rows[arrAHTVColIndex[intAHTVColIndexCounter]].cells[1].setAttribute('value',CalculateDepthRef(strCurrentValue,strCurrentUnit,strDefaultValue,strDepthValue,strDepthRefUnit));
                depthTable.rows[arrAHTVColIndex[intAHTVColIndexCounter]].cells[1].setAttribute('unit',GetSelectedUnit());
                depthTable.rows[arrAHTVColIndex[intAHTVColIndexCounter]].cells[1].innerText  = depthTable.rows[arrAHTVColIndex[intAHTVColIndexCounter]].cells[1].value + " " + strUnit;
            }
            else
            {   
                depthTable.rows[arrAHTVColIndex[intAHTVColIndexCounter]].cells[1].setAttribute('value',strCurrentValue);
                depthTable.rows[arrAHTVColIndex[intAHTVColIndexCounter]].cells[1].setAttribute('unit',strCurrentUnit);     
                depthTable.rows[arrAHTVColIndex[intAHTVColIndexCounter]].cells[1].innerText  = "No data";                       
            }
        }  
    }
}
/****End****/
/***************************************************************
            function to validate FUR start and end date
***************************************************************/
function ValidateEPCatalogDates()
{
        strStartDate = document.getElementById(GetObjectID("txtStartDate", "input")).value
        strEndDate = document.getElementById(GetObjectID("txtEndDate", "input")).value
		if(strStartDate == '' && strEndDate == '')
		{
		  return true;
		}
		return CallValidateDateService("txtStartDate","txtEndDate");
}

/***************************************************************
            function to change the class name 
***************************************************************/
function ChangeClassName(obj,className)
{
obj.className = className;
}
/***********************************************************************************
       function used for select all the columns by default on search results page for tabular report with tab
************************************************************************************/
function CheckUncheckAll(CheckBoxControl,parent,chkBxId)
{
var arrchkBox =parent.getElementsByTagName('input');
var isCheck = CheckBoxControl.checked;

            for (i=0; i < arrchkBox.length; i++) 
            {                
                if ((arrchkBox[i].type == 'checkbox')&&(arrchkBox[i].id == chkBxId))
                {
                        arrchkBox[i].checked = isCheck;
                }
            }
}
/*****************************************************

   Silverlight Map Export To Excel Functionality

*****************************************************/

function MapExportToExcel(titleData, datastring) 
{
    document.body.style.cursor = 'wait';
    String.prototype.Trim = function () 
    {
       return this.replace(/\|*\s*$/, "");
    }
    var xlApp = new ActiveXObject("Excel.Application");
    xlApp.Workbooks.Add();
    var XlSheet = xlApp.Workbooks(1).WorkSheets(1); 
    XlSheet.Name = "Export to Excel";
    var arrTitle = new Array();
    arrTitle = titleData.split(',');
    for (var i = 0; i < arrTitle.length; i++) 
    {
        XlSheet.Cells(1, i + 1).FormulaLocal = arrTitle[i];     
        XlSheet.Cells(1, i + 1).Font.Bold = true;
        XlSheet.Cells(1, i + 1).Interior.ColorIndex = 15;        
    }
    var arrData = new Array();
    arrData = datastring.split('~');
    for (i = 0; i < arrData.length; i++) 
    {
        var str = arrData[i].split(',');
        for (var j = 0; j < str.length; j++)
        {
            XlSheet.Cells(i + 2, j + 1).NumberFormat = "@";
            XlSheet.Cells(i + 2, j + 1).HorizontalAlignment = -4131;//xlLeft is the constant -4131 
            XlSheet.Cells(i + 2, j + 1).Value  = str[j];
        }
    }
    XlSheet.columns.AutoFit();   
    xlApp.visible = true;
    document.body.style.cursor = 'auto';
}
function HideShowLeftNav()
{   
    if(window.parent.Splitter == null)
    return;
    var radPaneLeft = window.parent.Splitter.getStartPane();
    radPaneLeft.collapse(1);
}
function ChangeCursor(tdName)
{
    var TD = document.getElementById(tdName);
    TD.style.cursor = 'hand';
}

/*********to open the file*/
function OpenFileViewer()
{
var fileUp = document.getElementById(GetObjectID('hidFilePath','input')).value;

    var extension = fileUp.substring(fileUp.lastIndexOf('.'));
    extension = extension.toString().toLowerCase();
    
  var w ;
  var obj;
  
  
       if(extension==".doc" ||  extension==".docx" || extension == ".txt")
       {
       w = new ActiveXObject("Word.Application");
      
       if (w != null) 
        {
            w.Visible = true;
            obj = w.Documents.Open(document.getElementById(GetObjectID('hidFilePath','input')).value);
        }
       }
       
       if(extension==".xls" ||  extension==".xlsx")
       {
       w = new ActiveXObject("Excel.Application");
       
        if (w != null) 
        {
            w.Visible = true;
		    var book = w.Workbooks.Open(document.getElementById(GetObjectID('hidFilePath','input')).value);
        }
       }

}

function FileSearchTypeSelectedIndexChange()
{
if(arguments.length>0)
  {
    var ddlFileSearchType = arguments[0];
    var blnDisabled =false;
    if(ddlFileSearchType.selectedIndex>0)
        {
            blnDisabled =true;
        }
    else
        {
            blnDisabled =false;
        }
    for(var index=1;index<arguments.length;index++)
        {
          var arrIdTagType =arguments[index].split('|');
          var objControl;
          
          if((objControl=document.getElementById(GetObjectID(arrIdTagType[0], arrIdTagType[1])))!=null)
          {
              if(GetObjectID(arrIdTagType[0], arrIdTagType[1]).toLowerCase().indexOf(ddlFileSearchType.options[ddlFileSearchType.selectedIndex].value.toLowerCase()) != -1)
              {
                 objControl.disabled =blnDisabled;
              }
              else
              {
                objControl.disabled = !blnDisabled;
              }
          }
        }
  }
}

/************************************************
Exporting Tab Tabular Search Results from report service...
*************************************************/
function ExportTabTabularSearchResults()
  { 
    document.body.style.cursor = 'wait';
    String.prototype.Trim = function () 
       {
         return this.replace(/\|*\s*$/, "");
       }     
        var strTitle    ="";
            try
            { 
              var xls = new ActiveXObject("Excel.Application");
              xls.Workbooks.add();      
              for(var index=0;index<arguments.length-1;index++)
	            {
	              xls.Workbooks(1).WorkSheets.add();      
	            }
              var rows ;
              var counter =1;
              var isExportValid =false;
               for(var index=0;index<arguments.length;index++)
	            {	
			
	             var arrTable =document.getElementById(arguments[index]);	
                    if((arrTable!=null)&&(IsDataRowSelected(arrTable))&&(IsDataColumnSelected(arrTable)))
		                {	
		                 var xslSheet = xls.Workbooks(1).WorkSheets(counter); 
		                 isExportValid =true;
	                     counter++; 	                   
		                    var arrSpan  = document.getElementsByTagName("span");	
		                    for(var j=0;j<arrSpan.length;j++)
		                        {
		                            if(arrSpan[j].className==arrTable.parentElement.parentElement.id)
		                                {
		                                    strTitle  = arrSpan[j].innerText;
		                                    break;
		                                }
		                        }			                       
			            
			              if(strTitle!="")
			                xslSheet.Name = strTitle; 			           
		                 rows = arrTable.rows;
				 var rowIndex =1;
				 var colIndex =1;
                            for (i = 1; i < rows.length; i++)
                                {                                
                                    if((rows[i].cells[0].innerHTML.indexOf("CHECKED")>=0)||(i==1))
                                    {
                                        var column = rows[i].cells;
					colIndex =1;
                                        for (j = 1; j < column.length; j++)
                                            {
                                                 if(rows[0].cells[j].innerHTML.indexOf("CHECKED")>=0)
                                                  {
                                                      if (i == 1)
                                                      {
                                                            xslSheet.Cells(rowIndex,colIndex).FormulaLocal = column[j].innerText.Trim();
                                                            xslSheet.Cells(rowIndex,colIndex).Font.Bold = true;
                                                            xslSheet.Cells(rowIndex,colIndex).Interior.ColorIndex = 15;                                                   
                                                       }
                                                       else
                                                       {
                                                        if(rows[1].cells[j].type!="" && rows[1].cells[j].type == "number" )
					                                    {
					                                          xslSheet.Cells(rowIndex,colIndex).NumberFormat = "0.00";
					                                          xslSheet.Cells(rowIndex,colIndex).HorizontalAlignment = -4152;//XlRight is the constant -4152 
					                                    }
					                                 else if(rows[1].cells[j].type!="" && rows[1].cells[j].type == "date" )
					                                     {
					                                          xslSheet.Cells(rowIndex,colIndex).NumberFormat = GetDateFormat();
					                                          xslSheet.Cells(rowIndex,colIndex).HorizontalAlignment = -4131;//xlLeft is the constant -4131 
                					                      
					                                     }
					                                 else
					                                     {
					                                          xslSheet.Cells(rowIndex,colIndex).NumberFormat = "@";
					                                          xslSheet.Cells(rowIndex,colIndex).HorizontalAlignment = -4131;//xlLeft is the constant -4131 
					                                     }     
                                                            xslSheet.Cells(rowIndex,colIndex).Value = "'" + column[j].innerText;
                                                             
                                                       }  
                                                       xslSheet.columns.AutoFit();  
                                                       colIndex++;
                                                   }                                               
                                            }
                                        rowIndex++;    
                                      }
                                  }        
                          }
                         
                   }
                   if(isExportValid)
                   {
                        alert('This option will only export the records displayed on the current page. If you have multiple pages you will have to export each page separately.'); 
                        xls.visible = true;
                   }
                   else
                   {
                        alert('Please select some record to export.');
                   }                    
               
            }
            catch (E)
            {
               
                alert("Either excel is not installed or Your browser security setting is not allowing to create excel object.");
            }
            document.body.style.cursor = 'auto';
 }
 /******************************************************
    Function to validate CheckBox is selected or not for row.
*******************************************************/
function IsDataRowSelected(table)
{    
    var boolChecked = false;   
    var checkBoxCounter = 0;
    if(table!=null)
    {
        var row = table.rows;
        for(i = 1; i < row.length; i++)
	    {
	        if(row[i].cells[0].innerHTML.indexOf("CHECKED")>=0)
		    {
			    boolChecked = true;
			    break;             
            }
	    } 
	}  
 return boolChecked;
}
 /******************************************************
    Function to validate CheckBox is selected or not for row.
*******************************************************/
function IsDataColumnSelected(table)
{    
    var boolChecked = false;   
    var checkBoxCounter = 0;
    if(table!=null)
    {
        var colmns = table.rows[0].cells;
        for(i = 0; i < colmns.length; i++)
	    {
	        if(colmns[i].innerHTML.indexOf("CHECKED")>=0)
		    {
			    boolChecked = true;
			    break;             
            }
	    } 
	}  
 return boolChecked;
}
/******************************************************
    Function to copy from data to to data or vice versa if any one is empty
*******************************************************/
function EPCatalogFromToDateCopy(currentTxtBxId,otherTxtBxId)
{
     var objCurrentTxtBx =  document.getElementById(GetObjectID(currentTxtBxId,'input'));
     var objOtherTxtBxId = document.getElementById(GetObjectID(otherTxtBxId,'input'));
    if((objCurrentTxtBx==null)||(objOtherTxtBxId==null))
        {
            return;
        }

  if(objCurrentTxtBx.value !="" && objOtherTxtBxId.value =="")
  {
     objOtherTxtBxId.value = objCurrentTxtBx.value;
  }
  else if(objCurrentTxtBx.value ==""&&objOtherTxtBxId.value !="") 
  {    
      objCurrentTxtBx.value = objOtherTxtBxId.value;
  }
}

function GetDateFormat()
{
    var strDateFormat = "dd-mmm-yyyy";
		var hidDateFormat =document.getElementById(GetObjectID("hidDateFormat",'input'));
		if(hidDateFormat!=null)
		{
		  strDateFormat = hidDateFormat.value;
	    }
		return strDateFormat;
}
/************************************************************************************/
/************** Funtions for Tabular Report with tab Export all*********************/
/************************************************************************************/
/***********************************************************************************
       function used for geting sellected columns from search/Context search reports
************************************************************************************/
function TabTabularExportAll()
{
 var searchType= document.getElementById(GetObjectID('hidSearchType','input')).value;
 var requestid = document.getElementById(GetObjectID('hidRequestID','input')).value;
 var strMaxRecord= document.getElementById(GetObjectID('hidMaxRecord','input')).value;
 var hidSelectedCol = document.getElementById(GetObjectID('hidSelectedColumns','input'));
 hidSelectedCol.value ='';
 var url ="/Pages/ExportToExcel.aspx";
 var objTable =null;
 var isExportAllValid =false;
 var ReportType ="TabTabularReport";
     for(var index=0;index<arguments.length;index++)
	    {
	        objTable =document.getElementById(arguments[index]);
	        if(objTable!=null)
	        {	
	            if(IsDataColumnSelected(objTable))
	            {
	                isExportAllValid =true;
	                hidSelectedCol.value = hidSelectedCol.value + GetSelectedColumnForTabReport(objTable) +"TAB"
	            }
	        }	
	    }
	 if(isExportAllValid)
	 {
	            var msgWindow=window.open('',ReportType,'width=400,height=300,scrollbars=no,resizable=no,status=no,left=100,top=100');
                document.forms[0].action=url + '?Searchtype=' + searchType+'&Requestid='+requestid+'&MaxRecord='+strMaxRecord;
                document.forms[0].method="post";
                document.forms[0].target=ReportType;
                document.forms[0].submit();
                msgWindow.focus();
                document.forms[0].target="_self";
                document.forms[0].action= msgWindow.opener.location.href;
                document.forms[0].method="post"; 
	 }
	 else
	 {
	    alert('Please select minimum one column.');  
	 }
}
function GetSelectedColumnForTabReport(table)
{    
    var row = table.rows;  
    var arrCheckBx = row[0].getElementsByTagName("input");
     var strSelectedColumns = "" ;
    for (i=0; i < arrCheckBx.length; i++)
    {        
        if (arrCheckBx[i].type == 'checkbox')
        {
            if (arrCheckBx[i].id == 'chkBxRow')
            {
                if(arrCheckBx[i].checked == true)
                {
                    strSelectedColumns =  strSelectedColumns + arrCheckBx[i].value +"|";
                }
            }
        }
    }
    return strSelectedColumns;  
}


/*************End for Tabular Report with tab Export all*****************/




/*****************************************************

   Silverlight Map Print Functionality

*****************************************************/

function MapPrintResults(printData) 
{
    screenW = screen.width;
    screenH = screen.height;
    var attributes = "toolbar=yes,location=no,directories=yes,resizable,menubar=yes,";
    attributes += "scrollbars=yes,width=800, height=600, left=100, top=25";
    var win = window.open("/_layouts/dream/print.htm", "Print",attributes);
    win.document.write(printData);
    win.document.close();
    win.print();
}

/***********************************************************************************
       function used to Map Print
************************************************************************************/
function PrintMapResults(title, htmlprefix, url, data,width) {


try
{
   screenW = screen.width;
    screenH = screen.height;
    var attributes = "toolbar=yes,location=no,directories=no,resizable=no,menubar=no,";
    attributes += "scrollbars=yes,width="+width+", height=700, left=100, top=25";
    var win = window.open("", "Print", attributes);
    win.document.write(htmlprefix);
    win.document.write("<div id='title' align='center' style='color:#000000;font-weight:bold;font-family:Arial'>" + title + "</div>");
    win.document.write("<hr align=left size=2 width=100%>");
    win.document.write("<img id='printout' src='" + url + "' />");
    win.document.write("<hr align=left size=2 width=100%>");
    win.document.write(data);
    win.document.close();   
    win.history.forward(1);
    win.document.attachEvent("onkeydown", Mapprint_onkeydown_handler);

    var arVersion = navigator.appVersion.split("MSIE");
    var version = parseFloat(arVersion[1]);

    if (version <= 6.0) 
   {
	win.onload = Setpng(win);
   
   }
   win.print();
}
catch(err)
  {
  
  }
 
}

/***********************************************************************************
       function used to disable F5 in Map Print Result window
************************************************************************************/

function Mapprint_onkeydown_handler(event) {
   
    switch (event.keyCode) {

        case 116: // 'F5'
            event.returnValue = false;
            event.keyCode = 0;           
            break;
    }
}

/***********************************************************************************
       function used to enable Png transparency
************************************************************************************/

function Setpng(win)
{
try
{
if(win.document.body.filters)
{
 win.print(); 
   for(var i=0; i<win.document.images.length; i++)
   {
      var myImage= win.document.images[i];

      var imgName = myImage.src.toUpperCase();

 	var imgID = (myImage.id) ? "id='" + myImage.id + "' " : "";

	   var imgClass = (myImage.className) ? "class='" + myImage.className + "' " : ""
	   var imgTitle = (myImage.title) ? 
		             "title='" + myImage.title  + "' " : "title='" + myImage.alt + "' "
	   var imgStyle = "display:inline-block;" + myImage.style.cssText
	   var strNewHTML = "<span " + imgID + imgClass + imgTitle
                  + " style=\"" + "width:" + myImage.width 
                  + "px; height:" + myImage.height 
                  + "px;" + imgStyle + ";"
                  + "filter:progid:DXImageTransform.Microsoft.AlphaImageLoader"
                  + "(src=\'" + myImage.src + "\', sizingMethod='scale');\"></span>"
	   myImage.outerHTML = strNewHTML;
     
  
}
}
 }
catch(err)
  {
  
  }
 
}
///**********Javascript fuctions for UBI level export excel functionality*************/
//***************************************************                   */
function ExportUBIToWorkSheet()
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
        var xls = new ActiveXObject("Excel.Application");
        xls.Workbooks.add();  
        var arrUBI = "";
        var arrTblMainRows = tblMain.rows;
        var UWIIndex ;
        if($("table#tblSearchResults thead tr th:contains('UWI')").length > 0)
        {
            UWIIndex = $("table#tblSearchResults thead tr th:contains('UWI')")[0].cellIndex;
        }
       else if($("table#tblSearchResults thead tr th:contains('UWBI')").length > 0)
        {
            UWIIndex = $("table#tblSearchResults thead tr th:contains('UWBI')")[0].cellIndex;
        }
       else if($("table#tblSearchResults thead tr th:contains('Unique Wellbore Identifier')").length > 0)
        {
            UWIIndex = $("table#tblSearchResults thead tr th:contains('Unique Wellbore Identifier')")[0].cellIndex;
        }
        else
        {
           return true;//there are no Unique id column in report,then return
        }
        for(var rowCounter = arrCheckedRows.length-1; rowCounter >=0; rowCounter--)
        {
            
            var strUBI = arrCheckedRows[rowCounter].cells[UWIIndex].innerText;
            if(arrUBI.indexOf(strUBI)>=0)
            {
              continue;
            }
            else
            {
              arrUBI += strUBI + "|"; 
            }
            var arrRows = $("table#tblSearchResults tbody tr::has(input:checked):contains('" + strUBI + "')");
            
            if(rowCounter != arrCheckedRows.length-1)
            {
                xls.Workbooks(1).WorkSheets.add();  
            }
            var xslSheet = xls.Workbooks(1).WorkSheets(1); 
            xslSheet.Name = strUBI; 	
           
            AddWorkSheet(arrTblMainRows,arrRows,xslSheet);
         }
        xls.visible = true;
    }
    catch(ex)
    {
        alert('Either excel is not installed or Your browser security setting is not allowing to create excel object.');
        alert(ex.message);
    }
    document.body.style.cursor = 'auto';
}
function AddWorkSheet(headerRow,dataRows,xslSheet)
{
var arrColTags = $("table#tblSearchResults col");
var rowIndex = 1;
var colStartIndex = 1;
for (var i = 0; i < dataRows.length; i++)
{   
    var column = dataRows[i].cells;
    if(column[1].innerHTML.indexOf("INPUT") >= 0)//added for report contaning chart to aviod check box row to be exported
    {
        colStartIndex = 2;
    }
    else
    {
        colStartIndex = 1;
    }
    var colIndex =1;
    for (var j = colStartIndex; j < column.length; j++)
    {
        if((arrColTags[j].className == "hide")||(headerRow[0].cells[j].innerHTML.indexOf("CHECKED")<0))
        {
            continue;
        }
        if (rowIndex == 1)
        {

            xslSheet.Cells(rowIndex, colIndex).FormulaLocal = headerRow[1].cells[j].innerText.Trim();
            xslSheet.Cells(rowIndex, colIndex).Font.Bold = true;
            xslSheet.Cells(rowIndex, colIndex).Interior.ColorIndex = 15;                                                   
        }

        if(headerRow[1].cells[j].type!="" && headerRow[1].cells[j].type == "number" )
            {
                 if(headerRow[1].cells[j].innerText.indexOf("Quality")>=0)
                 {
                    xslSheet.Cells(rowIndex+1, colIndex).NumberFormat = "0";
                 }
                 else
                 {
                    xslSheet.Cells(rowIndex+1, colIndex).NumberFormat = "0.00";
                 }
                 xslSheet.Cells(rowIndex+1,colIndex).HorizontalAlignment = -4152;//XlRight is the constant -4152 
            }
         else if(headerRow[1].cells[j].type!="" && headerRow[1].cells[j].type == "date" )
             {
                 xslSheet.Cells(i+2, j).NumberFormat = GetDateFormat();
                 xslSheet.Cells(rowIndex+1, colIndex).HorizontalAlignment = -4131;//xlLeft is the constant -4131 
              
             }
         else
             {
                  xslSheet.Cells(rowIndex+1, colIndex).NumberFormat = "@";
                  xslSheet.Cells(rowIndex+1, colIndex).HorizontalAlignment = -4131;//xlLeft is the constant -4131 
             }     
         xslSheet.Cells(rowIndex+1,colIndex).Value = column[j].innerText;

        xslSheet.columns.AutoFit();   
        colIndex++;                                               
    }
   rowIndex++;  
} 
}
//**************End***************/

//***************************Function for Reorder column******************/
//function sets the value of hidden field hidReorderCol to the current order of column
function GetColumnOrder(source,showHideTblId)
{
    var arrRows = $("table#" + showHideTblId + " tbody tr");
    var strColOrderStatus = "";
    var objSort = document.getElementById('linkReorder');//anchor tag object to call its onlick method to maintain paging and sorting status
    var arrCheckedRows = $("table#" + showHideTblId + " tbody tr:has(input:checked)");//added to check that all column cannot be unchecked/unhide
    try
    {
        
        if(arrCheckedRows.length <= 0)
        {
            alert("Please select atleast one column to display.");
            return false;
        }
        for(var i =1; i<arrRows.length;i++)
        {
            var chkbx = arrRows[i].getElementsByTagName("input")[0];          
          
           strColOrderStatus += '<column name="' + chkbx.value + '" display="' + chkbx.checked + '"/>';
           
        }
       var hidReorderCol = document.getElementById(GetObjectID('hidReorderColValue', 'input'));  
       var hidReorderSourceId = document.getElementById(GetObjectID('hidReorderSourceId', 'input'));  
       //var hidReorderSharedVeiwChkBxStatus = document.getElementById(GetObjectID('hidReorderSharedVeiwChkBxStatus', 'input'));
       //var chkBxShareReorder = document.getElementById("chkBxShareView");
      // if(chkBxShareReorder)
           // hidReorderSharedVeiwChkBxStatus.value = chkBxShareReorder.checked;
       hidReorderCol.value = strColOrderStatus;
       hidReorderSourceId.value = source.id;
       objSort.click();
    }
    catch(ex)
    {
         alert(ex.message);
    }
}
//**************End***************/
function GetSortingAnchorTag()
{
  var strImgUrl = "http://" + window.location.host ;
  var objSort = $("table#tblSearchResults thead img:[src=" + strImgUrl + "/_layouts/DREAM/images/UP_ACTIVE.gif]");
  if(objSort.length > 0)
  return objSort[0].parentNode;
  objSort = $("table#tblSearchResults thead img:[src=" + strImgUrl + "/_layouts/DREAM/images/DOWN_ACTIVE.gif]");
 if(objSort.length > 0)
  return objSort[0].parentNode;
  objSort = $("table#tblSearchResults thead img:[src=" + strImgUrl + "/_layouts/DREAM/images/UP_INACTIVE.gif]");
  return objSort[0].parentNode;
}
function OpenReorderPopUp()
{
    document.getElementById('showHideDiv').style.display = 'block';
    return false
}
function HideReorderPouUp() 
{
    document.getElementById('showHideDiv').style.display='none';
    return false;
}
function MoveRow(source)
{
    var currentRow = GetCurrentSelectedRow();
    if(currentRow == null)
    {
        alert('Please select at least one column to reorder.');
        return false;
    }
    if (source.id == 'btnTop') 
    {
        if(currentRow.previousSibling != null)
         currentRow.parentNode.insertBefore(currentRow, currentRow.parentNode.firstChild );

    }
    else if (source.id == 'btnBottom') 
    {
         if(currentRow.nextSibling != null)
         currentRow.parentNode.insertBefore(currentRow, currentRow.parentNode.lastChild );
         currentRow.parentNode.insertBefore(currentRow.parentNode.lastChild, currentRow);
    }
    else if (source.id == 'btnDown') 
    {
        if(currentRow.nextSibling != null)
        currentRow.parentNode.insertBefore(currentRow.nextSibling, currentRow);
    }
    else if (source.id == 'btnUp') 
    {
        if(currentRow.previousSibling != null)
        currentRow.parentNode.insertBefore(currentRow, currentRow.previousSibling);
    }
    return false;
}
 function GetCurrentSelectedRow()
 {
    var table = document.getElementById('tblShowHideColOption');
    var arrRow = table.rows ;
    var currentRow ;
    for(var i = 0; i < arrRow.length ; i++)
    {
        if(arrRow[i].style.backgroundColor == '#bdbdbd')
        {
            currentRow = arrRow[i];
            break;
        }
    }
    return currentRow;
 }

/////*********************END************************/
/**new method for feet meter conversion**/
/** End of new method for feet meter conversion **/

var userPreferenceValue;

function FeetMetreConversionNew(tableId, idVal)
{
    if(unitValue == '')
        userPreferenceValue = document.getElementById(GetObjectID('hidDepthUnit','input')).value;     
    
    if((userPreferenceValue == '') || (userPreferenceValue == 'GO'))
    {
        if (idVal.indexOf(document.getElementById(GetObjectID('hidDepthUnit','input')).value)>=0)
        {           
        }
        else
        {
            userPreferenceValue = 'changed';     
	        if(unitValue == '')
	        {
	            if(idVal == "Metres")
	            {
	                flag = flag + 1;
	                unitValue = idVal;
	            }
	            else
	            {
	                flag = flag + 1;
	                unitValue = idVal;
	            }
	        }
	        else
	        {
	            if(idVal == unitValue)
	            {
	                flag = flag + 1;
	            }
	            else
	            {
	                unitValue = idVal;
	                flag = 1;
	            }
	        }
	        if(flag <= 1)
	        {
	            var strSelectedUnit = GetSelectedUnit();
            var strDisplayType = GetDisplayType();
            if(strDisplayType == 'Data Sheet')
            {	
                ConvertFeetMetersDatasheet(strSelectedUnit);
                return;
            }
            var table = document.getElementById(tableId);
            if(table==null)
            {
                return;
            }
            var headerRowIndex = table.tHead.lastChild.rowIndex;
            var dataRowIndex = table.tBodies[0].firstChild.rowIndex;
            var arrIndex = new Array();
            var hdrRowCells = table.rows[headerRowIndex].cells;
            for(var i = 0; i < hdrRowCells.length; i++)
                {
                    if(hdrRowCells[i].innerText.indexOf('(m)') >= 0)
                    {
                        arrIndex.push(i);
                        hdrRowCells[i].innerHTML = hdrRowCells[i].innerHTML.replace('(m)','(ft)');
                    }
                    else if(hdrRowCells[i].innerText.indexOf('(ft)') >= 0)
                    {
                        arrIndex.push(i);
                        hdrRowCells[i].innerHTML = hdrRowCells[i].innerHTML.replace('(ft)','(m)');
                    }
                }
            var rows = table.rows;
            for(var i = dataRowIndex; i < rows.length; i++)
            {
                for(var j = 0; j < arrIndex.length; j++)
                {
                    //convert feet meter
                    if(strSelectedUnit == "feet")
                    {
                        rows[i].cells[arrIndex[j]].innerText = ConvertFeetMetre(rows[i].cells[arrIndex[j]].innerText,"f");
                    }
                    else if(strSelectedUnit == "meters")
                    {
                       rows[i].cells[arrIndex[j]].innerText = ConvertFeetMetre(rows[i].cells[arrIndex[j]].innerText,"m"); 
                    }
                }  
            }
            }
        }        
    }
    else
    {
        if (idVal.indexOf(userPreferenceValue)>=0)
        {
            userPreferenceValue = document.getElementById(GetObjectID('hidDepthUnit','input')).value;
        }
        else
        {    
            userPreferenceValue = 'changed';     
	        if(unitValue == '')
	        {
	            if(idVal == "Metres")
	            {
	                flag = flag + 1;
	                unitValue = idVal;
	            }
	            else
	            {
	                flag = flag + 1;
	                unitValue = idVal;
	            }
	        }
	        else
	        {
	            if(idVal == unitValue)
	            {
	                flag = flag + 1;
	            }
	            else
	            {
	                unitValue = idVal;
	                flag = 1;
	            }
	        }
	    if(flag <= 1)
	    {
	    var strSelectedUnit = GetSelectedUnit();
            var strDisplayType = GetDisplayType();
            if(strDisplayType == 'Data Sheet')
            {	
                ConvertFeetMetersDatasheet(strSelectedUnit);
                return;
            }
            var table = document.getElementById(tableId);
            if(table==null)
            {
                return;
            }
            var headerRowIndex = table.tHead.lastChild.rowIndex;
            var dataRowIndex = table.tBodies[0].firstChild.rowIndex;
            var arrIndex = new Array();
            var hdrRowCells = table.rows[headerRowIndex].cells;
            for(var i = 0; i < hdrRowCells.length; i++)
                {
                    if(hdrRowCells[i].innerText.indexOf('(m)') >= 0)
                    {
                        arrIndex.push(i);
                        hdrRowCells[i].innerHTML = hdrRowCells[i].innerHTML.replace('(m)','(ft)');
                    }
                    else if(hdrRowCells[i].innerText.indexOf('(ft)') >= 0)
                    {
                        arrIndex.push(i);
                        hdrRowCells[i].innerHTML = hdrRowCells[i].innerHTML.replace('(ft)','(m)');
                    }
                }
            var rows = table.rows;
            for(var i = dataRowIndex; i < rows.length; i++)
            {
                for(var j = 0; j < arrIndex.length; j++)
                {
                    //convert feet meter
                    if(strSelectedUnit == "feet")
                    {
                        rows[i].cells[arrIndex[j]].innerText = ConvertFeetMetre(rows[i].cells[arrIndex[j]].innerText,"f");
                    }
                    else if(strSelectedUnit == "meters")
                    {
                       rows[i].cells[arrIndex[j]].innerText = ConvertFeetMetre(rows[i].cells[arrIndex[j]].innerText,"m"); 
                    }
                }  
            }
        }
        }
    }
}
	/************************************************************
        function to convert feet-meters and vice versa on datasheet view
************************************************************/
function ConvertFeetMetersDatasheet(idVal)
{  
    var nRow = document.getElementById(GetObjectID('hidRowsCount','input')).value;
    var singleColName;
    var colNamesToConvert;
    colNamesToConvert = "Total Depth AH|Total Depth TV|Depth Reference Value";
    singleColName = colNamesToConvert.split("|");
    for (var iRow=1; iRow<=nRow; iRow++)
    {
        for(var cols = 0; cols < singleColName.length; cols++)
        {
            var tdId = singleColName[cols] + iRow;            
           var tdValue ='';
            if(document.getElementById(tdId)!=null)          
               tdValue = document.getElementById(tdId).innerText;
            			
			if(tdValue != '' && tdValue != ' ' && tdValue != '  ' && (tdValue.indexOf('No data')<0))
			{			    
	            if(idVal == 'feet')
	            {
	                tdValue = tdValue.replace('(m)', '')
	                //tdValue = RoundNumber(tdValue * 3.28084);
	                tdValue = (tdValue * 3.28084).toFixed(2);
	                document.getElementById(tdId).innerText = tdValue + ' (ft)';
	            }
	            else if(idVal == 'meters')
	            {
	                tdValue = tdValue.replace('(ft)', '')
	               // tdValue = RoundNumber(tdValue / 3.28084);
	                tdValue = (tdValue / 3.28084).toFixed(2);
	                document.getElementById(tdId).innerText = tdValue + ' (m)';
	            }
			}
        }
    }    
} 
   /************************************************************
        function to convert feet-meters and vice versa on datasheet view
************************************************************/
function GetDisplayType()
{
    var displayType = "";
    try
    {
        if(document.getElementById(GetObjectID('rdoViewMode_1','input')).checked)
        {
            displayType = "Tabular";
        }
        else if(document.getElementById(GetObjectID('rdoViewMode_0','input')).checked)
        {
            displayType = "Data Sheet";
        }
    }
    catch (EX)
    {
        displayType = "Tabular";
    }
    return displayType;
}
/** New Export to excel javascript function*/
function ExportToExcelNew()
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
        var xls = new ActiveXObject("Excel.Application");
        xls.Workbooks.add();  
        var arrTblMainRows = tblMain.rows;
        var xslSheet = xls.Workbooks(1).WorkSheets(1); 
        xslSheet.Name = "SearchResults";	
        AddWorkSheet(arrTblMainRows,arrCheckedRows,xslSheet);
        xls.visible = true;
    }
    catch(ex)
    {
        alert('Either excel is not installed or Your browser security setting is not allowing to create excel object.');
        alert(ex.message);
    }
    document.body.style.cursor = 'auto';
}
//Dream 4.0 changes start
/***************************************
   Function to  Validate listbox Selection.
***************************************/
function ValidateListBoxSelection(listboxId,supid,errorSpanid)
{
    var selectedItemCount = GetSelectedItemCount(listboxId);
    if(selectedItemCount > 999)
    {
        ShowHideHTMLElement(supid,'sup','inline');
        ShowHideHTMLElement(errorSpanid,'span','block');
        return false;
    }
    else
    {
        return true;
    }
}
/**********************************************************************
   Function to  Validate Well/wellbore advance search Listbox Selection.
***********************************************************************/
function ValidateWellListboxSelection()
{
    var strErrorMessage = "The maximum number of items per selection criteria is 999.You have exceeded this for \"%\". Please amend your criteria and try again.";
    var arrSelectedItemCount = new Array();
    var intBasinSelectedItems = GetSelectedItemCount('lstBasin');
    var intCountrySelectedItems = GetSelectedItemCount('lstCountry');
    var intStateSelectedItems = GetSelectedItemCount('lstState_Or_Province');
    var intCountySelectedItems = GetSelectedItemCount('lstCounty');
    var intFieldSelectedItems = GetSelectedItemCount('lstField_Identifier');
    if(intBasinSelectedItems>999)
        arrSelectedItemCount.push('Basin');
    if(intCountrySelectedItems>999)    
        arrSelectedItemCount.push('Country');
    if(intStateSelectedItems>999)   
        arrSelectedItemCount.push('State Or Province');
    if(intCountySelectedItems>999)   
        arrSelectedItemCount.push('County');
    if(intFieldSelectedItems>999)   
        arrSelectedItemCount.push('Field Name');
    if(arrSelectedItemCount.length == 0)
        return true;
    else if(arrSelectedItemCount.length == 1)
    {
        alert(strErrorMessage.replace('%',arrSelectedItemCount[0]));
        return false;
    }
    else
    {
        ShowHideHTMLElement('spanLstBxSelectionErr','span','block');
        ValidateListBoxSelection('lstBasin','spanBasinSup','spanBasinSelectionError');
        ValidateListBoxSelection('lstCountry','spanCountrySup','spanCountrySelectionError');
        ValidateListBoxSelection('lstState_Or_Province','spanStateSup','spanStateSelectionError');
        ValidateListBoxSelection('lstCounty','spanCountySup','spanCountySelectionError');
        ValidateListBoxSelection('lstField_Identifier','spanFieldSup','spanFieldSelectionError');
        return false;
    }    
}
/***************************************
   Function to  Get Selected Item Count of a listbox.
***************************************/
function GetSelectedItemCount(listboxId)
{
    var selectedItemCount = 0;
    var objListBox = document.getElementById(GetObjectID(listboxId,'select'));
    if(objListBox == null)
        return -1;
    for (var index = 0; index < objListBox.length; index++)
    {
        if(objListBox.options[index].selected)
        {
            selectedItemCount++;
        }
    }
    return  selectedItemCount;
}
/***************************************
   Function to  Show Hide HTML Element
***************************************/
function ShowHideHTMLElement(id,tagName,displayStatus)
{
    var objElement = document.getElementById(GetObjectID(id,tagName));
    if(objElement != null)
    {
        objElement.style.display = displayStatus;
    }
}
/***************************************
   Function to  Validate Basin Search
***************************************/
function ValidateBasinSearch()
{
    //Dream 3.1 fix
    var selectedItemCount = GetSelectedItemCount('lstBasin');
    if(selectedItemCount>999)
    {
        alert("The maximum number of items per selection criteria is 999.You have exceeded this for \"Basin\". Please amend your criteria and try again.");
        return false;
    }
    if(!ValidateField(false))
    {
        return false;
    }
    return true;    
}
/***************************************
   Function to  Validate Field Search
***************************************/
function ValidateFieldSearch()
{
    //Dream 3.1 fix
    var selectedItemCount = GetSelectedItemCount('lstCountry');
    if(selectedItemCount>999)
    {
        alert("The maximum number of items per selection criteria is 999.You have exceeded this for \"Country\". Please amend your criteria and try again.");
        return false;
    }
    if(!ValidateField(false))
    {
        return false;
    }
    return true;    
}
/***************************************
   Function to  Set Focus to the error element
***************************************/
function SetFocus(id,tagName)
{
    var objListBox = document.getElementById(GetObjectID(id,tagName));
    if(objListBox!=null)
      
        objListBox.focus();
}
/************************************************************
        function to GetObjectID
************************************************************/
function GetObjectID(objectName, TagName)
{
    var objectId = "";
    var blnIdFound = false;
    var arrHTMLElements = document.documentElement.getElementsByTagName(TagName);
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
//Dream 4.0 changes end