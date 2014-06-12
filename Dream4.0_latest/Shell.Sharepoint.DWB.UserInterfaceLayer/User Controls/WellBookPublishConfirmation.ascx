<%@ Control Language="C#" AutoEventWireup="true" Codebehind="WellBookPublishConfirmation.ascx.cs"
    Inherits="Shell.SharePoint.DWB.UserInterfaceLayer.WellBookPublishConfirmation" %>
<link href="/_Layouts/DREAM/Styles/DWBStyleSheetRel2_0.css" rel="stylesheet" type="text/css" />
<link href="/_Layouts/DREAM/Styles/DWBReportLayout.css" rel="stylesheet" type="text/css" />
<script language="javascript" src="/_Layouts/DREAM/Javascript/DWBJavascriptFunctionRel2_0.js"></script>
<asp:Panel ID="ExceptionBlock" Visible="false" runat="server">
    <asp:Label ID="lblException" runat="server" Text="" Visible="false" CssClass="labelMessage"></asp:Label>
    <br />
</asp:Panel>
<asp:Panel ID="pnlPublishBook" runat="server" Width="100%">
<table id="tblPublishConfirmation"  class="DWBtableAdvSrchBorder" cellspacing="2" cellpadding="2" border="0" width="100%"> 
    <tr>
        <td valign="top" style="width: 30%; height: 284px">
            <asp:Panel ID="pnlPublishProcess" runat="server" Wrap="false" Width="100%" Style="background-color: Gray;
                font-weight: bold" Height="100%">
               Step 1: Sign off Pages by Page Owner<br />
Step 2: Update Pages<br />
Step 3: Well Book Sign Off by Well Book Owner
                <br />
Step 4: Print Well Book
                <br />
Step 5: Well Review 
                <br />
Step 6: Update Pages and Unsign Pages<br />
Step 7: Well Book Sign Off by Well Book Owner<br />
Step 8: Publish Well Book 
                <br />
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
           - Frozen 
                <br />
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
            - Removing comments
                <br />
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
            - All pages are unsigned
                <br />
Step 9: Rename Live Well Book

            </asp:Panel>
        </td>
        <td width="70%" valign="top" style="height: 284px">
            <table width="100%">
                <tr align="left">
                    <td class="DWItemText" align="left" width="20%">
                        Book Name:&nbsp;</td>
                    <td class="DWBtdAdvSrchItem" width="55%">
                        <asp:Label ID="lblBookName" runat="server" Text="" CssClass="DWBqueryfieldmini" ></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 100%; height: 166px;" colspan="2" align="left">
                    <asp:Label ID="lblPublish" runat="server" Text="" Visible="false" CssClass="labelMessage"></asp:Label>
                        <asp:GridView ID="grdWellBookSummary" AutoGenerateColumns="False" runat="server"
                            ShowFooter="False" Width="75%" OnRowDataBound="grdWellBookSummary_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="Page_Owner" ItemStyle-Width="5%" />
                                <asp:TemplateField ItemStyle-HorizontalAlign="Right" ItemStyle-Width="5%">
                                    <ItemTemplate>                                       
                                        <asp:Label ID="TotalId" runat="server" Text='<%# Eval("Total") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Right" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="SignOffId" runat="server" Text='<%# Eval("Signed_Off") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Right" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="NotSigned_OffId" runat="server" Text='<%# Eval("NotSigned_Off") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Right" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <asp:Label ID="EmptyId" runat="server" Text='<%# Eval("Empty") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td class="DWItemText" width="20%">
                       Change book name:&nbsp;</td>
                    <td class="DWBtdAdvSrchItem">
                        <asp:TextBox ID="txtNewBookName" runat="server"  Width="80%" CssClass="DWBqueryfieldmini"></asp:TextBox>
                </tr>
                <tr>
                <td colspan="2" align="right">
                <asp:Button ID="cmdContinuePublish" runat="server" Text="Finish" OnClick="cmdContinuePublish_Click" Visible="false" CssClass="DWBbuttonAdvSrch" OnClientClick="return DWBValidateNewBookTitle();" />
                <asp:Button ID="cmdCancel" runat="server" Text="Cancel" OnClientClick="CloseWithoutPrompt()" CssClass="DWBbuttonAdvSrch" />&nbsp;
                </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
</asp:Panel>
<asp:Panel ID="pnlBookPublished" runat="server" Width="50%" Visible="false" Wrap="true">
<asp:Label ID="lblPublishStatus" runat="server" CssClass="labelMessage" ></asp:Label>
    <br />
    <br />
<br />
<asp:Button ID="btnClose" runat="server" Text="Close" OnClientClick="CloseChildAndRefreshParet('BookMaintenance.aspx','');" CssClass="DWBbuttonAdvSrch" /></asp:Panel>