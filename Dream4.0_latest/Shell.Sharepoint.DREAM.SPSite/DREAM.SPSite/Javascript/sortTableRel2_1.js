var flag = 0;
var unitValue = "";
var table;				
var ttable;				
var tableId;
var ttableId;			
var tableIdArray = new Array();
var sortDirArray = new Array();
var tableCount = 0;			
var rowArray = new Array();	
var rowIdArray = new Array();
var rowPropArray = new Array();
var titleRowArray = new Array();
var titleInnerHTMLArray = new Array();
var titleSpanCountArray = new Array();
var titleRowCellArray = new Array();
var titleSpanCellArray = new Array();
var colSpanArray = new Array();
var colTitleFilled = new Array();
var sortIndex;
var nRow, actualNRow, maxNCol;
var isIE;
var separateTitle = false;	
var linkEventString = 'onMouseOver=\'setCursor(this);' + '\' ' + 'onClick="setCursorWait(this);setTimeout(\'sortTable(';
var cellPropArray = new Array(	
	"align", "vAlign", "Blue",
	"noWrap", "width", "height", 
	"borderColor",
	"borderColorLight",
	"borderColorDark");
var colsToIgnoreArray;			
var colsToIgnore;			

// Configurable constants
var useCustomTitleFont = false;
//var titleFont = "Arial";
var recDelimiter = '|';
var cellPropDelimiter = ",";
var doSortUponLoad = false;
var defaultSortColumn = 0;
var useEuroDate = false;
var columnArray = new Array();	


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

// Main function. This is to be associated with onLoad event in <BODY>. 
function initTable(obj, ignore)
{	
	var tIndex;
	// Cols to ignore
	colsToIgnore = ignore;
	if (colsToIgnore != null) 
		colsToIgnoreArray = ignore.split(",");
	else
		colsToIgnore = null;

	// Local variables
	var countCol;
	var currentCell;
	var nColSpan, nRowSpannedTitleCol, colPos;
	var titleFound = false;
	var skipRow = false;
	var rNRowSpan, rNColSpan, parentNodeName;
	var cCellContent, cCellSetting;
	var sObj;
	var ctable;
	var doneftable = false;
	var cmd;
	var tableLoaded = false;

	// Initializing global table object variable
	if (obj.tagName == "TABLE")
	{
		// Assumes that the obj is THE OBJECT
		table = obj;
	}
	else
	{
		table = document.getElementById(obj);
		sObj = obj.id + "title";
		ttable = document.getElementById(sObj);
	}

	// Check whether it's an object
	if (table == null) return;

	// Check whether it's a table
	if (table.tagName != "TABLE") return;

	if (ttable != null && ttable.tagName == "TABLE")
		separateTitle = true;

	// No need to re-init if it's already done
	if (tableId == table.id && table.tainted == false) return;

	// Setting table id
	tableId = table.id;

	if (separateTitle) ttableId = ttable.id;

	// Initializing the max col number with the size of last data row
	maxNCol = table.rows[table.rows.length-1].cells.length;

	// Initializing arrays
	rowArray = new Array();
	rowIdArray = new Array();
	rowPropArray = new Array();
	colSpanArray = new Array();
	colTitleFilled = new Array();
	titleRowArray = new Array();
	titleInnerHTMLArray = new Array();
	titleSpanCountArray = new Array();
	titleRowCellArray = new Array();
	columnArray = new Array();
	
	for (var i=0; i<maxNCol; i++)
	{
		if (i == 0) continue;
		colTitleFilled[i] = false;
	}

	// Setting the number of rows
	nRow = table.rows.length;	
	

	// Should have at least 1 row
	if (nRow < 1) return;

	// Initialization of local variables
	actualNRow = 0;			// Number of actual data rows
	rNRowSpan = 0;			// Remaining rows in the row span
	rNColSpan = 0;			// Remaining cols in the col span
	nRowSpannedTitleCol = 0;	// Number of title cols from row span

	// Loop through rows
	for (var i=0; i<nRow; i++)
	{
		if (! separateTitle)
		{
			ctable = table;
		}
		else if (i == 0 && separateTitle && titleFound == false) 
		{
			ctable = ttable;
			nRow = ttable.rows.length + 1;
		}

		if (! doneftable && separateTitle && titleFound)
		{
			ctable = table;
			nRow = table.rows.length;
			i = 0;
			doneftable = true;
		}

		skipRow = false;
		// Skip if it's THEAD, TFOOT
		if (ctable.rows[i].parentNode != null)
		{
			parentNodeName = ctable.rows[i].parentNode.nodeName;
			parentNodeName.toUpperCase();
			if (parentNodeName == 'THEAD' ||
				parentNodeName == 'TFOOT')
			{
				skipRow = true;
			}
		}	
		nColSpan = 1, colPos = 0;
		// Loop through columns
		for (var j=0; j<ctable.rows[i].cells.length; j++)
		{
			// Do this if title has not been found
			if (titleFound == false)
			{
				if (ctable.rows[i].cells[j].rowSpan > 1)
				{
					if (ctable.rows[i].cells[j].colSpan < 2)
					{
						titleSpanCellArray[colPos] = ctable.rows[i].cells[j];
						titleRowArray[colPos] = ctable.rows[i];
						colTitleFilled[colPos] = true;
						nRowSpannedTitleCol++;
					}
					if (ctable.rows[i].cells[j].rowSpan - 1 > rNRowSpan)
					{
						rNRowSpan = 
							ctable.
							rows[i].cells[j].
							rowSpan - 1;

						if (ctable.rows[i].
							cells[j].colSpan > 1)
							rNColSpan = 
								rNRowSpan + 1;
					}
				}
			}
			if (ctable.rows[i].cells[j].colSpan > 1 &&
				rNColSpan == 0)
			{ 
				nColSpan = ctable.rows[i].cells[j].colSpan;
				colPos += nColSpan;
			}
			else
			{
				colPos++;
			}		
		}
					
		// Setting up the title cells
		if (titleFound == false && nColSpan == 1 && rNRowSpan == 0 && rNColSpan == 0)
		{
			colSpanArray[i] = true;
			titleFound = true;

			// Using individual cell as an array element
			countCol = 0;
			for (var j=0; j<ctable.rows[i].cells.length + nRowSpannedTitleCol; j++)
			{
				if (colTitleFilled[j] != true)
				{
					titleRowCellArray[j] = ctable.rows[i].cells[countCol];
					titleRowArray[j] = ctable.rows[i];
					countCol++;
				}
				else
				{
					titleRowCellArray[j] = titleSpanCellArray[j];					
				}
				titleInnerHTMLArray[j] = String(titleRowCellArray[j].innerHTML);
				titleSpanCountArray[j] = titleRowCellArray[j].rowSpan;
			}
		}
		// Setting up the data rows
		else if (titleFound == true && nColSpan == 1 && rNRowSpan == 0 && !skipRow)
		{
		    for (var j=0; j<ctable.rows[i].cells.length; j++)
			{
				// Can't have row span in record rows ...
				if (ctable.rows[i].cells[j].rowSpan > 1) return;
				currentCell = ctable.rows[i].cells[j];                
				cCellContent = String(currentCell.innerHTML);

				for (var k=0; k<cellPropArray.length; k++)
				{
					if (k == 0)
						cmd = "cCellSetting=" +	"String(currentCell." + cellPropArray[k] + ");"
					else
						cmd = "cCellSetting+=" + "cellPropDelimiter+" + "String(currentCell." +	cellPropArray[k] + ");"
				    eval(cmd);
				}

				if (j == 0)
				{				
					rowArray[actualNRow] = cCellContent;
					rowPropArray[actualNRow] = cCellSetting;
				}
				else
				{   
					rowArray[actualNRow] += recDelimiter + cCellContent;
					rowPropArray[actualNRow] += recDelimiter + cCellSetting;
				}
				
				if (j == ctable.rows[i].cells.length-1)
					rowArray[actualNRow] += recDelimiter + String(actualNRow);
			}
			
			// Inconsistent col lengh for data rows
			if (ctable.rows[i].cells.length > maxNCol)
				return;
			actualNRow++;
			colSpanArray[i] = false;
		}
		else if (nColSpan == 1 && rNRowSpan == 0 && 
			rNColSpan == 0 && titleFound == false && !skipRow)
		{
			colSpanArray[i] = false;
		}
		else
		{
			colSpanArray[i] = true;
		}
		
		// Counters for row/column spans
		if (rNRowSpan > 0) rNRowSpan--;
		if (rNColSpan > 0) rNColSpan--;
	}

	// If the row number is < 1, no need to do anything ...
	if (actualNRow < 1) return;

	// Sorting upon loading the page
	tIndex = findTableIndex(table.id);
	if (tIndex >= 0) tableLoaded = true;
	if (!tableLoaded)
	{
		tableIdArray[tableCount] = table.id;
		sortDirArray[tableCount] = false;
		tIndex = tableCount;
		tableCount++;
	}

	// Re-drawing the title row
	redrawTitle(false);

	// Sort if doSortUponLoad is set
	if (doSortUponLoad) 
	{
		sortTable(defaultSortColumn, table.id, 0);
		if (!tableLoaded) sortDirArray[tIndex] = true;
	}	
}

// Function called to re-draw title row 
function redrawTitle(isSort)
{
	var currentRow, innerHTML, newInnerHTML, cellIndex;
	var reAnchor, reUpDown, reLabel, cellAlign, makeBold;
	var cellOnmouseover, cellOnmouseout;
 	var cellClass;
	var cmd;
	var cCellPropArray = new Array();
	var tIndex;

	cellAlign = "";
	makeBold = false;
	reAnchor = / *\<a[^\>]*\>(.*) *\<\/a\>/i;
	reUpDown = /\<font[^\>]* *id=.*updown.*[^\>]*\>.*\<\/font\>/i;
	reLabel = /\>([^\<]*)\</g;

	tIndex = findTableIndex(table.id);

	// Re-drawing the title row
	for (var j=0; j<maxNCol; j++)
	{
		currentRow = titleRowArray[j];
		innerHTML = String(titleInnerHTMLArray[j]);
		cellIndex = titleRowCellArray[j].cellIndex;

		// Skips the columns if set to ignore
		if (colsToIgnore != null)
			if (indexExist(colsToIgnoreArray,cellIndex)) continue;

		// Recording cell settings
		for (var k=0; k<cellPropArray.length; k++)
		{
			cmd = "cCellPropArray[" + k + 
				"]=titleRowCellArray[j]." + 
				cellPropArray[k] + ";";
			eval(cmd);
		}
		cellClass = titleRowCellArray[j].getAttribute('className');

		currentRow.deleteCell(cellIndex);
		currentRow.insertCell(cellIndex);

		// Setting the font type for the title
		if (cellAlign != "")
			currentRow.cells[cellIndex].align = cellAlign;
		if (titleRowCellArray[j].tagName == "TH")
		{
			makeBold = true;
		}

		// Restoring cell settings
		for (var k=0; k<cellPropArray.length; k++)
		{
			cmd = "currentRow.cells[cellIndex]." + cellPropArray[k] + "=cCellPropArray[" + k + "];";
			eval(cmd);
		}

		currentRow.cells[cellIndex].setAttribute('className', cellClass, 0);

		if (titleSpanCountArray[j] > 1)
			currentRow.cells[cellIndex].rowSpan = titleSpanCountArray[j];
		newTitle = '';

		// Remove carriage return, linefeed, and tab
		innerHTML = innerHTML.replace(/\r|\n|\t/g, "");
		if (makeBold)
		{
			if (innerHTML.match(reLabel))
				innerHTML = 
					innerHTML.replace(reLabel, "<b>$1</b>");
			else
				innerHTML =
					innerHTML.replace(
						/(^.*$)/, "<b>$1</b>");
		}
		innerHTML = innerHTML.replace(reUpDown, "");
		innerHTML = innerHTML.replace(reAnchor, "$1");
		newInnerHTML =
			'<a ' + linkEventString + j + ',' +
			tIndex + ',' + 1 + ',' + 1 + ')\', 1);">';
		if (useCustomTitleFont)
			newInnerHTML += '<font face="' + titleFont + '">' +
				+ innerHTML + '</font>';
		else
			newInnerHTML += innerHTML;
		newInnerHTML += '</a>' + newTitle;

		currentRow.cells[cellIndex].innerHTML = newInnerHTML;
		titleRowCellArray[j] = currentRow.cells[cellIndex];		
	}
}

// Function called when user clicks on a title to sort
function sortTable(index, tobj, doInit, isIndex)
{
    if(index != 0)
    {
	    var obj, tIndex;
	    if (isIndex != null)
		    if (isIndex)
		    {
			    tIndex = tobj;
			    // Look up the table object
			    obj = tableIdArray[tobj];
		    }	
		    else
		    {
			    tIndex = findTableIndex(tobj.id);
			    obj = tobj;
		    }

	    // Re-inializing the table object
	    if (doInit) 
	    {
		    if (colsToIgnore != null) 
			    initTable(obj, colsToIgnore);
		    else
			    initTable(obj);
	    }
	    // Local variables
	    var rowContent, rowProp, cellProp;
	    var rowCount;
	    var rowIndex;
 	    var cellClass, cellOnClick, cellMouseOver, cellMouseOut;
	    var cmd;
    	
	    // Can't sort past the max allowed column size
	    if (index < 0 || index >= maxNCol) return;
    	
	    // Assignment of sort index
	    sortIndex = index;
	    // Doing the sort using JavaScript generic function for an Array
	    rowArray.sort(compare);

	    // Re-drawing the title row
	    if (doInit || doSortUponLoad) redrawTitle(true);

	    // Re-drawing the table
	    rowCount = 0;
	    for (var i=0; i<nRow; i++)
	    {
		    if (! colSpanArray[i])
		    {
			    for (var j=0; j<maxNCol; j++)
			    {
			        // Skips the columns if set to ignore
				    if (colsToIgnore != null)
					    if (indexExist(colsToIgnoreArray,j)) 
						    continue;
				    rowContent = rowArray[rowCount].
					    split(recDelimiter);
				    rowIndex = rowContent[maxNCol];
				    rowProp = rowPropArray[rowIndex].split(recDelimiter);
				    cellProp = rowProp[j].split(cellPropDelimiter);
				    cellClass = table.rows[i].cells[j].getAttribute('className');
 				    cellOnClick = table.rows[i].cells[j].onclick;
 				    cellMouseOver = table.rows[i].cells[j].onmouseover;
 				    cellMouseOut = table.rows[i].cells[j].onmouseout;

				    table.rows[i].deleteCell(j);
				    table.rows[i].insertCell(j);

				    // Restoring cell properties
				    for (var k=0; k<cellPropArray.length; k++)
				    {
					    cmd = "table.rows[i].cells[j]." + cellPropArray[k] + "=cellProp[" + k + "];";
					    eval(cmd);
				    }

				    table.rows[i].cells[j].innerHTML = rowContent[j];
				    table.rows[i].cells[j].setAttribute('className', cellClass, 0);
 				    table.rows[i].cells[j].onclick = cellOnClick;
 				    table.rows[i].cells[j].onmouseover = cellMouseOver;
 				    table.rows[i].cells[j].onmouseout = cellMouseOut;
			    }
			    rowCount++;
		    }
	    }
	    // Switching btw descending/ascending sort
	    if (doInit)
	    {
		    if (sortDirArray[tIndex])
			    sortDirArray[tIndex] = false;
		    else
			    sortDirArray[tIndex] = true;
	    }
	}	
	setCursorDefault();	
}

// Function to be used for Array sorting
function compare(a, b)
{
	// Getting the table index
	var tIndex;
	tIndex = findTableIndex(table.id);
 
	// Getting the element array for inputs (a,b)
	var aRowContent = a.split(recDelimiter);
	var bRowContent = b.split(recDelimiter);

	// Needed in case the data conversion is necessary
	var aToBeCompared, bToBeCompared;

	// Remove HTML tags 
	reRowText = /(\< *[^\>]*\>|\&nbsp\;)/g;
	aRowContent[sortIndex] = aRowContent[sortIndex].replace(reRowText, "");
	bRowContent[sortIndex] = bRowContent[sortIndex].replace(reRowText, "");

	// Euro Date format
	if (useEuroDate)
	{
		aRowContent[sortIndex] = 
			convertEuroDate(aRowContent[sortIndex]);
		bRowContent[sortIndex] = 
			convertEuroDate(bRowContent[sortIndex]);
	}

	if (isDate(aRowContent[sortIndex]) && isDate(bRowContent[sortIndex]))
	{
		aToBeCompared = new Date(aRowContent[sortIndex]);
		bToBeCompared = new Date(bRowContent[sortIndex]);
	}
	else if (! isNaN(aRowContent[sortIndex]) &&
		! isNaN(bRowContent[sortIndex]))
	{
		aToBeCompared = parseFloat(aRowContent[sortIndex], 10);
		bToBeCompared = parseFloat(bRowContent[sortIndex], 10);
	}
	else
	{
		aToBeCompared = aRowContent[sortIndex];
		bToBeCompared = bRowContent[sortIndex];
	}

	if (aToBeCompared < bToBeCompared)
		if (!sortDirArray[tIndex])
		{
			return -1;
		}
		else
		{
			return 1;
		}
	if (aToBeCompared > bToBeCompared)
		if (!sortDirArray[tIndex])
		{
			return 1;
		}
		else
		{
			return -1;
		}
	return 0;
}

// Function to determine whether it's a IETF recognized date string
function isDate(x)
{
	var xDate;
	xDate = new Date(x);
	if (xDate.toString() == 'NaN' || 
		xDate.toString() == 'Invalid Date')
		return false;
	else
		return true;
}

// Function to convert EURO date
function convertEuroDate(x)
{
	var reExp = /^ *(\d{2})\-(\d{2})\-(\d{4}) *$/g;
	var dArray;
	if (x.match(reExp))
		x = x.replace(reExp, "$2\/$1\/$3");	
	return(x);
}

// Functions to set the cursor
function setCursor(obj)
{
	var rowText, reRowText;

	reRowText = /(\< *[^\>]*\>|\&nbsp\;)/g;
	// Show hint text at the browser status bar
	rowText = String(obj.innerHTML);

	// Remove up/down 
	rowText = rowText.replace(/\<font id\=updown.*\<\/font\>/ig, "");
	// Remove HTML tags and &nbsp;
	rowText = rowText.replace(reRowText, "");
	// Remove carriage return, linefeed, and tab
	rowText = rowText.replace(/\r|\n|\t/g, "");
	
	// Setting window's status bar
	window.status = "Click to sort by " + String(rowText);

	// Setting title
	obj.title = "Click to sort by " + String(rowText);

	// Change the mouse cursor to pointer or hand
	if (isIE)
		obj.style.cursor = "hand";
	else
		obj.style.cursor = "pointer";
}

function setCursorWait(obj)
{
	if (isIE && document.all)
		for (var i=0;i<document.all.length;i++) 
			document.all(i).style.cursor = 'wait';
	else
	{
		obj.style.cursor = 'wait';
		document.body.style.cursor = 'wait';
	}
}

function setCursorDefault()
{
	if (isIE && document.all)
		for (var i=0;i<document.all.length;i++) 
			document.all(i).style.cursor = '';
	else
		document.body.style.cursor = '';
}

// Function to check browser type/version
function checkBrowser()
{
	if (navigator.appName == "Microsoft Internet Explorer"
		&& parseInt(navigator.appVersion) >= 4)
	{
		isIE = true;
		return true;
	}
	// For some reason, appVersion returns 5 for Netscape 6.2 ...
	else if (navigator.appName == "Netscape"
		&& navigator.appVersion.indexOf("5.") >= 0)
	{
		isIE = false;
		return true;
	}
	else
		return false;
}

// Function to check whether index exists in array 
function indexExist(x, index)
{
	var found = false;
	for (var i=0; i<x.length; i++)
	{
		if (index == x[i])
		{
			found = true;
			break;
		}
	}
	return found;
}

// Function to return table index given table id 
function findTableIndex(id)
{
	for (var i=0; i<tableCount; i++)
		if (tableIdArray[i] == id)
			return(i);
	return(-1);
}


/************************************************************
        function to convert feet-meters and vice versa
************************************************************/
function ConvertFeetMeters(idVal)
{
    var userPreferenceValue = document.forms[0].elements[GetObjectID('hidDepthUnit','input')].value;
    
    if((userPreferenceValue == '') || (userPreferenceValue == 'GO'))
    {        
        if (idVal.indexOf(document.forms[0].elements[GetObjectID('hidDepthUnit','input')].value)>=0)
        {           
        }
        else
        {
            userPreferenceValue = 'changed';     
	        if(unitValue == '')
	        {
	            if(idVal == "rbMeters")
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
	            var strReportType = document.forms[0].elements[GetObjectID('hidReportType','input')].value ;			
            	
	            if(strReportType.indexOf("SearchContext")>=0)
	            {
	                strReportType = strReportType.replace('SearchContext','');
	            }
            	
	            disableEnableFeetMeter(idVal);
                if((strReportType== 'Picks') || (strReportType== 'PicksDetail') || (strReportType== 'Logs By Field Depth'))
                {
                    ConvertFeetMetersPicksSearch(idVal);
                }
	            else if(strReportType== 'Logs by Well Wellbore')
	            {		
		            ConvertFeetMetersTabular(idVal);
	            }
                else
                {	
                    if(document.forms[0].elements[GetObjectID('hidDisplayType','input')].value == 'Data Sheet')
                    {	
                        ConvertFeetMetersDatasheet(idVal);
                    }
                    else if(document.forms[0].elements[GetObjectID('hidDisplayType','input')].value == 'Tabular')
                    {				
                        ConvertFeetMetersTabular(idVal);
                    }
                }
            }
        }        
    }
    else
    {
        if (idVal.indexOf(userPreferenceValue)>=0)
        {
            userPreferenceValue = document.forms[0].elements[GetObjectID('hidDepthUnit','input')].value;
        }
        else
        {    
            userPreferenceValue = 'changed';     
	        if(unitValue == '')
	        {
	            if(idVal == "rbMeters")
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
	    var strReportType = document.forms[0].elements[GetObjectID('hidReportType','input')].value ;			
    	
	    if(strReportType.indexOf("SearchContext")>=0)
	    {
	        strReportType = strReportType.replace('SearchContext','');
	    }
    	
	    disableEnableFeetMeter(idVal);
        if((strReportType== 'Picks') || (strReportType== 'Logs By Field Depth'))
        {
            ConvertFeetMetersPicksSearch(idVal);
        }
	    else if(strReportType== 'RecallLog')
	    {		
		    ConvertFeetMetersTabular(idVal);
	    }
        else
        {	
            if(document.forms[0].elements[GetObjectID('hidDisplayType','input')].value == 'Data Sheet')
            {	
                ConvertFeetMetersDatasheet(idVal);
            }
            else if(document.forms[0].elements[GetObjectID('hidDisplayType','input')].value == 'Tabular')
            {				
                ConvertFeetMetersTabular(idVal);
            }
        }
        }
        }
    }
}

var strDepthUnit;
/*********************************************************************
    function to convert feet-meters and vice versa for Detail Report 
**********************************************************************/
function ConvertFeetMetresDetail(idVal, searchType)
{        
    var strDisplayType = "";
    strDisplayType = GetDisplayType(strDisplayType);
    if(ValidateDetailFeetMetres(idVal))
    {
        if(searchType.indexOf("Picks") >= 0 || searchType.indexOf("Logs By Field Depth") >= 0 || searchType.indexOf("TimeDepth") >= 0)
        {
            //ConvertFeetMetersPicksSearch(idVal);
           FeetMetreConversionNew('tblSearchResults');
        }
        else if(strDisplayType == "Data Sheet")
        {
            ConvertFeetMetersDatasheet(idVal);
        }
        else
        {
            //ConvertFeetMetersTabular(idVal);
             FeetMetreConversionNew('tblSearchResults');
        }
    }
}
/**************************************************
    Validating FeetMetres radio button click
***************************************************/
function ValidateDetailFeetMetres(idValue)
{
    if(strDepthUnit != null)
    {
        if(strDepthUnit != idValue)
        {
            strDepthUnit = idValue;
            return true;
        }
        else
        {
            return false;
        }
    }
    else
    {
        strDepthUnit = idValue;
        return true;
    }
}

/************************************************************
        function to convert feet-meters and vice versa on tabular view
************************************************************/
function ConvertFeetMetersTabular(idVal)
{    
    // Local variables
    var rowContent;
    var rowCount;
    var colToConvert;
    var colNamesToConvert;
    var singleColName;	
    //find the column number to convert    
    for (var colHead=0; colHead < titleInnerHTMLArray.length; colHead++)
    {
            if(titleInnerHTMLArray[colHead].indexOf('(m)') >= 0 || titleInnerHTMLArray[colHead].indexOf('(ft)') >= 0)
            {	
                colToConvert = colHead;
                colToConvert = colHead;
                colCaption = table.rows[1].cells[colToConvert].innerHTML
                if(idVal == 'rbFeet')
                {                    
                    colCaption = colCaption.replace('(m)', '(ft)');                                    
                }
                else if(idVal == "rbMeters")
                {
                    colCaption = colCaption.replace('(ft)', '(m)');                    
                }
                table.rows[1].cells[colToConvert].innerHTML = colCaption
                                
                // Re-drawing the table
                rowCount = 0;
                for (var i=0; i<nRow; i++)
                {
                    if (! colSpanArray[i])
                    {
	                    for (var j=0; j<maxNCol; j++)
	                    {
		                    if(j == colToConvert)
		                    {
		                        rowContent = rowArray[rowCount].split(recDelimiter);
								
					var currentValue = columnArray[i][colToConvert].toString();
								if((columnArray[i][colToConvert] != '') && (columnArray[i][colToConvert] != ' ') && (currentValue.indexOf('No data')<0))
								{
									if(idVal == 'rbFeet')
									{
										columnArray[i][colToConvert] = parseFloat(columnArray[i][colToConvert]) * 3.28084
										rowContent[j] = parseFloat(columnArray[i][colToConvert])
									}
									else if(idVal == "rbMeters")
									{
										columnArray[i][colToConvert] = parseFloat(columnArray[i][colToConvert]) / 3.28084
										rowContent[j] = parseFloat(columnArray[i][colToConvert])
									}

	                                //table.rows[i].cells[j].innerHTML = RoundNumber(rowContent[j]);	 
	                                table.rows[i].cells[j].innerHTML = (rowContent[j]).toFixed(2);	                                                               
								}
                            }
	                    }
	                    rowCount++;
                    }
                }
                setCursorDefault();                
            }
        //}
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
	//newNumber = Math.round(valToRound * Math.pow(10, rLength))/Math.pow(10, rLength);
//	if (valToRound > 8191 && valToRound < 10485) 
//	{
//		valToRound = valToRound - 5000;
//		newNumber = Math.round(valToRound * Math.pow(10, rLength))/Math.pow(10, rLength);
//		newNumber = newNumber + 5000;
//	}
//	else
//	{
//		var newNumber = Math.round(valToRound * Math.pow(10, rLength))/Math.pow(10, rLength);
//	}
	return newNumber;
}

function CORE(X, N) 
{    
    // X to String with N decimal places
    var int = Math.floor(X), frc = ((X-int)+1.0) * Math.pow(10, N)
    frc = String(IntFrc(frc)) // Put chosen rounding algorithm here
    return (int + +(frc.charAt(0)=="2")) + '.' + frc.substring(1) 
}
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
    default : return " TypeError" } }

/************************************************************
        function to get table values in a two-dimensional array
************************************************************/ 
function GetTableValues(obj, ignore)
{    
    // Local variables
	var tIndex;
	var countCol;
	var currentCell;
	var nColSpan, nRowSpannedTitleCol, colPos;
	var titleFound = false;
	var skipRow = false;
	var rNRowSpan, rNColSpan, parentNodeName;
	var cCellContent, cCellSetting;
	var sObj;
	var ctable;
	var doneftable = false;
	var cmd;
	var tableLoaded = false;

	// Initializing global table object variable
	if (obj.tagName == "TABLE")
	{
		// Assumes that the obj is THE OBJECT
		table = obj;
	}
	else
	{
		table = document.getElementById(obj);
		sObj = obj.id + "title";
		ttable = document.getElementById(sObj);
	}

	// Check whether it's an object
	if (table == null) return;

	// Check whether it's a table
	if (table.tagName != "TABLE") return;

	if (ttable != null && ttable.tagName == "TABLE")
		separateTitle = true;

	// No need to re-init if it's already done
	if (tableId == table.id && table.tainted == false) return;

	// Setting table id
	tableId = table.id;

	if (separateTitle) ttableId = ttable.id;

	// Initializing the max col number with the size of last data row
	maxNCol = table.rows[table.rows.length-1].cells.length; //table.rows[12].cells.length = 40
	// Initializing arrays
	rowArray = new Array();
	rowIdArray = new Array();
	rowPropArray = new Array();
	colSpanArray = new Array();
	colTitleFilled = new Array();
	titleRowArray = new Array();
	titleInnerHTMLArray = new Array();
	titleSpanCountArray = new Array();
	titleRowCellArray = new Array();
	columnArray = new Array();
	
	for (var i=0; i<maxNCol; i++)//for(i = 0; i< 40;i++)if1==0 or i == 1 colTitleFilled[2---13] = false
	{
		if (i == 0 || i == 1)
		 continue;
		colTitleFilled[i] = false;
	}

	// Setting the number of rows
	nRow = table.rows.length; //13	
	
	// Should have at least 1 row
	if (nRow < 1) return; // greater

	// Initialization of local variables
	actualNRow = 0;			// Number of actual data rows
	rNRowSpan = 0;			// Remaining rows in the row span
	rNColSpan = 0;			// Remaining cols in the col span
	nRowSpannedTitleCol = 0;	// Number of title cols from row span

	// Loop through rows
	for (var i=0; i < nRow; i++)
	{
		if (! separateTitle)
		{
			ctable = table;
		}
		else if (i == 0 && separateTitle && titleFound == false) 
		{
			ctable = ttable;
			nRow = ttable.rows.length + 1;
		}

		if (! doneftable && separateTitle && titleFound)
		{
			ctable = table;
			nRow = table.rows.length;
			i = 0;
			doneftable = true;
		}

		skipRow = false;
		// Skip if it's THEAD, TFOOT
		if (ctable.rows[i].parentNode != null)
		{
			parentNodeName = ctable.rows[i].parentNode.nodeName;
			parentNodeName.toUpperCase();
			if (parentNodeName == 'THEAD' ||
				parentNodeName == 'TFOOT')
			{
				skipRow = true;
			}
		}
		nColSpan = 1, colPos = 0;
					
		// Setting up the title cells
		if (titleFound == false && nColSpan == 1 && rNRowSpan == 0 && rNColSpan == 0)
		{
			colSpanArray[i] = true;
			if (i == 1)
			{
			    titleFound = true;

			    // Using individual cell as an array element
			    countCol = 0;
			    for (var j=0; j<ctable.rows[i].cells.length + nRowSpannedTitleCol; j++)
			    {
				    if (colTitleFilled[j] != true)
				    {
					    titleRowCellArray[j] = ctable.rows[i].cells[countCol];
					    titleRowArray[j] = ctable.rows[i];
					    countCol++;
				    }
				    else
				    {
					    titleRowCellArray[j] = titleSpanCellArray[j];					
				    }
				    titleInnerHTMLArray[j] = String(titleRowCellArray[j].innerHTML);
				    titleSpanCountArray[j] = titleRowCellArray[j].rowSpan;
			    }
			}
		}

		// Setting up the data rows
		else if (titleFound == true && nColSpan == 1 && rNRowSpan == 0 && !skipRow)
		{
		    for (var j=0; j<ctable.rows[i].cells.length; j++)
			{
				// Can't have row span in record rows ...
				if (ctable.rows[i].cells[j].rowSpan > 1) return;
				currentCell = ctable.rows[i].cells[j];                
				cCellContent = String(currentCell.innerHTML);
                cCellContent = cCellContent.replace("&nbsp;", "");
				if (j == 0)
				{	
				    rowArray[actualNRow] = cCellContent;
				}
				else
				{   
					rowArray[actualNRow] += recDelimiter + cCellContent;
				}
				
				if (j == ctable.rows[i].cells.length-1)
					rowArray[actualNRow] += recDelimiter + String(actualNRow);
			}
			actualNRow++;
		}
	}

	if (actualNRow < 1) return;
	
	for (var iRow=1; iRow<nRow; iRow++)
	{
	    columnArray[iRow] = new Array(ctable.rows[iRow].cells.length);
	    for (var iCol=1; iCol<ctable.rows[iRow].cells.length; iCol++)
	    {
	        if (ctable.rows[iRow].cells[iCol].rowSpan > 1) return;
			currentCell = ctable.rows[iRow].cells[iCol];                
			cCellContent = String(currentCell.innerHTML);
			cCellContent = cCellContent.replace("&nbsp;", "");
	        columnArray[iRow][iCol] = cCellContent;
	    }
	}
}

/************************************************************
        function to convert feet-meters and vice versa on picks 
        search results
************************************************************/
function ConvertFeetMetersPicksSearch(idVal)
{    
    // Local variables
    var rowContent;
    var rowCount;
    var colToConvert;
    var colNamesToConvert;
    var singleColName;
    var colCaption;
    //find the column number to convert    
    for (var colHead=0; colHead < titleInnerHTMLArray.length; colHead++)
    {		
		if(titleInnerHTMLArray[colHead].indexOf('(m)') >= 0 || titleInnerHTMLArray[colHead].indexOf('(ft)') >= 0)
		{
			colToConvert = colHead;
			colCaption = table.rows[1].cells[colToConvert].innerHTML				
			if(idVal == 'rbFeet')
			{					
				colCaption = colCaption.replace('(m)', '(ft)');
				colCaption = colCaption.replace('(m)', '(ft)');				
			}
			else if(idVal == "rbMeters")
			{
				colCaption = colCaption.replace('(ft)', '(m)');     					
				colCaption = colCaption.replace('(ft)', '(m)');					
			}
			
			table.rows[1].cells[colToConvert].innerHTML = colCaption
			
			// Re-drawing the table
			rowCount = 0;
			for (var i=0; i<nRow; i++)
			{
				if (! colSpanArray[i])
				{
					for (var j=0; j<maxNCol; j++)
					{
						if(j == colToConvert)
						{
							rowContent = rowArray[rowCount].split(recDelimiter);
							//alert('***'+columnArray[i][colToConvert]+'***');
							
							var currentValue = columnArray[i][colToConvert].toString();
								if((columnArray[i][colToConvert] != '') && (columnArray[i][colToConvert] != ' ') && (currentValue.indexOf('No data')<0))
							{
								if(idVal == 'rbFeet')
								{
									columnArray[i][colToConvert] = parseFloat(columnArray[i][colToConvert]) * 3.28084
									rowContent[j] = parseFloat(columnArray[i][colToConvert])                                    
								}
								else if(idVal == "rbMeters")
								{
									columnArray[i][colToConvert] = parseFloat(columnArray[i][colToConvert]) / 3.28084
									rowContent[j] = parseFloat(columnArray[i][colToConvert])                                    
								}						
								//table.rows[i].cells[j].innerHTML = RoundNumber(rowContent[j]);
								table.rows[i].cells[j].innerHTML = (rowContent[j]).toFixed(2);
							}							
						}
					}
					rowCount++;
				}
			}
			setCursorDefault();                
		}        
    }
}
/////*********************END************************/