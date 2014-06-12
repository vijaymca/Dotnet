/***************************************************************
        function to set Alternate color in the Results Grid 
***************************************************************/
function AlternateColor(objTable)
{	
    var table = document.getElementById(objTable);	
	var cTR = table.getElementsByTagName('TR');  //collection of rows	
	for (i = 1; i < cTR.length; i++)
	{	    		    
	    var cnt = i % 2;	    
	    var tr = cTR.item(i);		    	    

        if( cnt > 0 )
        {            
	        tr.style.background = '#ECECEC';				    
	    }
	    else
	    {	     
	        tr.style.background='#FFFFFF';
	    }		
	}
}

var globalID;

function ShowHistoryRow(id, parentListName, rowID, auditFor)
{
    var table = document.getElementById("tblSearchResults");	
	var cTR = table.getElementsByTagName('TR');
	var name = cTR[rowID].cells[auditFor].innerHTML;
    
     var attributes="toolbar=no,location=no,directories=no,status=yes,resizable=no,menubar=no,";
     attributes+="scrollbars=no,width=550, height=600, left=260, top=130";
     var msgAudit = window.open("/Pages/AuditTrail.aspx?auditID=" + id + "&AuditTrail=" + parentListName + "&auditFor=" + name,"Audit",attributes);
     msgAudit.focus();
}


function openEditPage(id, listReportName)
{  
   if(listReportName == 'MasterPage')
   { 
    window.location.href = "/Pages/EditMasterPage.aspx?mode=edit&idValue="+ id;
   }
   if(listReportName == 'Template') 
   {
    window.location.href = "/Pages/EditTemplate.aspx?mode=edit&idValue="+ id;
    
   }
   if(listReportName == 'TemplatePages') 
   {
    window.location.href = "/Pages/Template.aspx?mode=edit&idValue="+ id;

   }
   if(listReportName == 'TemplateMasterPages')
   {
     window.location.href = "/Pages/EditTemplateMasterPage.aspx?mode=edit&idValue="+ id +"&listType="+listReportName;
     
   }
   if(listReportName == 'WellBook') 
   {
//    window.location.href = "/Pages/WellBook.aspx?mode=edit&idValue="+ id;
  window.location.href = "/Pages/EditBook.aspx?mode=edit&idValue="+ id;

   }
   if(listReportName == 'Chapters') 
   {
//    window.location.href = "/Pages/WellBookChapter.aspx?mode=edit&idValue="+ id;
window.location.href = "/Pages/EditChapter.aspx?mode=edit&idValue="+ id;

   }
     if(listReportName == 'ChapterPages') 
   {
//    window.location.href = "/Pages/MasterPage.aspx?mode=edit&idValue="+ id +"&listType=ChapterPages";
  window.location.href = "/Pages/EditChapterPage.aspx?mode=edit&idValue="+ id +"&listType=ChapterPages";

   }
    if(listReportName == 'BookPages') 
   {
//    window.location.href = "/Pages/MasterPage.aspx?mode=edit&idValue="+ id +"&listType=BookPages";
 window.location.href = "/Pages/EditBookPage.aspx?mode=edit&idValue="+ id +"&listType=BookPages";

   }
    if(listReportName == 'UserRegistration') 
   {
    window.location.href = "/Pages/EditUser.aspx?mode=edit&idValue="+ id +"&listType=UserRegistration";

   }

    if(listReportName == 'TeamRegistration') 
   {
    window.location.href = "/Pages/EditTeam.aspx?mode=edit&idValue="+ id +"&listType=TeamRegistration";

   }
   
   if(listReportName == 'StaffRegistration') 
   {

//     window.open("/Pages/EditStaffPrivilege.aspx?mode=edit&idValue="+ id +"&listType=StaffRegistration");

window.open ("/Pages/EditStaffPrivilege.aspx?mode=edit&idValue="+ id +"&listType=StaffRegistration","mywindow","status=1");
   }   
   //return false;
}


function ClosePopupAndRefreshParent(strTeamId, TEAMSTAFFLIST) 
{
     window.opener.location ="/Pages/StaffList.aspx?idValue="+ strTeamId +"&listType="+TEAMSTAFFLIST; 
     window.opener = top; 
      window.close() ; 
}


function openPages(parentId,listType)
{
    if(listType == 'TemplatePages')
   {
   window.location.href = "/Pages/MaintainTemplatePages.aspx?idValue="+parentId;
   }
    if(listType == 'Template')
   {
   window.location.href = "/Pages/MaintainTemplatePages.aspx?idValue="+parentId;
   }
    if(listType == 'AddRemoveTemplatePages')
   {
    window.location.href = "/Pages/TemplatePages.aspx?idValue="+parentId;
   }
    if(listType == 'AlterTemplatePageSequence')
   {
    window.location.href = "/Pages/AlterTemplatePageSequence.aspx?idValue="+parentId;
   }
    if(listType == 'UserRegistration') 
   {
    window.location.href = "/Pages/UserPrivileges.aspx?idValue="+ parentId +"&listType="+listType;

   }
    if(listType == 'StaffRegistration') 
   {
    window.location.href = "/Pages/UserPrivileges.aspx?idValue="+ parentId +"&listType="+listType;

   }
     if(listType == 'TeamRegistration') 
   {
    window.location.href = "/Pages/StaffList.aspx?idValue="+ parentId +"&listType="+listType;

   }
      if(listType == 'StaffPrivileges') 
   {
//   window.location.href = "/Pages/UserPrivileges.aspx?idValue="+ parentId +"&listType="+listType;
window.location.href = "/Pages/StaffPrivileges.aspx?idValue="+ parentId +"&listType="+listType;


   }
   return false;
}
 function DeselectCheck(checkBoxID)
 {    
    if(document.forms[0].elements[checkBoxID].checked)
        document.forms[0].elements[checkBoxID].checked = false;
    else
    {
        var table = document.getElementById(GetObjectID('tblSearchResults','table'));
        var row = table.rows; 
        var ctrCheckedRows = 0;
        var ctr = 0
        for(i = 1; i < row.length; i++)
	    {
	        if(row[i].cells[0].innerHTML.indexOf("chbSelectID")>=0)
		    {
		        ctrCheckedRows++;
		    }
		    if(row[i].cells[0].innerHTML.indexOf("CHECKED")>=0)
		    {
		        ctr++;
		    }
	    }	    
	    if(ctrCheckedRows == ctr)
	    {
	        document.forms[0].elements[checkBoxID].checked = true;
	    }
	    else
	    {
	        document.forms[0].elements[checkBoxID].checked = false;
	    }
    }
 }
 function selectAll(checkBoxID)
 {
    if(document.forms[0].elements[checkBoxID].checked)
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

function GetSelectedOptions(hdnSelectedOptions)
{
   var hdnSelected =  document.getElementById(hdnSelectedOptions);	

      hdnSelected.value =''; 
       for (i=0; i < document.forms[0].elements.length; i++) 
        {                
            if (document.forms[0].elements[i].type == 'checkbox')
            {
                if (document.forms[0].elements[i].id == 'chbSelectID')
                {
                    if(document.forms[0].elements[i].checked == true)
                    {
                 
                        hdnSelected.value = hdnSelected.value + document.forms[0].elements[i].value + ';';
                   
                    }
                }                    
            }
        }
        
        if(hdnSelected.value =='')
        {
            alert("Please select a Master Page.");
            return false;
        }

   }
   
  function ValidateMasterpage() 
  {
         var txtMasterpage = document.forms[0].elements[GetObjectID("txtPageTitle", "input")];
         var strPageTitle = txtMasterpage.value;
         strPageTitle = LTrim(strPageTitle);
         if(strPageTitle == ''||strPageTitle==""||strPageTitle.length == 0)
         {
            alert('Please enter the master page name.');
            txtMasterpage.focus();
            return false;
         }
         var titleTemplate = document.forms[0].elements[GetObjectID("txtTitleTemplate", "input")];
         var strTitleTemplate = titleTemplate.value;
          strTitleTemplate = LTrim(strTitleTemplate);
         if(strTitleTemplate == ''||strTitleTemplate==""||strTitleTemplate.length==0)
         {
            alert('Please enter the page title.');
            titleTemplate.focus();
            return false;
         }
          var cboDiscipline = document.forms[0].elements[GetObjectID("cboDiscipline","select")];
          if(cboDiscipline.selectedIndex == 0)
           {
            alert('Please select the  Discipline.');
            cboDiscipline.focus();
            return false;
           }
           
         // Default asset type is set to Wellbore and no --Select-- option available
         // The validation removed hence
         /*
          var strPageType = document.forms[0].elements[GetObjectID("cboAssetType", "select")];
          if(strPageType.selectedIndex==0)
           {
            alert('Please select the asset type.');
            strPageType.focus();
            return false;
           }*/
           
           
  }
  
  function ValidateConnectionType()
  {
       var strConnectionType = document.forms[0].elements[GetObjectID("cboConnectionType", "select")];
          var selectedtext = strConnectionType.options[strConnectionType.options.selectedIndex].text;
         if(selectedtext.indexOf('Automated') !=  -1)
         {
           alert('Master pages cannot be created for this type.');
            strConnectionType.selectedIndex = 2;
            return false;
         }
        
  }
  // Validates the Add template form
function ValidateDWBTemplate()
{

        var txtTemplateTitle = document.forms[0].elements[GetObjectID("txtTemplateTitle", "input")];
        var strTemplateTitle = txtTemplateTitle.value;
           strTemplateTitle = LTrim(strTemplateTitle);
         if(strTemplateTitle == ''||strTemplateTitle==""||strTemplateTitle.length==0)
         {
            alert('Please enter the template title.');
            txtTemplateTitle.focus();
            return false;
         }
                 
          var cboAssetType = document.forms[0].elements[GetObjectID("cboAssetType", "select")];
          if(cboAssetType.selectedIndex==0)
           {
            alert('Please select the asset type.');
            cboAssetType.focus();
            return false;
           }
        
           
}
function ValidateDWBChapterPage()
{
    var masterpages = document.forms[0].elements[GetObjectID("cboMasterPages", "select")];
      if(masterpages.selectedIndex == 0)
         {
            alert('Please select Master Page.');
            masterpages.focus();
            return false;
         }
        
}
  // Validates the Add template form
function ValidateDWBWellBook()
{

        var wellBookTitle = document.forms[0].elements[GetObjectID("txtWellBookTitle", "input")];
        var strwellBookTitle = wellBookTitle.value;
          strwellBookTitle = LTrim(strwellBookTitle);
         if(strwellBookTitle == ''||strwellBookTitle== ""||strwellBookTitle.length==0)
         {
            alert('Please enter the Well Book name.');
            wellBookTitle.focus();
            return false;
         }
         
         var team = document.forms[0].elements[GetObjectID("cboTeam", "select")];
        
         if(team.selectedIndex == 0)
         {
            alert('Please select the well book team.');
            team.focus();
            return false;
         }
        
           
}

function ValidateDWBChapter()
{

        var assetType = document.forms[0].elements[GetObjectID("cboAssetType", "select")];
        
         if(assetType.selectedIndex == 0)
         {
            alert('Please select Asset Type.');
            assetType.focus();
            return false;
         }
          //var field = document.forms[0].elements[GetObjectID("cboField", "select")];
         var field = document.forms[0].elements[GetObjectID("lstAssetValues", "select")];
          
         if(field.options.selectedIndex == -1)
         {
            alert('Please select Asset Value.');
            field.focus();
            return false;
         }
          var template = document.forms[0].elements[GetObjectID("cboTemplate", "select")];
          var selectedtext = template.options[template.options.selectedIndex].text;
          
         if(selectedtext.indexOf('Select') !=  -1)
         {
            alert('Please select Template.');
            template.focus();
            return false;
         }
         var chapterTitle = document.forms[0].elements[GetObjectID("txtChapterTitle", "input")];
        var strchapterTitle = chapterTitle.value;
        strchapterTitle = LTrim(strchapterTitle);
         if(strchapterTitle == '' || strchapterTitle == "" || strchapterTitle.length == 0)
         {
            alert('Please enter the Chapter Title.');
            chapterTitle.focus();
            return false;
         } 
         return IsValidText(chapterTitle,50,'Chapter Title');
          var chapterdecription = document.forms[0].elements[GetObjectID("txtDescription","textarea")]
         return IsValidText (chapterdecription,2000,'Chapter Description');
}

/// Function to validate for mandatory fields in Add/Edit User screen
function ValidateDWBUser()
{

//var txtUserName = document.forms[0].elements[GetObjectID("txtUserName", "input")];
//var strUserName = txtUserName.value;
//strUserName = LTrim(strUserName);        
//         if(strUserName == ''|| strUserName == "" || strUserName.length == 0 )
//         {
//            alert('Please enter the Name.');
//            txtUserName.focus();
//            return false;
//         }
//         var txtUserId = document.forms[0].elements[GetObjectID("txtUserId", "input")];
//          var strWindowsUserId = txtUserId.value;
//          strWindowsUserId = LTrim(strWindowsUserId);   
//         if(strWindowsUserId == ''|| strWindowsUserId == "" || strWindowsUserId.length == 0 )
//         {
//            alert('Please enter the Windows User Id.');
//            txtUserId.focus();
//            return false;
//         }

       var cboUserId = document.forms[0].elements[GetObjectID("cboUserId","select")];
          if(cboUserId.selectedIndex==0)
           {
            alert('Please select the  UserId.');
            cboUserId.focus();
            return false;
           }
         var cboDiscipline = document.forms[0].elements[GetObjectID("cboDiscipline","select")];
          if(cboDiscipline.selectedIndex==0)
           {
            alert('Please select the  Discipline.');
            cboDiscipline.focus();
            return false;
           }
           
//           var cboPrivileges = document.forms[0].elements[GetObjectID("cboPrivileges","select")];
//            if(cboPrivileges != null)
//             {
//               if(cboPrivileges.selectedIndex == 0)
//               {
//                alert('Please select a Privilege.');
//                cboPrivileges.focus();
//                return false;
//               }
//            }
        ///Added by Gopinath
        var discipline = cboDiscipline.options[cboDiscipline.selectedIndex].innerHTML;
        var spanMandatory = document.getElementById(GetObjectID("spnTeamMandotory","span"));
        //spnTeamMandotory
        
        if(discipline != 'Administrator')
        {
           var lstTeams = document.forms[0].elements[GetObjectID("lstTeams","select")];
            if(lstTeams != null)
             {
               if(lstTeams.selectedIndex == -1)
               {
                alert('Please select a Team.');
                lstTeams.focus();
                return false;
               }               
            }            
        }
        else
        {
            spanMandatory.innerHTML ="";            
        }
        
        
}

function OnUserDisciplineIsAdmin()
{
    try
    {
        var cboDiscipline = document.forms[0].elements[GetObjectID("cboDiscipline","select")];
        var discipline = cboDiscipline.options[cboDiscipline.selectedIndex].innerHTML;
        var lstTeams = document.forms[0].elements[GetObjectID("lstTeams","select")];
        var spanMandatory = document.getElementById(GetObjectID("spnTeamMandotory","span"));

         if(discipline == 'Administrator')
         {
            spanMandatory.innerHTML ="";
            lstTeams.disabled = true;
         }
         else
         {
            spanMandatory.innerHTML ="*";
            lstTeams.disabled = false;
         }
    }
    catch(Ex)
    {
    }
    return false;
}

/// Function to validate for mandatory fields in Add/Edit Team screen
function ValidateDWBTeam()
{
var txtTeamName = document.forms[0].elements[GetObjectID("txtTeamName", "input")];
var strTeamName = txtTeamName.value;
strTeamName = LTrim(strTeamName);         
         if(strTeamName == ''|| strTeamName == "" || strTeamName.length == 0 )
         {
            alert('Please enter the Team Name.');
            txtTeamName.focus();
            return false;
         }
                           
         var cboAssetOwner = document.forms[0].elements[GetObjectID("cboAssetOwner","select")];
          if(cboAssetOwner.selectedIndex==0)
           {
            alert('Please select an Asset Owner.');
            cboAssetOwner.focus();
            return false;
           }
                     
}



function OpenMasterPageList( templateRowId, operation)
{
 window.location.href = "/Pages/Template.aspx?mode="+ operation+ "&idValue="+ templateRowId;
}


function AddOneMasterPage( )
{
      var selectedMasterPage = document.forms[0].elements[GetObjectID("lbxFromMasterLibrary", "select")];
        var hdnlistboxvalue = document.forms[0].elements[GetObjectID("hdnTemplateListboxValue", "input")];
  
      var targetTemplate = document.forms[0].elements[GetObjectID("lbxTemplateConfiguration", "select")];
        var displayText;
       if(selectedMasterPage.options.selectedIndex == -1)
         {
          alert('Please select the page.');
         return; 
         } 
        var selectedValue = selectedMasterPage.options[selectedMasterPage.options.selectedIndex].value;
        //alert(selectedValue);
            if(hdnlistboxvalue == '')
            {
              hdnlistboxvalue =  selectedValue;
            }
            else
            {
               hdnlistboxvalue = hdnlistboxvalue +";" +  selectedValue;
             } 
        for(var j = 0; j<targetTemplate.options.length;j++)
            {
                if (targetTemplate.options[j].value.toLowerCase().match(selectedValue.toLowerCase()))
                {
                  alert('Master Page Already added.');
                    return false;
                }

            }
   var newOption = new Option(); // Create a new instance of ListItem 
    displayText = selectedMasterPage.options[j].text.split("-"); 
    newOption.text =displayText[1]; 
   newOption.value = selectedMasterPage.options[selectedMasterPage.options.selectedIndex].value; 
   targetTemplate.options[targetTemplate.length] = newOption; //Append the item in Target Listbox
}

function AddAllMasterPage()
{

      var selectedMasterPage = document.forms[0].elements[GetObjectID("lbxFromMasterLibrary", "select")];
      var targetTemplate = document.forms[0].elements[GetObjectID("lbxTemplateConfiguration", "select")];      
      var displayText;
       for(var j = 0; j<selectedMasterPage.options.length;j++)
            {
                    
                      if (!(CheckIfMasterPageIsAdded(selectedMasterPage.options[j].value)))
                        {
                     
                          var newOption = new Option(); // Create a new instance of ListItem 
                          displayText = selectedMasterPage.options[j].text.split("-"); 
                          
                          newOption.text =displayText[1]; 
                         newOption.value = selectedMasterPage.options[j].value; 
                         targetTemplate.options[targetTemplate.length] = new Option();
                         targetTemplate.options[targetTemplate.length - 1].value = newOption.value;
                        targetTemplate.options[targetTemplate.length - 1].text = newOption.text; 
                        } 
                  }
}
function CheckIfMasterPageIsAdded(  valueToCheck)
{
        var targetTemplate1 = document.forms[0].elements[GetObjectID("lbxTemplateConfiguration", "select")];
       for(var i = 0; i<targetTemplate1.options.length;i++)
           {
      
               if ((targetTemplate1.options[i].value.toLowerCase().match(valueToCheck.toLowerCase())))
                    { 
                            return true;
                     }
                  }
                  
                  return false;
}
function RemoveOneMasterPage( )
{
    var targetTemplate = document.forms[0].elements[GetObjectID("lbxTemplateConfiguration", "select")];
     if(targetTemplate.options.selectedIndex == -1)
         {
          alert('Please select the page.');
         return; 
         }  
         
       targetTemplate.remove(targetTemplate.options.selectedIndex);  
         
    
}
function RemoveAllMasterPage( )
{
    var targetTemplate = document.forms[0].elements[GetObjectID("lbxTemplateConfiguration", "select")];
    
    
        while(targetTemplate.length > 0)  {   targetTemplate.remove(0);  }
    
}
  /************************************************************
        function to truncate the junk characters in the ID
************************************************************/
/*function GetObjectID(objectName, TagName)
{
    var objectId = "";
    for(index = 0; index < document.documentElement.getElementsByTagName(TagName).length; index++)
    {
        objectId = document.documentElement.getElementsByTagName(TagName).item(index).id;
        var tempId = objectId.substr(objectId.length - objectName.length);
        if(objectName == tempId )
        {
            break;
        }
    }
    return objectId;
} */
function GetObjectID(objectName, TagName)
{
    var objectId = "";
    var blnIdFound =false;
    for(index = 0; index < document.documentElement.getElementsByTagName(TagName).length; index++)
    {
        objectId = document.documentElement.getElementsByTagName(TagName).item(index).id;
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

function DWBStatusUpdate(rowId, updateMode)
{
  __doPostBack(updateMode,rowId);
}
 //Sets the ColWidth                              
function DWBFixColWidth(tblID, reportType)

{

    var table = document.getElementById(tblID);
   
   var cTR = table.getElementsByTagName('TR');  //collection of rows	
	for (i = 0; i < cTR.length; i++)
	{ 	      
	    var tr = cTR.item(i);	
	   if(reportType == 'Template')
	    {
            tr.cells[0].style.width = "15%";
            tr.cells[0].style.whiteSpace = "nowrap";
            tr.cells[1].style.width = "10%";
            tr.cells[1].style.whiteSpace = "nowrap";
	        tr.cells[2].style.width = "10%";
	        tr.cells[2].style.whiteSpace = "nowrap";
	        tr.cells[3].style.width = "10%";
	        tr.cells[3].style.whiteSpace = "nowrap";
            tr.cells[4].style = "TemplateMasterPageViewCSS";
         }
          if(reportType == 'TemplatePages')
	    {
	         tr.cells[0].style.width = "10%";
	         tr.cells[0].style.whiteSpace = "nowrap";
             tr.cells[1].style.width = "80%";
             tr.cells[1].style.whiteSpace = "nowrap";
             tr.cells[2].style = "TemplateMasterPageViewCSS"; 
         }
           if(reportType == 'MasterPage')
	    {
	        tr.cells[0].style.width = "5%";
	
             tr.cells[1].style.width = "15%";    
              tr.cells[2].style.width = "10%";   
             tr.cells[3].style.width = "15%"; 
             tr.cells[4].style.width = "10%";     
             tr.cells[5].style.width = "10%";     
              tr.cells[6].style.width = "10%";      
	       
         }
           if(reportType == 'WellBook')
	    {
	         tr.cells[0].style.width = "20%";
             tr.cells[1].style.width = "10%";    
	         tr.cells[2].style.width = "8%";
             tr.cells[3].style.width = "8%";
             tr.cells[4].style.width = "8%"; 
               tr.cells[5].style.width = "8%";  
                tr.cells[6].style.width = "8%"; 
                tr.cells[7].style.width = "8%";  
                 tr.cells[8].style.width = "8%";  
                   tr.cells[9].style.width = "8%";   
         }
         if(reportType == 'BookPages')
	    {
	         tr.cells[0].style.width = "20%";
             tr.cells[1].style.width = "25%";    
	         tr.cells[2].style.width = "15%";
             tr.cells[3].style.width = "10%";
             tr.cells[4].style.width = "10%"; 
               tr.cells[5].style.width = "10%";  
           
         }
         
          if(reportType == 'Chapters')
	    {
	         tr.cells[0].style.width = "10%";
             tr.cells[1].style.width = "25%";    
	         tr.cells[2].style.width = "12%";
             tr.cells[3].style.width = "12%";
             tr.cells[4].style.width = "9%"; 
               tr.cells[5].style.width = "9%";  
                  tr.cells[6].style.width = "9%";  
                     tr.cells[7].style.width = "9%";  
           
         }
          if(reportType=='ChapterPages')
         {
          tr.cells[0].style.width = "10%";
             tr.cells[1].style.width = "40%";    
	         tr.cells[2].style.width = "15%";
             tr.cells[3].style.width = "15%";
             tr.cells[4].style.width = "15%"; 

         }
           if(reportType=='DWBHome')
         {
        
             tr.cells[0].style.width = "60%";
             tr.cells[1].style.width = "28%";    
	         tr.cells[2].style.width = "2%";
             tr.cells[3].style.width = "10%";
//              var rdblFavoritePanel = document.getElementById(GetObjectID('rdoFavourites','span'));
//             if(rdblFavoritePanel != null && rdblFavoritePanel.children.length > 0)
//             {
//                for(index = 0; index < rdblFavoritePanel.children.length; index++)
//                { 
//                  if(rdblFavoritePanel.children[index].tagName == 'INPUT' && rdblFavoritePanel.children[index].value == 'Favourites' && rdblFavoritePanel.children[index].checked == true)
//                  {
//                     /// Check the Header check box
//                     // chbHeader
//                     document.getElementById(GetObjectID('chbHeader','input')).checked = true;
//                     break;
//                  }
//                }
//             }
             
//              if(document.getElementById(GetObjectID('hdnShowAllOrNot','input')).value=="true")
//                {
//                document.getElementById(GetObjectID('chbHeader','input')).checked = true;
//                }
//                else
//                {
//                document.getElementById(GetObjectID('chbHeader','input')).checked = false;
//                }
         }
         
         if(reportType == 'UserRegistration')
         {
             tr.cells[0].style.width = "25%";
             tr.cells[1].style.width = "40%";    
	         tr.cells[2].style.width = "15%";
             tr.cells[3].style.width = "15%";
             tr.cells[4].style.width = "15%"; 
            tr.cells[4].style = "TemplateMasterPageViewCSS"; 
         }
         if(reportType == 'TeamRegistration')
         {
             tr.cells[0].style.width = "10%";
             tr.cells[1].style.width = "40%";    
	         tr.cells[2].style.width = "15%";
           tr.cells[4].style = "TemplateMasterPageViewCSS"; 
         }
          if(reportType == 'StaffRegistration')
         {
             tr.cells[0].style.width = "30%";
             tr.cells[1].style.width = "10%";    
	         tr.cells[2].style.width = "25%";
             tr.cells[3].style.width = "25%";
             tr.cells[4].style.width = "10%"; 
            tr.cells[4].style = "TemplateMasterPageViewCSS"; 
         }
         if(reportType == 'WellBookPageView')
         {
//             tr.cells[0].style.width = "20%";
//             tr.cells[1].style.width = "20%";    
//	         tr.cells[2].style.width = "10%";
//             tr.cells[3].style.width = "10%";
//             tr.cells[4].style.width = "5%"; 
//            tr.cells[4].style = "TemplateMasterPageViewCSS"; 
             tr.cells[0].style.width = "3%";
             if(tr.cells.length < 9)
                tr.cells[1].style.width = "15%";
             else
                tr.cells[1].style.width = "10%";
	         tr.cells[2].style.width = "5%";
             tr.cells[3].style.width = "15%";
             tr.cells[4].style.width = "12%"; 
             tr.cells[5].style.width = "10%";
             tr.cells[6].style.width = "5%";
             if(tr.cells.length < 9)
                tr.cells[7].style = "TemplateMasterPageViewCSS";                           
             else
             {
                tr.cells[7].style.width = "5%";
                tr.cells[8].style = "TemplateMasterPageViewCSS";
             }
         }
	} 
         
    
}

function openBookPages(id)

{
     window.location.href = "/Pages/MaintainBookPages.aspx?BookId="+ id;
  }
  
  function openChangepageOwner(id,listype)
  {
     window.location.href = "/Pages/ChangeOwner.aspx?idValue="+ id;
  }
  
  function openChapters(id, listtype)
  {
  window.location.href = "/Pages/MaintainChapters.aspx?BookId="+ id;
  }
  function openChapterPages(id,listtype)
  {
  window.location.href = "/Pages/MaintainChaptersPages.aspx?ChapterID="+ id;
  }
  
  
 //var objEPFilterSceen=null;
 function openEPCatalgueFilter()
  {
// var iWidth = 500;
// var iHeight = 500;

// var ileft = parseInt((screen.availWidth/2) - (iWidth/2));
// var itop = parseInt((screen.availHeight/2) - (iHeight/2));

// var sWindowFeatures = "width=" + iWidth + ",height=" + iHeight + ",status=yes,resizable=yes,left=" + ileft + ",top=" + itop + "screenX=" + ileft + ",screenY=" + itop;
	
 var PageId = document.getElementById(GetObjectID('hidPageId',"input")).value;
 var assetType = document.getElementById(GetObjectID('hidAssetType',"input")).value;
 var colValue = document.getElementById(GetObjectID('hidSelectedRows',"input")).value;
 var colName = document.getElementById(GetObjectID('hidSelectedCriteriaName',"input")).value;
  
// if(objEPFilterSceen != null)
//     {
//       try
//       {
//        objEPFilterSceen.location = "/Pages/EPFilterScreen.aspx?PageId=" + PageId+"&assetType=" + assetType;
//        }
//       catch(ex)
//       {
//        objEPFilterSceen = window.open("/Pages/EPFilterScreen.aspx?PageId=" + PageId+"&assetType=" + assetType,"EPFilterScreen",sWindowFeatures); 
//       }
//     }
//     else
//     {
//     objEPFilterSceen = window.open("/Pages/EPFilterScreen.aspx?PageId=" + PageId+"&assetType=" + assetType,"EPFilterScreen",sWindowFeatures); 
//     }
// 
// objEPFilterSceen.focus();
EPFilterPopup("/Pages/EPFilterScreen.aspx",PageId,assetType,colValue,colName);
 
 } 
 
 /**************************************************************
EP catalog results page opening as popup window
 **************************************************************/
function EPFilterPopup(url,PageID,assetType,selValue,CriteriaName)
{    
    var msgWindow=window.open('',assetType,'width=800,height=600,scrollbars=yes,resizable=yes,status=yes,left=100,top=100');
    document.forms[0].action=url + '?PageId=' + PageID + '&assetType=' + assetType + '&colValue=' + selValue + '&colName=' + CriteriaName;
    document.forms[0].method="post";
    document.forms[0].target=assetType;
    document.forms[0].submit();
    msgWindow.focus();
    document.forms[0].target="_self";
    document.forms[0].action= msgWindow.opener.location.href;
    document.forms[0].method="post"; 
}
        
      
      
function CloseWithoutPrompt()
{
window.opener=top;
window.close();
}


//function ReloadParent(msg)
//{
//if(msg!='')
//{
//alert(msg+'.');
//}
//var loc = window.opener.location;
//if(loc.Indexof("&action=Narrative") > 0)
//{
//    loc = loc.replace("&action=Narrative","");
//}
//else if(loc.Indexof("&action=StoryBoard") > 0)
//{
//    loc = loc.replace("&action=StoryBoard","");
//}
//else if(loc.Indexof("&action=Comments") > 0)
//{
//    loc = loc.replace("&action=Comments","");
//}
//loc = loc + "&action=Comments";
//window.opener.location=loc;

//window.opener = top;
//window.close();
//}

function eWBCheckIfUploadedAndRefreshParent()
{

    var lblErrorMessage = document.getElementById(GetObjectID('lblErrorMessage','span'));

    if(lblErrorMessage != null)
    {
        /// The message comparing should have . at end since it is asseinged as "Document uploaded successfully.".
        /// Removal of . will fail the code to close and refresh the parent window
        if(lblErrorMessage.innerText == 'Document uploaded successfully.')
        {
    //    var loc = window.opener.location;
    //    window.opener.location=loc;
        
           window.opener.document.getElementById(GetOpenerObjectID("btnFirePostBack","input")).onclick="return FirePostBack('true');"
           window.opener.document.getElementById(GetOpenerObjectID("btnFirePostBack","input")).click();
            window.close();    
        }
    }
}


function eWBValidateUpload(fileTypes)
{  
    var fileUp = document.getElementById(GetObjectID('fileUploader',"input"));
    var extension = fileUp.value.substring(fileUp.value.lastIndexOf('.'));
    var fileTypeWithSplitter;
    var blnFileTypeExists = false;
    var fileTypeMessage;
    
    if(extension!='')
        extension = extension.toString().toLowerCase();
     var arFileTypes = new Array();    
     arFileTypes = fileTypes.split("|");  
     
     for(index=0; index<arFileTypes.length; index++)
     {
        fileTypeWithSplitter = arFileTypes[index];
        if(index == 0)
        {
        fileTypeMessage = "*"+fileTypeWithSplitter + ","
        }
        else
        {
        if(fileTypeWithSplitter != '')
        {
           if(index == arFileTypes.length -1)
           {
              fileTypeMessage += "*"+fileTypeWithSplitter ;//+ ",";
           }
           else
           {
                fileTypeMessage += "*"+fileTypeWithSplitter + ",";
           }
        }
        }
        fileTypeWithSplitter = fileTypeWithSplitter + "|";
        if(fileTypeWithSplitter == extension+"|")
        {
            blnFileTypeExists = true;    
        }
     }
     
     if(blnFileTypeExists)
     {
        return true;
     }     
    else
    {
        if(extension=='')
        {
            alert('Please select a document to upload.');
            return false;
        }
        else
        {
            /// Bitmap (*.bmp)
            //- Graphics Interchange Format (*.gif)
            //- JPEG (*.jpg)
            //- Tagged Image File Format (*tif)
            //- Portable Network Graphic (*.ping)
            //- Adobe Portable Document File (*.pdf) 

//            alert('Please select a valid file format(*.bmp, *.gif, *.jpg, *.jpeg, *.jpe, *.tif, *.png, *.pdf).');
 alert('Please select a valid file format('+ fileTypeMessage +').');
            return false;
        }
    }

}

function GetOpenerObjectID(objectName, TagName)
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

function updateUploadFlag(flag,objectname,tagname)
{
window.opener.document.getElementById(GetOpenerObjectID(objectname,tagname)).value = flag;
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
//Does a postback with the ID.
function publishItem(bookid,listtype)
{
///// <TODO>
///// Call the confirmation popup window with Book Summary and Confirm Button
///// </TODO>
 var msgWindow=window.open('/Pages/PublishConfirmation.aspx?BookID='+ bookid  ,'PublishBook','width=1000,height=800,scrollbars=yes,location=no,resizable=yes,status=yes,menubar=no,toolbar=no,left=100,top=100');
//    msgWindow.focus();
// __doPostBack('Publish',bookid);
}

 function OpenBookViewer(bookid)
 {   
   
 var iWidth = 900;
 var iHeight = 800;

 var ileft = 100;
 var itop = 100;

 var sWindowFeatures = "width=" + iWidth + ",height=" + iHeight + ",status=yes,scrollbars=yes,resizable=yes,left=" + ileft + ",top=" + itop + ",screenX=" + ileft + ",screenY=" + itop;   
     var url = '/Pages/WellBookViewer.aspx?BookID='+ bookid + '&instance=new';
   var winBookViewer = window.open( url,'BookViewer',sWindowFeatures); 
   winBookViewer.focus();
   return false;
 }
 
 function ValidateUpdateChapterPageDetail(field)
 {

  if(field=='Narrative')
  {
    var objContent = document.getElementById(GetObjectID('txtNarrative', "textarea"));
    if(objContent.value.length > 0)
    {
    if(!IsValidText(objContent,2000,'Narrative'))
        return false;
    }
    else
    {
       alert('Please enter narrative before saving.');
       return false;
    }
  } 
  
    if(field=='Story Board')
    {
          var objContentSource = document.getElementById(GetObjectID('txtSource', "textarea"));
          if(objContentSource.value.length > 0)
          {
          if(!IsValidText(objContentSource,250,'Source'))
            return false;
          }
          else
          {
            alert('Please enter max of 250 character for Source.');
            return false;
          }
            
          var objContentGenPage = document.getElementById(GetObjectID('txtApplicationGeneratingPage', "textarea"));
           if(objContentGenPage.value.length > 0)
          {
          if(!IsValidText(objContentGenPage,250,'Application Generating Page'))
            return false;
          }
          else
          {
            alert('Please enter max of 250 character for Application Generating Page.');
            return false;
          }
          var objContentAppTemplate = document.getElementById(GetObjectID('txtApplicationTemplate', "textarea"));
         if(objContentAppTemplate.value.length > 0)
          {
          if(!IsValidText(objContentAppTemplate,250,'Application Template'))
            return false;
          }
          else
          {
            alert('Please enter max of 250 character for Application Template.');
            return false;
          }
     }
  return true;
  
//    var userName = document.getElementById(GetObjectID('hidUserName',"input")).value;
//    var pageOwner = document.getElementById(GetObjectID('txtPageOwner',"input")).value;
//    var bookOwner = document.getElementById(GetObjectID('hidBookOwner',"input")).value;    
//    
//    if(userName == pageOwner || userName == bookOwner)
//    {
//    return true;
//    }
//    else
//    {
//    alert('Only a Page Owner or Book Owner is allowed to update ' + field+'.');
//    return false;
//    }
 }
 
 function IsValidText(objContent,len, fieldname)
 {
  if(objContent.value.length>len)
        {
    alert('You can enter only upto ' + len + ' characters in ' + fieldname +'.');
        return false;
        }
        else
            return true;
   
 } 
 
 
 
function SetValueForShowAll()
{

    if(document.getElementById(GetObjectID('chbHeader','input')).checked)
    {
    document.getElementById(GetObjectID('hdnShowAllOrNot','input')).value = "true";
    }
    else
    {
    document.getElementById(GetObjectID('hdnShowAllOrNot','input')).value = "false";
    }
  return false;
}
 
 function SelectUnSelectAll(isHeader)
 {        
//  chbHeader
   if(isHeader == true) /// Check or UnCheck all books
   {   
   var objectId="";
  for(index = 0; index < document.documentElement.getElementsByTagName('input').length; index++)
    {
       objectId = document.documentElement.getElementsByTagName('input').item(index).id;
       if(objectId == 'chbIsFavorite')
       {
        // alert(document.documentElement.getElementsByTagName('input').item(index).disabled);
         if(document.documentElement.getElementsByTagName('input').item(index).disabled != true)
         {
         document.documentElement.getElementsByTagName('input').item(index).checked = document.getElementById('chbHeader').checked;
         }
       }
    }
    }
    else if(isHeader == false)
    {
      /// Unselect Favourite check box in Header Row     
      if(document.getElementById('chbHeader').checked)
      {
      document.getElementById('chbHeader').checked = !document.getElementById('chbHeader').checked;
      }
    }
 }
 
 function AddToFavorites()
 {

   document.getElementById(GetObjectID('hdnSelectedAsFavorite','input')).value = ";";
   for (i=0; i < document.forms[0].elements.length; i++) 
        {            
            if(document.forms[0].elements[i].id !=  'chbHeader')
            { 
                if (document.forms[0].elements[i].type == 'checkbox')
                {
                    if (document.forms[0].elements[i].checked)
                    {
                        document.getElementById(GetObjectID('hdnSelectedAsFavorite','input')).value += document.forms[0].elements[i].value + ";";
                    }   
//                    else
//                    {
//                       document.getElementById(GetObjectID('hdnSelectedAsNotFavorite','input')).value += document.forms[0].elements[i].value + ";";
//                    }       
                }
            }
        }
        if( document.getElementById(GetObjectID('hdnSelectedAsFavorite','input')).value == ";")
        {
           var btnText = document.getElementById(GetObjectID('btnAddToFavorites','input')).value;
           if(btnText == 'Add To Favourites')
           {
            alert('Please select atleast one book before adding to Favourites.');
           }
           else if(btnText == 'Remove From Favourites')
           {
            alert('Please select atleast one book before removing from Favourites.');
           }
          
           return false;
        }
        else
        {
          return true;
        }
        return false;
 }
 
 
 function LoadEPResultsPopup(url,searchType,assetType)
 {
 
  var iWidth = 650;
  var iHeight = 700;
  var ileft = parseInt((screen.availWidth/2) - (iWidth/2));
  var itop = parseInt((screen.availHeight/2) - (iHeight/2));
  var sWindowFeatures = "width=" + iWidth + ",height=" + iHeight + ",scrollbars=no,resizable=no,status=no,left=" + ileft + ",top=" + itop + "screenX=" + ileft + ",screenY=" + itop;
  
  setWindowTitle('Document Search Result');
  window.moveTo(ileft,itop); 
  window.resizeTo(iWidth,iHeight); 
  
  document.forms[0].target="_self";
  document.forms[0].action= url + "?SearchType=" + searchType + "&assetType=" + assetType;
  document.forms[0].method="post"; 
  document.forms[0].submit();
 }
 
 function downLoadType3Report(securityStatus)
 {
 
  if(securityStatus =='Confidential')
  {
     alert('Confidential Document please download different document.');
     //return false;
  }
  else
  {
      __doPostBack('DownLoad','LiveLink');
  }
 
 }
 
 function DisplayMessage(result)
{

    if(result =='True')
    {
        alert('The Document uploaded successfully.');
        window.opener.location.href = window.opener.location.href;

          if (window.opener.progressWindow)
        		
         {
            window.opener.progressWindow.close()
          }
         window.close();
    }
    else
    {
            alert('Error occured while uploading the document.');
    }
}

function LoadSearchSearchResultsPopup(url,searchType,assetType,pageId)
 {

  var iWidth = 650;
  var iHeight = 700;
  var ileft = parseInt((screen.availWidth/2) - (iWidth/2));
  var itop = parseInt((screen.availHeight/2) - (iHeight/2));
  
  setWindowTitle('Document Search Result');
  window.moveTo(ileft,itop); 
  window.resizeTo(iWidth,iHeight); 
  
  document.forms[0].target="_self";
  document.forms[0].action= url + "?SearchType=" + searchType + "&assetType=" + assetType + "&pageId=" + pageId;
  document.forms[0].method="post"; 
  document.forms[0].submit();
 }
 function OpenWellBookSummaryDescription(pageOwner,pageStatus,bookId)
 {

 var hdnpageStatus = document.getElementById(GetObjectID('hdnSelectedStatus',"input"));
    var hdnpageOwner = document.getElementById(GetObjectID('hdnPageOwner',"input"));
    var hdnBookId = document.getElementById(GetObjectID('hdnWellBookId',"input"));    
    hdnpageStatus.value = pageStatus;
    hdnBookId.value = bookId;
    hdnpageOwner.value = pageOwner;
  
    document.forms[0].action="/Pages/BookPageSummary.aspx?BookId=" + bookId + "&pageStatus=" + pageStatus + "&pageOwner=" + pageOwner;
    document.forms[0].method="post";
    document.forms[0].target="_self";
    document.forms[0].submit(); 

 }
 
 function PrintContent()
 { 
     var attributes="toolbar=yes,location=no,directories=yes,resizable,menubar=yes,";
     attributes+="scrollbars=yes,width=800, height=600, left=100, top=25";
     window.open("/_layouts/dream/DWBPrint.htm","Print",attributes);
 } 
 
 
 function DWBExportToExcelPreProd()
{       
    //alert('This option will only export the records displayed on the current page. If you have multiple pages you will have to export each page separately.'); 
	document.body.style.cursor = 'wait';	
    String.prototype.Trim = function () 
    {
        return this.replace(/\|*\s*$/, "");
    }    
	table = document.getElementById('tblSearchResults');
	
	try
    {
        var xls = new ActiveXObject("Excel.Application");
   
        xls.Workbooks.add();
        xls.Workbooks(1).WorkSheets(1).Name = "PreProduction RFT";
		
		var tempCount = 1;
		var rows=table.rows;
		var x=1;
		var charA = 65;       
	    var displayLength = 1;

		for(i = 0; i < rows.length; i++)
		{   
            var column = rows[i].cells;
            var colLength = column.length;
            
            var updateXValue = false;
		    for(j = 1; j <= colLength; j++)
		    {
	            xls.Cells(x, j).FormulaLocal = "'" + column[j-1].innerText.Trim();					
	            xls.Cells(x, j).Borders.Weight = 2;
                xls.columns.AutoFit();
                if(x == 1)
                {
                    xls.Cells(x, j).Font.Bold = true;
                    xls.Cells(x, j).Interior.ColorIndex = 15;
                }
                updateXValue = true;
	            if(j==colLength)
	            {
	                if(updateXValue)
	                    x=x+1;
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

function DWBExportToExcel()
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
//	    xls.Workbooks(1).WorkSheets(1).Name = "Search Result";
        xls.Workbooks(1).WorkSheets(1).Name = "eWB2";//for consistent usage of term eWB2 instead of Digital Well Book, DWB, eWell Book II.
		
		var tempCount = 1;
		var rows=table.rows;
		var x=1;
		var charA = 65;       
	    var displayLength = 1;
//		xls.Range("A1", "D1").HorizontalAlignment = -4108;
//		xls.Range("A1", "D1").MergeCells = true;

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
		            if((headercolumn[k-1].className != "printerFriendly") && (headercolumn[k-1].className != "tblHistoryReport"))  
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
		     
		    if((column[j-1].className != "printerFriendly") && (column[j-1].className != "tblHistoryReport"))  
		    { 
			            xls.Cells(x+1, j).FormulaLocal = column[j-1].innerText.Trim();					
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
function DWBExportToExcelWithCheckBoxes()
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
//	    xls.Workbooks(1).WorkSheets(1).Name = "Search Result";
        xls.Workbooks(1).WorkSheets(1).Name = "eWB2";//for consistent usage of term eWB2 instead of Digital Well Book, DWB, eWell Book II.
		
		var tempCount = 1;
		var rows=table.rows;
		var x=1;
		var charA = 65;       
	    var displayLength = 1;
//		xls.Range("A1", "D1").HorizontalAlignment = -4108;
//		xls.Range("A1", "D1").MergeCells = true;

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
		            if((headercolumn[k-1].className != "printerFriendly") && (headercolumn[k-1].className != "tblHistoryReport"))  
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
		     
		    if((column[j-1].className != "printerFriendly") && (column[j-1].className != "tblHistoryReport"))  
		    { 
			            xls.Cells(x+1, j-1).FormulaLocal = column[j-1].innerText.Trim();					
			            xls.Cells(x+1, j-1).Borders.Weight = 2;
		                xls.columns.AutoFit();				    
		                if(x == 1)
	                    {
	                        xls.Cells(x+1, j-1).Font.Bold = true;				    
		                    xls.Cells(x+1, j-1).Interior.ColorIndex = 15;		                    
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
 
//function QuickSearchPaging(URL,pageNumber,recordcount,sortBy,activeStatus,sortType)
//{
//    window.location.href = URL+"pagenumber="+pageNumber+"&RecordCount="+recordcount+"&sortby="+sortBy+"&sorttype="+sortType+"&status="+activeStatus;
//}

function eWBPagingAndSorting(URL,pageNumber,recordcount,sortBy,activeStatus,sortType)
{
    window.location.href = URL+"pagenumber="+pageNumber+"&RecordCount="+recordcount+"&sortby="+sortBy+"&sorttype="+sortType+"&status="+activeStatus;
}

function AddPageComments(URL,pageID,discipline)
{
 var iWidth = 500;
 var iHeight = 360;

 var ileft = parseInt((screen.availWidth/2) - (iWidth/2));
 var itop = parseInt((screen.availHeight/2) - (iHeight/2));

 var sWindowFeatures = "width=" + iWidth + ",height=" + iHeight + ",status=yes,resizable=yes,left=" + ileft + ",top=" + itop + ",screenX=" + ileft + ",screenY=" + itop;
 
 var objUploadComments = window.open( URL +"?pageID=" + pageID+"&discipline=" + discipline,"UploadComments",sWindowFeatures); 
 
 return false;

}
function SignOffPage(id,listtype)
{
 __doPostBack(listtype,id);
}

function ReloadParent(msg)
{
if(msg!='')
{
alert(msg);
}
var loc = window.opener.location.toString();
//if(loc.indexOf('&action=Narrative') != -1)
//{
//    var loc = loc.replace('&action=Narrative','');
//}
//else if(loc.indexOf('&action=StoryBoard') != -1)
//{
//    var loc = loc.replace('&action=StoryBoard','');
//}
//else if(loc.indexOf('&action=Comments') != -1)
//{
//    var loc = loc.replace('&action=Comments','');
//}
//else
//    var loc = loc;
window.opener.location=loc;
window.opener = top;
window.close();
}

var objUploadDocument = null;
function UpdatePageContents(pageid,pagetype)
{
 
 if(pagetype.indexOf('3')!=-1)
 {
    var type3 = document.getElementById(GetObjectID('hidtype3uploaded',"input"));
    if(type3 != null && type3.value!='true')
    {
    alert('Please do not upload documents that are classified as Confidential or Most Confidential.');
    }
    pagetype='3';
 }
 else
 {
 pagetype='2';
 }

var iWidth = 500;
 var iHeight = 120;

 var ileft = parseInt((screen.availWidth/2) - (iWidth/2));
 var itop = parseInt((screen.availHeight/2) - (iHeight/2));

 var sWindowFeatures = "width=" + iWidth + ",height=" + iHeight + ",status=yes,resizable=yes,left=" + ileft + ",top=" + itop + "screenX=" + ileft + ",screenY=" + itop;
	
 
     
             if(objUploadDocument != null)
             {
               try
               {
                  objUploadDocument.location = "/Pages/UploadDocument.aspx" +"?PageId=" + pageid + "&type=" + pagetype;
                }
               catch(ex)
               {
                 objUploadDocument = window.open( "/Pages/UploadDocument.aspx" +"?PageId=" + pageid +"&type=" + pagetype,"Type2Documents",sWindowFeatures); 
               }
              
             }
             else
             {
               objUploadDocument = window.open( "/Pages/UploadDocument.aspx" +"?PageId=" + pageid + "&type=" + pagetype,"Type2Documents",sWindowFeatures); 
             }
        
        objUploadDocument.focus();
}

function ViewPageContents(id,connectionType,pageurl,chapterId)
{

var iWidth = 800;
 var iHeight = 600;

 var ileft = parseInt((screen.availWidth/2) - (iWidth/2));
 var itop = parseInt((screen.availHeight/2) - (iHeight/2));

 var sWindowFeatures = "width=" + iWidth + ",height=" + iHeight + ",status=yes,scrollbars=yes,resizable=yes,left=" + ileft + ",top=" + itop + ",screenX=" + ileft + ",screenY=" + itop;
        if(connectionType.indexOf('2') !=  -1)
        {
         var objUploadComments = window.open( "/Pages/DWBUserDefinedDocumentViewer.aspx" +"?pageID=" + id+"&mode=view&ChapterID="+chapterId,"Type3Documents",sWindowFeatures); 
        }
         if(connectionType.indexOf('3') !=  -1)
        {
        
          var objUploadComments = window.open( "/Pages/DWBType3DocumentsViewer.aspx" +"?pageID=" + id +"&mode=view&ChapterID="+chapterId,"Type3Documents",sWindowFeatures); 
     
        }
        if(connectionType.indexOf('1') !=  -1)
        {
          if(pageurl =='DWBPreProductionRFT.aspx')
          {
           window.open( "/Pages/DWBPreProductionRFTViewer.aspx" +"?pageID=" + id +"&mode=view&ChapterID="+chapterId,"Type3Documents",sWindowFeatures); 
          }
         if(pageurl =='DWBWellHistoryReport.aspx')
          {
           window.open( "/Pages/DWBWellHistoryReportViewer.aspx" +"?pageID=" + id +"&mode=view&ChapterID="+chapterId,"Type3Documents",sWindowFeatures); 
          }
          if(pageurl =='DWBWellboreHeader.aspx')
          {
           window.open( "/Pages/DWBWellboreHeaderViewer.aspx" +"?pageID=" + id +"&mode=view&ChapterID="+chapterId,"DWBWellboreHeader",sWindowFeatures); 
          }
        if(pageurl =='WellSummary.aspx')
          {
           window.open( "/Pages/WellSummaryViewer.aspx" +"?pageID=" + id +"&mode=view&ChapterID="+chapterId,"DWBWellboreHeader",sWindowFeatures); 
          }
        }
}

function closeAllChildWellReview()
{
    try
    {
    if(objUploadDocument != null)
       {
       objUploadDocument.opener=top;
       objUploadDocument.close();
        }
    }
 catch(ex)
    {
    
    } 
}

function DWBValidatePageComments()
{
  var txtPageComments = document.forms[0].elements[GetObjectID("txtPageComments", "textarea")];
  var strPageComments = txtPageComments.value;
   strPageComments = LTrim(strPageComments);
  if(strPageComments != null && strPageComments.length > 0)
  {
    if(IsValidText (txtPageComments,250,'Comments'))
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
  alert('Please enter the Comments before saving.');
  return false;
  }
  // IsValidText (chapterTitle,2000,'Chapter Title');
}

function today()
{
/// Added brackt around (now.getMonth()+1) otherwise it concats as string instead of adding as number (If 20-Mar-2010 result is 20/21/2010 instead of 20/3/2010)
var now,rv;
now = new Date();
var rv = 
   now.getDate()  + "/" + 
   (now.getMonth()+1) + "/" + 
   now.getYear();
return rv;
}

function DWBFixCheckBoxAlignment(gridID)
{
   var gridView = document.getElementById(gridID);
   // gridView.childNodes[0].children - Rows
   // gridView.childNodes[0].children[0] - Header Row
   var noOfRows = gridView.childNodes[0].children.length;
  // Loop starts from Index = 1 by omitting the header row
   for(i =1; i < noOfRows; i ++)
   {
   gridView.childNodes[0].children[i].children[0].attributes["align"].value = "center";
   }
   return false;
}

/// Function to Trim the left end white spaces
function LTrim(str) 
{ 
 var i;
 var len = str.length; 
  for( i=0; i<len; i++ ) 
   { 
      if( str.charCodeAt(i) == 32 ) 
      { 
         str = str.substring(1, str.length ); 
         i--;    
         len--; 
      } 
      else 
      { 
         break; 
      } 
   } 
   return str; 
 }
 
 /// Function to Trim the right end white spaces
function RTrim(str) 
{ 
 var i;
 var len = str.length; 
  for( i=len; i>len; i-- ) 
   { 
      if( str.charCodeAt(i) == 32 ) 
      { 
         str = str.substring(1, str.length ); 
         i--;    
         len--; 
      } 
      else 
      { 
         break; 
      } 
   } 
   return str; 
 }
 function ValidateDWBChapterCriteria()
 {    
        var assetType = document.forms[0].elements[GetObjectID("cboAssetType", "select")];
        
         if(assetType.selectedIndex == 0)
         {
            alert('Please select Asset Type.');
            assetType.focus();
            return false;
         } 
         var txtCriteria = document.forms[0].elements[GetObjectID("txtCriteria", "input")];
         var strCriteria = txtCriteria.value;
         strCriteria = LTrim(strCriteria);
         if(strCriteria == ''||strCriteria=="" || strCriteria.length ==0 || strCriteria == "Wildcard = % or *")
         {
            alert('Please enter the Search Criteria.');
            txtCriteria.focus();
            return false;
         }
}

/// Function to check the Max length of the text boxes and alert user if goes beyond the allowed limit
function ValidateTextLength(obj,textlength,field)
{
  
if (obj.value.length<textlength) 
  return true;
else 
  { 
    if ((event.keyCode>=37 && event.keyCode<=40) || (event.keyCode==8) || (event.keyCode==46)) 
        event.returnValue = true;
    else
    {
        alert('Only ' + textlength + ' characters are allowed in ' + field +'.');
        event.returnValue = false; 
    }
  }

}

function validate(frm){
    var txtCriteria = document.forms[0].elements[GetObjectID(frm, "input")];
    digitCheck = /^\d+$/; 
    if(txtCriteria.value.length < 5)
    {
        if(digitCheck.test(txtCriteria.value))
        {
          return true;
        } 
        else 
        {
        if(txtCriteria.value.length == 0)
        {
        }
         else
         {
          alert("Please enter only number value.");
          txtCriteria.value = txtCriteria.value.substring(0, txtCriteria.value.length-1);
          return false;
          }
        }
    }    
}

/************************************************************************
   To Check Whether one or all Pages are selected to Changer Page Owner
**************************************************************************/
 function IsPageSelected(tableName)
 {
 
    try
    {           
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
		    return true;
        }
        else
        {
            for(j = 2; j < row.length; j++)
            {                
                if(row[j].cells[0].innerHTML.indexOf("CHECKED")>=0)
                {	                
	               return true;          
                }               
            }
	    }
	     alert('Please select atleast one page to change the page Owner.');
	    return false;
    }
    catch(Ex){}  
}
function ClearDefaultText(obj,defaultText)
    {
   
       if(obj != null)
       {
           if(obj.value== defaultText)
           {
             obj.value='';
             obj.style.color = 'Black';             
           }
       }
       return false;
    }
    
    function ResetToDefaultText(obj,defaultText)
    {
   
       if(obj != null)
       {
           if(obj.value =='')
           {             
             obj.value= defaultText;
             obj.style.color = 'silver';
           }
       }
       return false;
    }
    
function GetSelectedTreeNodes()
{
    var treeview = document.getElementById(GetObjectID("WellBookTree", "div"));
    var x = treeview.childNodes;
    for(var i = 0; i < x.length;i++)
    {
        var tab = x[i];
        var ang = tab.childNodes;
        for(var d = 0;d<ang.length;d++)
        {
            var chi = ang[d];
            alert('chi Value is : ' + chi.innerText);
        }
    }
}


/// Functions for Tree View Node Selection

  function OnCheckBoxCheckChanged(evt) 
    { 
        var src = window.event != window.undefined ? window.event.srcElement : evt.target; 
        var isChkBoxClick = false;
        if(src != null)
        {
         isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox"); 
        }      
        if (isChkBoxClick) { 
            var parentTable = GetParentByTagName("table", src); 
            var nxtSibling = parentTable.nextSibling; 
            if (nxtSibling && nxtSibling.nodeType == 1)//check if nxt sibling is not null & is an element node 
            { 
                if (nxtSibling.tagName.toLowerCase() == "div") //if node has children 
                { 
                    //check or uncheck children at all levels 
                    CheckUncheckChildren(parentTable.nextSibling, src.checked); 
                } 
            } 
            //check or uncheck parents at all levels 
            CheckUncheckParents(src, src.checked); 
        } 
    } 
    function CheckUncheckChildren(childContainer, check) 
    { 
        var childChkBoxes = childContainer.getElementsByTagName("input"); 
        var childChkBoxCount = childChkBoxes.length; 
        for (var i = 0; i < childChkBoxCount; i++) { 
            childChkBoxes[i].checked = check; 
        } 
    } 
    function CheckUncheckParents(srcChild, check)
     { 
        var parentDiv = GetParentByTagName("div", srcChild); 
        var parentNodeTable = parentDiv.previousSibling; 

        if (parentNodeTable) { 
            var checkUncheckSwitch; 

            if (check) //checkbox checked 
            { 
                var isAllSiblingsChecked = AreAllSiblingsChecked(srcChild); 
                if (isAllSiblingsChecked) 
                    checkUncheckSwitch = true; 
                else 
                    return; //do not need to check parent if any(one or more) child not checked 
            } 
            else //checkbox unchecked 
            { 
                checkUncheckSwitch = false; 
            } 

            var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input"); 
            if (inpElemsInParentTable.length > 0) { 
                var parentNodeChkBox = inpElemsInParentTable[0]; 
                parentNodeChkBox.checked = checkUncheckSwitch; 
                //do the same recursively 
                CheckUncheckParents(parentNodeChkBox, checkUncheckSwitch); 
            } 
        } 
    } 
    function AreAllSiblingsChecked(chkBox) 
    { 
        var parentDiv = GetParentByTagName("div", chkBox); 
        var childCount = parentDiv.childNodes.length; 
        for (var i = 0; i < childCount; i++) { 
            if (parentDiv.childNodes[i].nodeType == 1) //check if the child node is an element node 
            { 
                if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") { 
                    var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0]; 
                    //if any of sibling nodes are not checked, return false 
                    if (!prevChkBox.checked) { 
                        return false; 
                    } 
                } 
            } 
        } 
        return true; 
    } 
    //utility function to get the container of an element by tagname 
    function GetParentByTagName(parentTagName, childElementObj)
     { 
        var parent = childElementObj.parentNode; 
        while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) { 
            parent = parent.parentNode; 
        } 
        return parent; 
    } 

function DWBValidateNewBookTitle()
{
    var wellBookTitlePrevious = document.getElementById([GetObjectID("lblBookName","span")]);       
    var strwellBookTitlePrevious = "";
    if(wellBookTitlePrevious != null)
    {
    strwellBookTitlePrevious = wellBookTitlePrevious.innerText;
    }
    var wellBookTitle = document.forms[0].elements[GetObjectID("txtNewBookName", "input")];
    var strwellBookTitle = wellBookTitle.value;
    strwellBookTitle = LTrim(strwellBookTitle);
    if(strwellBookTitle == ''||strwellBookTitle== ""||strwellBookTitle.length==0)
     {
        alert('Please enter the Well Book name.');
        wellBookTitle.focus();
        return false;
     }
    else if((strwellBookTitlePrevious != null || strwellBookTitlePrevious != "" || strwellBookTitlePrevious != '' ) && strwellBookTitlePrevious == strwellBookTitle)
    {
       alert('Please enter the new name for Well Book.');
        wellBookTitle.focus();
        return false;
    }
     return true;
}    

/*******treeviewID - TreeView ClientID+ _Data (global variable stores treeview properties) ******/
function CheckTreeViewCheckBoxes(treeviewID)
{
   
   /// Gets the ID of the hidden variable stores the selected node id
   var treeViewDataSelectedNodeID = treeviewID.selectedNodeID.id;
   /// Gets the hidden variable stores the selected node id
   var hdnSelectedNode = document.getElementById(treeViewDataSelectedNodeID); 
          
      var checkbox1;
      /// document.getElementById(hdnSelectedNode.value) gets the <a> tag for the selected node
      /// document.getElementById(hdnSelectedNode.value).parentElement.firstChild gets the checkbox for the selected node
      checkbox1 = document.getElementById(hdnSelectedNode.value).parentElement.firstChild;      
      if(checkbox1 != null && checkbox1.tagName.toLowerCase() == "input" && checkbox1.type.toLowerCase() == "checkbox")
      {
         checkbox1.click()
      }
   
}

/****** Changed to click from selected node property . This code is to be REMOVED
function CheckTreeViewCheckBoxes(treeviewID)
{
 
   var treeview = document.getElementById(treeviewID);   
   if(treeview != null)
   {
   /// Getting root node check box
                      // treeview div      table        tr         td          checkbox
      var checkbox1 = treeview.children[0].children[0].children[0].children[2].children[0];
     /// Get the treeview and call click event.
     if(checkbox1 != null)
     {
     checkbox1.click();
     }
     /// Get the first node checkbox and call click event on that.
     
   }
} ********/

function CloseChildAndRefreshParet(parentURL,alertmsg)
{
 
  if(alertmsg != null && alertmsg != '' && alertmsg != "")
  {
     alert(alertmsg);
     return false;
  }
  else
  {
     window.opener.location=parentURL;
     window.opener = top;
     window.close()
  }
}


/*
function CheckFavoriteHeader()
{
   var rdblFavoritePanel = document.getElementById(GetObjectID('rdoFavourites','span'));
     if(rdblFavoritePanel != null && rdblFavoritePanel.children.length > 0)
     {
        for(index = 0; index < rdblFavoritePanel.children.length; index++)
        { 
          if(rdblFavoritePanel.children[index].tagName == 'INPUT' && rdblFavoritePanel.children[index].value == 'Favourites' && rdblFavoritePanel.children[index].checked == true)
          {
             /// Check the Header check box
             // chbHeader
             document.getElementById(GetObjectID('chbHeader','input')).checked = true;
             break;
          }
        }
     }
}*/

function openStoryBoardConfirmation(ctrlTreeViewID)
{ 
var isPage = false;
var ctrlTreeView = null;
if(ctrlTreeViewID != null)
{
 ctrlTreeView = $find (ctrlTreeViewID);
}
var continuePrint = false;
if(ctrlTreeView != null)
{
    var selectedNode =ctrlTreeView.get_selectedNode();
    if(selectedNode != null)
    {
        if(selectedNode.get_level() == 0)
        {
            var checkedNodes = ctrlTreeView.get_checkedNodes();    
            if(checkedNodes.length > 0)
            {
              continuePrint = true;
            }
            else
            {
              alert('Please select atleast one Chapter/Page to print.');
              return false;
            }
        }
        else if(selectedNode.get_level() == 1)
        {
             var noOfChildNodes = selectedNode.get_nodes().get_count();
             var checkedNodes = 0;
             childNodes = selectedNode._getAllItems();
             if(noOfChildNodes > 0)
             {
             for(i = 0; i < noOfChildNodes; i++)
             {
               if(childNodes[i].get_checked())
               {
                  continuePrint = true;
                  checkedNodes++;
                  break;
               }
             }
             }
             else if (noOfChildNodes == 0)
             {
                  continuePrint = true;
                  checkedNodes++;
             }
             if(checkedNodes == 0)
             {
             alert('Please select atleast one Page to print.');
             return false;
             }
        }
        else if(selectedNode.get_level() == 2)
        {
        isPage = true;
         continuePrint = true;
        }
    }    
    else
    {
      return false;
    }
}
if(continuePrint)
{    
    var attributes="toolbar=no,location=no,directories=no,resizable=no,menubar=no,";
    if(isPage)
    {
     attributes+="scrollbars=no,width=390, height=210, left=260, top=130";
    }
    else
    {
     attributes+="scrollbars=no,width=630, height=400, left=260, top=130";    
    }    
    var msgAudit = window.open("/Pages/StoryBoardConfirmation.aspx","StoryBoard",attributes);
    msgAudit.focus();
    return false;    
}
}

function continuePrint(continuePrint)
{
if(continuePrint)
{
return true;
}
else 
{
return false;
}
}

function assignStoryBoardResult(rblStoryBoardConfirmList,rblPageTitleConfirmList)
{
 /// Assign the selected value to hidden variable in parent window
 var rblStoryBoardConfirmOptionList2 = document.getElementById(rblStoryBoardConfirmList);
 var rblStoryBoardConfirmOptionList2 = rblStoryBoardConfirmOptionList2.getElementsByTagName("input"); 
 var blnStoryBoardOptionSelected = false;
 var blnPageTitleOptionSelected = false;
 for(index = 0; index < rblStoryBoardConfirmOptionList2.length; index++)
 {
   if(rblStoryBoardConfirmOptionList2[index].checked)
   {
   blnStoryBoardOptionSelected = true;
    if(rblStoryBoardConfirmOptionList2[index].value == 0)
     {
        // Not include story board
       window.opener.document.getElementById(GetOpenerObjectID("hdnIncludeStoryBoard","input")).value = false;
     }
     else if (rblStoryBoardConfirmOptionList2[index].value == 1)
     {
       // Include story board
       window.opener.document.getElementById(GetOpenerObjectID("hdnIncludeStoryBoard","input")).value = true;
     }
   }
 }
 /// Assign the selected value to hidden variable in parent window 
 var rblPageTitleConfirmOptionList2 = document.getElementById(rblPageTitleConfirmList);
 var rblPageTitleConfirmOptionList3 = rblPageTitleConfirmOptionList2.getElementsByTagName("input"); 

  for(index = 0; index < rblPageTitleConfirmOptionList3.length; index++)
 {
   if(rblPageTitleConfirmOptionList3[index].checked)
   {
   blnPageTitleOptionSelected = true;
    if(rblPageTitleConfirmOptionList3[index].value == 0)
     {
        // Not include story board
       window.opener.document.getElementById(GetOpenerObjectID("hdnIncludePageTitle","input")).value = false;
     }
     else if (rblPageTitleConfirmOptionList3[index].value == 1)
     {
       // Include story board
       window.opener.document.getElementById(GetOpenerObjectID("hdnIncludePageTitle","input")).value = true;
     }
   }
 }
 
 if(blnStoryBoardOptionSelected && blnPageTitleOptionSelected)
 {
 window.opener.document.getElementById(GetOpenerObjectID("btnPrint","input")).onclick="return continuePrint(true);"
 window.opener.document.getElementById(GetOpenerObjectID("btnPrint","input")).click();
 window.close();
 }
 return false;
}

function openDWBPrintDoc (docurl)
{
var dwbPringDocWindow = window.open(docurl,"eWB","","");
dwbPringDocWindow.focus();
}

function HideTabs(div , displayType)
{    
    var tabDiv = document.getElementById(GetObjectID(div, "div"));
    tabDiv.style.display = displayType;
}

function RedirectTo(URL)
{
   window.location.href = URL;
   return false;
}

function openPublishedBookList(id,bookName)
{
var iWidth = 800;
 var iHeight = 600;

 var ileft = parseInt((screen.availWidth/2) - (iWidth/2));
 var itop = parseInt((screen.availHeight/2) - (iHeight/2));

 var sWindowFeatures = "width=" + iWidth + ",height=" + iHeight + ",toolbar=no,location=no,directories=no,resizable=no,menubar=no,scrollbars=no,left=" + ileft + ",top=" + itop + ",screenX=" + ileft + ",screenY=" + itop;   
// var attributes="toolbar=no,location=no,directories=no,resizable=no,menubar=no,";
// attributes+="scrollbars=no,width=350, height=200, left=260, top=130";
     var url = "/Pages/DWBPublishedBookList.aspx?BookID="+id +"&BookName="+ bookName;
   var msgPublished = window.open(url,'BookList',sWindowFeatures);
   msgPublished.focus();
   return false;
}

function FirePostBack(status)
{
    if(status == 'true')
    {
        return true;
    }
}

/***************************************************************
       function used for opening a page in a pop up window
***************************************************************/
function openPopup(url)//,name,attributes)
{    
  var msgWin = window.open(url,"eWB","");
  msgWin.focus();
  setWindowTitle("eWB");  
}
/**************************************
    Function to set the window title.
***************************************/
function setWindowTitle(windowName)
{        
    window.document.title = windowName;
}
/**************************************
    Function for PrintByPagetype
    Added by Gopinath
    Date: 11/11/2010
***************************************/

//blnType checks Is it Book, Chapter or Page. If Page only only we need to take care Title and storeboardinfo.
function HiddenPrintByPageValues(blnType)
{

//TitleandStoryBoard
var blnStoryBoardOptionSelected = false;
var blnPageTitleOptionSelected = false;

/// TITLE Assign the selected value to hidden variable in parent window 
 var rblPageTitleConfirmOptionList2 = document.getElementById(GetObjectID("rblPageTitleConfirm","table"));
 
 var rblPageTitleConfirmOptionList3 = rblPageTitleConfirmOptionList2.getElementsByTagName("input"); 

  for(index = 0; index < rblPageTitleConfirmOptionList3.length; index++)
 {
   if(rblPageTitleConfirmOptionList3[index].checked)
   {
     if (rblPageTitleConfirmOptionList3[index].value == 1)
     {
        blnPageTitleOptionSelected = true;
     }
   }
 }
//Include page name
window.opener.document.getElementById(GetOpenerObjectID("hdnIncludePageTitle","input")).value = blnPageTitleOptionSelected;


///STORYBOARD Assign the selected value to hidden variable in parent window
 var rblStoryBoardConfirmOptionList2 = document.getElementById(GetObjectID("rblStoryBoardConfirm","table"));
 var rblStoryBoardConfirmOptionList2 = rblStoryBoardConfirmOptionList2.getElementsByTagName("input"); 
 for(index = 0; index < rblStoryBoardConfirmOptionList2.length; index++)
 {
   if(rblStoryBoardConfirmOptionList2[index].checked)
   {
     if (rblStoryBoardConfirmOptionList2[index].value == 1)
     {
       // Include story board
       blnStoryBoardOptionSelected = true;
     }
   }
 }
window.opener.document.getElementById(GetOpenerObjectID("hdnIncludeStoryBoard","input")).value = blnStoryBoardOptionSelected;

if(blnType == "True")//If it true then its user selected as "Book or Chapter".
{

    //PrintMyPages
    var chkPrintMyPages = document.getElementById(GetObjectID("chkPrintMyPagesOnly","input"));

    if(chkPrintMyPages != null && chkPrintMyPages.checked)
    {
        window.opener.document.getElementById(GetOpenerObjectID("hdnPrintMyPages","input")).value = true; 
    }
    else
    {
        window.opener.document.getElementById(GetOpenerObjectID("hdnPrintMyPages","input")).value = false;
    }

    //IncludeFilter
    var blnFilterSelect = false;
    var rblFilter = document.getElementById(GetObjectID("rblIncludeFilter","table"));
    if(rblFilter != null)
    {
        var rblFilter = rblFilter.getElementsByTagName("input");     

        for(index = 0; index < rblFilter.length; index++)
        {
            if(rblFilter[index].checked)
            {
                if(rblFilter[index].value == 1)
                    blnFilterSelect = true;
            }
         }
     }
     else
     {
       blnFilterSelect = true;
     }
     if(blnFilterSelect)
     {
        window.opener.document.getElementById(GetOpenerObjectID("hdnIncludeFilter","input")).value = true;
    }
    else
    {
        window.opener.document.getElementById(GetOpenerObjectID("hdnIncludeFilter","input")).value = false;
    }

    //Signed off
    var blnSignedOffYes = false;
    var blnSignedOffNo = false;
    var signedOffInfo = "";
    var chkSignedOff = document.getElementById(GetObjectID("chklPageSignedOff","table"));
     var chkSignedOff = chkSignedOff.getElementsByTagName("input"); 
     
     for(index = 0; index < chkSignedOff.length; index++)
     {
        if(index == 0)
        {
            if(chkSignedOff[index].checked)
            {
                blnSignedOffYes = true;
                signedOffInfo = "Yes";            
            }
        }
        else if(index == 1)
        {
            if(chkSignedOff[index].checked)
            {
                blnSignedOffNo = true;
                signedOffInfo = "No";
            }
        }          
     }
     
     if(blnSignedOffYes && blnSignedOffNo)
     {
        signedOffInfo = "both";
     }
     
      window.opener.document.getElementById(GetOpenerObjectID("hdnSignedOffPages","input")).value = signedOffInfo;
     
     //Empty Pages
     var blnEmptyPageYes = false;
     var blnEmptyPageNo = false;
     var emptyPageInfo = "";

     var chkEmptyPage = document.getElementById(GetObjectID("chklPageEmpty","table"));
     var chkEmptyPage = chkEmptyPage.getElementsByTagName("input"); 
     
     for(index = 0; index < chkEmptyPage.length; index++)
     {
        if(index == 0)
        {
            if(chkEmptyPage[index].checked)
            {
                blnEmptyPageYes = true;
                emptyPageInfo = "Yes";            
            }
        }
        else if(index == 1)
        {
            if(chkEmptyPage[index].checked)
            {
                blnEmptyPageNo = true;
                emptyPageInfo = "No";
            }
        }          
     }
     
     if(blnEmptyPageYes && blnEmptyPageNo)
     {
        emptyPageInfo = "both";
     }
     
     window.opener.document.getElementById(GetOpenerObjectID("hdnEmptyPages","input")).value = emptyPageInfo;
     
     //PageType
     var pageTypeInfo = "";
     var pageTypeCount = 0;
     var chkPageType = document.getElementById(GetObjectID("chkPageType","table"));
     var chkPageType = chkPageType.getElementsByTagName("input"); 
     
     for(index = 0; index < chkPageType.length; index++)
     {
        if(chkPageType[index].checked)
        {
            if(pageTypeInfo == null || pageTypeInfo == "")
            {
                pageTypeInfo = index.toString();
            }
            else
            {
                pageTypeInfo += ","+index.toString();
            }
            pageTypeCount++;
        }
     }
     
     if(pageTypeCount == "0" || pageTypeCount == "3")
     {
         pageTypeInfo = "none";     
     } 
     window.opener.document.getElementById(GetOpenerObjectID("hdnPageType","input")).value = pageTypeInfo;
     
    //pageNameCtrlId
    var cboPageName = document.getElementById(GetObjectID("cboPageName", "select"));
    var pageNameInfo = cboPageName.options[cboPageName.selectedIndex].innerHTML;

    if(pageNameInfo == "--Select All--")
    {
        pageNameInfo = "all"
    }
    window.opener.document.getElementById(GetOpenerObjectID("hdnPageName","input")).value = pageNameInfo;

    var discipline = document.getElementById(GetObjectID("cboDisciplineName","select"));
    var disciplineInfo = discipline.options[discipline.selectedIndex].innerHTML;//value
    if(disciplineInfo == "--Select All--")
    {
        disciplineInfo = "all"
    }

    window.opener.document.getElementById(GetOpenerObjectID("hdnDiscipline","input")).value = disciplineInfo;
}

 window.opener.document.getElementById(GetOpenerObjectID("btnPrint","input")).onclick="return continuePrint(true);"
 window.opener.document.getElementById(GetOpenerObjectID("btnPrint","input")).click();
 window.close();

 return false;
 
}

function checkPrintMyPages(checkBoxCtrlId, rblIncludeFilterId)
{   

    var pnlFilter = document.getElementById(GetObjectID("pnlIncludeFilter","div")).elements;
    var chkPrintMyPages = document.getElementById(checkBoxCtrlId);
    var rblFilterP = document.getElementById(rblIncludeFilterId);
    var rblFilter = rblFilterP.getElementsByTagName("input"); 
    
    if(chkPrintMyPages.checked)//if print pages checked enabled filter with selected NO.
    {
       rblFilterP.disabled = false;
       rblFilter[1].checked = true; // Index=0 --> YES ; Index=1 --> NO
    }
    else //if print pages unchecked disabled filter with selected YES.
    {
        rblFilterP.disabled = true;
        rblFilter[0].checked = true; // Index=0 --> YES ; Index=1 --> NO
    }
    
}


function AutoSelectDisciplinePerPageName()
{

    var cboPageNames = document.getElementById(GetObjectID("cboPageName","select"));
    var cboDiscipline = document.getElementById(GetObjectID("cboDisciplineName","select"));
    
    var chkPageType = document.getElementById(GetObjectID("chkPageType","table"));
    var chkPageType = chkPageType.getElementsByTagName("input"); 
   
    var discipline;
    if(cboPageNames.selectedIndex != 0)
    {
        discipline = cboPageNames.options[cboPageNames.selectedIndex].value; 
        
        for(index = 0; index < cboDiscipline.options.length; index++)
        {
            if(cboDiscipline.options[index].innerHTML == discipline)
            {
                cboDiscipline.selectedIndex = index;
            }        
        }
        
        //disabled PageType & discipline if PageNames selected.
        for(pageTypeIndex = 0; pageTypeIndex < chkPageType.length; pageTypeIndex++)
        {
            chkPageType[pageTypeIndex].checked = false;
            chkPageType[pageTypeIndex].disabled = true;
        }
        
        cboDiscipline.disabled = true;
    }
    else 
    {
        //enabled PageType & discipline if PageNames not selected.
        for(pageTypeIndex = 0; pageTypeIndex < chkPageType.length; pageTypeIndex++)
        {
            chkPageType[pageTypeIndex].disabled = false;
        }
        
        cboDiscipline.disabled = false;
        cboDiscipline.selectedIndex = 0;

    }
} 
/************************************
Module : Batch Import
Date : 17/11/2010
************************************/
function openBatchImport(bookId)
{
    var attributes="toolbar=no,location=no, status=yes, directories=no,resizable=no,menubar=no,";
     attributes+="scrollbars=no,width=640, height=190, left=260, top=350";
    var msgAudit = window.open("/Pages/BatchImport.aspx?bookId="+bookId,"BatchImport",attributes);
    msgAudit.focus();
    return false;    
}

function enableTextbox()
{
    var txtSharedPath =  document.getElementById(GetObjectID('txtSharedPath','input'));
    var lblWarnMsg =  document.getElementById(GetObjectID('lblWarningMsg','span'));
    //spanWarningMsg
    if(txtSharedPath.disabled)
    {
        txtSharedPath.disabled = false; 
        lblWarnMsg.style.display = "block";
    }
    
    return false;  
}

function openBatchImportConfiguration(bookId)
{ 
    var attributes="toolbar=no,location=no, status=yes, directories=no,resizable=yes,menubar=no,";
    attributes+="scrollbars=no,width=900,height=800,left=100,top=100";   
    window.open("/Pages/BatchImportConfiguration.aspx?bookId="+bookId,"BatchImportConfiguration",attributes);    
    window.close();
    return false; 
}
function openBatchImportConfirmation(bookId)
{    
    var attributes="toolbar=no,location=no, status=yes, directories=no,resizable=yes,menubar=no,";
    attributes+="scrollbars=no,width=900,height=800,left=100,top=100";   
    window.open("/Pages/BatchImportConfirmation.aspx?BookID="+bookId,"BatchImportConfirmation",attributes);    
    window.close();
    return false;

}
function openBatchImportStatus()
{    
    var attributes="toolbar=no,location=no,status=yes, directories=no,resizable=yes,menubar=no,";
    attributes+="scrollbars=yes,width=900,height=800,left=100,top=100";   
    window.open("/Pages/BatchImportLogReport.aspx", "BatchImportStatus" ,attributes);    
    window.close();
    return false;
}


function gvHeaderSelectAll(checkBoxID)
 {

    if(checkBoxID.checked)
    {
        for (i=0; i < document.forms[0].elements.length; i++) 
        {                
            if (document.forms[0].elements[i].type == 'checkbox')
            {
               document.forms[0].elements[i].checked = true;
            }
        }
    }
    else
    {
        for (i=0; i < document.forms[0].elements.length; i++) 
        {            
            if (document.forms[0].elements[i].type == 'checkbox')
            {
                document.forms[0].elements[i].checked = false;                    
            }
        }
    }
 }
 
 function SelectAllWithHeader()
 {
 //Select/unselect Checkbox header automatically if all checkboxes checked/unchecked.
        var countCheckboxChecked = 0;
        var countTotalCheckbox = 0;
        for (i=0; i < document.forms[0].elements.length; i++) 
        {                
            if (document.forms[0].elements[i].type == 'checkbox')
            {
                countTotalCheckbox++;
                if (document.forms[0].elements[i].id != 'chbHeader')
                {
                    if(document.forms[0].elements[i].checked == true)
                    {
                        countCheckboxChecked++;
                    }     
                }       
            }
        }
        
        if((countTotalCheckbox-1) == countCheckboxChecked)
        {
            document.getElementById('chbHeader').checked = true;
        }
        else
        {
            document.getElementById('chbHeader').checked = false;            
        }
        
 }
 
 function CheckAtleastOneRecordSelected()
 {
        var gvTable = document.getElementById(GetObjectID("gvBatchImportConfirmation","table"));
        var chkEachPage = gvTable.getElementsByTagName("input"); 
        var chkAll = document.getElementById('chbHeader').checked;
        var blnVerifyEachChk = false;
        
        if(!chkAll)
        {
            for(chkIndex = 0; chkIndex < chkEachPage.length; chkIndex++)
            {   
                if(chkEachPage[chkIndex].checked)
                {
                    blnVerifyEachChk =  true;
                }
            }
            
            if(!blnVerifyEachChk)
            {
                alert("Please select at least one page name to import.");
                return false;
            }
        }        
        
 }
/****** End Gopinath Javascript **********/

/**************************************
    Function for Simplfy SignOff
    Added by Praveena
    Date: 12/11/2010
***************************************/

function BulkSignOff()
 { 
    var hdnSignOffPageId =  document.getElementById(GetObjectID('hdnSignOffPageId','input'));
    var tblSearchResults = document.getElementById("tblSearchResults");
    var arrInputTags = tblSearchResults.getElementsByTagName("INPUT");
    var blnChecked = false;
    for (i=0; i <arrInputTags.length; i++) 
    {            
        if(arrInputTags[i].type == 'checkbox' )
        { 

            if (arrInputTags[i].type != 'chbHeader')
            {
                if (arrInputTags[i].checked)
                {
                    hdnSignOffPageId.value += arrInputTags[i].value + ";";
                    blnChecked = true;
                }                       
            }
        }
    }
    if(!blnChecked)
    {
        var btnText = document.getElementById(GetObjectID('btnSignOffID','input')).value;
        if(btnText == 'Sign Off')
        {
            alert('Please select atleast one page before Signing Off.');
        }
        else if(btnText == 'Unsign Off')
        {
            alert('Please select atleast one page before Unsigning Off.');
        }
        return false;
    }
    else
    {
        return true;
    }
 }
 
 function SelectUnSelectAllPages(isHeader)
 {       
    if(isHeader == true) 
    {   
        var objectId="";
        for(index = 0; index < document.documentElement.getElementsByTagName('input').length; index++)
        {
            objectId = document.documentElement.getElementsByTagName('input').item(index).id;
            if(objectId == 'chbSelectID')
            {        
                if(document.documentElement.getElementsByTagName('input').item(index).disabled != true)
                {
                document.documentElement.getElementsByTagName('input').item(index).checked = document.getElementById('chbHeader').checked;
                }
            }
        }
    }
    else if(isHeader == false)
    {       
        if(document.getElementById('chbHeader').checked)
        {
        document.getElementById('chbHeader').checked = !document.getElementById('chbHeader').checked;
        }
    }
 }
 
 function DisablePageType()
{
    var cboPageNames = document.getElementById(GetObjectID('cboPageNameID','select'));          
    var chkPageTypeP = document.getElementById(GetObjectID('cblPageTypeID','table'));
    var chkPageType = chkPageTypeP.getElementsByTagName("input"); 
    
    if(cboPageNames.selectedIndex != 0)
    {
        for(index = 0; index < chkPageType.length; index++)
        {
                 chkPageType[index].checked = false;
                 chkPageType[index].disabled = true;
        }                     
    }
    else 
    {
        for(index = 0; index < chkPageType.length; index++)
        {
                 chkPageType[index].disabled = false;
        }    
    }
} 

function ConfirmReset()
{
 var hdnResetStatus =  document.getElementById(GetObjectID('hdnResetStatus','input'));
 var reset= confirm("Reset will restore the configurations to default/Pre-configured values. Do you really want to continue?");
 if (reset== true)
 {
   hdnResetStatus.value="true";
   return true;
 }
 else
 {
  hdnResetStatus.value="false";
 }
}
/********* End of Praveena Javascript ******/

/** CustomListViewer**/
function doSortAndPaging(URL, pageNumber,recordcount,sortBy,activeStatus,sortType)
{
   var mixedPagingData = URL + ";" +pageNumber + ";" + recordcount + ";" + sortBy + ";" + activeStatus + ";" + sortType;
__doPostBack(mixedPagingData, 'SortOnPaging');
}

/** for dynamic resizing **/
function Resize()
{  
    setTimeout(function()
    {
        try
        {
              var div=document.getElementById(GetObjectID("docviewerdiv","div"));  
                div.style.height=parseInt(GetHeight()-250)+"px";  
           }
           catch(ex)
           {
           }
        }, 200);
}   
           
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

function GetWidth()
{
    var x = 0;
    if (self.innerWidth)
    {
            x = self.innerWidth;
    }
    else if (document.documentElement && document.documentElement.clientWidth)
    {
            x = document.documentElement.clientWidth;
    }
    else if (document.body)
    {
            x = document.body.clientWidth;
    }                    
    return x;
}
function OnClientLoadedHandler(splitter, args)   
{  
      
    var splitter = $find(GetObjectID("RadSplitter1","div"));      
    splitter.set_height(GetHeight()-80);         
    splitter.set_width(GetWidth()-32);   
    
}
/** for dynamic resizing **/

/*** for DREAM 4.0 - eWB 2.0 - Hide/Reveal Empty Pages Module starts here***/
/**** Code not in use
 function OnClientCheckedChanged(sender, eventArgs) 
 {
 debugger;
            var checked = (sender.get_checked()) ? "checked" : "unchecked";
            var radTreeView = $find(GetObjectID("RadTreeView1","div"));  
            if(radTreeView != null)
            {
              if(radTreeView.get_selectedNode() != null)
              {
                 alert("Level: " + radTreeView.get_selectedNode().get_level());
                 if( radTreeView.get_selectedNode().get_level() == 0)
                 {
                   return false;
                 }
                 else if ( radTreeView.get_selectedNode().get_level() == 1)
                 {
                   return true;
                 }
              }
            }
            return false;
 }
function HideEmptyPages()
{

debugger;
            //var checked = (sender.get_checked()) ? "checked" : "unchecked";
            var radTreeView = $find(GetObjectID("RadTreeView1","div"));  
            if(radTreeView != null)
            {
             var selectedNodes = radTreeView.get_selectedNodes();
              if(radTreeView.get_selectedNode() != null)
              {
                 alert("Level: " + radTreeView.get_selectedNode().get_level());
                 if( radTreeView.get_selectedNode().get_level() == 0)
                 {                   
                   return false;
                 }
                 else if ( radTreeView.get_selectedNode().get_level() >= 1)
                 { /// If selected node is Chapter or Page, we may need to reload the TreeNode.
                   /// If selected node is Chapter and it is expanded reload the childnodes of Chapter node.
                    if(radTreeView.get_selectedNode().get_expanded())
                    {
                     radTreeView.get_selectedNode().set_postBack();
                     return true;
                     }
                     else
                     {
                       return false;
                     }
                   return false;
                 }
              }
            }
            return false;
}
****/

/*** for DREAM 4.0 - eWB 2.0 - Hide/Reveal Empty Pages Module ends here***/

/*** for DREAM 4.0 - eWB 2.0 - Delete Module starts here***/

/*** function to show Remove Options pop up div***/
function ShowRemoveOptions(rowId, updateMode)
{  
  /// Set the default options 
  SetDefaultRemoveOptions();
  /// Set the onclick event of "OK" button click or set the values is hidden field
  if(GetHTMLObject(window,'btneWBArchiveOK','input') )
  GetHTMLObject(window,'btneWBArchiveOK','input').onclick =  function(){ContinueArchiveOrRemove(rowId,updateMode);};//"return ContinueArchiveOrRemove('"+ rowId +"','"+ updateMode +"');";
  /// Show the div
  return pop('diveWBArchiveOptions');
}

/*** function to set the default values in Remove Options pop-up***/
function SetDefaultRemoveOptions()
{
if (GetHTMLObject(window,'rdblArchiveForFuture', 'input') && GetHTMLObject(window,'rdblArchiveForFuture', 'input').checked == false)
{
GetHTMLObject(window,'rdblArchiveForFuture', 'input').checked = true;
}

if (GetHTMLObject(window,'rdblActivateRecord', 'input') && GetHTMLObject(window,'rdblActivateRecord', 'input').checked == false)
{
GetHTMLObject(window,'rdblActivateRecord', 'input').checked = true;
}

if(GetHTMLObject(window,'rdblPermanentRemove', 'input') && GetHTMLObject(window,'rdblPermanentRemove', 'input').checked == true)
{
 GetHTMLObject(window,'rdblPermanentRemove', 'input').checked = false;
}
}

/*** function to Archive/Delete the selected item ***/
function ContinueArchiveOrRemove(rowId, updateMode)
{
hide('diveWBArchiveOptions');

if (GetHTMLObject(window,'rdblArchiveForFuture', 'input') && GetHTMLObject(window,'rdblArchiveForFuture', 'input').checked == true)
{
 updateMode = "Archive";
}
else if(GetHTMLObject(window,'rdblActivateRecord', 'input') && GetHTMLObject(window,'rdblActivateRecord', 'input').checked == true)
{
 updateMode = "Activate";
}
else if(GetHTMLObject(window,'rdblPermanentRemove', 'input') && GetHTMLObject(window,'rdblPermanentRemove', 'input').checked == true)
{
 updateMode = "Remove";
}
__doPostBack(updateMode,rowId);

return true;
}

/*** for DREAM 4.0 - eWB 2.0 - Delete Module ends here***/

/*** for DREAM 4.0 - eWB 2.0 - Customise Chapter module starts here ***/

function OpeneWBChpaterReorderPopUp(controlId,radListId,chkBoxCtrlId)
{

// Check if the all items in RadList is checked. If so set the CheckBox as checked= checked else false.
 var list = $find(radListId);;
    if(list != null)
    {
        var listItems = list.get_items();
        var intChapterCount = listItems.get_count();    
        var checkedItemsCount = list.get_checkedItems().length;
        if(intChapterCount == checkedItemsCount)
        {
          GetHTMLObject(window,chkBoxCtrlId,'input').checked = true;
        } 
        else
        {
          GetHTMLObject(window,chkBoxCtrlId,'input').checked = false;
        }  
    }
 var e = window.event;
    var x = e.clientX;
    var y = e.clientY;
    GetHTMLObject(window,controlId,'div').style.display = 'block';
  
    GetHTMLObject(window,controlId,'div').style.left = x - 300;
    GetHTMLObject(window,controlId,'div').style.top = y
    return false;
}

function HideeWBChpaterReorderPopUp(controlId)
{
 GetHTMLObject(window,controlId,'div').style.display='none';
  
    return false;
}


function SelectDeselectAll(control,radListId)
{
    var list = $find(radListId);;
    if(list != null)
    {
        var listItems = list.get_items();
        var intChapterCount = listItems.get_count();       
        var intChapterIndex=0;
        list.trackChanges();
          for(intChapterIndex =0; intChapterIndex < intChapterCount; intChapterIndex++)
          {
            if(control.checked)
            {
            listItems.getItem(intChapterIndex).set_checked(true);
            }
            else
            {
             listItems.getItem(intChapterIndex).set_checked(false);
            }
          }
        list.commitChanges();
    }
}


 function OnClientItemCheckedHandler(sender, eventArgs) 
 {
   var item = eventArgs.get_item();
   var radListBox = item.get_listBox();
   radListBox.trackChanges();
   item.set_checked(item.get_checked());
  radListBox.commitChanges();

 }

function ReOrderItemsInSession(control,radListId,currentSessionOnly)
{

 var list = $find(radListId);
    if(list != null)
    {
        var listItems = list.get_items();
        var intChapterCount = listItems.get_count();       
        var intChapterIndex=0;

           if(list.get_checkedItems().length <= 0)
             {
                alert('Please select atleast one chapter name');
                return false;
             }                     
    }
    if(currentSessionOnly == true)
    {
 __doPostBack("CustomiseChaptersInSession","true");
    }
    else
    {
    __doPostBack("CustomiseChaptersForFuture","true");
    }
return false;
}



/*** for DREAM 4.0 - eWB 2.0 - Customise Chapter module ends here ***/