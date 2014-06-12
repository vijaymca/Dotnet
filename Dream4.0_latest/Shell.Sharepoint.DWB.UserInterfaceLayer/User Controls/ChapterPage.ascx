<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChapterPage.ascx.cs" Inherits="Shell.SharePoint.DWB.UserInterfaceLayer.ChapterPage" %>
<%--<script language="javascript" src="/_Layouts/DREAM/Javascript/CastleBusyBoxRel2_1.js"></script>--%>
<script language="javascript" src="/_Layouts/DREAM/Javascript/DWBJavascriptFunctionRel2_0.js"></script>
<link href="/_Layouts/DREAM/Styles/DWBStyleSheetRel2_0.css" rel="stylesheet" type="text/css" />


<%--<!--Display Progress bar Frame starts here-->
<iframe id="BusyBoxIFrame" visible="false" style="border:3px double #D2D2D2" name="BusyBoxIFrame" frameBorder="0" scrolling="no" ondrop="return false;">
</iframe>
<SCRIPT>
	// Instantiate our BusyBox object
	var busyBox = new BusyBox("BusyBoxIFrame", "busyBox", 1, "processing", ".gif", 125, 147, 207);	
</SCRIPT>--%>
<!--Display Progress bar Frame Ends here-->
<div id="AdvancedSearchContainer">                        
<asp:Panel ID="pnlTemplate" runat="server" Width="100%">
<table class="DWBtableAdvSrchBorder" cellspacing="0" cellpadding="2" border="0" width="100%">
    <tr>
        <td class="DWBtdAdvSrchHeader" colspan="2" style="font-weight: normal">
            &nbsp;<asp:Label ID="lblChapter" runat="server" Text="Add Page" Width="121px" Font-Bold="True"></asp:Label></td>
    </tr>
</table>
<br />
<asp:Panel ID ="ExceptionBlock" Visible="false" runat="server">    
    <asp:Label ID="lblException" runat="server" Text="" Visible="false" CssClass="labelMessage"></asp:Label> 
    <br />   
</asp:Panel>
    <asp:HiddenField ID="hdnListitemStatus" runat="server" />
<table class="DWBtableAdvSrchBorder" cellspacing="2" cellpadding="2" border="0" width="100%"> 
    <tr>
             <td class="DWBtdAdvSrchSubHeader" colspan="2" style="height: 18px"><b>Details</b>
                &nbsp;[ <span class ="DWBMandatoryMessage">* indicates mandatory field</span>
            ]</td>
        </tr>             
    <tr>
        <td class="DWItemText" style="width: 36%; height: 26px;">
          <span class ="DWBMandatory">*</span> Pages in Library</td>
        <td class="DWBtdAdvSrchItem" style="width: 80%; height: 26px;">
            &nbsp;<asp:DropDownList ID="cboMasterPages" runat="server" AutoPostBack="True" Width="231px" OnSelectedIndexChanged="cboMasterPages_SelectedIndexChanged">
              <asp:ListItem Value ="--Select--"></asp:ListItem> 
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td class="DWItemText" style="width: 36%; height: 26px;">
          Sign Off Discipline</td>
        <td class="DWBtdAdvSrchItem" style="width: 80%; height: 26px;">
            &nbsp;<asp:TextBox ID="txtDiscipline" runat="server" Enabled="False" Width="227px"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="DWItemText" style="width: 36%; height: 35px">
            Page Owner</td>
        <td class="DWBtdAdvSrchItem" style="width: 80%; height: 35px">
            &nbsp;<asp:DropDownList ID="cboPageOwner" runat="server" CssClass="DWBdropdownAdvSrch"
                Width="232px">
                 <asp:ListItem Value ="--Select--"></asp:ListItem> 
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td align="center"  style=" height: 28px" colspan ="2">
            <asp:Button ID="cmdSave" runat="server" Width ="10%" Text="Save"  CssClass="DWBbuttonAdvSrch" OnClientClick="javascript:return ValidateDWBChapterPage();" OnClick="cmdSave_Click" />
            <asp:Button ID="cmdCancel" runat="server" Text="Cancel" Width ="10%" CssClass="DWBbuttonAdvSrch" OnClick="cmdCancel_Click" Font-Size="X-Small" />&nbsp;
        </td>
        
  
    </tr>
</table>     
</asp:Panel>
</div>
<script type="text/JavaScript">
    setWindowTitle('Add Page to Chapter');    
</script>
