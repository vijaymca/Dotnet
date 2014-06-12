//Thease below functions are using  AHDTVDConverter.ascx page only.
/***************************************************************
             function is use to validate the HTML table, if user entered value in AH and TV columns
***************************************************************/
function ValidateTable()
{
       var table = document.getElementById(GetObjectID('tblConvertRows','table'));
        var timeformat = /^-?([0-9]{0,18})(\.[0-9]{0,18})?$/;
        var timeformat2 = /^-?([0-9]{0,7})(\.[0-9]{0,18})?$/;
        var timeformat1 = /^-?([0-9]{0,18})$/;
        var val;
        var boolAH=false;
        var boolTV =false;
       var firstvalue = table.rows[1].cells[0].childNodes[0].value;
         var lblProject = document.getElementById(GetObjectID('lblResultProject','span'));
         if( lblProject.innerHTML == "" ||  lblProject.innerHTML=="none")
         {
            alert("Projected Coordinate System is empty, Please select another Wellbore name.");
            return false;
         }
       for(var rowindex=2;rowindex<table.rows.length;rowindex++)
	    {
		    for(var columnindex=0;columnindex<=2;columnindex++)
		    {  
			    if(columnindex==0 ||columnindex==2)
			    {
			      if(table.rows[rowindex].cells[columnindex].childNodes[0].value !=null && table.rows[rowindex].cells[columnindex].childNodes[0].value !="" && table.rows[rowindex].cells[columnindex].childNodes[0].value !=" ")
			      {
			         if(!table.rows[rowindex].cells[columnindex].childNodes[0].value.match(timeformat) || isNaN(table.rows[rowindex].cells[columnindex].childNodes[0].value))
			         {
			         
			           
			               alert('Please enter a valid numerical value.');
                            return false;
			         }
			         
			      }
			    }
		    }
		}		   
         for(var rowindex=2;rowindex<table.rows.length-1;rowindex++)
         {
               if(table.rows[rowindex].cells[4].childNodes[0].value ==null || table.rows[rowindex].cells[4].childNodes[0].value =="" || table.rows[rowindex].cells[5].childNodes[0].value ==null || table.rows[rowindex].cells[5].childNodes[0].value =="")
               {
	              if(table.rows[rowindex].cells[0].childNodes[0].value !=null && table.rows[rowindex].cells[0].childNodes[0].value !="" && table.rows[rowindex].cells[0].childNodes[0].value !=" ")
	              {
	                boolAH =true;			             
	              }
	              if(table.rows[rowindex].cells[2].childNodes[0].value !=null && table.rows[rowindex].cells[2].childNodes[0].value !="" && table.rows[rowindex].cells[2].childNodes[0].value !=" ")
	              {
	                boolTV=true;
	              }
	          }
    	       
            		    
         }
         if(boolAH && boolTV)
         {
            alert("Please enter value on only AH Depth or TV Depth.");
            return false;
         }
         else
         {
            if(boolAH)
            {	                
                document.getElementById(GetObjectID('hidDepthMode', 'input')).value = "MD";
            }
            if(boolTV)
            {
                 document.getElementById(GetObjectID('hidDepthMode', 'input')).value = "TVD";
            }
         }
       
}
/***************************************************************
             function use to validate the numeric values when user change the depthreference dropdown.
***************************************************************/
function ValidateTableforDepthref()
{
        var table =null;
        var tblId =null;
        
        tblId =GetObjectID('tblConvertRows','table');
        if(tblId!=null && tblId!="")
        {
          table  = document.getElementById(tblId);
        }
        
	//var table = document.getElementById(GetObjectID('tblConvertRows','table'));  
	if(table!=null)                      
	{
	var timeformat = /^-?([0-9]{0,18})(\.[0-9]{0,18})?$/;
	var timeformat2 = /^-?([0-9]{0,7})(\.[0-9]{0,18})?$/;
	var timeformat1 = /^-?([0-9]{0,18})$/;
	var val;
	var firstvalue = table.rows[1].cells[0].childNodes[0].value;
	var boolAH=false;
	var boolTV =false;        
    for(var rowindex=2;rowindex<table.rows.length;rowindex++)
	{
		for(var columnindex=0;columnindex<=2;columnindex++)
		{  
			if(columnindex==0 ||columnindex==2)
			{
			  if(table.rows[rowindex].cells[columnindex].childNodes[0].value !=null && table.rows[rowindex].cells[columnindex].childNodes[0].value !="" && table.rows[rowindex].cells[columnindex].childNodes[0].value !=" ")
			  {
				 if(table.rows[rowindex].cells[columnindex].childNodes[0].value.match(timeformat) ||isNaN(table.rows[rowindex].cells[columnindex].childNodes[0].value))
				 {
					alert('Please enter a valid numerical value.');
					
					
					var ddlReport = document.getElementById(GetObjectID("drpDepthReference", "select"));
		            for(rowindex=0;rowindex< ddlReport.options.length;rowindex++)
		            {		 
		               if(ddlReport.options[rowindex].text == document.getElementById(GetObjectID('hidDrpValue', 'input')).value)
		               {
				            ddlReport.options[rowindex].selected=true;
				            break;	              
		               }
		            }     
						return false;
				 }
			  }
			}
		}
	}		
	for(var rowindex=2;rowindex<table.rows.length-1;rowindex++)
	{
		   if(table.rows[rowindex].cells[4].childNodes[0].value ==null || table.rows[rowindex].cells[4].childNodes[0].value =="" || table.rows[rowindex].cells[5].childNodes[0].value ==null || table.rows[rowindex].cells[5].childNodes[0].value =="")
		   {
			  if(table.rows[rowindex].cells[0].childNodes[0].value !=null && table.rows[rowindex].cells[0].childNodes[0].value !="" && table.rows[rowindex].cells[0].childNodes[0].value !=" ")
			  {
				boolAH =true;			             
			  }
			  if(table.rows[rowindex].cells[2].childNodes[0].value !=null && table.rows[rowindex].cells[2].childNodes[0].value !="" && table.rows[rowindex].cells[2].childNodes[0].value !=" ")
			  {
				boolTV=true;
			  }
		  }       
	}
	if(boolAH && boolTV)
	{
		alert("Please enter value on only AH Depth or TV Depth.");
		var ddlReport = document.getElementById(GetObjectID("drpDepthReference", "select"));
		for(rowindex=0;rowindex< ddlReport.options.length;rowindex++)
		{		 
		   if(ddlReport.options[rowindex].text == document.getElementById(GetObjectID('hidDrpValue', 'input')).value)
		   {
				ddlReport.options[rowindex].selected=true;
				break;	              
		   }
		}     
		return false;
	}
	    return true;
 }
 else
 {
    alert("No Depths to Convert.");
    var ddlReport = document.getElementById(GetObjectID("drpDepthReference", "select"));
		for(rowindex=0;rowindex< ddlReport.options.length;rowindex++)
		{		 
		   if(ddlReport.options[rowindex].text == document.getElementById(GetObjectID('hidDrpValue', 'input')).value)
		   {
				ddlReport.options[rowindex].selected=true;
				break;	              
		   }
		}     
    return false;
 }
}

/***************************************************************
          function use to validate the value is numeric or not
***************************************************************/
function isNumericTVDCon(val)
{
	if(val.length != 0)
	{
		var rowindex;			
		var iChars = "0123456789 X";				
		for(rowindex=0;rowindex<val.length;rowindex++)
		{					
			if (iChars.indexOf(val.charAt(rowindex)) == -1) 					
			{					
				return true;
			}
		}	
	}
}
/***************************************************************
          function use to Export the table values to Excel
***************************************************************/
function AHTVCalculatorExport()
{    
        
	try
	{	
	   var table =null;
        var tblId =null;
        
        tblId =GetObjectID('tblConvertRows','table');
        if(tblId!=null && tblId!="")
        {
          table  = document.getElementById(tblId);
        }
        if(table != null)
        {
	        var drpWellbore = document.getElementById(GetObjectID("drpWellbore", "select"));
            var drpWellboreText = drpWellbore.options[drpWellbore.selectedIndex].text;	        
            var lblCountry = document.getElementById(GetObjectID('lblResultCountry','span'));  
            var lblProject = document.getElementById(GetObjectID('lblResultProject','span'));
            var lblResultWellborePath = document.getElementById(GetObjectID('lblResultWellborePath','span'));
            var lblResultPDL = document.getElementById(GetObjectID('lblResultPDL','span'));
            
            //var table = document.getElementById(GetObjectID('tblConvertRows','table'));
            var xls = new ActiveXObject("Excel.Application");
                      
            xls.Workbooks.add();
            xls.Workbooks(1).WorkSheets(1).Name = "AHTVCaculator"; 

            xls.Cells(1,1).FormulaLocal = "Wellbore";
            xls.Cells(1,1).Font.Bold = true;
            xls.Cells(1,2).FormulaLocal = drpWellboreText;


            xls.Cells(2,1).FormulaLocal = "Country";
            xls.Cells(2,1).Font.Bold = true;
            xls.Cells(2,2).FormulaLocal = lblCountry.innerHTML;

            xls.Cells(3,1).FormulaLocal = "Projected Coordinate System";
            xls.Cells(3,1).Font.Bold = true;
            xls.Cells(3,2).FormulaLocal = lblProject.innerHTML;

            xls.Cells(4,1).FormulaLocal = "Wellbore Path";
            xls.Cells(4,1).Font.Bold = true;
            xls.Cells(4,2).FormulaLocal = lblResultWellborePath.innerHTML;
            
             xls.Cells(5,1).FormulaLocal = "PDL";
            xls.Cells(5,1).Font.Bold = true;
            xls.Cells(5,2).FormulaLocal = lblResultPDL.innerHTML;
     
     
           for(columnindex = 0; columnindex <= 7; columnindex++)
           {
              table.rows[0].cells[columnindex].childNodes[0].innerText = table.rows[0].cells[columnindex].childNodes[0].innerText;
              xls.Cells(6,columnindex+1).FormulaLocal = table.rows[0].cells[columnindex].childNodes[0].innerText;
              xls.Cells(6,columnindex+1).Font.Bold = true;
           }      
 
	        for(rowindex = 1; rowindex < table.rows.length; rowindex++)
	        {         
	            m = 6+rowindex;		            
                for(columnindex = 0; columnindex <= 7; columnindex++)
                {	
                			        
	               // xls.Cells(m, columnindex+1).FormulaLocal = "'"+table.rows[rowindex].cells[columnindex].childNodes[0].value;
	                xls.Cells(m, columnindex+1).FormulaLocal = table.rows[rowindex].cells[columnindex].childNodes[0].value;
	                  xls.Cells(m, columnindex+1).NumberFormat = "0.00";
	                xls.Cells(m, columnindex+1).Font.Bold = false;				        
                    //xls.Cells(m, columnindex+1).Interior.ColorIndex = 15;
                    xls.columns.AutoFit();
                }		
		    }		
	        xls.visible = true;	     
	    }
	    else	
        {
         alert("No records to export.");
        }
       }
	catch( E )
    {
          alert('Either excel is not installed or Your browser security setting is not allowing to create excel object.');
    }  
    
  
}
/***************************************************************
          function use to get the Selected depth reference value
***************************************************************/
function getAHTVGValue(depthRef,currentUnit,depthRefDefaultUnit)
{
    var ddlReport = document.getElementById(GetObjectID("drpDepthReference", "select"));
    var strValue = "";
    for(var rowindex=0; rowindex<ddlReport.length;rowindex++)
    {
        if(ddlReport.options[rowindex].text == depthRef)
        {          
           strValue = ddlReport.options[rowindex].value;
           break;
        }        
    }
    if(currentUnit == depthRefDefaultUnit)
    {
        return strValue;
    }
    else if(depthRefDefaultUnit == "metres")
    {
        return AHTVConvertFeetMetre(strValue,"f")
    }
    else if(depthRefDefaultUnit == "feet")
    {
       return AHTVConvertFeetMetre(strValue,"m")
    }
    
}

function AHTVConvertFeetMetre(value,unit)
{
    var convertedValue = "";
    if(!isNaN(parseFloat(value)))
    {
      if(unit =='f' || unit =='F')
      {
       convertedValue = (parseFloat(value)/3.28084).toFixed(2);
      }
      else if(unit =='m' || unit =='M')
      {
      convertedValue = (parseFloat(value)*3.28084).toFixed(2);
      }
    }
  return convertedValue;
  
}
/***************************************************************
          function use to calculate the AH-TVD depth values in 0,1,2 and 3 columns.
***************************************************************/
function AHTVConvertor(t)
{   
    var tblId = GetObjectID('tblConvertRows','table');
    if(tblId!=null && tblId!="")
    {
      table  = document.getElementById(tblId);
    }
    if(table == null)
    {
        alert("No Depths to Convert.");
	    return false;
    }
    //if(ValidateTableforDepthref())
    if(true)
    {
    var cboDepthRef= document.getElementById(GetObjectID("drpDepthReference", "select"));	
	var Text = cboDepthRef.options[cboDepthRef.selectedIndex].text;				
	var Value = cboDepthRef.options[cboDepthRef.selectedIndex].value;
	//*added by dev
	var strCurrentUnit = GetrdoValue();
	var strDepthRefDefaultUnit = document.getElementById(GetObjectID("hidDepthRefDefaultUnit", "input")).value;	
	//*end
	var BF = getAHTVGValue('BF', strCurrentUnit, strDepthRefDefaultUnit);
	var DF = getAHTVGValue('DF', strCurrentUnit, strDepthRefDefaultUnit);
	var GL = getAHTVGValue('GL', strCurrentUnit, strDepthRefDefaultUnit);
	var KB = getAHTVGValue('KB', strCurrentUnit, strDepthRefDefaultUnit);			
	var PDL= getAHTVGValue('PDL', strCurrentUnit, strDepthRefDefaultUnit);
	var RT = getAHTVGValue('RT', strCurrentUnit, strDepthRefDefaultUnit);
	
	
	changeHeaderValue(Text);
	var table = document.getElementById('tblConvertRows'); 
	var table = document.getElementById(GetObjectID('tblConvertRows','table'));
	var m = getAHTVGValue(document.getElementById(GetObjectID('hidDrpValue', 'input')).value, strCurrentUnit, strDepthRefDefaultUnit);
	
	for(var rowindex=1;rowindex<table.rows.length;rowindex++)
	{
		
			//if(table.rows[rowindex].cells[4].childNodes[0].value !=null && table.rows[rowindex].cells[4].childNodes[0].value !="" && table.rows[rowindex].cells[5].childNodes[0].value !=null &&table.rows[rowindex].cells[5].childNodes[0].value !="")
			if(true)
			{
			    if(Text=="GL")
			    {
			        if(table.rows[rowindex].cells[0].childNodes[0].value !=null &&table.rows[rowindex].cells[0].childNodes[0].value !="")
			                   table.rows[rowindex].cells[0].childNodes[0].value = (parseFloat(parseFloat(table.rows[rowindex].cells[0].childNodes[0].value) - parseFloat(m))+parseFloat(GL)).toFixed(2);	
                    if(table.rows[rowindex].cells[2].childNodes[0].value !=null && table.rows[rowindex].cells[2].childNodes[0].value !="")			           
			            table.rows[rowindex].cells[2].childNodes[0].value = (parseFloat(parseFloat(table.rows[rowindex].cells[2].childNodes[0].value) - parseFloat(m))+parseFloat(GL)).toFixed(2);	
			        
			    }
			    if(Text=="DF")
			    {
					if(table.rows[rowindex].cells[0].childNodes[0].value !=null &&table.rows[rowindex].cells[0].childNodes[0].value !="")
			            table.rows[rowindex].cells[0].childNodes[0].value = (parseFloat(parseFloat(table.rows[rowindex].cells[0].childNodes[0].value) - parseFloat(m))+parseFloat(DF)).toFixed(2);	
			        if(table.rows[rowindex].cells[2].childNodes[0].value !=null && table.rows[rowindex].cells[2].childNodes[0].value !="")	
			            table.rows[rowindex].cells[2].childNodes[0].value = (parseFloat(parseFloat(table.rows[rowindex].cells[2].childNodes[0].value) - parseFloat(m))+parseFloat(DF)).toFixed(2);	
			       
			    }
	    	    if(Text=="KB")
                {
                    if(table.rows[rowindex].cells[0].childNodes[0].value !=null &&table.rows[rowindex].cells[0].childNodes[0].value !="")    
                          table.rows[rowindex].cells[0].childNodes[0].value = (parseFloat(parseFloat(table.rows[rowindex].cells[0].childNodes[0].value) - parseFloat(m))+parseFloat(KB)).toFixed(2);	
                    if(table.rows[rowindex].cells[2].childNodes[0].value !=null && table.rows[rowindex].cells[2].childNodes[0].value !="")	
			            table.rows[rowindex].cells[2].childNodes[0].value = (parseFloat(parseFloat(table.rows[rowindex].cells[2].childNodes[0].value) - parseFloat(m))+parseFloat(KB)).toFixed(2);	
			        
			    }
		        if(Text=="PDL")
			    {
			        if(table.rows[rowindex].cells[0].childNodes[0].value !=null &&table.rows[rowindex].cells[0].childNodes[0].value !="")  			           
			           table.rows[rowindex].cells[0].childNodes[0].value = (parseFloat(parseFloat(table.rows[rowindex].cells[0].childNodes[0].value) - parseFloat(m))).toFixed(2);	
			        if(table.rows[rowindex].cells[2].childNodes[0].value !=null &&table.rows[rowindex].cells[2].childNodes[0].value !="") 			            
			            table.rows[rowindex].cells[2].childNodes[0].value = (parseFloat(parseFloat(table.rows[rowindex].cells[2].childNodes[0].value) - parseFloat(m))).toFixed(2);	
			        
			    }
			    if(Text=="BF")
			    {
					if(table.rows[rowindex].cells[0].childNodes[0].value !=null &&table.rows[rowindex].cells[0].childNodes[0].value !="")    
			            table.rows[rowindex].cells[0].childNodes[0].value = (parseFloat(parseFloat(table.rows[rowindex].cells[0].childNodes[0].value) - parseFloat(m))+parseFloat(BF)).toFixed(2);	
					if(table.rows[rowindex].cells[2].childNodes[0].value !=null && table.rows[rowindex].cells[2].childNodes[0].value !="")	
			            table.rows[rowindex].cells[2].childNodes[0].value = (parseFloat(parseFloat(table.rows[rowindex].cells[2].childNodes[0].value) - parseFloat(m))+parseFloat(BF)).toFixed(2);				         
			    }
			   
		}
		else
		{
//		    if(Text=="GL")
//			    {
//					if(table.rows[rowindex].cells[0].childNodes[0].value !=null && table.rows[rowindex].cells[0].childNodes[0].value !="")			        
//			            table.rows[rowindex].cells[1].childNodes[0].value = (parseFloat(parseFloat(table.rows[rowindex].cells[0].childNodes[0].value) - parseFloat(m))+parseFloat(GL)).toFixed(2);	
//					if(table.rows[rowindex].cells[2].childNodes[0].value !=null && table.rows[rowindex].cells[2].childNodes[0].value !="")
//			            table.rows[rowindex].cells[3].childNodes[0].value = (parseFloat(parseFloat(table.rows[rowindex].cells[2].childNodes[0].value) - parseFloat(m))+parseFloat(GL)).toFixed(2);	
//			        
//			    }
//			    if(Text=="DF")
//			    {
//					if(table.rows[rowindex].cells[0].childNodes[0].value !=null && table.rows[rowindex].cells[0].childNodes[0].value !="")			        		      
//			            table.rows[rowindex].cells[1].childNodes[0].value = (parseFloat(parseFloat(table.rows[rowindex].cells[0].childNodes[0].value) - parseFloat(m))+parseFloat(DF)).toFixed(2);	
//					if(table.rows[rowindex].cells[2].childNodes[0].value !=null && table.rows[rowindex].cells[2].childNodes[0].value !="")
//			            table.rows[rowindex].cells[3].childNodes[0].value = (parseFloat(parseFloat(table.rows[rowindex].cells[2].childNodes[0].value) - parseFloat(m))+parseFloat(DF)).toFixed(2);	
//			        
//			    }
//	    	    if(Text=="KB")
//                {
//                    if(table.rows[rowindex].cells[0].childNodes[0].value !=null && table.rows[rowindex].cells[0].childNodes[0].value !="")	
//			            table.rows[rowindex].cells[1].childNodes[0].value = (parseFloat(parseFloat(table.rows[rowindex].cells[0].childNodes[0].value) - parseFloat(m))+parseFloat(KB)).toFixed(2);	
//			        if(table.rows[rowindex].cells[2].childNodes[0].value !=null && table.rows[rowindex].cells[2].childNodes[0].value !="")
//			            table.rows[rowindex].cells[3].childNodes[0].value = (parseFloat(parseFloat(table.rows[rowindex].cells[2].childNodes[0].value) - parseFloat(m))+parseFloat(KB)).toFixed(2);	
//			       
//			    }
//		        if(Text=="PDL")
//			    {
//					if(table.rows[rowindex].cells[0].childNodes[0].value !=null && table.rows[rowindex].cells[0].childNodes[0].value !="")	
//			            table.rows[rowindex].cells[1].childNodes[0].value = table.rows[rowindex].cells[0].childNodes[0].value;
//			        if(table.rows[rowindex].cells[2].childNodes[0].value !=null && table.rows[rowindex].cells[2].childNodes[0].value !="")
//			             table.rows[rowindex].cells[3].childNodes[0].value = table.rows[rowindex].cells[2].childNodes[0].value;
//			    }
//			    if(Text=="BF")
//			    {
//			        if(table.rows[rowindex].cells[0].childNodes[0].value !=null && table.rows[rowindex].cells[0].childNodes[0].value !="")	
//			            table.rows[rowindex].cells[1].childNodes[0].value = (parseFloat(parseFloat(table.rows[rowindex].cells[0].childNodes[0].value) - parseFloat(m))+parseFloat(BF)).toFixed(2);	
//			        if(table.rows[rowindex].cells[2].childNodes[0].value !=null && table.rows[rowindex].cells[2].childNodes[0].value !="")
//			            table.rows[rowindex].cells[3].childNodes[0].value = (parseFloat(parseFloat(table.rows[rowindex].cells[2].childNodes[0].value) - parseFloat(m))+parseFloat(BF)).toFixed(2);				         
//			       
//			    }
		}	        
	}
	AddAHTVLastSelectedValue();
	var hdnFirstAHDepthValue =document.getElementById(GetObjectID('hdnFirstAHDepthValue', 'input'));	
	hdnFirstAHDepthValue.value = table.rows[1].cells[0].childNodes[0].value;
	
	var hdnLastAHDepthValue =document.getElementById(GetObjectID('hdnLastAHDepthValue', 'input'));	
	hdnLastAHDepthValue.value = table.rows[table.rows.length-1].cells[0].childNodes[0].value;
	}    
}

/***************************************************************
          function use to get the previous selected depth refernce dropdown value.
***************************************************************/
function AddAHTVLastSelectedValue()
{
    var ddlDepthRef =document.getElementById(GetObjectID("drpDepthReference", "select"));  
   var hidDrpValue =document.getElementById(GetObjectID('hidDrpValue', 'input'));
	if(ddlDepthRef!=null && hidDrpValue!=null )
    {
        hidDrpValue.value = ddlDepthRef[ddlDepthRef.selectedIndex].text;
    }
}
/***************************************************************
          function use to change the header text.
***************************************************************/
function changeHeaderValue(rdoDepthRef)
{
	var table = document.getElementById(GetObjectID('tblConvertRows','table'));   
	var FeetMeter ="";
	var m =document.getElementById(GetObjectID('hidDrpValue', 'input')).value;	

    for(var columnindex=0;columnindex<=7;columnindex++)
    {   
		if(columnindex!=1 && columnindex!=3)
		{
			table.rows[0].cells[columnindex].innerHTML = table.rows[0].cells[columnindex].innerHTML.replace(m,rdoDepthRef);                    
		}
    }  
}
/***************************************************************
          function use to open AHDepth Popup in new window
***************************************************************/
function openAHDepthPopup()
 {  
    var drpDepthReference = document.getElementById(GetObjectID("drpDepthReference", "select"));         
    var rdoValue = GetrdoValue()    
    Text ="text";
    var rdo;
	if(rdoValue == "metres")
	    rdoValue ="m";
	 else
	    rdoValue = "ft";    	    
    window.open("/Pages/AHDepthPopup.aspx?TopDepthReference="+Text+"&rdoValue="+rdoValue+"","name","width=300, height=200,left="+ screen.width/4 + ",top=" + screen.height/3);   
    return false;
 }
 /***************************************************************
          function use to get selected radio button value.
***************************************************************/
 function GetrdoValue()
 {
	var rdoValue;
    for(var rowindex = 0; rowindex < document.forms[0].elements.length; rowindex++)
    {
        var element = document.forms[0].elements[rowindex];	     
      if(document.forms[0].elements[GetObjectID('rdoDepthUnitsMetres', 'input')].checked==true)
        {    
			rdoValue = "metres";            
        }
         if(document.forms[0].elements[GetObjectID('rdoDepthUnitsFeet', 'input')].checked==true)
         {    
            rdoValue = "feet";            
         }
   }
   return rdoValue    
 }
 /***************************************************************
          function use to open TVDepth Popup in new window
***************************************************************/
 function openTVDepthPopup()
 { 
	var ddlReport = document.getElementById(GetObjectID("drpDepthReference", "select"));
	var Text = ddlReport.options[ddlReport.selectedIndex].text;				
	var Value = ddlReport.options[ddlReport.selectedIndex].value;
	var rdoValue = GetrdoValue()
	var rdo;
	if(rdoValue == "metres")
		rdoValue ="m";
	else
		rdoValue = "ft"
	window.open("/Pages/TVDepthPopup.aspx?TopDepthReference="+Text+"&rdoValue="+rdoValue+"","name","width=300, height=200,left="+ screen.width/4 + ",top=" + screen.height/3);       
	return false;
 }
/************************************************************
        function to get the number of rows need to add the HTML tables.
************************************************************/
function jsInputBox()
{	
	 var tlink=prompt("Please enter the number of rows you want to add.","");	
	if(tlink!="" && tlink!="0" && tlink !=null || isNaN(tlink))
	{   
	    if(isNumericTVDCon(tlink))
        {
            alert ("Please enter a valid numerical value.");	
            return false;
        }
        if(tlink > 20)
        {
            alert("You can add only 20 rows at a time.");
            tlink = 20;
        
        }
		document.getElementById(GetObjectID("hidRows", "input")).value = tlink-1       
	}
	else
	{	 
	  return false;
	}
}

/************************************************************
        function use to clear all the rows and columns values except 1 and 2 row.
************************************************************/
function ClearAllRows()
{

       var table =null;
        var tblId =null;
        
        tblId =GetObjectID('tblConvertRows','table');
        if(tblId!=null && tblId!="")
        {
          table  = document.getElementById(tblId);
        }
         if(table!=null)           
           {      
            for(var rowindex=1;rowindex<=table.rows.length-1;rowindex++)
            {
                for(var columnindex=0;columnindex<=7;columnindex++)
                {
                table.rows[rowindex].cells[columnindex].childNodes[0].value ="";
                }
            }
            return true;
         }
   
     
}
/************************************************************
        function use to clear all the rows and columns values except 1 and 2 row.
************************************************************/
function clearRows()
{

    var table = document.getElementById(GetObjectID('tblConvertRows','table'));
    var m = table.rows.length
    
    for(var rowindex=1;rowindex<table.rows.length;rowindex++)
    {
        for(var columnindex=0;columnindex<=7;columnindex++)
        {
            if(columnindex==0)
            {                
                if(rowindex!=1 && rowindex!=m-1)                
                table.rows[rowindex].cells[columnindex].childNodes[0].value ="";               
            }
            else
            {
			   if(rowindex!=1 && rowindex!=m-1)
				{
				if(columnindex!=1);
					table.rows[rowindex].cells[columnindex].childNodes[0].innerText ="";
				}
				if(rowindex== m-1)
				{
					if(columnindex!=1)
					 table.rows[rowindex].cells[columnindex].childNodes[0].innerText ="";
				}				
				if(rowindex==1 && columnindex!=1)
				{
					table.rows[rowindex].cells[columnindex].childNodes[0].innerText ="";
				}
                
            }            
        }
    }
    return false;
}
/************************************************************
        function use to calculate the Feet / meter in 
************************************************************/
function FeetMeter(idVal)
{

    var table =null;
    var tblId =null;
    
    tblId =GetObjectID('tblConvertRows','table');
    if(tblId!=null && tblId!="")
    {
      table  = document.getElementById(tblId);
    }
        
	//table = document.getElementById(GetObjectID('tblConvertRows','table'));
	  if(table!=null)           
	  {
	    if(table.rows[0].cells[0].innerText.indexOf("(ft)")>=0)
	    {
		    if(idVal == "ft")
		    {
		     return false;
		    }
		    else
		    {
		      callFeetMeter();
		    }
	    }
	    if(table.rows[0].cells[0].innerText.indexOf("(m)")>=0)
	    {
		    if(idVal == "m")
	        {
	         return false;
	        }
	        else
	        {
	          callFeetMeter();
	        }    
	    }
	}
	else
	{
	    alert("No Depths to Convert.");
	    return false;
	}
}
/************************************************************
        function use to change the header text from (ft) to (m) / (m) to (ft)
************************************************************/
function callFeetMeter()
{
	var table = document.getElementById(GetObjectID('tblConvertRows','table'));
	var FeetMeter ="";
    //for(var columnindex=0;columnindex<=7;columnindex++)
    for(var columnindex=0;columnindex<=3;columnindex++)//columnindex changed to three for UAT fixes
    {  
		if(table.rows[0].cells[columnindex].innerText.indexOf("(ft)")>=0)
		 {
			table.rows[0].cells[columnindex].innerHTML = table.rows[0].cells[columnindex].innerHTML.replace("(ft)","(m)");	 			
		 }
         else
         {  
           if(table.rows[0].cells[columnindex].innerText.indexOf("(m)")>=0)
           {
            table.rows[0].cells[columnindex].innerHTML = table.rows[0].cells[columnindex].innerHTML.replace("(m)","(ft)");   	       
           }       
           
         }          
    }     
   calculateFeetMeter();  
}
/************************************************************
        function use to calculate the Feet/Meter
************************************************************/
function calculateFeetMeter()
{
	var table = document.getElementById(GetObjectID('tblConvertRows','table'));
	var FeetMeter= GetrdoValue();
	for(var rowindex=1;rowindex<table.rows.length;rowindex++)
	{
		//for(var columnindex=0;columnindex<=7;columnindex++)
		for(var columnindex=0;columnindex<=3;columnindex++)//columnindex changed to three for UAT fixes
		{
			if(table.rows[rowindex].cells[columnindex].childNodes[0].value !=null ||table.rows[rowindex].cells[columnindex].childNodes[0].value !="")
			{
				if(FeetMeter == 'metres')
				{   
					if(table.rows[rowindex].cells[columnindex].childNodes[0].value !=null && table.rows[rowindex].cells[columnindex].childNodes[0].value !="" &&table.rows[rowindex].cells[columnindex].childNodes[0].value !=" ")
					{
						table.rows[rowindex].cells[columnindex].childNodes[0].value =(parseFloat(table.rows[rowindex].cells[columnindex].childNodes[0].value)/3.28084).toFixed(2);
					}
				}
				else
				{
					if(table.rows[rowindex].cells[columnindex].childNodes[0].value !=null && table.rows[rowindex].cells[columnindex].childNodes[0].value !="" && table.rows[rowindex].cells[columnindex].childNodes[0].value !=" ")
					table.rows[rowindex].cells[columnindex].childNodes[0].value =(parseFloat(table.rows[rowindex].cells[columnindex].childNodes[0].value)*3.28084).toFixed(2);				
				}  
			}    
		}
	}
}

//Thease below functions are using  AHDTVDPopup.ascx page only.
/************************************************************
        Function is use to close the current popup window
************************************************************/
function CloseChildAndRefreshParet(parentURL,alertmsg)
{ 
     window.opener.location=parentURL;
     window.opener = top;
     window.close()
}
/************************************************************
        function use to get the popup query string value.
************************************************************/
function getQueryStringValue(query)
{
	 // get the current URL
	 var url = window.location.toString();
	 //get the parametres
	 url.match(/\?(.+)$/);
	 var params = RegExp.$1; 
	 // split up the query string and store in an
	 // associative array 	 
	 var queryStringList = {};
	 var params = params.split("&");
	 
	 for(var rowindex=0;rowindex<params.length;rowindex++)
	 {
	 	var tmp = params[rowindex].split("=");
	 	queryStringList[tmp[0]] = tmp[1];
	 }
	 return (unescape(queryStringList[query]).replace("#",''));	 
}

/************************************************************
        function is use to validate the AHDepth popup window
************************************************************/
function CallAHParentmethod()
{
    var TVTopDepth = document.getElementById(GetObjectID("txtAHTopDepth", "input"));
    var TVBottomDepth = document.getElementById(GetObjectID("txtAHBottomDepth", "input"));
    var TVDepthInterval = document.getElementById(GetObjectID("txtAHDepthInterval", "input"));
    
    TVTopDepth = TVTopDepth.value;
    TVBottomDepth = TVBottomDepth.value
    TVDepthInterval = TVDepthInterval.value;       
   
    var timeformat = /^-?([0-9]{0,18})(\.[0-9]{0,18})?$/;
    var timeformat2 = /^-?([0-9]{0,7})(\.[0-9]{0,18})?$/;
    var timeformat1 = /^-?([0-9]{0,18})$/;  
 
    if(TVTopDepth.length == 0)
    {
        alert("Please enter Top Depth.");
        return false;
    }
    else
    {
        if(!TVTopDepth.match(timeformat) || isNaN(TVTopDepth))
        {
            alert('Please enter a valid numerical value Top Depth.');
            return false;
         }  
    }
    if(TVBottomDepth.length == 0)
    {
        alert("Please enter Bottom Depth.");
        return false;
    }
    else
    {
       
      if(!TVBottomDepth.match(timeformat) || isNaN(TVBottomDepth))
        {
            alert('Please enter a valid numerical value for Bottom Depth.');
            return false;
         }       
    }
    if(TVDepthInterval.length == 0 || TVDepthInterval == 0)
    {
        alert("Please enter Depth Interval.");
        return false;
    }
     else
    {
        if(isNumericTVDCon(TVDepthInterval))
        {
            alert ("Please enter a valid numerical value for Depth Interval.");	
            return false;
        }
    }
    TVTopDepth = parseFloat(TVTopDepth);
    TVBottomDepth = parseFloat(TVBottomDepth);
    TVDepthInterval = parseFloat(TVDepthInterval);
    if(TVDepthInterval >= TVBottomDepth)
    {
        alert("Depth Interval should less than Bottom Depth.");
        return false;
    }
    if(TVTopDepth >= TVBottomDepth)
    {
         alert("Top depth should less than Bottom Depth.");
        return false;
    }
    
    var m = parseFloat((TVBottomDepth-TVTopDepth)/TVDepthInterval)
    if( m!= null && m !="" && m ==0)
    {
        alert("Please enter correct Depth values.");
        return false;
    }
    else
    {
       // window.opener.TVDepth(TVTopDepth, TVBottomDepth, TVDepthInterval);
    }
   
}


//Thease below functions are using  TVDepthPopup.ascx page only.

/************************************************************
        function is use to validate the TVDepth popup window
************************************************************/
function CallTVParentmethod()
{   
    var TVTopDepth = document.getElementById(GetObjectID("txtTVTopDepth", "input"));
    var TVBottomDepth = document.getElementById(GetObjectID("txtTVBottomDepth", "input"));
    var TVDepthInterval =document.getElementById(GetObjectID("txtTVDepthInterval", "input"));
    TVTopDepth = TVTopDepth.value;
    TVBottomDepth = TVBottomDepth.value
    TVDepthInterval = TVDepthInterval.value;            
   
    var timeformat = /^-?([0-9]{0,18})(\.[0-9]{0,18})?$/;
    var timeformat2 = /^-?([0-9]{0,7})(\.[0-9]{0,18})?$/;
    var timeformat1 = /^-?([0-9]{0,18})$/;
     if(TVTopDepth.length == 0)
    {
        alert("Please enter Top Depth.");
        return false;
    }
    else
    {
       if(!TVTopDepth.match(timeformat) || isNaN(TVTopDepth))
        {
            alert('Please enter a valid numerical value Top Depth.');
            return false;
         }  
    }
    if(TVBottomDepth.length == 0)
    {
        alert("Please enter Bottom Depth.");
        return false;
    }
    else
    {
         if(!TVBottomDepth.match(timeformat) || isNaN(TVBottomDepth))
        {
            alert('Please enter a valid numerical value for Bottom Depth.');
            return false;
         }       
    }
    if(TVDepthInterval.length == 0 || TVDepthInterval ==0)
    {
        alert("Please enter Depth Interval.");
        return false;
    }
     else
    {
        if(isNumericTVDCon(TVDepthInterval))
        {
            alert ("Please enter a valid numerical value for Depth Interval.");	
            return false;
        }
    }
    TVTopDepth = parseFloat(TVTopDepth);
    TVBottomDepth = parseFloat(TVBottomDepth);
    TVDepthInterval = parseFloat(TVDepthInterval);
    if(TVDepthInterval >= TVBottomDepth)
    {
        alert("Depth Interval should less than Bottom Depth.");
        return false;
    }
    if(TVTopDepth >= TVBottomDepth)
    {
         alert("Top depth should less than Bottom Depth.");
        return false;
    }
    
    var m = parseFloat((TVBottomDepth-TVTopDepth)/TVDepthInterval)
    if( m!= null && m !="" && m ==0)
    {
        alert("Please enter correct Depth values.");
        return false;
    }
    else
    {
        //window.opener.TVDepth(TVTopDepth, TVBottomDepth, TVDepthInterval);
    }
}
 

   