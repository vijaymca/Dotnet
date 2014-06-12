<%@ Control Language="C#" AutoEventWireup="true" Codebehind="ReservoirAdvSearch.ascx.cs"
    Inherits="Shell.SharePoint.DREAM.Site.UI.UserControls.ReservoirAdvSearch" %>
<%@ Register Assembly="Telerik.Web.UI, Version=2010.1.415.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4"
    Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<script type="text/JavaScript" language="javascript" src="/_Layouts/DREAM/Javascript/SRPJavaScriptFunctionsRel3_0.js"></script>

<div id="AdvancedSearchContainer">
    <asp:Panel ID="AdvancedSearchContent" DefaultButton="cmdSearch" runat="server" Width="100%">
        <table class="tableAdvSrchBorder" cellspacing="0" cellpadding="4" border="0" width="100%">
            <tr>
                <td class="tdAdvSrchHeader" colspan="4">
                    <b>Advanced Search - Reservoir </b>
                </td>
            </tr>
        </table>
        <asp:Label ID="lblException" runat="server" Text="" Visible="false" CssClass="labelMessage"></asp:Label>
        <table class="tableAdvSrchBorder" cellspacing="0" cellpadding="4" border="0" width="100%">
            <tr>
                <td width="20%">
                    Search Condition</td>
                <td width="30%">
                    <asp:RadioButtonList Width="150px" ID="rdblSearchCond" runat="server" RepeatDirection="Horizontal"
                        CssClass="queryfieldmini">
                        <asp:ListItem Selected="True">AND</asp:ListItem>
                        <asp:ListItem>OR</asp:ListItem>
                    </asp:RadioButtonList></td>
                <td align="left" colspan="2" width="50%">
                </td>
            </tr>
            <tr>
                <td width="20%">
                    Saved Search
                </td>
                <td width="30%">
                    <asp:DropDownList ID="cboSavedSearch" runat="server" CssClass="dropdownAdvSrch" Width="152px"
                        OnSelectedIndexChanged="cboSavedSearch_SelectedIndexChanged" AutoPostBack="True">
                        <asp:ListItem>---Select---</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Image ID="imgSavedSearch" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
                <td align="left" width="50%">
                    &nbsp;<asp:Button ID="cmdSearchButton" runat="server" Text="Search" CssClass="buttonAdvSrch"
                        OnClientClick="if(!ValidateField())return false;" OnClick="cmdSearch_Click" />
                    <input type="button" id="cmdResetButton" runat="server" value="Reset" class="buttonAdvSrch"
                        onserverclick="cmdReset_Click" onclick="EnableButton();" /></td>
            </tr>
        </table>
        <br />
        <table class="tableAdvSrchBorder" cellspacing="0" cellpadding="4" border="0" width="100%">
            <tr>
                <td class="fieldSearchHeader" width="100%" colspan="3">
                    <b>Search By File</b>[<a href="javascript:ResetFileSearchCriteria()" class="LinkTxt">Reset File Search Criteria</a>]
                </td>
            </tr>
            <tr>
                <td width="20%">
                    Search By
                </td>
                <td width="30%">
                    <asp:DropDownList ID="cboSearchCriteria" runat="server" Width="152px" CssClass="dropdownAdvSrch">
                        <asp:ListItem>---Select---</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td align="left" width="50%">
                </td>
            </tr>
            <tr>
                <td width="20%">
                    Select File to search
                </td>
                <td width="30%">
                    <input id="fileUploader" class="button" contenteditable="false" type="file" runat="server"
                        style="background-color: Gray" />
                </td>
                <td align="left" width="50%">
                </td>
            </tr>
            <tr>
                <td class="fieldSearchHeader" colspan="3">
                    <b>Search </b>
                </td>
            </tr>
            <tr>
                <td style="width: 20%">
                    Identifier
                </td>
                <td style="width: 30%">
                    <asp:TextBox ID="txtReservoir_Identifier" runat="server" Width="150px" CssClass="queryfieldmini"></asp:TextBox>
                    <asp:Image ID="imgIdentifier" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" />
                </td>
                <td align="left" width="50%">
                    [Wildcard = * OR %]
                </td>
            </tr>
        </table>
        <table class="tableAdvSrchBorder" cellspacing="0" cellpadding="0" border="0" width="100%">
            <tr>
                <td class="fieldSearchHeader">
                    <input type="CHECKBOX" name="chkGeology" onclick="TableToggler(this,'tblGeology');"
                        checked="checked">
                    <b>Geology </b>
                </td>
                <td class="fieldSearchHeader">
                    <input type="CHECKBOX" name="chkPetrophysics" onclick="TableToggler(this,'tblPetrophysics');"
                        checked="checked">
                    <b>Petrophysics</b></td>
            </tr>
            <tr>
                <td width="50%" valign="top">
                    <table id="tblGeology" cellspacing="0" cellpadding="4" border="0" width="100%">
                        <tr>
                            <td width="40%">
                                <input type="button" id="cmdChronostraticName" runat="server" value="Chronostratigraphic Name"
                                    class="buttonAdvSrch" style="width: 155px" />
                            </td>
                            <td width="40%">
                                <asp:TextBox ID="txtChronostratigraphy" runat="server" Width="150px" CssClass="queryfieldmini"></asp:TextBox>
                            </td>
                            <td width="20%">
                                <input type="button" id="cmdChronostraticClear" runat="server" value="Clear" class="buttonAdvSrch"
                                    style="width: 66px" />
                            </td>
                        </tr>
                        <tr>
                            <td width="40%">
                                <input type="button" id="cmdDepositionalEnv" runat="server" value="Depositional Environment"
                                    class="buttonAdvSrch" style="width: 155px" />
                            </td>
                            <td width="40%">
                                <asp:TextBox ID="txtDepositionalEnv" runat="server" Width="150px" CssClass="queryfieldmini"></asp:TextBox>
                            </td>
                            <td width="20%">
                                <input type="button" id="cmdDepositionalEnvClear" runat="server" value="Clear" class="buttonAdvSrch"
                                    style="width: 66px" />
                            </td>
                        </tr>
                        <tr>
                            <td width="40%">
                                Grain Size Mean</td>
                            <td colspan="2" width="60%">
                                <asp:DropDownList ID="cboGrainSizeMean" runat="server" CssClass="dropdownAdvSrch"
                                    Width="152px">
                                    <asp:ListItem>---Select---</asp:ListItem>
                                </asp:DropDownList>
                                <asp:Image ID="imgGrainSizeMean" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
                        </tr>
                        <tr>
                            <td width="40%">
                                Lithology (main)</td>
                            <td colspan="2" width="60%">
                                <telerik:RadComboBox ID="radCboLithologyMain" runat="server" Width="152px" EnableVirtualScrolling="true"
                                    EnableItemCaching="True" OnClientSelectedIndexChanging="LoadLithologySecondary"
                                    OnItemsRequested="cmdRadCboLithologyMainComboBox_ItemsRequested" CssClass="AdvSearchRadcombo"
                                    Font-Names="Verdana" Font-Size="11px" ForeColor="#000" Skin="Sitefinity">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="---Select---" CssClass="AdvSearchRadcombo" />
                                    </Items>
                                </telerik:RadComboBox>
                                &nbsp;<asp:Image ID="imgLithologyMain" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
                        </tr>
                        <tr>
                            <td width="40%">
                                Lithology (secondary)</td>
                            <td colspan="2" width="60%">
                                <telerik:RadComboBox ID="radCboLithologySecondary" runat="server" Width="152px" EnableVirtualScrolling="true"
                                    EnableItemCaching="True" OnClientItemsRequested="ItemsLoaded" OnItemsRequested="cmdRadLithologySecondaryComboBox_ItemsRequested"
                                    CssClass="AdvSearchRadcombo" Font-Names="Verdana" Font-Size="11px" ForeColor="#000"
                                    OnClientSelectedIndexChanged="OnClientSelectedIndexChanged" Skin="Sitefinity">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="---Select---" CssClass="AdvSearchRadcombo" />
                                    </Items>
                                </telerik:RadComboBox>
                                &nbsp;<asp:Image ID="imgLithologySecondary" runat="server" ImageAlign="AbsMiddle"
                                    ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
                        </tr>
                        <tr>
                            <td width="40%">
                                Lithostrat Group</td>
                            <td colspan="2" width="60%">
                                <telerik:RadComboBox ID="radCboLithostratGroup" runat="server" Width="152px" EnableVirtualScrolling="true"
                                    EnableItemCaching="True" OnClientSelectedIndexChanging="LoadLithostratFormation"
                                    OnItemsRequested="cmdRadLithostratGroupComboBox_ItemsRequested" CssClass="AdvSearchRadcombo"
                                    Font-Names="Verdana" Font-Size="11px" ForeColor="#000" Skin="Sitefinity" ShowMoreResultsBox="true">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="---Select---" CssClass="AdvSearchRadcombo" />
                                    </Items>
                                </telerik:RadComboBox>
                                &nbsp;<asp:Image ID="imgLithostratGroup" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" />
                            </td>
                        </tr>
                        <tr>
                            <td width="40%">
                                Lithostrat Formation</td>
                            <td colspan="2" width="60%">
                                <telerik:RadComboBox ID="radCboLithostratFormation" runat="server" Width="152px"
                                    ShowMoreResultsBox="true" EnableVirtualScrolling="true" EnableItemCaching="True"
                                    OnClientSelectedIndexChanging="LoadLithostratMember" OnItemsRequested="cmdRadLithostratFormationComboBox_ItemsRequested"
                                    CssClass="AdvSearchRadcombo" Font-Names="Verdana" Font-Size="11px" ForeColor="#000"
                                    OnClientItemsRequesting="OnClientItemRequesting" OnClientItemsRequested="ItemsLoaded"
                                    EmptyMessage="---Select---" Skin="Sitefinity">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="---Select---" CssClass="AdvSearchRadcombo" />
                                    </Items>
                                </telerik:RadComboBox>
                                &nbsp;<asp:Image ID="imgLithostratFormation" runat="server" ImageAlign="AbsMiddle"
                                    ImageUrl="/_layouts/DREAM/images/icon_help.gif" />&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td width="40%">
                                Lithostrat Member</td>
                            <td colspan="2" width="60%">
                                <telerik:RadComboBox ID="radCboLithostratMember" runat="server" Width="152px" ShowMoreResultsBox="true"
                                    EnableVirtualScrolling="true" EnableItemCaching="True" OnItemsRequested="cmdRadLithostratMemberComboBox_ItemsRequested"
                                    CssClass="AdvSearchRadcombo" Font-Names="Verdana" Font-Size="11px" ForeColor="#000"
                                    OnClientItemsRequesting="OnClientItemRequesting" OnClientItemsRequested="ItemsLoaded"
                                    EmptyMessage="---Select---" Skin="Sitefinity">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="---Select---" CssClass="AdvSearchRadcombo" />
                                    </Items>
                                </telerik:RadComboBox>
                                &nbsp;<asp:Image ID="imgLithostratMember" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
                        </tr>
                    </table>
                </td>
                <td width="50%" valign="top">
                    <table id="tblPetrophysics" cellspacing="0" cellpadding="4" border="0" width="100%">
                        <tr>
                            <td style="width: 40%;">
                                Permeability*
                            </td>
                            <td>
                                Min
                            </td>
                            <td>
                                <asp:TextBox ID="txtPermeability_Min" runat="server" CssClass="queryfieldmini" Width="60px"></asp:TextBox>
                            </td>
                            <td>
                                Max
                            </td>
                            <td nowrap="nowrap">
                                <asp:TextBox ID="txtPermeability_Max" runat="server" CssClass="queryfieldmini" Width="60px"></asp:TextBox>
                                md
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 40%;">
                                Porosity*
                            </td>
                            <td>
                                Min
                            </td>
                            <td>
                                <asp:TextBox ID="txtPorosityMin" runat="server" CssClass="queryfieldmini" Width="60px"></asp:TextBox>
                            </td>
                            <td>
                                Max
                            </td>
                            <td nowrap="nowrap">
                                <asp:TextBox ID="txtPorosityMax" runat="server" CssClass="queryfieldmini" Width="60px"></asp:TextBox>
                                %
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 40%; white-space: nowrap;">
                                Sand Reservoir<br />
                                Net/Gross(Avg)*</td>
                            <td align="left" colspan="3">
                                <asp:TextBox ID="txtSandReservoir_Net_Gross_avg" runat="server" CssClass="queryfieldmini"
                                    Width="130px"></asp:TextBox>
                            </td>
                            <td nowrap="nowrap">
                                Ratio (Decimal)</td>
                        </tr>
                        <tr>
                            <td style="width: 40%; white-space: nowrap;">
                                Sand Reservoir
                                <br />
                                Net/Gross(Max)*</td>
                            <td align="left" colspan="3">
                                <asp:TextBox ID="txtSandReservoir_Net_Gross_max" runat="server" CssClass="queryfieldmini"
                                    Width="130px" CausesValidation="True"></asp:TextBox>
                            </td>
                            <td nowrap="nowrap">
                                Ratio (Decimal)
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 40%; white-space: nowrap;">
                                Sand Reservoir
                                <br />
                                Net/Gross(Min)*</td>
                            <td align="left" colspan="3">
                                <asp:TextBox ID="txtSandReservoir_Net_Gross_min" runat="server" CssClass="queryfieldmini"
                                    Width="130px"></asp:TextBox>
                            </td>
                            <td nowrap="nowrap">
                                Ratio (Decimal)
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 40%; white-space: nowrap;">
                                Water Saturation(Max)*</td>
                            <td align="left" colspan="3">
                                <asp:TextBox ID="txtWaterSaturation_max" runat="server" CssClass="queryfieldmini"
                                    Width="130px"></asp:TextBox>
                            </td>
                            <td nowrap="nowrap">
                                Ratio(Decimal)
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <b>* Search will be Performed for +/-50% of entered values</b></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="fieldSearchHeader">
                    <input type="CHECKBOX" name="chkProduction" onclick="TableToggler(this,'tblProduction');"
                        checked="checked">
                    <b>Production</b>
                </td>
                <td class="fieldSearchHeader">
                    <input type="CHECKBOX" name="Reservoir" onclick="TableToggler(this,'tblReservoirEngineering');"
                        checked="checked">
                    <b>Reservoir Engineering</b>
                </td>
            </tr>
            <tr width="100%">
                <td width="50%" valign="top">
                    <table id="tblProduction" cellspacing="0" cellpadding="4" border="0" width="100%">
                        <tr>
                            <td width="40%" style="white-space: nowrap;">
                                Gas Initially
                                <br />
                                In Place (GIIP)*</td>
                            <td colspan="2" width="60%">
                                <asp:TextBox ID="txtGasInitiallyInPlace" runat="server" Width="150px" CssClass="queryfieldmini"
                                    EnableTheming="False"></asp:TextBox>
                                <asp:Image ID="imgGasInitiallyInPlace" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
                        </tr>
                        <tr>
                            <td>
                                Stock Tank Oil Initially
                                <br />
                                In Place (STOIIP)*</td>
                            <td colspan="2">
                                <asp:TextBox ID="txtStockTankOilInitially" runat="server" CssClass="queryfieldmini"
                                    Width="150px"></asp:TextBox>
                                <asp:Image ID="imgStockTankOilInitially" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
                        </tr>
                        <tr>
                            <td>
                                Production Status</td>
                            <td colspan="2">
                                <telerik:RadComboBox ID="radCboProductionStatus" runat="server" CssClass="AdvSearchRadcombo"
                                    Width="152px" DropDownWidth="152px" OnClientSelectedIndexChanged="OnProductionStatusSelectedIndexChanged"
                                    Font-Names="Verdana" Font-Size="11px" ForeColor="#000" Skin="Sitefinity">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="---Select---" ToolTip="---Select---" />
                                    </Items>
                                </telerik:RadComboBox>
                                &nbsp;<asp:Image ID="imgProductionStatus" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
                        </tr>
                        <tr>
                            <td>
                                Recovery Factor Oil*</td>
                            <td colspan="2">
                                <asp:TextBox ID="txtRecoveryFactorOil" runat="server" CssClass="queryfieldmini" Width="150px"></asp:TextBox>&nbsp;
                                %</td>
                        </tr>
                        <tr>
                            <td>
                                Recovery Factor Gas*</td>
                            <td colspan="2">
                                <asp:TextBox ID="txtRecoveryFactorGas" runat="server" CssClass="queryfieldmini" Width="150px"></asp:TextBox>&nbsp;
                                %</td>
                        </tr>
                        <tr>
                            <td>
                                Recoverable Oil*</td>
                            <td colspan="2">
                                <asp:TextBox ID="txtRecoverableOil" runat="server" CssClass="queryfieldmini" Width="150px"></asp:TextBox>
                                Mmbbl</td>
                        </tr>
                        <tr>
                            <td width="40%">
                                Recoverable Gas*</td>
                            <td colspan="2">
                                <asp:TextBox ID="txtRecoverableGas" runat="server" CssClass="queryfieldmini" Width="150px"></asp:TextBox>
                                MMscf</td>
                        </tr>
                    </table>
                </td>
                <td width="50%" valign="top">
                    <table id="tblReservoirEngineering" cellspacing="0" cellpadding="4" border="0" width="100%">
                        <tr style="width: 100%">
                            <td width="40%">
                                Drive Mechanism
                            </td>
                            <td width="60%">
                                <asp:DropDownList ID="cboDriveMechanism" runat="server" CssClass="dropdownAdvSrch"
                                    Width="152px">
                                    <asp:ListItem>---Select---</asp:ListItem>
                                </asp:DropDownList>
                                <asp:Image ID="imgDriveMechanism" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
                        </tr>
                        <tr>
                            <td>
                                Hydrocarbon<br />
                                Type(main)</td>
                            <td>
                                <asp:DropDownList ID="cboHydrocarbonMain" runat="server" CssClass="dropdownAdvSrch"
                                    Width="152px">
                                    <asp:ListItem Text="---Select---" Value="---Select---"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:Image ID="imgHydrocarbonMain" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
                        </tr>
                        <tr>
                            <td>
                                Hydrocarbon<br />
                                Type(secondary)</td>
                            <td>
                                <asp:DropDownList ID="cboHydrocarbonSecondary" runat="server" CssClass="dropdownAdvSrch"
                                    Width="152px">
                                    <asp:ListItem>---Select---</asp:ListItem>
                                </asp:DropDownList>
                                <asp:Image ID="imgHydrocarbonSecondary" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
                        </tr>
                        <tr>
                            <td>
                                Oil Viscosity*</td>
                            <td>
                                <asp:TextBox ID="txtOilViscosity" runat="server" CssClass="queryfieldmini" Width="150px"></asp:TextBox>&nbsp;cp
                                <asp:Image ID="imgOilViscosity" Visible="false" runat="server" ImageAlign="AbsMiddle"
                                    ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
                        </tr>
                        <tr>
                            <td>
                                Pressure<br />
                                Reservoir (initial)*</td>
                            <td>
                                <asp:TextBox ID="txtPressureReservoirInitial" runat="server" CssClass="queryfieldmini"
                                    Width="150px"></asp:TextBox>&nbsp;psi
                                <asp:Image ID="imgPressureReservoirInitial" runat="server" ImageAlign="AbsMiddle"
                                    ImageUrl="/_layouts/DREAM/images/icon_help.gif" Visible="false" /></td>
                        </tr>
                        <tr>
                            <td>
                                Oil Gravity*</td>
                            <td style="white-space: nowrap">
                                <asp:TextBox ID="txtOilGravity" runat="server" CssClass="queryfieldmini" Width="150px"></asp:TextBox>&nbsp;Degrees
                                API
                                <asp:Image ID="imgOilGravity" Visible="false" runat="server" ImageAlign="AbsMiddle"
                                    ImageUrl="/_layouts/DREAM/images/icon_help.gif" /></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="AdvSaveSearchCriteria">
                    <b>&nbsp;Save Search Criteria</b></td>
            </tr>
            <tr>
                <td colspan="2" width="100%">
                    <table cellspacing="0" cellpadding="4" border="0" width="100%">
                        <tr>
                            <td width="20%" class="tdAdvSrchItemNbrdr" colspan="2">
                                Search Name
                            </td>
                            <td width="30%">
                                <asp:TextBox ID="txtSaveSearch" runat="server" CssClass="queryfieldmini" Width="150px"></asp:TextBox>
                                <asp:Image ID="imgSearchName" runat="server" ImageAlign="AbsMiddle" ImageUrl="/_layouts/DREAM/images/icon_help.gif" />
                            </td>
                            <td width="10%">
                                <asp:CheckBox Height="18px" ToolTip="Type of Save Search(Personal/Shared)" Text="Shared"
                                    ID="chbShared" runat="server" />
                            </td>
                            <td width="50%" align="center">
                                <input id="cmdSaveSearch" runat="server" class="buttonAdvSrch" onserverclick="cmdSaveSearch_Click"
                                    type="button" value="Save Search" />
                                <asp:Button ID="cmdSearch" runat="server" CssClass="buttonAdvSrch" OnClick="cmdSearch_Click"
                                    Text="Search" />
                                <input id="cmdReset" runat="server" class="buttonAdvSrch" onclick="EnableButton();"
                                    onserverclick="cmdReset_Click" type="button" value="Reset" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hidDepositionalColumn" runat="server" />
        <asp:HiddenField ID="hidDepositionalValue" runat="server" />
    </asp:Panel>
</div>
<input type="hidden" id="hidWordContent" runat="server" />

<script type="text/javascript">
  
setWindowTitle('Advanced Search - Reservoir');
function LoadLithostratFormation(combo, eventArgs)
{   var LithostratFormationCombo = $find("<%= radCboLithostratFormation.ClientID %>");    
    var LithostratMemberCombo = $find("<%= radCboLithostratMember.ClientID %>");
    var item = eventArgs.get_item();

    if (item.get_index() > 0 && item.get_text() != "---Select---")
    {    
      LithostratFormationCombo.set_text("Loading...");    
      if(item.get_value().length == 0)
      {
       LithostratFormationCombo.requestItems("empty", false);                                 
      }
       else
       {
        LithostratFormationCombo.requestItems(item.get_value(), false);                                
       }
    }
    else
    {     
        LithostratFormationCombo.set_text(" ");
        LithostratFormationCombo.clearItems();        
        LithostratMemberCombo.set_text(" ");
        LithostratMemberCombo.clearItems();
    }
}

function LoadLithostratMember(combo, eventArgs)
{    
    var item = eventArgs.get_item();
  if(item.get_index() > 0 && item.get_text() != "---Select---")
    {
   var  LithostratMemberCombo = $find("<%= radCboLithostratMember.ClientID %>");
    LithostratMemberCombo.set_text("Loading...");   
    /// When item.get_value.length == 0, ItemRequested event is not firing. Hardcoding with "empty" as value
    /// This value is checked at server side and replaced with empty string before raising webservice request.
     if(item.get_value().length == 0)
      {
       LithostratMemberCombo.requestItems("empty", false);                                 
      }
       else
       {
        LithostratMemberCombo.requestItems(item.get_value(), false);                                
       } 
                       
    }                   
}
function LoadLithologySecondary(combo, eventArgs)
{
  var LithologySecondaryCombo=$find("<%= radCboLithologySecondary.ClientID %>");
  var item = eventArgs.get_item();    
  LithologySecondaryCombo.set_text("Loading...");    
  LithologySecondaryCombo.requestItems(item.get_value(), false);  
}

</script>

