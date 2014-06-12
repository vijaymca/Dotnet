<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChapterMaintenance.ascx.cs" Inherits="Shell.SharePoint.DWB.UserInterfaceLayer.ChapterMaintenance" %>

<%--<script language="javascript" src="/_Layouts/DREAM/Javascript/DWBJavascriptFunction.js"></script>
--%><link href="/_Layouts/DREAM/Styles/DWBStyleSheetRel2_0.css" rel="stylesheet" type="text/css" />

<table class="DWBtableAdvSrchBorder" cellspacing="2" cellpadding="2" border="0" width="100%"> 
<tr>
<td>
<asp:Label ID="lblException" runat="server" Text="" Visible="false" CssClass="labelMessage"></asp:Label> 
</td>
</tr>
<tr>
    <td class="DWItemText" style="width: 28%; height: 28px;">
        Asset Type
         </td>
        <td class="DWBtdAdvSrchItem" style="width: 60%; height: 28px;">
           <asp:TextBox ID="txtAssetType" runat ="server" CssClass="DWBqueryfieldmini" Enabled="false" Width="152px"></asp:TextBox>
           </td>
           <td style="width:20%;height: 28px;">
           <asp:Button ID="btnPrint" runat="server" Text="Print this Chapter" OnClick="cmdPrintChapter_Click" CssClass="button"  />
           </td>
    </tr>
    <tr>
        <td class="DWItemText" style="width: 28%; height: 26px;">
        Name</td>
        <td class="DWBtdAdvSrchItem" style="width: 80%; height: 26px;" colspan="2">
            <asp:TextBox ID="txtAssetValue" runat ="server" CssClass="DWBqueryfieldmini" Enabled="false" Width="275px" ></asp:TextBox></td>
    </tr>
    <tr>
        <td class="DWItemText" style="width: 28%; height: 35px">
         Template
         </td> 
        <td class="DWBtdAdvSrchItem" style="width: 80%; height: 35px" colspan="2">
            <asp:TextBox ID="txtTemplateTitle" runat ="server" CssClass="DWBqueryfieldmini" Enabled="false" Width="275px" ></asp:TextBox>
          </td> 
    </tr>
 
    <tr>
   <td  class="DWItemText" style="width: 28%; height: 80px;">
       Description</td>
      <td  class="DWBtdAdvSrchItem" style="width: 80%; height: 80px;" colspan="2">
          <asp:TextBox ID="txtDescription" runat="server" Height="100px" TextMode="MultiLine" Width="275px" CssClass="DWBqueryfieldmini" Enabled="false"></asp:TextBox></td> 
   </tr>
</table>
<input type="hidden" runat ="server" id="hdnListitemStatus" />
<asp:HiddenField ID="hdnIncludeStoryBoard" runat="server" />
<asp:HiddenField ID="hdnIncludePageTitle" runat="server" />
<asp:HiddenField ID="hdnPrintMyPages" runat="server" />
<asp:HiddenField ID="hdnIncludeFilter" runat="server" />
<asp:HiddenField ID="hdnSignedOffPages" runat="server" />
<asp:HiddenField ID="hdnEmptyPages" runat="server" />
<asp:HiddenField ID="hdnPageType" runat="server" />
<asp:HiddenField ID="hdnPageName" runat="server" />
<asp:HiddenField ID="hdnDiscipline" runat="server" />