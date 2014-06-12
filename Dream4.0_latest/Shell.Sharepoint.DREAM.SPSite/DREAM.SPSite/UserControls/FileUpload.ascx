<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FileUpload.ascx.cs" Inherits="Shell.SharePoint.DREAM.Site.UI.FileUpload" %>

<asp:Panel ID="AdvancedSearchContent" runat="server" Width="100%" DefaultButton ="btnOK">
<table width="100%" cellpadding="5px" cellspacing="0">
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
      <asp:Button id="btnOK"  Text ="Upload" runat ="server"  CssClass ="button"  OnClientClick ="return ValidateUpload();" OnClick="BtnOK_Click"  Width="90px"/>
                &nbsp;                           
       <input type ="button" id="btnCancel" runat ="server" value ="Cancel" class ="button" style ="width:90px" onclick ="javascript:CloseWithoutPrompt()" />
        <input type ="button" id="btnClose" visible = "false" runat ="server" value ="Close" style ="width:90px" class ="button" onclick ="javascript:CheckIfUploadedAndRefreshParent()" />
                    </td>                       
                    </tr>
</table>     
</asp:Panel>