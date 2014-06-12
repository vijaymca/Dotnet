<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageStoryBoard.ascx.cs" Inherits="Shell.SharePoint.DWB.UserInterfaceLayer.PageStoryBoard" %>

<link href="/_Layouts/DREAM/Styles/DWBStyleSheetRel2_0.css" rel="stylesheet" type="text/css" />
<asp:Panel ID="pnlStoryBoard" Visible ="false" runat="server">
<asp:Panel ID ="ExceptionBlock" Visible="false" runat="server">   
<br /> 
    <asp:Label ID="lblException" runat="server" Text="" Visible="false" CssClass="labelMessage"></asp:Label> 
    <br />   
</asp:Panel>

<div>
<table Width="80%">
<tr Width="100%">
<td align="left">
    <asp:Table ID="Table1" runat="server" CssClass= "scrollTable" Width="100%" BorderWidth="1px" BorderColor=" #336699">
        <asp:TableHeaderRow ID="TableHeaderRow1" runat="server" Width="100%" CssClass ="fixedHeaderPageSequence">
            <asp:TableHeaderCell ID="TableHeaderCell1" style="text-align:right;" runat="server" ColumnSpan=2 Width="100%">[Fields marked with * are editable.]</asp:TableHeaderCell>
            <%--<asp:TableHeaderCell ID="TableHeaderCell2" HorizontalAlign="Center" runat="server" Width="70%">Information</asp:TableHeaderCell>--%>        
        </asp:TableHeaderRow>
        <asp:TableRow ID="Row1" runat="server" Width="100%" CssClass="evenRowStyle">
        <asp:TableCell ID="MetaData1" Width="30%" Font-Bold="true" HorizontalAlign="right" runat="server" VerticalAlign="top">Page Type</asp:TableCell>
        <asp:TableCell ID="Information1" Width="70%" runat="server">
        <asp:TextBox ID="txtPageType" style="border: none;" Width="100%" runat="server" Enabled="false"></asp:TextBox>
        </asp:TableCell>
        </asp:TableRow> 
        <asp:TableRow ID="Row2" runat="server" Width="100%" CssClass="oddRowStyle">
        <asp:TableCell ID="MetaData2" Width="30%" Font-Bold="true" HorizontalAlign="right" runat="server" VerticalAlign="top">Page Title</asp:TableCell>
        <asp:TableCell ID="Information2" Width="70%" runat="server">
        <asp:TextBox ID="txtPageTitle" style="border: none;" Width="100%" BackColor="#ECECEC" runat="server" Enabled="false"></asp:TextBox>
        </asp:TableCell>        
        </asp:TableRow> 
        <asp:TableRow ID="Row3" runat="server" Width="100%" CssClass="evenRowStyle">
        <asp:TableCell ID="MetaData3" Width="30%" Font-Bold="true" HorizontalAlign="right" runat="server" VerticalAlign="top">Connection Type</asp:TableCell>
        <asp:TableCell ID="Information3" Width="70%" runat="server">
        <asp:TextBox ID="txtConnectionType" Width="100%" style="border: none;" runat="server" Enabled="false"></asp:TextBox>
        </asp:TableCell>
        </asp:TableRow> 
        <asp:TableRow ID="Row4" runat="server" Width="100%" CssClass="oddRowStyle">
        <asp:TableCell ID="MetaData4" Width="30%" Font-Bold="true" HorizontalAlign="right" runat="server" VerticalAlign="top">* Source</asp:TableCell>
        <asp:TableCell ID="Information4" Width="70%" runat="server">
        <asp:TextBox ID="txtSource" style="border: none;" Width="100%" Height ="70px" BackColor="#ECECEC" runat="server" Enabled="false" TextMode ="MultiLine" Wrap ="true" onkeydown="return ValidateTextLength(this,250,'Source');"></asp:TextBox>
        </asp:TableCell>
        </asp:TableRow> 
        <asp:TableRow ID="Row5" runat="server" Width="100%" CssClass="evenRowStyle">
        <asp:TableCell ID="MetaData5" Width="30%" HorizontalAlign="right" Font-Bold="true" runat="server" VerticalAlign="top">Discipline</asp:TableCell>
        <asp:TableCell ID="Information5" Width="70%" runat="server">        
        <asp:TextBox ID="txtDiscipline" style="border: none;" Width="100%" runat="server" Enabled="false"></asp:TextBox>
        </asp:TableCell>
        </asp:TableRow> 
        <asp:TableRow ID="Row6" runat="server" Width="100%" CssClass="oddRowStyle">
        <asp:TableCell ID="MetaData6" Width="30%" HorizontalAlign="right" Font-Bold="true" runat="server" VerticalAlign="top">Master Page Name</asp:TableCell>
        <asp:TableCell ID="Information6" Width="70%" runat="server">
        <asp:TextBox ID="txtMasterPageName" style="border: none;" Width="100%" BackColor="#ECECEC" runat="server" Enabled="false"></asp:TextBox>
        </asp:TableCell>
        </asp:TableRow> 
        <asp:TableRow ID="Row7" runat="server" Width="100%" CssClass="evenRowStyle">
        <asp:TableCell ID="MetaData7" Width="30%" HorizontalAlign="right" Font-Bold="true" runat="server" VerticalAlign="top">* Application Generating Page</asp:TableCell>
        <asp:TableCell ID="Information7" Width="70%" runat="server">
        <asp:TextBox ID="txtApplicationGeneratingPage" style="border: none;"  Width="100%" Height ="70px" runat="server" Enabled="false" TextMode ="MultiLine" Wrap ="true" onkeydown="return ValidateTextLength(this,250,'Application Generating Page');"></asp:TextBox>
        </asp:TableCell>
        </asp:TableRow> 
        <asp:TableRow ID="Row8" runat="server" Width="100%" CssClass="oddRowStyle">
        <asp:TableCell ID="MetaData8" Width="30%" HorizontalAlign="right" Font-Bold="true" runat="server" VerticalAlign="top">* Application Template</asp:TableCell>
        <asp:TableCell ID="Information8" Width="70%" runat="server">
        <asp:TextBox ID="txtApplicationTemplate" style="border: none;"  Width="100%" Height ="70px" BackColor="#ECECEC" runat="server" Enabled="false" TextMode ="MultiLine" Wrap ="true" onkeydown="return ValidateTextLength(this,250,'Application Template');"></asp:TextBox>
        </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="Row9" runat="server" Width="100%" CssClass="evenRowStyle">
        <asp:TableCell ID="MetaData9" Width="30%" HorizontalAlign="right" Font-Bold="true" runat="server" VerticalAlign="top">SOP</asp:TableCell>
        <asp:TableCell ID="Information9" Width="70%" runat="server">
        <asp:TextBox ID="txtSOP" style="border: none;" Width="100%" runat="server" Enabled="false"></asp:TextBox>
        </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="Row10" runat="server" Width="100%" CssClass="oddRowStyle">
        <asp:TableCell ID="MetaData10" Width="30%" HorizontalAlign="right" Font-Bold="true" runat="server" VerticalAlign="top">Created By</asp:TableCell>
        <asp:TableCell ID="Information10" Width="70%" runat="server">
        <asp:TextBox ID="txtCreatedBy" style="border: none;" Width="100%" BackColor="#ECECEC" runat="server" Enabled="false"></asp:TextBox>
        </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="Row11" runat="server" Width="100%" CssClass="evenRowStyle">
        <asp:TableCell ID="MetaData11" Width="30%" HorizontalAlign="right" Font-Bold="true" runat="server" VerticalAlign="top">Creation Date</asp:TableCell>
        <asp:TableCell ID="Information11" Width="70%" runat="server">
        <asp:TextBox ID="txtCreationDate" style="border: none;" Width="100%" runat="server" Enabled="false"></asp:TextBox>
        </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="Row12" runat="server" Width="100%" CssClass="oddRowStyle">
        <asp:TableCell ID="MetaData12" Width="30%" HorizontalAlign="right" Font-Bold="true" runat="server" VerticalAlign="top">Page Owner</asp:TableCell>
        <asp:TableCell ID="Information12" Width="70%" runat="server">
        <asp:TextBox ID="txtPageOwner" style="border: none;" Width="100%" BackColor="#ECECEC" runat="server" Enabled="false"></asp:TextBox>
        </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</td>
</tr>
<tr Width="100%">
<td align="right">
    <asp:Button ID="btnSave" Visible="false" CssClass="DWBbuttonAdvSrch" runat="server" Text="Save" OnClientClick ="return ValidateUpdateChapterPageDetail('Story Board');" OnClick="BtnSave_Click" />
  
    <input type="hidden" runat ="server" id="hidUserName" />
     <input type="hidden" runat ="server" id="hidBookOwner" />
</td>
</tr>
</table>
</div>
</asp:Panel>

