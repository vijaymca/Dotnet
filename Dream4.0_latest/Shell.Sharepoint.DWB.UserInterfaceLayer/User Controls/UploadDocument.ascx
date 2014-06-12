<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UploadDocument.ascx.cs" Inherits="Shell.SharePoint.DWB.UserInterfaceLayer.UploadDocument" %>
<link href="/_LAYOUTS/DREAM/styles/DREAMStyleSheetRel2_1.css" rel="stylesheet" type="text/css" />
<%--<script language="javascript" src="/_Layouts/DREAM/Javascript/CastleBusyBoxRel2_1.js"></script>--%>
<script language="javascript" src="/_Layouts/DREAM/Javascript/DREAMJavaScriptFunctionsRel2_1.js"></script>
<script language="javascript" src="/_Layouts/DREAM/Javascript/DWBJavascriptFunctionRel2_0.js"></script>


<asp:Panel ID="AdvancedSearchContent" runat="server" Width="100%" DefaultButton ="btnUpload">
<table width="100%">
 <tr>
 <td>
        <asp:Label ID="lblErrorMessage" runat="server" Text="" Visible="false" CssClass="labelMessage" /></td>
 </tr>
    
    <tr>
    <tr>
    
      <td>
      <input type="file" runat ="server" id="fileUploader" style ="width:100%"  class ="button"/>
            </td>
            </tr> 
            <tr>
 <td>
       </td>
 </tr>
    
    <tr>
                        <tr>
        <td valign="top" align ="right">
      <asp:Button id="btnUpload"  Text ="Upload" runat ="server"  CssClass ="button"  OnClick="btnUpload_Click"  Width="90px"/>
                &nbsp;                           
       <input type ="button" id="btnCancel" runat ="server" value ="Cancel" class ="button" style ="width:90px" onclick ="javascript:CloseWithoutPrompt()" />
        <input type ="button" id="btnClose" visible = "false" runat ="server" value ="Close" style ="width:90px" class ="button" onclick ="javascript:eWBCheckIfUploadedAndRefreshParent()" />
                    </td>                       
                    </tr>
</table>    
   <input type ="hidden" id="hidPageId" runat="server"/>
  
<script  language ="javascript" type="text/javascript">

window.attachEvent("onunload",eWBCheckIfUploadedAndRefreshParent);

</script>
</asp:Panel>