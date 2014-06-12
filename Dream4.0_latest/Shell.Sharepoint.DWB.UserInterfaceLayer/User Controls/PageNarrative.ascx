<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PageNarrative.ascx.cs" Inherits="Shell.SharePoint.DWB.UserInterfaceLayer.PageNarrative" %>

<link href="/_Layouts/DREAM/Styles/DWBStyleSheetRel2_0.css" rel="stylesheet" type="text/css" />
<%--<link href="/_Layouts/DREAM/Styles/DWBReportLayout.css" rel="stylesheet" type="text/css" />--%>

<br />
<asp:Panel ID ="ExceptionBlock" Visible="false" runat="server">    
    <asp:Label ID="lblException" runat="server" Text="" Visible="false" CssClass="labelMessage"></asp:Label> 
    <br />   
</asp:Panel>

<table width="90%"> <!--// class="scrollTable"> !-->
<tr>
<td align="right" width="100%">
<asp:Button ID="btnAdd" runat="server" CssClass="DWBbuttonAdvSrch" Text="Save" OnClientClick = "return ValidateUpdateChapterPageDetail('Narrative');" OnClick="btnAdd_Click" Visible="false" />
</td>
</tr>
<tr>
<td width="100%" style="margin:10px;">
    <asp:TextBox ID="txtNarrative" runat="server" Height="126px" TextMode="MultiLine" Enabled="false" onkeydown="return ValidateTextLength(this,2000,'Narrative');" Width="100%" >&lt;&lt;No Narrative has been added to the page.&gt;&gt;</asp:TextBox>
</td>
</tr>
</table>
