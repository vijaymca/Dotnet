<%@ Control Language="C#" AutoEventWireup="true" Codebehind="QueryBuilder.ascx.cs"
    Inherits="Shell.SharePoint.DREAM.Site.UI.UserControls.QueryBuilder" %>
<%@ Register Assembly="Telerik.Web.UI, Version=2010.1.415.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4"
    Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:UpdatePanel ID="updatePanelQuerySearch" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <contenttemplate>
<asp:Panel ID="querySearchPanel" DefaultButton="btnSearch" runat="Server" Width="100%"
    Height="100%">
    <table class="tableAdvSrchBorder" cellspacing="0" cellpadding="0" border="0" width="100%">
        <tr>
            <td class="tdBasinAdvSrchHeader" style="font-weight: bold;" colspan="4">
                &nbsp;Query Search
            </td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="ExceptionPanel" Visible="false" runat="server">
        <asp:Label ID="lblException" runat="server" Text="" CssClass="labelMessage"></asp:Label>
    </asp:Panel>
    <br />
    <table class="tableAdvSrchBorder" cellspacing="0" cellpadding="0" border="0" width="100%">
        <tr>
            <td width="20%" height="25px" colspan="1" class="tdAdvSrchItemNbrdr">
                &nbsp;Saved Search</td>
            <td width="80%" class="SaveSearchColumn" colspan="3">
                <asp:DropDownList ID="cboSavedSearch"
                    CssClass="dropdownAdvSrch" runat="server" Width="185px" AutoPostBack="True" OnSelectedIndexChanged="CboSavedSearch_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:Image ID="imgSavedSearch" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif"
                    ToolTip="Saved Search" /></td>
        </tr>
    </table>
    <br />
    <table border="0" cellpadding="4" cellspacing="0" class="tableAdvSrchBorder" style="width:100%;
        height:100%">
        <tr>
            <td class="tdAdvSrchSubHeader" colspan="4" height="15px">
                <b>Search</b>&nbsp;</td>
        </tr>
        <tr style="height:100%">
            <td valign="top" colspan="1">
                <div id="divRadTreeView" style="vertical-align: top; height: 400px; overflow: auto;">
                    <telerik:RadTreeView ID="querySearchTree" runat="server" Height="100%" Width="100%"
                        MaxDataBindDepth="4" OnNodeClick="QuerySearchTree_SelectedNodeChanged" OnClientNodeClicking="OnQueryBuilderClientNodeClicking"   />
                </div>
            </td>
            <td valign="top" width="70%" style="border-left: lightsteelblue 1px solid; height: 297px;"
                colspan="3">
                <asp:Panel ID="columnSearchPanel" Visible="false" runat="server">
                    <table cellspacing="0" cellpadding="0" border="0" width="100%">
                        <tr align="right" valign="top">
                            <td>
                                <asp:ImageButton ID="editButton" OnClientClick="return ValidateCriteria();" runat="server"
                                    ImageUrl="/_layouts/DREAM/images/edit_sql.gif" OnClick="Edit_Click" />
                                <asp:ImageButton ID="viewButton" OnClientClick="return ValidateCriteria();" runat="server"
                                    ToolTip="View SQL" ImageUrl="/_layouts/DREAM/images/view_sql.gif" OnClick="ViewButton_Click" />
                                <asp:ImageButton ID="saveButton" OnClientClick="return ValidateQuerySaveSearchCriteria();"
                                    runat="server" ImageUrl="/_layouts/DREAM/images/save_search.gif" OnClick="SaveButton_Click"
                                    ToolTip="Save Search" />
                                <asp:ImageButton ID="ClearButton" runat="server" ImageUrl="/_layouts/DREAM/images/reset.gif"
                                    OnClick="ClearButton_Click" ToolTip="Reset" />
                                <asp:ImageButton ID="RunButton" OnClientClick="return ValidateQuerySearchCriteria();"
                                    runat="server" ImageUrl="/_layouts/DREAM/images/go.gif" OnClick="RunButton_Click"
                                    ToolTip="Go" />
                            </td>
                        </tr>
                        <tr>
                            <td height="10px">
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div>
                                    <table align="center" border="0" cellspacing="0" style="border-collapse: collapse;"
                                        width="100%">
                                        <tr height="20px" bgcolor="#BDBDBD" style="border-left: 1px solid red; border-right: 1px solid red;">
                                            <td style="height: 20px; width: 2%" align="left" class="BrderPropertyMain">
                                                <asp:CheckBox runat="server" Checked="true" ID="chbHeaderColumn" onclick="Javascript:CheckUnCheckAllColumns('tblColumnNames',this);" />
                                            </td>
                                            <td style="height: 20px; width: 54%;" align="left" class="BrderProperty">
                                                <font color="black" face="Verdana"><b>Column Name</b></font>
                                            </td>
                                            <td style="height: 20px; width: 15.5%;" align="left" class="BrderProperty">
                                                <font color="black" face="Verdana"><b>Operator</b></font>
                                            </td>
                                            <td style="height: 20px; width: 28.5%;" class="BrderProperty" align="left">
                                                <font color="black" face="Verdana"><b>Criteria</b></font>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="vertical-align: top; height: 300px; overflow: auto;">
                                    <asp:GridView ID="tblColumnNames" AutoGenerateColumns="false" runat="server" ShowHeader="false">
                                        <SelectedRowStyle BackColor="#388AD0" Font-Bold="True" ForeColor="White" Width="100%" />
                                        <AlternatingRowStyle BackColor="#EFEFEF" />
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="2%">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chbColumns" Checked="true" runat="server" onclick="Javascript:UnCheckHeader('tblColumnNames',this);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="columnName_Text" HeaderText="Column Name" ItemStyle-Width="56%" />
                                            <asp:TemplateField HeaderText="Operator" ItemStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="cboOperator" EnableViewState="true" CssClass="dropdownAdvSrch"
                                                        runat="server" onchange="javascript:EnableCriteria(this);">
                                                        <asp:ListItem Text="--Select--"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Criteria" ItemStyle-Width="26.5%">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtCriteria" EnableViewState="true" runat="server"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td height="20px">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtSQLQuery" CssClass="queryfieldmini" runat="server" Height="100px"
                                    ReadOnly="true" TextMode="MultiLine" Width="100%">&lt;&lt;View SQL Query over here&gt;&gt;</asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td height="20px">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%">
                                    <tr width="100%">
                                        <td style="text-align: left;">
                                            Search Name &nbsp; &nbsp;<asp:TextBox ID="txtSaveSearchName" CssClass="queryfieldmini"
                                                Width="120px" runat="server"></asp:TextBox>&nbsp;
                                            <asp:CheckBox ID="chbShared" Text="Share" runat="server" />
                                        </td>
                                        <td style="text-align: right;">
                                            <asp:Button ID="btnSaveSearch" OnClientClick="return ValidateQuerySaveSearchCriteria()"
                                                CssClass="buttonAdvSrch" runat="server" Text="Save Search" OnClick="BtnSaveSearch_Click" />
                                            <asp:Button ID="btnSearch" OnClientClick="return ValidateQuerySearchCriteria()" CssClass="buttonAdvSrch"
                                                runat="server" Text="Search" OnClick="BtnSearch_Click" />
                                            <asp:Button ID="btnReset" CssClass="buttonAdvSrch" runat="server" Text="Reset" OnClick="BtnReset_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
   <asp:XmlDataSource ID="XmlDataSource1" runat="server"></asp:XmlDataSource>
</asp:Panel>
</contenttemplate>
</asp:UpdatePanel>
 
<script type="text/JavaScript">
   setWindowTitle('Query Search');
   ApplyStyleQrySrchCombo();
   EnableQueryCriteria();  
</script>

