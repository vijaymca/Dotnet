<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AlterPageSequence.ascx.cs" Inherits="Shell.SharePoint.DWB.UserInterfaceLayer.AlterPageSequence" %>
<%--<script language="javascript" src="/_Layouts/DREAM/Javascript/CastleBusyBoxRel2_1.js"></script>--%>
<script language="javascript" src="/_Layouts/DREAM/Javascript/DWBJavascriptFunctionRel2_0.js">

</script>
<link href="/_Layouts/DREAM/Styles/DWBStyleSheetRel2_0.css" rel="stylesheet" type="text/css" />
<link href="/_Layouts/DREAM/Styles/DWBReportLayout.css" rel="stylesheet" type="text/css" />
<table class="DWBtableAdvSrchBorder" cellspacing="0" cellpadding="2" border="0" width="100%">
    <tr>
        <td class="DWBtdAdvSrchHeader" colspan="2" style="font-weight: bold">
            <asp:Label ID="lbPageSequence" runat="server" Font-Bold="True" Text="Alter Page Sequence"
                Width="158px"></asp:Label></td>
    </tr>
</table>
<br />
<asp:Panel ID ="ExceptionBlock" Visible="false" runat="server">    
    <asp:Label ID="lblException" runat="server" Text="" Visible="false" CssClass="labelMessage"></asp:Label> 
    <br />   
</asp:Panel>
<asp:Button ID="btnRenumber" runat="server" OnClick="btnRenumber_Click" Text="Renumber" CssClass="DWBbuttonAdvSrch" />
<br />
<br/>
<div id ="tableContainer" class ="tableContainer" style="width:450px;border:0px solid #FFFFFF">
<asp:Table ID="Table1" runat="server" Height="59px" CssClass= "scrollTable" BorderWidth="1px" BorderColor=" #9b9797">
    <asp:TableHeaderRow runat="server" CssClass ="fixedHeaderPageSequence">
        <asp:TableHeaderCell runat="server" HorizontalAlign="center" Text="Seq" Width="5%"></asp:TableHeaderCell>
        <asp:TableHeaderCell runat="server" HorizontalAlign="center" Text="Title" Width="50%"></asp:TableHeaderCell>
        <asp:TableHeaderCell runat="server" Text="AssetType" HorizontalAlign="center" Width="25%"></asp:TableHeaderCell>
    </asp:TableHeaderRow>
   </asp:Table>
</div>
&nbsp; &nbsp;
&nbsp; &nbsp;
<table style="width: 647px">
    <tr>
      <td>
          &nbsp; &nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click"  CssClass="DWBbuttonAdvSrch" Text="Save" Width="77px" />
          <asp:Button ID="btnCancel" runat="server" CssClass="DWBbuttonAdvSrch"
              OnClick="btnCancel_Click" Text="Cancel" /></td> 
        <td style="width: 324px">
            </td>
             <td>
        </td>
    </tr>
</table>
