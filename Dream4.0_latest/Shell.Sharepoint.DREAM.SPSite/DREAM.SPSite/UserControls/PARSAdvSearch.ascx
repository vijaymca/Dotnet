<%@ Control Language="C#" AutoEventWireup="true" Codebehind="PARSAdvSearch.ascx.cs"
    Inherits="Shell.SharePoint.DREAM.Site.UI.UserControls.PARSAdvSearch" %>
<div id="AdvancedSearchContainer">
    <asp:Panel ID="AdvancedSearchContent" DefaultButton="cmdSearch" runat="server" Width="100%">
        <table class="tableAdvSrchBorder" cellspacing="0" cellpadding="4" border="0" width="100%">
            <tr>
                <td class="tdAdvSrchHeader" colspan="4">
                    <b>Advanced Search - Project Archives</b>
                </td>
            </tr>
        </table>
        <asp:Panel ID="ExceptionBlock" Visible="false" runat="server">
            <br />
            <asp:Label ID="lblException" runat="server" Text="" Visible="false" CssClass="labelMessage"></asp:Label>
        </asp:Panel>
        <br />
        <table class="tableAdvSrchBorder" cellspacing="0" cellpadding="4" border="0" width="100%">
            <tr>
                <td width="20%" class="tdAdvSrchItemNbrdr ">
                    Saved Search</td>
                <td width="30%" class="tdAdvSrchItemNbrdr ">
                    <asp:DropDownList ID="cboSavedSearch" runat="server" CssClass="dropdownAdvSrch" Width="185px"
                        OnSelectedIndexChanged="cboSavedSearch_SelectedIndexChanged" AutoPostBack="True">
                        <asp:ListItem>---Select---</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Image ID="imgSavedSearch" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
                <td align="left" colspan="2" width="50%">
                    &nbsp;<asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonAdvSrch"
                        OnClientClick="if(!ValidatePARS())return false;" OnClick="cmdSearch_Click" />
                    <input type="button" id="btnReset" runat="server" value="Reset" class="buttonAdvSrch"
                        onserverclick="cmdReset_Click" onclick="EnableButton();if(!ValidateAdvSearchReset())return false;" /></td>
            </tr>
        </table>
        <br />
        <table class="tableAdvSrchBorder" cellspacing="0" cellpadding="4" border="0" width="100%">
            <tr>
                <td colspan="5" class="tdAdvSrchSubHeader">
                    <b>Search By File</b>[<a href="javascript:ResetFileSearchCriteria()" class="LinkTxt">Reset File Search Criteria</a>]</td>
            </tr>
            <tr>
                <td valign="top" class="tdAdvSrchItem" style="width: 20%">
                    Search By</td>
                <td colspan="4" class="tdAdvSrchItem">
                    <asp:DropDownList ID="cboSearchCriteria" runat="server" CssClass="dropdownAdvSrch"
                        Width="185px">
                        <asp:ListItem>---Select---</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td valign="top" class="tdAdvSrchItem" style="width: 20%">
                    Select File to search</td>
                <td colspan="4" class="tdAdvSrchItem">
                    <input type="file" id="fileUploader" runat="server" class="button" contenteditable="false" /></td>
            </tr>
            <tr>
                <td colspan="5" class="tdAdvSrchSubHeader">
                    <b>Search</b></td>
            </tr>
            <tr>
                <td class="tdAdvSrchItem" width="178px">
                    <span>Title</span></td>
                <td class="tdAdvSrchItem">
                    <asp:TextBox ID="txtXMTITLE" runat="server" Width="180px" CssClass="queryfieldmini"></asp:TextBox>
                    <asp:Image ID="imgTitle" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
                <td class="tdAdvSrchItem" colspan="2" width="50%">
                    [Wildcard = * OR %]</td>
            </tr>
            <tr>
                <td class="tdAdvSrchItem" width="178px" rowspan="">
                    Description</td>
                <td class="tdAdvSrchItem" width="30%">
                    <asp:TextBox ID="txtXMDESCRIPTION" runat="server" Width="180px" CssClass="queryfieldmini"></asp:TextBox>
                    <asp:Image ID="imgDescription" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
                <td class="tdAdvSrchItem" width="20%">
                    Project Name</td>
                <td class="tdAdvSrchItem" width="30%">
                    <asp:TextBox ID="txtXMPROJECTNAME" runat="server" Width="180px" CssClass="queryfieldmini"></asp:TextBox>
                    <asp:Image ID="imgProjectName" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
            </tr>
            <tr>
                <td class="tdAdvSrchItem" width="178px">
                    Milestone</td>
                <td class="tdAdvSrchItem">
                    <asp:TextBox ID="txtXMMILESTONE" runat="server" Width="180px" CssClass="queryfieldmini"></asp:TextBox>
                    <asp:Image ID="imgMilestone" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
                <td class="tdAdvSrchItem" width="20%">
                    Basin Identifier</td>
                <td class="tdAdvSrchItem" width="30%">
                    <asp:TextBox ID="txtXMBASIN" runat="server" Width="180px" CssClass="queryfieldmini"></asp:TextBox>
                    <asp:Image ID="imgBasin" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
            </tr>
            <tr>
                <td class="tdAdvSrchItem" width="178px">
                    Field</td>
                <td class="tdAdvSrchItem">
                    <asp:TextBox ID="txtXMFIELD" runat="server" Width="180px" CssClass="queryfieldmini"></asp:TextBox>
                    <asp:Image ID="imgField" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
                <td class="tdAdvSrchItem" width="20%">
                    Asset</td>
                <td class="tdAdvSrchItem" width="30%">
                    <asp:TextBox ID="txtXMASSET" runat="server" Width="180px" CssClass="queryfieldmini"></asp:TextBox>
                    <asp:Image ID="imgAsset" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
            </tr>
            <tr>
                <td class="tdAdvSrchItem" width="178px">
                    Country</td>
                <td class="tdAdvSrchItem">
                    <asp:TextBox ID="txtXMCOUNTRY" runat="server" Width="180px" CssClass="queryfieldmini"></asp:TextBox>
                    <asp:Image ID="imgCountry" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
                <td class="tdAdvSrchItem" width="20%">
                    Data Owner</td>
                <td class="tdAdvSrchItem" width="30%">
                    <asp:TextBox ID="txtXMOWNER" runat="server" Width="180px" CssClass="queryfieldmini"></asp:TextBox>
                    <asp:Image ID="imgDataOwner" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
            </tr>
            <tr>
                <td class="tdAdvSrchItem" width="178px">
                    Owner Organisation</td>
                <td class="tdAdvSrchItem">
                    <asp:TextBox ID="txtXMOWNERORGANISATION" runat="server" Width="180px" CssClass="queryfieldmini"></asp:TextBox>
                    <asp:Image ID="imgOwnerOrganisation" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
                <td class="tdAdvSrchItem" width="20%">
                    &nbsp;</td>
                <td class="tdAdvSrchItem" width="30%">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="tdAdvSrchSubHeader" colspan="5">
                    <b>Date Archived</b>[<a href="javascript:resetPARSDateTable()" class="LinkTxt">Reset
                        Date Criteria</a>]
                </td>
            </tr>
            <tr>
                <td class="tdAdvSrchItem" width="178px" height="10px">
                    &nbsp;</td>
                <td class="tdAdvSrchItem" width="80%" height="10px" colspan="3">
                    <asp:RadioButton ID="rbLastWeek" runat="server" GroupName="DateArchived" Text="Last Week"
                        onclick="javascript:EnableDisableDates(this);" />
                    <asp:RadioButton ID="rbLastMonth" runat="server" GroupName="DateArchived" Text="Last Month"
                        onclick="javascript:EnableDisableDates(this);" />
                    <asp:RadioButton ID="rbLastYear" runat="server" GroupName="DateArchived" Text="Last Year"
                        onclick="javascript:EnableDisableDates(this);" />
                    <asp:RadioButton ID="rbSelectDates" runat="server" GroupName="DateArchived" Text="Select Dates"
                        onclick="javascript:EnableDisableDates(this);" /></td>
            </tr>
            <tr id="trDates" runat="server" visible="true" style="display: none">
                <td class="tdAdvSrchItem" height="10px" width="178px">
                    Start Date</td>
                <td id="Td1" class="tdAdvSrchItem" height="10px" runat="server" width="30%">
                    &nbsp;<asp:TextBox ID="txtStartDate" runat="server" CssClass="queryfieldmini" Width="180px"></asp:TextBox>
                    <img src="/_layouts/images/calendar.gif" align="Middle" onclick="showCalendarControl('txtStartDate');"
                        style="cursor: hand;" />
                </td>
                <td class="tdAdvSrchItem" height="10px" width="20%">
                    End Date</td>
                <td id="Td2" class="tdAdvSrchItem" height="10px" runat="server" width="30%">
                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="queryfieldmini" Width="180px"></asp:TextBox>
                    <img src="/_layouts/images/calendar.gif" align="Middle" onclick="showCalendarControl('txtEndDate');"
                        style="cursor: hand;" />
                </td>
            </tr>
            <tr>
                <td colspan="5" class="tdAdvSrchSubHeader" align="left">
                    <asp:CheckBox TextAlign="Left" ToolTip="Reset Geographical fields" ID="chbGeographicalSearch"
                        runat="server" Height="18px" OnClick="javascript:showPARSLatLongTable('TR1','TR2','TR3','TR4','TR5','TR6');SetGeographicalDefaultValues(this)" /><b>Outline
                            of archive bounding box (WGS84)</b></td>
            </tr>
            <tr id="TR1" style="display: none;">
                <td class="tdPARSAdvSrchSubHeader" colspan="5">
                    Min</td>
            </tr>
            <tr id="TR2" style="display: none;">
                <td class="tdAdvSrchItem" width="20%">
                    Latitude</td>
                <td class="tdAdvSrchItem" colspan="4">
                    <asp:TextBox ID="txtMinLatDeg" runat="server" CssClass="queryfieldmini" Width="50px" onblur="javascript:SetTextBoxValue(this,'0');"></asp:TextBox>
                    deg &nbsp;<asp:TextBox ID="txtMinLatMin" runat="server" CssClass="queryfieldmini"
                        Width="50px" onblur="javascript:SetTextBoxValue(this,'0');"></asp:TextBox>
                    min &nbsp;
                    <asp:TextBox ID="txtMinLatSec" runat="server" CssClass="queryfieldmini" Width="50px" onblur="javascript:SetTextBoxValue(this,'0');"></asp:TextBox>
                    sec&nbsp;
                    <asp:TextBox ID="txtMinLatNS" runat="server" MaxLength="1" Width="50px" CssClass="queryfieldmini"></asp:TextBox>
                    N/S&nbsp;<br />
                </td>
            </tr>
            <tr id="TR3" style="display: none;">
                <td class="tdAdvSrchItem" width="20%">
                    Longitude</td>
                <td class="tdAdvSrchItem" colspan="4">
                    <asp:TextBox ID="txtMinLonDeg" runat="server" CssClass="queryfieldmini" Width="50px" onblur="javascript:SetTextBoxValue(this,'0');"></asp:TextBox>
                    deg &nbsp;<asp:TextBox ID="txtMinLonMin" runat="server" CssClass="queryfieldmini"
                        Width="50px"  onblur="javascript:SetTextBoxValue(this,'0');"></asp:TextBox>
                    min &nbsp;
                    <asp:TextBox ID="txtMinLonSec" runat="server" CssClass="queryfieldmini" Width="50px" onblur="javascript:SetTextBoxValue(this,'0');"></asp:TextBox>
                    sec&nbsp;
                    <asp:TextBox ID="txtMinLonEW" runat="server" MaxLength="1" Width="50px" CssClass="queryfieldmini"></asp:TextBox>
                    E/W&nbsp;<br />
                </td>
            </tr>
            <tr id="TR4" style="display: none;">
                <td class="tdPARSAdvSrchSubHeader" colspan="5">
                    Max</td>
            </tr>
            <tr id="TR5" style="display: none;">
                <td class="tdAdvSrchItem" width="20%">
                    Latitude</td>
                <td class="tdAdvSrchItem" colspan="4">
                    <asp:TextBox ID="txtMaxLatDeg" runat="server" CssClass="queryfieldmini" Width="50px" onblur="javascript:SetTextBoxValue(this,'0');"></asp:TextBox>
                    deg&nbsp;
                    <asp:TextBox ID="txtMaxLatMin" runat="server" CssClass="queryfieldmini" Width="50px" onblur="javascript:SetTextBoxValue(this,'0');"></asp:TextBox>
                    min&nbsp;
                    <asp:TextBox ID="txtMaxLatSec" runat="server" CssClass="queryfieldmini" Width="50px" onblur="javascript:SetTextBoxValue(this,'0');"></asp:TextBox>
                    sec&nbsp;&nbsp;
                    <asp:TextBox ID="txtMaxLatNS" runat="server" Width="50px" MaxLength="1" CssClass="queryfieldmini"></asp:TextBox>
                    N/S&nbsp;<br />
                </td>
            </tr>
            <tr id="TR6" style="display: none;">
                <td class="tdAdvSrchItem" width="20%">
                    Longitude</td>
                <td class="tdAdvSrchItem" colspan="4">
                    <asp:TextBox ID="txtMaxLonDeg" runat="server" CssClass="queryfieldmini" Width="50px" onblur="javascript:SetTextBoxValue(this,'0');"></asp:TextBox>
                    deg&nbsp;
                    <asp:TextBox ID="txtMaxLonMin" runat="server" CssClass="queryfieldmini" Width="50px" onblur="javascript:SetTextBoxValue(this,'0');"></asp:TextBox>
                    min&nbsp;
                    <asp:TextBox ID="txtMaxLonSec" runat="server" CssClass="queryfieldmini" Width="50px" onblur="javascript:SetTextBoxValue(this,'0');"></asp:TextBox>
                    sec &nbsp;
                    <asp:TextBox ID="txtMaxLonEW" runat="server" MaxLength="1" Width="50px" CssClass="queryfieldmini"></asp:TextBox>
                    E/W&nbsp;<br />
                </td>
            </tr>
            <tr>
                <td colspan="5" height="2px">
                </td>
            </tr>
            <tr>
                <td colspan="5" class="tdAdvSrchSubHeader">
                    <b>Save Search Criteria</b>
                </td>
            </tr>
            <tr>
                <td width="20%" class="tdAdvSrchItemNbrdr">
                    Search Name</td>
                <td width="30%" class="tdAdvSrchItemNbrdr">
                    <asp:TextBox ID="txtSaveSearch" runat="server" Width="180px" CssClass="queryfieldmini"></asp:TextBox>
                    <asp:Image ID="imgSearchName" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" />
                </td>
                <td height="25px" width="10%" class="tdAdvSrchItemNbrdr">
                    <asp:CheckBox Height="18px" ToolTip="Type of Save Search(Personal/Shared)" Text="Shared"
                        ID="chbShared" runat="server" />
                </td>
                <td align="left" colspan="2" class="tdAdvSrchItemNbrdr" width="50%">
                    <input type="button" id="cmdSaveSearch" runat="server" value="Save Search" class="buttonAdvSrch"
                        onserverclick="cmdSaveSearch_Click" onclick="if(!ValidateSaveSrchPARS())return false;" />
                    <asp:Button ID="cmdSearch" runat="server" Text="Search" CssClass="buttonAdvSrch"
                        OnClientClick="if(!ValidatePARS(false))return false;" OnClick="cmdSearch_Click" />
                    <input type="button" id="cmdReset" runat="server" value="Reset" class="buttonAdvSrch"
                        onserverclick="cmdReset_Click" onclick="EnableButton();if(!ValidateAdvSearchReset())return false;" /></td>
            </tr>
        </table>
    </asp:Panel>
</div>

<script type="text/JavaScript">
    setWindowTitle('Advanced Search - Project Archives');
</script>

<input type="hidden" id="hidWordContent" runat="server" />