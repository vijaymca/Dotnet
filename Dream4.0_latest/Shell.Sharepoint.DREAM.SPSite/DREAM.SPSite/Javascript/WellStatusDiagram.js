
/******************************************************************************
Procedure name    :  fnWriteResMores
Description       :  Write Well to Excel Workbook
Input arguments   :  (strName,strReservoir)
Return arguments  :  (none)
******************************************************************************/
function  fnWritePDF(attachmentid,attachmentname){
   
	var xmlid    = document.all['idXMLdoc'].XMLDocument;
	var rootNode = xmlid.documentElement;  
	
	var filters = "All *.*|*.*"
	//var filters = "Adobe Acrobat (*.pdf)|*.pdf"	

	var docSubmit = new ActiveXObject("Microsoft.XMLDOM");
	var re = new RegExp("xmlns=\"" + xmlid.documentElement.namespaceURI + "\"","g");	
		
	docSubmit.async = false; 
   docSubmit.loadXML(xmlid.xml.replace(re,""));    

	var poster = new ActiveXObject("Microsoft.XMLHTTP");
	poster.open("POST" , "/datatype/wellStatusDiagrams/asp/getAttachment.asp?attachment_id="+ attachmentid+"&attachment_name="+ attachmentname, false);
	poster.send(docSubmit);	
		
	var str = poster.responseStream 				
	
	  var SaveFileName=fnSaveFileNameCommonDialog(filters,attachmentname);
    if (SaveFileName==false){
         return false;
    };    
                  
   var fso = new ActiveXObject("Scripting.FileSystemObject");
   var a = fso.CreateTextFile(SaveFileName, 1,1);  
   
	a.Write(str);
  
   a.Close();
      
	var WshShell = new ActiveXObject("WScript.Shell");
	//WshShell.Run(SaveFileName)
	//WshShell.Run("notepad " + SaveFileName, 1, false);
				
};

/******************************************************************************
Procedure name    :  fnGetSaveFileNameCommonDialog
Description       :  Pops up the file dialog box
Input arguments   :  filters Filters for saveas protocol
Return arguments  :  (filename)
******************************************************************************/  
function fnSaveFileNameCommonDialog(filters,name) {
   var cdlOFNOverwritePrompt=0x2;
   var cdlOFNHideReadOnly=0x4;
   var cdlOFNPathMustExist=0x800;
   var cdlOFNCreatePrompt=0x2000;
   var cdlOFNNoReadOnlyReturn=0x8000;
   var cdlOFNExplorer=0x80000;
   var cdlOFNLongNames=0x200000;
            
   CommonDialog1.CancelError=true;
   CommonDialog1.Filter=filters;
   CommonDialog1.Flags=cdlOFNOverwritePrompt|cdlOFNHideReadOnly|cdlOFNPathMustExist|cdlOFNCreatePrompt|cdlOFNNoReadOnlyReturn|cdlOFNExplorer|cdlOFNLongNames;
	CommonDialog1.FileName=name; 
	
   try{
      CommonDialog1.ShowSave(); 
      return CommonDialog1.FileName;
   }
   catch(e){
      return false;
   };
}; 

/******************************************************************************
Procedure name    :  fnValidateWSDForm
Description       :  This method validates the User Preference values entered for the
					 Well Status Diagram dynamic generation.
					 This form will update the top frame of the window, asking ERO
					 to provide the WSD based on the user settings entered; these
					 settings are then saved to the database
Input arguments   :  frm
Return arguments  :  (none)
******************************************************************************/  
function fnValidateWSDForm(frm) {
	frm = document.frmWSDPreferences;
	
	if (confirm("Are you sure you wish to generate a Well Status Diagram?")) {

		if (frm != null) {
			
			var strUrl = frm.WSDUrl.value;
			
			var formElement = null;
			var formValue = null;
			
			var strNewUrl = strUrl;
			
			//manipulate the elements of the form
			for (var i = 0; i < frm.elements.length; i++) {
			
				formElement = frm.elements[i].name.toLowerCase();
				formValue = frm.elements[i].value;
				
				//find the frm input value in the strUrl
				if (strUrl.indexOf(formElement)!= -1) {
					
					if (formElement != "usecache") {
						
						//get the start position of the url parameter
						var posAtBeginning = strUrl.indexOf(formElement);
						
						//get the length of the parameter string and value
						var lenToNextParam = strUrl.substring(posAtBeginning).indexOf("&");
					
						//use the above info to gather the string to replace
						var strToReplace = strUrl.substr(posAtBeginning,lenToNextParam);
						
						//the new parameter set
						var strReplacement = formElement + "=" + formValue;
					
						//alert("Str To Replace: " + strToReplace);
					
						//alert("Replacement String:" + strReplacement);
					
						strNewUrl = strNewUrl.replace(strToReplace, strReplacement);
					}
				}
			}
			
			var strUseCache = null;
			
			if (frm.usecache.checked) {
				strUseCache = "usecache=True"
			} else {
				strUseCache = "usecache=False";
			}
			
			//replace with the cache param value
			strNewUrl = strNewUrl.replace(strUrl.substr(strUrl.indexOf("usecache")),strUseCache);
			//alert(strNewUrl);
			//alert("New Url To Call: " + strNewUrl);
			
			//first of all show a "waiting" page for the user
			//parent.frames[0].location.href = "http://145.26.249.102:555/datatype/wellStatusDiagrams/asp/wellStatusDiagramsWaitPage.pdf";

			//pause(10000);
			
			//Now submit the form to the save user preferences page.
			frm.submit();
			
			//Now update the top frame with the new URL to call
			//parent.document.frames[0].location.href = strNewUrl;
			parent.frames[0].location.href = strNewUrl;
			
		}
	}
	
	return false;
}


function pause(numberMillis) 
{
	var now = new Date();
	var exitTime = now.getTime() + numberMillis;
	while (true) 
	{
		now = new Date();
		if (now.getTime() > exitTime)
		return;
	}
} 

/******************************************************************************
Procedure name    :  fnValidateWSDForm
Description       :  This method validates the User Preference values entered for the
					 Well Status Diagram dynamic generation.
					 This form will update the top frame of the window, asking ERO
					 to provide the WSD based on the user settings entered; these
					 settings are then saved to the database
Input arguments   :  frm
Return arguments  :  (none)
******************************************************************************/  
function OpenWellStatusDiagram() 
{
	var hidWSDUrl = document.getElementById('hidWSDUrl');
	var ddlDatum = document.getElementById('ddlDatum');
	var ddlTemplate = document.getElementById('ddlTemplate');
	var ddlUnits = document.getElementById('ddlUnits');
	var chbxUsecache = document.getElementById('chbxUsecache');
	
	if (confirm("Are you sure you wish to generate a Well Status Diagram?")) 
	{

		if (true) 
		{
			var strUrl = hidWSDUrl.value;
			var formElement = null;
			var formValue = null;
			var arrSelectElements = document.getElementsByTagName('select');
			var strNewUrl = strUrl;
			//http://sww-discovery.shell.com:500/ERO/WsdDynamic.aspx?DataTypeName=WSD_DYNAMIC&ID=11000080304101&template=EP%20As%20Built%20Info.ppc&datum=NAP&units=EPE&usecache=True
			//manipulate the elements of the form
			for (var i = 0; i < arrSelectElements.length; i++) 
			{
			
				formElement = arrSelectElements[i].name.toLowerCase();
				formValue = arrSelectElements[i].value;
				
				//find the frm input value in the strUrl
				if (strUrl.indexOf(formElement)!= -1) {
					
					if (formElement != "usecache") {
						
						//get the start position of the url parameter
						var posAtBeginning = strUrl.indexOf(formElement);
						
						//get the length of the parameter string and value
						var lenToNextParam = strUrl.substring(posAtBeginning).indexOf("&");
					
						//use the above info to gather the string to replace
						var strToReplace = strUrl.substr(posAtBeginning,lenToNextParam);
						
						//the new parameter set
						var strReplacement = formElement + "=" + formValue;
					
						//alert("Str To Replace: " + strToReplace);
					
						//alert("Replacement String:" + strReplacement);
					
						strNewUrl = strNewUrl.replace(strToReplace, strReplacement);
					}
				}
			}
			
			var strUseCache = null;
			
			if (chbxUsecache.checked) {
				strUseCache = "usecache=True"
			} else {
				strUseCache = "usecache=False";
			}
			//replace with the cache param value
			strNewUrl = strNewUrl.replace(strUrl.substr(strUrl.indexOf("usecache")),strUseCache);
			//Now update the top frame with the new URL to call
			window.open('http://sww-discovery.shell.com:500/ERO/WsdDynamic.aspx?DataTypeName=WSD_DYNAMIC&ID=11000080265101&template=EP As Built Info.ppc&datum=NAP&units=EPE&usecache=True');
		}
	}
	
	return false;
}
/******************************************************************************
Procedure name    :  fnOpenWindow
Description       :  Opens popup window
Input arguments   :  strUrl
Return arguments  :  (none)
******************************************************************************/
function fnOpenWindow(strUrl)
{
    var urlStems = document.URL.split("/"); // split at protocol
    try 
    {    
        // The window is closed
        objWinLL = window.open('','_blank','width=800,height=800,status=yes,resizable=yes,scrollbars=yes');
        objWinLL.document.write("<frameset rows='60%,*'>");
        objWinLL.document.write("<frame name='LivelinkFrame' src='"+  strUrl +"'>");
        objWinLL.document.write("<frame name='Well Status Diagram Preferences' src='http://moss2k7:9999/Pages/WellStatusDiagram.aspx'>");
        objWinLL.document.write("</frameset>");
        objWinLL.focus();
    } 
    catch (exception) 
    {
        result = reportRuntimeError(exception);
    }
}