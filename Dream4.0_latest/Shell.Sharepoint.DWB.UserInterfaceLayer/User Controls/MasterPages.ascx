<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MasterPages.ascx.cs" Inherits="Shell.SharePoint.DWB.UserInterfaceLayer.MasterPages" %>

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
<asp:Panel ID="pnlMasterPage" DefaultButton="cmdOK" runat="server" Width="100%">
<table class="DWBtableAdvSrchBorder" cellspacing="0" cellpadding="2" border="0" width="100%">
    <tr>
        <td class="DWBtdAdvSrchHeader" colspan="2" style="font-weight: normal">
            <asp:Label ID="lblMasterPage" runat="server" Font-Bold="True" Text="Add Master Page"
                ></asp:Label></td>
    </tr>
</table>
<br />
<asp:Panel ID ="ExceptionBlock" Visible="false" runat="server">    
    <asp:Label ID="lblException" runat="server" Text="" Visible="false" CssClass="labelMessage"></asp:Label> 
    <br />   
</asp:Panel>
    <asp:HiddenField ID="hdnListItemStatus" runat="server" />
<table class="DWBtableAdvSrchBorder" cellspacing="2" cellpadding="2" border="0" width="100%"> 
    <tr>
            <td class="DWBtdAdvSrchSubHeader" colspan="2" style="height: 18px"><b>Details</b>
                &nbsp;[ <span class ="DWBMandatoryMessage">* indicates mandatory field</span>
            ]</td>
        </tr>             
    <tr>
    
        <td class="DWItemText" style="width: 23%">
           <span class ="DWBMandatory">*</span>
            <span>Name</span></td>
        <td class="DWBtdAdvSrchItem" style="width: 77%">
            <asp:TextBox ID="txtPageTitle" runat="server" Width="45%" CssClass="DWBqueryfieldmini" TabIndex="1" MaxLength="80"></asp:TextBox>
        </td>           
    </tr>
    <tr>
       <td class="DWItemText" style="width: 23%; height: 28px;">
          <span class ="DWBMandatory">*</span> 
            <span>Page Title</span></td>
        <td class="DWBtdAdvSrchItem" style="width: 77%; height: 28px;">
            <asp:TextBox ID="txtTitleTemplate" runat="server" Width="45%" CssClass="DWBqueryfieldmini" TabIndex="2"></asp:TextBox>           
        </td>
    </tr>   
    <tr>
       <td class="DWItemText" style="width: 23%">
          <span class ="DWBMandatory">*</span> <span>Asset Type</span></td>
        <td class="DWBtdAdvSrchItem" style="width: 77%;">
            <asp:DropDownList ID="cboAssetType" runat="server" CssClass="DWBdropdownAdvSrch" Width="35%" TabIndex="4" OnSelectedIndexChanged="cboAssetType_SelectedIndexChanged" AutoPostBack="True">
                    <asp:ListItem>--Select--</asp:ListItem>
                    <asp:ListItem>Well</asp:ListItem>
                    <asp:ListItem>Field</asp:ListItem>
                    <asp:ListItem>Well Bore</asp:ListItem>
                </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="DWItemText" style="width: 23%" >
        <span class ="DWBMandatory">*</span>
            <span>Sign Off Discipline</span></td>
        <td class="DWBtdAdvSrchItem" style="width: 77%">
            <asp:DropDownList ID="cboDiscipline" runat="server" CssClass="DWBdropdownAdvSrch" Width="35%" TabIndex="5" >
                    <asp:ListItem>--Select--</asp:ListItem>
                </asp:DropDownList>
        </td>
    </tr> 
    <tr>
  
        <td class="DWItemText" style="width: 23%">
            <span>Standard Operating Procedure</span></td>
        <td class="DWBtdAdvSrchItem" style="width: 77%">
            <asp:TextBox ID="txtSOP" runat="server" Width="45%" CssClass="DWBqueryfieldmini" TabIndex="6"></asp:TextBox>           
        </td>
    </tr>
    <tr>
         <td class="DWItemText" style="width: 23%">
             <span class ="DWBMandatory">*</span>
            <span>Connection Type</span></td>
        <td class="DWBtdAdvSrchItem" style="width: 77%">
            <asp:DropDownList ID="cboConnectionType" runat="server" CssClass="DWBdropdownAdvSrch" Width="35%" TabIndex="7" onchange ="javascript:return ValidateConnectionType();">
                    <asp:ListItem>--Select--</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                </asp:DropDownList>
        </td>
    </tr>
   <tr>
     <td class="DWItemText" style="width: 23%">
            <span>
                <asp:Label ID="lblTemplates" runat="server" Text="Add this page to Template(s)" Width="166px" CssClass="DWItemText"></asp:Label></span></td>
   <td class="DWBtdAdvSrchItem" style="width: 77%;">
       <asp:ListBox ID="lstTemplates" runat="server" SelectionMode="Multiple" Width="45%">
           <asp:ListItem>Select Asset type to view applicable Templates</asp:ListItem>
       </asp:ListBox></td>
   </tr> 
    <tr>
        <td align="center" colspan="2">
            <asp:Button ID="cmdOK" runat="server" Text="Save" CssClass="DWBbuttonAdvSrch"  OnClientClick="javascript:return ValidateMasterpage();" OnClick="cmdOK_Click" TabIndex="10"/>
            <asp:Button ID="cmdCancel" runat="server" Text="Cancel" CssClass="DWBbuttonAdvSrch" OnClick="cmdCancel_Click" TabIndex="9"/>&nbsp;
        </td>
    </tr>
</table>     
</asp:Panel>
</div>
<script type="text/JavaScript">
    setWindowTitle('Master Page');    
</script>