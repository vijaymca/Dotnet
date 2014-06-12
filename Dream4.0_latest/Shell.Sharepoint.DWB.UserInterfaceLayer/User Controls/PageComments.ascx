<%@ Control Language="C#" AutoEventWireup="true" Codebehind="PageComments.ascx.cs"
    Inherits="Shell.SharePoint.DWB.UserInterfaceLayer.PageComments" %>
<script language="javascript" src="/_Layouts/DREAM/Javascript/DWBJavascriptFunctionRel2_0.js">

</script>

<link href="/_Layouts/DREAM/Styles/DWBStyleSheetRel2_0.css" rel="stylesheet" type="text/css" />
<asp:Panel runat="server" ID="pnlComment" Width="95%" Visible="false">
<table class="DWBtableAdvSrchBorder" cellspacing="0" cellpadding="2" border="0" width="100%">
    <tr>
        <td class="DWBtdAdvSrchHeader" colspan="2" style="font-weight: bold">
            <asp:Label ID="lbPageSequence" runat="server" Font-Bold="True" Text="Page Comments"></asp:Label></td>
    </tr>
</table>
<br />
<asp:Panel ID="ExceptionBlock" Visible="false" runat="server">
    <asp:Label ID="lblException" runat="server" Text="" Visible="false" CssClass="labelMessage"></asp:Label>
    <br />
</asp:Panel>
<table cellspacing="0" cellpadding="2" border="0" width="90%">
    <tr>
        <td colspan="2" style="text-align:center;">                
            <table cellspacing="2" cellpadding="2" border="0" width="100%">
                <tr>
                    <td style="width: 100%; height: 100px; margin:5px 5px 5px 5px;" colspan="2" align="left" valign="top">                        
                        <asp:TextBox ID="txtPageComments" runat="server" MaxLength="250" Height="100px" Width="80%" CausesValidation="True" TextMode="MultiLine" onkeydown="return ValidateTextLength(this,250,'Comments');" ></asp:TextBox>                        
                    </td>
                </tr>
                <tr>
                    <td style="width:100%" colspan="2" align="right">
                        <asp:CheckBox ID="chkShareComments" runat="server" Text="Share Comments" Checked="false" />
                        &nbsp;&nbsp; &nbsp;
                        <asp:Button ID="btnSave" CssClass="DWBbuttonAdvSrch" runat="server" Text="Save" OnClientClick="return DWBValidatePageComments();" OnClick="btnSave_Click"  />                        
                    </td>
                </tr>
            </table>                    
        </td>
    </tr>
    <tr>
        <td align="center" valign="top" width="950px">
        <div style="clear: both; overflow:auto; position:relative; margin:5PX 5PX 5PX 5PX; width:screen.width; max-height:350px;">
            <asp:GridView ID="grdAuditTrail" AutoGenerateColumns="False" runat="server" ShowFooter="False"
                Width="100%" Height="98%">
                <HeaderStyle BackColor="#E0ECF0" Font-Bold="True" />
                <AlternatingRowStyle BackColor="#ECECEC" />
                <Columns>
                    <asp:BoundField DataField="UserName" HeaderText="User Name" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" />
                    <asp:BoundField DataField="Comment" HeaderText="Comments" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="30%"/>                   
                    <asp:BoundField DataField="Created" HeaderText="Date/Time" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="10%"/>
                </Columns>
            </asp:GridView>
            </div>
        </td>
    </tr>
</table>
</asp:Panel>        