/* 
 * *******************************************************************************
 * File Name        :   DREAMJavaScriptFunctionsRel3_0.js
 * Project Name     :   DREAM 3.0
 * Type             :   JavaScript Function  
 * Date_Created     :   June 14 2010
 * Version          :   3.0 
 * *******************************************************************************
 */
 
/**************************************************
function to open DWB Context Search Results Page
***************************************************/
function OpenDWBContextSearchLink(searchtype,assettype,columnIndex)
{
    if(GetEDMSelectedRowIdentifiers())
    {
        ContextSearchPopup('/Pages/DWBContextSearchResults.aspx?assettype='+assettype+'&SearchType='+searchtype,searchtype);
    }
}

/**************************************************
function to Open Published DWB Books list.
***************************************************/
function openPublishedBookList(id,bookName)
{
var iWidth = 800;
 var iHeight = 600;

 var ileft = parseInt((screen.availWidth/2) - (iWidth/2));
 var itop = parseInt((screen.availHeight/2) - (iHeight/2));

 var sWindowFeatures = "width=" + iWidth + ",height=" + iHeight + ",toolbar=no,location=no,directories=no,resizable=no,menubar=no,scrollbars=no,left=" + ileft + ",top=" + itop + ",screenX=" + ileft + ",screenY=" + itop;   
 var url = "/Pages/DWBPublishedBookList.aspx?BookID="+id +"&BookName="+ bookName;
   var msgPublished = window.open(url,'BookList',sWindowFeatures);
   msgPublished.focus();
   return false;
}

/**************************************************
function to Open DWB Book Viewer popup.
***************************************************/
function OpenBookViewer(bookid,searchType)
 {   
   
 var iWidth = 900;
 var iHeight = 800;

 var ileft = 100;
 var itop = 100;

 var sWindowFeatures = "width=" + iWidth + ",height=" + iHeight + ",status=yes,scrollbars=yes,resizable=yes,left=" + ileft + ",top=" + itop + ",screenX=" + ileft + ",screenY=" + itop;   
     var url = '/Pages/WellBookViewer.aspx?BookID='+ bookid + '&instance=new'+'&SearchType=' + searchType;
  
   
    var msgWindow = window.open("", 'WellBookViewer', sWindowFeatures);
        document.forms[0].action = url ;
        document.forms[0].method = "post";
        document.forms[0].target = "WellBookViewer";
        document.forms[0].submit();
   return false;
 }
 
 /**************************************************
function to width of Columns.
***************************************************/                            
function DWBFixColWidth(tblID, reportType)
{

    var table = document.getElementById(tblID);
   
   var colTR = table.getElementsByTagName('TR');  //collection of rows	
	for (iRowIndex = 0; iRowIndex < colTR.length; iRowIndex++)
	{ 	      
	    var tr = colTR.item(iRowIndex);	
	  
           if(reportType=='DWBHome')
         {
        
             tr.cells[0].style.width = "60%";
             tr.cells[1].style.width = "28%";    
	         tr.cells[2].style.width = "2%";
             tr.cells[3].style.width = "10%";

         }
         
       
	} 
         
    
}

/**************************************************
function to export DWB Context Search results.
***************************************************/     
function DWBExportToExcel()
{       

    alert('This option will only export the records displayed on the current page. If you have multiple pages you will have to export each page separately.'); 
	document.body.style.cursor = 'wait';
	var reportType =  document.getElementById(GetObjectID('hidReportType',"input")).value;
    String.prototype.Trim = function () 
    {
        return this.replace(/\|*\s*$/, "");
    }    
	table = document.getElementById('tblSearchResults');
	
	try
    {
        var xls = new ActiveXObject("Excel.Application");
   
        xls.Workbooks.add();
        xls.Workbooks(1).WorkSheets(1).Name = reportType;
		
		var tempCount = 1;
		var rows=table.rows;
		var iBeginIndex=1;
		var charA = 65;       
	    var displayLength = 1;

		for(iRowIndex = 0; iRowIndex < rows.length; iRowIndex++)
		{
		    if(iRowIndex == 0)
		    {	
		        xls.Cells(iRowIndex+1, 1).FormulaLocal = reportType;					
		        xls.columns.AutoFit();
		        xls.Cells(iRowIndex+1, 1).Font.Bold = true;		       
		        xls.Cells(iRowIndex+1, 1).Font.Color = 0;
		        xls.Cells(iRowIndex+1, 1).Interior.ColorIndex = 16;
		        xls.Cells(iRowIndex+1, 1).Borders.Weight = 2;	
		         for(iCellIndex = 1; iCellIndex <= rows[iRowIndex].cells.length; iCellIndex++)
		         {		        
		           var headercolumn = rows[iRowIndex].cells;		          
		            if((headercolumn[iCellIndex-1].id != "hidePrintCol") )
		            { 
		              displayLength = displayLength + 1;
		            }
		         }    	        		        
		    }
		    
            var column = rows[iRowIndex].cells;
            var colLength = column.length;
            
            var updateXValue = false;
		    for(iColIndex = 1; iColIndex <= colLength; iColIndex++)
		    {
		     
		    if((column[iColIndex-1].id != "hidePrintCol"))
		    { 
			            xls.Cells(iBeginIndex+1, iColIndex).FormulaLocal = "'" + column[iColIndex-1].innerText.Trim();					
			            xls.Cells(iBeginIndex+1, iColIndex).Borders.Weight = 2;
		                xls.columns.AutoFit();				    
		                if(iBeginIndex == 1)
	                    {
	                        xls.Cells(iBeginIndex+1, iColIndex).Font.Bold = true;				    
		                    xls.Cells(iBeginIndex+1, iColIndex).Interior.ColorIndex = 15;		                    
	                    }
	                    updateXValue = true;	                    
	         }
	         if(iColIndex==colLength)
	            {
	                if(updateXValue)
	                    iBeginIndex=iBeginIndex+1;
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

/// ====================================
/// Module Name:Well Test Data
/// Description: element in run data
/// Date:16-July-2010
/// Modified Lines:7959-7968
/// ====================================

/***************************************
    function to show/hide the column groups
***************************************/
function ShowHideWellTestColumnGroups(parentChkId,tblId)
{
	var chkParent = document.getElementById(parentChkId);			
	var tblParent = document.getElementById(tblId);
	if((chkParent.checked))//&&(chkChild.checked))
	{
		for(var iRowIndex=0;iRowIndex<tblParent.rows.length;iRowIndex++)
		{

			for(var iCellIndex=0;iCellIndex<tblParent.rows[iRowIndex].cells.length;iCellIndex++)
			{
				if(tblParent.rows[iRowIndex].cells[iCellIndex].style.display == "none")
				{
					tblParent.rows[iRowIndex].cells[iCellIndex].style.display = "block";
				}
			}
		}
	}
	
	else 
	{
		for(var iRowIndex=0;iRowIndex<tblParent.rows.length;iRowIndex++)
		{

			for(var iCellIndex=0;iCellIndex<tblParent.rows[iRowIndex].cells.length;iCellIndex++)
			{
				if(tblParent.rows[iRowIndex].cells[iCellIndex].style.display == "block")
				{
					tblParent.rows[iRowIndex].cells[iCellIndex].style.display = "none";
				}
			}
		}
	}
}

/************************************************
Exporting Search Results from report service to Excel.
*************************************************/
function ExportWellTestSearchResults()
  {
    alert('This option will only export the records displayed on the current page. If you have multiple pages you will have to export each page separately.'); 
   
    document.body.style.cursor = 'wait';
    String.prototype.Trim = function () 
       {
         return this.replace(/\|*\s*$/, "");
       }     
        var strTitle = "";
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
		                    for(var iArrSpanLen=0;iArrSpanLen<arrSpan.length;iArrSpanLen++)
		                        {		                      		                        
		                            if(arrSpan[iArrSpanLen].className==arrTable.parentElement.parentElement.id)
		                                {
		                                    strTitle  = arrSpan[iArrSpanLen].innerText;
		                                    break;
		                                }
		                        }			                       
			            
			              if(strTitle!="")
			                xslSheet.Name = strTitle; 			           
		                 rows = arrTable.rows;
                            for (iRowIndex = 0; iRowIndex < rows.length; iRowIndex++)
                                {                                
                                            var column = rows[iRowIndex].cells;                                         
                                            for (iCellIndex = 0; iCellIndex < column.length; iCellIndex++)
                                                {
                                                    if (iRowIndex == 2)
                                                  {                                                
                                                    xslSheet.Cells(iRowIndex+1, iCellIndex+1).FormulaLocal = column[iCellIndex].innerText.Trim();
                                                    xslSheet.Cells(iRowIndex+1, iCellIndex+1).Font.Bold = true;
                                                    xslSheet.Cells(iRowIndex+1, iCellIndex+1).Interior.ColorIndex = 15;                                                   
                                                   }
                                                   else
                                                   {
                                               
                                                 if(iCellIndex < rows[2].cells.length)
                                                 {
                                                   if(rows[2].cells[iCellIndex].type != null && rows[2].cells[iCellIndex].type!="" && rows[2].cells[iCellIndex].type == "Number" )
					                                    {
					                                         xslSheet.Cells(iRowIndex+1, iCellIndex+1).NumberFormat = "0.00";
					                                         xslSheet.Cells(iRowIndex+1, iCellIndex+1).HorizontalAlignment = -4152;//XlRight is the constant -4152 
					                                    }
					                                 else if(rows[2].cells[iCellIndex].type != null && rows[2].cells[iCellIndex].type!="" && rows[2].cells[iCellIndex].type == "date" )
					                                     {
					                                         xslSheet.Cells(iRowIndex+1, iCellIndex+1).NumberFormat = GetDateFormat();
					                                         xslSheet.Cells(iRowIndex+1, iCellIndex+1).HorizontalAlignment = -4131;//xlLeft is the constant -4131 
                					                      
					                                     }
					                                 else
					                                     {
					                                      if((rows[2].cells[iCellIndex].innerText.indexOf("UWI")>=0)||(rows[2].cells[iCellIndex].innerText.indexOf("UWBI")>=0) )
					                                      {
					                                         xslSheet.Cells(iRowIndex+1, iCellIndex+1).NumberFormat = "0";
					                                      }
					                                         xslSheet.Cells(iRowIndex+1, iCellIndex+1).HorizontalAlignment = -4131;//xlLeft is the constant -4131 
					                                     }     
                                                     xslSheet.Cells(iRowIndex+1, iCellIndex+1).Value = column[iCellIndex].innerText;
                                                         } /// If to check no of cells ends
                                                   }  /// Else ends
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
 
 
 /***************************************************************
            function to validate dates in EDM Report
***************************************************************/
function ValidateWellTestReportDates()
{
   return CallValidateDateService("txtStartDate","txtEndDate");
}
/**************************************************
function to open Chart in new window.
***************************************************/     

 function OpenChartLink(control,selectedUWBI,selectedUWI,searchType,criteriaName)
{     
      var url = "";
  document.forms[0].elements[GetObjectID('hidReportSelectRow','input')].value = '';
      document.forms[0].elements[GetObjectID('hidReportSelectRow','input')].value = selectedUWBI;
      var selectedCriteriaName = document.forms[0].elements[GetObjectID('hidSelectedCriteriaName','input')].value;
 
 var selectedDepthUnit = "Feet";
        if(document.forms[0].elements[GetObjectID('rbFeet','input')].checked)
        {
        selectedDepthUnit = "Feet";
        }
        else if(document.forms[0].elements[GetObjectID('rbMeters','input')].checked)
        {
        selectedDepthUnit = "Metres";
        }
      
       if(searchType == "DirectionalSurveyDetail")
      {
      url = "/Pages/DREAMDirSurveyChart.aspx?hidReportSelectRow="+selectedUWBI+"&hidSelectedCriteriaName="+selectedCriteriaName+"&hdnSelectedDepthUnit="+selectedDepthUnit+"&hdnSelectedUWI="+selectedUWI;
      }
      else if(searchType == "PicksDetail")
      {
      url = "/Pages/DREAMPicksChart.aspx?hidReportSelectRow="+selectedUWBI+"&hidSelectedCriteriaName="+selectedCriteriaName+"&hdnSelectedDepthUnit="+selectedDepthUnit+"&hdnSelectedUWI="+selectedUWI;
      }
        var msgWindow = window.open("", "searchTypeChart", 'width=800,height=600,scrollbars=yes,resizable=yes,status=yes,toolbar=no,left=100,top=100');
        document.forms[0].action = url ;
        document.forms[0].method = "post";
        document.forms[0].target = "searchTypeChart";
        document.forms[0].submit();         
        
        document.forms[0].target="_self";
   document.forms[0].action= msgWindow.opener.location.href;
   document.forms[0].method="post"; 
   msgWindow.focus();  
}

/*  function OpenChartLink(control,selectedUWBI,selectedUWI,searchType,criteriaName)
{

      document.forms[0].elements[GetObjectID('hidReportSelectRow','input')].value = '';
      document.forms[0].elements[GetObjectID('hidReportSelectRow','input')].value = selectedUWBI;
      var selectedCriteriaName = document.forms[0].elements[GetObjectID('hidSelectedCriteriaName','input')].value;
   
      var selectedDepthUnit = "Feet";
        if(document.forms[0].elements[GetObjectID('rbFeet','input')].checked)
        {
        selectedDepthUnit = "Feet";
        }
        else if(document.forms[0].elements[GetObjectID('rbMeters','input')].checked)
        {
        selectedDepthUnit = "Metres";
        }
     var url = "";
     if(searchType == "DirectionalSurveyDetail")
      {
      url = "/Pages/DREAMDirSurveyChart.aspx?hidReportSelectRow="+selectedUWBI+"&hidSelectedCriteriaName="+selectedCriteriaName+"&hdnSelectedDepthUnit="+selectedDepthUnit+"&hdnSelectedUWI="+selectedUWI;
      }
      else if(searchType == "PicksDetail")
      {
      url = "/Pages/DREAMPicksChart.aspx?hidReportSelectRow="+selectedUWBI+"&hidSelectedCriteriaName="+selectedCriteriaName+"&hdnSelectedDepthUnit="+selectedDepthUnit+"&hdnSelectedUWI="+selectedUWI;
      }
  
   var manager = GetRadWindowManager();
   var oWnd = manager.getWindowByName("objRadWindow");
   
    var popupWidth = document.body.clientWidth;
    var popupHeight = document.body.clientHeight;
     var cLeft  = (popupWidth /2) -(630/2);
    var cTop = (popupHeight/2) - (600/2);
    
   if(oWnd != null)
   {
    oWnd.setUrl(url);
    oWnd.set_title(searchType);

    oWnd.set_width(600);
    oWnd.set_height(630);
oWnd.Center();
    oWnd.show(); 
oWnd.MoveTo(cLeft,cTop);
   }

    return false;
    
}*/

/*********************************
Function to locate position of any control on the page.
************************************/
function FindControlPosition(obj)
 {

    var curleft =  0;
    var curtop = 0;
    if (obj.offsetParent) 
    {
    do 
    {
    		curleft += obj.offsetLeft;
    		curtop += obj.offsetTop;
    } while (obj = obj.offsetParent);
    return [curleft,curtop];
    }
}

/*********************************
Function change the Opacity of RadWindow while slide.
************************************/
 function OnClinetSlide(sender, eventArgs) 
 {
    var oWnd = GetRadWindow(); 
    //get a reference to the window and set its opacity               
    if(oWnd != null)
    {
    oWnd.set_opacity(sender.get_value());
    }
    return false;
}

/*********************************
Function to get reference to RadWindow on the page.
************************************/
 function GetRadWindow()   
    {   
        var oWindow = null;   
        if (window.radWindow) oWindow = window.radWindow;   
        else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;   
        return oWindow;   
    }  

/*********************************
Function to Show/Hide the div content.
************************************/
function ShowHide(obj)
	{

	   var lnkShowHide = document.getElementById(obj);
	   if(lnkShowHide != null && lnkShowHide.innerText == "Show filter options")
	   {
	   var divContent = document.getElementById('divContent');
	   if(divContent != null)
	   {
	      divContent.style.display = "block";
	      lnkShowHide.innerText = "Hide filter options";
	   }
	   }
	   else if(lnkShowHide != null && lnkShowHide.innerText == "Hide filter options")
	   {
	   var divContent = document.getElementById('divContent');
	   if(divContent != null)
	   {
	      divContent.style.display = "none";
	      lnkShowHide.innerText = "Show filter options";
	   }
	   }
	   return false;
	}
	
/*********************************
Function to conver between temperature units
************************************/
function TemperatureUnitConvertor(objSelectedRadioBtn)
{
	var table = document.getElementById('tblSearchResults');
	if(table == null)
	{
	    return;
	}
	var strSelectedUnit = objSelectedRadioBtn.value;
	var headerRowIndex = table.tHead.lastChild.rowIndex;
	var dataRowIndex = table.tBodies[0].firstChild.rowIndex;
	var arrIndex = new Array();
	var hdrRowCells = table.rows[headerRowIndex].cells;
	for(var i = 0; i < hdrRowCells.length; i++)
	    {
	        if(hdrRowCells[i].innerText.indexOf(' (degC)') >= 0)
	        {
	            arrIndex.push(i);
	            hdrRowCells[i].innerHTML = hdrRowCells[i].innerHTML.replace('(degC)','(degF)');
	        }
	        else if(hdrRowCells[i].innerText.indexOf(' (degF)') >= 0)
	        {
	            arrIndex.push(i);
	            hdrRowCells[i].innerHTML = hdrRowCells[i].innerHTML.replace('(degF)','(degC)');
	        }
	    }
	var rows = table.rows;
	for(var i = dataRowIndex; i < rows.length; i++)
	{
	    for(var j = 0; j < arrIndex.length; j++)
	    {
	        rows[i].cells[arrIndex[j]].innerText = ConvertorToTemperatureUnit(rows[i].cells[arrIndex[j]].innerText,strSelectedUnit);
	    }  
	}
}
/*********************************
Function to a value to a given temperature unit
************************************/
function ConvertorToTemperatureUnit(value,convertTo)
{
    var strConvertedValue = value;
    if(!isNaN(parseFloat(value)))
    {
        if(convertTo == "degC")
        {
          //  strConvertedValue = (parseFloat(value)/33.8).toFixed(2);
          //  c=(f-32)/1.8
          var farenheitValue = parseFloat(value);
          strConvertedValue = ((farenheitValue - 32)/1.8).toFixed(2);
        }
        else if(convertTo == "degF")
        {
          //  strConvertedValue = (parseFloat(value)*33.8).toFixed(2);
          // f=(c*1.8)+32
          var centigradeValue = parseFloat(value);
          strConvertedValue = ((centigradeValue * 1.8) + 32).toFixed(2);
        }
    }
    return strConvertedValue;
}
/*********************************
Function to conver between pressure units
************************************/
function PressureUnitConvertor(objSelectedRadioBtn)
{
	var table = document.getElementById('tblSearchResults');
	if(table==null)
	{
	    return;
	}
	var strCurrentUnit = objSelectedRadioBtn.parentNode.previousSelectedValue;//radiobutton list top span object
	var strSelectedUnit = objSelectedRadioBtn.value;
	var headerRowIndex = table.tHead.lastChild.rowIndex;
	var dataRowIndex = table.tBodies[0].firstChild.rowIndex;
	var arrIndex = new Array();
	var hdrRowCells = table.rows[headerRowIndex].cells;
	for(var i = 0; i < hdrRowCells.length; i++)
	    {
	        if(hdrRowCells[i].innerText.indexOf('(barA)') >= 0)
	        {
	            arrIndex.push(i);
	            hdrRowCells[i].innerHTML = hdrRowCells[i].innerHTML.replace('(barA)','(' + strSelectedUnit + ')');
	        }
	        else if(hdrRowCells[i].innerText.indexOf('(kPa)') >= 0)
	        {
	            arrIndex.push(i);
	            hdrRowCells[i].innerHTML = hdrRowCells[i].innerHTML.replace('(kPa)','(' + strSelectedUnit + ')');
	        }
	         else if(hdrRowCells[i].innerText.indexOf('(psiA)') >= 0)
	        {
	            arrIndex.push(i);
	            hdrRowCells[i].innerHTML = hdrRowCells[i].innerHTML.replace('(psiA)','(' + strSelectedUnit + ')');
	        }
	    }
	var rows = table.rows;
	for(var i = dataRowIndex; i < rows.length; i++)
	{
	    for(var j = 0; j < arrIndex.length; j++)
	    {
	        rows[i].cells[arrIndex[j]].innerText = ConvertorToPressureUnit(rows[i].cells[arrIndex[j]].innerText,strCurrentUnit,strSelectedUnit);
	    }   
	}
	//setting current selected unit to table property 'previousSelectedValue'
	objSelectedRadioBtn.parentNode.previousSelectedValue = objSelectedRadioBtn.value;
}
/*********************************
Function to convert a given value from a given pressuint to another
************************************/
function ConvertorToPressureUnit(value,convertFrom,convertTo)
{
    var strConvertedValue = value;
    if(!isNaN(parseFloat(value)))
    {
        if(convertFrom == "barA")
        {
            if(convertTo == "kPa")
            {
                strConvertedValue = (parseFloat(value)*100).toFixed(2);
            }
            else if(convertTo == "psiA")
            {
                strConvertedValue = (parseFloat(value)*14.503774).toFixed(2);
            }
        }
        else if(convertFrom == "kPa")
        {
            if(convertTo == "barA")
            {
                strConvertedValue = (parseFloat(value)*0.01).toFixed(2);
            }
            else if(convertTo == "psiA")
            {
                strConvertedValue = (parseFloat(value)*0.145038).toFixed(2);
            }
        }
        else if(convertFrom == "psiA")
        {
            if(convertTo == "barA")
            {
                strConvertedValue = (parseFloat(value)*0.068948).toFixed(2);
            }
            else if(convertTo == "kPa")
            {
                strConvertedValue = (parseFloat(value)*6.894745 ).toFixed(2);
            }
        }
    }
    return strConvertedValue;
}
/*********************************
Function to show hide context search filter opntions
************************************/
function ShowHideFilterOptions(expImage,divFilter)
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

/*********************************
Function to show selected items
************************************/
 function SetListBoxSelectedValues(controlID)
   {
     
       var lst = document.getElementById(controlID);
       if(lst && lst.selectedIndex -1)
       {
           for(i = lst .options.length-1; i >=0; i--)
           {       
              if(lst.options[i].selected)
               {
                 lst.options[i].selected=true;
               } 

          
           }
       }
   
   }
   
/*********************************
Function to called on click of Close button.
It closes the busybox instance on the RadWindow 
************************************/
function OnClientClose(sender, eventArgs)
 {  
     try
    {
        if(document.getElementById("BusyBoxIFrame")!= null)
        {                 
            busyBox.Hide();    
            return true;       
        }       
    }
    catch(Ex)
    {
        return true;
    }
 }
 
 /*********************************
Function to called on node click in Query Builder screen. [QueryBuilder.ascx]
Expand/Collapse the selected node.
************************************/
  function OnQueryBuilderClientNodeClicking(sender, eventArgs)
   { 
     var node = eventArgs.get_node(); 
          
       if(node._getChildren().get_count()>0)
        {  
                if(node.get_expanded())
                {
                 node.collapse();                
              
                }
                else
                {
                  node.expand();          
                
                 }
                 eventArgs.set_cancel(true);         
       
         }
   
   }
   
/**************************************
Function to open Context Search Menu popup
***************************************/
function ListContextSearchPopup(url,searchType) 
{
    var title = searchType.toString();
    var msgWindow = window.open("", title, 'width=800,height=600,scrollbars=yes,resizable=yes,status=yes,left=100,top=100');
    document.forms[0].action = url + "?asset=" + searchType + "&listSearchType=" + searchType + "&country=0";
    document.forms[0].method="post";
    document.forms[0].target = title;
    document.forms[0].submit(); 
    document.forms[0].target="_self";
    document.forms[0].action= msgWindow.opener.location.href;
    document.forms[0].method="post"; 
    msgWindow.focus();  
}
/**************************************
Function called on Client Actions in RadWindow 
Client actions like Minimise,Restore, Maximise
***************************************/
function OnClientCommand(sender, eventArgs)
{
    var commandName = eventArgs.get_commandName();
    var manager = GetRadWindowManager();
    var oWnd = manager.getWindowByName("objRadWindow");
    var popupWidth = document.body.clientWidth;
    var popupHeight = document.body.clientHeight;
    
   if(commandName == 'Minimize')
   {   
   /// Calculate Left and Top co-ordinates   
    var cLeft  = (popupWidth /2) -(630/2);
    var cTop = popupHeight;
    
       if(oWnd != null)
       {        
       oWnd.MoveTo(0,cTop);
       }
   }
   else if(commandName == 'Restore')
   {     
   /// Calculate Left and Top co-ordinates
    var cLeft  = (popupWidth /2) -(630/2);
    var cTop = (popupHeight/2) - (600/2);
    
       if(oWnd != null)
       {        
       oWnd.MoveTo(cLeft,cTop);
       }
   }
   return false;
}


/***************************************
    Print option for Mechanical data Report.
***************************************/
function DWBResultsRprintContent(tableName)
{   
       var objWindow;
       var counter = 0;
       var isConfirmed = confirm("This option will print the records displayed on the current page.")
       if(isConfirmed == true)
       {		   
            var attributes="toolbar=yes,location=no,directories=yes,resizable,menubar=yes,";
            attributes+="scrollbars=yes,width=800, height=600, left=100, top=25";
            objWindow=window.open("/_layouts/dream/printDWBListOfBooks.htm","Print",attributes);   
            objWindow.opener =window.self; 
            objWindow.focus();     
        }
        else
        {
           alert("Print option is enabled only for search results and details page.");
        }      
}
/************************************************
function to get Radio Button List selected item
*************************************************/
function GetRdoBtnLstSelectedItem(radioBtnLstID)
{
    var rdbLst= document.getElementById(GetObjectID(radioBtnLstID,'span')); 
    var rdbLstCollection  =rdbLst.getElementsByTagName("input");
    var objSelectedItem = null;
    for(var i = 0; i < rdbLstCollection.length; i++ )
    {      
        if(rdbLstCollection[i].checked == true)
        {
            objSelectedItem = rdbLstCollection[i];
            break;
        }
    }        
    return objSelectedItem;	 
}
/*********************************
Function to conver between temperature units in tab report
************************************/
function TemperatureUnitConvertorInTabReport()
{

     for(var index=0;index<arguments.length;index++)
	{
	    var table = document.getElementById(arguments[index]);
	    if(table == null)
	    {
	        continue;
	    }
	    var strSelectedUnit =  GetRdoBtnLstSelectedItem('rdoTemperatureUnit').value;
	    var headerRowIndex = table.tHead.lastChild.rowIndex;
	    var dataRowIndex = table.tBodies[0].firstChild.rowIndex;
	    var arrIndex = new Array();
	    var hdrRowCells = table.rows[headerRowIndex].cells;
	    for(var i = 0; i < hdrRowCells.length; i++)
	        {
	            if(hdrRowCells[i].innerText.indexOf(' (degC)') >= 0)
	            {
	                arrIndex.push(i);
	                hdrRowCells[i].innerHTML = hdrRowCells[i].innerHTML.replace('(degC)','(degF)');
	            }
	            else if(hdrRowCells[i].innerText.indexOf(' (degF)') >= 0)
	            {
	                arrIndex.push(i);
	                hdrRowCells[i].innerHTML = hdrRowCells[i].innerHTML.replace('(degF)','(degC)');
	            }
	        }
	    var rows = table.rows;
	    for(var i = dataRowIndex; i < rows.length; i++)
	    {
	        for(var j = 0; j < arrIndex.length; j++)
	        {
	            rows[i].cells[arrIndex[j]].innerText = ConvertorToTemperatureUnit(rows[i].cells[arrIndex[j]].innerText,strSelectedUnit);
	        }  
	    }
	}
}
/*********************************
Function to conver between pressure units in tab report
************************************/
function PressureUnitConvertorInTabReport()
{
	 for(var index=0;index<arguments.length;index++)
	{
	    var table = document.getElementById(arguments[index]);
	    if(table==null)
	    {
	        continue;
	    }
	    var objSelectedItem =GetRdoBtnLstSelectedItem('rdoPressureUnit');
	    var strCurrentUnit = objSelectedItem.parentNode.previousSelectedValue;//radiobutton list top span object
	    var strSelectedUnit = objSelectedItem.value;;
	    var headerRowIndex = table.tHead.lastChild.rowIndex;
	    var dataRowIndex = table.tBodies[0].firstChild.rowIndex;
	    var arrIndex = new Array();
	    var hdrRowCells = table.rows[headerRowIndex].cells;
	    for(var i = 0; i < hdrRowCells.length; i++)
	        {
	            if(hdrRowCells[i].innerText.indexOf('(barA)') >= 0)
	            {
	                arrIndex.push(i);
	                hdrRowCells[i].innerHTML = hdrRowCells[i].innerHTML.replace('(barA)','(' + strSelectedUnit + ')');
	            }
	            else if(hdrRowCells[i].innerText.indexOf('(kPa)') >= 0)
	            {
	                arrIndex.push(i);
	                hdrRowCells[i].innerHTML = hdrRowCells[i].innerHTML.replace('(kPa)','(' + strSelectedUnit + ')');
	            }
	             else if(hdrRowCells[i].innerText.indexOf('(psiA)') >= 0)
	            {
	                arrIndex.push(i);
	                hdrRowCells[i].innerHTML = hdrRowCells[i].innerHTML.replace('(psiA)','(' + strSelectedUnit + ')');
	            }
	        }
	    var rows = table.rows;
	    for(var i = dataRowIndex; i < rows.length; i++)
	    {
	        for(var j = 0; j < arrIndex.length; j++)
	        {
	            rows[i].cells[arrIndex[j]].innerText = ConvertorToPressureUnit(rows[i].cells[arrIndex[j]].innerText,strCurrentUnit,strSelectedUnit);
	        }   
	    }	   
	}
	/// NOTE: Don't add this line inside the arguments loop. 
	/// Adding this line inside arguments loop will stop the values in second tab and others converting the values.
	
	 //setting current selected unit to table property 'previousSelectedValue'
	    objSelectedItem.parentNode.previousSelectedValue = objSelectedItem.value;
}